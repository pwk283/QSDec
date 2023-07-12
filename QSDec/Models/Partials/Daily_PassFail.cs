using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSDec.Steps._1;

namespace QSDec.Models
{
    public partial class Daily_PassFail
    {
        public Daily_PassFail() { }
        public Daily_PassFail(string policyNumber, string status, short transactionId, DateTime printDate)
        {
            PolicyNum = policyNumber;
            Status = status;
            TransID = transactionId;
            PrintDate = printDate;
        }

    }
}
