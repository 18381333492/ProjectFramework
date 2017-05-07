using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace WeiXin.Base
{
    public class UrlVerify
    {
        /// <summary>
        /// 验证开发url
        /// </summary>
        /// <param name="sToken">用于配置开发者申请的 Token</param>
        /// <param name="sEchostr">随机字符串</param>
        /// <param name="sSignature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="sTimestamp">时间戳</param>
        /// <param name="sNonce">随机数</param>
        /// <returns></returns>
        public static bool Valid(string sToken, string sEchostr, string sSignature, string sTimestamp, string sNonce)
        {
            string[] ArrTmp = { sToken, sTimestamp, sNonce };
            Array.Sort(ArrTmp);//字典排序
            string sRandomStr = string.Join("", ArrTmp);
            sRandomStr = FormsAuthentication.HashPasswordForStoringInConfigFile(sRandomStr, "SHA1");
            sRandomStr = sRandomStr.ToLower();
            return sRandomStr == sSignature;
        }
    }
}
