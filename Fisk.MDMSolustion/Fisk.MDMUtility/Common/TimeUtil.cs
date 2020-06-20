using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fisk.MDM.Utility.Common
{
    public class TimeUtil
    {
        public static DateTime GetCstDateTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var shanghaiZone = DateTimeZoneProviders.Tzdb["Asia/Shanghai"];
            return now.InZone(shanghaiZone).ToDateTimeUnspecified();
        }
    }

    public static class DateTimeExtentions
    {
        public static DateTime ToCstTime(this DateTime time)
        {
            return TimeUtil.GetCstDateTime();
        }
        /// <summary>
        /// 时间 格式化 24小时制 被格式化为  (yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <param name="time">被格式化时间</param>
        /// <returns>格式化后的时间字符串(yyyy-MM-dd HH:mm:ss)</returns>
        public static string ToStringYMD24HMS(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
