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
    public class ShopSetController : SuperController
    {
        // GET: Admin/ShopSet
        public ActionResult Index()
        {
            IShopSetManager shop = LoadInterface<IShopSetManager>();
            EHECD_ShopSetDTO dto = shop.GetShopSet(SessionUser.User);
            IList<EHECD_CustomServiceDTO> custom = shop.GetCustomers(SessionUser.User);
            ViewBag.CustomerList = custom;
            return View(dto);
        }
        /// <summary>
        /// 设置店铺信息
        /// </summary>
        public void SetShopMessage()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            if (dic["sCity"].ToString() == "市辖区" || dic["sCity"].ToString() == "市辖县")
            {
                dic["sCity"] = dic["sProvice"];
            }
            var city = dic["sCity"].ToString();
            if (dic["sCity"].ToString() == "重庆市")
            {
                dic["sFristLetter"] = "c";
            }
            else {
                dic["sFristLetter"] = PinYinHelper.GetPinYin(city, true).Substring(0, 1);
            }
           
            var ret = LoadInterface<IShopSetManager>().SetShopMessage( SessionUser.User,dic);
            result.Succeeded = ret > 0 ;
            result.Msg=result.Succeeded?"":"设置失败，请联系管理员";
        }
        /// <summary>
        /// 获取店铺轮播图
        /// </summary>
        public void GetImages()
        {
            IShopSetManager shop = LoadInterface<IShopSetManager>();
            IList<EHECD_ImagesDTO> image = shop.GetImageList(SessionUser.User);
            bool flag = image!= null;
            result.Data = image;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }
    }
}