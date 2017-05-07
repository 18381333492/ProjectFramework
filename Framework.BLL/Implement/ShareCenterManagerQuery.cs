using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.DI;
using Framework.MapperConfig;
using Framework.Helper;

namespace Framework.BLL
{
    public partial  class ShareCenterManager :IShareCenterManager
    {
        

        /// <summary>
        /// 根据会员ID获取分享客的相关信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetShareInfo(Guid ClientId)
        {

            string Sql = string.Format(@"SELECT ID FROM EHECD_Client WHERE sRegCode='{0}'
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}')
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client 
                                        WHERE sRegCode IN 
                                        (SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}'))", ClientId.ToString());


            //查询分享订单数
            var dic=query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT Count(*) AS OrderCount
                                                                                  FROM EHECD_Orders AS A 
                                                                                  LEFT JOIN dbo.EHECD_Client AS B
                                                                                  ON A.sClientID=B.ID
                                                                                  WHERE A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday 
                                                                                  AND A.sClientID IN({0})", Sql),null);

            //查询分享订单累计金额
            var temp = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT SUM(iTotalPrice) AS dPriceCount
                                                                                  FROM EHECD_Orders AS A 
                                                                                  LEFT JOIN dbo.EHECD_Client AS B
                                                                                  ON A.sClientID=B.ID
                                                                                  WHERE A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday 
                                                                                  AND A.sClientID IN({0})", Sql), null);
            dic.Add("dPriceCount", temp["dPriceCount"]);

            //查询分享客的下级会员总数
            temp = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT COUNT(*) AS ClientCount FROM  ({0}) as temp", Sql), null);
            dic.Add("ClientCount", temp["ClientCount"]);

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int Days = DateTime.DaysInMonth(year, month);
            string start = string.Format("{0}/{1}/1 00:00:00", year, month);
            string end = string.Format("{0}/{1}/{2} 23:59:59", year, month, Days);
            //查询本月的新增会员
            temp = query.SingleQuery<Dictionary<string, object>>(string.Format(@"SELECT COUNT(*) AS MonthClientCount 
                                                                                              FROM  EHECD_Client WHERE ID IN({0}) 
                                                                                              AND dAddTime>=@start AND dAddTime<=@end", Sql), new { start= start,end=end });
            dic.Add("MonthClientCount", temp["MonthClientCount"]);

            /******************************佣金的提成***********************************/

            dic.Add("predIncome", 0.ToDecimal());

            //查询分享客的预计收益

            temp = query.SingleQuery<Dictionary<string, object>>(@"select SUM(A.iPrice) AS predIncome from  dbo.EHECD_ClientBalanceDetail AS A
                                                                    LEFT JOIN dbo.EHECD_Orders AS B
                                                                    ON A.sOrderID=B.ID
                                                                    LEFT JOIN dbo.EHECD_ReturnOrders AS C 
                                                                    ON A.sOrderID=C.sOrderID
                                                                    where A.iType=11 AND A.iClientType=1 
                                                                    AND A.sClientID=@sClientID
                                                                    AND (B.iState=1 OR (B.iState=3 AND C.iState!=2 AND C.iState!=1))", new { sClientID=ClientId });
            if(temp!=null&& temp["predIncome"] != null)
            {
                dic["predIncome"] = temp["predIncome"].ToDecimal();
            }
            return dic;
        }


        /// <summary>
        /// 获取分享客会员
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="type">0-全部，1：一级，2：二级，3：三级</param>
        ///  <param name="info"></param>
        /// <returns></returns>
        public override object GetMember(Guid ClientId,int type,PageInfo info)
        {
            info.OrderBy = "dAddTime";
            info.orderType = OrderType.DESC;
            string Sql = string.Empty;
            switch (type)
            {
                case 0:Sql= string.Format(@"SELECT ID FROM EHECD_Client WHERE sRegCode='{0}'
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}')
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client 
                                        WHERE sRegCode IN 
                                        (SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}'))", ClientId.ToString());break;
                    //一级会员
                case 1: Sql = string.Format(@"SELECT ID FROM EHECD_Client WHERE sRegCode='{0}'", ClientId.ToString()); break;
                    //二级会员
                case 2: Sql = string.Format(@"SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}')", ClientId.ToString());break;
                    //三级会员
                case 3:Sql=string.Format(@"SELECT ID FROM  EHECD_Client 
                                                          WHERE sRegCode IN 
                                        (SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}'))"
                                       , ClientId.ToString());break;

            }

           return query.PaginationQuery<Dictionary<string, object>>(string.Format(@"SELECT C.dAddTime,C.sHeadPic,C.sNickName,C.sPhone,
                                                         OrderCount=(SELECT COUNT(*) FROM EHECD_Orders AS A LEFT JOIN EHECD_Client AS B    
                                                                     ON A.sClientID=B.ID 
                                                                     WHERE  A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday AND C.ID=A.sClientID),
                                                         OrderdPrice=(SELECT SUM(iTotalPrice) FROM EHECD_Orders AS A LEFT JOIN EHECD_Client AS B
                                                                     ON A.sClientID=B.ID 
                                                                     WHERE  A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday AND C.ID=A.sClientID)
                                                         FROM EHECD_Client AS C
                                                         WHERE C.ID IN({0})", Sql), info, null);
       
        }



        /// <summary>
        /// 获取本月新增会员数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override object GetNewlyMember(Guid ClientId,PageInfo info)
        {
            info.OrderBy = "dAddTime";
            info.orderType = OrderType.DESC;
            string Sql = string.Format(@"SELECT ID FROM EHECD_Client WHERE sRegCode='{0}'
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}')
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client 
                                        WHERE sRegCode IN 
                                        (SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}'))", ClientId.ToString());
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int Days = DateTime.DaysInMonth(year, month);
            string start = string.Format("{0}/{1}/1 00:00:00", year, month);
            string end = string.Format("{0}/{1}/{2} 23:59:59", year, month, Days);

            return query.PaginationQuery<Dictionary<string, object>>(string.Format(@"SELECT C.dAddTime,C.sHeadPic,C.sNickName,C.sPhone,
                                                         OrderCount=(SELECT COUNT(*) FROM EHECD_Orders AS A LEFT JOIN EHECD_Client AS B    
                                                                     ON A.sClientID=B.ID 
                                                                     WHERE  A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday AND C.ID=A.sClientID),
                                                         OrderdPrice=(SELECT SUM(iTotalPrice) FROM EHECD_Orders AS A LEFT JOIN EHECD_Client AS B
                                                                     ON A.sClientID=B.ID 
                                                                     WHERE  A.bIsDeleted=0 AND A.iState>0 AND A.iState<3 AND A.dBookTime>=B.dBirthday AND C.ID=A.sClientID)
                                                         FROM EHECD_Client AS C
                                                         WHERE C.ID IN({0}) 
                                                         AND C.dAddTime>=@start AND C.dAddTime<=@end", Sql), info,new { start = start, end = end });
        }



        /// <summary>
        /// 获取分享客的收支明细数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override object PaymentDetail(Guid ClientId, PageInfo info)
        {
            info.OrderBy = "dFinishTime";
            info.orderType = OrderType.DESC;
            string Sql = string.Format(@"SELECT ID FROM EHECD_Client WHERE sRegCode='{0}'
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}')
                                        UNION  ALL
                                        SELECT ID FROM  EHECD_Client 
                                        WHERE sRegCode IN 
                                        (SELECT ID FROM  EHECD_Client WHERE sRegCode IN(SELECT ID  FROM EHECD_Client WHERE sRegCode='{0}'))", ClientId.ToString());

           return  query.PaginationQuery<Dictionary<string, object>>(string.Format(@"SELECT  A.dFinishTime,
                                                                 A.sOrderNo,B.sGoodsName,A.iType,C.sGoodsPictures,B.iCommission,
                                                                 B.iSinglePrice,C.iCommissionType,C.dMoney,--iCommissionType:佣金类型(1--固定金额，2--商品价格比例)
                                                                 D.sNickName,E.sShopName,F.dStartTime,F.sEndTime
                                                                 FROM EHECD_Orders AS A 
                                                                 LEFT JOIN EHECD_OrdersGoods AS B
                                                                 ON A.ID=B.sOrderID
                                                                 LEFT JOIN EHECD_Goods AS C
                                                                 ON B.sGoodsPrimaryKey=C.ID
                                                                 LEFT JOIN EHECD_Client AS D
                                                                 ON D.ID=A.sClientID
                                                                 LEFT JOIN EHECD_ShopSet AS E
                                                                 ON A.sStoreID=E.sShopID
                                                                 LEFT JOIN EHECD_RoomDetail AS F
                                                                 ON F.sOrderId=A.ID
                                                                 WHERE A.iState>0 AND A.iState<3 AND A.dBookTime>=D.dBirthday  AND A.sClientID IN({0})", Sql), info,null);

        }


        /// <summary>
        /// 获取提现记录
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override object GetCashRecord(Guid ClientId, PageInfo info)
        {
            info.OrderBy = "dApplyTime";
            info.orderType = OrderType.DESC;
            return query.PaginationQuery<Dictionary<string, object>>(@"SELECT dApplyTime,iWithdrawMoney,iState
                                                                                        FROM EHECD_WithdrawCash 
                                                                                        WHERE sWithdrawMemberType=2 AND sWithdrawMemberID=@ClientId", info, new { ClientId = ClientId });
        }


        /// <summary>
        /// 获取结算中心的相关信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetAccountCenterInfo(Guid ClientId)
        {
            List<EHECD_WithdrawCashDTO> list = query.QueryList<EHECD_WithdrawCashDTO>(@"SELECT * FROM EHECD_WithdrawCash 
                                                                            WHERE sWithdrawMemberType=2 AND sWithdrawMemberID=@ClientId", new { ClientId = ClientId }).ToList();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            //已体现的
            var WithdrawEd = list.Where(m => m.iState == 2).Sum(m => m.iWithdrawMoney);
            //提现审核中的
            var WithdrawING= list.Where(m => m.iState <2).Sum(m => m.iWithdrawMoney);

            dic.Add("WithdrawEd", WithdrawEd);
            dic.Add("WithdrawING", WithdrawING);
            dic.Add("dIncome", GetClientMoney(ClientId));


            return dic;
        }


        /// <summary>
        /// 获取分享客的可提现金额
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override  decimal GetClientMoney(Guid ClientId)
        {
            var dic = query.SingleQuery<Dictionary<string, object>>(@"SELECT dIncome FROM EHECD_Client WHERE ID=@ClientId",new { ClientId = ClientId });
            return dic["dIncome"].ToDecimal();
        }


        /// <summary>
        /// 获取分享客店铺的数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="Info"></param>
        /// <param name="bIsShare">是否分享 0-未分享，1-已分享</param>
        /// <param name="sKeyword">店铺名字</param>
        /// <returns></returns>
        public override object GetShopManagement(Guid ClientId,PageInfo Info, int bIsShare,string sKeyword)
        {
            object data = null;
            string search = string.Empty;
            if (!string.IsNullOrEmpty(sKeyword))
            {
                search = string.Format(" AND sShopName LIKE '%{0}%'", sKeyword);
            }
            if (bIsShare == 1)
            {

                string Sql = @"SELECT  
                                                                            1 AS bIsDeleted,B.sShopName,A.sShopID,
                                                                            GoodsCount=(SELECT COUNT(*) FROM EHECD_Goods WHERE sStoreId=A.sShopID),
                                                                            sImagePath=(SELECT TOP 1 sImagePath FROM EHECD_Images WHERE iState=6 AND  sBelongID=A.sShopID)
                                                                            FROM EHECD_SharedClientInfo AS A
                                                                            LEFT JOIN EHECD_ShopSet AS B
                                                                            ON A.sShopID=B.sShopID
                                                                            LEFT JOIN EHECD_SystemUser AS C
                                                                            ON B.sShopID=C.ID
                                                                            WHERE C.bIsDeleted=0 AND C.tUserState=0 AND A.bIsForzen=0 AND A.sClientID=@sClientID";
                 data = query.PaginationQuery<Dictionary<string, object>>(Sql+search, Info, new { sClientID = ClientId });
            }

            if(bIsShare == 0)
            { //未分享

                string Sql = @"SELECT  
                               0 AS bIsDeleted ,A.sShopName,A.sShopID,
                               GoodsCount=(SELECT COUNT(*) FROM EHECD_Goods WHERE sStoreId=A.sShopID), 
                               sImagePath=(SELECT TOP 1 sImagePath FROM EHECD_Images WHERE iState=6 AND  sBelongID=A.sShopID)
                               FROM EHECD_ShopSet AS A
                               LEFT JOIN EHECD_SystemUser AS B
                               ON A.sShopID=B.ID
                               WHERE B.tUserState=0 AND B.bIsDeleted=0 AND
                               A.sShopID 
                               NOT IN
                              (SELECT sShopID FROM EHECD_SharedClientInfo WHERE bIsForzen=0 AND  sClientID=@sClientID)";
                data = query.PaginationQuery<Dictionary<string, object>>(Sql + search, Info, new { sClientID = ClientId });

            }

            return data;
        }


        /// <summary>
        /// 获取店铺下面的商品列表
        /// </summary>
        /// <param name="sShopId"></param>
        /// <param name="Info"></param>
        /// <param name="sGoodsName"></param>
        /// <returns></returns>
        public override object GetShopGoodsList(Guid sShopId, PageInfo Info, string sGoodsName)
        {

            StringBuilder sSql = new StringBuilder();
            sSql.Append(@"SELECT ID,sGoodsName,sGoodsPictures,dGoodsFisrtPrice,iCommissionType,dMoney,
                                 OneLevel=(SELECT iLevelOneCommissionPrecent FROM  EHECD_BaseSetting),
                                 SecLevel=(SELECT iLevelTwoCommissionPrecent FROM  EHECD_BaseSetting),
                                 ThridLevel=(SELECT iLevelThreeCommissionPrecent FROM  EHECD_BaseSetting)
                                 FROM EHECD_Goods
                                 WHERE  bIsDeleted=0 AND bShelves=1 AND sStoreId=@sShopId");

            if (!string.IsNullOrEmpty(sGoodsName))
            {
                sSql.AppendFormat(" AND sGoodsName LIKE '%{0}%'", sGoodsName);
            }
            return query.PaginationQuery<Dictionary<string, object>>(sSql.ToString(),Info,new { sShopId= sShopId });
        }


        /// <summary>
        /// 获取分享海报管理图片
        /// </summary>
        /// <returns></returns>
        public override string GetSharePosterImg()
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT TOP 1 sImagePath FROM EHECD_Images WHERE iState=2",null)["sImagePath"].ToString();
        }


        /// <summary>
        /// 获取会员的上三级会员
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public override List<clientData> Gethigher(Guid ClientID)
        {
            var list = new List<clientData>();

            //查询一级
            var one = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID=@ID",new { ID= ClientID });
            if (one != null)
            {
                list.Add(new clientData()
                {
                    sNickName = one.sNickName,
                    sOpenId = one.sOpenId,
                    sPhone = one.sPhone,
                    order = 1
                });
            }
            //查询二级
            var sec = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID IN (SELECT sRegCode FROM EHECD_Client WHERE ID=@ID)", new { ID = ClientID });
            if (sec != null)
            {
                list.Add(new clientData()
                {
                    sNickName = sec.sNickName,
                    sOpenId = sec.sOpenId,
                    sPhone = sec.sPhone,
                    order = 2
                });
            }

            //查询三级
            var thid = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID IN(SELECT sRegCode FROM EHECD_Client 
                                                                    WHERE ID IN (SELECT sRegCode FROM EHECD_Client WHERE ID=@ID))", new { ID = ClientID });
            if (thid != null)
            {
                list.Add(new clientData()
                {
                    sNickName = thid.sNickName,
                    sOpenId = thid.sOpenId,
                    sPhone = thid.sPhone,
                    order = 3
                });
            }
            return list;
        }
    }
}
