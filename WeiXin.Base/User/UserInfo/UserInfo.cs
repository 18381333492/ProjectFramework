using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WeiXin.Tool;
using Newtonsoft.Json.Linq;

namespace WeiXin.Base.User.UserInfo
{
    [Serializable]
    public class UserInfo
    {
        /// <summary>
        /// 消息处理类
        /// </summary>
        public IAction action { get; set; }

        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public string subscribe
        {
            get;
            set;
        }

        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid
        {
            get;
            set;
        }

        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname
        {
            get;
            set;
        }

        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city
        {
            get;
            set;
        }

        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country
        {
            get;
            set;
        }

        /// <summary>
        ///  用户所在省份
        /// </summary>
        public string province
        {
            get;
            set;
        }

        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language
        {
            get;
            set;
        }

        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，
        /// 0代表640*640正方形头像），用户没有头像时该项为空
        /// </summary>
        public string headimgurl
        {
            get;
            set;
        }

        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public long subscribe_time
        {
            get;
            set;
        }

        /// <summary>
        /// json数据转化为UserInfo对象
        /// </summary>
        /// <param name="sJsonData"></param>
        public static UserInfo Init(string sJsonData)
        {
            return JsonConvert.DeserializeObject<UserInfo>(sJsonData);
        }

        /// <summary>
        /// 通过OPENID,ACCESS_TOKEN,Lang 获取用户数据
        /// </summary>
        /// <param name="sOpenId">普通用户的标识，对当前公众号唯一</param>
        /// <param name="sACCESS_TOKEN">调用接口凭证</param>
        /// <param name="sLang">返回国家地区语言版本，zh_CN 简体，zh_TW 繁体，en 英语</param>
        /// <param name="iSubscribe">关注状态  1:已关注  0:未关注</param>
        public static UserInfo Get(string sOpenId, string sACCESS_TOKEN, string sLang = "zh_CN", int iSubscribe = 1)
        {
            if (string.IsNullOrEmpty(sLang))
                sLang = "zh_CN";
            string sUrl = string.Empty;
            //if (iSubscribe == 1)
            //    sUrl = string.Format(
            //        "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}", sACCESS_TOKEN,
            //        sOpenId, sLang);
            //else
            //    sUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}",
            //        sACCESS_TOKEN, sOpenId, sLang);

            sUrl = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}",
                    sACCESS_TOKEN, sOpenId, sLang);

            string sResult = HttpHelper.Get(sUrl, Encoding.UTF8);
            JObject jResult = JObject.Parse(sResult);
            JToken jErrcode = jResult["errcode"];
            if (null != jErrcode && !jErrcode.ToString().Equals("0"))
                return null;
            else
                return Init(sResult);
        }

        /// <summary>
        /// 保存用户信息（向表 EHECD_WeixinUser 写微信用户数据）
        /// </summary>
        /// <param name="sWeiXinId">公众号ID</param>
        /// <param name="iInviteid">邀请人ID</param>
        /// <returns></returns>
        public bool Save(string sWeiXinId,string iInviteid=null)
        {
            if (string.IsNullOrEmpty(iInviteid))
                iInviteid = string.Empty;
            return action.Add(this, sWeiXinId, iInviteid);
        }
    }
} 
