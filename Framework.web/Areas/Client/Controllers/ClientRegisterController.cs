using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Client.Controllers
{
    using AppCache;
    using Framework.BLL;
    using Helper;
    using System.Text;
    using WeiXin.Tool;
    using WeiXin.Base.User.UserInfo;
    using DTO;

    public class ClientRegisterController : APISuperController
    {
        // GET: Client/Register
        public ActionResult Index()
        {
            string code = Request.QueryString["code"];
            string appid = config.WebConfig.LoadDynamicJson("weixin").AppId;

            if (string.IsNullOrEmpty(code))
            {
                var path = string.Format("{0}{1}", config.WebConfig.LoadDynamicJson("weixin").LocalDomain, Request.Url.LocalPath);
                // 获取code
                string code_url = string.Format(
                    @"https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=lk#wechat_redirect",
                    appid,
                    HttpUtility.UrlEncode(path, Encoding.UTF8));
                return Redirect(code_url);
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

                UserInfo user = UserInfo.Get(openid, access_token);

                if (user != null)
                    SetSessionInfo("weixin_user", user);

            }
            return View();

        }

        /// <summary>
        /// 查看用户协议
        /// </summary>
        /// <returns></returns>
        public ActionResult UserProtocol()
        {
            return View();
        }

        /// <summary>
        /// 载入用户协议
        /// </summary>
        public void LoadUserProtocal()
        {
            var bll = LoadInterface<IClientRegisterManager>();
            result.Data = bll.LoadUserProtocal();
            result.Succeeded = true;
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        public void GetPhoneCheckNumber()
        {
            var param = LoadParam<Dictionary<string, string>>()["phoneNumber"];

            var bll = LoadInterface<IClientRegisterManager>();

            // 判断手机号是否已经被注册
            if (!bll.ExistsClientRegistPhoneNumber(param))
            {
                result.Msg = "该号码已被注册";
                return;
            }

            // 判断当前微信号是否已经绑定过手机
            var user = (UserInfo)GetSessionInfo("weixin_user");
            if (!bll.ExistsClientRegistWeiXin(user.openid)) {
                result.Msg = "该微信号已经绑定过手机号";
                return;
            }

            //检查该账户在指定时间内是否已经获取过登录名
            if (ApplicationCache.Instance.GetValue(param+"isGetGode") != null&& ((bool)ApplicationCache.Instance.GetValue(param+"isGetGode"))==true)
            {
                result.Succeeded = false;
                result.Msg = "60秒内已经获取过验证短信，请等待60秒后再获取";
                return;
            }

            var messger = base.LoadInterface<Validate.IMessager>();     //短信发送接口
            var code = messger.RandomTool.GetRandomNumberString(5);      //随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定

//#if DEBUG
//            result.Succeeded = true;
//            result.Data = code;
//            ApplicationCache.Instance.SetValue(param, code, 10*60);// 缓存验证码

//            ApplicationCache.Instance.SetValue(param+"isGetGode",true,60);//缓存对象标志用户【获取验证码】这个动作(60秒)
//#endif

            if (messger.SendMessage(param, string.Format("感谢您访问友客分享商城，您的验证码是{0}", code)))
            {
                ApplicationCache.Instance.Delete(param);//先清除  在缓存
                ApplicationCache.Instance.SetValue(param, code, 10*60);

                ApplicationCache.Instance.SetValue(param+"isGetGode", true, 60);//缓存对象标志用户【获取验证码】这个动作(60秒)
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "发送短消息至手机失败，请联系管理员";
            }
        }

        /// <summary>
        /// 注册用户
        /// </summary> 
        public void RegisteClient()
        {
            dynamic param = LoadDynamicParam();
            string phoneNumber = param.phoneNumber;
            string rcode = param.rcode;
            var code = ApplicationCache.Instance.GetValue(phoneNumber);
            var user = (UserInfo)GetSessionInfo("weixin_user");
            if (code != null)
            {
                if (code.ToString() == rcode)
                {
                    if (user != null)
                    {
                        var bll = LoadInterface<IClientRegisterManager>();
                        string pwd = param.psd;
                        var ret = bll.RegisteClient(new EHECD_ClientDTO
                        {
                            sOpenId = user.openid,
                            sHeadPic = user.headimgurl,
                            sNickName = user.nickname,
                            sPassWord = Security.GetMD5Hash(pwd),
                            dAccountBalance = 0,
                            bIsDeleted = false,
                            dAddTime = DateTime.Now,
                            iClientType = 0,
                            iSex = user.sex.ToInt32(),
                            ID = GuidHelper.GetSecuentialGuid(),
                            sPhone = phoneNumber,
                            sLoginName = phoneNumber,
                            iState = 1,
                            iSource = 2
                        });

                        result.Succeeded = ret != null;
                        result.Msg = result.Succeeded ? "" : "注册失败，您的微信号已进行了手机验证";
                        if (result.Succeeded)
                        {
                            GetSessionInfoNoResident("weixin_user");
                            SetSessionInfo("_client", ret);
                        }
                    }   
                    else
                    {
                        result.Msg = "未获取到您的微信授权信息，注册失败";
                    }
                }
                else
                {
                    result.Msg = "验证码错误，请重新获取";
                }
            }
            else
            {
                result.Msg = "验证码失效，请重新获取";
            }
        }
    }
}