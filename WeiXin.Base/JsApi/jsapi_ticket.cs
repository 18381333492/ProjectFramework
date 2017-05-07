using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXin.Tool;

namespace WeiXin.Base.JsApi
{
    /// <summary>
    /// JS-SDK使用权限签名
    /// <para>生成签名之前必须先了解一下jsapi_ticket，jsapi_ticket是公众号用于调用微信JS接口的临时票据。正常情况下，
    /// jsapi_ticket的有效期为7200秒，通过access_token来获取。由于获取jsapi_ticket的api调用次数非常有限，
    /// 频繁刷新jsapi_ticket会导致api调用受限，影响自身业务，开发者必须在自己的服务全局缓存jsapi_ticket 。
    /// </para>
    /// </summary>
    public class jsapi_ticket
    {
        /// <summary>
        /// 微信Js API Ticket
        /// </summary>
        private string sTicket
        {
            get
            {
                return this.GetTicket();
            }
        }

        /// <summary>
        /// 微信原始ID
        /// </summary>
        private string sWeiXinAccount { get; set; }

        /// <summary>
        /// AppID(应用ID)
        /// </summary>
        private string sAppId { get; set; }

        /// <summary>
        /// AppSecret(应用密钥)
        /// </summary>
        private string sAppSecret { get; set; }

        /// <summary>
        /// 微信Access_Token
        /// </summary>
        private string sAccess_Token { get; set; }

        /// <summary>
        /// 初始化信息,并取得对应的微信Access_Token
        /// </summary>
        /// <param name="sWeiXinAccount">微信原始ID</param>
        /// <param name="sAppId">AppID(应用ID)</param>
        /// <param name="sAppSecret">AppSecret(应用密钥)</param>
        private jsapi_ticket(string sWeiXinAccount, string sAppId, string sAppSecret)
        {
            this.sWeiXinAccount = sWeiXinAccount;
            this.sAppId = sAppId;
            this.sAppSecret = sAppSecret;
            //获取微信Access_Token
            this.sAccess_Token = Access_TokenList.GetToken(sWeiXinAccount, sAppId, sAppSecret);
        }

        /// <summary>
        /// 
        /// </summary>
        private string _ticketString = string.Empty;

        /// <summary>
        /// 生成ticket的时间，用于比较token在一定时间(不超过2小时)内是否还有效
        /// </summary>
        private DateTime _dtGenerateTime = DateTime.Now;

        /// <summary>
        /// ticket有效时间 单位秒
        /// </summary>
        private int _expireTime = 7000;

        private string GetTicket()
        {
            string sResult = string.Empty;
            if (_ticketString == string.Empty ||
                (DateTime.Now - _dtGenerateTime).TotalSeconds > _expireTime)
            {
                lock (this)
                {
                    if (_ticketString == string.Empty)
                    {
                        _ticketString = GetWeixinTicket();
                        if (_ticketString != string.Empty)
                        {
                            JObject tempJson = JObject.Parse(_ticketString);
                            if (tempJson["ticket"] != null)
                            {
                                _ticketString = tempJson["ticket"].ToString();
                                _dtGenerateTime = DateTime.Now;
                            }
                            else
                            {
                                _ticketString = string.Empty;
                            }
                        }
                    }
                }
            }
            sResult = _ticketString;
            return sResult;
        }

        /// <summary>
        /// 获取微信js Ticket
        /// </summary>
        /// <returns></returns>
        private string GetWeixinTicket()
        {
            string sResult = string.Empty;

            string sUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + sAccess_Token + "&type=jsapi";
            sResult = HttpHelper.Get(sUrl);
            return sResult;
        }


        private static List<jsapi_ticket> apiTicketList = new List<jsapi_ticket>();
        private static object _lock = new object();

        /// <summary>
        /// 获取JS API ticket
        /// </summary>
        /// <param name="sWeiXinAccount">微信原始ID</param>
        /// <param name="sAppId">AppID(应用ID)</param>
        /// <param name="sAppSecret">AppSecret(应用密钥)</param>
        /// <returns>ticket</returns>
        public static string GetTicket(string sWeiXinAccount, string sAppId, string sAppSecret)
        {
            if (string.IsNullOrEmpty(sWeiXinAccount))
                throw new ArgumentException("微信原始ID不能为空");
            if (string.IsNullOrEmpty(sAppId))
                throw new ArgumentException("AppID(应用ID)不能为空");
            if (string.IsNullOrEmpty(sAppSecret))
                throw new ArgumentException("AppSecret(应用密钥)不能为空");

            string sResult = string.Empty;
            jsapi_ticket apiTicket;
            lock (_lock)
            {
                apiTicket = apiTicketList.Find(m => m.sWeiXinAccount.Equals(sWeiXinAccount));
                if (apiTicket == null)
                {
                    apiTicket = new jsapi_ticket(sWeiXinAccount, sAppId, sAppSecret);
                    apiTicketList.Add(apiTicket);
                }
            }
            sResult = apiTicket.sTicket;
            return sResult;
        }

    }

}
