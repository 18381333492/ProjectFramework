using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;
using Framework.Helper;

namespace Framework.BLL
{

    /// <summary>
    /// 房态管理的业务处理
    /// </summary>
    public class CountAnalysisManager : ICountAnalysisManager
    {


        /// <summary>
        /// 分页获取订单统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string,object>> GetOrderList(PageInfo page, Dictionary<string, object> where,int tUserType,Guid? sStoreID, out string data)
        {
            data = string.Empty;
            string sKeyWord= where.ContainsKey("sKeyWord") ? where["sKeyWord"].ToString() : string.Empty;
            int iState = where.ContainsKey("iState") ? where["iState"].ToInt32() : -1;
            string dStartTime = where.ContainsKey("dStartTime")? where["dStartTime"].ToString() : string.Empty;
            string dEndTime = where.ContainsKey("dEndTime")? where["dEndTime"].ToString() : string.Empty;
            StringBuilder sql = new StringBuilder();
            //订单统计数量和金额的SQL的语句
            StringBuilder CountSql = new StringBuilder();
            switch (tUserType)
            {
                case 0:
                    {//总后台
                        page.OrderBy = "dBookTime";
                        page.orderType = OrderType.DESC;
                        sql.Append(@"SELECT 
                                    A.ID,
                                    A.sOrderNo,
                                    A.dBookTime,     
                                    A.iTotalPrice,
                                    A.iState,
                                    A.sReceiver,
                                    B.sShopName
                                    FROM EHECD_Orders AS A LEFT JOIN 
                                    EHECD_ShopSet AS B ON  A.sStoreID=B.sShopID Where 1=1 AND A.iState<3");

                        CountSql.Append(@"  select count(*) as Number ,SUM(iTotalPrice) AS dPrices 
                                            from EHECD_Orders AS A 
                                            LEFT JOIN dbo.EHECD_ShopSet AS B 
                                            ON A.sStoreID=B.sShopID
                                            where iState<3 AND 1=1");
                        break;
                    }
                case 1:
                    {//店铺后台
                        page.OrderBy = "dBookTime";
                        page.orderType = OrderType.DESC;
                        sql.AppendFormat(@"SELECT 
                                    ID,
                                    sOrderNo,
                                    dBookTime,     
                                    iTotalPrice,
                                    iState,
                                    sReceiver
                                    FROM EHECD_Orders 
                                    WHERE iState<3 AND sStoreID='{0}'", sStoreID.ToString());

                        CountSql.AppendFormat("select count(*) as Number ,SUM(iTotalPrice) AS dPrices from EHECD_Orders where iState<3 AND sStoreID='{0}'", sStoreID.ToString());
                        break;
                    }
                case 2:
                    {//合伙人后台
                        page.OrderBy = "dBookTime";
                        page.orderType = OrderType.DESC;
                        sql.AppendFormat(@"SELECT 
                                    A.ID,
                                    A.sOrderNo,
                                    A.dBookTime,     
                                    A.iTotalPrice,
                                    A.iState,
                                    A.sReceiver,
                                    A.sStoreID,
                                    B.sShopName    
                                    FROM EHECD_Orders AS A
									LEFT JOIN EHECD_ShopSet AS B
									ON B.sShopID=A.sStoreID
									WHERE A.iState<3
								    AND  A.bIsDeleted=0
									AND  A.sPartnerID='{0}' ", sStoreID.ToString());

                        CountSql.AppendFormat(@"SELECT COUNT(*) AS Number,SUM(iTotalPrice) AS dPrices 
                                    FROM EHECD_Orders AS A
									LEFT JOIN EHECD_ShopSet AS B
									ON B.sShopID=A.sStoreID
									WHERE A.iState<3
								    AND  A.bIsDeleted=0
									AND  A.sPartnerID='{0}'", sStoreID.ToString());
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(sKeyWord))
            {//状态查询
                sql.AppendFormat(" AND B.sShopName LIKE '%{0}%' ", sKeyWord);

                CountSql.AppendFormat(" AND B.sShopName LIKE '%{0}%' ", sKeyWord);
            }

            if (iState >= 0)
            {//状态查询
                sql.AppendFormat(" AND iState={0} ", iState);
                CountSql.AppendFormat(" AND iState={0} ", iState);
            }
            if (!string.IsNullOrEmpty(dStartTime))
            {//开始时间
                dStartTime = DateTime.Parse(dStartTime).ToString("yyyy-MM-dd")+" 00:00:00";
                sql.AppendFormat(" AND dBookTime>='{0}' ", dStartTime);
                CountSql.AppendFormat(" AND dBookTime>='{0}' ", dStartTime);
            }
            if (!string.IsNullOrEmpty(dEndTime))
            {//结束时间
                dEndTime = DateTime.Parse(dEndTime).ToString("yyyy-MM-dd") + " 23:59:59";
                sql.AppendFormat(" AND dBookTime<='{0}' ", dEndTime);
                CountSql.AppendFormat(" AND dBookTime<='{0}' ", dEndTime);
            }
            var entry = query.SingleQuery<Dictionary<string, object>>(CountSql.ToString(), null);
            if (entry.ContainsKey("Number")&&entry["Number"] != null)
            {
                data = data+ entry["Number"].ToString()+",";
            }
            if (entry.ContainsKey("dPrices")&&entry["dPrices"]!=null)
            {
                data = data+ entry["dPrices"].ToString();
            }
            return query.PaginationQuery<Dictionary<string, object>>(sql.ToString(), page, null);
        }


        /// <summary>
        /// 分页获取销售金额统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetSalesList(PageInfo page, Dictionary<string, object> where, int tUserType, Guid? sStoreID, out string data)
        {
            data = string.Empty;
            string sGoodsName = where.ContainsKey("sKeyWord") ? where["sKeyWord"].ToString() : string.Empty;
            string dStartTime = where.ContainsKey("dStartTime") ? where["dStartTime"].ToString() : string.Empty;
            string dEndTime = where.ContainsKey("dEndTime") ? where["dEndTime"].ToString() : string.Empty;
            StringBuilder timeSearch = new StringBuilder();
            if (!string.IsNullOrEmpty(dStartTime))
            {//时间查询
                timeSearch.AppendFormat(" AND dBookTime>='{0}' ", dStartTime);
            }
            if (!string.IsNullOrEmpty(dEndTime))
            {
                timeSearch.AppendFormat(" AND dBookTime<='{0}' ", dEndTime);
            }

            StringBuilder sql = new StringBuilder();

            //销售统计数量和金额的SQL的语句
            StringBuilder CountSql = new StringBuilder();
            page.OrderBy = "iTotalPrice";
            page.orderType=OrderType.DESC;
            switch (tUserType)
            {
                case 0:
                    {//总后台
                        sql.AppendFormat(@"select 
                                        D.*,E.sShopName 
                                        from 
                                       (select sStoreID,sGoodsName,sGoodsCategory,sGoodsId,
                                        SUM(iTotalPrice) AS iTotalPrice, 
                                        Sum(iAmount) AS iAmount 
                                        from
                                        (select A.sStoreID,C.sGoodsName,C.sGoodsCategory,A.iTotalPrice,B.iAmount,C.ID AS sGoodsId
                                         FROM EHECD_Orders AS A
                                         LEFT JOIN EHECD_OrdersGoods AS B
                                         ON A.ID=B.sOrderID 
                                         LEFT JOIN EHECD_Goods AS C
                                         ON C.ID=B.sGoodsPrimaryKey
		                                 WHERE  A.iState=2 {0}) AS A
                                         group by
                                         sStoreID,sGoodsName,sGoodsCategory,sGoodsId) AS D 
                                         Left join 
                                         EHECD_ShopSet AS E 
                                         ON D.sStoreID=E.sShopID Where 1=1 ", timeSearch.ToString());

                        CountSql.AppendFormat(@"SELECT SUM(B.iAmount) AS Number,
                                                       SUM(A.iTotalPrice) AS dPrices
                                                       FROM EHECD_Orders AS A
                                                       LEFT JOIN EHECD_OrdersGoods AS B
                                                       ON A.ID=B.sOrderID 
                                                       LEFT JOIN EHECD_Goods AS C
                                                       ON C.ID=B.sGoodsPrimaryKey
		                                               WHERE A.iState=2 {0}", timeSearch.ToString());
                        break;
                    }
                case 1:
                    {//店铺后台
                        sql.AppendFormat(@"select  entry1.*  FROM                                         
                                                   (select  sStoreID, sGoodsName,sGoodsCategory ,sGoodsId,
                                                    SUM(iTotalPrice) AS iTotalPrice, 
                                                    Sum(iAmount) AS iAmount 
                                                    FROM
                                                   (select A.sStoreID,C.sGoodsName,C.sGoodsCategory,A.iTotalPrice,B.iAmount,C.ID AS sGoodsId
                                                    FROM EHECD_Orders AS A
                                                    LEFT JOIN EHECD_OrdersGoods AS B
                                                    ON A.ID=B.sOrderID 
                                                    LEFT JOIN EHECD_Goods AS C
                                                    ON C.ID=B.sGoodsPrimaryKey
		                                            WHERE A.sStoreID='{1}' AND A.iState=2 {0}) AS entry
		                                            group by sStoreID,sGoodsName,sGoodsCategory,sGoodsId) AS entry1
                                                    WHERE 1=1 ", timeSearch.ToString(), sStoreID);

                        CountSql.AppendFormat(@"SELECT SUM(B.iAmount) AS Number,
                                                       SUM(A.iTotalPrice) AS dPrices
                                                       FROM EHECD_Orders AS A
                                                       LEFT JOIN EHECD_OrdersGoods AS B
                                                       ON A.ID=B.sOrderID 
                                                       LEFT JOIN EHECD_Goods AS C
                                                       ON C.ID=B.sGoodsPrimaryKey
		                                               WHERE A.sStoreID='{0}' AND A.iState=2 {1}", sStoreID, timeSearch.ToString());
                        break;
                    }
                case 2:
                    {//合伙人后台
                        sql.AppendFormat(@"select  entry1.*,E.sShopName  FROM                                         
                                                   (select  sStoreID, sGoodsName,sGoodsCategory ,sGoodsId,
                                                    SUM(iTotalPrice) AS iTotalPrice, 
                                                    Sum(iAmount) AS iAmount 
                                                    FROM
                                                   (select A.sStoreID,C.sGoodsName,C.sGoodsCategory,A.iTotalPrice,B.iAmount,C.ID AS sGoodsId
                                                    FROM EHECD_Orders AS A
                                                    LEFT JOIN EHECD_OrdersGoods AS B
                                                    ON A.ID=B.sOrderID 
                                                    LEFT JOIN EHECD_Goods AS C
                                                    ON C.ID=B.sGoodsPrimaryKey
		                                            WHERE A.sPartnerID='{1}' AND A.iState=2 {0}) AS entry
		                                            group by sStoreID,sGoodsName,sGoodsCategory,sGoodsId) AS entry1
		                                            LEFT JOIN  EHECD_ShopSet AS E 
		                                            ON entry1.sStoreID=E.sShopID 
                                                    WHERE 1=1 ", timeSearch.ToString(), sStoreID);//此处的店铺的就是合伙人的ID

                        CountSql.AppendFormat(@"SELECT SUM(B.iAmount) AS Number,
                                                       SUM(A.iTotalPrice) AS dPrices
                                                       FROM EHECD_Orders AS A
                                                       LEFT JOIN EHECD_OrdersGoods AS B
                                                       ON A.ID=B.sOrderID 
                                                       LEFT JOIN EHECD_Goods AS C
                                                       ON C.ID=B.sGoodsPrimaryKey
		                                               WHERE A.sPartnerID='{0}' AND A.iState=2 {1}", sStoreID, timeSearch.ToString());
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(sGoodsName))
            {//商品名称查询
                sql.AppendFormat(" AND sGoodsName Like '%{0}%' ", sGoodsName);
                CountSql.AppendFormat(" AND sGoodsName Like '%{0}%' ", sGoodsName);
            }
            //销售金额的统计数量和金额
            var entry = query.SingleQuery<Dictionary<string, object>>(CountSql.ToString(), null);
            if (entry!=null&&entry.ContainsKey("Number")&&entry["Number"] !=null)
            {
                data = data + entry["Number"].ToString() + ",";
            }
            if (entry != null&&entry.ContainsKey("dPrices") && entry["dPrices"] != null)
            {
                data = data + entry["dPrices"].ToString();
            }

            return query.PaginationQuery<Dictionary<string, object>>(sql.ToString(), page, null);
        }


        /// <summary>
        /// 分页获取退货统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>>GetReturnList(PageInfo page, Dictionary<string, object> where, int tUserType, Guid? sStoreID, out string data)
        {
            data = string.Empty;
            int iState = where.ContainsKey("iState") ? where["iState"].ToInt32() : -1;
            string sKeyWord = where.ContainsKey("sKeyWord") ? where["sKeyWord"].ToString() : string.Empty;
            string dStartTime = where.ContainsKey("dStartTime") ? where["dStartTime"].ToString() : string.Empty;
            string dEndTime = where.ContainsKey("dEndTime") ? where["dEndTime"].ToString() : string.Empty;
            StringBuilder sql = new StringBuilder();

            //退货统计数量和金额的SQL的语句
            StringBuilder CountSql = new StringBuilder();

            page.OrderBy = "dInsertTime";
            page.orderType = OrderType.DESC;

            switch (tUserType)
            {
                case 0:
                    {//总后台
                        sql.Append(@"SELECT
                                    sReturnNo,
                                    sOrderNo,
                                    A.iState,
                                    dInsertTime,
                                    sReceiver,
                                    sShopName
                              FROM  EHECD_ReturnOrders AS A 
                                    LEFT JOIN EHECD_Orders AS B
                                    ON A.sOrderID=B.ID 
                                    LEFT JOIN EHECD_ShopSet AS C 
                                    ON C.sShopID=B.sStoreID 
                                    Where 1=1 AND  B.iState=3 ");

                        CountSql.Append(@"select 
                                                COUNT(*) as Number,
                                                SUM(iAmount) AS Amount,
                                                SUM(iTotalMoney) AS dPrices 
                                          FROM  EHECD_ReturnOrders AS A 
                                                  LEFT JOIN EHECD_Orders AS B
                                                  ON A.sOrderID=B.ID 
                                                  LEFT JOIN EHECD_ShopSet AS C 
                                                  ON C.sShopID=B.sStoreID
                                                  LEFT JOIN EHECD_OrdersGoods AS D
                                                  ON D.sOrderID=B.ID
                                             Where 1=1 AND  B.iState=3");
                        break;
                    }
                case 1:
                    {//店铺后台

                        sql.AppendFormat(@"SELECT
                                        sReturnNo,
                                        sOrderNo,
                                        A.iState,--退货状态
                                        dInsertTime,
                                        sReceiver
                                        FROM  EHECD_ReturnOrders AS A 
                                        LEFT JOIN EHECD_Orders AS B
                                        ON A.sOrderID=B.ID 
                                        WHERE B.sStoreID='{0}' AND  B.iState=3", sStoreID);

                        CountSql.AppendFormat(@"SELECT
                                                  COUNT(*) as Number,
                                                  SUM(iAmount) AS Amount,
                                                  SUM(iTotalMoney) AS dPrices
                                                  FROM  EHECD_ReturnOrders AS A 
                                                  LEFT JOIN EHECD_Orders AS B
                                                  ON A.sOrderID=B.ID 
                                                  LEFT JOIN EHECD_ShopSet AS C 
                                                  ON C.sShopID=B.sStoreID
                                                  LEFT JOIN EHECD_OrdersGoods AS D
                                                  ON D.sOrderID=B.ID
                                                  WHERE B.sStoreID='{0}' AND  B.iState=3", sStoreID);
                        break;
                    }
                case 2:
                    {//合伙人后台
                        sql.AppendFormat(@"SELECT
                                        sReturnNo,
                                        sOrderNo,
                                        A.iState,--退货状态
                                        dInsertTime,
                                        sReceiver,
                                        sShopName 
                                        FROM  EHECD_ReturnOrders AS A 
                                        LEFT JOIN EHECD_Orders AS B
                                        ON A.sOrderID=B.ID 
                                        LEFT JOIN EHECD_ShopSet AS C 
                                        ON C.sShopID=B.sStoreID
                                        WHERE B.sPartnerID='{0}' AND B.iState=3
                                      ", sStoreID);

                        CountSql.AppendFormat(@"  SELECT
                                                  COUNT(*) as Number,
                                                  SUM(iAmount) AS Amount,
                                                  SUM(iTotalMoney) AS dPrices
                                                  FROM  EHECD_ReturnOrders AS A 
                                                  LEFT JOIN EHECD_Orders AS B
                                                  ON A.sOrderID=B.ID 
                                                  LEFT JOIN EHECD_ShopSet AS C 
                                                  ON C.sShopID=B.sStoreID
                                                  LEFT JOIN EHECD_OrdersGoods AS D
                                                  ON D.sOrderID=B.ID
                                                  WHERE B.sPartnerID='{0}' AND B.iState=3", sStoreID);
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(sKeyWord))
            {//模糊查询 退货编号/订单号
                sql.AppendFormat(" AND (sReturnNo Like '%{0}%' OR sOrderNo Like '%{0}%') ", sKeyWord);
            }
            if (iState >= 0)
            {//状态查询
                sql.AppendFormat(" AND A.iState={0} ", iState);
                CountSql.AppendFormat(" AND A.iState={0} ", iState);
            }
            if (!string.IsNullOrEmpty(dStartTime))
            {//开始时间
                sql.AppendFormat(" AND dInsertTime>='{0}' ", dStartTime);
                CountSql.AppendFormat(" AND dInsertTime>='{0}' ", dStartTime);
            }
            if (!string.IsNullOrEmpty(dEndTime))
            {//结束时间
                sql.AppendFormat(" AND dInsertTime<='{0}' ", dEndTime);
                CountSql.AppendFormat(" AND dInsertTime<='{0}' ", dEndTime);
            }

            //退货的统计数量和金额
            var entry = query.SingleQuery<Dictionary<string, object>>(CountSql.ToString(), null);
            if (entry!=null&&entry.ContainsKey("Number") && entry["Number"] != null)
            {
                data = data + entry["Number"].ToString() + ",";
            }
            if (entry != null && entry.ContainsKey("Amount") && entry["Amount"] != null)
            {
                data = data + entry["Amount"].ToString() + ",";
            }
            if (entry != null && entry.ContainsKey("dPrices") && entry["dPrices"] != null)
            {
                data = data + entry["dPrices"].ToString();
            }

            return query.PaginationQuery<Dictionary<string, object>>(sql.ToString(), page, null);
        }
    }
}
