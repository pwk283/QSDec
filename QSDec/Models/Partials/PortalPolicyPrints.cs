using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSDec.Steps._3;

namespace QSDec.Models
{
    public partial class PortalPolicyPrints
    {
        public PortalPolicyPrints() { }
        public PortalPolicyPrints(string policyNum, short mBrokerNo, short transID, string fileName, string mInsuredName, string trtyp, DateTime effdate, string polType)
        {
            LoadDate = AppSettings.RunDate;
            PolicyNum = policyNum;
            MBrokerNo = mBrokerNo;
            TransID = transID;
            FileName = fileName;
            MInsuredName = mInsuredName;
            Trtype = trtyp;
            effDate = effdate;
            PolType = polType;
        }
    }
}
