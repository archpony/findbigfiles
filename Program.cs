using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace findbigfiles {
    struct FileData {
        public string Path;
        public long Size;
    }

    class Program 
    {
        static void Main(string[] args) 
        {
            Dictionary<string, string> ad = getArgs(args);
            if (!ad.ContainsKey("-dir")) {
                PrintHelp();
                return;
            }
            FileWalker fw = new FileWalker();

            if (ad.ContainsKey("-size")) {
                SizeChecker sz = new SizeChecker(Utils.StringToSize(ad["-size"]));
                fw.Checkers.Add(sz);
            }

            if (ad.ContainsKey("-time")) {
                TimeChecker tm = new TimeChecker(Utils.StringToTime(ad["-time"]));
                fw.Checkers.Add(tm);
            }

            CmdBuilder cb = null;
            if (ad.ContainsKey("-exec")) {
                cb = new CmdBuilder(ad["-exec"]);
            }

            fw.TraverseDir(ad["-dir"]);

            if (cb == null) {
                foreach (FileData fd in fw.FoundFiles.OrderBy(o => o.Size)) {
                    Console.WriteLine("{0}\t{1}", fd.Path, Utils.FormatSize(fd.Size));
                }
            } else {
                foreach (FileData fd in fw.FoundFiles) {
                    Process p = new Process();
                    ProcessStartInfo si = new ProcessStartInfo();
					si.FileName = cb.AppName;
                    si.Arguments = cb.Build(fd.Path);
                    si.UseShellExecute = false;
                    si.WindowStyle = ProcessWindowStyle.Hidden;
                    si.WorkingDirectory = Directory.GetCurrentDirectory();
                    p.StartInfo = si;
                    p.Start();
                    p.WaitForExit();
                }
            }
        }

        
        private static void PrintHelp() 
        {
            System.Console.Out.WriteLine("Usage: findbigfiles -dir=<path> [-size=<XX>] [-time=<XX><m|h|d|w>] [-exec=\"a command line\"]");
        }

        

        static Dictionary<string, string> getArgs(string[] args) 
        {
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            foreach (string s in args) {
                if (!s.StartsWith("-")) throw new Exception("Wrong argument: " + s);
                int pos = s.IndexOf('=');
                if (pos < 0) argDic.Add(s, "");
                else argDic.Add(s.Substring(0, pos), s.Substring(pos + 1));
            }
            return argDic;
        }
    }
}
