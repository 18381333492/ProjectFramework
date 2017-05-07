using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class ClientManager : IClientManager
    {
        /// <summary>
        /// 页面数据初始化绑定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_ClientDTO> GetPageList(PageInfo info, Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ID,sName,sNickName,iClientType,sPhone,dAddTime from EHECD_Client where bIsDeleted=0");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                sb.AppendFormat("and (sNickName like '%{0}')", dic["sKeyword"].ToString());
            }
            return query.PaginationQuery<EHECD_ClientDTO>(sb.ToString(), info, null);
        }

        /// <summary>
        /// 查询单个用户的完整信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO QueryClientInfo(EHECD_ClientDTO param)
        {
            return query.SingleQuery<EHECD_ClientDTO>("select * from EHECD_Client where ID = @ID", new { param.ID });
        }

        /// <summary>
        /// 载入客户列表【后台客户管理】
        /// </summary>
        /// <param name="info"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> LoadClient(PageInfo info, Dictionary<string, object> param)
        {
            string baseSql = @"SELECT
	                            (
		                            SELECT
			                            SUM (d.iPrice) iTotalPrice
		                            FROM
			                            EHECD_ClientBalanceDetail d
		                            WHERE
			                            d.bIsDeleted = 0 --未删除的
		                            AND d.iType = 5 --类型为5（分享客获得佣金）
		                            AND d.iClientType = 1 --客户类型为分享客
		                            AND d.sClientID = c.ID
		                            GROUP BY
			                            d.sClientID
	                            ) iTotalPrice,
	                            c.ID,
	                            c.sNickName,
	                            c.sPhone,
	                            c.iClientType,
	                            c.iState,
	                            c.dAddTime
                            FROM
	                            EHECD_Client c
                            WHERE
	                            1 = 1";

            var sb = new StringBuilder();

            var iState = CommonHelper.GetDictionaryValue("iState", param, typeof(Byte));
            var iClientType = CommonHelper.GetDictionaryValue("iClientType", param, typeof(Byte));
            var eTime = CommonHelper.GetDictionaryValue("eTime", param, typeof(DateTime));
            var sTime = CommonHelper.GetDictionaryValue("sTime", param, typeof(DateTime));
            var sName = CommonHelper.GetDictionaryValue("sName", param, typeof(String));

            if (iState != null && iState.ToByte() != 9) sb.AppendFormat(" AND c.iState = {0}", iState);
            if (iClientType != null && iClientType.ToByte() != 9) sb.AppendFormat(" AND c.iClientType = {0}", iClientType);
            if (eTime != null) sb.AppendFormat(" AND c.dAddTime <= '{0}'", eTime);
            if (sTime != null) sb.AppendFormat(" AND c.dAddTime >= '{0}'", sTime);
            if (!String.IsNullOrEmpty(sName.ToString())) sb.AppendFormat(" AND (c.sNickName LIKE '%{0}%' OR c.sPhone LIKE '%{0}%')", sName);

            baseSql += sb.ToString();

            return query.PaginationQuery<Dictionary<string, object>>(baseSql, info, null);

        }

        /// <summary>
        /// 载入店铺分享客
        /// </summary>
        /// <param name="info"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> LoadSharedClient(PageInfo info, Dictionary<string, object> param)
        {
            string baseSql = string.Format(@"SELECT
	                                s.ID,
	                                s.sClientID,
	                                s.dInsertTime,
	                                s.bIsForzen,
	                                c.sPhone,
	                                c.sNickName,
	                                (
		                               SELECT
			                            SUM (d.iPrice) iTotalPrice
		                            FROM
			                            EHECD_ClientBalanceDetail d
		                            WHERE
			                            d.bIsDeleted = 0 --未删除的
		                            AND d.iType = 5 --类型为5（分享客获得佣金）
		                            AND d.iClientType = 1 --客户类型为分享客
		                            AND d.sClientID = c.ID
                                    AND d.iShopID='{1}'
		                            GROUP BY
			                            d.sClientID
	                                ) iTotalPrice,
	                                (
		                                SELECT
			                                COUNT (1)
		                                FROM
			                                EHECD_SharedGoods
		                                WHERE
			                                sClientID = s.sClientID
	                                ) gcount
                                FROM
	                                EHECD_SharedClientInfo s,
	                                EHECD_Client c
                                WHERE
	                                s.sClientID = c.ID
                                AND s.sShopID = '{0}' ", CommonHelper.GetDictionaryValue("shopid",param,typeof(string)).ToString(),param["shopid"].ToString());

            var sb = new StringBuilder();

            var iState = CommonHelper.GetDictionaryValue("iState", param, typeof(Byte));            
            var eTime = CommonHelper.GetDictionaryValue("eTime", param, typeof(DateTime));
            var sTime = CommonHelper.GetDictionaryValue("sTime", param, typeof(DateTime));
            var sName = CommonHelper.GetDictionaryValue("sName", param, typeof(String));

            if (iState != null && iState.ToByte() != 9) sb.AppendFormat(" AND s.bIsForzen = {0}", iState);           
            if (eTime != null) sb.AppendFormat(" AND s.dInsertTime <= '{0}'", eTime);
            if (sTime != null) sb.AppendFormat(" AND s.dInsertTime >= '{0}'", sTime);
            if (!String.IsNullOrEmpty(sName.ToString())) sb.AppendFormat(" AND (c.sNickName LIKE '%{0}%' OR c.sPhone LIKE '%{0}%')", sName);

            baseSql += sb.ToString();

            return query.PaginationQuery<Dictionary<string, object>>(baseSql, info, null);
        }

        /// <summary>
        /// 查出所有会员的信息
        /// </summary>
        /// <returns></returns>
        public override IList<EHECD_ClientDTO> SearchPeople()
        {
            return query.QueryList<EHECD_ClientDTO>("select ID,sName,sNickName,sPhone from EHECD_Client where bIsDeleted=0 and iState=1", null);
        }

        /// <summary>
        /// 查出所有分销客的电话
        /// </summary>
        /// <returns></returns>
        public override IList<EHECD_ClientDTO> SerachVipPhone()
        {
            return query.QueryList<EHECD_ClientDTO>("select sPhone from EHECD_Client where bIsDeleted=0 and iClientType=1 and iState=1", null);
        }

        /// <summary>
        /// 载入普通用户的订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_OrdersDTO> LoadClientOrders(PageInfo info, Guid ID)
        {
            return query.PaginationQuery<EHECD_OrdersDTO>("SELECT ID,sOrderNo,sReceiverPhone,iTotalPrice,iType,dBookTime,iState FROM [dbo].[EHECD_Orders] WHERE sClientID = @sClientID AND bIsDeleted=0 ", info, new { sClientID = ID });
        }

        /// <summary>
        /// 载入店铺用户的订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_OrdersDTO> LoadClientOrdersByShopID(PageInfo info, Guid ID, Guid sShopID)
        {
            return query.PaginationQuery<EHECD_OrdersDTO>(@"SELECT ID,sOrderNo,sReceiverPhone,iTotalPrice,iType,
                                                            dBookTime,iState FROM [dbo].[EHECD_Orders] 
                                                            WHERE sClientID = @sClientID AND sStoreID=@sStoreID AND  bIsDeleted=0", info, new { sClientID = ID, sStoreID = sShopID });
        }


        /// <summary>
        /// 载入分享客分享的商品
        /// </summary>
        /// <param name="info"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> LoadSharedGoods(PageInfo info, Guid guid)
        {

            return query.PaginationQuery<Dictionary<string, object>>(@"SELECT A.*,B.sGoodsName,B.sGoodsPictures,C.sShopName FROM 
                                                                                EHECD_SharedGoods AS A 
                                                                                LEFT JOIN EHECD_Goods AS B
                                                                                ON B.ID=A.sGoodsID
                                                                                LEFT JOIN EHECD_ShopSet C
                                                                                ON B.sStoreId=C.sShopID
                                                                                WHERE sClientID = @sClientID", info, new { sClientID = guid });
        }


        ///汤台重写
        /// <summary>
        /// 载入分享客分享的商品
        /// </summary>
        /// <param name="info">参数</param>
        /// <param name="guid">分享客的ID</param>
        /// <returns>查询结果</returns>
        public override PagingRet<Dictionary<string, object>> LoadSharedGoodsList(PageInfo info, Guid guid, Guid sShopId)
        {
            return query.PaginationQuery<Dictionary<string, object>>(@"SELECT A.*,B.sGoodsName,B.sGoodsPictures FROM 
                                                                                EHECD_SharedGoods AS A 
                                                                                LEFT JOIN EHECD_Goods AS B
                                                                                ON B.ID=A.sGoodsID
                                                                                WHERE sClientID = @sClientID AND B.sStoreId=@sShopId", info, 
                                                                                new { sClientID = guid , sShopId = sShopId });
        }

        /// <summary>
        /// 查询该电话号码是否注册过
        /// </summary>
        /// <param name="sPhone"></param>
        public override bool IsExit(string sPhone)
        {
            bool res = true;
            var client=query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE sPhone=@sPhone AND bIsDeleted=0",new { sPhone= sPhone });
            if (client == null)
            {//不存在
                res = false;
            }
            return res;

        }
    }
}
