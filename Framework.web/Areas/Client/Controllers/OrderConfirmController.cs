using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Client.Controllers
{
    using Framework.BLL;
    using Framework.DTO;
    using Models;
    [ClientLoginFilter]
    public class OrderConfirmController : APISuperController
    {
        // GET: Client/OrderConfirm
        public ActionResult Index()
        {
            return View((EHECD_ClientDTO)GetSessionInfo("_client"));
        }

        /// <summary>
        /// 载入订单需要的数据
        /// </summary>
        public void LoadOrderInfo()
        {
            var bll = LoadInterface<IOrderConfirmManager>();
            var ret = bll.LoadOrderInfo(RequestParameters.dynamicData);
            result.Data = ret;
            result.Succeeded = true;
        }

        /// <summary>
        /// 载入优惠券
        /// </summary>
        public void LoadCoupons()
        {
            var bll = LoadInterface<IOrderConfirmManager>();
            var ret = bll.LoadCoupons((EHECD_ClientDTO)GetSessionInfo("_client"),RequestParameters.dynamicData);
            result.Data = ret;
            result.Succeeded = true;
        }


        /// <summary>
        /// 检查商品库存
        /// </summary>
        public void checkCount()
        {
            var bll = LoadInterface<IOrderConfirmManager>();
            bool res = bll.checkCount(RequestParameters.dynamicData);
            if (res)
            {
                result.Succeeded = true;
            }
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        public void MakeOrder()
        {
            var Client = (EHECD_ClientDTO)GetSessionInfo("_client");
            var bll = LoadInterface<IOrderConfirmManager>();

            var order = LoadParam<EHECD_OrdersDTO>();
            order.sClientID = Client.ID;
            order.sClientLoginName = Client.sLoginName;

            int res = bll.MakeOrder(order, RequestParameters.dynamicData);
            if (res > 0)
            {
                result.Data = new
                {
                    sOrderId = order.ID,
                    sOrderNo=order.sOrderNo,
                };
                result.Succeeded = true;
            }
            if (res == -1)
            {
                result.Msg = "房间数量不足,已被定完!";
            }
            if (res == -2)
            {
                result.Msg = "该票务数量不足,已被定完!";
            }
            if (res==-3)
            {
                result.Msg = "只能选择今天以后日期!";
            }
        }
    }
}