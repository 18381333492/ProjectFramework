using Framework.BLL;
using Framework.Dapper;
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
    /// <summary>
    /// 平台用户控制器
    /// author 王其
    /// </summary>
    public class FinanceBackgroundController : SuperController
    {
        #region 视图

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 店铺详情
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailStore(Guid ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        /// <summary>
        /// 合伙人详情
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailPartner(Guid ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        /// <summary>
        /// 分享客详情
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailShare(Guid  ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        #endregion

        #region 操作
        /// <summary>
        /// 查询数据
        /// </summary>
        public void LoadData()
        {
            //获取分页数据(平台用户)
            var pageInfo = LoadParam<Dapper.PageInfo>();
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            decimal iServicePrice = -1.00M;
            Dapper.PagingRet<Dictionary<string,object>> list=manager.GetFinanceBackgroundListByTT(pageInfo, RequestParameters.dynamicData.data,out iServicePrice);
            result.Data = list;
            result.Succeeded = true;
            result.Msg = iServicePrice.ToString();
        }

        /// <summary>
        /// 店铺详情数据
        /// </summary>
        public void GetDetailStoreList() {
            var pageInfo = LoadParam<Dapper.PageInfo>();
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            Dapper.PagingRet<Dictionary<string, object>> list = manager.GetBackgroundStoreDetails(pageInfo, RequestParameters.dynamicData.data);

            // --1提现，2充值，3退款，4购买 5收入佣金,6服务费,7订单收入,8.佣金支出9.服务费支出,10合伙人提成
            //统计实际收入的金额
            var totalPrices = list.Result.Where(m => m["iType"].ToInt32() == 4).Sum(m => m["iPrice"].ToDecimal());
            //统计总共退款的金额
            var returnPrices = list.Result.Where(m => m["iType"].ToInt32() == 3).Sum(m => m["iPrice"].ToDecimal());
            //统计总共提现的金额
            var drawPrices = list.Result.Where(m => m["iType"].ToInt32() == 1).Sum(m => m["iPrice"].ToDecimal());

            //统计服务费(只有收入的订单才有服务费)
            var totalServer = list.Result.Where(m => m["iType"].ToInt32() == 4).Sum(m => m["iServicePrice"].ToDecimal());
            //统计退款的服务费
            var returnServer = list.Result.Where(m => m["iType"].ToInt32() == 3).Sum(m => m["iServicePrice"].ToDecimal());

            totalServer = totalServer - returnServer;//实际的服务费
          
            totalPrices = totalPrices - returnPrices - drawPrices;//实际收入的价格

            result.Data = new Dictionary<string, object>() {
                { "list",list},
                { "totalPrices",totalPrices},
                { "totalServer",totalServer}
            };
            result.Succeeded = true;
        }

        /// <summary>
        /// 分享客详情数据
        /// </summary>
        public void GetDetailShareList()
        {
            var pageInfo = LoadParam<Dapper.PageInfo>();
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            Dapper.PagingRet<Dictionary<string, object>> list = manager.GetBackgroundShareDetails(pageInfo, RequestParameters.dynamicData.data);
            bool flag = list != null;
            result.Data = list;
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 合伙人详情
        /// </summary>
        public void GetDetailPartnerList()
        {
            var pageInfo = LoadParam<Dapper.PageInfo>();
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            Dapper.PagingRet<Dictionary<string, object>> list = manager.GetBackgroundPartnerDetails(pageInfo, RequestParameters.dynamicData.data);

            //获取基本设置里面的合伙人提成比例
            var domin = LoadInterface<IBaseSettingManager>();
            var setting = domin.GetBaseSetting();

            foreach (var m in list.Result)
            {
                if (m["iType"].ToInt32() == 4)
                {
                    m["iPrice"] = m["iServicePrice"].ToDecimal() * setting.iPartnerCommissionPrecent * 0.01.ToDecimal();
                }
                if(m["iType"].ToInt32() == 3)
                {
                    m["iPrice"] = (m["iServicePrice"].ToDecimal() * setting.iPartnerCommissionPrecent * 0.01.ToDecimal())*-1.ToDecimal();
                }
            }
            //// --1提现，2充值，3退款，4购买 5收入佣金,6服务费,7订单收入,8.佣金支出9.服务费支出,10合伙人提成
            ////统计总共付款的金额
            //var totalPrices = list.Result.Where(m => m["iType"].ToInt32() == 4).Sum(m => m["iPrice"].ToDecimal());
            ////统计总共退款的金额
            //var returnPrices = list.Result.Where(m => m["iType"].ToInt32() == 3).Sum(m => m["iPrice"].ToDecimal());

            ////统计服务费(只有收入的才有服务费)
            //var totalServer = list.Result.Where(m => m["iType"].ToInt32() == 4).Sum(m => m["iServicePrice"].ToDecimal());
            ////统计实际支付佣金
            //var totaliCommission = list.Result.Sum(m => m["iCommissionPrice"].ToDecimal());

            result.Data = result.Data = new Dictionary<string, object>() {
                { "list",list}
            };
            result.Succeeded = true;
        }

        #endregion
    }
}