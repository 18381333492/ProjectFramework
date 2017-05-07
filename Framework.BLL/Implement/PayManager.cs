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

    /// <summary>
    /// 支付相关的业务处理
    /// </summary>
    public  class PayManager : IPayManager
    {

        /// <summary>
        /// 根据订单ID检查订单状态
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public override int CheckOrder(Guid sOrderId)
        {
            var dic = query.SingleQuery<Dictionary<string, object>>(@"SELECT iState FROM EHECD_Orders WHERE bIsDeleted=0 AND ID=@ID", new { ID = sOrderId });
            int iState = dic["iState"].ToInt32();
            return iState;//订单状态 0待付款 1待使用 2-已核销 3-维权
        }


        /// <summary>
        /// 支付成功修改订单状态
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public override int AlterOrderState(string sOrderNo)
        {
            var order = query.QueryList<EHECD_OrdersDTO>(@"SELECT  * FROM  EHECD_Orders WHERE bIsDeleted=0 AND iState=0 AND sOrderNo=@sOrderNo", new { sOrderNo= sOrderNo });
            if (order != null && order.Count == 1)
            {//修改订单状态

                   //查询出购买会员
                var client = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM  EHECD_Client WHERE ID=@ID",new { ID=order[0].sClientID });

                   //查询该订单的订单商品
                var orderGoods = query.SingleQuery<EHECD_OrdersGoodsDTO>(@"SELECT * FROM EHECD_OrdersGoods WHERE sOrderID=@sOrderID", new { sOrderID = order[0].ID });

                   //查询该订单的商家
                //var buss = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT * FROM  EHECD_SystemUser WHERE ID=@ID", new { ID = order[0].sStoreID});
                    
                    //查询出基础设置的佣金三级比例
                var setting = query.SingleQuery<EHECD_BaseSettingDTO>(@"SELECT TOP 1 * FROM EHECD_BaseSetting",null);


                decimal totaliCommission =0;//订单的实际佣金
                /***一级会员***/
                var temp = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID=@ID",new { ID= client.sRegCode});

                StringBuilder sSql = new StringBuilder();
                if (temp != null)
                {
                    totaliCommission = totaliCommission + orderGoods.iCommission.Value * setting.iLevelOneCommissionPrecent.ToDecimal() * 0.01.ToDecimal();
                    //添加分享客的预计收益明细

                    var OneShare = new EHECD_ClientBalanceDetailDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sClientID = temp.ID,//一级会员ID
                        sOrderNo = sOrderNo,
                        iShopID = order[0].sStoreID,
                        iType = 11,//分享客预计收益
                        sOrderID = order[0].ID,
                        iClientType = 1,
                        iPrice = totaliCommission,
                        sRemark = "分享客预计收益",
                        sUserName = temp.sNickName,
                        iMethod = 1,
                        iCommissionPrice = totaliCommission,
                        iServicePrice = orderGoods.iServicePrice,
                        dChangeTime = order[0].dBookTime,
                        bIsDeleted = false
                    };

                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(OneShare));
                }
                /***二级会员***/
                temp = query.SingleQuery<EHECD_ClientDTO>(@"select * from EHECD_Client 
                                                                    WHERE ID IN(SELECT sRegCode  FROM EHECD_Client 
                                                                    WHERE ID=@ID)", new { ID = client.sRegCode });
                if (temp != null)
                {
                    totaliCommission = totaliCommission + orderGoods.iCommission.Value * setting.iLevelTwoCommissionPrecent.ToDecimal() * 0.01.ToDecimal();

                    //添加二级会员预计收益
                    var SecShare = new EHECD_ClientBalanceDetailDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sClientID = temp.ID,//二级会员ID
                        sOrderNo = sOrderNo,
                        iShopID = order[0].sStoreID,
                        iType = 11,//分享客预计收益
                        sOrderID = order[0].ID,
                        iClientType = 1,
                        iPrice = orderGoods.iCommission.Value * setting.iLevelTwoCommissionPrecent.ToDecimal() * 0.01.ToDecimal(),
                        sRemark = "分享客预计收益",
                        sUserName = temp.sNickName,
                        iMethod = 1,
                        iCommissionPrice = orderGoods.iCommission.Value * setting.iLevelTwoCommissionPrecent.ToDecimal() * 0.01.ToDecimal(),
                        iServicePrice = orderGoods.iServicePrice,
                        dChangeTime = order[0].dBookTime,
                        bIsDeleted = false
                    };
                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(SecShare));
                }

                /***三级会员***/
                temp = query.SingleQuery<EHECD_ClientDTO>(@" select * from  EHECD_Client 
                                                                            where ID IN (select sRegCode from EHECD_Client 
                                                                            WHERE ID IN (SELECT sRegCode  FROM EHECD_Client 
                                                                            WHERE ID=@ID))", new { ID = client.sRegCode });
                if (temp != null)
                {
                    totaliCommission = totaliCommission + orderGoods.iCommission.Value * setting.iLevelThreeCommissionPrecent.ToDecimal() * 0.01.ToDecimal();

                    //添加三级会员预计收益
                    var ThirShare = new EHECD_ClientBalanceDetailDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        sClientID = temp.ID,//三级会员ID
                        sOrderNo = sOrderNo,
                        iShopID = order[0].sStoreID,
                        iType = 11,//分享客预计收益
                        sOrderID = order[0].ID,
                        iClientType = 1,
                        iPrice = orderGoods.iCommission.Value * setting.iLevelThreeCommissionPrecent.ToDecimal() * 0.01.ToDecimal(),
                        sRemark = "分享客预计收益",
                        sUserName = temp.sNickName,
                        iMethod = 1,
                        iCommissionPrice = orderGoods.iCommission.Value * setting.iLevelThreeCommissionPrecent.ToDecimal() * 0.01.ToDecimal(),
                        iServicePrice = orderGoods.iServicePrice,
                        dChangeTime = order[0].dBookTime,
                        bIsDeleted = false
                    };
                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(ThirShare));
                }
                //添加购买记录
                var record = new EHECD_ClientBalanceDetailDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sClientID = order[0].sClientID,
                    sOrderID = order[0].ID,
                    sOrderNo = order[0].sOrderNo,
                    sRemark = "购买记录",
                    bIsDeleted = false,
                    dChangeTime = order[0].dBookTime,//订单的下单时间
                    sUserName = client.sNickName,
                    iType = 4,//购买
                    iShopID = order[0].sStoreID,
                    iMethod =2,//收入
                    iClientType=client.iClientType.ToByte(),
                    iCommissionPrice= totaliCommission,//实际给出的佣金
                    iServicePrice=orderGoods.iServicePrice,
                    iPrice= order[0].iTotalPrice- orderGoods.iServicePrice- totaliCommission,//实际的收入
                    iBeforePrice=0,
                    iAfterPrice=0,
                    PartnerID= order[0].sPartnerID,//该订单的合作人ID 
                };

                sSql.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_ClientBalanceDetailDTO>(record));
                //添加跟新订单状态的Sql语句
                sSql.AppendLine(DBSqlHelper.GetUpdateSQL<EHECD_OrdersDTO>(new EHECD_OrdersDTO()
                {
                    iState = 1,
                    dPayTime = DateTime.Now,
                }, string.Format("WHERE ID='{0}'", order[0].ID.ToString())));
                return  excute.ExcuteTransaction(sSql.ToString());
            }
            else
            {//订单处理失败
                return -1;
            }
        }

        /// <summary>
        /// 根据订单编号获取订单所属店铺的Openid
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public override string GetOpenid(string sOrderNo)
        {
            var data = query.SingleQuery<Dictionary<string,object>>(@"SELECT C.sOpenId
                                                            FROM  EHECD_Orders AS A 
                                                            LEFT JOIN EHECD_SystemUser AS B
                                                            ON A.sStoreID=B.ID
                                                            LEFT JOIN EHECD_Client AS C
                                                            ON B.sLoginName=C.sLoginName
                                                            WHERE A.bIsDeleted=0  AND A.sOrderNo=@sOrderNo", new { sOrderNo = sOrderNo });
            if (data != null)
            {
                return data["sOpenId"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
