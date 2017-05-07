using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Framework.BLL
{
   public class MerchantManager:IMerchantManager
    {

        /// <summary>
        /// 本店所有订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetAllOrder(PageInfo page, string ID)
        {
            PagingRet<Dictionary<string, object>> info = query.PaginationQuery<Dictionary<string, object>>(@"SELECT a.ID,
	                                                                                                        b.sGoodsName,
                                                                                                            b.sGoodsPrimaryKey,
	                                                                                                        a.iState,
	                                                                                                        a.iTotalPrice
                                                                                                        FROM
	                                                                                                        EHECD_Orders a,
	                                                                                                        EHECD_OrdersGoods b
                                                                                                        WHERE
	                                                                                                        a.bIsDeleted = 0
                                                                                                        AND b.bIsDeleted = 0
                                                                                                        AND a.ID = b.sOrderID AND sStoreID =@sStoreID", page, new { sStoreID = ID });

            for (int i=0;i<info.Result.Count();i++) {
                if (int.Parse(info.Result[i]["iState"].ToString()) == 3)
                {
                    var orderState = query.SingleQuery<Dictionary<string, object>>("SELECT iState FROM EHECD_ReturnOrders WHERE sOrderID=@sOrderID", new { sOrderID = info.Result[i]["ID"] });
                    if (int.Parse(orderState["iState"].ToString()) == 0)
                    {
                        info.Result[i]["sOrder"] = "退款审核中";
                    }
                    if (int.Parse(orderState["iState"].ToString()) == 1)
                    {
                        info.Result[i]["sOrder"] = "接收审核";
                    }
                    if (int.Parse(orderState["iState"].ToString()) == 2)
                    {
                        info.Result[i]["sOrder"] = "退款成功";
                    }

                }
                
                if (int.Parse(info.Result[i]["iState"].ToString()) == 0)
                {
                    info.Result[i]["sOrder"] = "待付款";
                }
                if (int.Parse(info.Result[i]["iState"].ToString()) == 1)
                {
                    info.Result[i]["sOrder"] = "待使用";
                }
                if (int.Parse(info.Result[i]["iState"].ToString()) == 2)
                {
                    info.Result[i]["sOrder"] = "已核销";
                }
                var image = query.SingleQuery<Dictionary<string, object>>("SELECT  sGoodsPictures FROM EHECD_Goods WHERE ID=@ID", new { ID = info.Result[i]["sGoodsPrimaryKey"] });
                info.Result[i]["goodsImage"] = image["sGoodsPictures"];
            }

            
            return info;
        }
        /// <summary>
        /// 未支付订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string,object>> GetNotPay(PageInfo page, string ID)
        {
            PagingRet<Dictionary<string, object>> info = query.PaginationQuery<Dictionary<string, object>>(@"SELECT a.ID,
	                                                                                                        b.sGoodsName,
                                                                                                            b.sGoodsPrimaryKey,
	                                                                                                        a.iState,
	                                                                                                        a.iTotalPrice
                                                                                                        FROM
	                                                                                                        EHECD_Orders a,
	                                                                                                        EHECD_OrdersGoods b
                                                                                                        WHERE
	                                                                                                        a.bIsDeleted = 0
                                                                                                        AND b.bIsDeleted = 0
                                                                                                        AND a.iState=0
                                                                                                        AND a.ID = b.sOrderID AND sStoreID =@sStoreID", page, new { sStoreID = ID });

            if (info.Result.Count() != 0)
            {
                for (int i = 0; i < info.Result.Count(); i++)
                {
                    var image = query.SingleQuery<Dictionary<string, object>>("SELECT  sGoodsPictures FROM EHECD_Goods WHERE   ID=@ID", new { ID = info.Result[i]["sGoodsPrimaryKey"] });
                    info.Result[i]["goodsImage"] = image["sGoodsPictures"];
                }
            }
                return info;
            
        }
        /// <summary>
        /// 未使用订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetNotUsed(PageInfo page, string ID)
        {
            PagingRet<Dictionary<string, object>> info = query.PaginationQuery<Dictionary<string, object>>(@"SELECT a.ID,
	                                                                                                        b.sGoodsName,
                                                                                                            b.sGoodsPrimaryKey,
	                                                                                                        a.iState,
	                                                                                                        a.iTotalPrice
                                                                                                        FROM
	                                                                                                        EHECD_Orders a,
	                                                                                                        EHECD_OrdersGoods b
                                                                                                        WHERE
	                                                                                                        a.bIsDeleted = 0
                                                                                                        AND b.bIsDeleted = 0
                                                                                                        AND a.iState=1
                                                                                                        AND a.ID = b.sOrderID AND sStoreID =@sStoreID", page, new { sStoreID = ID });


            for (int i = 0; i < info.Result.Count(); i++)
            {
                var image = query.SingleQuery<Dictionary<string, object>>("SELECT  sGoodsPictures FROM EHECD_Goods WHERE ID=@ID", new { ID = info.Result[i]["sGoodsPrimaryKey"] });
                info.Result[i]["goodsImage"] = image["sGoodsPictures"];
            }
            return info;
        }
        /// <summary>
        /// 已使用订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetHaveUsed(PageInfo page, string ID)
        {
            PagingRet<Dictionary<string, object>> info = query.PaginationQuery<Dictionary<string, object>>(@"SELECT a.ID,
	                                                                                                        b.sGoodsName,
                                                                                                            b.sGoodsPrimaryKey,
	                                                                                                        a.iState,
	                                                                                                        a.iTotalPrice
                                                                                                        FROM
	                                                                                                        EHECD_Orders a,
	                                                                                                        EHECD_OrdersGoods b
                                                                                                        WHERE
	                                                                                                        a.bIsDeleted = 0
                                                                                                        AND b.bIsDeleted = 0
                                                                                                        AND a.iState=2
                                                                                                        AND a.ID = b.sOrderID AND sStoreID =@sStoreID", page, new { sStoreID = ID });


            for (int i = 0; i < info.Result.Count(); i++)
            {
                var image = query.SingleQuery<Dictionary<string, object>>("SELECT  sGoodsPictures FROM EHECD_Goods WHERE  ID=@ID", new { ID = info.Result[i]["sGoodsPrimaryKey"] });
                info.Result[i]["goodsImage"] = image["sGoodsPictures"];
            }
            return info;
        }
        /// <summary>
        /// 获取所有分销客
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_ClientDTO> GetStoreShare(PageInfo page, string ID)
        {
            return query.PaginationQuery<EHECD_ClientDTO>(@"SELECT
	                                                            ID,
	                                                            sNickName,
	                                                            sPhone,
	                                                            sHeadPic
                                                            FROM
	                                                            EHECD_Client
                                                            WHERE
	                                                            ID IN (
		                                                            SELECT
			                                                            sClientID
		                                                            FROM
			                                                            EHECD_SharedClientInfo
		                                                            WHERE
			                                                            sShopID = @sStoreID
		                                                            AND bIsDeleted = 0
	                                                            )
                                                            AND bIsDeleted = 0", page, new { sStoreID = ID });
        }
        /// <summary>
        /// 分销客分销商品
        /// </summary>
        /// <param name="page"></param>
        /// <param name="shopID"></param>
        /// <param name="sClientID"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetShareDetail(PageInfo page, string shopID, string sClientID)
        {
            var detail= query.PaginationQuery<Dictionary<string, object>>(@"SELECT sGoodsID FROM EHECD_SharedGoods WHERE sClientID='"+ sClientID + "' and sShopID='"+ shopID + "' AND bIsDeleted=0", page,null);
            for (int i = 0; i < detail.Result.Count(); i++) {
                var ii = detail.Result[i]["sGoodsID"];
                var goodDetail=query.SingleQuery<Dictionary<string,object>>("SELECT ID,sGoodsName,sGoodsPictures,dGoodsFisrtPrice FROM EHECD_Goods WHERE  ID=@ii",new { ii= ii});
                detail.Result[i]["goodname"] = goodDetail["sGoodsName"];
                detail.Result[i]["sGoodsPictures"] = goodDetail["sGoodsPictures"];
                detail.Result[i]["dGoodsFisrtPrice"] = goodDetail["dGoodsFisrtPrice"];
            }
            return detail;
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Dictionary<string, object> GetDetail(string ID)
        {
            var obj = query.SingleQuery<Dictionary<string, Object>>(@"SELECT
	                                                                a.ID,
	                                                                a.sOrderNo,
	                                                                a.iTotalPrice,
	                                                                a.dBookTime,
	                                                                a.dPayTime,
	                                                                a.dFinishTime,
	                                                                a.sStoreID,
	                                                                a.sDescribe,
	                                                                a.iState,
	                                                                a.sClientID,
	                                                                a.sCouponID,
                                                                    a.iType,a.sReceiver,
	                                                                b.sGoodsPrimaryKey
                                                                FROM
	                                                                EHECD_Orders a,
	                                                                EHECD_OrdersGoods b
                                                                WHERE
	                                                                a.bIsDeleted = 0
                                                                AND b.bIsDeleted = 0
                                                                AND a.ID = b.sOrderID
                                                                AND a.ID =@ID", new { ID = ID });
            var good = query.SingleQuery<Dictionary<string, object>>("SELECT sGoodsName,sGoodsPictures FROM EHECD_Goods WHERE  ID=@ID", new { ID = obj["sGoodsPrimaryKey"] });//获取商品名和商品图片

            var client = query.SingleQuery<Dictionary<string, object>>("SELECT sName,sPhone FROM EHECD_Client WHERE ID=@ID", new { ID = obj["sClientID"] });//购买客户的姓名以及手机

            obj["goodName"] = good["sGoodsName"];//商品名
            obj["goodPicture"] = good["sGoodsPictures"];//商品图片
            obj["clientName"] = client["sName"];//客户姓名
            obj["sPhone"] = client["sPhone"];//客户手机

                if (int.Parse(obj["iState"].ToString()) == 0)
                {
                    obj["orderState"] = "待付款";
                }
                if (int.Parse(obj["iState"].ToString()) == 1)
                {
                    obj["orderState"] = "待使用";
                }
                if (int.Parse(obj["iState"].ToString()) == 2)
                {
                    obj["orderState"] = "已核销";
                }
            if (int.Parse(obj["iState"].ToString()) == 3) {
                var goodState = query.SingleQuery<Dictionary<string, object>>("SELECT iState FROM EHECD_ReturnOrders WHERE sOrderID=@sOrderID", new { sOrderID = obj["ID"] });//查询维权的状态
                
                if (int.Parse(goodState["iState"].ToString()) == 3) {
                    obj["orderState"] = "退款审核中";
                }
                if (int.Parse(goodState["iState"].ToString()) == 1)
                {
                    obj["orderState"] = "接收申请";
                }
                if (int.Parse(goodState["iState"].ToString()) == 2)
                {
                    obj["orderState"] = "退款成功";
                }
                if (int.Parse(goodState["iState"].ToString()) == 3)
                {
                    obj["orderState"] = "拒绝退款";
                }
            }

            if (int.Parse(obj["iType"].ToString()) == 0)
            {//如果订单是客房
                var time = query.SingleQuery<Dictionary<string, object>>("SELECT CONVERT(VARCHAR(10),dStartTime,101) StartTime ,CONVERT(VARCHAR(10),sEndTime,101) EndTime,datediff(day,dStartTime,sEndTime) dayTotal FROM EHECD_RoomDetail   WHERE  sOrderId=@ID", new { ID = obj["ID"] });//查询房间的入住时间以及退房时间
                obj["inTime"] = DateTime.Parse(time["StartTime"].ToString()).Month + "/" + DateTime.Parse(time["StartTime"].ToString()).Day;//开始时间
                obj["outTime"] = DateTime.Parse(time["EndTime"].ToString()).AddDays(1).Month + "/" + DateTime.Parse(time["EndTime"].ToString()).AddDays(1).Day;//结束时间
                obj["totalTime"] = Convert.ToInt32(time["dayTotal"]) + 1;
            }
            if (obj["sCouponID"].ToString() != null || obj["sCouponID"].ToString() != "") {
                var coupon = query.SingleQuery<Dictionary<string, object>>("SELECT iCoiCouponPrice,iUsePrice FROM EHECD_Coupon WHERE bIsDeleted=0 AND ID=@ID", new { ID = obj["sCouponID"] });
                if (coupon != null)
                {
                    obj["couponprice"] = coupon["iCoiCouponPrice"];
                    obj["couponUse"] = coupon["iUsePrice"];
                }
                else {
                    obj["couponprice"] = "";
                    obj["couponUse"] = "";
                }
               
            }
           
            
                return obj;

        }
        /// <summary>
        /// 获取分销客的基本信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO GetShareClient(string ID)
        {
            return query.SingleQuery<EHECD_ClientDTO>(@"SELECT
                                                        ID,
                                                        sName,sNickName,
                                                        sPhone,
                                                        sHeadPic
                                                    FROM

                                                        EHECD_Client
                                                    WHERE  ID=@ID", new { ID = ID });
        }


       
    }
}
