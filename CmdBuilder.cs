using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles
{
    class CmdBuilder
    {
        private string CmdTmpl;
        public CmdBuilder(string cmd)
        {
            CmdTmpl = cmd;
        }

        public string Build(string arg) 
        {
            if (CmdTmpl.IndexOf("{}") == -1)
                return CmdTmpl + " " + arg;
            return CmdTmpl.Replace("{}", arg);
        }
    }
}
