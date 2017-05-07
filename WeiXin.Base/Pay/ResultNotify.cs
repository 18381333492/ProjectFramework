using WeiXin.Base.Pay.Lib;

namespace WeiXin.Base.Pay
{
    using System.Web;

    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// 必须设置订单状态更新事件
    /// </summary>
    public class ResultNotify : WxPayNotify
    {
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                WxPayLog.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }

            //微信支付订单号
            string transaction_id = notifyData.GetValue("transaction_id").ToString();
            //订单总金额，单位为分
            string total_fee = notifyData.GetValue("total_fee").ToString();
            //支付完成时间,格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器
            string time_end = notifyData.GetValue("time_end").ToString();
            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                WxPayLog.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                var out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                var bIsSuccess = OnUpdateOrderState(out_trade_no, transaction_id, total_fee, time_end);
                res.SetValue("return_code", bIsSuccess ? "SUCCESS" : "FAIL");
                res.SetValue("return_msg", bIsSuccess ? "OK" : "ERROR");
                WxPayLog.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                HttpContext.Current.Response.Write(res.ToXml());
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 查询订单
        /// </summary>
        /// <param name="transaction_id"></param>
        /// <returns></returns>
        private bool QueryOrder(string transaction_id)
        {
            #region 查询订单
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            } 
            #endregion
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="out_trade_id">订单号</param>
        /// <param name="transaction_id">微信支付订单号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="time_end">支付完成时间,格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器</param>
        /// <returns></returns>
        public delegate bool UpdateOrderHandler(
            string out_trade_id, string transaction_id, string total_fee, string time_end);

        /// <summary>
        /// 更新订单事件
        /// </summary>
        public event UpdateOrderHandler UpdateOrderStateEvent;

        /// <summary>
        /// 更新订单状态处理事件
        /// 用于更新平台订单状态
        /// </summary>
        /// <param name="out_trade_id">订单号</param>
        /// <param name="transaction_id">微信支付订单号</param>
        /// <param name="total_fee">订单总金额，单位为分</param>
        /// <param name="time_end">支付完成时间,格式为yyyyMMddhhmmss，如2009年12月27日9点10分10秒表示为20091227091010。时区为 GMT+8 beijing。该时间取自微信支付服务器</param>
        /// <returns></returns>
        public bool OnUpdateOrderState(string out_trade_id, string transaction_id, string total_fee, string time_end)
        {
            if (null != UpdateOrderStateEvent)
                return UpdateOrderStateEvent(out_trade_id, transaction_id, total_fee, time_end);
            return false;
        }
    }
}