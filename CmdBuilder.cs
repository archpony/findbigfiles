using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles
{
    class CmdBuilder
    {
        private string CmdTmpl;
		public string AppName {
			get;
			set;
		}
        public CmdBuilder(string cmd)
        {
			int osID = (int) Environment.OSVersion.Platform;
			if (osID < 4) {
				AppName = "cmd.exe";
				CmdTmpl = "/c \"" + cmd;
			} else {
				AppName = "/bin/sh";
				CmdTmpl = "-c \"" + cmd;
			}
        }

        public string Build(string arg) 
        {
            if (CmdTmpl.IndexOf("{}") == -1)
                return CmdTmpl + " " + arg + "\"";
			return CmdTmpl.Replace("{}", arg)+ "\"";
        }
    }
}
