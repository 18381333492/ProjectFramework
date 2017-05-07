using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Client.Controllers
{
    using BLL;

    public class ClientLoginController : APISuperController
    {
        // GET: Client/Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        public void Login()
        {
            var loginInfo = LoadParam<Dictionary<string,string>>();
            var bll = LoadInterface<IClientLoginManager>();
            var ret = bll.Login(loginInfo);
            if(ret != null)
            {
                if (ret.iState ==0)
                {//该用户被冻结
                    result.Succeeded = false;
                    result.Msg = "该用户已被冻结,不能登录";
                }
                else
                {
                    result.Succeeded = true;
                    SetSessionInfo("_client", ret);
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "登录失败！用户名或密码错误";
            }
        }
    }
}