using Framework.AppCache;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Client.Controllers
{
    using BLL;
    using Framework.AppCache;

    public class ClientPasswordController : APISuperController
    {
        // GET: Client/ClientPassword
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取手机验证码
        /// </summary>
        public void GetPhoneCheckNumber()
        {
            var param = LoadParam<Dictionary<string, string>>()["phoneNumber"];
            var bll = LoadInterface<IClientManager>();

            if (!bll.IsExit(param))
            {//改电话号码不存在
                result.Succeeded = false;
                result.Msg = "该号码未注册过,不能进行此操作";
            }
            else
            {
                //检查该账户在指定时间内是否已经获取过登录名
                if (ApplicationCache.Instance.GetValue(param + "FindisGetGode") != null && ((bool)ApplicationCache.Instance.GetValue(param + "FindisGetGode")) == true)
                {
                    result.Succeeded = false;
                    result.Msg = "60秒内已经获取过验证短信，请等待60秒后再获取";
                    return;
                }

                var messger = base.LoadInterface<Validate.IMessager>();     //短信发送接口
                var code = messger.RandomTool.GetRandomNumberString(5);      //随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定
                                                                             //var code = messger.RandomTool.GetMixRandomString(32);      //随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定
                                                                             //#if DEBUG
                                                                             //            result.Succeeded = true;
                                                                             //            result.Data = code;
                                                                             //            ApplicationCache.Instance.SetValue(param, code, 10 * 60);

                //            ApplicationCache.Instance.SetValue(param+"FindisGetGode", true, 60);//缓存对象标志用户【获取验证码】这个动作(60秒)
                //#endif

                if (messger.SendMessage(param, string.Format("感谢您访问友客分享商城，您的验证码是{0}", code)))
                {
                    ApplicationCache.Instance.Delete(param);//先清除  在缓存
                    ApplicationCache.Instance.SetValue(param, code, 10 * 60);

                    ApplicationCache.Instance.SetValue(param + "FindisGetGode", true, 60);//缓存对象标志用户【获取验证码】这个动作(60秒)
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "发送短消息至手机失败，请联系管理员";
                }
            }
        }

        /// <summary>
        /// 跳转到修改密码
        /// </summary>
        public void ToResetPassword()
        {
            var param = LoadParam<Dictionary<string, string>>();
            var code = ApplicationCache.Instance.GetValue(param["phoneNumber"]);
            if (code != null)
            {
                if (code.ToString().Equals(param["code"]))
                {
                    // 将用户要修改的手机号临时保存在会话，免得下一个页面他通过技术手段去篡改别人的手机号
                    SetSessionInfo("temp_c_pwd_pn", param["phoneNumber"]);
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "验证码错误，请重新输入";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "验证码已失效，请重新获取验证码";
            }
        }

        /// <summary>
        /// 获取修改密码页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            return View();
        }

        /// <summary>
        /// 完成修改密码 
        /// </summary>
        public void CompleteChangePWD()
        {
            var param = LoadParam<Dictionary<string, string>>();
            var phoneNumber = GetSessionInfoNoResident("temp_c_pwd_pn");
            if (phoneNumber != null)
            {
                if (param["phoneNumber"].Equals(phoneNumber.ToString()))
                {
                    var bll = LoadInterface<IClientLoginManager>();
                    result.Succeeded = bll.ChangePassword(param["phoneNumber"], param["pwd"]);
                    result.Msg = result.Succeeded ? "" : "修改密码失败，请联系管理员";
                }
                else
                {
                    result.Msg = "修改密码的手机号不正确，请返回重新获取验证码";
                }
            }
            else
            {
                result.Msg = "修改密码的手机号获取失败，请返回重新获取验证码";
            }
        }

       
    }
}