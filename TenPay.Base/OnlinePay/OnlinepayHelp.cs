using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;




namespace Onlinepay
{
    public class OnlinepayHelp
    {
        /// <summary>
        /// 创建在线支付地址
        /// </summary>
        /// <param name="sChannelType">渠道类型 07-PC，08-手机</param>
        /// <param name="sOrderId">订单编号</param>
        /// <param name="sOrderNo">订单号</param>
        /// <param name="sMoney">金额</param>
        /// <param name="sNotifyUrl">异步通知地址</param>
        /// <param name="sReturnUrl">同步通知地址</param>
        /// <param name="bIsAuto"></param>
        /// <returns></returns>
        static public string CreateOnlinepayUrl(string sChannelType, string sOrderId, string sOrderNo, string sMoney,
            string sNotifyUrl, string sReturnUrl,string sBody ,bool bIsAuto = true)
        {
            #region 创建在线支付地址
            Dictionary<string, string> param = new Dictionary<string, string>();

            string notify_url = sNotifyUrl;
            string return_url = "http://" + HttpContext.Current.Request.Url.Authority + sReturnUrl;

            //填写参数

            param["version"] = "5.0.0";//版本号
            param["encoding"] = "UTF-8";//编码方式
            param["certId"] = CertUtil.GetSignCertId();      //证书ID
            param["txnType"] = "01";//交易类型
            param["txnSubType"] = "01";//交易子类
            param["bizType"] = "000201";//业务类型
            param["frontUrl"] = return_url;    //前台通知地址      
            param["backUrl"] = notify_url;  //后台通知地址，改自己的外网地址
            param["signMethod"] = "01";//签名方法
            param["channelType"] = sChannelType;//渠道类型，07-PC，08-手机
            param["accessType"] = "0";//接入类型
            param["merId"] = ConfigurationManager.AppSettings["MerId"];//商户号，请改成自己的商户号
            param["orderId"] = sOrderNo;//商户订单号，8-40位数字字母
            param["txnTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");//订单发送时间
            param["txnAmt"] = (decimal.Parse(sMoney) * 100).ToString("f0");//交易金额，单位分
            param["currencyCode"] = "156";//交易币种
            param["orderDesc"] = sBody;//订单描述，暂时不会起作用
            param["reqReserved"] = sOrderId;//请求方保留域，透传字段，查询、通知、对账文件中均会原样出现

            SDKUtil.Sign(param, Encoding.UTF8);
            // 将SDKUtil产生的Html文档写入页面，从而引导用户浏览器重定向   
            string html = string.Empty;
            if (bIsAuto)
                html = SDKUtil.CreateAutoSubmitForm(SDKConfig.FrontTransUrl, param, Encoding.UTF8);
            else
                html = SDKUtil.CreateManualSubmitForm(SDKConfig.FrontTransUrl, param, Encoding.UTF8);
            return html;
            #endregion
        }

        /// <summary>
        /// 获取交易流水号
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <param name="sNotifyUrl"></param>
        /// <param name="sReturnUrl"></param>
        /// <param name="sMsg"></param>
        /// <param name="iModeType">类型：1：商品订单支付  5：充值</param>
        /// <returns></returns>
        static public bool CreateQueryID(string sOrderId, string sNotifyUrl, string sReturnUrl, ref string sMsg, int iModeType = 1)
        {
            #region 获取交易流水号
            Dictionary<string, string> param = new Dictionary<string, string>();

            string notify_url = "http://" + HttpContext.Current.Request.Url.Authority + sNotifyUrl;
            string return_url = "http://" + HttpContext.Current.Request.Url.Authority + sReturnUrl;
            string sOrderNo = string.Empty;
            decimal dPaid = 0;
            //if (iModeType == 1)
            //{
            //    EHECD_Orders_View item = GetView(sOrderId);
            //    dPaid = item.dTotalPrice + item.dLogisticsPrice;
            //    if (item.PayMethods != null && item.PayMethods.Count > 0)
            //    {
            //        dPaid -= item.PayMethods.Sum(o => o.dAmount);
            //    }
            //    sOrderNo = item.sOrderNo;
            //}
            //else if (iModeType == 5)
            //{
            //    string[] strAry = sOrderId.Split('|');
            //    EHECD_ClientRecharge_Item item = GetRechargeInfo(strAry[0]);
            //    dPaid = item.dPrice;
            //    sOrderNo = item.sNumber;
            //}

            //填写参数
            param["version"] = "5.0.0";//版本号
            param["encoding"] = "UTF-8";//编码方式
            param["certId"] = CertUtil.GetSignCertId();      //证书ID
            param["txnType"] = "01";//交易类型
            param["txnSubType"] = "01";//交易子类
            param["bizType"] = "000201";//业务类型
            param["frontUrl"] = return_url;    //前台通知地址      
            param["backUrl"] = notify_url;  //后台通知地址，改自己的外网地址
            param["signMethod"] = "01";//签名方法
            param["channelType"] = "08";//渠道类型，07-PC，08-手机
            param["accessType"] = "0";//接入类型
            param["merId"] = ConfigurationManager.AppSettings["MerId"];//商户号，请改成自己的商户号
            param["orderId"] = sOrderNo;//商户订单号，8-40位数字字母
            param["txnTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");//订单发送时间
            param["txnAmt"] = (dPaid * 100).ToString("f0");//交易金额，单位分
            param["currencyCode"] = "156";//交易币种
            param["orderDesc"] = "订单描述";//订单描述，暂时不会起作用
            param["reqReserved"] = sOrderId;//请求方保留域，透传字段，查询、通知、对账文件中均会原样出现

            SDKUtil.Sign(param, Encoding.UTF8);


            // 初始化通信处理类
            HttpClient hc = new HttpClient(SDKConfig.AppRequestUrl);
            //// 发送请求获取通信应答
            int status = hc.Send(param, Encoding.UTF8);
            // 返回结果
            string result = hc.Result;
            if (status == 200)
            {
                Dictionary<string, string> resData = SDKUtil.CoverstringToDictionary(result);
                string respcode = resData["respCode"];
                if (SDKUtil.Validate(resData, Encoding.UTF8))
                {
                    if (respcode == "00")
                    {
                        sMsg = resData["tn"];
                        return true;
                    }

                    sMsg = resData["respMsg"];
                    return false;

                }
                sMsg = "商户端验证返回报文签名失败";
                return false;
            }
            sMsg = "请求失败,返回报文=[" + result + "]";
            return false;
            #endregion
        }


