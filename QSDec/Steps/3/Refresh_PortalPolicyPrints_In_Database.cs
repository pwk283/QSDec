using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSDec.Models;

namespace QSDec.Steps._3
{
    public static class Refresh_PortalPolicyPrints_In_Database // Originally: Portal Prints
    {

        public static void Process()
        {
            using (var ar = new AREntities())
            {
                //todo: both can be done in c# vs stored proc
                ar.prc_DailyTapeImport(); // reloads data from the linux system's policy_activity table to the dailyactivity_report table (both same structure)

                // More complex sql. Joins passfail, qspdf, and dailyactivity.  Modifies/normalizes data.  Inserts results into portalpolicyprints table.
                //ar.prc_PolicyPrintFeedLoad(); 

                LoadPortalPolicyPrints();

            }
        }

        public static void LoadPortalPolicyPrints()
        {

            using (var ar = new AREntities())
            {
                var pdfprints = (from pf in ar.Daily_PassFail.ToList()
                                 join pdf in ar.Daily_QSPdf.ToList()
                                 on new { pf.PolicyNum, pf.TransID } equals new { pdf.PolicyNum, pdf.TransID }
                                 where pf.Status == "PASS"
                                 select new { pf.PolicyNum, pdf.TransID, pdf.FileName, pdf.TransType }).ToList();

                var dailytransactions = ar.DailyActivity_Report.ToList();

                List<PortalPolicyPrints> PortalPolicyList = new List<PortalPolicyPrints>();
                try
                {
                    foreach (var p in pdfprints)
                    {
                        var t = dailytransactions.First(q => q.MState == GetPolState(p.PolicyNum) && q.MAcctNo + q.Filler11 == p.PolicyNum.Substring(2, 8));
                        PortalPolicyList.Add(new PortalPolicyPrints(p.PolicyNum, t.MBrokerNo.Value, p.TransID, p.FileName, t.MInsuredName, GetTranType(p.TransType, t.MUserTranRequest), GetEffDate(t.MOrigEff.ToString()), GetPolType(t)));

                    }

                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                }
                ar.Append(ar.PortalPolicyPrints, PortalPolicyList.ToList());

            }
        }
        public static DateTime GetEffDate(string effDate) => DateTime.ParseExact(effDate, "yyMMdd", null);
        public static int GetPolState(string policyNumber) => int.Parse(policyNumber.Substring(0, 2));
        public static string GetPolType(DailyActivity_Report dar)
            => GetPolType(dar.MCtLine03.Value, dar.MCtLine24.Value, dar.MCtLine51.Value, dar.MCtLine22.Value, dar.MCtLine55.Value);
        public static string GetPolType(int mctLine03, int mctLine24, int mctLine51, int mctLine22, int mctLine55)
        {
            if (mctLine03 > 0) { return "AUTO"; }
            if (mctLine24 > 0) { return "UMB"; }
            if (mctLine51 > 0) { return "PROP"; }
            if (mctLine22 > 0) { return "GL"; }
            if (mctLine55 > 0) { return "IM"; }

            return "XXX";
        }
        public static string GetTranType(string transType, string transRequest)
        {
            if (transType != "Issue") return transType;
            switch (transRequest)
            {
                case "01": return "New";
                case "06": return "Renewal";
                case "13": return "Reinstatement";
                default: throw new Exception($"{transRequest} transRequest a not valid id");
            }
        }
    }
}

//SELECT*
//FROM[AR].[dbo].[Daily_PassFail] a
//INNER JOIN[AR].[dbo].[Daily_QSPdf] b on a.policynum = b.[PolicyNum]
//INNER JOIN dbo.DailyActivity_Report c on cast(mstate as char(2)) + MAcctNo + filler11 = b.policynum
//where[status] = 'PASS'