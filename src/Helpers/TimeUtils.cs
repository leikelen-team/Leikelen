using System;

namespace cl.uv.leikelen.src.Helpers
{
    public static class TimeUtils
    {
        public static string TimeSpanToShortString(this TimeSpan t)
        {
            string shortForm = "";
            if (t.Hours > 0)
            {
                shortForm += t.Hours.ToString("00") + ":";
            }
            shortForm += t.Minutes.ToString("00") + ":";
            shortForm += t.Seconds.ToString("00");
            return shortForm;
        }

        public static string GetStringTime()
        {
            return System.DateTime.Now.ToString("HH-mm-ss-fff", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }

        public static string GetStringTime(this DateTime dateTime)
        {
            return dateTime.ToString("HH-mm-ss-fff", System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }

        public static string GetStringTime(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format, System.Globalization.CultureInfo.CurrentUICulture.DateTimeFormat);
        }
    }
}
