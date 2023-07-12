using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QSDec.Utilities.Backup
{
    public static class BackUpFiles
    {
        public static void Process()
        {

            File.Delete(AppSettings.PassFailFilePath);

            string[] b = Directory.GetFiles(AppSettings.PdfSplittingPathBackup, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < b.Length; i++)
            {
                File.Delete(AppSettings.PdfSplittingPathBackup + Path.GetFileName(b[i]));
            }

            string[] s = Directory.GetFiles(AppSettings.PdfSplittingPath, "*.pdf", SearchOption.AllDirectories);
            for (int i = 0; i < s.Length; i++)
            {
                File.Move(s[i], AppSettings.PdfSplittingPathBackup + Path.GetFileName(s[i]));
            }

        }
    }
}
