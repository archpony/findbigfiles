using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles {
    class SizeChecker:FileChecker
    {

        private long minSize;

        public SizeChecker(long size) {
            minSize = size;
        }

        #region FileChecker Members

        public bool Check(System.IO.FileInfo fi) {
            return fi.Length > minSize;
        }

        #endregion
    }
}
