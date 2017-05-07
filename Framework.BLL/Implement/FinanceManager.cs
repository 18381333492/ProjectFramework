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
    /// 财务【实现】
    /// </summary>
    /// <returns></returns>
    public class FinanceManager : IFinanceManager
    {
        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int ApplyWithdraw(EHECD_WithdrawCashDTO dto,EHECD_SystemUserDTO user)
        {
            dto.sWithdrawNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            StringBuilder sb = new StringBuilder();
            sb.Append(DBSqlHelper.GetInsertSQL<EHECD_WithdrawCashDTO>(dto)).Append(";");
            // 生成提现编号
           
            EHECD_SystemUserDTO sys = new EHECD_SystemUserDTO();
            //查出账号余额
           var money= query.SingleQuery<EHECD_SystemUserDTO>("SELECT iAccountNalance FROM EHECD_SystemUser WHERE ID=@ID", new { ID = user.ID });
            sys.iAccountNalance = money.iAccountNalance - dto.iWithdrawMoney;
            sb.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(sys, string.Format(" Where ID='{0}'", user.ID)));
            return excute.ExcuteTransaction(sb.ToString());
        }

        /// <summary>
        /// 获取提现人的提现信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override EHECD_WithdrawCashDTO GetApplierInfo(Guid? id)
        {
            return query.SingleQuery<EHECD_WithdrawCashDTO>(@"SELECT
	                                                                TOP (1) iWithdrawMoney,
	                                                                sBankCardNo,
	                                                                sBankName,
	                                                                sBankCardUserName,
	                                                                dApplyTime
                                                                FROM
	                                                                EHECD_WithdrawCash
                                                                WHERE
	                                                                sWithdrawMemberID = @sWithdrawMemberID
                                                                ORDER BY
	                                                                dApplyTime DESC", new { sWithdrawMemberID = id });
        }

        /// <summary>
        /// 获取【后台 财务管理】【合伙人详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetBackgroundPartnerDetails(PageInfo pageInfo, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dChangeTime";
            string PartnerID = where.ID.Value.ToString();
            string typeString = "A.iType=4 OR A.iType=3 OR A.iType=1";//默认的类型查询
            //3：退款 4：收入
            if (!string.IsNullOrEmpty(where.iType.Value.ToString()) && where.iType.Value.ToString() != "-1")
            {
                typeString = string.Format(" A.iType={0} ", where.iType.Value.ToString());
            }

            StringBuilder sSql = new StringBuilder();//查询的SQL语句

            sSql.AppendFormat(@"SELECT A.sOrderNo,
	                             A.iCommissionPrice,
	                             A.iMethod,
                                 A.iType,
	                             A.iPrice,
	                             A.iServicePrice,
	                             A.sUserName,
	                             B.sShopName,
	                             A.dChangeTime
                          FROM  EHECD_ClientBalanceDetail  AS A 
                                 LEFT JOIN EHECD_ShopSet AS B
                                 ON A.iShopID=B.sShopID
                                 LEFT JOIN EHECD_Orders AS C
                                 ON A.sOrderID=C.ID
                          WHERE  ({1})
                                 AND A.PartnerID='{0}'    
                                 ", PartnerID, typeString);
            if (where.dValidDateEnd.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);
                    sSql.AppendFormat(string.Format(" AND  A.dChangeTime>='{0} 00:00:00' AND  A.dChangeTime<='{1} 23:59:59' ",
                                           string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days)));
                }
            }

            //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()))
            {//开始时间
                sSql.AppendFormat(string.Format(" AND  A.dChangeTime>='{0}'", where.dValidDateStart.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {//结束时间
                sSql.AppendFormat(string.Format(" AND  A.dChangeTime<='{0}'", where.dValidDateEnd.Value.ToString()));
            }

            return query.PaginationQuery<Dictionary<string, object>>(sSql.ToString(), pageInfo, null);
        }

        /// <summary>
        /// 获取【后台 财务管理】【分享客详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetBackgroundShareDetails(PageInfo pageInfo, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dChangeTime";
            StringBuilder builder = new StringBuilder();
            string sql = @"SELECT
	                                a.sOrderNo,
	                                a.sOrderID,
	                                a.iShopID,
	                                a.iMethod,
	                                a.iType,--1提现 5收入佣金
	                                a.iPrice,
	                                a.dChangeTime,
	                                b.sReceiver,
	                                c.sShopName
                                FROM
	                                EHECD_ClientBalanceDetail a 
                                    LEFT JOIN EHECD_Orders b
                                    ON a.sOrderID=b.ID
                                    LEFT JOIN EHECD_ShopSet c
	                                ON a.iShopID=c.sShopID
                                WHERE
                                    (a.iType=5 OR a.iType=1) AND
                                    a.iClientType = 1";

            builder.Append(sql);
            builder.Append(string.Format(@" AND a.sClientID='{0}' ", where.ID.Value.ToString()));
         
            //时间查询
            if (where.dValidDateEnd.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);

                    builder.AppendFormat("AND dChangeTime>='{0} 00:00:00' AND dChangeTime<='{1} 23:59:59'",
                                             string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days));
                }
            }
            //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()))
            {//开始时间
                builder.AppendFormat(string.Format(" AND dChangeTime>='{0}'", where.dValidDateStart.Value.ToString()));     
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {//结束时间
                builder.AppendFormat(string.Format(" AND dChangeTime<='{0}'", where.dValidDateEnd.Value.ToString()));      
            }

            //类别查询 【查询】
            if (!string.IsNullOrEmpty(where.iType.Value.ToString()) && where.iType.Value.ToString() != "-1")
            {
                builder.Append(string.Format(" AND a.iType={0} ", where.iType.Value.ToString()));
            }

            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }

        /// <summary>
        /// 获取【后台 财务管理】【店铺详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetBackgroundStoreDetails(PageInfo pageInfo, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dChangeTime";
            string sShopID = where.ID.Value.ToString();
            string typeString = "A.iType=4 OR A.iType=3 OR A.iType=1";//默认的类型查询
            //1:提现 3：退款 4：收入
            if (!string.IsNullOrEmpty(where.sType_store.Value.ToString()) && where.sType_store.Value.ToString() != "-1")
            {//按类型查询
                typeString = string.Format(" A.iType={0} ", where.sType_store.Value.ToString());
            }
            StringBuilder sSql = new StringBuilder();//查询的SQL语句


            sSql.AppendFormat(@"SELECT A.sOrderNo,
	                             A.iCommissionPrice,
	                             A.iMethod,
                                 A.iType,
	                             A.iPrice,
	                             A.iServicePrice,
	                             A.sUserName,
	                             A.dChangeTime
                          FROM  EHECD_ClientBalanceDetail  AS A 
                                 LEFT JOIN EHECD_Orders AS B
                                 ON A.sOrderID=B.ID
                          WHERE  ({1})
                                AND A.iShopID='{0}'   
                                 ", sShopID, typeString);
            if (where.dValidDateEnd_store.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);
                    sSql.AppendFormat(string.Format(" AND A.dChangeTime>='{0} 00:00:00' AND A.dChangeTime<='{1} 23:59:59' ",
                                           string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days)));
                }
            }

            //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()))
            {//开始时间
                sSql.AppendFormat(string.Format(" AND A.dChangeTime>='{0}'", where.dValidDateStart_store.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
            {//结束时间
                sSql.AppendFormat(string.Format(" AND A.dChangeTime<='{0}'", where.dValidDateEnd_store.Value.ToString()));
            }
            return query.PaginationQuery<Dictionary<string, object>>(sSql.ToString(), pageInfo, null);

        }


        /// <summary>
        /// 获取平台用户财务列表
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetFinanceBackgroundList(Dapper.PageInfo pageInfo, dynamic where, out decimal iServicePrice)
        {
            iServicePrice = 0.00M;
            pageInfo.orderType = OrderType.DESC;
            StringBuilder builder = new StringBuilder();
            string sql = @"SELECT
	                            *
                            FROM
	                            (
		                            SELECT
			                            *
		                            FROM
			                            FN_SHOP_FINANCE_PARTNER ('{0}', '{1}')
		                            UNION ALL
			                            SELECT
				                            *
			                            FROM
				                            FN_SHOP_FINANCE_STORE ('{0}', '{1}')
			                            UNION ALL
				                            SELECT
					                            *
				                            FROM
					                            FN_SHOP_FINANCE_SHARE ('{0}', '{1}')
	                            ) AS A
                            WHERE   
	                            1=1 ";

            //周期时间统计 【查询】时间默认是最近一个月 
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()) && !string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {
                sql = string.Format(sql, where.dValidDateStart.Value.ToString(), where.dValidDateEnd.Value.ToString());
                if (pageInfo.PageIndex <= 1)
                {
                    //服务费(部分)
                    var tempDic = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT
	                                                                            SUM (iServicePrice) iServicePrice
                                                                            FROM
	                                                                            EHECD_ClientBalanceDetail
                                                                            WHERE
	                                                                            dChangeTime >= '{0}'
                                                                            AND dChangeTime <= '{1}' AND iType=7 ", where.dValidDateStart.Value.ToString(), where.dValidDateEnd.Value.ToString()), null);
                    if (tempDic == null || tempDic["iServicePrice"] == null) {
                        tempDic = new Dictionary<string,object>();
                        tempDic["iServicePrice"] = 0.00M;
                    }
                    Decimal.TryParse(tempDic["iServicePrice"].ToString(), out iServicePrice);
                }
            }
            else
            {
                sql = string.Format(sql, "1970-01-01 08:00", DateTime.Now);
                if (pageInfo.PageIndex <= 1)
                {
                    //服务费（全部）
                    var tempDic = query.SingleQuery<Dictionary<string, object>>(@"SELECT
	                                                                            SUM (iServicePrice) iServicePrice
                                                                            FROM
	                                                                            EHECD_ClientBalanceDetail WHERE iType=7 ", null);
                    if (tempDic == null) tempDic = new Dictionary<string, object>();
                    if (tempDic["iServicePrice"]==null)
                    {
                        tempDic["iServicePrice"] = 0.00M;
                    }
                    Decimal.TryParse(tempDic["iServicePrice"].ToString(), out iServicePrice);
                }
            }
            builder.Append(sql);

            //角色查询 【查询】
            if (!string.IsNullOrEmpty(where.sRole.Value.ToString()) && where.sRole.Value.ToString() != "-1")
            {
                builder.Append(string.Format(" AND sRole={0} ", where.sRole.Value.ToString()));
            }

            //店铺名称/合伙人/分享客 【查询】
            if (!string.IsNullOrEmpty(where.sName.Value.ToString()))
            {
                builder.Append(string.Format(" AND sName LIKE '%{0}%' ", where.sName.Value.ToString()));
            }

            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }

        /*****************************财务统计的重写***************************************/


        /// <summary>
        /// 获取平台用户财务列表
        /// [汤台重写]
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetFinanceBackgroundListByTT(Dapper.PageInfo pageInfo, dynamic where, out decimal iServicePrice)
        {
            StringBuilder builder = new StringBuilder();//查询语句

            StringBuilder source = new StringBuilder();//数据源

            StringBuilder serverSql = new StringBuilder();//查询服务费sql语句

            source.Append("SELECT * FROM EHECD_ClientBalanceDetail WHERE 1=1 ");

            serverSql.Append(@"SELECT A.* FROM 
                                        EHECD_ClientBalanceDetail AS A LEFT JOIN EHECD_Client AS B 
                                        ON A.[sClientID]=B.ID
                                        WHERE 1=1 AND (iType=3 OR iType=4) ");
            //时间查询
            if (where.dValidDateEnd.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);

                    source.AppendFormat("AND dChangeTime>='{0} 00:00:00' AND dChangeTime<='{1} 23:59:59'",
                                             string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days));

                    serverSql.AppendFormat("AND  dChangeTime>='{0} 00:00:00' AND dChangeTime<='{1} 23:59:59'",
                                         string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days));
                }
            }

            //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()))
            {//开始时间
                source.AppendFormat(string.Format(" AND dChangeTime>='{0}'", where.dValidDateStart.Value.ToString()));

                serverSql.AppendFormat(string.Format(" AND dChangeTime>='{0}'", where.dValidDateStart.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {//结束时间
                source.AppendFormat(string.Format(" AND dChangeTime<='{0}'", where.dValidDateEnd.Value.ToString()));

                serverSql.AppendFormat(string.Format(" AND dChangeTime<='{0}'", where.dValidDateEnd.Value.ToString()));
            }

            builder.AppendFormat(@"select * from
                        ( select  client,sum(iPrice) as iPrice ,1 as iClientType,d.sNickName as Name from 
                         (select sClientID as client,sum([iPrice]) AS [iPrice] from ({0}) as temp WHERE iClientType=1 AND [iType]=5  group by [sClientID]--佣金收入
	                     UNION ALL
                         select sClientID as client,-sum([iPrice]) AS [iPrice] from ({0}) as temp WHERE iClientType=1 AND [iType]=1  group by [sClientID])--提现
                         as a LEFT JOIN EHECD_Client as d ON a.client=d.ID group by client,d.sNickName
                         --分享客
                    UNION ALL
                        select client, sum(iPrice) as iPrice, 2 as iClientType,e.sShopName as Name from
                       (select iShopID as client,sum(iPrice) AS iPrice from ({0}) as temp WHERE  [iType]=4  group by iShopID--收入
	                    UNION ALL
                        select iShopID as client,-sum(iPrice) AS iPrice from ({0}) as temp WHERE [iType]=1  group by iShopID--提现
	                    UNION ALL
                        select iShopID as client,-sum(iPrice) AS iPrice from ({0}) as temp WHERE [iType]=3  group by iShopID) --退款
                        as b LEFT JOIN EHECD_ShopSet as e ON b.client=e.sShopID group by client,e.sShopName
                        --店铺
                    UNION ALL
                        select client, sum(iPrice) as iPrice, 3 as iClientType,f.sUserName as Name from
                        (select PartnerID as client,(sum(iServicePrice)*(select top 1 iPartnerCommissionPrecent from dbo.EHECD_BaseSetting)*0.01) AS iPrice from ({0}) as temp WHERE  [iType]=4 AND PartnerID IS NOT NULL  group by PartnerID--收入
	                    UNION ALL
                        select PartnerID as client,-sum(iPrice) AS iPrice from ({0}) as temp WHERE  [iType]=1 AND PartnerID IS NOT NULL  group by PartnerID--提现
	                    UNION ALL
                        select PartnerID as client,-(sum(iServicePrice)*(select top 1 iPartnerCommissionPrecent from dbo.EHECD_BaseSetting)*0.01) AS iPrice from ({0}) as temp WHERE  [iType]=3 AND PartnerID IS NOT NULL  group by PartnerID)--退款
                        as c LEFT JOIN EHECD_SystemUser as f ON c.client=f.ID group by client,f.sUserName) as temptwo WHERE 1=1 
                        --合伙人
               ", source.ToString());
            //角色查询 【查询】
            if (!string.IsNullOrEmpty(where.sRole.Value.ToString()) && where.sRole.Value.ToString() != "-1")
            {
                builder.Append(string.Format(" AND iClientType={0} ", where.sRole.Value.ToString()));

                serverSql.Append(string.Format(" AND A.iClientType={0} ", where.sRole.Value.ToString()));
            }

            //店铺名称/合伙人/分享客 【查询】
            if (!string.IsNullOrEmpty(where.sName.Value.ToString()))
            {
                builder.Append(string.Format(" AND Name LIKE '%{0}%' ", where.sName.Value.ToString()));

                serverSql.Append(string.Format(" AND B.sNickName LIKE '%{0}%' ", where.sName.Value.ToString()));
            }

            //服务费(部分)
            iServicePrice = 0;
            var list = query.QueryList<EHECD_ClientBalanceDetailDTO>(serverSql.ToString(),null);
            if (list != null && list.Count > 0)
            {//有数据的时候
                var totalserver = list.Where(m => m.iType == 4).Sum(m => m.iServicePrice);
                var returnserver = list.Where(m => m.iType == 3).Sum(m => m.iServicePrice);
                iServicePrice = totalserver.Value - returnserver.Value;
            }
            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }



        /// <summary>
        ///  获取店铺的财务数据列表
        /// [汤台重写]
        /// </summary>
        /// <param name="pageInfo">分页查参数</param>
        /// <param name="where">查询条件</param>
        /// <param name="PartnerID">合伙人主键ID</param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetBackgroundStoreDetails(PageInfo pageInfo, dynamic where, Guid sShopID)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dChangeTime";
            string typeString = "A.iType=4 OR A.iType=3 OR A.iType=1";//默认的类型查询
            //1:提现 3：退款 4：收入
            if (!string.IsNullOrEmpty(where.sType_store.Value.ToString()) && where.sType_store.Value.ToString() != "-1")
            {//按类型查询
                typeString = string.Format(" A.iType={0} ", where.sType_store.Value.ToString());
            }
            StringBuilder sSql = new StringBuilder();//查询的SQL语句


            sSql.AppendFormat(@"SELECT A.sOrderNo,
	                             A.iCommissionPrice,
	                             A.iMethod,
                                 A.iType,
	                             A.iPrice,
	                             A.iServicePrice,
	                             A.sUserName,
	                             A.dChangeTime
                          FROM  EHECD_ClientBalanceDetail  AS A 
                                 LEFT JOIN EHECD_Orders AS B
                                 ON A.sOrderID=B.ID
                          WHERE  ({1})
                                AND A.iShopID='{0}'   
                                 ", sShopID.ToString(), typeString);
            if (where.dValidDateEnd_store.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);
                    sSql.AppendFormat(string.Format(" AND A.dChangeTime>='{0} 00:00:00' AND A.dChangeTime<='{1} 23:59:59' ",
                                           string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days)));
                }
            }

            //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()))
            {//开始时间
                sSql.AppendFormat(string.Format(" AND A.dChangeTime>='{0}'", where.dValidDateStart_store.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
            {//结束时间
                sSql.AppendFormat(string.Format(" AND A.dChangeTime<='{0}'", where.dValidDateEnd_store.Value.ToString()));
            }
            return query.PaginationQuery<Dictionary<string, object>>(sSql.ToString(), pageInfo, null);

        }


        /// <summary>
        ///  获取合伙人财务
        /// [汤台重写]
        /// </summary>
        /// <param name="pageInfo">分页查参数</param>
        /// <param name="where">查询条件</param>
        /// <param name="PartnerID">合伙人主键ID</param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetFinancePartnerList(PageInfo pageInfo, dynamic where,Guid PartnerID)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy= "dChangeTime";
            string typeString = "A.iType=4 OR A.iType=3";//默认的类型查询
            //3：退款 4：收入
            if (!string.IsNullOrEmpty(where.sType_store.Value.ToString()) && where.sType_store.Value.ToString() != "-1")
            {
                typeString=string.Format(" A.iType={0} ", where.sType_store.Value.ToString());
            }

            StringBuilder sSql = new StringBuilder();//查询的SQL语句

            sSql.AppendFormat(@"SELECT A.sOrderNo,
	                             A.iCommissionPrice,
	                             A.iMethod,
                                 A.iType,
	                             A.iPrice,
	                             A.iServicePrice,
	                             A.sUserName,
	                             B.sShopName,
	                             A.dChangeTime
                          FROM  EHECD_ClientBalanceDetail  AS A 
                                 LEFT JOIN EHECD_ShopSet AS B
                                 ON A.iShopID=B.sShopID
                                 LEFT JOIN EHECD_Orders AS C
                                 ON A.sOrderID=C.ID
                          WHERE  ({1})
                                 AND A.PartnerID='{0}'    
                                 ", PartnerID.ToString(), typeString);
            if (where.dValidDateEnd_store.Value.ToString() != "2080-12-31")
            {
                if (string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()) && string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
                {//默认的时间是一个月
                    DateTime now = DateTime.Now;
                    int Days = DateTime.DaysInMonth(now.Year, now.Month);
                    sSql.AppendFormat(string.Format(" AND  A.dChangeTime>='{0} 00:00:00' AND  A.dChangeTime<='{1} 23:59:59' ",
                                           string.Format("{0}-{1}-1", now.Year, now.Month), string.Format("{0}-{1}-{2}", now.Year, now.Month, Days)));
                }
            }

             //时间查询
            if (!string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()))
            {//开始时间
                sSql.AppendFormat(string.Format(" AND  A.dChangeTime>='{0}'", where.dValidDateStart_store.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
            {//结束时间
                sSql.AppendFormat(string.Format(" AND  A.dChangeTime<='{0}'", where.dValidDateEnd_store.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.sShopName.Value.ToString()))
            {//店铺查询
                sSql.AppendFormat(string.Format(" AND  B.sShopName LIKE '%{0}%'", where.sShopName.Value.ToString()));
            }

             return query.PaginationQuery<Dictionary<string,object>>(sSql.ToString(),pageInfo,null);
        }

        /*****************************财务统计的重写***************************************/

        /// <summary>
        /// 获取合伙人财务
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetFinancePartnerList(Dapper.PageInfo pageInfo, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            StringBuilder builder = new StringBuilder();//列表SQL
            StringBuilder iCommissionPriceSQLBuilder = new StringBuilder();//我的提成SQL
            StringBuilder iTotlePriceSQLBuilder = new StringBuilder();//金额SQL
            decimal iCommissionPrice = -1.00M,  //我的提成
                    iTotlePrice = -1.00M;      //金额

            string sql = string.Format(@"SELECT
	                                            *
                                            FROM
	                                            (
		                                            SELECT
			                                            a.*, b.sShopName,
			                                            c.sPartnerID
		                                            FROM
			                                            V_SHOP_FINANCE_DETAIL a,
			                                            EHECD_ShopSet b,
			                                            EHECD_SystemUser c
		                                            WHERE
			                                            1 = 1
		                                            AND a.iShopID = b.sShopID
		                                            AND a.iShopID = c.ID
	                                            ) d
                                            WHERE
	                                            d.sPartnerID='{0}' ", where.ID.Value.ToString());

            builder.Append(sql);
            //iCommissionPriceSQL.Append(sql);
            //iTotlePriceSQL.Append(sql);
            //周期时间统计 【查询】时间默认是最近一个月 
            if (!string.IsNullOrEmpty(where.dValidDateStart_store.Value.ToString()) && !string.IsNullOrEmpty(where.dValidDateEnd_store.Value.ToString()))
            {
                //((operateTime>='1970 -01-01 08:00' AND operateTime<='2016-10-24 10:45:06') or sType=3)
                builder.Append(string.Format(" AND operateTime>='{0}' AND operateTime<='{1}' ", where.dValidDateStart_store.Value.ToString(), where.dValidDateEnd_store.Value.ToString()));
                if (pageInfo.PageIndex <= 1)
                {
                    //我的提成
                    iCommissionPriceSQLBuilder.Append(string.Format(@"SELECT
		                                                                    SUM (c.iPrice) iPrice
	                                                                    FROM
		                                                                    (
			                                                                    SELECT
				                                                                    a.sClientID,
				                                                                    a.iShopID,
				                                                                    a.iPrice,
				                                                                    a.dChangeTime,
				                                                                    b.sPartnerID
			                                                                    FROM
				                                                                    EHECD_ClientBalanceDetail a,
				                                                                    EHECD_SystemUser b
			                                                                    WHERE
				                                                                    a.iType = 10 --提成收入
			                                                                    AND a.iClientType = 3 --合伙人
			                                                                    AND a.bIsDeleted = 0
			                                                                    AND a.iShopID = b.ID
			                                                                    AND a.dChangeTime >= '{0}'
			                                                                    AND a.dChangeTime <= '{1}'
		                                                                    ) c
	                                                                    WHERE
		                                                                    c.sPartnerID = '{2}' ",
                                                    where.dValidDateStart_store.Value.ToString(), where.dValidDateEnd_store.Value.ToString(),
                                                    where.ID.Value.ToString()));

                    //金额(部分)
                    iTotlePriceSQLBuilder.Append(string.Format(@"SELECT
                                                     iPrice
                                                    FROM
                                                     FN_SHOP_FINANCE_PARTNER ('{0}', '{1}')
                                                    WHERE
                                                     ID = '{2}' ",
                                where.dValidDateStart_store.Value.ToString(), where.dValidDateEnd_store.Value.ToString(),
                                where.ID.Value.ToString()));
                }
            }
            else
            {
                builder.Append(string.Format(" AND operateTime>='{0}' AND operateTime<='{1}' ", "1970 -01-01 08:00", DateTime.Now));
                if (pageInfo.PageIndex <= 1)
                {
                    //我的提成
                    iCommissionPriceSQLBuilder.Append(string.Format(@"SELECT
	                                                                        SUM (c.iPrice) iPrice
                                                                        FROM
	                                                                        (
		                                                                        SELECT
			                                                                        a.sClientID,
			                                                                        a.iShopID,
			                                                                        a.iPrice,
			                                                                        a.dChangeTime,
			                                                                        b.sPartnerID
		                                                                        FROM
			                                                                        EHECD_ClientBalanceDetail a,
			                                                                        EHECD_SystemUser b
		                                                                        WHERE
			                                                                        a.iType = 10 --提成收入
		                                                                        AND a.iClientType = 3 --合伙人
		                                                                        AND a.bIsDeleted = 0
		                                                                        AND a.iShopID = b.ID
	                                                                        ) c
                                                                        WHERE
	                                                                        c.sPartnerID = '{0}' ", where.ID.Value.ToString()));

                    //金额(部分)
                    iTotlePriceSQLBuilder.Append(string.Format(@"SELECT
                                                     iPrice
                                                    FROM
                                                     FN_SHOP_FINANCE_PARTNER ('{0}', '{1}')
                                                    WHERE
                                                     ID = '{2}' ",
                                "1970-01-01 08:00", DateTime.Now, where.ID.Value.ToString()));
                }
            }

            //1：退款 2：收入 3：店铺提现 【查询】
            if (!string.IsNullOrEmpty(where.sType_store.Value.ToString()) && where.sType_store.Value.ToString() != "-1")
            {
                builder.Append(string.Format(" AND sType={0} ", where.sType_store.Value.ToString()));
            }

            //我的提成
            var iCommissionDic = query.SingleQuery<Dictionary<string, object>>(iCommissionPriceSQLBuilder.ToString(), null);
            if (iCommissionDic == null || iCommissionDic["iPrice"] == null)
            {
                iCommissionDic = new Dictionary<string, object>();
                iCommissionDic["iPrice"] = 1.00M;
            }
            Decimal.TryParse(iCommissionDic["iPrice"].ToString(), out iCommissionPrice);
            //金额总计（全部）
            var ipriceDic = query.SingleQuery<Dictionary<string, object>>(iTotlePriceSQLBuilder.ToString(), null);
            if (ipriceDic == null || ipriceDic["iPrice"] == null)
            {
                ipriceDic = new Dictionary<string, object>();
                ipriceDic["iPrice"] = 1.00M;
            }
            Decimal.TryParse(ipriceDic["iPrice"].ToString(), out iTotlePrice);
            //查询总结果
            Dapper.PagingRet<Dictionary<string, object>> result = query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
            if (result != null && result.MaxCount > 0)
            {
                result.Result[0]["iTotleCommissionePrice"] = iCommissionPrice;
                result.Result[0]["iTotlePrice"] = iTotlePrice;
            }
            //返回结果
            return result;
        }

        /// <summary>
        /// 获取店铺财务
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetFinanceStoreList(Dapper.PageInfo pageInfo, dynamic where)
        {
            return query.PaginationQuery<Dictionary<string, object>>("select * from V_SHOP_FINANCE_DETAIL", pageInfo, null);
        }

        /// <summary>
        /// 获取店铺可提现金额
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetStoreAviliableMoney(Guid? id)
        {
            var data = query.SingleQuery<Dictionary<string, object>>(@"SELECT iAccountNalance AS iPrice FROM EHECD_SystemUser WHERE ID=@ID", new { ID = id });
            // 申请中金额
            var applayMoneyDic = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT
	                                                                                            SUM(iWithdrawMoney) iWithdrawMoney
                                                                                            FROM
	                                                                                            EHECD_WithdrawCash
                                                                                            WHERE
	                                                                                            bIsDeleted = 0
                                                                                            AND sWithdrawMemberType = 0--店铺
                                                                                            AND iState != 2 --非提现成功
                                                                                            AND sWithdrawMemberID=@sWithdrawMemberID "), new { sWithdrawMemberID = id });

            // 提现金额
            var iprice = data != null&& data["iPrice"]!=null ? data["iPrice"] : 0.00M;

            // 申请中金额
            var applayMoney = applayMoneyDic != null&& applayMoneyDic["iWithdrawMoney"]!=null ? applayMoneyDic["iWithdrawMoney"] : 0.00M;

            return new Dictionary<string, object>() {

                { "iprice",iprice },
                { "applayMoney",applayMoney}

            };
        }


        /// <summary>
        /// 获取店铺可提现金额
        /// [汤台重写]
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetStoreAviliableMoneyByTT(Guid? id)
        {
            // 可提现金额
            var data = query.SingleQuery<Dictionary<string, object>>(@"SELECT iAccountNalance AS iPrice FROM EHECD_SystemUser WHERE ID=@ID", new { ID = id });

            // 申请中金额
            var applayMoneyDic = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT
	                                                                                            SUM(iWithdrawMoney) iWithdrawMoney
                                                                                            FROM
	                                                                                            EHECD_WithdrawCash
                                                                                            WHERE
	                                                                                            bIsDeleted = 0
                                                                                            AND sWithdrawMemberType = 0--店铺
                                                                                            AND iState != 2 --非提现成功
                                                                                            AND sWithdrawMemberID=@sWithdrawMemberID "), new { sWithdrawMemberID = id });

            // 提现金额
            var iprice = data != null && data["iPrice"] != null ? data["iPrice"] : 0.00M;

            // 申请中金额
            var applayMoney = applayMoneyDic != null && applayMoneyDic["iWithdrawMoney"] != null ? applayMoneyDic["iWithdrawMoney"] : 0.00M;

            return new Dictionary<string, object>() {

                { "iprice",iprice },
                { "applayMoney",applayMoney}

            };
        }


        /// <summary>
        /// 获取合伙人的可提现金额
        /// [汤台重写]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override Dictionary<string, object> GetPartnerAviliableMoneyByTT(Guid? id)
        {
            // 可提现金额
            var data = query.SingleQuery<Dictionary<string, object>>(@"SELECT iAccountNalance AS iPrice FROM EHECD_SystemUser WHERE ID=@ID", new { ID=id });

            // 申请中金额
            var applayMoneyDic = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT
	                                                                                            SUM(iWithdrawMoney) iWithdrawMoney
                                                                                            FROM
	                                                                                            EHECD_WithdrawCash
                                                                                            WHERE
	                                                                                            bIsDeleted = 0
                                                                                            AND sWithdrawMemberType =1--合伙人
                                                                                            AND iState != 2 --非提现成功
                                                                                            AND sWithdrawMemberID=@sWithdrawMemberID "), new { sWithdrawMemberID = id });

            // 提现金额
            var iprice = data != null && data["iPrice"] != null ? data["iPrice"] : 0.00M;

            // 申请中金额
            var applayMoney = applayMoneyDic != null && applayMoneyDic["iWithdrawMoney"] != null ? applayMoneyDic["iWithdrawMoney"] : 0.00M;

            return new Dictionary<string, object>() {

                { "iprice",iprice },
                { "applayMoney",applayMoney}

            };
        }



        /// <summary>
        /// 获取合伙人可提现金额和申请中的金额
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetPartnerAviliableMoney(Guid? id)
        {
            // 可提现金额
            var data = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT
	                                                                                        iAccountNalance
                                                                                            FROM
	                                                                                         EHECD_SystemUser
	                                                                                        
                                                                                        WHERE
	                                                                                        ID = '{0}'",id), null);

            // 申请中金额
            var applayMoneyDic = query.SingleQuery<Dictionary<string,object>>(string.Format(@"SELECT
	                                                                                            SUM(iWithdrawMoney) iWithdrawMoney
                                                                                            FROM
	                                                                                            EHECD_WithdrawCash
                                                                                            WHERE
	                                                                                            bIsDeleted = 0
                                                                                            AND sWithdrawMemberType = 1--合伙人
                                                                                            AND iState != 2 --非提现成功
                                                                                            AND sWithdrawMemberID=@sWithdrawMemberID "),new { sWithdrawMemberID = id });

            // 提现金额
            var iprice = data!=null&& data["iAccountNalance"] !=null? data["iAccountNalance"] :0.00M;

            // 申请中金额
            var applayMoney = applayMoneyDic != null && applayMoneyDic["iWithdrawMoney"] != null ? applayMoneyDic["iWithdrawMoney"] : 0.00M;

            return new Dictionary<string, object>() {

                { "iprice",iprice },
                { "applayMoney",applayMoney}

            };
        }

        /// <summary>
        /// 获取提现设置，店铺/分享客/合伙人 的提现设置（间隔天数，最低提现额度）
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, object> GetWithdrawCash(string roleName)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT * FROM EHECD_WithdrawSetting WHERE sRoleName=@sRoleName ", new { sRoleName = roleName });
        }
    }
}
