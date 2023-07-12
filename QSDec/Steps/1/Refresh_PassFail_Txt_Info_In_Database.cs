using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QSDec.Models;

namespace QSDec.Steps._1
{

    public static class Refresh_PassFail_Txt_Info_In_Database //Originally: PassFail
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process()
        {
            EnforceReady();

            var lines = Load(AppSettings.PassFailFilePath);

            var records = lines.Select(CreateDailyPassFail);

            using (var ar = new AREntities())
                ar.Reload(ar.Daily_PassFail, records);


            log.Info($"Step 1: PassFail record count: {records.Count()}");

        }
        public static Daily_PassFail CreateDailyPassFail(PassFailLine line)
            => new Daily_PassFail(line.PolicyNumber, line.Status, line.TransactionId, AppSettings.RunDate);
       
        public static bool Ready() => File.Exists(AppSettings.PassFailFilePath);

        public static void EnforceReady()
        {
            if (Ready() == false) throw new Exception("Passfail file not found");
        }
        public static List<PassFailLine> Load(string path) 
            => File.ReadAllLines(path).Where(q => q.Length == 80).Select(line => new PassFailLine(line)).ToList();


    }

}
