using Framework.DTO;
using Framework.Validate;
using Framework.web.Controllers;
using Framework.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class MenuController : SuperController
    {
        // GET: Admin/Menu
        [HttpPost]
        public PartialViewResult Index()
        {
            return PartialView();
        }

        //载入菜单
        public void LoadMenu()
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                result.Data = MenuTagCreator.CreateBootStrapTreeMenu(userRoleMenu);
                result.Succeeded = true;
            }
            else
            {
                result.Data = "";
                result.Succeeded = false;
            }
        }
    }
}