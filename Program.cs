using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace findbigfiles {
    struct FileData {
        public string Path;
        public long Size;
    }

    class Program {
        const long KILO = 1024;
        const long MEGA = 1024*KILO;
        const long GIGA = 1024*MEGA;
        const long SECOND = 10000000;
        const long MINUTE = 60 * SECOND;
        const long HOUR = 60 * MINUTE;
        const long DAY = 24 * HOUR;
        const long WEEK = 7 * DAY;
        static List<FileData> findFiles = new List<FileData>();
        static long minSize = 0;
        static long minTime = 0;
        static DateTime dtNow = DateTime.Now;

        static void Main(string[] args) {
            Dictionary<string, string> ad = getArgs(args);
            if (!ad.ContainsKey("-dir")) {
                PrintHelp();
                return;
            }
            if (ad.ContainsKey("-size"))
                minSize = StringToSize(ad["-size"]);

            if (ad.ContainsKey("-time"))
                minTime = StringToTime(ad["-time"]);

            TraverseDir(ad["-dir"]);
            DateTime dt = DateTime.Now;
            dt.AddSeconds(1.0);

            foreach (FileData fd in findFiles.OrderBy(o => o.Size)) {
                Console.WriteLine("{0}\t{1}", fd.Path, FormatSize(fd.Size));
            }

        }

        private static long StringToTime(string sval) {
            long time = 0;
            char mod = sval[sval.Length - 1];
            long mult;
            switch (mod) {
                case 'm':
                    mult = MINUTE;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                case 'h':
                    mult = HOUR;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                case 'd':
                    mult = DAY;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                case 'w':
                    mult = WEEK;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                default:
                    throw new Exception("Wrong time suffix, see help");
            }
            try {
                time = Convert.ToInt64(sval) * mult;
            } catch { };
            return time;
        }

        private static void PrintHelp() {
            System.Console.Out.WriteLine("Usage: findbigfiles -dir=<path> [-size=<XX>] [-time=<XX><m|h|d|w>]");
        }

        private static long StringToSize(string sval) {
            long size = 0;
            char mod = sval[sval.Length - 1];
            long mult;
            switch (mod) {
                case 'K': 
                    mult = KILO;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                case 'M':
                    mult = MEGA;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                case 'G':
                    mult = GIGA;
                    sval = sval.Substring(0, sval.Length - 1);
                    break;
                default:
                    mult = 1;
                    break;
            }
            try {
                size = Convert.ToInt64(sval) * mult;
            } catch {};
            return size;
        }

        static Dictionary<string, string> getArgs(string[] args) {
            Dictionary<string, string> argDic = new Dictionary<string, string>();
            foreach (string s in args) {
                if (!s.StartsWith("-")) throw new Exception("Wrong argument: " + s);
                int pos = s.IndexOf('=');
                if (pos < 0) argDic.Add(s, "");
                else argDic.Add(s.Substring(0, pos), s.Substring(pos + 1));
            }
            return argDic;
        }
        //Pricious tines 

        private static void TraverseDir(string dirname) {
            try {
                foreach (string cfile in Directory.EnumerateFiles(dirname)) {
                    try {
                        FileInfo fi = new FileInfo(cfile);
                        FileData fd;
                        if (fi.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue;
                        fd.Path = cfile;
                        fd.Size = fi.Length;
                        if (minSize > 0 && fd.Size < minSize) continue;
                        if (minTime > 0 && dtNow.ToFileTime() - fi.LastWriteTime.ToFileTime() > minTime) continue;
                        findFiles.Add(fd);
                    } catch (UnauthorizedAccessException UAEx) {
                        Console.WriteLine(UAEx.Message);
                    }
                }
            } catch (UnauthorizedAccessException ex) {
                Console.WriteLine(ex.Message);
                return;
            }
            foreach (string cdir in Directory.EnumerateDirectories(dirname)) {
                if (!Directory.Exists(cdir)) continue;
                DirectoryInfo di = new DirectoryInfo(cdir);
                if (di.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue;
                TraverseDir(cdir);
            }
        }

        private static string FormatSize(long fsize) {
            if (fsize > GIGA) return (Convert.ToString(fsize / GIGA)) + "G";
            if (fsize > MEGA) return (Convert.ToString(fsize / MEGA)) + "M";
            if (fsize > KILO) return (Convert.ToString(fsize / KILO)) + "k";
            return Convert.ToString(fsize);
        }
    }
}
