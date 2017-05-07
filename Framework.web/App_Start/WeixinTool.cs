using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using WeiXin.Base;
using WeiXin.Base.Menu;

namespace Framework.web.App_Start
{
    using Framework.BLL;
    using Helper;
    using WeiXin.Base.Send.PassiveResponse.Message;

    public class WeiXinTool
    {
        #region 微信菜单


        /// <summary>
        /// 设置微信菜单
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        internal static bool buildMenu(IList<EHECD_WeChartMenuDTO> menuList, EHECD_SystemUserDTO user)
        {
            Guid? parentID = null;

            // 这里引用WeiXin.Base.Menu;
            var menu = new Menu();

            foreach (var item in menuList)
            {
                parentID = item.sSubmenuID;
                if (parentID == null)
                {
                    // 当前 sSubmenuID 的所有子菜单
                    IList<EHECD_WeChartMenuDTO> clildlist = menuList.Where(o => o.sSubmenuID == item.ID).ToList<EHECD_WeChartMenuDTO>();

                    //有子菜单
                    if (clildlist != null && clildlist.Count > 0)
                    {

                        IList<MenuInfo> Rclildlist = new List<MenuInfo>();
                        // 创建子菜单集合
                        for (int i = 0; i < clildlist.Count; i++)
                        {
                            MenuInfo menuinfo = clildlist[i].iTouchType == 1 ? MenuInfo.View(clildlist[i].sMenuName, clildlist[i].sKeyword) : MenuInfo.Click(clildlist[i].sMenuName, clildlist[i].sKeyword);
                            Rclildlist.Add(menuinfo);
                        }

                        ChildMenu childmenu = new ChildMenu();
                        childmenu.name = item.sMenuName;
                        childmenu.sub_button.AddRange(Rclildlist);
                        menu.button.Add(childmenu);
                    }
                    else
                    {
                        //触发类型(0-关键词，1-链接)
                        MenuInfo menuinfo = item.iTouchType == 1 ? MenuInfo.View(item.sMenuName, item.sKeyword) : MenuInfo.Click(item.sMenuName, item.sKeyword);
                        menu.button.Add(menuinfo);
                    }
                }
            }
            //获取微信配置
            var set = DI.DIEntity.GetInstance().GetImpl<IWechartManager>().GetSetByShopID(user);
            if (set != null)
            {
                //创建菜单
                bool bFlag = false;

                var accessToken = new Access_Token(set.sOriginalID, set.sAppId, set.sAppSecret);
                string sAccessToken = accessToken.sToken;
                bFlag = new HandleMenu().Create(sAccessToken, menu);
                return bFlag;
            }
            else
            {
                return false;
            }
        }

        #endregion


        /// <summary>
        /// 获取关注自动回复内容
        /// </summary>
        /// <param name="fromUserName">用户openid</param>
        /// <param name="toUserName">微信公众号原始id</param>
        /// <param name="eventKey">如果是二维码扫码关注的这里是二维码内容</param>
        /// <returns>生成的回复结果</returns>
        internal static string BuildAttentionAutoReply(string fromUserName, string toUserName, string eventKey)
        {
            var bll = Framework.DI.
                            DIEntity.
                            GetInstance().
                            GetImpl<IWechartManager>(); // 处理业务类

            var retStr = string.Empty;                  // 返回的xml字符串

            // 通过公众号原始ID去获取对应的公众号的关注回复
            var ret = bll.LoadAttentionAutoReply(
                                    fromUserName,
                                    toUserName,
                                    eventKey);          // 获取的回复内容

            if (null != ret)
            {
                var state = (EHECD_WechartReplyTypeDTO)ret["set"];
                var rets = (List<EHECD_WeCharReplyDTO>)ret["ret"];

                if (rets.Count > 0)
                {
                    retStr = state.sContentType.ToBoolean() ?
                                BuildPassiveNewsMessage(fromUserName, toUserName, rets) :            // 图文混合
                                BuildPassiveTextMessage(fromUserName, toUserName, rets[0].sContent);// 文本
                }
            }

            return retStr;
        }

