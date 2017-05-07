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
    public class ShopHomeController : APISuperController
    {
        // GET: Client/ShopHome
        #region 店铺首页
        public ActionResult Index()
        {

            // 获取从其他页面传递过来的ID
            var shopID = Request["ID"].ToString();

            ViewBag.shopID = shopID;

            // 获取并判断 当前进入用户是否是通过分享客分享链接进入
            var clientID = Request.QueryString["sUserID"]; //分享客的主键ID

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
                var clientIDStr = string.IsNullOrEmpty(clientID) ? "" : "&sUserID=" + clientID;
                string redirect_url = path + string.Format("?ID={0}{1}", shopID, clientIDStr);

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
                    var ret = LoadInterface<IShopDetailManager>().SearchOpernID(user.openid);//当前会员信息
                    if (ret == null)
                    {
                        //如果不是会员
                        EHECD_ClientDTO dto = new EHECD_ClientDTO();
                        dto.sOpenId = user.openid;
                        dto.sNickName = user.nickname;
                        dto.iSex = Convert.ToInt32(user.sex);
                        dto.sHeadPic = user.headimgurl;
                        dto.sRegCode = client.ID;
                        dto.sIDType = (-1).ToString();//一级会员标识,不能被发展为下线  1-一级会员 -1-非一级会员
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

                                    if (ret.sIDType != "1")//不是一级会员才能成为下线
                                    {
                                        //更新信息
                                        var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                        //发送消息
                                        if (isSuccess) SendMessageToSuper(set, ret, client);
                                    }
                                }
                                else
                                {
                                    //如果clientID有上级
                                    if (ret.sIDType != "1")
                                    {//非一级才能发展下线
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
                                                {///【B的下级不是A的上级】
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

            }

            //获取微信JSApi配置信息
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
            //return View();
           
        }
        /// <summary>
        /// 店铺详情页
        /// </summary>
        public void Shop() {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            IShopDetailManager manager = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>();
            //var dic = new Dictionary<string, object>() { { "sUserID", (GetSessionInfo("_client") as EHECD_ClientDTO).ID } };
            //string clientID = new Guid(dic["sUserID"].ToString());
            var clientID = "";
            if ((EHECD_ClientDTO)GetSessionInfo("_client") == null)
            {
                clientID = null;
            }
            else {
                var client = (EHECD_ClientDTO)GetSessionInfo("_client");
                clientID= client.ID.ToString();
            }
            var shop = manager.ShopHome(data["ID"].ToString(), clientID);

            if (shop != null) {
                result.Data = shop;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "读取失败";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        //周边列表绑定
        public void AroundHome()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().AroundHome(page, data);
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

        //门票列表绑定

        public void TicketList()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().TicketHome(page, data);
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
        /// 收藏
        /// </summary>
        [ClientLoginFilter]
        public void CollectionIn()
        {
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var clientID = Client.ID;
            var isOrNot = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().IsCollect(dic["iGoodsID"].ToString(), clientID.ToString());
            if (isOrNot == null)
            {
               
                EHECD_CollectDTO dto =LoadParam<EHECD_CollectDTO>();
                dto.iClientID =clientID;
                
                dto.bIsCollect = true;
                var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().CollectionIn(dto);
                result.Succeeded = list > 0;
                result.Msg = result.Succeeded ? "" : "添加收藏失败，请联系系统管理员";
            }
            else {
                var deleteCollect = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().CancelCollect(dic["iGoodsID"].ToString(), clientID.ToString());
                result.Succeeded = deleteCollect > 0;
                result.Msg = result.Succeeded ? "" : "取消收藏失败，请联系系统管理员";
            }
        }

        /// <summary>
        /// 住宿页面绑定
        /// </summary>
        public void GetShopHome()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().GetShopHome(data, page);
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

        [ClientLoginFilter]
        public void GetShopCoupon()
        {
            var client = (EHECD_ClientDTO)GetSessionInfo("_client");
            EHECD_CouponDetailsDTO dto = LoadParam<EHECD_CouponDetailsDTO>();
            dto.sUserName = client.sLoginName;
            dto.sUserID = client.ID;
            var ret = LoadInterface<IShopDetailManager>().GetShopCoupon(dto);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "领取优惠券失败，请联系系统管理员";
            }
            //result.Succeeded = ret > 0;
            //result.Msg = result.Succeeded ? "" : "领取优惠券失败，请联系系统管理员";
        }
        
        #endregion

        #region 店主故事
        public ActionResult HostStory()
        {
            return View();
        }
        /// <summary>
        /// 获取店主故事
        /// </summary>
        public void GetStore()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            IShopDetailManager manager = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>();
            EHECD_ShopSetDTO story = manager.HosterStory(data["ID"].ToString());
            if (story != null)
            {
                result.Data = story;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "读取失败";
            }

        }

        #endregion

        #region 游记列表
        public ActionResult TravelList()
        {
            return View();
        }

        public void GetTravel()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo> (RequestParameters.dataStr);
           var list =DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().GetAllTraveNote(page, data["ID"].ToString());
            if (list != null)
            {
                result.Data = list;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }

        }
        #endregion

        #region 写游记
        [ClientLoginFilter]
        public ActionResult WriteTravel()
        {
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            //WxKeyId
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
        }

        [ClientLoginFilter]
        public void WriteTravelNote()
        {
            
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            EHECD_TravelsNotesDTO dto = LoadParam<EHECD_TravelsNotesDTO>();
            dto.sHeadImges = Client.sHeadPic;
            dto.sAuthor = Client.sNickName;
            
            var ret = LoadInterface<IShopDetailManager>().WriteTravel(dto,dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加游记失败，请联系系统管理员";
        }
        #endregion

        #region 游记详情
        public ActionResult TravelDetail()
        {
            return View();
        }

        public void GetTravelByID()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var list = LoadInterface<IShopDetailManager>().GetTravelByID(dic["ID"].ToString());
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

        #region 商品详情
        public ActionResult RoomDetail()
        {
            // 获取从其他页面传递过来的ID
            var sGoodsID = Request["ID"].ToString();

            ViewBag.sGoodsID = sGoodsID;

            //  获取并判断 当前进入用户是否是通过分享客分享链接进入
            var clientID = Request.QueryString["sUserID"];

            //wenxin_user_openid
            string code = Request.QueryString["code"];
            string appid = config.WebConfig.LoadDynamicJson("weixin").AppId;

            // 初始化微信配置信息参数
            string WxKeyId = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            EHECD_WeChatSetDTO set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSet(WxKeyId);

            if (string.IsNullOrEmpty(code))
            {

                // ============Action:微信授权===========

                var path = string.Format("{0}{1}", config.WebConfig.LoadDynamicJson("weixin").LocalDomain, Request.Url.LocalPath);

                // 回调地址
                var clientIDStr = string.IsNullOrEmpty(clientID) ? "" : "&sUserID=" + clientID;
                string redirect_url = path + string.Format("?ID={0}{1}", sGoodsID, clientIDStr);

                // 获取code
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

                    // ==================【网页授权的access_token】可以进行授权后接口调用，获取用户基本信息接口=============
                    string access_token = ret_param.access_token, openid = ret_param.openid;
                    // 获取用户基本信息
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
                        dto.sIDType = (-1).ToString();//一级会员标识,不能被发展为下线  1-一级会员 -1-非一级会员
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
                                    if (ret.sIDType != "1")//非一级会员才能发展
                                    {
                                        //更新信息
                                        var isSuccess = LoadInterface<IShopDetailManager>().UpdateClientSuper(ret.ID.ToString(), client.ID.ToString());

                                        //发送消息
                                        if (isSuccess) SendMessageToSuper(set, ret, client);
                                    }
                                }
                                else
                                {
                                    if (ret.sIDType != "1")//非一级会员才能发展
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

            }

            // 获取微信JSApi配置信息
            jsapi_config item = new jsapi_config(set.sOriginalID, set.sAppId, set.sAppSecret, Request.Url.ToString());
            return View(item);
            //return View();
        }

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
                    if (upUpShare.sRegCode != null) {
                        var FristShare = LoadInterface<IShopDetailManager>().GetClient(upUpShare.sRegCode.ToString());//查出第三级的信息
                        if(FristShare!=null) Send(token.sToken, FristShare.sOpenId, 3, dto);//发送给三级
                    }
                }
            }
            else
            {

                // 发送给上级分销客【当前分销客】
                Send(token.sToken, client.sOpenId, 1, dto);
            }
        }

        public void HomeDetail()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var clientID = "";
            if (Client == null)
            {
                clientID = null;
            }
            else {
                clientID = Client.ID.ToString(); ;
            }
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().HomeDetail(data["ID"].ToString(), clientID);
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

        /// <summary>
        /// 检查当前分享的人员
        /// 1.【是否已经注册】bIsRegisted  0 否 1 是
        /// 2.【用户类型】    iClientType  0 普通客户 1 分销客
        /// </summary>
        public void CheckUser()
        {
            var param = LoadParam<Dictionary<string, object>>();
            // 获取用户登录ID
            var sOpenId= (GetSessionInfo("wenxin_user_openid") as UserInfo).openid;
            param["sOpenId"] = sOpenId;
            var res = LoadInterface<IShopDetailManager>().CheckUser(param);
            bool flag = res != null;
            result.Msg = flag ? "成功":"失败";
            result.Data = res;
            result.Succeeded = flag;
        }

        #region 门票详情
        public ActionResult TicketDetail()
        {
            return View();
        }
        #endregion


       

        #region 评论列表
        public ActionResult CommentDetail()
        {
            return View();
        }
        /// <summary>
        /// 获取所有评论
        /// </summary>
        public void GetAllComment()
        {
            var data = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().GetAllComment(page, data["ID"].ToString());
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
        /// 获取商品的评分
        /// </summary>
        public void GetCommentScore()
        {
            Dictionary<string, object> dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var list = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>().GetCommentScore(dic["ID"].ToString());
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

        /// <summary>
        /// 发送微信模板消息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="id"></param>
        /// <param name="clientStage"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public string Send(string access_token,string id,int clientStage,EHECD_ClientDTO client)
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
            tempJsonChild.Add(new JProperty("keyword2", new JObject(new JProperty("value", clientStage+"级"))));
            tempJsonChild.Add(new JProperty("keyword3", new JObject(new JProperty("value", client.sPhone))));
            tempJsonChild.Add(new JProperty("keyword4", new JObject(new JProperty("value", time))));
            tempJsonChild.Add(new JProperty("remark", new JObject(new JProperty("value",""))));

            tempJson.Add(new JProperty("data",tempJsonChild));

            // ======组装数据完成======

            //string temps = "{\"touser\": \"" + id + "\"," +
            //            "\"template_id\": \"uQ7BxHjKZ-Vcmg5Zcy0PiRWdPVCA8Es5_ofh5xYMU0w\", " +
            //            "\"topcolor\": \"#FFFFFF\", " +
            //            "\"data\": " +
            //            "{\"first\": {\"value\": \"有新的分享客进入友客了！\"}," +
            //            "\"keyword1\": { \"value\": \'"+client.sNickName+"\'}," +
            //            "\"keyword2\": { \"value\": \'"+ clientStage+ "\'}," +
            //            "\"keyword3\": { \"value\": \'"+client.sPhone+"\'}," +
            //            "\"keyword4\": { \"value\": \'" + DateTime.Now + "\'}," +
            //            "\"remark\": {\"value\": \"\" }}}";

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
        /// <summary>
        /// 插入分销商品
        /// </summary>
        public void InsertShareGoods() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopDetailManager>().InsertShareGoods(dic);
            result.Data = ret;
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "";
        }

        /// <summary>
        /// 是否是本店的分享客
        /// </summary>
        public void IsShareBelong() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            if (Client == null) {
                dic["sclient"] = "";
            }
            else
            {
                dic["sclient"] = Client.ID.ToString();
            };
            var ret = LoadInterface<IShopDetailManager>().IsShareBelong(dic);
            result.Data = ret;
            bool flag = ret != null;
            result.Succeeded = flag;
            result.Msg = result.Succeeded ? "" : "";
        }
        /// <summary>
        /// 判断用户是否被冻结
        /// </summary>
        [ClientLoginFilter]
        public void IsFrozen() {
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var ret = LoadInterface<IShopDetailManager>().IsFrozen(Client.ID.ToString());
            result.Data = ret;
            result.Succeeded = ret != null;
            result.Msg = result.Succeeded ? "" : "获取失败，联系管理员";
        }


        public void IsShare()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopDetailManager>().IsShare(dic);
            result.Data = ret;
            result.Succeeded = ret>0;
            result.Msg = result.Succeeded ? "" : "失败，联系管理员";
        }
    }
}