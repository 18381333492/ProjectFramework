using Framework.BLL;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class HosterMessageController : SuperController
    {
        // GET: Admin/HosterMessage
        public ActionResult Index()
        {
            IShopSetManager shop = LoadInterface<IShopSetManager>();
            EHECD_ShopSetDTO dto = shop.GetShopSet(SessionUser.User);
            return View(dto);
        }
        /// <summary>
        /// 更改店铺信息
        /// </summary>
        public void UpdateHoster()
        {
            EHECD_ShopSetDTO shop = LoadParam<EHECD_ShopSetDTO>();
            var vet = LoadInterface<IShopSetManager>().HosterMessage(shop, SessionUser.User);
            result.Succeeded = vet > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系管理员";
        }
    }
}