using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 店铺管理
    /// </summary>
    public class ShopController : SuperController
    {
        // GET: Admin/Shop
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
            var ID = Request.QueryString["ID"];
            EHECD_SystemUserDTO user = LoadInterface<IShopManager>().GetUser(ID);
            return PartialView(user);

        }
        /// <summary>
        /// 详情页面
        /// </summary>
        /// <returns></returns>
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
        /// 添加店铺
        /// </summary>
        public void AddShop()
        {
            Dictionary<string, object> data = LoadParam<Dictionary<string, object>>();
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            IShopManager shop = LoadInterface<IShopManager>();
            if (shop.TheSeamLoginName(user)!=null)
            {
                result.Msg = "已有相同的用户名";

            }
            else
            {
                data["sFristLetter"]= PinYinHelper.GetPinYin(data["sCity"].ToString(), true).Substring(0, 1);
                var res = shop.AddShop(data);
                result.Succeeded = res > 0;
                result.Msg = result.Succeeded ? "" : "新增店铺失败，请联系系统管理员";
                
            }
        }


        /// <summary>
        /// 冻结/解冻
        /// </summary>
        public void FreezeCheck()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopManager>().FreezeCheck(dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "冻结失败，请联系系统管理员";

        }
        /// <summary>
        /// 修改密码
        /// </summary>
        public void UpDatePassword()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            var ret = LoadInterface<IShopManager>().UpDatePassword(user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改密码失败，请联系系统管理员";
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void DeleteCheck()
        {
            Dictionary<string, object> ID = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IShopManager>().DeleteCheck(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";

        }
        /// <summary>
        /// 获取所有的合伙人
        /// </summary>
        public void GetAllPartner()
        {
            var ret = LoadInterface<IShopManager>().GetAllPartner();
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
        /// 修改合伙人
        /// </summary>
        public void UpdatePartner()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            var ret = LoadInterface<IShopManager>().UdatePartner(user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改密码失败，请联系系统管理员";
        }

       

    }
}