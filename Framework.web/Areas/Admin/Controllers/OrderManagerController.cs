using Framework.BLL;
using Framework.Dapper;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;
using Framework.DTO;


namespace Framework.web.Areas.Admin.Controllers
{
    public class OrderManagerController : SuperController
    {
        #region 视图
        /// <summary>
        /// 列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.RoleType = SessionUser.User.tUserType;
            return View();
        }
        #endregion

        #region 操作
        /// <summary>
        /// 载入订单数据
        /// </summary>
        public void LoadOrderData()
        {
            //获取上传参数
            var param = LoadParam<IDictionary<string, object>>();
            //设置分页参数
            var pageInfo = new PageInfo
            {
                OrderBy = "dBookTime",
                orderType = OrderType.DESC,
                PageIndex = param["PageIndex"].ToInt32(),
                PageSize = param["PageSize"].ToInt32()
            };
            //判断当前的用户类型，加载对应的数据
            EHECD_SystemUserDTO user = SessionUser.User;
            byte? type = user.tUserType;
            IOrderManager manager = LoadInterface<IOrderManager>();
            PagingRet<IDictionary<string, object>> resultList = null;
            if (type == 0)
            {
                //平台用户
                resultList = manager.LoadOrderData(pageInfo, null, param);
                if (resultList != null && resultList.MaxCount > 0)
                {
                    resultList.Result[0]["isStore"] = false;
                }
            }
            else if (type == 1)
            {
                //店铺用户
                resultList = manager.LoadOrderData(pageInfo, user.ID, param);
                if (resultList != null && resultList.MaxCount > 0)
                {
                    resultList.Result[0]["isStore"] = true;
                }
            }
            else if (type == 2)
            {
                //合伙人用户（合伙人暂时没有优惠劵功能）
                result.Data = "";
                result.Msg = "该类型用户没有此功能";
                result.Succeeded = false;
                return;
            }
            else
            {
                //没有这种类型
                result.Data = "";
                result.Msg = "数据库中没有这种类型";
                result.Succeeded = false;
                return;
            }

            //返回数据
            if (resultList == null)
            {
                result.Data = "";
                result.Msg = "未获取到数据";
                result.Succeeded = false;
                return;
            }

            result.Data = resultList;
            result.Msg = "获取成功";
            result.Succeeded = true;
        }

        /// <summary>
        /// 跳转到订单详情
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public PartialViewResult ToOrderDetail()
        {
            return PartialView("OrderDetail");
        }

        /// <summary>
        /// 载入订单详情
        /// </summary>
        /// <param name="order"></param>
        public void LoadOrderDetailById()
        {
            var order = LoadParam<EHECD_OrdersDTO>();
            var bll = LoadInterface<IOrderManager>();

            var ret = bll.LoadOrderDetailById(order);

            result.Data = ret;

            result.Succeeded = true;
        } 
        #endregion
    }
}