using Framework.BLL;
using Framework.Dapper;
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
    public class GuestRoomController : SuperController
    {
        // GET: Admin/GuestRoom
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 载入客房类型列表
        /// </summary>
        public void LoadGuestRoomTypes()
        {
            var pageInfo = LoadParam<PageInfo>();

            var types = LoadInterface<IGuestRoomTypeManager>().
                        GetPageList(pageInfo);

            result.Data = types;
            result.Succeeded = true;
        }

        /// <summary>
        /// 跳转到添加客房类型视图
        /// </summary>
        /// <returns></returns>
        public PartialViewResult ToAddGuestRoomType()
        {
            return PartialView("AddGuestRoomType");
        }

        /// <summary>
        /// 添加客房类型
        /// </summary>
        public void AddGuestRoomType()
        {
            var room = LoadParam<EHECD_GuestRoomTypeDTO>();

            var ret = LoadInterface<IGuestRoomTypeManager>().AddGuestRoomType(room);

            result.Succeeded = ret;

            result.Msg = ret ? "" : "添加客房类型失败，请联系管理员";
        }

        /// <summary>
        /// 删除客房类型
        /// </summary>
        public void DeleteGuestRoomTypes()
        {
            var roomType = LoadParam<EHECD_GuestRoomTypeDTO>();

            var ret = LoadInterface<IGuestRoomTypeManager>().DeleteRoomType(roomType,SessionUser.User);

            result.Succeeded = ret;

            result.Msg = ret ? "" : "删除客房类型失败，请联系管理员";
        }
    }
}