        /// <summary>
        /// 获取商城设置
        /// </summary>
        ///// <returns></returns>
        public static EHECD_WeChatSetDTO GetSettings()
        {
            var bll = Framework.DI.DIEntity.GetInstance().GetImpl<IWechartManager>();
            string id = web.config.WebConfig.LoadDynamicJson("weixin").WxKeyId;
            return bll.GetSet(id);
        }

        /// <summary>
        /// 得到开发者验证 Token
        /// </summary>
        /// <returns></returns>
        internal static string GetToken()
        {
            var settings = GetSettings();
            return settings.sToken;
        }

        /// <summary>
        /// 得到微信开发者信息
        /// </summary>
        /// <param name="sWeiXinAccount"></param>
        /// <param name="sAppId"></param>
        /// <param name="sAppSecret"></param>
        internal static void GetAppIDAppSecret(out string sWeiXinAccount, out string sAppId, out string sAppSecret)
        {
            sWeiXinAccount = sAppId = sAppSecret = string.Empty;
            var settings = GetSettings();
            sWeiXinAccount = settings.sOriginalID;
            sAppId = settings.sAppId;
            sAppSecret = settings.sAppSecret;
        }

        /// <summary>
        /// 重构被动响应图文消息
        /// <para>如果图文消息条数超出8条，则发送不成功</para>
        /// </summary>
        /// <param name="sResult"></param>
        /// <param name="sFromUserName"></param>
        /// <param name="sToUserName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        internal static string BuildPassiveNewsMessage(string sFromUserName, string sToUserName, List<EHECD_WeCharReplyDTO> list)
        {
            string sMessage = string.Empty;

            int count = 0;
            NewsMessage.Article[] articles = new NewsMessage.Article[list.Count];
            NewsMessage.Article article = null;
            NewsMessage.Article.Item article_item = null;
            foreach (var articleItem in list)
            {
                article_item = new NewsMessage.Article.Item();
                article = new NewsMessage.Article();
                article_item.Title = articleItem.sTitle;
                article_item.Url = articleItem.sShopUrl;
                article_item.PicUrl = articleItem.sPictureUrl;
                article_item.Description = articleItem.sContent;
                article.item = article_item;
                articles[count++] = article;
            }
            NewsMessage _message = new NewsMessage()
            {
                ArticleCount = count,
                FromUserName = sToUserName,
                ToUserName = sFromUserName,
                Articles = articles
            };
            return _message.ToString();
        }

        /// <summary>
        /// 重构被动响应文本消息
        /// </summary>
        /// <param name="sResult"></param>
        /// <param name="sFromUserName"></param>
        /// <param name="sToUserName"></param>
        /// <param name="sContent"></param>
        /// <returns></returns>
        internal static string BuildPassiveTextMessage(string sFromUserName, string sToUserName, string sContent)
        {
            string sMessage = string.Empty;

            TextMessage _message = new TextMessage()
            {
                Content = sContent,
                FromUserName = sToUserName,
                ToUserName = sFromUserName
            };
            return _message.ToString();
        }


        /// <summary>
        /// 构建关键字回复消息
        /// </summary>
        /// <param name="sResult"></param>
        /// <param name="fromUserName"></param>
        /// <param name="toUserName"></param>
        /// <param name="keyword"></param>
        /// <param name="sContentType">回复类型，0纯文本，1图文消息</param>
        /// <returns></returns>
        public static string BuildKeyWordMessage(string sResult, string fromUserName, string toUserName, string keyword)
        {
            var bll = DI.DIEntity.GetInstance().GetImpl<IWechartManager>();
            var sContentType = 0;
            var ret = bll.GetKeyWordMessage(toUserName, keyword, out sContentType);

            try
            {
                sResult = sContentType == 0 ? 
                            BuildPassiveTextMessage(fromUserName, toUserName, ret[0].sContent) : 
                            BuildPassiveNewsMessage(fromUserName, toUserName, ret);
            }
            catch { }


            // 如果没有启用就返回空字符串 
            return sResult;
        }

    }
}
