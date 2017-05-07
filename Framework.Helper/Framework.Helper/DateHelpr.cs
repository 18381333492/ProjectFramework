using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public class DateHelpr
    {
        /// <summary>
        /// 获取本月第一天
        /// </summary>
        /// <returns></returns>
        public static string GetNowMonthFirstDayStr()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd 00:00:00");
        }

        /// <summary>
        /// 获取本月最后一天
        /// </summary>
        /// <returns></returns>
        public static string GetNowMonthLastDayStr()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        }

        /// <summary>
        /// 获取上月第一天
        /// </summary>
        /// <returns></returns>
        public static string GetLastMonthFirstDayStr()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month, 1).ToString("yyyy-MM-dd 00:00:00");
        }

        /// <summary>
        /// 获取上月最后一天
        /// </summary>
        /// <returns></returns>
        public static string GetLastMonthLastDayStr()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59");
        }

        /// <summary>
        /// 获取本月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNowMonthFirstDayDate()
        {
            return DateTime.Parse(GetNowMonthFirstDayStr());
        }

        /// <summary>
        /// 获取本月最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNowMonthLastDayDate()
        {
            return DateTime.Parse(GetNowMonthLastDayStr());
        }

        /// <summary>
        /// 获取上月第一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastMonthFirstDayDate()
        {
            return DateTime.Parse(GetLastMonthFirstDayStr());
        }

        /// <summary>
        /// 获取上月最后一天
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastMonthLastDayDate()
        {
            return DateTime.Parse(GetLastMonthLastDayStr());
        }
    }
}
