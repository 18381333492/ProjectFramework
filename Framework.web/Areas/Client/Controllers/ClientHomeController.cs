using Framework.BLL;
using Framework.Dapper;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.DTO;
using Framework.web.Models;
using WeiXin.Base.JsApi;
using WeiXin.Base.User.UserInfo;
using WeiXin.Base;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.IO;
using Framework.Helper;
using WeiXin.Tool;
using Framework.SystemLog;
using Newtonsoft.Json;

namespace Framework.web.Areas.Client.Controllers
{
    /// <summary>
    /// 客户端首页
    /// </summary>
    public class ClientHomeController : APISuperController
    {
        #region 视图
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            // 获取并判断 当前进入用户是否是通过分享客分享链接进入
            var clientID = Request.QueryString["sUserID"];

            // wenxin_user_openid
            string code = Request.QueryString["code"];
            string appid = config.WebConfig.LoadDynamicJson("weixin").AppId;

            // 初始化微信配置信息参数
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);

            if (string.IsNullOrEmpty(code))
            {

                //      ============ Action:微信授权 ===========

                var path = string.Format("{0}{1}", config.WebConfig.LoadDynamicJson("weixin").LocalDomain, Request.Url.LocalPath);

                //     回调地址
                // var clientIDStr = string.IsNullOrEmpty(clientID) ? "" : "&sUserID=" + clientID;
                string redirect_url = path + string.Format("?sUserID={0}", clientID);

                //获取code
                string code_url = string.Format(
                    @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=lk#wechat_redirect",
                    appid,
                    HttpUtility.UrlEncode(redirect_url, Encoding.UTF8));
                return Redirect(code_url);
            }
            else
            {
                UserInfo user = null;
                if (GetSessionInfo("wenxin_user_openid") == null)
                {
                    string appsecret = config.WebConfig.LoadDynamicJson("weixin").AppSecret;

                    // 通过code换取网页授权access_token
                    string url = string.Format(
                        @"https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                        appid, appsecret, code);
                    dynamic ret_param = JSONHelper.GetModel(HttpHelper.Post(url, string.Empty));

                    //          ==================【网页授权的access_token】可以进行授权后接口调用，获取用户基本信息接口 =============
                    string access_token = ret_param.access_token, openid = ret_param.openid;
                    //         获取用户基本信息
                    user = UserInfo.Get(openid, access_token);
                    if (user != null)
                        SetSessionInfo("wenxin_user_openid", user);
                }
                else
                {

                    user = (GetSessionInfo("wenxin_user_openid") as UserInfo);

                }
                //     通过分享客分享链接进入
                if (clientID != null && !string.IsNullOrEmpty(clientID.ToString()))
                {
                    //获取 【分享此链接的分销客】的基本信息
                    var client = LoadInterface<IShopDetailManager>().GetClient(clientID.ToString());

                    // 在数据表中查询该微信登录的账号是否已经是会员 给该用户创建新的会员信息【无账户密码】
                    var ret = LoadInterface<IShopDetailManager>().SearchOpernID(user.openid);
                    if (ret == null)
                    {
                        //如果不是会员
                        EHECD_ClientDTO dto = new EHECD_ClientDTO();
                        dto.sOpenId = user.openid;
                        dto.sNickName = user.nickname;
                        dto.iSex = Convert.ToInt32(user.sex);
                        dto.sHeadPic = user.headimgurl;
                        dto.sRegCode = client.ID;
                        var let = LoadInterface<IShopDetailManager>().CreateNewClient(dto);

                        //             给上级分销客发消息
                        SendMessageToSuper(set, dto, client);
                    }
                    else
                    {

                        //============== 如果数据库中存在当前用户，判断是否有上级【一个上级能有多个下级，但是一个下级只能有一个上级】===========

                        //判断当前用户是否有上级
                        bool flag = LoadInterface<IShopDetailManager>().IsHaveSuper(ret.ID.ToString());

                        //如果没有上级
                        if (!flag)
                        {
                            //判断当前用户是否是通过自己的链接进来【A不是通过自己的链接进来的】
                            if (new Guid(clientID) != ret.ID)
                            {
                                //如果不是通过自己的链接进来【自己不能成为自己的下级】

                                if (client.sRegCode == null)
                                {
                                    //没有上级
                                    //更新信息
                                    var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                    //发送消息
                                    if (isSuccess) SendMessageToSuper(set, ret, client);
                                }
                                else
                                {
                                    //如果clientID有上级
                                    if (client.sRegCode != ret.ID)//【A的上级不是B】
                                    {
                                        //如果进入的不是我的上级【上级与下级不能循环】，如果是已经是clientID的上级，则不能成为他的下级
                                        var lower = LoadInterface<IShopDetailManager>().AllLower(ret.ID.ToString());//查找当前点进来的用户的下级
                                        if (lower != null)
                                        {//【B有下级】
                                            bool havaLower = false;
                                            for (int i = 0; i < lower.Count(); i++)
                                            {
                                                if (lower[i].ID == client.sRegCode)
                                                {
                                                    havaLower = true;
                                                    break;
                                                }
                                            }
                                            if (havaLower == false)
                                            {//【B的下级不是A的上级】
                                                bool havaThree = false;
                                                //如果当前用户的下级不是clientID的上级，成为下级【下级也不能是clientID的上级】
                                                for (int i = 0; i < lower.Count(); i++)
                                                {
                                                    var fourth = LoadInterface<IShopDetailManager>().AllLower(lower[i].ID.ToString());
                                                    if (fourth != null)
                                                    {
                                                        for (int j = 0; i < fourth.Count(); j++)
                                                        {
                                                            if (client.sRegCode == fourth[j].ID)
                                                            {
                                                                havaThree = true;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                                        //发送消息
                                                        if (isSuccess) SendMessageToSuper(set, ret, client);
                                                    }
                                                }
                                                if (havaThree == false)
                                                {
                                                    //【B的下级的下级不是A的上级】
                                                    var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                                    //发送消息
                                                    if (isSuccess) SendMessageToSuper(set, ret, client);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //如果当前用户没有下级就成为当前clientID的下级
                                            var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                            //发送消息
                                            if (isSuccess) SendMessageToSuper(set, ret, client);
                                        }

                                    }
                                }

                            }


                        }

                    }
                }

            }

            //获取微信JSApi配置信息
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
        }

        /// <summary>
        /// 切换定位
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeLocalAddressView()
        {
            return View();
        }

        /// <summary>
        /// 领取优惠劵
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCouponView()
        {
            return View();
        }

        /// <summary>
        /// 首页展示页详情
        /// </summary>
        /// <returns></returns>
        public ActionResult GetHomeShowBannerDetailView()
        {
            ViewBag.ID = Request.QueryString["ID"];
            return View();
        }

        /// <summary>
        /// 通过搜索框搜索结果页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSelectView()
        {
            ViewBag.Type = Request["id"];//指定初始化的搜索结果类型
            ViewBag.sGoodsName = Request["sGoodsName"];//搜索的商品名称
            return View();
        }

        /// <summary>
        /// 点击首页【民宿】【票务】【周边】【附近】图标进入的页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOtherView()
        {
            ViewBag.Type = Request["id"];//指定初始化的搜索结果类型
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            //WxKeyId
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
        }

        /// <summary>
        /// 获取秒杀专区页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSeckillView()
        {
            return View();
        }

        /// <summary>
        /// 获取特卖专区页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSpecailView()
        {
            return View();
        }

        #endregion

        #region 操作

        /// <summary>
        /// 获取首页数据
        /// </summary>
        public void GetClientHomeData()
        {
            var param = LoadParam<Dictionary<string, object>>();
            IClientHomeManager manager = DI.DIEntity.GetInstance().GetImpl<IClientHomeManager>();
            var data = manager.QueryClietHomeData(param);
            bool flag = data == null;
            result.Data = data;
            result.Succeeded = !flag;
        }

        /// <summary>
        /// 切换定位
        /// </summary>
        public void ChangeLocalAddress()
        {
            var param = LoadParam<Dictionary<string, object>>();
            var data = LoadInterface<IClientHomeManager>().QuerySelectCity(param);
            bool flag = data == null;
            result.Data = data;
            result.Succeeded = !flag;

        }

        /// <summary>
        /// 领取优惠劵(列表显示)
        /// </summary>
        public void GetCouponList()
        {
            var param = LoadParam<PageInfo>();//分页参数

            var dic = new Dictionary<string, object>();
            var userID = GetSessionInfo("_client");//获取用户信息
            if (userID == null)
            {
                dic["sUserID"] = "";
            }
            else
            {
                dic["sUserID"] = (userID as EHECD_ClientDTO).ID;
            }
            //var dic = new Dictionary<string, object>() { { "sUserID", "1BF7FBD5-4030-4AE6-96C5-86D5C3CF0249" } };//模拟数据
            var data = LoadInterface<IClientHomeManager>().QueryCouponList(param, dic);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 领取优惠劵（操作）
        /// </summary>
        [ClientLoginFilter]
        public void GetCoupon()
        {
            var param = LoadParam<Dictionary<string, object>>();
            param["sUserID"] = (GetSessionInfo("_client") as EHECD_ClientDTO).ID;
            param["sUserName"] = (GetSessionInfo("_client") as EHECD_ClientDTO).sLoginName;
            var res = LoadInterface<IClientHomeManager>().ExcuteGetCoupon(param);//领取优惠劵
            bool flag = res > 0;
            result.Msg = flag ? "领取成功" : "领取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取首页展示详情
        /// </summary>
        public void GetHomeShowBannerDetail()
        {
            var param = LoadParam<Dictionary<string, object>>();
            var data = LoadInterface<IClientHomeManager>().QueryShowBannerDetail(param);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取名宿搜索结果
        /// </summary>
        public void GetStoreSelectResult()
        {
            var param = LoadParam<Dictionary<string, object>>();
            var pageInfo = new PageInfo()
            {
                PageIndex = Helper.ConvertHelper.ToInt32(param["pageIndex"]),
                PageSize = Helper.ConvertHelper.ToInt32(param["pageSize"]),
            };
            var data = LoadInterface<IClientHomeManager>().QueryByStore(pageInfo, param);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取票务搜索结果
        /// </summary>
        public void GetTiketSelectResult()
        {
            try
            {
                var param = LoadParam<Dictionary<string, object>>();
                var pageInfo = new PageInfo()
                {
                    PageIndex = Helper.ConvertHelper.ToInt32(param["pageIndex"]),
                    PageSize = Helper.ConvertHelper.ToInt32(param["pageSize"]),
                };
                var data = LoadInterface<IClientHomeManager>().QueryByTiket(pageInfo, param);
                bool flag = data != null;
                result.Data = data;
                result.Msg = flag ? "获取成功" : "获取失败";
                result.Succeeded = flag;

            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
            }
           
        }

        /// <summary>
        /// 获取周边搜索结果
        /// </summary>
        public void GetAroundSelectResult()
        {
            try
            {
                var param = LoadParam<Dictionary<string, object>>();
                var pageInfo = new PageInfo()
                {
                    PageIndex = Helper.ConvertHelper.ToInt32(param["pageIndex"]),
                    PageSize = Helper.ConvertHelper.ToInt32(param["pageSize"]),
                };
                var ttt =JsonConvert.SerializeObject(pageInfo) +JsonConvert.SerializeObject(param);
                var data = LoadInterface<IClientHomeManager>().QueryByAround(pageInfo, param);
                bool flag = data != null;
                result.Data = data;
                result.Msg = flag ? "获取成功" : "获取失败";
                result.Succeeded = flag;

            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 秒杀专区
        /// </summary>
        public void GetSecKillList()
        {
            var param = LoadParam<PageInfo>();
            var data = LoadInterface<IClientHomeManager>().QuerySecKillList(param);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 特卖专区
        /// </summary>
        public void GetSpecailSaleList()
        {
            var param = LoadParam<PageInfo>();
            var data = LoadInterface<IClientHomeManager>().QuerySpecialSaleList(param);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取当前定位城市下面的区（这些区必须有有客商城的店铺）
        /// </summary>
        public void GetCountryByCityList()
        {
            var param = LoadParam<Dictionary<string, object>>();
            var data = LoadInterface<IClientHomeManager>().QueryCountryByCity(param);
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 获取所有房型
        /// </summary>
        public void GetRoomTypeList()
        {
            var data = LoadInterface<IClientHomeManager>().QueryRoomTypeList();
            bool flag = data != null;
            result.Data = data;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 根据搜索框的输入实时查询商品列表
        /// </summary>
        public void QueryGoodsByWhere()
        {
            var param = LoadParam<Dictionary<string, object>>();
            var res = LoadInterface<IClientHomeManager>().QueryGoodsByWhere(param);
            bool flag = res != null;
            result.Data = res;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 判断用户是否是分销客
        /// </summary>
        public void IsShareClient()
        {
            // 获取用户登录ID
            var sOpenId = (GetSessionInfo("wenxin_user_openid") as UserInfo).openid;
            var res = LoadInterface<IClientHomeManager>().IsShareClient(sOpenId);
            bool flag = res != null;
            result.Msg = flag ? "成功" : "失败";
            result.Data = res;
            result.Succeeded = flag;
        }

        #endregion

        /// <summary>
        /// 给上级发消息,通知有人成为下线
        /// </summary>
        /// <param name="set">微信基础设置</param>
        /// <param name="dto">当前用户</param>
        /// <param name="client">发出链接的分销客</param>
        private void SendMessageToSuper(EHECD_WeChatSetDTO set, EHECD_ClientDTO dto, EHECD_ClientDTO client)
        {
            // ======================【基础支持的access_token】调用 发送模板消息的接口===============================
            Access_Token token = new Access_Token(set.sOriginalID, set.sAppId, set.sAppSecret);

            // 如果该分销客有上级
            if (client.sRegCode != null && !string.IsNullOrEmpty(client.sRegCode.ToString()))
            {

                // 发送给上级分销客
                Send(token.sToken, client.sOpenId, 1, dto);

                // 获取上上级的openid
                var upUpShare = LoadInterface<IShopDetailManager>().GetClient(client.sRegCode.ToString());

                // 发送给上上级分销客
                if (upUpShare != null)
                {
                    Send(token.sToken, upUpShare.sOpenId, 2, dto);
                    if (upUpShare.sRegCode != null)
                    {
                        var FristShare = LoadInterface<IShopDetailManager>().GetClient(upUpShare.sRegCode.ToString());//查出第三级的信息
                        if (FristShare != null) Send(token.sToken, FristShare.sOpenId, 3, dto);//发送给三级
                    }
                }

            }
            else
            {
                // 发送给上级分销客【当前分销客】
                Send(token.sToken, client.sOpenId, 1, dto);
            }
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="id"></param>
        /// <param name="clientStage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public string Send(string access_token, string id, int clientStage, EHECD_ClientDTO client)
        {
            dynamic postData = new ExpandoObject();
            postData.touser = id;
            postData.template_id = "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w";
            postData.url = string.Empty;
            postData.topcolor = "#ffffff";
            postData.data = new ExpandoObject();

            // ====组装基础POST数据====
            var tempJson = new JObject();

            tempJson.Add(new JProperty("touser", id));
            tempJson.Add(new JProperty("template_id", "uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w"));
            var time = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            // data 数据部分
            var tempJsonChild = new JObject();
            tempJsonChild.Add(new JProperty("first", new JObject(new JProperty("value", "有新的分享客进入友客了！"))));
            tempJsonChild.Add(new JProperty("keyword1", new JObject(new JProperty("value", client.sNickName))));
            tempJsonChild.Add(new JProperty("keyword2", new JObject(new JProperty("value", clientStage + "级"))));
            tempJsonChild.Add(new JProperty("keyword3", new JObject(new JProperty("value", client.sPhone))));
            tempJsonChild.Add(new JProperty("keyword4", new JObject(new JProperty("value", time))));
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