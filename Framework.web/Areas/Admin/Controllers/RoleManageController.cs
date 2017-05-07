using Framework.web.Controllers;
using Framework.DTO;
using Framework.DI;
using System.Web.Mvc;
using Framework.BLL;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class RoleManageController : SuperController
    {
        // GET: Admin/RoleManage
        public PartialViewResult Index()
        {
            return PartialView();
        }
        
        #region 跳转动作
        /// <summary>
        /// 跳转到添加角色页面
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ToAddRole()
        {
            return PartialView("AddRole");
        }

        /// <summary>
        /// 跳转到编辑角色页面
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PartialViewResult ToEditRole(EHECD_RoleDTO role)
        {
            return PartialView("EditRole", role);
        }

        /// <summary>
        /// 跳转到分配菜单给角色页面
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PartialViewResult ToDistributionMenu(EHECD_RoleDTO role)
        {
            ViewBag.TreeData = DI.DIEntity
                            .GetInstance()
                            .GetImpl<IRoleManager>()
                            .LoadDistributionMenu(role);
            return PartialView("DistributionMenu");
        }

        /// <summary>
        /// 跳转到分配菜单按钮给角色页面
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PartialViewResult ToDistributionButton(EHECD_RoleDTO role)
        {
            ViewBag.TreeData = DI.DIEntity
                            .GetInstance()
                            .GetImpl<IRoleManager>()
                            .LoadDistributionMenuButton(role);
            return PartialView("DistributionButton");
        }
        #endregion

        #region 对角色的操作
        // 载入角色列表
        public void LoadRoles()
        {
            var param = RequestParameters.data;
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            var ret = manager.LoadRoles(param);
            result.Data = ret;
            result.Succeeded = true;
        }

        // 分配菜单权限
        public void DistributionMenu(EHECD_RoleDTO role)
        {
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            CreateSyslogInfo();
            result.Succeeded = manager.DistributionMenu(role, RequestParameters.dynamicData) > 0;
            result.Msg = !result.Succeeded ? "" : "分配角色菜单失败，请联系系统管理员";
        }

        // 分配菜单按钮权限
        public void DistributionButton(EHECD_RoleDTO role)
        {
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            CreateSyslogInfo();
            result.Succeeded = manager.DistributionMenuButton(role, RequestParameters.dynamicData) > 0;
            result.Msg = !result.Succeeded ? "" : "分配角色菜单按钮失败，请联系系统管理员";
        }

        //编辑角色
        public void EditRole()
        {
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            CreateSyslogInfo();
            var ret = manager.EditRole(JSONHelper.GetModel<EHECD_RoleDTO>(RequestParameters.dataStr), RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = !result.Succeeded ? ret == -1 ? "编辑角色失败，已有相同角色名" : "编辑角色失败，请联系系统管理员" : "";
        }

        //添加角色
        public void AddRole()
        {
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            CreateSyslogInfo();
            var ret = manager.AddRole(RequestParameters.data, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = !result.Succeeded ? ret == -1 ? "添加角色失败，已有相同角色名" : "添加角色失败，请联系系统管理员" : "";
        }

        //删除角色
        public void DeleteRole()
        {
            var manager = DIEntity.GetInstance().GetImpl<IRoleManager>();
            CreateSyslogInfo();
            var ret = manager.DeleteRole(((dynamic)RequestParameters.data).ID.ToString(), RequestParameters.dynamicData);
            if (ret > 0)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "删除失败，请联系管理员";
            }
        }
        #endregion
    }
}