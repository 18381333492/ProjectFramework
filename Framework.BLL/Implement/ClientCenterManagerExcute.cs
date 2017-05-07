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
using Framework.web.config;

namespace Framework.BLL
{
    public partial  class ClientCenterManager : IClientCenterManager
    {
        #region 会员中心的相关操作

        
        /// <summary>
        /// 设置站内信为为以读
        /// </summary>
        /// <param name="ID"></param>
        public override int SetiRecStatus(Guid ID)
        {
           return  excute.UpdateSingle<EHECD_SysMessageDetailDTO>(new EHECD_SysMessageDetailDTO(){  iRecStatus=1 },
                    string.Format("WHERE ID='{0}'",ID.ToString()));
        }


        /// <summary>
        /// 成为分享客
        /// </summary>
        /// <param name="ClientId"></param>
        /// <returns></returns>
        public override int IntoShareClient(Guid ClientId)
        {
            var Random = new RandomHelper();
            Random._numberchar = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("numbers").ToString().Split(',');
            Random._chchar = WebConfig.LoadDynamicJson("RandomMessagerCharArray").GetValue("enChars").ToString().Split(',');
            string sIDCard = Random.GetMixNumberLetter().ToUpper();
            //iClientType:0-普通客户,1-分销客
            return excute.Update(@"UPDATE EHECD_Client SET iClientType=1,sIDCard=@sIDCard WHERE ID=@ClientId",new { sIDCard = sIDCard , ClientId = ClientId });
        }


