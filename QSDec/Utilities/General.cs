using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSDec
{
    public static class General
    {

        public static void Act(params Action[] actions) => actions.ToList().ForEach(a => a());

    }
}
