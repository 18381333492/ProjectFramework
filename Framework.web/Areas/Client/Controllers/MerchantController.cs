using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using Framework.web.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WeiXin.Base;
using WeiXin.Base.JsApi;
using WeiXin.Base.User.UserInfo;
using WeiXin.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Framework.web.Areas.Client.Controllers
{
    public class MerchantController : APISuperController
    {
        // GET: Client/Merchant

        #region 订单列表
        public ActionResult Index()
        {
            //var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            //if (manageID == null)
            //{
            //    return View("/Client/MerchantLogin/Index"); 
            //}
            return View();
        }
        /// <summary>
        /// 获取所有订单信息
        /// </summary>
      
        public void GetAllOrder()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var manageID = (Dictionary<string,object>)GetSessionInfo("_merchant");
            
                var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetAllOrder(page, manageID["sShopID"].ToString());
                if (list != null)
                {
                    result.Data = list;
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "获取失败";
                }
            
            
        }
        /// <summary>
        /// 获取未付款
        /// </summary>
      
        public void GetNoPay()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetNotPay(page, manageID["sShopID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        /// <summary>
        /// 获取未使用
        /// </summary>
     
        public void GetNoUse()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetNotUsed(page, manageID["sShopID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        /// <summary>
        /// 获取已使用
        /// </summary>
       
        public void GetHaveUsed()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetHaveUsed(page, manageID["sShopID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }


        #endregion



        #region 扫描核销
        public ActionResult Scanner()
        {
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            //WxKeyId
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
          //  return View();
            
        }  
        
        /// <summary>
        /// 订单核销 
        /// </summary>
        public void CancelOrder() {
            Dictionary<string, object> orderID = LoadParam<Dictionary<string, object>>();
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            var ret = LoadInterface<IShopDetailManager>().CancelOrder(orderID["sOrderNo"].ToString(), manageID["sShopID"].ToString());
            if (ret != null)
            {
                result.Data = ret["message"];
                result.Succeeded = true;
                Dictionary<string, object> dic1 =(Dictionary<string,object>) ret["dic1"];
                Dictionary<string, object> dic2 = (Dictionary<string, object>)ret["dic2"];
                Dictionary<string, object> dic3 = (Dictionary<string, object>)ret["dic3"];
                //如果有一级分销客，发消息
                if (Boolean.Parse(dic1["havaOne"].ToString()) == true) {
                    SendMessageToSuper(dic1);
                 }
                //如果有二级分销客，发消息
                if (Boolean.Parse(dic2["havaOne"].ToString()) == true)
                {
                    SendMessageToSuper(dic2);
                }
                //如果有三级分销客，发消息
                if (Boolean.Parse(dic3["havaOne"].ToString()) == true)
                {
                    SendMessageToSuper(dic3);
                }

            }
            else
            {
                result.Succeeded = false;
                result.Msg = "核销失败";
            }
        }
        #endregion

        #region 分享客列表
        public ActionResult ShareList()
        {
            return View();
        }
        /// <summary>
        /// 本店所有分销客
        /// </summary>
      
        public void GetStoreShare()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetStoreShare(page, manageID["sShopID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        #endregion

        #region 分享客详情
        public ActionResult ShareDetail()
        {
            return View();
        }
        #endregion
        /// <summary>
        /// 分销客的姓名等基本信息
        /// </summary>
        public void GetShareClient()
        {
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetShareClient(dic["ID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }

        /// <summary>
        /// 分销客详情
        /// </summary>
       
        public void GetShareDetail()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var manageID = (Dictionary<string, object>)GetSessionInfo("_merchant");
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetShareDetail(page, manageID["sShopID"].ToString(), dic["sClientID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        #region 订单详情
        public ActionResult Detail()
        {
            return View();
        }


        public void GetDetail() {
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IMerchantManager>().GetDetail(dic["ID"].ToString());
            bool flag = list != null;
            result.Data = list;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }
        #endregion


        private void SendMessageToSuper(Dictionary<string,object> dic)
        {
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);

            // ======================【基础支持的access_token】调用 发送模板消息的接口===============================
            Access_Token token = new Access_Token(set.sOriginalID, set.sAppId, set.sAppSecret);

                // 发送给上级分销客【当前分销客】
                Send(token.sToken,dic);
            
        }

        public string Send(string access_token,Dictionary<string,object> dic)
        {
            dynamic postData = new ExpandoObject();
            postData.touser = dic["sOpenId"];
            postData.template_id = "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w";
            postData.url = string.Empty;
            postData.topcolor = "#ffffff";
            postData.data = new ExpandoObject();

            // ====组装基础POST数据====
            var tempJson = new JObject();

            tempJson.Add(new JProperty("touser", dic["sOpenId"]));
            tempJson.Add(new JProperty("template_id", "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w"));

            // data 数据部分
            var tempJsonChild = new JObject();
            tempJsonChild.Add(new JProperty("first", new JObject(new JProperty("value", "恭喜您获得佣金！"))));
            tempJsonChild.Add(new JProperty("keyword1", new JObject(new JProperty("value", dic["name"]))));
            tempJsonChild.Add(new JProperty("keyword2", new JObject(new JProperty("value",  dic["stage"]))));
            tempJsonChild.Add(new JProperty("keyword3", new JObject(new JProperty("value", dic["goodsname"]))));
            tempJsonChild.Add(new JProperty("keyword4", new JObject(new JProperty("value", dic["money"]))));
            tempJsonChild.Add(new JProperty("remark", new JObject(new JProperty("value", ""))));
            tempJson.Add(new JProperty("data", tempJsonChild));

          

            byte[] bytes = Encoding.UTF8.GetBytes(tempJson.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + access_token);
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
            return strResult;


        }
    }
}