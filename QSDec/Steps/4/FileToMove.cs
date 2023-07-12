using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSDec.Steps._4
{
    public class FileToMove
    {
        public FileToMove() { }

        public FileToMove(string policyNumber, string filePath)
        {
            PolicyNumber = policyNumber;
            FilePath = filePath;
        }

        public string PolicyNumber { get; set; }
        public string FilePath { get; set; }
    }
}
