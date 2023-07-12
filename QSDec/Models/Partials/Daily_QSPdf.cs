using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSDec.Steps._2;

namespace QSDec.Models
{
    public partial class Daily_QSPdf
    {
        public Daily_QSPdf() { }

        public Daily_QSPdf(string policyNumber, string transactionType, short transactionId, string filePath, DateTime printDate)
        {
            PolicyNum = policyNumber;
            TransType = transactionType;
            TransID = transactionId;
            FileName = filePath;
            PrintDate = printDate;
        }

    }
}
