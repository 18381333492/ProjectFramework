using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    public partial class OrderManager : IOrderManager
    {
        /// <summary>
        /// 分页查询订单数据
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="info">分页条件</param>
        /// <param name="param">查询条件</param>
        /// <returns>分页结果</returns>
        public override PagingRet<IDictionary<string, object>> LoadOrderData(PageInfo info, Guid? id, IDictionary<string, object> param)
        {
            info.orderType = OrderType.DESC;
            info.OrderBy = "dBookTime";
            var oStringBuilder = new StringBuilder();
            if (id == null)
            {
                //平台用户
                oStringBuilder.Append(@"SELECT
	                                    *
                                    FROM
	                                    (
		                                    SELECT
                                                a.ID,
			                                    a.sOrderNo,
			                                    a.sReceiver,
			                                    a.iTotalPrice,
			                                    a.iType,
			                                    --0客房 1门票 2周边产品
			                                    b.sShopName,
			                                    a.dBookTime,
			                                    a.iState --0待付款 1待使用 2-已核销
		                                   FROM
			                                  EHECD_Orders a
			                                  LEFT JOIN EHECD_ShopSet b
			                                  ON a.sStoreID= b.sShopID
		                                  WHERE 
		                                   a.bIsDeleted = 0 AND a.iState!=3
	                                    ) d WHERE 1=1");
            }
            else {
                //店铺用户
                oStringBuilder.Append(string.Format(@"SELECT
	                                                        *
                                                        FROM
	                                                        (
		                                                        SELECT
                                                                    a.ID,
			                                                        a.sStoreID,
			                                                        a.sOrderNo,
			                                                        a.sReceiver,
			                                                        a.iTotalPrice,
			                                                        a.iType,
			                                                        --0客房 1门票 2周边产品
			                                                        a.dBookTime,
			                                                        c.sPhone,
			                                                        a.iState --0待付款 1待使用 2-已核销
		                                                        FROM
			                                                        EHECD_Orders a
			                                                        LEFT JOIN EHECD_Client c
			                                                        ON a.sClientID=c.ID
		                                                        WHERE 
		                                                         a.bIsDeleted = 0 AND a.iState!=3
	                                                        ) d
                                                        WHERE
	                                                        sStoreID ='{0}' ", id));
            }

            

            //获取查询参数
            var sStartTime = Helper.CommonHelper.GetDictionaryValue("sTime", param, typeof(string));
            var sEndTime = Helper.CommonHelper.GetDictionaryValue("eTime", param, typeof(string));
            var sName = Helper.CommonHelper.GetDictionaryValue("sName", param, typeof(string));
            var iState = Helper.CommonHelper.GetDictionaryValue("iState", param, typeof(string));
            var sStoreWhere = Helper.CommonHelper.GetDictionaryValue("sStoreWhere", param, typeof(string));

            if (!string.IsNullOrEmpty(sStartTime.ToString())) oStringBuilder.AppendLine(string.Format(" AND dBookTime >= '{0}'", sStartTime.ToString()));
            if (!string.IsNullOrEmpty(sEndTime.ToString())) oStringBuilder.AppendLine(string.Format(" AND dBookTime <= '{0}'", sEndTime.ToString()));
            if (!string.IsNullOrEmpty(sName.ToString())) oStringBuilder.AppendLine(string.Format(" AND (sOrderNo LIKE '%{0}%' OR sReceiver LIKE '%{0}%' OR sShopName LIKE '%{0}%')", sName.ToString()));
            if (!string.IsNullOrEmpty(iState.ToString())&& iState.ToString()!="-1") oStringBuilder.AppendLine(string.Format(" AND iState={0} ",iState.ToString()));
            if (!string.IsNullOrEmpty(sStoreWhere.ToString())) oStringBuilder.Append(string.Format(" AND (sOrderNo LIKE '%{0}%' OR sPhone LIKE '%{0}%') ",sStoreWhere.ToString()));

            return query.PaginationQuery<IDictionary<string, object>>(oStringBuilder.ToString(), info, null);
        }

        /// <summary>
        /// 查询订单详情
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public override object LoadOrderDetailById(EHECD_OrdersDTO order)
        {
            var orderRet = query.SingleQuery<Dictionary<string,object>>(@"SELECT
	                                                                            *
                                                                            FROM
	                                                                            (
		                                                                            SELECT
			                                                                            a.ID,
			                                                                            a.sOrderNo,
			                                                                            a.iState,
			                                                                            a.sReceiver,
			                                                                            a.sReceiverPhone,
			                                                                            a.dPayTime,
			                                                                            a.dFinishTime,
			                                                                            a.dBookTime,
			                                                                            b.iUsePrice,
			                                                                            b.iCoiCouponPrice,
			                                                                            a.iTotalPrice,
			                                                                            a.sDescribe,
                                                                                        c.iState as returniState
		                                                                            FROM
			                                                                            EHECD_Orders a
		                                                                            LEFT JOIN EHECD_Coupon b ON a.sCouponID = b.ID
                                                                                    LEFT JOIN EHECD_ReturnOrders c ON a.ID = c.sOrderID   
	                                                                            ) d
                                                                            WHERE
	                                                                            ID = @ID ", new { order.ID});

            string sql = "";//0客房 1门票 2周边产品
            if (order.iType == 0)
            {
                //客房订单
                sql = string.Format(@"--客房
                                    SELECT
	                                    b.ID,
	                                    b.sGoodsName,
	                                    a.iSinglePrice*a.iAmount as iTotalPrice,
	                                    c.dStartTime,
	                                    c.sEndTime
                                    FROM
	                                    EHECD_OrdersGoods a,
	                                    EHECD_Goods b,
	                                    EHECD_RoomDetail c
                                    WHERE
	                                    a.sOrderID = '{0}'
                                    AND a.sGoodsPrimaryKey = b.ID
                                    AND c.sOrderId = a.sOrderID ", order.ID);
            }
            else if (order.iType == 1)
            {
                //票务
                sql = string.Format(@"--票务
                                        SELECT
	                                        c.sGoodsName,
	                                        a.iAmount,
	                                        a.iAmount * a.iSinglePrice iTotalPrice,
	                                        (
		                                        SELECT
			                                        TOP (1) CONVERT(VARCHAR(100),dStartTime,23) 
		                                        FROM
			                                        EHECD_RoomDetail
                                                WHERE
			                                        a.sOrderID = sOrderID
	                                        ) dStartTime
                                        FROM
	                                        EHECD_OrdersGoods a,
	                                        EHECD_Goods c
                                        WHERE
	                                        a.sGoodsPrimaryKey = c.ID
                                        AND a.sOrderID = '{0}'", order.ID);
            }
            else if(order.iType==2){
                //周边
                sql = string.Format(@"--周边
                                        SELECT
	                                        a.sGoodsName,
	                                        a.iAmount * a.iSinglePrice AS iTotalPrice
                                        FROM
	                                        EHECD_OrdersGoods a
                                        WHERE
	                                        a.sOrderID = '{0}' ", order.ID);
            }
            var orderGoodsRet = query.QueryList<Dictionary<string, object>>(sql, null);

            var ret = new Dictionary<string,object>
            {
                { "order",orderRet},
                { "orderGoods",orderGoodsRet}
            };

            return ret;
        }
    }
}
