using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QSDec.Steps._2
{
    public class QSPdfFile
    {
        public QSPdfFile() { }
        public QSPdfFile(string path)
        {// ex. "31-S610-01-16-Reinstate-Trans6-20170522_1032", "04-A012-02-18-Issue-Trans2-20170522_1033", "31-S610-02-16-Reinstate-Trans4-20170522_1033"
            FilePath = path;

            var relative = Path.GetFileNameWithoutExtension(path);

            PolicyNumber = relative.Substring(0, 13).Replace("-", "");

            var parts = relative.Substring(14).Split('-');

            TransactionType = parts[0];
            TransactionId = short.Parse(parts[1].ToLowerInvariant().Replace("trans", ""));
        }

        public string PolicyNumber { get; set; }
        public string TransactionType { get; set; } // Originally IssueType
        public short TransactionId { get; set; }
        public string FilePath { get; set; }
    }


}
