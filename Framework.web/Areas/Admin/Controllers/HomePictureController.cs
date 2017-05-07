using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class HomePictureController : SuperController
    {
        // GET: Admin/HomePicture
        public ActionResult Index()
        {
            return View();
        }
        public void GetList()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var ret = LoadInterface<IHomePictureManager>().GetPageList(page);
            result.Data = ret;
            if (result.Data != null)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取数据失败";
            }
        }

        public ActionResult Add() {
            return PartialView();
        }
        //编辑
        public ActionResult Edit()
        {
            var ID = Request.QueryString["ID"];
            EHECD_ImagesDTO user = LoadInterface<IHomePictureManager>().GetPicture(ID);
            return PartialView(user);
            
        }

        public ActionResult PicturePreview()
        {
            return PartialView();
        }
        /// <summary>
        /// 添加图片
        /// </summary>
        public void AddPicture()
        {
            EHECD_ImagesDTO dto = LoadParam<EHECD_ImagesDTO>();
            var ret = LoadInterface<IHomePictureManager>().AddPicture(dto);
            result.Succeeded = ret>0;
            result.Msg = result.Succeeded ? "" : "添加失败，请联系系统管理员";
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        public void DeletePicture()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var ret = LoadInterface<IHomePictureManager>().Delete(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";
        }
        /// <summary>
        /// 修改图片信息
        /// </summary>
        public void EditPicture()
        {
            EHECD_ImagesDTO dto = LoadParam<EHECD_ImagesDTO>();
            var ret = LoadInterface<IHomePictureManager>().Update(dto);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改文章失败，请联系系统管理员";
        }
        /// <summary>
        /// 显示/隐藏
        /// </summary>
        public void Display()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IHomePictureManager>().DisplatPicture(dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "操作失败，请联系系统管理员";
        }


        public void lookImage()
        {
            var ret = LoadInterface<IHomePictureManager>().lookImage();
            if (ret != null)
            {
                result.Succeeded = true;
                result.Data = ret;
            }
            else {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }


        public void deleteHomePicture()
        {
            var dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IHomePictureManager>().deletePicture(dic["ID"].ToString());
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "失败，请联系系统管理员";
        }
    }
}