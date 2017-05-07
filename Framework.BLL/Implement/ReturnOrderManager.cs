using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;

namespace Framework.BLL
{
    public class ReturnOrderManager : IReturnOrderManager
    {
        /// <summary>
        /// 编辑操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int DoEdit(EHECD_ReturnOrdersDTO dto)
        {
            StringBuilder builder = new StringBuilder();

            // 查询出这个退货订单的基本信息
            EHECD_ReturnOrdersDTO tempReturnOrder = query.SingleQuery<EHECD_ReturnOrdersDTO>(@"SELECT * FROM EHECD_ReturnOrders WHERE ID=@ID AND bIsDeleted=0", new { ID = dto.ID });

            // 查询出这个订单原始信息
            EHECD_OrdersDTO tempOrder = query.SingleQuery<EHECD_OrdersDTO>(@"SELECT * FROM EHECD_Orders WHERE bIsDeleted=0 AND ID=@ID", new { ID = tempReturnOrder.sOrderID });
            if (tempOrder.iState == 2)
            {
                return -2;//订单已核销
            }

            //查询原订单的订单商品
            EHECD_OrdersGoodsDTO orderGoods = query.SingleQuery<EHECD_OrdersGoodsDTO>(@"SELECT * FROM EHECD_OrdersGoods WHERE sOrderID=@sOrderID", new { sOrderID = tempReturnOrder.sOrderID });
            // 当前编辑是将状态变成退款成功
            if (dto.iState == 2 && tempOrder.iState == 3)
            {

                // 更新退款状态
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ReturnOrdersDTO>(dto, string.Format(" WHERE ID='{0}' ", dto.ID))).Append(";");

                // 查询出这个会员的信息(购买人)
                EHECD_ClientDTO tempClient = query.SingleQuery<EHECD_ClientDTO>(@"SELECT
	                                                        *
                                                        FROM
	                                                        [dbo].[EHECD_Client]
                                                        WHERE
	                                                        ID = @ID
                                                       ", new { ID = tempReturnOrder.sClientID });

                // 查询出这个商家的基本信息
                EHECD_SystemUserDTO tempUserInfo = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT
	                                                                                            *
                                                                                            FROM
	                                                                                            [dbo].[EHECD_SystemUser]
                                                                                            WHERE
	                                                                                            ID = @ID
                                                                                            AND tUserType = 1 --店铺
                                                                                            AND tUserState = 0 --正常
                                                                                            AND bIsDeleted = 0 --未删除", new { ID = tempOrder.sStoreID });

                // 记录余额变动（商家支出退款金额，会员收入退款金额）

                ////退款 itype=3
                //builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO()
                //{

                //    ID = Helper.GuidHelper.GetSecuentialGuid(),
                //    bIsDeleted = false,
                //    dChangeTime = DateTime.Now,
                //    iPrice = tempReturnOrder.iTotalMoney,
                //    iAfterPrice = tempUserInfo.iAccountNalance - tempReturnOrder.iTotalMoney,
                //    iBeforePrice = tempUserInfo.iAccountNalance,
                //    iClientType = 2,
                //    iCommissionPrice = 0.00M,
                //    iMethod = 1,//支出
                //    iServicePrice = 0.00M,
                //    iShopID = tempOrder.sStoreID,
                //    iType = 3,
                //    sClientID = tempClient.ID,
                //    sOrderID = tempOrder.ID,
                //    sOrderNo = tempOrder.sOrderNo,
                //    sRemark = "商家 支出 退款金额",
                //    sUserName= tempClient.sNickName,
                //    PartnerID= tempUserInfo.sPartnerID//退款的合伙人

                //})).Append(";");

                //// 商家 余额
                //builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO()
                //{

                //    ID = tempOrder.sStoreID,
                //    iAccountNalance= tempClient.dAccountBalance - tempReturnOrder.iTotalMoney                

                //}, string.Format(" WHERE ID='{0}' ", tempReturnOrder.ID))).Append(";");

                //查询原订单的收入明细
                var orderbancle = query.SingleQuery<EHECD_ClientBalanceDetailDTO>(@"SELECT * FROM EHECD_ClientBalanceDetail 
                                                                                 WHERE sOrderID=@sOrderID AND iType=4", new
                {
                    sOrderID = tempOrder.ID
                });

                // 退款 iType=3
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(new EHECD_ClientBalanceDetailDTO()
                {
                    ID = Helper.GuidHelper.GetSecuentialGuid(),
                    bIsDeleted = false,
                    dChangeTime = tempOrder.dBookTime,//订单的下单时间
                    iPrice = orderbancle.iPrice,//店铺的实际收入价格
                    iAfterPrice = tempClient.dAccountBalance - tempReturnOrder.iTotalMoney,
                    iBeforePrice = tempClient.dAccountBalance,
                    iClientType = (byte)tempClient.iClientType,
                    iCommissionPrice = orderbancle.iCommissionPrice,//实际的佣金
                    iMethod = 1,//支出
                    iServicePrice = orderbancle.iServicePrice,//服务费
                    iShopID = tempOrder.sStoreID,
                    iType = 3,
                    sClientID = tempClient.ID,
                    sOrderID = tempOrder.ID,
                    sOrderNo = tempOrder.sOrderNo,
                    sRemark = "会员收入 退款金额",
                    sUserName = tempOrder.sReceiver,
                    PartnerID = tempOrder.sPartnerID//订单的合伙人ID

                })).Append(";");

                //// 会员 余额
                //builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO()
                //{

                //    ID = tempClient.ID,
                //    dAccountBalance = tempClient.dAccountBalance + tempReturnOrder.iTotalMoney

                //}, string.Format(" WHERE ID='{0}' ", tempReturnOrder.ID))).Append(";");

            }
            else
            {
                // 更新退款状态
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ReturnOrdersDTO>(dto, string.Format(" WHERE ID='{0}' ", dto.ID))).Append(";");
            }

