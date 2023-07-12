using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using QSDec.Models;

namespace QSDec.Steps._4
{
    public static class Acquire_Pdfs_To_Be_Distributed  //MoveFilesForPrinting
    {

        public static void Process() => Copy(Load());

        public static List<FileToMove> Load()
        {
            using (var ar = new AREntities())
            {// originally: prc_QSPDFsToPrint
                return (from pf in ar.Daily_PassFail.ToList()
                        join pdf in ar.Daily_QSPdf.ToList()
                        on new { pf.PolicyNum, pf.TransID } equals new { pdf.PolicyNum, pdf.TransID }
                        where pf.Status == "PASS"
                        select new FileToMove(pf.PolicyNum, pdf.FileName)).ToList();
            }
        }

        public static void Copy(List<FileToMove> files)
            => files.ForEach(file => Copy(file));

        public static void Copy(FileToMove file)
        {
            if (File.Exists($"{AppSettings.PdfSplittingPath}{file.PolicyNumber}.pdf"))
            {

                File.Delete($"{AppSettings.PdfSplittingPath}{file.PolicyNumber}.pdf");
            }

            File.Copy($"{file.FilePath}", $"{AppSettings.PdfSplittingPath}{file.PolicyNumber}.pdf");
        }


    }


}
