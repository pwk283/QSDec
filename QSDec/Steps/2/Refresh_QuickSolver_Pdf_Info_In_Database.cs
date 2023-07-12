using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QSDec.Models;

namespace QSDec.Steps._2
{

    public static class Refresh_QuickSolver_Pdf_Info_In_Database //Originally: QSPdf
    {

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process()
        {
            var files = Load(AppSettings.QuickSolverPdfPath);
            using (var ar = new AREntities())
                
            {
                var records = files.Select(CreateDaily_QSPdf).JoinPassFail(ar.Daily_PassFail).SelectMostRecentQSPdfs();
                ar.Reload(ar.Daily_QSPdf, records);

                log.Info($"Step 2: Daily_QSPdf record count: {records.Count()}");
  
            }
        }

        public static bool Ready()
        {
            return Directory.GetFiles(AppSettings.QuickSolverPdfPath, "*.pdf").Length > 0;

            //todo: should determine if there were any files for that day???
        }
        public static Daily_QSPdf CreateDaily_QSPdf(QSPdfFile line)
            => new Daily_QSPdf(line.PolicyNumber, line.TransactionType, line.TransactionId, line.FilePath, AppSettings.RunDate);

        public static List<QSPdfFile> Load(string path)
            => Directory.GetFiles(path, "*.pdf").Where(f => !f.ToLowerInvariant().Contains("-autoidcards-"))  // 072319 - added idenfiying QS autocards
            .Select(f => new QSPdfFile(f)).ToList();

        public static List<Daily_QSPdf> SelectMostRecentQSPdfs(this IEnumerable<Daily_QSPdf> pdfs)
        {
            var fileList = pdfs.OrderByDescending(q => q.FileName);
            var a = (from fl in fileList
                     group fl by fl.PolicyNum
                     into x
                     select x.First()).ToList();
            return a;
        }

        public static List<Daily_QSPdf> JoinPassFail(this IEnumerable<Daily_QSPdf> pdfs, IEnumerable<Daily_PassFail> dpfs)
        {
            
            var a = (from pdf in pdfs.ToList()
                     join dpf in dpfs.ToList()
                     on new { pdf.PolicyNum, pdf.TransID } equals new { dpf.PolicyNum, dpf.TransID }
                     where dpf.Status == "PASS"
                     select pdf).ToList();
            return a;

        }
    }

}
