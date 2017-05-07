using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.web.config;
using Framework.Helper;

namespace TenPay.Base
{
    public class TenPayConfig
    {
        /// <summary>
        /// 微信公众号唯一标识
        /// </summary>
        public static string appid =WebConfig.LoadDynamicJson("weixin").AppId;

        /// <summary>
        /// 微信公众号密码
        /// </summary>
        public static string appsecret = WebConfig.LoadDynamicJson("weixin").AppSecret;

        /// <summary>
        /// 商户号(微信支付分配的商户号)
        /// </summary>
        public static string mch_id =WebConfig.LoadDynamicJson("weixin").Mch_Id;

        /// <summary>
        /// 密钥(key设置路径：微信商户平台(pay.weixin.qq.com)-->账户设置-->API安全-->密钥设置)
        /// </summary>
        public static string key = WebConfig.LoadDynamicJson("weixin").APISignKey;

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <returns></returns>
        public static string nonce_str()
        {
            // 生成随机数算法
            //微信支付API接口协议中包含字段nonce_str，主要保证签名不可预测。
            //我们推荐生成随机数算法如下：调用随机数函数生成，将得到的值转换为字符串。
            Random Ran = new Random();
            string nonce_str = Ran.Next(11111111, 99999999).ToString() + "YKFX";
            return Security.GetMD5Hash(nonce_str);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static string time_stamp()
        {
            //时间戳
            //标准北京时间，时区为东八区，自1970年1月1日 0点0分0秒以来的秒数。
            //注意：部分系统取到的值为毫秒级，需要转换成秒(10位数字).
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}
