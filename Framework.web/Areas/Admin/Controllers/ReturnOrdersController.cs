using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 维权菜单管理
    /// </summary>
    public class ReturnOrdersController : SuperController
    {
        #region 视图
        /// <summary>
        /// 列表视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            return View();
        }

        /// <summary>
        /// 跳转到订单详情
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ActionResult Detail()
        {
            return View();
        }
        #endregion

        #region 操作
        /// <summary>
        /// 载入订单数据
        /// </summary>
        public void LoadReturnOrderData()
        {
            //获取上传参数
            var param = LoadParam<IDictionary<string, object>>();
            //设置分页参数
            var pageInfo = new PageInfo
            {
                OrderBy = "dInsertTime",
                orderType = OrderType.DESC,
                PageIndex = param["pageIndex"].ToInt32(),
                PageSize = param["pageSize"].ToInt32()
            };
            //判断当前的用户类型，加载对应的数据
            EHECD_SystemUserDTO user = SessionUser.User;
            byte? type = user.tUserType;
            IReturnOrderManager manager = LoadInterface<IReturnOrderManager>();
            PagingRet<IDictionary<string, object>> resultList = null;
            if (type == 0)
            {
                //平台用户
                resultList = manager.LoadReturnOrderData(pageInfo, null, param);
                if (resultList != null && resultList.MaxCount > 0)
                {
                    resultList.Result[0]["isStore"] = false;
                }
            }
            else if (type == 1)
            {
                //店铺用户
                resultList = manager.LoadReturnOrderData(pageInfo, user.ID, param);
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
        /// 编辑
        /// </summary>
        public void DoEdit()
        {
            var data=LoadParam<EHECD_ReturnOrdersDTO>();
            IReturnOrderManager manager = LoadInterface<IReturnOrderManager>();
            int res = manager.DoEdit(data);
            if (res > 0)
            {
                result.Succeeded = true;
                result.Msg = "操作成功!";
            }
            else
            {
                if (res == -2)
                {
                    result.Succeeded = false;
                    result.Msg = "该订单已核销!";
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "操作失败!";
                }
            }
            
        }

        /// <summary>
        /// 载入订单详情
        /// </summary>
        /// <param name="order"></param>
        public void LoadOrderDetailById()
        {
            var order = LoadParam<Dictionary<string,object>>();
            var bll = LoadInterface<IReturnOrderManager>();

            var ret = bll.LoadOrderDetailById(order);

            result.Data = ret;

            result.Succeeded = true;
        }
        #endregion
    }
}