            return excute.ExcuteTransaction(builder.ToString());
        }




        /// <summary>
        /// 订单数据
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override object LoadOrderDetailById(Dictionary<string, object> order)
        {
            string id = order["ID"].ToString();
            int type = Helper.ConvertHelper.ToInt32(order["iType"].ToString());

            var orderRet = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	                                                                            *
                                                                            FROM
	                                                                            (
		                                                                            SELECT
			                                                                            o.ID,
			                                                                            a.sOrderNo,
			                                                                            o.iState,
			                                                                            a.sReceiver,
			                                                                            c.sPhone,
			                                                                            a.dPayTime,
			                                                                            a.dFinishTime,
			                                                                            a.dBookTime,
			                                                                            b.iUsePrice,
			                                                                            b.iCoiCouponPrice,
			                                                                            a.iTotalPrice,
			                                                                            a.sDescribe
		                                                                            FROM
			                                                                            EHECD_ReturnOrders o
		                                                                            LEFT JOIN EHECD_Orders a ON a.ID = o.sOrderID
		                                                                            LEFT JOIN EHECD_Client c ON a.sClientID = c.ID
		                                                                            LEFT JOIN EHECD_Coupon b ON a.sCouponID = b.ID
	                                                                            ) d
                                                                            WHERE
	                                                                            ID = @ID ", new { ID=id });

            string sql = "";//0客房 1门票 2周边产品
            if (type == 0)
            {
                //客房订单
                sql = string.Format(@"--客房
                                    SELECT
	                                    ro.ID,
	                                    b.sGoodsName,
	                                    a.iSinglePrice * a.iAmount AS iTotalPrice,
	                                    c.dStartTime,
	                                    c.sEndTime
                                    FROM
	                                    EHECD_ReturnOrders ro,
	                                    EHECD_OrdersGoods a,
	                                    EHECD_Goods b,
	                                    EHECD_RoomDetail c
                                    WHERE
	                                    ro.sOrderID = a.sOrderID
                                    AND a.sGoodsPrimaryKey = b.ID
                                    AND c.sOrderId = a.sOrderID
                                    AND ro.ID = '{0}' ", id);
            }
            else if (type == 1)
            {
                //票务
                sql = string.Format(@"--票务
                                        SELECT
	                                        c.sGoodsName,
	                                        a.iAmount,
	                                        a.iAmount * a.iSinglePrice iTotalPrice,
	                                        (
		                                        SELECT
			                                        TOP (1) dStartTime
		                                        FROM
			                                        EHECD_RoomDetail
		                                        WHERE
			                                        a.sOrderID = sOrderID
	                                        ) dStartTime
                                        FROM
	                                        EHECD_ReturnOrders ro,
	                                        EHECD_OrdersGoods a,
	                                        EHECD_Goods c
                                        WHERE
	                                        ro.sOrderID = a.sOrderID
                                        AND a.sGoodsPrimaryKey = c.ID
                                        AND ro.ID = '{0}'", id);
            }
            else if (type == 2)
            {
                //周边
                sql = string.Format(@"--周边
                                        SELECT
	                                        a.sGoodsName,
	                                        a.iAmount * a.iSinglePrice AS iTotalPrice
                                        FROM
	                                        EHECD_ReturnOrders ro,
	                                        EHECD_OrdersGoods a
                                        WHERE
	                                        ro.sOrderID = a.sOrderID
                                        AND ro.ID = '{0}' ", id);
            }
            var orderGoodsRet = query.QueryList<Dictionary<string, object>>(sql, null);

            var ret = new Dictionary<string, object>
            {
                { "order",orderRet},
                { "orderGoods",orderGoodsRet}
            };

            return ret;
        }

        /// <summary>
        /// 根据ID获取订单列表
        /// </summary>
        /// <param name="info"></param>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<IDictionary<string, object>> LoadReturnOrderData(PageInfo pageInfo, Guid? id, IDictionary<string, object> param)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dInsertTime";

            StringBuilder builder = new StringBuilder();

            builder.Append(@"SELECT
	                                *
                                FROM
	                                (
		                                SELECT
			                                b.ID,
			                                c.sStoreID,
			                                c.sOrderNo,
			                                a.sReturnNo,
			                                a.sGoodsName,
			                                b.dInsertTime,
			                                c.iTotalPrice,
			                                d.sShopName,
			                                b.sDescribe,
			                                c.iType,
			                                b.iState --0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款
		                                FROM
			                                EHECD_ReturnOrdersGoods a,
			                                EHECD_ReturnOrders b,
			                                EHECD_Orders c,
			                                EHECD_ShopSet d
		                                WHERE
			                                a.sReturnID = b.ID
		                                AND b.sOrderID = c.ID
		                                AND c.sStoreID = d.sShopID
                                        AND c.iState=3--原订单的状态必须是退款
	                                ) f
                                WHERE
	                                1 = 1 ");
            //获取查询的参数
            var iState = Helper.CommonHelper.GetDictionaryValue("iState",param,typeof(string));//0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款
            var dStartTime = Helper.CommonHelper.GetDictionaryValue("sTime",param,typeof(string));
            var dEndTime = Helper.CommonHelper.GetDictionaryValue("eTime",param,typeof(string));

            if (id != null) builder.Append(string.Format(@" AND sStoreID='{0}' ",id));//店铺用户
            if (!string.IsNullOrEmpty(iState.ToString())&&iState.ToString()!="-1") builder.Append(string.Format(@" AND iState={0} ",iState.ToString()));
            if (!string.IsNullOrEmpty(dStartTime.ToString())) builder.Append(string.Format(@" AND dInsertTime>='{0}' ", dStartTime.ToString()));
            if (!string.IsNullOrEmpty(dEndTime.ToString())) builder.Append(string.Format(@" AND dInsertTime<='{0}' ", dEndTime.ToString()));

            return query.PaginationQuery<IDictionary<string,object>>(builder.ToString(),pageInfo,null);
        }
    }
}
