using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiXin.Base.Pay.Lib;

namespace Framework.BLL
{
    /// <summary>
    /// 提现 【实现】
    /// author 王其
    /// </summary>
    public class WithCashManager : IWithCashManager
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public override PagingRet<EHECD_WithdrawCashDTO> GetList(Dapper.PageInfo pageInfo, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dApplyTime";
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format(@"SELECT
	                                            ID,
	                                            sWithdrawNumber,
	                                            CASE
                                            WHEN a.sWithdrawMemberType = 2 THEN
	                                            (
		                                            SELECT
			                                            c.sNickName
		                                            FROM
			                                            EHECD_Client c
		                                            WHERE
			                                            c.ID = a.sWithdrawMemberID
	                                            )
                                            ELSE
	                                            (
		                                            SELECT
			                                            s.sUserName
		                                            FROM
			                                            EHECD_SystemUser s
		                                            WHERE
			                                            s.ID = a.sWithdrawMemberID
	                                            )
                                            END sWithdrawMemberName,
                                             iWithdrawMoney,
                                             dApplyTime,
                                             iState
                                            FROM
	                                            EHECD_WithdrawCash AS a
                                            WHERE
	                                            bIsDeleted = 0"));
            //设置查询条件
            if (!string.IsNullOrEmpty(where.dApplyTimeStart.Value.ToString())&&!string.IsNullOrEmpty(where.dApplyTimeEnd.Value.ToString()))
            {
                //设置开始时间和结束时间
                builder.Append(string.Format(@" AND dApplyTime>='{0}'  AND dApplyTime<='{1}' ", where.dApplyTimeStart.Value.ToString(), where.dApplyTimeEnd.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.iState.Value.ToString())&& where.iState.Value.ToString()!="-1")
            {
                //设置状态
                builder.Append(string.Format(@" AND iState={0} ", where.iState.Value.ToString()));
            }
            //查询数据
            return query.PaginationQuery<EHECD_WithdrawCashDTO>(builder.ToString(),pageInfo,null);
        }

        /// <summary>
        /// 提现(-1：更新数据库用户信息失败 -2：微信转账失败 1：操作成功)
        /// </summary>
        /// <param name="dto">当前提现用户</param>
        /// <returns></returns>
        public override int DoWithdrawCash(EHECD_WithdrawCashDTO dto, out string message)
        {
            lock (this)
            {
                // 查询当前这个用户 在数据库中的状态和角色
                var tempDto = query.SingleQuery<EHECD_WithdrawCashDTO>(@"SELECT * FROM EHECD_WithdrawCash WHERE bIsDeleted=0 AND ID=@ID", new { ID = dto.ID });
                if (tempDto.iState == 2)
                {
                    message = "状态已为提现成功,不能再次操作!";
                    return -3;
                }
                StringBuilder builder = new StringBuilder();
                // 当前用户在数据库中的装态不是 【提现成功】状态,并且角色是分销客
                if (tempDto.sWithdrawMemberType == 2)
                {//分享客
                 // 查询分销客信息
                    var tempShareDto = query.SingleQuery<EHECD_ClientDTO>(@"SELECT
	                                                                        *
                                                                        FROM
	                                                                        EHECD_Client
                                                                        WHERE
	                                                                        ID = @ID
                                                                        AND bIsDeleted = 0 --未删除
                                                                        AND iClientType = 1 --分销客", new { ID = tempDto.sWithdrawMemberID });

                    // 如果当前操作是将用户状态修改为提现成功，则在线转账给提现人员
                    if (dto.iState == 2)
                    {
                        // 提现金额 至 当前账户
                        // 获取转账的基本信息，转账金额【上传金额默认是“分”为单位，但是微信设置最低转账金额为“1元”】
                        decimal tempAmount = Convert.ToDecimal((tempDto.iWithdrawMoney)) * 100;
                        int amount = Convert.ToInt32(tempAmount);

                        // 生成商户订单
                        string partner_trade_no = WxPayApi.GenerateOutTradeNo();
                        var isSuccess = WeiXin.Base.Pay.EnterprisePay.Run(partner_trade_no, tempShareDto.sOpenId.ToString(), tempShareDto.sNickName, amount, "提现成功", out message);

                        // 发送成功 
                        if (isSuccess)
                        {
                            // 查询当分销客提现的店铺----------------

                            // 则更新数据,并做余额变动

                            // 更新提现记录状态
                            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID))).Append(";");

                            // 插入余额变动记录，分销客提现
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO()
                            {

                                ID = Helper.GuidHelper.GetSecuentialGuid(),
                                bIsDeleted = false,
                                dChangeTime = DateTime.Now,
                                iBeforePrice = tempShareDto.dAccountBalance,
                                iAfterPrice = tempShareDto.dAccountBalance - tempDto.iWithdrawMoney,
                                iClientType = 1,
                                iCommissionPrice = 0.00M,
                                iMethod = 1,
                                iPrice = tempDto.iWithdrawMoney,
                                iServicePrice = 0.00M,
                                iType = 1,
                                sClientID = tempShareDto.ID,
                                sUserName = tempShareDto.sLoginName,
                                sOrderID = tempDto.ID,
                                sOrderNo = tempDto.sWithdrawNumber,
                                sRemark = tempDto.sRemark,
                                iShopID = null

                            })).Append(";");
                            return excute.ExcuteTransaction(builder.ToString()) > 0 ? 1 : -1;
                        }
                        else
                        {
                            // 发送失败，不更新数据
                            // message = "微信转账操作失败";
                            return -2;
                        }
                    }
                    else
                    {
                        // 如果当前操作不是将状态修改为提现成功，则直接改变用户状态
                        int resCode1 = excute.UpdateSingle<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID)) > 0 ? 1 : -1;
                        message = resCode1 == 1 ? "更新用户状态成功" : "更新用户状态失败";
                        return resCode1;
                    }

                }
                else
                {
                    // 当前用户在数据库中的装态是 【提现成功】状态或则角色不是分销客
                    // 则不需用在线转账 直接改变用户信息和状态
                    // 则更新数据

                    // 查询店铺和合伙人信息
                    var tempStoreAndPartnerDto = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT
	                                                                        *
                                                                        FROM
	                                                                        EHECD_SystemUser
                                                                        WHERE
	                                                                        ID = @ID
                                                                        AND bIsDeleted = 0 --未删除", new { ID = tempDto.sWithdrawMemberID });


                    if (tempDto.sWithdrawMemberType == 0)
                    {//店铺
                        if (tempDto.iState == 1 && dto.iState == 2)
                        {//线下转账
                         // 插入余额变动记录,店铺提现
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO()
                            {
                                ID = Helper.GuidHelper.GetSecuentialGuid(),
                                bIsDeleted = false,
                                dChangeTime = DateTime.Now,
                                iBeforePrice = tempStoreAndPartnerDto.iAccountNalance,
                                iAfterPrice = tempStoreAndPartnerDto.iAccountNalance - tempDto.iWithdrawMoney,
                                // 提现用户类型
                                iClientType = 2,//店铺
                                iCommissionPrice = 0.00M,
                                iMethod = 1,
                                iPrice = tempDto.iWithdrawMoney,
                                iServicePrice = 0.00M,
                                iType = 1,
                                sClientID = tempStoreAndPartnerDto.ID,
                                sUserName = tempStoreAndPartnerDto.sLoginName,
                                sOrderID = tempDto.ID,
                                sOrderNo = tempDto.sWithdrawNumber,
                                sRemark = tempDto.sRemark,
                                iShopID = tempStoreAndPartnerDto.ID,
                                PartnerID = null
                            })).Append(";");

                            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID)));

                            int res = excute.ExcuteTransaction(builder.ToString()) > 0 ? 1 : -1;
                            message = res == 1 ? "更新用户状态成功" : "更新用户状态失败";
                            return res;
                        }
                        else
                        {//修改状态
                         // 如果当前操作不是将状态修改为提现成功，则直接改变用户状态
                            int resCode1 = excute.UpdateSingle<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID)) > 0 ? 1 : -1;
                            message = resCode1 == 1 ? "更新用户状态成功" : "更新用户状态失败";
                            return resCode1;
                        }
                    }
                    else
                    {//合伙人
                        if (tempDto.iState == 1 && dto.iState == 2)
                        {//线下转账
                         // 插入余额变动记录,店铺提现
                            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO()
                            {
                                ID = Helper.GuidHelper.GetSecuentialGuid(),
                                bIsDeleted = false,
                                dChangeTime = DateTime.Now,
                                iBeforePrice = tempStoreAndPartnerDto.iAccountNalance,
                                iAfterPrice = tempStoreAndPartnerDto.iAccountNalance - tempDto.iWithdrawMoney,
                                // 提现用户类型
                                iClientType = 3,//店铺
                                iCommissionPrice = 0.00M,
                                iMethod = 1,
                                iPrice = tempDto.iWithdrawMoney,
                                iServicePrice = 0.00M,
                                iType = 1,
                                sClientID = tempStoreAndPartnerDto.ID,
                                sUserName = tempStoreAndPartnerDto.sLoginName,
                                sOrderID = tempDto.ID,
                                sOrderNo = tempDto.sWithdrawNumber,
                                sRemark = tempDto.sRemark,
                                iShopID = null,
                                PartnerID = tempStoreAndPartnerDto.ID
                            })).Append(";");

                            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID)));

                            int res = excute.ExcuteTransaction(builder.ToString()) > 0 ? 1 : -1;
                            message = res == 1 ? "更新用户状态成功" : "更新用户状态失败";
                            return res;
                        }
                        else
                        {//修改状态
                         // 如果当前操作不是将状态修改为提现成功，则直接改变用户状态
                            int resCode1 = excute.UpdateSingle<EHECD_WithdrawCashDTO>(dto, string.Format(" where ID='{0}' ", dto.ID)) > 0 ? 1 : -1;
                            message = resCode1 == 1 ? "更新用户状态成功" : "更新用户状态失败";
                            return resCode1;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override EHECD_WithdrawCashDTO GetSingle(Guid? id)
        {
            return query.SingleQuery<EHECD_WithdrawCashDTO>(@"SELECT
	                                                                TOP (1) ID,
	                                                                sWithdrawNumber,
	                                                                CASE
                                                                WHEN a.sWithdrawMemberType = 2 THEN
	                                                                (
		                                                                SELECT
			                                                                c.sNickName
		                                                                FROM
			                                                                EHECD_Client c
		                                                                WHERE
			                                                                c.ID = a.sWithdrawMemberID
	                                                                )
                                                                ELSE
	                                                                (
		                                                                SELECT
			                                                                s.sUserName
		                                                                FROM
			                                                                EHECD_SystemUser s
		                                                                WHERE
			                                                                s.ID = a.sWithdrawMemberID
	                                                                )
                                                                END sWithdrawMemberName,
                                                                 iWithdrawMoney,
                                                                 iState,
                                                                 sBankCardNo,
                                                                 sBankCardUserName,
                                                                 sBankName,
                                                                 sRemark,
                                                                 sWithdrawMemberType
                                                                FROM
	                                                                EHECD_WithdrawCash AS a
                                                                WHERE
	                                                                bIsDeleted = 0
                                                                AND ID =@ID", new { ID= id });
        }

        /// <summary>
        /// 获取设置列表数据
        /// </summary>
        /// <returns></returns>
        public override IList<EHECD_WithdrawSettingDTO> GetSettingList()
        {
            return query.QueryList<EHECD_WithdrawSettingDTO>(@" SELECT * FROM EHECD_WithdrawSetting", null);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public override int DoSetting(List<EHECD_WithdrawSettingDTO> list)
        {
            StringBuilder builder = new StringBuilder();
            //构造SQL语句
            foreach (var item in list)
            {
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_WithdrawSettingDTO>(item, string.Format(@" WHERE ID='{0}' ",item.ID))).Append(";");
            }

            return excute.ExcuteTransaction(builder.ToString());
        }
    }
}
