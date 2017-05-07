using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 店铺短信
    /// </summary>
    public class ShopMessageController :SuperController
    {
        // GET: Admin/ShopMessage
        public ActionResult Index()
        {
            EHECD_SystemUserDTO dto = LoadInterface<IShopManager>().GetMessageCount(SessionUser.User.ID.ToString());
            return View(dto);
        }

        public ActionResult Edit()
        {
            return PartialView();
        }
        /// <summary>
        /// 页面数据绑定
        /// </summary>
        public void GetPageList()
        {
            PageInfo info = LoadParam<PageInfo>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopManager>().GetPageList(info,dic);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        /// <summary>
        /// 获取店铺短信数
        /// </summary>
        public void GetCount()
        {
            EHECD_SystemUserDTO CC = LoadParam<EHECD_SystemUserDTO>();
            var ret = LoadInterface<IShopManager>().GetMessageCount(CC.ID.ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
        /// <summary>
        /// 修改短信数
        /// </summary>
        public void UpdateCount()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            var ret =LoadInterface<IShopManager>().UpdateCount(user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改票数失败，请联系系统管理员";
        }
        /// <summary>
        /// 获得平台短信票数
        /// </summary>
        public void GetMessageCount()
        {
            var ret = LoadInterface<IShopManager>().GetMessageCount(SessionUser.User.ID.ToString());

            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }

        public void UpdateMessageCount()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            user.ID = SessionUser.User.ID;
            var ret = LoadInterface<IShopManager>().UpdateCount(user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改票数失败，请联系系统管理员";
        }
        public void UpdateTerraceCount()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            user.ID = new Guid(dic["ID"].ToString());
            user.sMessage = int.Parse(dic["sMessage"].ToString());
            EHECD_SystemUserDTO SYS = new EHECD_SystemUserDTO();
            SYS.ID = SessionUser.User.ID;
            var name = LoadInterface<IShopManager>().GetMessageCount(SessionUser.User.ID.ToString());
            SYS.sMessage = name.sMessage - int.Parse(dic["difference"].ToString());
            var ret = LoadInterface<IShopManager>().UpdateTerraceCount(user,SYS);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改票数失败，请联系系统管理员";
        }


    }
}