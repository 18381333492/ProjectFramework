using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DTO;
using Framework.web.Controllers;
using Framework.Dapper;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 提现管理
    /// Author 王其
    /// </summary>
    public class WithdrawCashController : SuperController
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
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            string id = Request.QueryString["ID"];
            IWithCashManager manager = LoadInterface<IWithCashManager>();
            EHECD_WithdrawCashDTO modal = manager.GetSingle(new Guid(id));
            return View(modal);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            string id = Request.QueryString["ID"];
            IWithCashManager manager = LoadInterface<IWithCashManager>();
            EHECD_WithdrawCashDTO modal = manager.GetSingle(new Guid(id));
            return View(modal);
        }

        /// <summary>
        /// 提现设置页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Setting()
        {
            IWithCashManager manager = LoadInterface<IWithCashManager>();
            IList<EHECD_WithdrawSettingDTO> list = manager.GetSettingList();
            return View(list);
        }

        #endregion

        #region 操作
        /// <summary>
        /// 查询数据
        /// </summary>
        public void LoadData()
        {
            //获取分页数据
            var pageInfo = LoadParam<Dapper.PageInfo>();
            IWithCashManager manager = LoadInterface<IWithCashManager>();
            PagingRet<EHECD_WithdrawCashDTO> resultList = resultList = manager.GetList(pageInfo, RequestParameters.dynamicData.data);
            if (resultList == null)
            {
                result.Data = "";
                result.Msg = "获取失败";
                result.Succeeded = false;
            }
            else
            {
                result.Data = resultList;
                result.Msg = "获取成功";
                result.Succeeded = true;
            }
            
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public void DoEdit()
        {
            var data = LoadParam<EHECD_WithdrawCashDTO>();
            IWithCashManager manager = LoadInterface<IWithCashManager>();
            string message = string.Empty;
            int res = manager.DoWithdrawCash(data, out message);
            result.Data = "";
            result.Msg = message;
            result.Succeeded = res == 1;

        }

        /// <summary>
        /// 设置
        /// </summary>
        public void DoSetting()
        {
            var data = LoadParam<Dictionary<string, object>>();
            List<EHECD_WithdrawSettingDTO> list = new List<EHECD_WithdrawSettingDTO>();
            //string[] ids = JSONHelper.GetJsonString(data["ID"]).Replace("[\"", "").Replace("\"]", "").Replace("\"", "").Split(',');
            //string[] iMinimumMoneys = JSONHelper.GetJsonString(data["iMinimumMoney"]).Replace("[\"", "").Replace("\"]", "").Replace("\"", "").Split(',');
            //string[] iIntervalDays = JSONHelper.GetJsonString(data["iIntervalDays"]).Replace("[\"", "").Replace("\"]", "").Replace("\"", "").Split(',');
            List<string> ids = JSONHelper.GetModel<List<string>>(data["ID"].ToString());
            List<string> iMinimumMoneys = JSONHelper.GetModel<List<string>>(data["iMinimumMoney"].ToString()); 
            List<string> iIntervalDays = JSONHelper.GetModel<List<string>>(data["iIntervalDays"].ToString());
            for (int i=0;i< ids.Count; i++)
            {
                EHECD_WithdrawSettingDTO dto = new EHECD_WithdrawSettingDTO();
                dto.ID = new Guid(ids[i]);
                dto.iMinimumMoney = ConvertHelper.ToDecimal(iMinimumMoneys[i]);
                dto.iIntervalDays = ConvertHelper.ToInt32(iIntervalDays[i]);
                list.Add(dto);
                dto = null;
            }
            IWithCashManager manager=LoadInterface<IWithCashManager>();
            bool flag=manager.DoSetting(list)>0;
            result.Data = "";
            result.Msg = flag ? "设置成功" : "设置失败";
            result.Succeeded = flag;
        }

        #endregion

        #region 返回结果处理

        /// <summary>
        /// 微信转账回调
        /// </summary>
        public void ResultNotify()
        {
            WeiXin.Base.Pay.ResultNotify notify = new WeiXin.Base.Pay.ResultNotify();
            notify.UpdateOrderStateEvent += Notify_UpdateOrderStateEvent;
            notify.ProcessNotify();
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="out_trade_id"></param>
        /// <returns></returns>
        private bool Notify_UpdateOrderStateEvent(string out_trade_id, string transaction_id, string total_fee, string time_end)
        {
            int i = 0;
            return true;
        }
        #endregion
    }
}