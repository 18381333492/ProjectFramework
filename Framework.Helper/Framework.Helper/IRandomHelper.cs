using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public interface IRandomHelper
    {
        /// <summary>
        /// 获取指定位数的数字随机数字符串
        /// </summary>
        /// <param name="s">最小数</param>
        /// <param name="e">最大数</param>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        [Obsolete("已经改成了用GetRandomNumberString来生成，这个就不用了")]
        string GetRandomIntString(int s = 0, int e = int.MaxValue, int bit = 4);

        /// <summary>
        /// 获取混合的随机码
        /// </summary>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        string GetMixRandomString(int bit= 8);

        /// <summary>
        /// 获取数字的随机码
        /// </summary>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        string GetRandomNumberString(int bit = 4);

        /// <summary>
        /// 获取中文的随机码
        /// </summary>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        string GetRandomCHSString(int bit = 4);

        /// <summary>
        /// 获取日文的随机码
        /// </summary>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        string GetRandomJPString(int bit = 4);

        /// <summary>
        /// 获取英文的随机码
        /// </summary>
        /// <param name="bit">位数</param>
        /// <returns>生成结果</returns>
        string GetRandomENString(int bit = 4);

        /// <summary>
        /// 设置中文字符集合
        /// </summary>
        string[] CHSChars { set; }

        /// <summary>
        /// 设置日文字符集合
        /// </summary>
        string[] JPChars { set; }

        /// <summary>
        /// 设置英文字符集合
        /// </summary>
        string[] ENChars { set; }

        /// <summary>
        /// 设置数字字符集合
        /// </summary>
        string[] NumberChars { set; }
    }
}
