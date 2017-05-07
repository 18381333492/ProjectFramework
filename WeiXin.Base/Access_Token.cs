using Newtonsoft.Json.Linq;
using System;
using WeiXin.Tool;
namespace WeiXin.Base
{
    /// <summary>
    /// 用于获取数据中的微信信息
    /// <para>包括微信ID和开发者凭据</para>
    /// </summary>
    /// <param name="sWeiXinAccount"></param>
    /// <param name="sAppId"></param>
    /// <param name="sAppSecret"></param>
    public delegate void Token(out string sWeiXinAccount, out string sAppId, out string sAppSecret);

    /// <summary>
    /// 调用微信API的Token生成类
    /// </summary>
    public class Access_Token
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="token"></param>
        public Access_Token(Token token) {
            token(out this._sWeiXinAccount, out this._sAppId, out this._sAppSecret);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sWeiXinAccount">微信ID</param>
        /// <param name="sAppId"></param>
        /// <param name="sAppSecret"></param>
        public Access_Token(string sWeiXinAccount, string sAppId, string sAppSecret)
        {
            this._sWeiXinAccount = sWeiXinAccount;
            this._sAppId = sAppId;
            this._sAppSecret = sAppSecret;
        }

        /// <summary>
        /// 微信Token（调用API的Token 而非验证连接的Token）
        /// </summary>
        public string sToken
        {
            get
            {
                return this.GetToken();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string _tokenString = string.Empty;

        /// <summary>
        /// 生成token的时间，用于比较token在一定时间(不超过2小时)内是否还有效
        /// </summary>
        private DateTime _dtGenerateTime = DateTime.Now;

        /// <summary>
        /// 微信ID
        /// </summary>
        public string sWeiXinAccount
        {
            get { return this._sWeiXinAccount; }
        }
        /// <summary>
        /// 微信ID
        /// </summary>
        private string _sWeiXinAccount = string.Empty;

        /// <summary>
        /// AppId
        /// </summary>
        private string _sAppId = string.Empty;

        /// <summary>
        /// AppSecret
        /// </summary>
        private string _sAppSecret = string.Empty;

        /// <summary>
        /// token有效时间 单位秒
        /// </summary>
        private int _expireTime = 7000;

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private string GetToken()
        {
            string sResult = string.Empty;
            if (_tokenString == string.Empty ||
                (DateTime.Now - _dtGenerateTime).TotalSeconds > _expireTime)
            {
                lock (this)
                {
                    if (_tokenString == string.Empty)
                    {
                        _tokenString = GetWeixinToken();
                        if (_tokenString != string.Empty)
                        {
                            JObject tempJson = JObject.Parse(_tokenString);
                            if (tempJson["access_token"] != null)
                            {
                                _tokenString = tempJson["access_token"].ToString();
                                _dtGenerateTime = DateTime.Now;
                            }
                            else
                            {
                                _tokenString = string.Empty;
                            }
                        }
                    }
                }
            }
            sResult = _tokenString;
            return sResult;
        }

        /// <summary>
        /// 获取微信Token
        /// </summary>
        /// <returns></returns>
        private string GetWeixinToken()
        {
            string sResult = string.Empty;
            string sAppId = _sAppId;
            string sAppSecret = _sAppSecret;
            string sUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + sAppId + "&secret=" + sAppSecret;
            sResult = HttpHelper.Get(sUrl);
            return sResult;
        }
    }
}