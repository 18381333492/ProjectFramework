using Framework.BLL;
using Framework.DTO;
using Framework.web.Controllers;
using Framework.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TenPay.Base;
using Framework.Helper;
using System.Text;
using WeiXin.Tool;
using Framework.web.config;
using Onlinepay;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using WeiXin.Base;
using Framework.AppCache;

namespace Framework.web.Areas.Client.Controllers
{
    /// <summary>
    /// 支付控制器
    /// </summary>
    [ClientLoginFilter]
    public class PayController : APISuperController
    {
        // GET: Client/Pay
        private IPayManager domin;
        public PayController()
        {
            domin = LoadInterface<IPayManager>();
        }

        #region 微信公众号支付

        /// <summary>
        /// 支付页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            string code = Request.QueryString["code"];
            string appid = TenPayConfig.appid;
            string openid = string.Empty;
            if (string.IsNullOrEmpty(code))
            {
                var path = string.Format("{0}{1}", config.WebConfig.LoadDynamicJson("weixin").LocalDomain, Request.Url.LocalPath);
                // 获取code
                string code_url = string.Format(
                    @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=lk#wechat_redirect",
                    appid,
                    HttpUtility.UrlEncode(path, Encoding.UTF8));
                return Redirect(code_url);
            }
            else
            {
                string appsecret = TenPayConfig.appsecret;

                // 通过code换取网页授权access_token
                string url = string.Format(
                    @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    appid, appsecret, code);

                dynamic ret_param = JSONHelper.GetModel(HttpHelper.Post(url, string.Empty));

                string access_token = ret_param.access_token;
                openid = ret_param.openid;
            }
            ViewBag.openid = openid;
            return View();
        }

        /// <summary>
        /// 检查订单的状态
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        [HttpPost]
        public void CheckOrder()
        {
            var client = ((EHECD_ClientDTO)GetSessionInfo("_client"));//获取会员信息
            string sPhone = client.sPhone;//会员电话
            string sNickName = client.sNickName;//会员昵称
            var data = LoadParam<Dictionary<string, object>>();
            Guid sOrderId = data["sOrderId"].ToGuid();
            string sOrderNo = data["sOrderNo"].ToString();
            string total_fee = (decimal.Parse(data["total_fee"].ToString())*100).ToString("f0");
            string openid = data["openid"].ToString();
            string sGoodsName = data["sGoodsName"].ToString();
            var spbill_create_ip = Request.UserHostAddress;
            //检查订单状态
            int res =domin.CheckOrder(sOrderId);
            switch (res)
            {
                case 0:
                    {
                        string prepay_id = string.Empty;
                        bool sResult = JsPay(sOrderId, total_fee, spbill_create_ip, sOrderNo, openid, sGoodsName, sPhone, sNickName, out prepay_id);
                        if (sResult)
                        {//获取预支付Id成功
                            var param = new Dictionary<string, string>();
                            param.Add("appId", TenPayConfig.appid);
                            param.Add("timeStamp", TenPayConfig.time_stamp());
                            param.Add("nonceStr", TenPayConfig.nonce_str());
                            param.Add("package", string.Format("prepay_id={0}", prepay_id));
                            param.Add("signType", "MD5");
                            //创建签名
                            string paySign = TenPaySign.CreateSign(param);
                            result.Data = new
                            {
                                appId = param["appId"],//公众号名称，由商户传入     
                                timeStamp = param["timeStamp"], //时间戳，自1970年以来的秒数     
                                nonceStr = param["nonceStr"], //随机串     
                                package = param["package"],
                                signType = param["signType"], //微信签名方式：     
                                paySign= paySign //微信签名 
                            };
                            result.Succeeded = true;
                        }
                        else
                        {
                            result.Msg = "参数错误!";
                        }
                        break;
                    }
                case 1: result.Msg = "该订单已支付过,不能再支付!"; break;
                default: result.Msg = "该订单状态异常,不能发起支付!"; break;
            }
        }

