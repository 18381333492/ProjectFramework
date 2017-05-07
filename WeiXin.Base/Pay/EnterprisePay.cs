using WeiXin.Base.Pay.Lib;

namespace WeiXin.Base.Pay
{
    /// <summary>
    /// 企业付款
    /// </summary>
    public class EnterprisePay
    {
        /// <summary>
        /// 付款
        /// </summary>
        /// <param name="partner_trade_no">商户订单号</param>
        /// <param name="openid">商户appid下，某用户的openid</param>
        /// <param name="re_user_name">收款用户真实姓名</param>
        /// <param name="amount">企业付款金额，单位为分</param>
        /// <param name="desc">企业付款操作说明信息。必填</param>
        /// <param name="err_message">错误消息</param>
        public static bool Run(string partner_trade_no, string openid, string re_user_name, int amount, string desc, out string err_message)
        {
            #region 付款
            err_message = string.Empty;
            WxPayData data = new WxPayData();
            data.SetValue("partner_trade_no", partner_trade_no);
            data.SetValue("openid", openid);
            data.SetValue("re_user_name", re_user_name);
            data.SetValue("amount", amount);
            data.SetValue("desc", desc);

            WxPayData result = WxPayApi.MmPay(data);
            if (result.IsSet("return_code") && result.GetValue("return_code").ToString().Equals("SUCCESS"))
            {
                if (result.IsSet("result_code") && result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    err_message = result.GetValue("result_code").ToString();
                    return true;
                }
                else {
                    err_message = result.GetValue("return_msg").ToString();
                    return false;
                }
            }
            else {
                err_message = result.GetValue("return_msg").ToString();
                return false;
            } 
            #endregion
        }
    }
}
