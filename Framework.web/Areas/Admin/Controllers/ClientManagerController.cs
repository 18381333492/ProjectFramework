using Framework.BLL;
using Framework.Dapper;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;
using Framework.DTO;

namespace Framework.web.Areas.Admin.Controllers
{
    public class ClientManagerController : SuperController
    {
        // GET: Admin/ClientManager
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 载入用户数据
        /// </summary>
        public void LoadClients()
        {
            var param = LoadParam<Dictionary<string, object>>();

            var bll = LoadInterface<IClientManager>();

            PageInfo info = null;

            if (param["sortable"].ToBoolean())
            {
                info = new PageInfo()
                {
                    OrderBy = "iTotalPrice",
                    orderType = param["sort"].ToInt32() == 0 ? OrderType.ASC : OrderType.DESC,
                    PageIndex = param["PageIndex"].ToInt32(),
                    PageSize = param["pageSize"].ToInt32()
                };
            }
            else
            {
                info = new PageInfo()
                {
                    OrderBy = "dAddTime",
                    orderType = OrderType.DESC,
                    PageIndex = param["PageIndex"].ToInt32(),
                    PageSize = param["pageSize"].ToInt32()
                };
            }

            result.Data = bll.LoadClient(info, param);

            result.Succeeded = true;
        }

        /// <summary>
        /// 冻结/解冻客户
        /// </summary>
        public void ForzenClients()
        {
            var param = LoadParam<List<EHECD_ClientDTO>>();

            var bll = LoadInterface<IClientManager>();

            result.Succeeded = bll.ForzenClients(param, SessionUser.User);

            result.Msg = result.Succeeded ? "" : "抱歉，冻结/解冻操作失败，请联系管理员！";
        }

        /// <summary>
        /// 载入会员详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public PartialViewResult ClientDetail(EHECD_ClientDTO param)
        {            

            var bll = LoadInterface<IClientManager>();

            param = bll.QueryClientInfo(param);

            if (param.iClientType == 0)
            {
                return PartialView("NormalClient", param);
            }
            else
            {
                return PartialView("SharedClient", param);
            }
        }

        /// <summary>
        /// 载入普通会员订单
        /// </summary>
        public void LoadClientOrders()
        {
            var param = LoadParam<Dictionary<string, object>>();
            
            var info = new PageInfo()
            {
                OrderBy = "dBookTime",
                orderType = OrderType.DESC,
                PageIndex = param["PageIndex"].ToInt32(),
                PageSize = param["pageSize"].ToInt32()
            };

            var bll = LoadInterface<IClientManager>();

            result.Data = bll.LoadClientOrders(info, Guid.Parse(param["ID"].ToString()));

            result.Succeeded = true;
        }

        /// <summary>
        /// 订单详情页面视图
        /// </summary>
        /// <returns></returns>
        public ActionResult OrderDetail()
        {
            return View();
        }


        /// <summary>
        /// 载入分享的商品
        /// </summary>
        public void LoadSharedGoods()
        {
            var param = LoadParam<Dictionary<string, object>>();

            var info = new PageInfo()
            {
                OrderBy = "dInsertTime",
                orderType = OrderType.DESC,
                PageIndex = param["PageIndex"].ToInt32(),
                PageSize = param["pageSize"].ToInt32()
            };

            var bll = LoadInterface<IClientManager>();

            result.Data = bll.LoadSharedGoods(info, Guid.Parse(param["ID"].ToString()));

            result.Succeeded = true;
        }
    }
}