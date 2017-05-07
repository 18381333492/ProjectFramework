using Framework.BLL;
using Framework.Dapper;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{

    /// <summary>
    /// 统计分析控制器
    /// </summary>
    public class CountAnalysisController : SuperController
    {
        // GET: Admin/RoomManage

        private ICountAnalysisManager domin;

        public CountAnalysisController()
        {
            domin = LoadInterface<ICountAnalysisManager>();
        }

        //用户类型 0：平台用户，1：店铺，2：合伙人

        #region 视图

        /// <summary>
        /// 订单统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Order()
        {
            ViewBag.tUserType = SessionUser.User.tUserType.ToInt32();
            return View();
        }


        /// <summary>
        /// 销售金额统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Sales()
        {
            ViewBag.tUserType = SessionUser.User.tUserType.ToInt32();
            return View();
        }

        /// <summary>
        /// 退货统计
        /// </summary>
        /// <returns></returns>
        public ActionResult Return()
        {
            ViewBag.tUserType = SessionUser.User.tUserType.ToInt32();
            return View();
        }

        #endregion

      
        /// <summary>
        /// 获取订单数据列表
        /// </summary>
        public void OrderList()
        {
            var PageInfo = LoadParam<PageInfo>();
            PageInfo.orderType = OrderType.DESC;
            var param = LoadParam<Dictionary<string, object>>();
            int tUserType = SessionUser.User.tUserType.ToInt32();
            var sStoreId = SessionUser.User.ID;
            var data = string.Empty;
            result.Succeeded = true;
            result.Data = domin.GetOrderList(PageInfo, param, tUserType, sStoreId,out data);
            result.Msg = data;
        }


        /// <summary>
        /// 获取销售数据列表
        /// </summary>
        public void SalesList()
        {
            var PageInfo = LoadParam<PageInfo>();
            PageInfo.orderType = OrderType.DESC;
            var param = LoadParam<Dictionary<string, object>>();
            int tUserType = SessionUser.User.tUserType.ToInt32();
            var sStoreId = SessionUser.User.ID;
            var data = string.Empty;
            result.Succeeded = true;
            result.Data = domin.GetSalesList(PageInfo, param, tUserType, sStoreId,out data);
            result.Msg = data;
        }


        /// <summary>
        /// 获取退货数据列表
        /// </summary>
        public void ReturnList()
        {
            var PageInfo = LoadParam<PageInfo>();
            PageInfo.orderType = OrderType.DESC;
            var param = LoadParam<Dictionary<string, object>>();
            int tUserType = SessionUser.User.tUserType.ToInt32();
            var sStoreId = SessionUser.User.ID;
            var data = string.Empty;
            result.Succeeded = true;
            result.Data = domin.GetReturnList(PageInfo, param, tUserType, sStoreId, out data);
            result.Msg = data;
        }
    }
}