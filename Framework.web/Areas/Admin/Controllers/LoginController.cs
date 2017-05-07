using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DTO;
using Framework.Validate;
using Framework.web.Controllers;
using Framework.AppCache;
using Framework.BLL;

namespace Framework.web.Areas.Admin.Controllers
{
    public class LoginController : SuperController
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public void ChangePWD()
        {
            var param = LoadParam<Dictionary<string, string>>();
            if (ApplicationCache.Instance.GetValue(param["sLoginName"]) != null)
            {
                var code = ApplicationCache.Instance.GetValue(param["sLoginName"]).ToString();
                if (code == param["m_msg_code"])
                {
                    var login/*登录业务*/ = base.LoadInterface<ILogin>();
                    result.Succeeded = login.ChangePassword(new EHECD_SystemUserDTO { sLoginName = param["sLoginName"], sPassWord = param["npsw"] }) != null;
                    result.Msg = result.Succeeded ? "" : "重置密码失败，请联系管理员";
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "很抱歉，您所输入的验证码错误，重置密码失败";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "短信验证码已过期，请重新获取";
            }
        }

        /// <summary>
        /// 发送重置密码的短信验证码
        /// </summary>
        public void SendMessage()
        {
            var param/*要获取短信验证码的登录名*/ = LoadParam<Dictionary<string, string>>()["sloginName"];

            //检查该账户在指定时间内是否已经获取过登录名
            if (ApplicationCache.Instance.GetValue(param) != null)
            {
                result.Succeeded = false;
                result.Msg = "两分钟内已经获取过验证短信，请等待两分后再获取";
                return;
            }

            var login/*登录业务*/ = base.LoadInterface<ILogin>();
            var mnumber/*根据登录名获取到的电话号码（电话号是唯一的）*/ = login.QueryMobileNumberByLoginName(param);
            if (mnumber != null)
            {
                var messger/*短信发送接口*/ = base.LoadInterface<Validate.IMessager>();
                var code/*随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定*/ = messger.RandomTool.GetRandomNumberString();
                if (messger.SendMessage(mnumber, string.Format(messger.ChangePWDMessage, code)))
                {
                    ApplicationCache.Instance.SetValue(param, code, 120);
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "发送短消息至手机失败，请联系管理员";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "没有获取到该登录名的联系电话，请确认登录名是否正确？";
            }
        }

        /// <summary>
        /// 跳转到重置密码界面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ToForgetPWD()
        {
            return PartialView("ForgetPWD");
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            //获取上传的账户信息
            var user = LoadParam<EHECD_SystemUserDTO>();

            //获取session信息
            SessionInfo session = GetSessionInfo(SessionInfo.USER_SESSION_NAME) as SessionInfo;

            //校验验证码:这里偷了个懒，用的EHECD_SystemUserDTO的sUserName临时装了一下
            if (user.sUserName.ToLower() != session.ImgCode.VCodeContent.ToLower())
            {
                result.Succeeded = false;
                result.Msg = "验证码验证失败，请输入正确的验证码";
                return;
            }

            ILogin login = LoadInterface<ILogin>();

            //临时用地址装一下ip的值，主要用于写登录的系统日志
            user.sAddress = RequestParameters.dynamicData.IP.Value.ToString();

            var dto = login.Login(user);

            if (dto != null)
            {
                if (dto.tUserState == 1)
                {
                    result.Succeeded = false;
                    result.Msg = "登录失败，该用户已被冻结";
                    return;
                }

                //获取该用户的权限
                var userRoleMenu = login.LoadUserRoleMenuInfo(dto);
                if (userRoleMenu.LoadSuccess)
                {
                    session.SessionUser.User = dto;
                    SetSessionInfo(SessionInfo.USER_SESSION_NAME/*将用户的信息放入session*/, session);
                    SetSessionInfo(SessionInfo.USER_MENUS/*将用户的权限和菜单等信息放入session*/, userRoleMenu);
                    result.Succeeded = true;
                    result.Data = "/Admin/Main";
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "登录失败，权限菜单获取失败";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "登录失败，用户名或密码错误";
            }
        }
    }
}