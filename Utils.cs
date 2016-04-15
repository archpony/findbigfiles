using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace findbigfiles {
    static class Utils {
        const long SECOND = 10000000;
        const long MINUTE = 60 * SECOND;
        const long HOUR = 60 * MINUTE;
        const long DAY = 24 * HOUR;
        const long WEEK = 7 * DAY;
        const long KILO = 1024;
        const long MEGA = 1024 * KILO;
        const long GIGA = 1024 * MEGA;

        public static long StringToTime(string sval) {
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


        public static long StringToSize(string sval) {
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
            } catch { };
            return size;
        }

        public static string FormatSize(long fsize) {
            if (fsize > GIGA) return (Convert.ToString(fsize / GIGA)) + "G";
            if (fsize > MEGA) return (Convert.ToString(fsize / MEGA)) + "M";
            if (fsize > KILO) return (Convert.ToString(fsize / KILO)) + "k";
            return Convert.ToString(fsize);
        }

    }
}
