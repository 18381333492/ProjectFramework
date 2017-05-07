using Framework.BLL;
using Framework.Dapper;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{

    /// <summary>
    /// 房态管理控制器
    /// </summary>
    public class RoomManageController : SuperController
    {
        // GET: Admin/RoomManage

        private IRoomManageManager domin;

        public RoomManageController()
        {
            domin = LoadInterface<IRoomManageManager>();
        }

        #region 视图

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(string sGoodsId, string iHouseCount)
        {
            ViewBag.sGoodsId = sGoodsId;
            ViewBag.iHouseCount = iHouseCount;
            return View();
        }

        #endregion

        /// <summary>
        /// 获取房态管理数据列表
        /// </summary>
        public void List()
        {
            PageInfo info = LoadParam<PageInfo>();
            info.orderType = OrderType.DESC;
            info.OrderBy = "ID";
            var param = LoadParam<Dictionary<string, object>>();
            result.Data = domin.GetList(info, param, SessionUser.User.ID.Value);
            result.Succeeded = true;
        }


        /// <summary>
        /// 根据商品ID获取房间数量详情
        /// </summary>
        public void GetRoomDetail()
        {
            var param = LoadParam<Dictionary<string, object>>();
            Guid sGoodsId = new Guid(param["sGoodsId"].ToString());
            int year = param["year"].ToInt32();
            int month = param["month"].ToInt32();
            var RoomList = domin.GetRoomDetail(sGoodsId, year, month);
            result.Data = JSONHelper.GetJsonString<List<int>>(RoomList);
            result.Succeeded = true;
        }

    }
}