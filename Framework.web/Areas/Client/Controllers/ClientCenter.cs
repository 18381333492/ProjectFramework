using Framework.BLL;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;
using Framework.Dapper;
using Framework.web.Models;
using Framework.DTO;
using Framework.AppCache;
using WeiXin.Base.JsApi;
using System.Net;
using System.Text;
using System.IO;
using WeiXin.Base;
using Framework.web.config;
using System.Web;

namespace Framework.web.Areas.Client.Controllers
{
    /// <summary>
    ///  用户个人中心
    /// </summary>
    [ClientLoginFilter]
    public class ClientCenterController : APISuperController
    {

        private IClientCenterManager domin = DI.DIEntity.GetInstance().GetImpl<IClientCenterManager>();

        #region 视图
        /// <summary>
        /// 会员个人中心页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //iClientType: 0 - 普通客户,1 - 分销客
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            ViewBag.Img = Client.sHeadPic;
            ViewBag.iClientType = Client.iClientType;
            return View();
        }
        #endregion


        /// <summary>
        /// 获取会员昵称
        /// </summary>
        public void GetClientNickName()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            result.Data = domin.GetClientNickName(ClientId);
            result.Succeeded = true;
        }


        /// <summary>
        ///  获取待付款和待使用订单未读数目和获取站内信未读条数
        /// </summary>
        public void GetClientInfo()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            Dictionary<string, object> Collection = domin.GetPayUseOrderCount(ClientId) as Dictionary<string, object>;
            var temp= domin.GetNoReadMessageCount(ClientId) as Dictionary<string, object>;
            Collection.Add("infoCount", temp["infoCount"]);
            result.Data = Collection;
            result.Succeeded = true;
        }


        #region 站内信

        /// <summary>
        /// 站内信
        /// </summary>
        /// <returns></returns>
        public ActionResult Message()
        {
            return View();
        }

        /// <summary>
        /// 站内信列表数据
        /// </summary>
        public void MessageList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            PageInfo Info = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            result.Data = domin.GetSysMessageList(ClientId, Info);
            result.Succeeded = true;
        }

        /// <summary>
        /// 根据ID获取站内信详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult MessageDetail(Guid ID)
        {
            var model = domin.GetSysMessageDetail(ID);
            domin.SetiRecStatus(ID);//设置状态为以读
            return View(model);
            
        }

        #endregion


        #region 订单相关

        /// <summary>
        /// 订单视图
        /// </summary>
        /// <param name="iState"></param>
        /// <returns></returns>
        public ActionResult Order(int iState)
        {
            //iState:-1所有的订单, 0待付款 1待使用 2-已核销 3-维权
            switch (iState)
            {
                case -1: ViewBag.sTitle = "全部订单"; break;
                case 0:ViewBag.sTitle = "待付款订单";break;
                case 1: ViewBag.sTitle = "待使用订单"; break;
                case 2: ViewBag.sTitle = "核销订单"; break;
                case 3: ViewBag.sTitle = "维权订单"; break;
                default:
                    ViewBag.sTitle = string.Empty;break;
            }
            ViewBag.iState = iState;
            return View();
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="ID"></param>
        public ActionResult OrderDetail(Guid ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        /// <summary>
        /// 获取订单详情数据
        /// </summary>
        /// <param name="ID"></param>
        public void LoadOrderDetail()
        {
            var param = LoadParam<Dictionary<string,object>>();
            Guid ID = new Guid(param["ID"].ToString());
            result.Data = domin.GetOrderDetail(ID);
            result.Succeeded = true;
        }

        /// <summary>
        /// 会员的订单列表数据
        /// </summary>
        public void OrderList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            int iState =Convert.ToInt32(RequestParameters.dynamicData.data.iState);
            PageInfo Info = LoadParam<PageInfo>();
            result.Data = domin.GetOrderListByState(ClientId, iState, Info);
            result.Succeeded = true;
        }

        /// <summary>
        /// 根据订单ID取消订单
        /// </summary>
        public void OrderCancel()
        {
            Guid sOrderId = LoadParam<Dictionary<string, object>>()["sOrderId"].ToGuid();
            if (domin.OrderCancel(sOrderId) > 0)
            {
                result.Succeeded = true;
            }
        }


        /// <summary>
        /// 根据订单ID申请退款
        /// </summary>
        public void OrderReturn()
        {
            Guid sOrderId = LoadParam<Dictionary<string, object>>()["sOrderId"].ToGuid();
            int res = domin.OrderReturn(sOrderId);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                if (res == -2)
                {
                    result.Succeeded = false;
                    result.Msg = "订单已核销";
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "操作失败!";
                }
                  
            }
        }


        /// <summary>
        /// 订单评价页面
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderAppraise()
        {
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            //WxKeyId
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
        }

        /// <summary>
        /// 评价订单
        /// </summary>
        public void Appraise()
        {
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var comment = LoadParam<EHECD_CommentDTO>();
            comment.sCommenterName = Client.sNickName;
            comment.sCommenterID = Client.ID;
            if (domin.OrderAppraise(comment)>0)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Msg = "评价失败";
            }
        }


        /// <summary>
        /// 查看评价页面
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public ActionResult CheckAppraise(Guid sOrderId)
        {
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var model = domin.GetAppraise(sOrderId);
            ViewBag.sHeadPic = Client.sHeadPic;
            ViewBag.sNickName = Client.sNickName;
            model["dReplayTime"]= string.Format("{0:g}", model["dReplayTime"]);
            return View(model);
        }

        /// <summary>
        /// 从微信服务器上下载图片
        /// </summary>
        public void LoadImageByWX()
        {
            string[] serverIds = LoadParam<Dictionary<string, string>>()["serverId"].Split(',');
            //获取微信access_token
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            Access_Token token = new Access_Token(set.sOriginalID,set.sAppId,set.sAppSecret);

            string sPathArray = string.Empty;
            //循环下载图片
            foreach (var serverId in serverIds)
            {
                var sUrl = string.Format("https://api.weixin.qq.com/cgi-bin/media/get?access_token={0}&media_id={1}", token.sToken, serverId);
                string sDate = DateTime.Now.ToString("yyyy-MM");
                string sFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";

                HttpWebRequest webRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(sUrl);
                webRequest.ProtocolVersion = HttpVersion.Version10;
                webRequest.Timeout = 30000;
                webRequest.Method = "GET";
                webRequest.UserAgent = "Mozilla/4.0";
                webRequest.Headers.Add("Accept-Encoding", "gzip, deflate");
                HttpWebResponse webResponse = (System.Net.HttpWebResponse)webRequest.GetResponse();
                System.IO.Stream streamReceive = webResponse.GetResponseStream();

                MemoryStream memoryStream = new MemoryStream();
                const int bufferLength = 1024;
                int actual;
                byte[] buffer = new byte[bufferLength];
                while ((actual = streamReceive.Read(buffer, 0, bufferLength)) > 0)
                {
                    memoryStream.Write(buffer, 0, actual);
                }
                streamReceive.Close();
                memoryStream.Position = 0;

                byte[] buffers = new byte[memoryStream.Length];
                memoryStream.Read(buffers, 0, buffers.Length);
                memoryStream.Close();
                //将图片上传到微信服务器上
                string url = WebConfig.LoadElement("url");
                string res=UploadByteImage(buffers, "OrderImg.jpeg", url);
                var obj= JSONHelper.GetModel(res);
                //组装数据
                sPathArray= sPathArray+obj["filePath"].ToString()+",";
            }
            result.Succeeded = true;
            result.Data = sPathArray.TrimEnd(',');
        }

        /// <summary>
        /// 上传图片到图片服务器上
        /// </summary>
        /// <param name="postData">byte图片数据</param>
        /// <param name="byteFileName">文件名称</param>
        /// <param name="url">标准 服务器图片地址</param>
        /// <returns></returns>
        public  string UploadByteImage(byte[] postData, string byteFileName, string url)
        {
            using (MemoryStream msstream = new MemoryStream())
            {
                //2.将【标准 服务器图片地址】修改为【base64 服务器图片地址】
                string requestFileExtension = byteFileName.Substring(byteFileName.LastIndexOf('.'));//获取后缀名
                string fileName = GuidHelper.GetSecuentialGuid().ToString() + requestFileExtension;
                url += "&action=custom&imagename=" + fileName;

                //3.准备请求 设置参数
                Encoding encoding = Encoding.UTF8;//设置编码格式
                HttpWebRequest request = WebRequest.CreateHttp(url);
                request.Method = "POST";

                // 边界符
                var boundary = DateTime.Now.Ticks.ToString("x");

                // 边界符
                var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
                // 最后的结束符
                var endBoundary = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

                request.ContentType = "multipart/form-data; boundary=" + boundary;

                // 写入文件
                const string filePartHeader =
                    "Content-Disposition: form-data; name=\"postImg\"; filename=\"{0}\"\r\n" +
                     "Content-Type: application/octet-stream\r\n\r\n";

                var header = string.Format(filePartHeader, fileName);
                var headerbytes = Encoding.UTF8.GetBytes(header);

                msstream.Write(beginBoundary, 0, beginBoundary.Length);
                msstream.Write(headerbytes, 0, headerbytes.Length);

                msstream.Write(postData, 0, postData.Length);//将数据写入流中

                // 写入最后的结束边界符
                msstream.Write(endBoundary, 0, endBoundary.Length);

                request.ContentLength = msstream.Length;
                msstream.Position = 0;

                //4.Stream操作
                Stream requestStream = null;//建立请求输入流
                StreamReader reader = null;//获取服务器返回流
                try
                {
                    //4.1输入流写入数据
                    requestStream = request.GetRequestStream();//获取输入流     
                    var tempBuffer = new byte[msstream.Length];
                    msstream.Read(tempBuffer, 0, tempBuffer.Length);
                    requestStream.Write(tempBuffer, 0, tempBuffer.Length);
                    requestStream.Close();

                    //4.2输出流读取数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    reader = new StreamReader(response.GetResponseStream(), encoding);
                    string responseStr = reader.ReadToEnd();//读取返回流数据

                    return responseStr;//返回Json数据
                }
                finally
                {
                    if (requestStream != null)
                    {
                        requestStream.Close();//关闭输入流
                    }
                    if (reader != null)
                    {
                        reader.Close(); //关闭输出流
                    }
                }
            }
        }


        #endregion


        #region 个人信息

        /// <summary>
        /// 个人信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonInfo()
        {
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            //WxKeyId
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());

            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            ViewBag.sHeadPic = Client.sHeadPic;
            ViewBag.sPhone = Client.sPhone;
            ViewBag.sNickName = Client.sNickName;
            return View(item);
        }

        /// <summary>
        /// 获取用户的基本信息
        /// </summary>
        public void AlertInfo()
        {
            var client = ((EHECD_ClientDTO)GetSessionInfo("_client"));
            var data = LoadParam<Dictionary<string, string>>();
            string sHeadPic = data["sHeadPic"];
            string sNickName = data["sNickName"];
            if(domin.AlertInfo(client.ID.ToGuid(), sNickName, sHeadPic) > 0)
            {//更新session
                client.sNickName = sNickName;
                client.sHeadPic = sHeadPic;
                SetSessionInfo("_client", client);
                result.Succeeded = true;
            }
        }

        #endregion


        #region 收藏夹
        
        /// <summary>
        /// 我的收藏夹
        /// </summary>
        /// <returns></returns>
        public ActionResult Collection()
        {
            return View();
        }

        /// <summary>
        /// 获取会员的收藏夹
        /// </summary>
        public void LoadCollectionList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            PageInfo Info = LoadParam<PageInfo>();
            var iCollectType = LoadParam<Dictionary<string, object>>()["iCollectType"].ToInt32();
            result.Data = domin.GetCollectionList(Info, ClientId, iCollectType);
            result.Succeeded = true;       
        }

        /// <summary>
        /// 删除收藏夹
        /// </summary>
        public void CancelCollect()
        {
            string Ids= LoadParam<Dictionary<string, object>>()["Ids"].ToString();
            if (domin.CancelCollect(Ids)>0)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Msg = "操作失败!";
            }
        }
        #endregion


        #region 我的优惠券

        /// <summary>
        /// 我的优惠券
        /// </summary>
        /// <returns></returns>
        public ActionResult Coupon()
        {
            return View();
        }


        /// <summary>
        /// 获取会员的优惠券
        /// </summary>
        public void GetCouponList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            PageInfo Info = LoadParam<PageInfo>();
            int bIsUsed = LoadParam<Dictionary<string, object>>()["bIsUsed"].ToInt32();
            result.Data = domin.GetCouponList(Info,ClientId,bIsUsed);
            result.Succeeded = true;
        }

        #endregion


        #region 帮助中心
        /// <summary>
        /// 帮助中心
        /// </summary>
        public ActionResult HelpCenter()
        {
            return View();
        }

        /// <summary>
        /// 获取帮助中心数据列表
        /// </summary>
        public void HelpCenterList()
        {
            result.Data = domin.GetHelpCenterList();
            result.Succeeded = true;
        }

        /// <summary>
        /// 根据ID获取帮助中心详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult HelpCenterDeatil(Guid ID)
        {
            var model=domin.GetHelpCenterDetail(ID) as EHECD_ArticleDTO;
            ViewBag.sContent = model.sContent;
            ViewBag.sTitle = model.sTitle;
            return View();
        }

        #endregion


        #region 客户电话

        /// <summary>
        /// 获取客户电话
        /// </summary>
        public void GetCustomerPhone()
        {

        }

        #endregion


        #region 退出登录
        /// <summary>
        /// 退出登录
        /// </summary>
        public void Quit()
        {
            //移除session并移除cookie
            Session["_client"] = null;
            Session.Remove("_client");
            HttpCookie aCookie = Request.Cookies["_client"];
            aCookie.Value = null;
            Response.SetCookie(aCookie);
            result.Succeeded = true;
        }

        #endregion


        #region 成为分享客


        /// <summary>
        /// 成为分享客
        /// </summary>
        public ActionResult ShareClient()
        {
            int iState = 0;
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            if (domin.IntoShareClient(ClientId)>0)
            {//申请成功，更新Session
                iState = 1;
                var client=LoadInterface<IClientManager>();
                var ret = client.QueryClientInfo(new EHECD_ClientDTO()
                {
                    ID = ClientId
                });
                SetSessionInfo("_client", ret);
            }
            return View("~/Areas/Client/Views/ClientCenter/ShareClientSuccess.cshtml", iState);
        }
        #endregion


        #region 成为合伙人

        /// <summary>
        /// 成为合伙人视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Partner(int New=0)
        {
            if (New == 0)
            {
                var client = ((EHECD_ClientDTO)GetSessionInfo("_client"));
                int iState = 0; //0 - 未通过 1 - 已通过 2 - 拒绝
                if (!domin.IsPartner(client.sPhone, out iState))
                {//未申请过
                    return View();
                }
                else
                {//已申请过
                    return View("~/Areas/Client/Views/ClientCenter/Success.cshtml", iState);
                }
            }
            else
            {//申请失败再次申请.
                return View();
            }
        }
        /// <summary>
        /// 成为合伙人
        /// </summary>
        public void IntoPartner()
        {
            var param = LoadParam<EHECD_ApplyDTO>();
            int res = Checked(param.sMobileNum);
            if (res == 10)
            {
                var sCode = LoadParam<Dictionary<string, string>>()["sCode"];
                if (ApplicationCache.Instance.GetValue(param.sMobileNum) != null)
                {
                    if (ApplicationCache.Instance.GetValue(param.sMobileNum).ToString() == sCode)
                    {//验证码验证成功
                        var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
                        param.sClientID = ClientId;
                        if (domin.IntoPartner(param) > 0)
                        {
                            result.Succeeded = true;
                        }
                        else
                        {
                            result.Msg = "申请合伙人失败!";
                        }
                    }
                    else
                    {
                        result.Msg = "短信验证码验证失败!";
                    }
                }
                else
                {
                    result.Msg = "短信验证码验证时效,请重现获取验证码!";
                }
            }
            else
            {//检查不通过
                switch (res)
                {     //0：后台用户，1：店铺，2：合伙人
                    case 0: result.Msg = "该帐号已是平台用户,不能申请为合伙人"; break;
                    case 1: result.Msg = "该帐号已是商家,不能申请为合伙人"; break;
                    case 2: result.Msg = "该帐号已是合伙人,不能申请为合伙人"; break;
                    case -11: result.Msg = "该帐号正在申请成为合伙人,不能申请为合伙人"; break;
                    case -22: result.Msg = "该帐号正在申请成为商家,不能申请为合伙人"; break;
                    default:
                        result.Msg = "申请商家失败!"; break;
                }
            }
        }

        /// <summary>
        /// 获取合伙人手机验证码
        /// </summary>
        public void GetPartnerCheckCode()
        {
            var param = LoadParam<Dictionary<string, string>>()["sPhone"];//手机号码

            int res = Checked(param);//检查能否申请
            if (res == 10)
            {
                var outNumber = param + "TY";//时效的标识（唯一）

                //检查该账户在指定时间内是否已经获取过登录名
                if (ApplicationCache.Instance.GetValue(outNumber) != null)
                {
                    result.Succeeded = false;
                    result.Msg = "一分钟内已经获取过验证短信，请等待一分后再获取";
                    return;
                }

                var messger = base.LoadInterface<Validate.IMessager>();     //短信发送接口
                var code = messger.RandomTool.GetRandomNumberString(5);      //随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定
                if (messger.SendMessage(param, code))
                {
                    result.Succeeded = true;
                    result.Msg = "验证码发送成功!";
                    ApplicationCache.Instance.SetValue(outNumber, code, 60);
                    ApplicationCache.Instance.Delete(param);//先清除  在缓存
                    ApplicationCache.Instance.SetValue(param, code, 60 * 10);
                }
                else
                {
                    result.Msg = "验证码发送失败!";
                }
            }
            else
            {
                //检查不通过
                switch (res)
                {     //0：后台用户，1：店铺，2：合伙人
                    case 0: result.Msg = "该帐号已是平台用户,不能申请为合伙人"; break;
                    case 1: result.Msg = "该帐号已是商家,不能申请为合伙人"; break;
                    case 2: result.Msg = "该帐号已是合伙人,不能申请为合伙人"; break;
                    case -11: result.Msg = "该帐号正在申请成为合伙人,不能申请为合伙人"; break;
                    case -22: result.Msg = "该帐号正在申请成为商家,不能申请为合伙人"; break;
                    default:
                        result.Msg = "申请失败!"; break;
                }
            }
        }

        /// <summary>
        /// 申请合伙人, 成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Success()
        {
            return View();
        }

        #endregion


        #region 商家入驻

        /// <summary>
        /// 商家入驻视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Business(int New = 0)
        {
            if (New == 0)
            {
                var client = ((EHECD_ClientDTO)GetSessionInfo("_client"));
                int iState = 0; //0 - 未通过 1 - 已通过 2 - 拒绝
                if (!domin.IsBusiness(client.sPhone, out iState))
                {//未申请过
                    return View();
                }
                else
                {//已申请过
                    return View("~/Areas/Client/Views/ClientCenter/BusinessSuccess.cshtml", iState);
                }
            }
            else
            {//申请失败再次申请.
                return View();
            }
        }

        /// <summary>
        /// 获取成为商家的手机验证码
        /// </summary>
        public void GetBusinessCheckCode()
        {
            var param = LoadParam<Dictionary<string, string>>()["sPhone"];//手机号码

            int res = Checked(param);//检查该电话是否可以申请
            if (res == 10)
            {
                var outNumber = param + "TT";//时效的标识（唯一）
                                             //检查该账户在指定时间内是否已经获取过登录名
                if (ApplicationCache.Instance.GetValue(outNumber) != null)
                {
                    result.Succeeded = false;
                    result.Msg = "一分钟内已经获取过验证短信，请等待一分后再获取";
                    return;
                }

                var messger = base.LoadInterface<Validate.IMessager>();     //短信发送接口
                var code = messger.RandomTool.GetRandomNumberString(5);      //随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定
                if (messger.SendMessage(param, code))
                {
                    result.Succeeded = true;
                    result.Msg = "验证码发送成功!";
                    ApplicationCache.Instance.SetValue(outNumber, code, 60);
                    ApplicationCache.Instance.Delete(param);//先清除  在缓存
                    ApplicationCache.Instance.SetValue(param, code, 60 * 10);
                }
                else
                {
                    result.Msg = "验证码发送失败!";
                }
            }
            else
            {
                //检查不通过
                switch (res)
                {     //0：后台用户，1：店铺，2：合伙人
                    case 0: result.Msg = "该帐号已是平台用户,不能申请为商家"; break;
                    case 1: result.Msg = "该帐号已是商家,不能申请为商家"; break;
                    case 2: result.Msg = "该帐号已是合伙人,不能申请为商家"; break;
                    case -11: result.Msg = "该帐号正在申请成为合伙人,不能申请为商家"; break;
                    case -22: result.Msg = "该帐号正在申请成为商家,不能申请为商家"; break;
                    default:
                        result.Msg = "申请商家失败!"; break;
                }
            }
        }

        /// <summary>
        /// 成为商家
        /// </summary>
        public void IntoBusiness()
        {
            var param = LoadParam<EHECD_ApplyDTO>();
            int res = Checked(param.sMobileNum);
            if (res == 10)
            {//检查通过
                var sCode = LoadParam<Dictionary<string, string>>()["sCode"];//获取验证码
                if (ApplicationCache.Instance.GetValue(param.sMobileNum) != null)
                {
                    if (ApplicationCache.Instance.GetValue(param.sMobileNum).ToString() == sCode)
                    {//验证码验证成功
                        var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
                        param.sClientID = ClientId;
                        if (domin.IntoBusiness(param) > 0)
                        {
                            result.Succeeded = true;
                        }
                        else
                        {
                            result.Msg = "申请商家入驻失败!";
                        }
                    }
                    else
                    {
                        result.Msg = "短信验证码验证失败!";
                    }
                }
                else
                {
                    result.Msg = "短信验证码验证时效,请重现获取验证码!";
                }
            }
            else
            {//检查不通过
                switch (res)
                {     //0：后台用户，1：店铺，2：合伙人
                    case 0:  result.Msg = "该帐号已是平台用户,不能申请为商家"; break;
                    case 1:  result.Msg = "该帐号已是商家,不能申请为商家"; break;
                    case 2:  result.Msg = "该帐号已是合伙人,不能申请为商家"; break;
                    case -11: result.Msg = "该帐号正在申请成为合伙人,不能申请为商家"; break;
                    case -22: result.Msg = "该帐号正在申请成为商家,不能申请为商家"; break;
                    default:
                        result.Msg = "申请商家失败!";break;
                }
            }
        }


        /// <summary>
        /// 商家入驻成功页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BusinessSuccess()
        {
            return View();
        }

        #endregion



        /// <summary>
        ///  协议页面
        /// </summary>
        /// <param name="sTitle">标题</param>
        /// <returns></returns>
        public ActionResult Protocol(string sTitle)
        {
            var model = domin.GetProtocolByTitle(sTitle);
            return View(model);
        }


        /// <summary>
        /// 判断该用户是否是平台用户还是合伙人还是商家
        /// </summary>
        public int Checked(string sPhone)
        {
            int res = domin.Check(sPhone);
            return res;
        }
    }
}