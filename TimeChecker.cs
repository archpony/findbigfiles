using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles {
    class TimeChecker:FileChecker 
    {
        private DateTime dtNow = DateTime.Now;
        private long minTime;

        public TimeChecker(long time) 
        {
            minTime = time;
        }
        #region FileChecker Members

        public bool Check(System.IO.FileInfo fi) 
        {
            return dtNow.ToFileTime() - fi.LastWriteTime.ToFileTime() < minTime;
        }

        #endregion

    }
}
