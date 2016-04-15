using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles {
    class FileWalker 
    {
        public List<FileChecker> Checkers{ get; set; }
        public List<FileData> FoundFiles { get; set; }

        public FileWalker() {
            Checkers = new List<FileChecker>();
            FoundFiles = new List<FileData>();
        }

        public void TraverseDir(string dirname) {
            try {
                foreach (string cfile in Directory.EnumerateFiles(dirname)) {
                    try {
                        FileInfo fi = new FileInfo(cfile);
                        FileData fd;
                        if (fi.Attributes.HasFlag(FileAttributes.ReparsePoint)) continue;
                        fd.Path = cfile;
                        fd.Size = fi.Length;
                        bool useFile = true;
                        foreach (FileChecker fc in Checkers) {
                            useFile = fc.Check(fi);
                            if (!useFile) break;
                        }
                        if (useFile) 
                            FoundFiles.Add(fd);
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
    }
}
