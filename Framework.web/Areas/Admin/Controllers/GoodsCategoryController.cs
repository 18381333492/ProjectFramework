using Framework.BLL;
using Framework.DI;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class GoodsCategoryController : SuperController
    {
        // GET: Admin/GoodsCategory
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 载入商品分类
        /// </summary>
        public void LoadGoodsCategory()
        {
            var bll = LoadInterface<IGoodsCategoriesManager>();
            result.Data = bll.LoadGoodsCategories();
            result.Succeeded = true;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        public void DeleteGoodsCategory()
        {
            var c = LoadParam<EHECD_CategoriesDTO>();
            CreateSyslogInfo();
            var ret = LoadInterface<IGoodsCategoriesManager>().DeleteGoodsCategory(c, RequestParameters.dynamicData);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除商品分类失败，请联系管理员";
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        public void AddGoodsCategory()
        {
            var c = LoadParam<EHECD_CategoriesDTO>();
            CreateSyslogInfo();
            var ret = LoadInterface<IGoodsCategoriesManager>().AddGoodsCategories(c, RequestParameters.dynamicData);

            result.Succeeded = ret != null;

            result.Msg = result.Succeeded ? "" : "添加商品种类" + c.sCategoryName + "失败，请联系管理员";

            if (!result.Succeeded) return;

            result.Data = new
            {
                id = ret.ID,
                text = ret.sCategoryName,
                sCategoryCaption = ret.sCategoryCaption,
                addDate = ret.dInsertTime,
                iOrder = ret.iOrder,
                sPID = ret.PID,
                sImgUri = ret.sImgUri
            };
        }

        /// <summary>
        /// 跳转到添加分类视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ToAddGoodsCategory()
        {
            return PartialView("AddGoodsCategory");
        }

        /// <summary>
        /// 跳转到编辑分类视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ToEditGoodsCategory(EHECD_CategoriesDTO c)
        {
            var bll = LoadInterface<IGoodsCategoriesManager>();
            var obj = bll.LoadGoodsCategorie(c);
            return PartialView("EditGoodsCategory", obj);
        }
    }
}