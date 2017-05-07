using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.App_Start;
//using Framework.web.App_Start;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class WeChartMenuController : SuperController
    {
        // GET: Admin/WeChartMenu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }
        public ActionResult Edit()
        {
            return PartialView();
        }
        /// <summary>
        /// 获取菜单列表 
        /// </summary>
        public void GetMenuList()
        {
            PageInfo info = LoadParam<PageInfo>();

            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var ret = bll.GetMenuList(info, param);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        public void AddMenu()
        {
            EHECD_WeChartMenuDTO dto = LoadParam<EHECD_WeChartMenuDTO>();

            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var ret = bll.AddMenu(dto, param);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加菜单失败，请联系系统管理员";
        }
        /// <summary>
        /// 获取所有的父菜单
        /// </summary>
        public void GetAllMenu()
        {
            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var ret =bll.GetAllMenu(param);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 查看是否有重名的菜单名
        /// </summary>
        public void SearchMenu()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var ret = bll.SearchMenu(dic["sMenuName"].ToString(), param);
            if (ret == null)//如果未找到相同的菜单名则成功
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 根据ID查看详情
        /// </summary>
        public void SearchDetail()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().SearchDetail(dic["ID"].ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 修改菜单
        /// </summary>
        public void EditMenu()
        {
            EHECD_WeChartMenuDTO dto = LoadParam<EHECD_WeChartMenuDTO>();
            var ret = LoadInterface<IWechartManager>().EditMenu(dto);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改菜单失败，请联系系统管理员";
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        public void DeleteMenu() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().DeleteMenus(dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除菜单失败，请联系系统管理员";
        }

        /// <summary>
        /// 生成菜单
        /// </summary>
        public void GeneratorMenu()
        {
            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var userMenus = bll.GetAllMenuByShopID(param);
            result.Succeeded = WeiXinTool.buildMenu(userMenus, param);
            if (!result.Succeeded) result.Msg = "生成菜单失败，请联系管理员";
        }

        /// <summary>
        /// 判断父类菜单有多少个
        /// </summary>
        public void GetFatherMenuNumber()
        {
            var param = SessionUser.User;
            var bll = LoadInterface<IWechartManager>();
            if (param.tUserType == 0)
            {

                // 如果当前是平台用户，从配置文件中获取数据
                param.ID = config.WebConfig.LoadDynamicJson("weixin").WxKeyId;

            }

            var ret = bll.GetFatherMenuNumber(param.ID.ToString());
            if (ret <= 2)
            {
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "已存在三个父类菜单";
            }
        }
    }
}