        /// <summary>
        ///  微信公众号支付
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <param name="total_fee"></param>
        /// <param name="spbill_create_ip"></param>
        /// <param name="sOrderNo"></param>
        /// <param name="prepay_id">预支付ID</param>
        /// <param name="openid">Openid</param>
        /// <returns></returns>
        private bool JsPay(Guid sOrderId, string total_fee, string spbill_create_ip, string sOrderNo,string openid, string sGoodsName ,string sPhone,string sNickName, out string prepay_id)
        {
            //设置缓存
            ApplicationCache.Instance.SetValue(sOrderNo, new PayMessage()
            {
                sOrderNo = sOrderNo,
                total_fee = decimal.Parse(total_fee) / 100,
                sGoodsName = sGoodsName,
                sPhone = sPhone,
                sNickName = sNickName

            }, 120);

            prepay_id = string.Empty;
            var client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var Parameters = new Dictionary<string, string>();

            string notify_url=config.WebConfig.LoadDynamicJson("weixin").notify_url;

            Parameters.Add("appid", TenPayConfig.appid);
            Parameters.Add("mch_id", TenPayConfig.mch_id);
            Parameters.Add("nonce_str", TenPayConfig.nonce_str());
            Parameters.Add("body", "友客商城订单");//订单描述
            Parameters.Add("out_trade_no", sOrderNo);
            Parameters.Add("total_fee", total_fee);//订单总金额，单位为分
            Parameters.Add("spbill_create_ip", spbill_create_ip);//终端IP(APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP)
            Parameters.Add("notify_url", notify_url);
            Parameters.Add("trade_type", "JSAPI");
            Parameters.Add("product_id", sOrderId.ToString());//订单ID
            Parameters.Add("openid", openid);
            //创建签名
            string sign = TenPaySign.CreateSign(Parameters);
            Parameters.Add("sign", sign);
            //组装统一下单数据格式 
            string RequestData = TenPayHelp.InstallXml(Parameters);
            //请求统一下单支付API
            string sUrl = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            string sResult = TenPayHelp.HttpPost(sUrl, RequestData);//调用接口
            //解析数据
            var NewParameters = TenPayHelp.GetDictionaryFromCDATAXml(sResult);

            if (TenPaySign.CheckSign(NewParameters))
            {//验证签名
                if (NewParameters["return_code"] == "SUCCESS")
                {
                    if (NewParameters["result_code"] == "SUCCESS")
                    {//统一下单成功
                        prepay_id = NewParameters["prepay_id"];//该值有效期为2小时
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 微信异步回调函数
        /// </summary>
        public void AsyncCallBack()
        {
            lock (this)
            {
                Response.Clear();
                Response.ContentType = "text/xml";
                var ResponeDic = new Dictionary<string, string>();
                ResponeDic.Add("return_code", "FAIL");
                ResponeDic.Add("return_msg", "业务处理失败");

                byte[] bNetStream = Request.BinaryRead(Request.ContentLength);//获取微信回调的请求数据的字节流
                string sParam = System.Text.Encoding.Default.GetString(bNetStream);

                var Parameters = TenPayHelp.GetDictionaryFromCDATAXml(sParam);
                if (TenPaySign.CheckSign(Parameters))
                {//验证签名
                    if (Parameters["return_code"] == "SUCCESS")
                    {
                        if (Parameters["result_code"] == "SUCCESS")
                        {//支付成功
                            string sOrderNo = Parameters["out_trade_no"];
                            /*
                             * 业务处理
                             */
                            int res =domin.AlterOrderState(sOrderNo);
                            if (res > 0)
                            {//业务处理成功
                                ResponeDic["return_code"] = "SUCCESS";
                                ResponeDic["return_msg"] = "OK";
                                Response.Write(TenPayHelp.InstallCDATAXml(ResponeDic));
                                //发送模板消息
                                 var message= ApplicationCache.Instance.GetValue(sOrderNo) as PayMessage;
                                SendMessage(message);
                            }
                            else
                            {
                                Response.Write(TenPayHelp.InstallCDATAXml(ResponeDic));
                            }          
                        }
                        else
                        {
                            Response.Write(TenPayHelp.InstallCDATAXml(ResponeDic));
                        }
                    }
                    else Response.Write(TenPayHelp.InstallCDATAXml(ResponeDic));
                }
                else
                {
                    Response.Write(TenPayHelp.InstallCDATAXml(ResponeDic));
                }
            }
        }


        /// <summary>
        /// 支付成功发送模板消息
        /// </summary>
        public void SendMessage(PayMessage message)
        {
            try
            {
                //发送短信通知
                var messger = base.LoadInterface<Validate.IMessager>();//短信接口
                messger.SendMessage(message.sPhone,
                    string.Format("【友客分享商城】您购买的商品{0}已成功付款,请注意查看", message.sGoodsName));


                /**发送模板消息**/
                //获取微信access_token
                string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
                EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
                Access_Token token = new Access_Token(set.sOriginalID, set.sAppId, set.sAppSecret);

                string sOpenId = domin.GetOpenid(message.sOrderNo);

                // ====组装基础POST数据====
                var tempJson = new JObject();

                tempJson.Add(new JProperty("touser", sOpenId));
                tempJson.Add(new JProperty("template_id", "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w"));

                // data 数据部分
                var tempJsonChild = new JObject();
                tempJsonChild.Add(new JProperty("first", new JObject(new JProperty("value", "你有新的订单请注意！"))));
                tempJsonChild.Add(new JProperty("keyword1", new JObject(new JProperty("value", message.sOrderNo))));
                tempJsonChild.Add(new JProperty("keyword2", new JObject(new JProperty("value", message.sGoodsName))));
                tempJsonChild.Add(new JProperty("keyword3", new JObject(new JProperty("value", "￥"+message.total_fee.ToString("f2")))));
                tempJsonChild.Add(new JProperty("keyword4", new JObject(new JProperty("value", message.sNickName))));
                tempJsonChild.Add(new JProperty("remark", new JObject(new JProperty("value", message.sPhone))));

                tempJson.Add(new JProperty("data", tempJsonChild));

                // ======组装数据完成======

                byte[] bytes = Encoding.UTF8.GetBytes(tempJson.ToString());
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + token.sToken);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "json";
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                //声明一个HttpWebRequest请求    
                request.Timeout = 90000;
                //设置连接超时时间    
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.UTF8;

                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                string strResult = streamReader.ReadToEnd();
                streamReceive.Dispose();
                streamReader.Dispose();
            }
            finally { }
        } 

        #endregion





        #region 银联支付


        //银联异步通知地址onlinepay_notify
        private static readonly string online_notify_url = ConfigurationManager.AppSettings["onlinepay_notify"];

        // POST /OnlinePay/OnlineUrl?orderId=&platform=
        /// <summary>
        /// 创建银联支付url
        /// </summary>
        /// <param name="sOrderId">订单ID</param>
        /// <param name="platform">渠道类型 07 - PC，08 - 手机</param>
        /// <returns></returns>
        public void OnlineUrl()
        {
            string platform = "08";//手机
            var data = LoadParam<Dictionary<string, object>>();
            Guid sOrderId = data["sOrderId"].ToGuid();
            string sOrderNo = data["sOrderNo"].ToString();
            string total_fee = data["total_fee"].ToString();
            int res = domin.CheckOrder(sOrderId);
            switch (res)
            {
                case 0:
                    {
                        string spayUrl =//支付Url
                       OnlinepayHelp.CreateOnlinepayUrl(
                       platform, sOrderId.ToString(), sOrderNo, total_fee.ToString(),
                        online_notify_url,//异步通知地址
                       "/Client/Pay/Success/",
                       "有客商城订单");
                        result.Succeeded = true;
                        result.Data = SafeHtmlValue(spayUrl);
                        break;
                    }
                case 1: result.Msg = "该订单已支付过,不能再支付!"; break;
                default: result.Msg = "该订单状态异常,不能发起支付!"; break;
            }
        }

        /// <summary>
        /// 在线支付 异步通知接口
        /// </summary>
        public void OnlineNotify()
        {
            #region 在先支付异步通知接口
            if (Request.HttpMethod == "POST")
            {
                // 使用Dictionary保存参数
                Dictionary<string, string> resData = new Dictionary<string, string>();
                NameValueCollection coll =Request.Form;
                string[] requestItem = coll.AllKeys;
                for (int i = 0; i < requestItem.Length; i++)
                {
                    resData.Add(requestItem[i], Request.Form[requestItem[i]]);
                }
                string respcode = resData["respCode"];
                ////返回报文中不包含UPOG,表示Server端正确接收交易请求,则需要验证Server端返回报文的签名
                if (SDKUtil.Validate(resData, Encoding.UTF8))
                {
                    if (respcode == "00")
                    {
                        //订单编号
                        string sOrderNo = resData["reqReserved"];
                        //支付金额
                        string sMoney = resData["txnAmt"];
                        /* * 订单处理 * */
                        if (domin.AlterOrderState(sOrderNo) >0)
                        {
                            //反馈银联
                            Response.Write("success");
                        }
                        else
                        {
                            Response.Write("fail");
                        }
                    }
                    else
                        Response.Write("fail");
                }
                else
                    Response.Write("商户端验证银联返回报文结果");
            }
            else
                Response.Write("fail");
            #endregion
        }

        /// <summary>
        /// 将字符串转换为安全的HTML值
        /// </summary>
        /// <param name="str">源串</param>
        /// <returns>结果</returns>
        public string SafeHtmlValue(string str)
        {
            #region 将字符串转换为安全的HTML值
            return string.IsNullOrEmpty(str)
                    ? str
                    : str.Replace("&", "&amp;")
                        .Replace("<", "&lt;")
                        .Replace(">", "&gt;")
                        .Replace("'", "&apos;")
                        .Replace("\"", "&quot;")
                        .Replace("=", "&3D;");
            #endregion
        }

        #endregion


        /// <summary>
        /// 支付成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Success()
        {
            return View();
        }

        //发送支付成功需要模板消息
        [Serializable]
        public class PayMessage
        {
            public string sOrderNo;
            public string sGoodsName;
            public decimal total_fee;
            public string sPhone;
            public string sNickName;
        }
    }
}