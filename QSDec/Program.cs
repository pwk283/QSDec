using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QSDec
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static int Main(string[] args)
        {
            try
            {

                Steps._1.Refresh_PassFail_Txt_Info_In_Database.Process();
                Steps._2.Refresh_QuickSolver_Pdf_Info_In_Database.Process();
                Steps._3.Refresh_PortalPolicyPrints_In_Database.Process();
                Steps._4.Acquire_Pdfs_To_Be_Distributed.Process();
                Steps._5.Distribute_Pdfs_To_ImageRight_And_Printer.Process();
                Utilities.Backup.BackUpFiles.Process();

                log.Info("QSDec Complete");

            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                log4net.LogManager.GetLogger("EmailLogger").Error(ex);
            }

            return 0;
        }

    }

}
