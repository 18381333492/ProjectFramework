using Framework;
using Framework.BLL;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class SystemUserController : SuperController
    {
        // GET: Admin/SystemUser
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 编辑系统用户
        /// </summary>
        public void EditSystemUser()
        {
            var user = JSONHelper.GetModel<DTO.EHECD_SystemUserDTO>(RequestParameters.dataStr);
            CreateSyslogInfo();
            var ret = DI.DIEntity
                                .GetInstance()
                                .GetImpl<ISystemUserManager>()
                                .EditSystemUser(user, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = !result.Succeeded ? ret == -2 ? "编辑用户失败，已有相同联系手机号码" : "编辑用户失败，请联系系统管理员" : "";
        }

        /// <summary>
        /// 删除系统用户
        /// </summary>
        public void DeleteSystemUser()
        {
            var user = JSONHelper.GetModel<DTO.EHECD_SystemUserDTO>(RequestParameters.dataStr);
            CreateSyslogInfo();
            var ret = DI.DIEntity
                                .GetInstance()
                                .GetImpl<ISystemUserManager>()
                                .DeleteSystemUser(user, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = !result.Succeeded ? "删除用户失败，请联系系统管理员" : "";
        }

        /// <summary>
        /// 冻结用户
        /// </summary>
        public void FrozenSystemUser()
        {
            var user = JSONHelper.GetModel<DTO.EHECD_SystemUserDTO>(RequestParameters.dataStr);
            CreateSyslogInfo();
            var ret = DI.DIEntity
                                .GetInstance()
                                .GetImpl<ISystemUserManager>()
                                .FrozenSystemUser(user, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = !result.Succeeded ? "冻结用户失败，请联系系统管理员" : "";
        }

        /// <summary>
        /// 添加系统用户
        /// </summary>
        public void AddSystemUser()
        {
            var user = JSONHelper.GetModel<DTO.EHECD_SystemUserDTO>(RequestParameters.dataStr);
            CreateSyslogInfo();
            var ret = DI.DIEntity
                                .GetInstance()
                                .GetImpl<ISystemUserManager>()
                                .AddSystemUser(user, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;

            if (!result.Succeeded)
            {
                if (ret == -1)
                {
                    result.Msg = "添加用户失败，已有相同的登录名";
                }
                else if (ret == -2)
                {
                    result.Msg = "添加用户失败，已有相同的手机号";
                }
                else
                {
                    result.Msg = "添加用户失败，请联系系统管理员";
                }
            }
        }

        /// <summary>
        /// 载入系统用户
        /// </summary>
        public void LoadSystemUser()
        {
            var pageinfo = JSONHelper.GetModel<Dapper.PageInfo>(RequestParameters.dataStr);
            var systemUserManager = DI.DIEntity.GetInstance().GetImpl<ISystemUserManager>();
            result.Data = systemUserManager.LoadSystemUsers(pageinfo, RequestParameters.dynamicData);
            result.Succeeded = true;
        }

        /// <summary>
        /// 给系统用户分配角色
        /// </summary>
        public void DistributionRole(DTO.EHECD_SystemUserDTO user)
        {
            CreateSyslogInfo();
            var ret = DI.DIEntity
                .GetInstance()
                .GetImpl<ISystemUserManager>()
                .DistributionRole(user, RequestParameters.dynamicData);

            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "分配用户角色失败，请联系管理员";
        }

        /// <summary>
        /// 跳转到添加系统用户
        /// </summary>
        /// <returns>部分视图</returns>
        public PartialViewResult ToAddSystemUser()
        {
            return PartialView("AddSystemUser");
        }

        /// <summary>
        /// 跳转到编辑系统用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <returns>部分视图</returns>
        public PartialViewResult ToEditSystemUser(DTO.EHECD_SystemUserDTO user)
        {
            user = DI.DIEntity
                       .GetInstance()
                       .GetImpl<BLL.ISystemUserManager>()
                       .GetSystemUserInfoById(user);
            return PartialView("EditSystemUser", user);
        }

        /// <summary>
        /// 跳转到分配角色
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <returns>部分视图</returns>
        public PartialViewResult ToDistributionRole(DTO.EHECD_SystemUserDTO user)
        {
            var bs = DI.DIEntity
                        .GetInstance()
                        .GetImpl<IRoleManager>();

            var rols = bs.LoadAllRoles();

            var alreadyHasRoles = bs.LoadUserRole(user);

            if (rols != null && rols.Count > 0)
            {

                if (alreadyHasRoles != null && alreadyHasRoles.Count > 0)
                {
                    var ret = (from o in rols
                               select new
                               {
                                   ID = o.ID.ToString(),
                                   sRoleName = o.sRoleName,
                                   check = alreadyHasRoles.Where(m => o.ID == m.ID).Select(m =>
                                    {
                                        return "checked";
                                    }).FirstOrDefault()
                               }).Select(m => JSONHelper.GetModel<object>(JSONHelper.GetJsonString<object>(m))).ToList();

                    ViewBag.RoleData = ret;
                }
                else
                {
                    var ret = (from o in rols
                               select new
                               {
                                   ID = o.ID.ToString(),
                                   sRoleName = o.sRoleName,
                                   check = ""
                               }).Select(m => JSONHelper.GetModel<object>(JSONHelper.GetJsonString<object>(m))).ToList();

                    ViewBag.RoleData = ret;
                }
            }

            return PartialView("DistributionRole", user);
        }
    }
}