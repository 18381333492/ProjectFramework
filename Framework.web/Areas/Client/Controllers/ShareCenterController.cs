using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using Framework.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WeiXin.Tool;
using WeiXin.Base.User;
using WeiXin.Base.User.UserInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using WeiXin.Base;
using Framework.Helper;

namespace Framework.web.Areas.Client.Controllers
{
    /// <summary>
    ///  分享中心
    /// </summary>
    public class ShareCenterController : APISuperController
    {
        private IShareCenterManager domin;
        public ShareCenterController()
        {
            domin = LoadInterface<IShareCenterManager>();
        }

        /// <summary>
        /// 分享中心页面
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult Index()
        {
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            ViewBag.sIDCard = Client.sIDCard;
            ViewBag.sNickName = Client.sNickName;
            return View();
        }


        /// <summary>
        /// 本月新增会员
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult NewlyMember()
        {
            return View();
        }

        /// <summary>
        /// 加载本月新增会员数据列表
        /// </summary>
        [ClientLoginFilter]
        public void NewlyMemberList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            PageInfo Info = LoadParam<PageInfo>();
            result.Data = domin.GetNewlyMember(ClientId, Info);
            result.Succeeded = true;
        }


        #region 结算中心

        /// <summary>
        /// 结算中心
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult AccountCenter()
        {
            return View();
        }

