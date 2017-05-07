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
    public class HosterStoryController : SuperController
    {
        // GET: Admin/HosterStory
        public ActionResult Index()
        {
            IShopSetManager shop = LoadInterface<IShopSetManager>();
            EHECD_ShopSetDTO dto = shop.GetShopSet(SessionUser.User);
            return View(dto);
        }

        public ActionResult LookDetail() {
            return PartialView();
        }

       
    }
}