        /// <summary>
        /// 成为合伙人
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int IntoPartner(EHECD_ApplyDTO model)
        {
            int iState;
            if (!IsPartner(model.sMobileNum, out iState))
            {
                model.ID = Helper.GuidHelper.GetSecuentialGuid();
                model.sAddress = model.sProvice + model.sCity + model.sCounty;
                model.dCreateTime = DateTime.Now;
                model.iState = 0;//申请状态时为未通过.
                model.iType = false;//合伙人
                return excute.InsertSingle<EHECD_ApplyDTO>(model);
            }
            else
            { //标识已经申请过.编辑该合伙人信息
                var temp = query.SingleQuery<EHECD_ApplyDTO>(@"SELECT * FROM EHECD_Apply 
                                                                    WHERE sMobileNum=@sPhone AND
                                                                    iType=0 --申请类型(0-合伙人 1-商家) AND bIsDeleted=0", new { sPhone = model.sMobileNum });
                temp.dCreateTime = DateTime.Now;
                temp.iState = 0; //申请状态时为未通过.
                temp.sName = model.sName;
                temp.sMainBusiness = model.sMainBusiness;
                temp.sProvice = model.sProvice;
                temp.sCity = model.sCity;
                temp.sCounty = model.sCounty;
                temp.sAddress = model.sProvice + model.sCity + model.sCounty;
                return excute.UpdateSingle<EHECD_ApplyDTO>(temp, string.Format("WHERE ID='{0}'", temp.ID));
            }
        }

        /// <summary>
        /// 成为商家
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int IntoBusiness(EHECD_ApplyDTO model)
        {
            int iState;
            if (!IsBusiness(model.sMobileNum, out iState))
            {
                model.ID = Helper.GuidHelper.GetSecuentialGuid();
                // model.sAddress = model.sProvice + model.sCity + model.sCounty;
                model.dCreateTime = DateTime.Now;
                model.iState = 0;//申请状态时为未通过.
                model.iType = true;//商家
                return excute.InsertSingle<EHECD_ApplyDTO>(model);
            }
            else
            { //标识已经申请过.编辑该商家信息
                var temp = query.SingleQuery<EHECD_ApplyDTO>(@"SELECT * FROM EHECD_Apply 
                                                                    WHERE sMobileNum=@sPhone AND
                                                                    iType=1 --申请类型(0-合伙人 1-商家) AND bIsDeleted=0", new { sPhone = model.sMobileNum });
                temp.sShopName = model.sShopName;
                temp.dCreateTime = DateTime.Now;
                temp.iState = 0; //申请状态时为未通过.
                temp.sName = model.sName;
                temp.sMainBusiness = model.sMainBusiness;
                temp.sProvice = model.sProvice;
                temp.sCity = model.sCity;
                temp.sCounty = model.sCounty;
                temp.sAddress = model.sProvice + model.sCity + model.sCounty;
                return excute.UpdateSingle<EHECD_ApplyDTO>(temp, string.Format("WHERE ID='{0}'", temp.ID));
            }
        }


        /// <summary>
        /// 根据电话号码检查该用户是否是商家，合伙人,或者平台用户
        /// </summary>
        /// <param name="sPhone"></param>
        /// <returns></returns>
        public override int Check(string sPhone)
        {
            int res =-1;
            var user = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT * FROM EHECD_SystemUser WHERE sLoginName=@sLoginName", new { sLoginName = sPhone });
            if (user != null)
            {
                res = user.tUserType.Value;//用户类型 0：后台用户，1：店铺，2：合伙人
            }
            else
            {
                //查询注册申请表
                var model = query.SingleQuery<EHECD_ApplyDTO>(@"SELECT * FROM EHECD_Apply 
                                                                    WHERE sMobileNum=@sPhone AND iState=0 AND bIsDeleted=0 --审核状态中的
                                                                     --申请类型(0-合伙人 1-商家)", new { sPhone = sPhone });
                if (model != null)
                {
                    if (model.iType==false)
                    {
                        res = -11;//合伙人
                    }
                    else
                    {
                        res = -22;//商家
                    }      
                }
                else
                {
                    res=10;//可以申请
                }
            }
            return res;
        }

   
        /// <summary>
        /// 评价订单
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public override int OrderAppraise(EHECD_CommentDTO comment)
        {
            var data = query.SingleQuery<Dictionary<string, object>>(@"SELECT A.ID,A.sOrderNo,A.sStoreID,
                                                                                B.sGoodsPrimaryKey AS sGoodsID
                                                                                FROM  EHECD_Orders AS A
                                                                                LEFT JOIN  EHECD_OrdersGoods AS B
                                                                                ON A.ID = B.sOrderID 
                                                                                WHERE A.ID=@sOrderID", new { sOrderID=comment.sOrderID});
            comment.ID = Helper.GuidHelper.GetSecuentialGuid();
            comment.sOrderID =data["ID"].ToGuid();
            comment.sOrderNo = data["sOrderNo"].ToString();
            comment.sGoodsID= data["sGoodsID"].ToGuid();
            comment.sStoreID = data["sStoreID"].ToGuid();
            comment.dCommentTime = DateTime.Now;
            comment.bIsReplay = false;//是否回复  默认false;
            return excute.InsertSingle<EHECD_CommentDTO>(comment);
        }


        /// <summary>
        /// 根据订单ID取消订单
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public override int OrderCancel(Guid sOrderId)
        {
            return excute.UpdateSingle<EHECD_OrdersDTO>(new EHECD_OrdersDTO()
            {
                bIsDeleted = true
            },string.Format("WHERE ID='{0}'", sOrderId.ToString()));
        }


        /// <summary>
        /// 申请订单退款
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public override int OrderReturn(Guid sOrderId)
        {
            EHECD_OrdersDTO order = query.SingleQuery<EHECD_OrdersDTO>(@"SELECT * FROM EHECD_Orders WHERE ID=@sOrderId",new { sOrderId= sOrderId });
            EHECD_OrdersGoodsDTO orderGoods = query.SingleQuery<EHECD_OrdersGoodsDTO>(@"SELECT * FROM EHECD_OrdersGoods WHERE sOrderID=@sOrderId", new { sOrderId = sOrderId });

            if (order.iState == 1)
            {//待使用的订单 才能申请退款
                StringBuilder sSql = new StringBuilder();
                //添加退货订单
                EHECD_ReturnOrdersDTO returnOrder = new EHECD_ReturnOrdersDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    iState = 0,//状态状态（0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款）
                    sOrderID = order.ID,
                    iTotalMoney = order.iTotalPrice,//退款的实际金额
                    sClientID = order.sClientID,
                    sReturnNo = DateTime.Now.ToString("yyyyMMddHHmmssfff"),//退货单号
                    dInsertTime = DateTime.Now,
                    sDescribe = order.sDescribe,
                    sReason = string.Empty,
                    iOriginalTotaMoney = order.iOriginalTotalPrice,
                    bIsDeleted = false
                };
                //添加退货订单商品
                EHECD_ReturnOrdersGoodsDTO returnOrderGoods = new EHECD_ReturnOrdersGoodsDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    iAmount = orderGoods.iAmount.ToByte(),
                    sReturnID = returnOrder.ID,
                    sReturnNo = returnOrder.sReturnNo,
                    sClientID = order.sClientID,
                    iSinglePrice = orderGoods.iSinglePrice,//商品单价
                    dTotalPrice = orderGoods.iSinglePrice * orderGoods.iAmount,//商品总价
                    sGoodsName = orderGoods.sGoodsName,
                    sGoodsPrimaryKey = orderGoods.sGoodsPrimaryKey,
                    bIsDeleted = false,
                };
                //修改订单状态
                sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_OrdersDTO>(new EHECD_OrdersDTO()
                {
                    iState = 3//维权
                }, string.Format("WHERE ID='{0}'", sOrderId.ToString())));
                sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_ReturnOrdersDTO>(returnOrder));
                sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_ReturnOrdersGoodsDTO>(returnOrderGoods));

                return excute.ExcuteTransaction(sSql.ToString());
            }
            else
            {
                return -2;//订单已核销
            }
        }



        /// <summary>
        /// 根据ID删除收藏夹
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public override int CancelCollect(string Ids)
        {
            string sSql = string.Format(@"UPDATE EHECD_Collect SET bIsDeleted=1 WHERE ID IN({0})", Ids);
            return excute.Update(sSql, null);
        }


        /// <summary>
        /// 修改个人信息
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="sNickName"></param>
        /// <param name="sHeadPic"></param>
        /// <returns></returns>
        public override int AlertInfo(Guid ClientId,string sNickName,string sHeadPic)
        {
            return excute.UpdateSingle<EHECD_ClientDTO>(new EHECD_ClientDTO()
            {
                sNickName = sNickName,
                sHeadPic = sHeadPic
            }, string.Format("WHERE ID='{0}'", ClientId.ToString()));
        }
        #endregion
    }
}
