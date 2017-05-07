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
    public class SharePosterController : SuperController
    {
        // GET: Admin/SharePoster
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 上传海报分享
        /// </summary>
        public void UploadImage() {
            EHECD_ImagesDTO dto = LoadParam<EHECD_ImagesDTO>();
            var ret = LoadInterface<ISharePosterManager>().SharePosterEdit(SessionUser.User, dto);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "上传分享图片失败，请联系管理员";
        }
        public void GetImage() {
            var ret = LoadInterface<ISharePosterManager>().GetImage(SessionUser.User);
            bool flag = ret != null;
            result.Data = ret;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }
    }
}