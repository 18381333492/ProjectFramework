using Framework.BLL;
using Framework.Dapper;
using Framework.Validate;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class MainController : SuperController
    {
        //进入首页主页面
        public ActionResult Index()
        {
            var user = SessionUser;
            if (user != null)
            {
                return View("Index", user.User);
            }
            else
            {
                return ExitSystem();
            }
        }

        //退出系统后台
        public RedirectResult ExitSystem()
        {
            SetSessionInfo/*清空用户信息*/(SessionInfo.USER_SESSION_NAME, null);
            SetSessionInfo/*清空用户菜单信息*/(SessionInfo.USER_MENUS, null);
            if (SessionUser != null)
            {
                DI.DIEntity
                    .GetInstance()
                    .GetImpl<ISystemLogManager>()
                    .InsertSystemLog(
                        new DTO.EHECD_SystemLogDTO
                        {
                            bIsDeleted = false,
                            dInsertTime = DateTime.Now,
                            ID = Helper.GuidHelper.GetSecuentialGuid(),
                            sDomainDetail = "系统用户退出登录",
                            sIPAddress = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress,
                            sLoginName = SessionUser.User.sLoginName,
                            sUserName = SessionUser.User.sUserName,
                            sDoMainId = SessionUser.User.ID.ToString(),
                            tDoType = (Int16)(DTO.SYSTEM_LOG_TYPE.LOGON | DTO.SYSTEM_LOG_TYPE.SYSTEMUSER)
                        },
                         DI.DIEntity
                        .GetInstance()
                        .GetImpl<Dapper.ExcuteHelper>()
                    );
            }
            return Redirect("/Admin/Login");
        }

        //获取上传图片服务器地址
        [HttpPost]
        public void GetServerUrl()
        {
            result.Succeeded = true;
            result.Data = web.config.WebConfig.LoadElement("url");
        }
    }
}