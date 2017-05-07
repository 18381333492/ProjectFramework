using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXin.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class Access_TokenList
    {
        /// <summary>
        /// API接口token列表
        /// </summary>
        private static List<Access_Token> apiTokenList = new List<Access_Token>();

        private static object _lock = new object();

        /// <summary>
        /// 获取API token
        /// </summary>
        /// <param name="sWeiXinAccount">微信ID</param>
        /// <param name="sAppId"></param>
        /// <param name="sAppSecret"></param>
        /// <returns></returns>
        public static string GetToken(string sWeiXinAccount, string sAppId, string sAppSecret)
        {
            string sResult = string.Empty;
            Access_Token apiToken;
            lock (_lock)
            {
                apiToken = apiTokenList.Find(m => m.sWeiXinAccount.Equals(sWeiXinAccount));
                if (apiToken == null)
                {
                    apiToken = new Access_Token(sWeiXinAccount, sAppId, sAppSecret);
                    apiTokenList.Add(apiToken);
                }
            }
            sResult = apiToken.sToken;
            return sResult;
        }

        /// <summary>
        /// 获取API token
        /// </summary>
        /// <param name="token">委托，用于获取微信ID和开发者凭据</param>
        /// <returns></returns>
        public static string GetToken(Token token) {
            string sWeiXinAccount, sAppId, sAppSecret,sResult;

            token(out sWeiXinAccount,out sAppId,out sAppSecret);
            
            Access_Token apiToken;
            lock (_lock)
            {
                apiToken = apiTokenList.Find(m => m.sWeiXinAccount.Equals(sWeiXinAccount));
                if (apiToken == null)
                {
                    apiToken = new Access_Token(sWeiXinAccount, sAppId, sAppSecret);
                    apiTokenList.Add(apiToken);
                }
            }
            sResult = apiToken.sToken;
            return sResult;
        }
    }
}