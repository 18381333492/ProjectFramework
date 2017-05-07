using Framework.BLL;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.DTO;
using Framework.Helper;
using Framework.AppCache;
using System.Linq;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 店铺财务控制器
    /// </summary>
    public class FinanceStoreController : SuperController
    {
        #region 视图

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.ID = SessionUser.User.ID;
            ViewBag.iStoreAviliableMoney = LoadInterface<IFinanceManager>().GetStoreAviliableMoney(SessionUser.User.ID);
            return View();
        }

        /// <summary>
        /// 是否可以提现
        /// </summary>
        public void IsWithdrawAble()
        {
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            Dictionary<string, object> dic = manager.GetWithdrawCash("店铺");
            //最新的提现记录
            EHECD_WithdrawCashDTO dto = manager.GetApplierInfo(SessionUser.User.ID);
            if (dto != null)
            {
                DateTime pretime = DateTime.Parse(dto.dApplyTime.Value.ToString("yyyy-MM-dd"));//上次的提现时间

                var now = DateTime.Now.ToString("yyyy-MM-dd");
                //求相差的时间间隔
                TimeSpan d3 = DateTime.Parse(now).Subtract(pretime);
                if (d3.Days > dic["iIntervalDays"].ToInt32())
                {
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = string.Format("每{0}天可提现一次", dic["iIntervalDays"].ToInt32() + 1);
                }
            }
            else {

                // 如果没有提现记录
                result.Msg = "现在为可以提现时间段，可以进行提现操作";
                result.Succeeded = true;

            }
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <returns></returns>
        public ActionResult Apply()
        {
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            EHECD_WithdrawCashDTO dto = manager.GetApplierInfo(SessionUser.User.ID);
            if (dto == null) {
                dto = new EHECD_WithdrawCashDTO();
            }
            ViewBag.iMinimumMoney = manager.GetWithdrawCash("店铺")["iMinimumMoney"];//最低提现额度
            ViewBag.iStoreAviliableMoney = LoadInterface<IFinanceManager>().GetStoreAviliableMoney(SessionUser.User.ID)["iprice"];
            ViewBag.Phone = SessionUser.User.sMobileNum;
            return View(dto);
        }

        #endregion

        #region 操作
        /// <summary>
        /// 店铺详情数据
        /// </summary>
        public void GetDetailStoreList()
        {
            var pageInfo = LoadParam<Dapper.PageInfo>();
            var tStoreAviliableMoney= LoadInterface<IFinanceManager>().GetStoreAviliableMoneyByTT(SessionUser.User.ID);
            IFinanceManager manager = LoadInterface<IFinanceManager>();
            Dapper.PagingRet<Dictionary<string, object>> list = manager.GetBackgroundStoreDetails(pageInfo, RequestParameters.dynamicData.data, SessionUser.User.ID.Value);
            bool flag = list != null;

            // --1提现，2充值，3退款，4购买 5收入佣金,6服务费,7订单收入,8.佣金支出9.服务费支出,10合伙人提成
            //统计总共付款的金额
            var totalPrices = list.Result.Where(m => m["iType"].ToInt32() == 4).Sum(m => m["iPrice"].ToDecimal());
            //统计总共退款的金额
            var returnPrices = list.Result.Where(m => m["iType"].ToInt32() == 3).Sum(m => m["iPrice"].ToDecimal());
            //统计总共提现的金额
            var drawPrices = list.Result.Where(m => m["iType"].ToInt32() == 1).Sum(m => m["iPrice"].ToDecimal());

            //统计服务费(只有收入的订单才有服务费)
            var totalServer = list.Result.Where(m=>m["iType"].ToInt32()==4).Sum(m => m["iServicePrice"].ToDecimal());
            //统计退款的服务费
            var returnServer = list.Result.Where(m => m["iType"].ToInt32() == 3).Sum(m => m["iServicePrice"].ToDecimal());

            totalServer = totalServer - returnServer;//实际的服务费

            totalPrices = totalPrices - returnPrices- drawPrices;//实际收入的价格

            result.Data = new Dictionary<string, object>() {
                { "list",list},
                { "tStoreAviliableMoney",tStoreAviliableMoney},
                { "totalPrices",totalPrices},
                { "totalServer",totalServer}
            };
            result.Msg = flag ? "获取成功" : "获取失败";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 申请提现
        /// </summary>
        public void DoApply()
        {
            var param = SessionUser.User.sLoginName;
            var data = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IFinanceManager>().GetStoreAviliableMoney(SessionUser.User.ID);
            if (ret["iprice"].ToDecimal() - ret["applayMoney"].ToDecimal() < data["iWithdrawMoney"].ToDecimal())
            {
                result.Succeeded = false;
                result.Msg = "余额不足";
            }
            //判断短信验证码是否正确
            else if (ApplicationCache.Instance.GetValue(param) != null)
            {
                var code = ApplicationCache.Instance.GetValue(param).ToString();
                if (code == data["storeSMS"].ToString())
                {
                    EHECD_WithdrawCashDTO dto = new EHECD_WithdrawCashDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        iSource = 2,
                        bIsDeleted = false,
                        dApplyTime = DateTime.Now,
                        iState = 0,
                        sWithdrawMemberID = SessionUser.User.ID,
                        sWithdrawMemberName = SessionUser.User.sLoginName,
                        sWithdrawMemberType = 0,
                        iWithdrawMoney = ConvertHelper.ToDecimal(data["iWithdrawMoney"]),
                        sBankCardNo = data["sBankCardNo"].ToString(),
                        sBankCardUserName = data["sBankCardUserName"].ToString(),
                        sBankName = data["sBankName"].ToString(),
                        sMethod = 0
                    };

                    IFinanceManager manager = LoadInterface<IFinanceManager>();
                    bool flag = manager.ApplyWithdraw(dto,SessionUser.User) > 0;
                    result.Data = null;
                    result.Msg = flag ? "申请成功" : "申请失败";
                    result.Succeeded = flag;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "很抱歉，您所输入的验证码错误，申请提现失败";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "短信验证码已过期，请重新获取";
            }
        }

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        public void SendMessage()
        {
            /*要获取短信验证码的登录名*/
            var param = SessionUser.User.sLoginName;
            var timeoutparam = SessionUser.User.sLoginName +"ttttt";

            //检查该账户在指定时间内是否已经获取过登录名
            if (ApplicationCache.Instance.GetValue(timeoutparam) != null)
            {
                result.Succeeded = false;
                result.Msg = "两分钟内已经获取过验证短信，请等待两分后再获取";
                return;
            }

            var login/*登录业务*/ = base.LoadInterface<ILogin>();
            var mnumber/*根据登录名获取到的电话号码（电话号是唯一的）*/ = login.QueryMobileNumberByLoginName(param);
            if (mnumber != null)
            {
                var messger/*短信发送接口*/ = base.LoadInterface<Validate.IMessager>();
                var code/*随机数字的字符码，默认是0-int最大值之间的4位字符，可以自己指定*/ = messger.RandomTool.GetRandomNumberString();
                if (messger.SendMessage(mnumber, string.Format("您正在执行提现操作，您的验证码为{0}，两分钟后将重新发送", code)))
                {
                    ApplicationCache.Instance.Delete(timeoutparam);
                    ApplicationCache.Instance.Delete(param);
                    ApplicationCache.Instance.SetValue(timeoutparam, code, 2*60);
                    ApplicationCache.Instance.SetValue(param, code, 10*60);
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "发送短消息至手机失败，请联系管理员";
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "没有获取到该登录名的联系电话，请确认登录名是否正确？";
            }
        }

        #endregion
    }
}