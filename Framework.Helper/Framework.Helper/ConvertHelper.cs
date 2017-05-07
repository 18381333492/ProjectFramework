using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public static class ConvertHelper
    {
        /// <summary>
        /// object 转 bool
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static bool ToBoolean(this object s, bool def = default(bool))
        {
            bool result;
            return Boolean.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 char
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static char ToChar(this object s, char def = default(char))
        {
            char result;
            return Char.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 decimal
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s, decimal def = default(decimal))
        {
            decimal result;
            return Decimal.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 double
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static double ToDouble(this object s, double def = default(double))
        {
            double result;
            return Double.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 float
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static float ToSingle(this object s, float def = default(float))
        {
            float result;
            return Single.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 byte
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static byte ToByte(this object s, byte def = default(Byte))
        {
            byte result;
            return Byte.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 sbyte
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static sbyte ToSByte(this object s, sbyte def = default(sbyte))
        {
            sbyte result;
            return SByte.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 short
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static short ToInt16(this object s, short def = default(short))
        {
            short result;
            return Int16.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 ushort
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static ushort ToUInt16(this object s, ushort def = default(ushort))
        {
            ushort result;
            return UInt16.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 int
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int ToInt32(this object s, int def = default(int))
        {
            int result;
            return Int32.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 uint
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static uint ToUInt32(this object s, uint def = default(uint))
        {
            uint result;
            return UInt32.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 long
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static long ToInt64(this object s, long def = default(long))
        {
            long result;
            return Int64.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 ulong
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static ulong ToUInt64(this object s, ulong def = default(ulong))
        {
            ulong result;
            return UInt64.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object s, DateTime def = default(DateTime))
        {
            DateTime result;
            return DateTime.TryParse(s.ToString(), out result) ? result : def;
        }

        /// <summary>
        /// object 转 Guid
        /// </summary>
        /// <param name="s"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static Guid ToGuid(this object s, Guid def = default(Guid))
        {
            Guid result;
            return Guid.TryParse(s.ToString(), out result) ? result : def;
        }
    }
}
