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
    public class PartnerShopController : SuperController
    {
        // GET: Admin/PartnerShop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            var ID = Request.QueryString["ID"];
            EHECD_SystemUserDTO user = LoadInterface<IPartnerManager>().GetUserByID(ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (user != null)
            {
                ViewBag.picture = LoadInterface<IPartnerManager>().SerachName(ID);
            }
            return PartialView(user);
        }
        /// <summary>
        /// 合伙人店铺页面数据绑定
        /// </summary>
        public void GetPageList()
        {
            PageInfo info = LoadParam<PageInfo>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopManager>().GetPartnerShop(info, dic,SessionUser.User);
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
    }
}