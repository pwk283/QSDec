using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace QSDec.Steps._5
{
    public static class Distribute_Pdfs_To_ImageRight_And_Printer // Originally: PDFSplitting
    {
        // This process has excluded the creation of bookmark xml and lst files, 
        // because they appear to soley be used for splitting out a pdf document and creating the IDX file for ImageRight

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Process()
        {
 
                var files = Load();

                files.ForEach(path => Process(path));

                log.Info($"Step 5: ImageRight record count: {files.Count()}");

        }

        public static void Process(string filePath)
        {
            try
            {
                var bookmarks = Pdf.GetBookmarks(filePath);

                if (bookmarks.FirstOrDefault()?.Kids.Count > 0)
                {
                    //There is a top level bookmark that needs to be skipped
                    ProcessPolicy(filePath, bookmarks.First().Kids);
                }
                else if (bookmarks.Count == 0) { ProcessDec(filePath); } //todo: research if this is needed anymore
                else { throw new Exception($"{filePath} has the wrong structure of bookmarks"); }

            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                log4net.LogManager.GetLogger("EmailLogger").Error(ex + filePath);
            }
           

        }


        public static List<string> Load() => Directory.GetFiles(AppSettings.PdfSplittingPath, "*.pdf").ToList();

        #region Decs

        public static void ProcessDec(string filePath)
        {
            var policyNumber = Path.GetFileNameWithoutExtension(filePath);

            var target = $"{AppSettings.PrintServerDecPrintsPath}{DecFileName(policyNumber, DateTime.Now)}";

            File.Copy(filePath, target);
        }

        public static string DecFileName(string policyNumber, DateTime asOf)
            => $"Underwriting_{policyNumber}_Policy Information_Dec_D_{asOf.ToString("yyyy_MM_dd")}Original.pdf";


        #endregion

        #region Policy

        public static void ProcessPolicy(string filePath, List<Bookmark> bookmarks)
        {
            var policyNumber = Path.GetFileNameWithoutExtension(filePath);

            for (var x = 0; x < bookmarks.Count; x++)
            {
                ProcessBookmark(policyNumber, bookmarks[x], x + 1);
            }

            //ProcessIDX(policyNumber, bookmarks);
            Idx.Process(policyNumber, bookmarks);
        }

        public static void ProcessBookmark(string policyNumber, Bookmark bookmark, int number)
        {
            var filePath = $"{AppSettings.ImageRightFormsTextImportPath}{policyNumber}_{number}.pdf";

            File.WriteAllBytes(filePath, bookmark.Pages);
        }

        #endregion



    }
}
