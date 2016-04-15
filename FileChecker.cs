using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles {
    interface FileChecker {
        bool Check(FileInfo fi);
    }
}
