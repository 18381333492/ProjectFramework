using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeiXin.Base.Pay
{
    /// <summary>
    /// 微信支付的参数模型
    /// </summary>
    public class WxOpenModel
    {
        /// <summary>
        /// AppId（应用ID）
        /// </summary>
        public string sAppId { get; set; }

        /// <summary>
        /// sAppSecret（应用密钥）
        /// </summary>
        public string sAppSecret { get; set; }

        /// <summary>
        /// 商户号
        /// </summary>
        public string sMchId { get; set; }

        /// <summary>
        /// API秘钥
        /// </summary>
        public string sPaySignKey { get; set; }

        /// <summary>
        /// 微信支付回调地址
        /// </summary>
        public string sTenpayNotify { get; set; }

        /// <summary>
        /// 微信支付发起支付地址
        /// </summary>
        public string sSendUrl { get; set; }
    }
}
