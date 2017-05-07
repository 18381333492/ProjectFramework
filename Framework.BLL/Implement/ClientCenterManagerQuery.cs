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
    public partial  class ClientCenterManager : IClientCenterManager
    {

        /// <summary>
        /// 根据会员ID获取会员昵称信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetClientNickName(Guid ClientId)
        {
            var dic=query.SingleQuery<Dictionary<string, object>>(@"SELECT 
                                                                            sNickName
                                                                            FROM EHECD_Client WHERE ID=@ID", new { ID= ClientId });
            //获取平台电话号码
            var temp = query.SingleQuery<Dictionary<string, object>>(@"SELECT sMallPhone  FROM EHECD_BaseSetting",null);

            dic.Add("sMallPhone", temp["sMallPhone"].ToString());
            return dic;
        }

        /// <summary>
        /// 根据会员ID获取会员信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetClientInfo(Guid ClientId)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT iClientType,
                                                                            sNickName,
                                                                            sHeadPic,
                                                                            sPhone,
                                                                            FROM EHECD_Client WHERE ID=@ID", new { ID = ClientId });
        }



        /// <summary>
        ///  根据会员主键ID获取待付款和待付款订单数目
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetPayUseOrderCount(Guid ClientId)
        {    ///0待付款 1待使用
            return query.SingleQuery<Dictionary<string, object>>(@"DECLARE @payCount int 
                                                SELECT  @payCount=COUNT(*) FROM (select * from EHECD_Orders where bIsDeleted=0) as entry where iState=0 and sClientID=@ID 
                                                SELECT @payCount as payCount, COUNT(*) as useCount FROM 
                                                (select * from EHECD_Orders where bIsDeleted=0) as entry where iState=1 and sClientID=@ID", new { ID= ClientId });
        }


        /// <summary>
        ///  根据会员主键ID获取站内信未读条数
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetNoReadMessageCount(Guid ClientId)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT COUNT(*) infoCount FROM EHECD_SysMessageDetail 
                                                                    Where bIsDeleted=0 AND iRecStatus=0 AND sReceiverID=@ID", new { ID= ClientId });
        }

        /// <summary>
        /// 分页获会员取站内信数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetSysMessageList(Guid ClientId, PageInfo info)
        {
            info.OrderBy = "dInsertTime";
            info.orderType = OrderType.DESC;
            return query.PaginationQuery<Dictionary<string, object>>(@"SELECT   A.ID,A.iRecStatus,A.dInsertTime,B.sMsgTitle,B.sMsgContent
                                                                                FROM EHECD_SysMessageDetail AS A
                                                                                LEFT JOIN EHECD_SysMessage AS B
                                                                                ON A.sMailID=B.ID
                                                                                Where A.bIsDeleted=0 AND A.sReceiverID=@ID", info,new { ID= ClientId });
        }


        /// <summary>
        /// 根据ID获取站内信的详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Dictionary<string, object> GetSysMessageDetail(Guid ID)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT A.dInsertTime,B.sMsgTitle,B.sMsgContent FROM 
                                                                           EHECD_SysMessageDetail AS A 
                                                                           LEFT JOIN EHECD_SysMessage AS B
                                                                           ON A.sMailID=B.ID WHERE A.ID=@ID",new { ID= ID });
        }


        /// <summary>
        /// 根据状态获取相应的订单的数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="iState">-1所有的订单 0待付款 1待使用 2-已核销 3-维权</param>
        /// <param name="info"></param>
        /// <returns></returns>
        public override object GetOrderListByState(Guid ClientId,int iState, PageInfo info)
        {
            info.OrderBy = "dBookTime";
            info.orderType = OrderType.DESC;
            StringBuilder Sql = new StringBuilder();
            Sql.Append(@"SELECT A.ID,A.sOrderNo,A.iTotalPrice,A.iState,A.dBookTime,A.iType,
                                                                                IsAppraise=(SELECT COUNT(*) FROM EHECD_Comment WHERE sOrderID=A.ID),--是否已评价该订单
                                                                                B.sGoodsName, B.iAmount, C.sGoodsPictures, D.sShopName, E.dStartTime, E.sEndTime,
                                                                                F.iState AS returnState--退货状态（0 - 退款审核中，1 - 接受申请，2 - 退款成功，3 - 拒绝退款）
                                                                                FROM  EHECD_Orders AS A
                                                                                LEFT JOIN  EHECD_OrdersGoods AS B
                                                                                ON A.ID = B.sOrderID
                                                                                LEFT JOIN EHECD_Goods AS C
                                                                                ON B.sGoodsPrimaryKey = C.ID
                                                                                LEFT JOIN EHECD_ShopSet AS D
                                                                                ON A.sStoreID = D.sShopID
                                                                                LEFT JOIN EHECD_RoomDetail AS E
                                                                                ON A.ID = E.sOrderId
                                                                                LEFT JOIN EHECD_ReturnOrders AS F
                                                                                ON A.ID = F.sOrderID
                                                                                WHERE A.bIsDeleted=0 AND A.sClientID=@ID");
            if (iState >= 0)
            {//根据状态查询的订单
                Sql.Append(" AND A.iState=@iState");
            }
            return query.PaginationQuery<Dictionary<string, object>>(Sql.ToString(), info, new { ID = ClientId, iState = iState });
        }


        /// <summary>
        /// 根据订单ID获取订单详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override object GetOrderDetail(Guid ID)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT A.ID,A.sOrderNo,A.iTotalPrice,A.iState,A.dBookTime,A.dPayTime,A.dFinishTime,A.iType,A.sDescribe,A.sReceiver,A.sReceiverPhone,
                                                                                IsAppraise=(SELECT COUNT(*) FROM EHECD_Comment WHERE sOrderID=A.ID),--是否已评价该订单
                                                                                B.sGoodsPrimaryKey,B.sGoodsName,B.iAmount,C.sGoodsPictures,D.sShopName,E.dStartTime,E.sEndTime,
                                                                                F.sNickName,F.sPhone,G.iUsePrice,G.iCoiCouponPrice,
                                                                                H.iState AS returnState --退货状态（0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款）
                                                                                FROM  EHECD_Orders AS A 
                                                                                LEFT JOIN  EHECD_OrdersGoods AS B
                                                                                ON A.ID=B.sOrderID 
                                                                                LEFT JOIN EHECD_Goods AS C 
                                                                                ON B.sGoodsPrimaryKey=C.ID
                                                                                LEFT JOIN EHECD_ShopSet AS D
                                                                                ON A.sStoreID=D.sShopID
                                                                                LEFT JOIN EHECD_RoomDetail AS E
                                                                                ON A.ID=E.sOrderId 
                                                                                LEFT JOIN EHECD_Client AS F
                                                                                ON A.sClientID=F.ID
                                                                                LEFT JOIN EHECD_Coupon AS G
                                                                                ON A.sCouponID=G.ID
                                                                                LEFT JOIN EHECD_ReturnOrders AS H
                                                                                ON A.ID=H.sOrderID
                                                                                WHERE A.ID=@ID", new { ID=ID});
        }


        /// <summary>
        /// 根据会员ID获取所有状态的订单
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetAllOrder(Guid ClientId, PageInfo info)
        {
            info.OrderBy = "dBookTime";
            info.orderType = OrderType.DESC;
            return query.PaginationQuery<Dictionary<string, object>>(@"SELECT A.ID,A.sOrderNo,A.iTotalPrice,A.iState,A.dBookTime,
                                                                                B.sGoodsName,B.iAmount
                                                                                FROM  EHECD_Orders AS A 
                                                                                LEFT JOIN  EHECD_OrdersGoods AS B
                                                                                ON A.ID=B.sOrderID 
                                                                                WHERE A.sClientID=@ID", info,new { ID = ClientId });
        }

        /// <summary>
        /// 根据订单ID获取订单商品评价
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public override Dictionary<string,object> GetAppraise(Guid sOrderId)
        {
            return query.SingleQuery<Dictionary<string, object>>(@"SELECT A.sCommentImgPath,A.sCommentContent,A.bIsReplay,A.sReplayContent,
                                                                            A.dReplayTime,A.iCommentScore,
                                                                            B.sGoodsName
                                                                            FROM  EHECD_Comment AS A
                                                                            LEFT JOIN EHECD_OrdersGoods AS B
                                                                            ON A.sOrderID=B.sOrderID
                                                                            WHERE A.sOrderID=@sOrderId", new { sOrderId= sOrderId });
        }


        /// <summary>
        /// 根据会员ID获取收藏夹
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="ClientId"></param>
        /// <param name="iCollectType">收藏类型:0-店铺，1-客房，2-票务，3-周边</param>
        /// <returns></returns>
        public override object GetCollectionList(PageInfo Info,Guid ClientId,int iCollectType)
        {
            //收藏类型iCollectType(0-店铺，1-客房，2-票务，3-周边)
            if (iCollectType == 0)
            {//店铺
                return query.PaginationQuery<Dictionary<string, object>>(@"SELECT A.ID, A.iCollectType, B.sShopName,B.sAddress,B.sShopID AS sGoodsId,
                                                                      iCommentScore=(SELECT SUM(iCommentScore)/COUNT(*) as Number FROM EHECD_Comment WHERE sStoreID=B.sShopID),
                                                                      dPrice=(SELECT MIN(dGoodsFisrtPrice) FROM dbo.EHECD_Goods WHERE sStoreId=B.sShopID),
                                                                      sImagePath=(SELECT TOP 1 sImagePath FROM EHECD_Images WHERE iState=6 AND sBelongID=B.sShopID) 
                                                                      FROM EHECD_Collect AS A
                                                                      LEFT JOIN EHECD_ShopSet AS B
                                                                      ON A.iGoodsID=B.sShopID 
                                                                      WHERE A.bIsDeleted=0 
                                                                      AND A.iCollectType=0
                                                                      AND A.iClientID=@ClientId", Info, new { ClientId = ClientId });
            }
            else
            {//商品
                return query.PaginationQuery<Dictionary<string, object>>(@"SELECT A.ID, A.iCollectType, B.sGoodsPictures,B.sGoodsName as sShopName,B.sGoodsCategory,B.dGoodsFisrtPrice,B.ID AS sGoodsId      
                                                                      FROM EHECD_Collect AS A
                                                                      LEFT JOIN EHECD_Goods AS B
                                                                      ON A.iGoodsID=B.ID
                                                                      WHERE A.bIsDeleted=0 
                                                                      AND iCollectType=@iCollectType
                                                                      AND A.iClientID=@ClientId", Info, new { iCollectType= iCollectType, ClientId = ClientId });
            }
                
        }


        /// <summary>
        /// 获取会员优惠卷数据列表
        /// </summary>
        /// <param name="Info"></param>
        /// <param name="ClientId"></param>
        /// <param name="bIsUsed">是否使用(0-否 1-是)</param>
        /// <returns></returns>
        public override object GetCouponList(PageInfo Info, Guid ClientId, int bIsUsed)
        {
            StringBuilder sSql = new StringBuilder();
            sSql.Append(@"SELECT A.bIsUsed,
                                 B.iCoiCouponPrice,B.iUsePrice,B.dValidDateStart,B.dValidDateEnd,
                                 C.sShopName
                                 FROM EHECD_CouponDetails AS A 
                                 LEFT JOIN EHECD_Coupon AS B
                                 ON A.sCouponID=B.ID
                                 LEFT JOIN EHECD_ShopSet AS C 
                                 ON B.sStoreID=C.sShopID
                                 WHERE B.bIsDeleted=0 
                                 AND A.sUserID=@sUserID AND B.iCoiCouponPrice!=0");
            if (bIsUsed == 0)
            {//未过期
                sSql.AppendFormat(" AND B.dValidDateEnd>='{0}' AND A.bIsUsed={1}", DateTime.Now.ToString("yyyy-MM-dd"), bIsUsed);
            }
            else
            {//已过期
                sSql.AppendFormat(" AND (B.dValidDateEnd<'{0}' OR A.bIsUsed={1})", DateTime.Now.ToString("yyyy-MM-dd"), bIsUsed);
            }
            DateTime Now = DateTime.Now;
            return query.PaginationQuery<Dictionary<string, object>>(sSql.ToString(), Info,new { sUserID= ClientId});
        }


        /// <summary>
        /// 获取会员的优惠券数据列表
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override object GetCouponList(Guid ClientId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取帮助中心数据列表
        /// </summary>
        /// <returns></returns>
        public override object GetHelpCenterList()
        {
            return query.QueryList<Dictionary<string, object>>(@"SELECT ID,sTitle FROM EHECD_Article Where bIsDeleted=0 ORDER BY dPublishTime DESC", null);
        }

        /// <summary>
        /// 根据ID获取帮助中心详情
        /// </summary>
        /// <returns></returns>
        public override object GetHelpCenterDetail(Guid ID)
        {
            return query.SingleQuery<EHECD_ArticleDTO>(@"SELECT sTitle,sContent FROM EHECD_Article WHERE ID=@ID", new { ID = ID });
        }

        
        /// <summary>
        /// 根据标题获取协议
        /// </summary>
        /// <param name="sTitle"></param>
        /// <returns></returns>
        public override object GetProtocolByTitle(string sTitle)
        {
            return query.SingleQuery<EHECD_ArticleDTO>(@"SELECT * FROM EHECD_Article WHERE sTitle=@sTitle AND bIsDeleted=0", new { sTitle = sTitle });
        }


        /// <summary>
        /// 根据电话判断用户是否是合伙人以及状态
        /// </summary>
        /// <param name="sPhone"></param>
        /// <param name="iState">0-未通过 1-已通过 2-拒绝</param>
        /// <returns></returns>
        public override bool IsPartner(string sPhone,out int iState)
        {
            var model= query.SingleQuery<EHECD_ApplyDTO>(@"SELECT * FROM EHECD_Apply 
                                                                    WHERE sMobileNum=@sPhone AND
                                                                    iType=0 AND bIsDeleted=0--申请类型(0-合伙人 1-商家)", new { sPhone = sPhone });
            if (model != null)
            {
                iState = model.iState.ToInt32();
                return true;
            }
            else
            {
                iState = -1;
                return false;
            }
        }


        /// <summary>
        /// 根据电话判断用户是否是商家以及申请状态
        /// </summary>
        /// <param name="sPhone"></param>
        /// <param name="iState">0-未通过 1-已通过 2-拒绝</param>
        /// <returns></returns>
        public override bool IsBusiness(string sPhone, out int iState)
        {
            var model = query.SingleQuery<EHECD_ApplyDTO>(@"SELECT * FROM EHECD_Apply 
                                                                    WHERE sMobileNum=@sPhone AND
                                                                    iType=1 AND bIsDeleted=0--申请类型(0-合伙人 1-商家)", new { sPhone = sPhone });
            if (model != null)
            {
                iState = model.iState.ToInt32();
                return true;
            }
            else
            {
                iState = -1;
                return false;
            }
        }

    }
}
