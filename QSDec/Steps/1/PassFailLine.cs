using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSDec.Steps._1
{
    public class PassFailLine
    {
        public PassFailLine() { }
        public PassFailLine(string policyNumber, string status, short transactionId)
        {
            PolicyNumber = policyNumber;
            Status = status;
            TransactionId = transactionId;
        }
        
        public PassFailLine(string line) : this(line.Substring(11, 10), line.Substring(76, 4), short.Parse(line.Substring(5, 3).Trim())) { }


        public string PolicyNumber { get; }
        public string Status { get; }
        public short TransactionId { get; }

    }
}
