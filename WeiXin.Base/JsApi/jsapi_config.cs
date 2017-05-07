using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace WeiXin.Base.JsApi
{
    public class jsapi_config
    {
        /// <summary>
        /// 公众号的唯一标识
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 生成签名的时间戳
        /// </summary>
        public long timestamp
        {
            get;
            private set;
        }

        /// <summary>
        /// 生成签名的随机串
        /// </summary>
        public string nonceStr
        {
            get;
            private set;
        }

        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; private set; }

        /// <summary>
        /// 当前网页的URL，不包含#及其后面部分
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// jsapi_ticket
        /// </summary>
        public string ticket { get; private set; }

        /// <summary>
        /// 获取JS API ticket
        /// </summary>
        /// <param name="sWeiXinAccount">微信原始ID</param>
        /// <param name="sAppId">AppID(应用ID)</param>
        /// <param name="sAppSecret">AppSecret(应用密钥)</param>
        /// <returns>ticket</returns>
        public jsapi_config(string sWeiXinAccount, string sAppId, string sAppSecret, string url)
        {
            if (string.IsNullOrEmpty(sWeiXinAccount))
                throw new ArgumentException("微信原始ID不能为空");
            if (string.IsNullOrEmpty(sAppId))
                throw new ArgumentException("AppID(应用ID)不能为空");
            if (string.IsNullOrEmpty(sAppSecret))
                throw new ArgumentException("AppSecret(应用密钥)不能为空");
            if (string.IsNullOrEmpty(url))
                throw new ArgumentException("请求地址不能为空");

            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            this.timestamp = (long)(DateTime.Now - startTime).TotalSeconds;

            this.nonceStr = "WX-ehecd" + new Random().Next(100000, 9999999) + "O";

            this.appId = sAppId;
            this.url = url;
            //ticket
            this.ticket = jsapi_ticket.GetTicket(sWeiXinAccount, sAppId, sAppSecret);
            //签名
            this.signature = GenerateSignature();
        }

        /// <summary>
        /// 生成签名
        /// 签名校验地址http://mp.weixin.qq.com/debug/cgi-bin/sandbox?t=jsapisign
        /// </summary>
        /// <returns></returns>
        private string GenerateSignature()
        {
            Dictionary<string, string> signatureParams = new Dictionary<string, string>();
            signatureParams.Add("jsapi_ticket", this.ticket);
            signatureParams.Add("noncestr", this.nonceStr);
            signatureParams.Add("timestamp", this.timestamp.ToString());
            signatureParams.Add("url", this.url);

            var query = from s in signatureParams select string.Concat(s.Key, "=", s.Value);
            string param = string.Join("&", query.ToArray<string>());
            var result = FormsAuthentication.HashPasswordForStoringInConfigFile(param, "SHA1");

            return result.ToLower();
        }
    }
}