        /// <summary>
        /// 获取交易流水号
        /// </summary>
        /// <param name="sOrderNo">订单号</param>
        /// <param name="sNotifyUrl">支付回调地址</param>
        /// <param name="sReturnUrl">支付成功回掉地址</param>
        /// <param name="sMsg">交易流水号</param>
        /// <param name="dPaid">支付金额</param>
        /// <returns></returns>
        static public bool CreateQueryID(string sOrderNo, string sNotifyUrl, string sReturnUrl, ref string sMsg, decimal dPaid,string sBody)
        {
            #region 获取交易流水号
            Dictionary<string, string> param = new Dictionary<string, string>();

            string notify_url = sNotifyUrl;
            string return_url = "http://" + HttpContext.Current.Request.Url.Authority + sReturnUrl; 

            //填写参数
            param["version"] = "5.0.0";//版本号
            param["encoding"] = "UTF-8";//编码方式
            param["certId"] = CertUtil.GetSignCertId();      //证书ID
            param["txnType"] = "01";//交易类型
            param["txnSubType"] = "01";//交易子类
            param["bizType"] = "000201";//业务类型
            param["frontUrl"] = return_url;    //前台通知地址      
            param["backUrl"] = notify_url;  //后台通知地址，改自己的外网地址
            param["signMethod"] = "01";//签名方法
            param["channelType"] = "08";//渠道类型，07-PC，08-手机
            param["accessType"] = "0";//接入类型
            param["merId"] = ConfigurationManager.AppSettings["MerId"];//商户号，请改成自己的商户号
            param["orderId"] = sOrderNo;//商户订单号，8-40位数字字母
            param["txnTime"] = DateTime.Now.ToString("yyyyMMddHHmmss");//订单发送时间
            param["txnAmt"] = (dPaid * 100).ToString("f0");//交易金额，单位分
            param["currencyCode"] = "156";//交易币种
            param["orderDesc"] = sBody;//订单描述，暂时不会起作用
            param["reqReserved"] = sOrderNo;//请求方保留域，透传字段，查询、通知、对账文件中均会原样出现

            SDKUtil.Sign(param, Encoding.UTF8); 

            // 初始化通信处理类
            HttpClient hc = new HttpClient(SDKConfig.AppRequestUrl);
            //// 发送请求获取通信应答
            int status = hc.Send(param, Encoding.UTF8);
            // 返回结果
            string result = hc.Result;
            if (status == 200)
            {
                Dictionary<string, string> resData = SDKUtil.CoverstringToDictionary(result);
                string respcode = resData["respCode"];
                //if (SDKUtil.Validate(resData, Encoding.UTF8))
                //{
                    if (respcode == "00")
                    {
                        sMsg = resData["tn"];
                        return true;
                    } 
                    sMsg = resData["respMsg"];
                    return false; 
                //}
                sMsg = "商户端验证返回报文签名失败";
                return false;
            }
            sMsg = "请求失败,返回报文=[" + result + "]";
            return false;
            #endregion
        }
    }
}