using Framework.BLL;
using Framework.Dapper;
using Framework.DI;
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
    public class ArticleController : SuperController
    {
        // GET: Admin/Article
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
            var id = Request.QueryString["ID"];
            EHECD_ArticleDTO dto = DIEntity.GetInstance().GetImpl<IArticle>().searchText(id);
            return PartialView(dto);
        }

        /// <summary>
        /// 文章预览
        /// </summary>
        /// <returns></returns>
        public ActionResult Preview()
        {
            return View();
        }



        /// <summary>
        /// 获取列表绑定
        /// </summary>
        public void GetList()
        {
            PageInfo page = LoadParam<PageInfo>();
            var ret = LoadInterface<IArticle>().GetPageList(page);
            result.Data = ret;
            if (ret != null)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取数据失败";
            }
            
            
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        public void Insert()
        {
            Article data = LoadParam<Article>();
            var ret= DIEntity.GetInstance().GetImpl<IArticle>().Insert(data);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加文章失败，请联系系统管理员";
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        public void PreviewArticle()
        {
            Article data = LoadParam<Article>();
            data.ID = GuidHelper.GetSecuentialGuid();
            var ret = DIEntity.GetInstance().GetImpl<IArticle>().Preview(data);
            result.Data = data.ID;
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "预览文章失败，请联系系统管理员";
        }


        /// <summary>
        /// 修改文章
        /// </summary>
        public void UpdateText()
        {
            Article data = JSONHelper.GetModel<Article>(RequestParameters.dataStr);
            var ret  = DIEntity.GetInstance().GetImpl<IArticle>().Update(data);
            result.Data = data.ID;
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改文章失败，请联系系统管理员";
        }
        /// <summary>
        /// 删除文章
        /// </summary>
        public void DeleteText()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var ret = DIEntity.GetInstance().GetImpl<IArticle>().Delete(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除文章失败，请联系系统管理员";
        }
    }
}