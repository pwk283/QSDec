using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace QSDec
{
    public static class AppSettings
    {

        public static readonly string QuickSolverPdfPath = ConfigurationManager.AppSettings[nameof(QuickSolverPdfPath)];

        public static readonly string PassFailFilePath = ConfigurationManager.AppSettings[nameof(PassFailFilePath)];

        public static readonly string PdfSplittingPath = ConfigurationManager.AppSettings[nameof(PdfSplittingPath)];

        public static readonly string PdfSplittingPathBackup = ConfigurationManager.AppSettings[nameof(PdfSplittingPathBackup)];

        public static readonly string PrintServerDecPrintsPath = ConfigurationManager.AppSettings[nameof(PrintServerDecPrintsPath)];

        public static readonly DateTime RunDate = DateTime.Now;

        public static readonly string ImageRightFormsTextImportPath = ConfigurationManager.AppSettings[nameof(ImageRightFormsTextImportPath)];

    }
}