        /// <summary>
        /// 获取结算中心相关信息
        /// </summary>
        [ClientLoginFilter]
        public void GetAccountCenterInfo()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            result.Data = domin.GetAccountCenterInfo(ClientId);
            result.Succeeded = true;
        }

        /// <summary>
        /// 获取分享客相关信息
        /// </summary>
        [ClientLoginFilter]
        public void GetShareInfo()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            var dic= domin.GetShareInfo(ClientId) as Dictionary<string,object> ;
            decimal inter = domin.GetClientMoney(ClientId);//获取可提现金额
            //if (temp != null && temp["predIncome"] != null)
            //{
            //    dic["predIncome"] = temp["predIncome"].ToDecimal();
            //}
            dic["predIncome"] = dic["predIncome"].ToDecimal() + inter;
            result.Data = dic;
            result.Succeeded = true;
        }


        /// <summary>
        /// 提现页面
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult WithdrawCash()
        {
            return View();
        }

        /// <summary>
        /// 提现成功
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult WithdrawCashSuccess()
        {
            return View();
        }

        /// <summary>
        /// 提现
        /// </summary>
        [ClientLoginFilter]
        public void WithdrawCashHandle()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.Value;
            EHECD_WithdrawCashDTO cash = LoadParam<EHECD_WithdrawCashDTO>();
            cash.sWithdrawMemberID = ClientId;
            string Result = string.Empty;
            int res = domin.WithdrawCashHandle(cash,out Result);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Msg = Result;
            }
        }

        /// <summary>
        /// 提现记录
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult CashRecord()
        {
            return View();
        }

        /// <summary>
        /// 加载提现记录数据列表
        /// </summary>
        [ClientLoginFilter]
        public void LoadCashRecordData()
        {
            PageInfo Info = LoadParam<PageInfo>();
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            result.Data = domin.GetCashRecord(ClientId, Info);
            result.Succeeded = true;
        }


        /// <summary>
        /// 收支明细
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult PaymentDetail()
        {
            return View();
        }

        /// <summary>
        /// 加载收支明细数据列表
        /// </summary>
        [ClientLoginFilter]
        public void  LoadPaymentDetailList()
        {
            PageInfo Info = LoadParam<PageInfo>();
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            result.Data = domin.PaymentDetail(ClientId, Info);
            result.Succeeded = true;
        }

        #endregion


        #region 我的会员

        /// <summary>
        /// 我的会员
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult Member()
        {
            return View();
        }

        /// <summary>
        /// 加载我的会员列表数据
        /// </summary>
        [ClientLoginFilter]
        public void LoadMemberList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            int type = LoadParam<Dictionary<string, object>>()["type"].ToInt32();
            PageInfo Info = LoadParam<PageInfo>();
            result.Data=domin.GetMember(ClientId, type, Info);
            result.Succeeded = true;
        }

        #endregion


        #region 我的分享海报

        /// <summary>
        /// 我的分享海报
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult SharePoster()
        {
            ViewBag.Img = domin.GetSharePosterImg();
            ViewBag.ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            //获取域名
            string dominName = config.WebConfig.LoadDynamicJson("weixin").LocalDomain;
            ViewBag.dominName = dominName;
            return View();
        }


        /// <summary>
        /// 扫码成为下级
        /// </summary>
        public ActionResult IntoMemberHandle(Guid ClientId)
        {

            string code = Request.QueryString["code"];
            string appid = config.WebConfig.LoadDynamicJson("weixin").AppId;

            if (string.IsNullOrEmpty(code))
            {
                var path = string.Format("{0}{1}", config.WebConfig.LoadDynamicJson("weixin").LocalDomain, Request.Url.LocalPath+string.Format("?ClientId={0}", ClientId.ToString()));
                // 获取code
                string code_url = string.Format(
                    @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=lk#wechat_redirect",
                    appid,
                    HttpUtility.UrlEncode(path, Encoding.UTF8));
               return   Redirect(code_url);
            }
            else
            {
                string appsecret = config.WebConfig.LoadDynamicJson("weixin").AppSecret;

                // 通过code换取网页授权access_token
                string url = string.Format(
                    @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                    appid, appsecret, code);

                dynamic ret_param = JSONHelper.GetModel(HttpHelper.Post(url, string.Empty));

                string access_token = ret_param.access_token, openid = ret_param.openid;
                //获取微信用户信息
                UserInfo user =UserInfo.Get(openid, access_token);

                EHECD_ClientDTO client = new EHECD_ClientDTO()
                {
                    sNickName = user.nickname,
                    sHeadPic = user.headimgurl,
                    iClientType = 0,
                    sRegCode = ClientId,
                    dBirthday=DateTime.Now,//成为下级的时间
                    iState = 0,
                    sOpenId = user.openid,
                    dAddTime = DateTime.Now,
                    ID = GuidHelper.GetSecuentialGuid()
                };
                //获取域名
                string dominName = config.WebConfig.LoadDynamicJson("weixin").LocalDomain;

                string sNickName = client.sNickName;
                string sPhone = string.Empty;
                int res = domin.IntoMember(client, out sNickName,out sPhone);
                if (res > 0)
                {
                    SendMessage(ClientId, sNickName, sPhone);// 发送模板消息

                    return Redirect(dominName + "/Client/ClientHome");
                }
                else
                {
                    if (res == 0) res = -4;
                   return Redirect(dominName + "/Client/ShareCenter/Failed?tip="+ res + "");
                }
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name=""></param>
        public void SendMessage(Guid ClientId, string sNickName,string sPhone)
        {
            var list = domin.Gethigher(ClientId);
            foreach(var m in list)
            {
                Send(m, sNickName, sPhone);//发送模板消息
            }
        }

        /// <summary>
        /// 发送微信模板消息(发展下线的模板消息)
        /// </summary>
        /// <param name="ClientId">直属上级ID</param>
        public void Send(clientData client, string sNickName,string sPhone)
        {

            try
            {
                string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
                EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
                Access_Token token = new Access_Token(set.sOriginalID, set.sAppId, set.sAppSecret);


                // ====组装基础POST数据====
                var tempJson = new JObject();

                tempJson.Add(new JProperty("touser", client.sOpenId));//发送的openid
                tempJson.Add(new JProperty("template_id", "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w"));
                var time = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
                // data 数据部分
                var tempJsonChild = new JObject();
                tempJsonChild.Add(new JProperty("first", new JObject(new JProperty("value", "有新的分享客进入友客了！"))));
                tempJsonChild.Add(new JProperty("keyword1", new JObject(new JProperty("value", sNickName))));
                tempJsonChild.Add(new JProperty("keyword2", new JObject(new JProperty("value", client.order + "级"))));
                tempJsonChild.Add(new JProperty("keyword3", new JObject(new JProperty("value", sPhone))));
                tempJsonChild.Add(new JProperty("keyword4", new JObject(new JProperty("value", time))));
                tempJsonChild.Add(new JProperty("remark", new JObject(new JProperty("value", ""))));

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




        /// <summary>
        /// 海报扫码成为下级失败页面
        /// </summary>
        public ActionResult Failed()
        {
            return View();
        }



        public ActionResult FailedTwo()
        {
            return View();
        }
        #endregion


        #region 分享店铺管理

        /// <summary>
        /// 分享店铺管理
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult ShopManagement()
        {
            return View();
        }

        /// <summary>
        /// 记载分享店铺数据列表
        /// </summary>
        [ClientLoginFilter]
        public void LoadShopManagementList()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            PageInfo Info = LoadParam<PageInfo>();
            var dic = LoadParam<Dictionary<string, object>>();
            int bIsDeleted = dic["bIsDeleted"].ToInt16();
            string sKeyWord = dic["sKeyWord"].ToString().Trim();
            result.Data=domin.GetShopManagement(ClientId, Info, bIsDeleted, sKeyWord);
            result.Succeeded = true;
        }

        /// <summary>
        /// 分享店铺
        /// </summary>
        [ClientLoginFilter]
        public void ShareShop()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            var model = LoadParam<EHECD_SharedClientInfoDTO>();
            model.sClientID = ClientId;
            int res = domin.ShareShop(model);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            if (res == -1)
            {
                result.Data = -1;
                result.Msg = "申请失败，您已被此店冻结";
            }
        }

        /// <summary>
        /// 取消店铺的分享
        /// </summary>
        [ClientLoginFilter]
        public void CancelShare()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            Guid sShopID=LoadParam<Dictionary<string, object>>()["sShopID"].ToGuid();
            if (domin.CancelShare(ClientId,sShopID)>0)
            {
                result.Succeeded = true;
            }
        }

        /// <summary>
        /// 店铺详情
        /// </summary>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult ShopDetail(int IsShared)
        {
            ViewBag.IsShared = IsShared;
            return View();
        }

        /// <summary>
        /// 加载店铺商品列表
        /// </summary>
        [ClientLoginFilter]
        public void LoadShopGoodsList()
        {
            var dic = LoadParam<Dictionary<string, object>>();
            string sGoodsName = dic["sGoodsName"].ToString().Trim();
            Guid sShopId = dic["sShopId"].ToGuid();
            PageInfo info = LoadParam<PageInfo>();
            result.Data = domin.GetShopGoodsList(sShopId, info, sGoodsName);
            result.Succeeded = true;
        }


        /// <summary>
        /// 分享商品
        /// </summary>
        [ClientLoginFilter]
        public void ShareGoods()
        {
            var ClientId = ((EHECD_ClientDTO)GetSessionInfo("_client")).ID.ToGuid();
            var param = LoadParam<Dictionary<string, object>>();
            var sGoodsId = param["sGoodsId"].ToGuid();
            int res = domin.ShareGoods(ClientId, sGoodsId);
            if (res > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                if (res == -1)
                {
                    result.Data = -1; //"你不是该店铺的分享客,分享商品将不会发展下线";
                }
            }
        }

        #endregion


        #region 分享客须知


        /// <summary>
        /// 分享客须知
        /// </summary>
        /// <param name="sTitle"></param>
        /// <returns></returns>
        [ClientLoginFilter]
        public ActionResult Protocol(string sTitle)
        {
            var domin = LoadInterface<IClientCenterManager>();
            var model = domin.GetProtocolByTitle(sTitle);
            return View(model);
        }

        #endregion
    }
}