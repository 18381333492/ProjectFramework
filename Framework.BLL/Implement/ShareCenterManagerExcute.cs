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
    public partial class ShareCenterManager : IShareCenterManager
    {
        #region 分享中心的相关操作

        /// <summary>
        /// 分享店铺
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override int ShareShop(EHECD_SharedClientInfoDTO model)
        {
            //查询是否是该店铺的分享客
            var temp = query.SingleQuery<EHECD_SharedClientInfoDTO>(@"SELECT * FROM EHECD_SharedClientInfo WHERE sClientID=@sClientID AND sShopID=@sShopID",
                                                                    new { sClientID = model.sClientID, sShopID = model.sShopID });
            if (temp == null)
            {
                model.ID = GuidHelper.GetSecuentialGuid();
                model.dInsertTime = DateTime.Now;
                model.bIsForzen = false;//默认未被冻结
                return excute.InsertSingle<EHECD_SharedClientInfoDTO>(model);
            }
            else
            {
                if (temp.bIsForzen == true)//被冻结
                    return -1;
                else return -2;
            }
        }


        /// <summary>
        /// 取消店铺的分享
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="sShopId"></param>
        /// <returns></returns>
        public override int CancelShare(Guid ClientId, Guid sShopId)
        {

            return excute.Update(@"DELETE  EHECD_SharedClientInfo WHERE sClientID = @sClientID AND sShopID = @sShopID",
                                                                       new { sClientID = ClientId, sShopID = sShopId });
        }


        /// <summary>
        /// 扫码成为下级
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public override int IntoMember(EHECD_ClientDTO client, out string sNickName,out string sPhone)
        {
            var temp = query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE sOpenId=@sOpenId", new { sOpenId = client.sOpenId });//当前会员
            sNickName = client.sNickName;
            sPhone = string.Empty;
            if (temp == null)
            {//没有注册过 直接成为他的下级
                client.sIDType = "-1";//非一级会员
                return excute.InsertSingle<EHECD_ClientDTO>(client);
            }
            else
            {//存在直接修改
             //查询上级
             // var user = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID=@ID", new { client.sRegCode });
                sPhone = temp.sPhone;
                if (temp.ID == client.sRegCode)
                {
                    return -2;//自己不能成为自己的下级
                }
                if (temp.sIDType == "1")
                    return -5;//你是一级会员不能成为下线
                //============== 如果数据库中存在当前用户，判断是否有上级【一个上级能有多个下级，但是一个下级只能有一个上级】===========
                //判断当前用户是否有上级
                var dic = query.SingleQuery<Dictionary<string, object>>(@"SELECT COUNT(*) AS Number FROM EHECD_Client WHERE ID=@ID", new { ID = temp.sRegCode });
                if (dic["Number"].ToInt32() > 0)
                {//存在上级
                    return -1;//一个下级只能有一个上级
                }
                else
                {//当前会员没有上级

                    //查询分享的链接的会员的上级
                    var higher = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client 
                                                                        WHERE ID IN(SELECT sRegCode FROM EHECD_Client WHERE ID=@ID)", new { ID = client.sRegCode });

                    if (higher != null)
                    {
                        if (higher.ID.ToString() == temp.ID.ToString())
                        {//当前会员的上级是发展的下级 造成循环依赖
                            return -2;
                        }
                        else
                        {
                            //查询当前的会员的所有下级
                            var allclient = query.QueryList<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE sRegCode=@sRegCode", new { sRegCode = temp.ID });

                            //查询分享链接的会员信息
                            var shareClient = query.SingleQuery<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client WHERE ID=@ID", new { ID = client.sRegCode });

                            var havaLower = false;//标识
                            //A的上级都不能是当前的会员的所有下级任何一个
                            foreach (var item in allclient)
                            {
                                if (shareClient.sRegCode == item.ID)
                                {
                                    havaLower = true;
                                    break;
                                }
                            }

                            //查询当前的会员的所有下级的下级
                            var allclienttoclient = query.QueryList<EHECD_ClientDTO>(@"SELECT * FROM EHECD_Client 
                                                                        WHERE sRegCode IN (SELECT ID FROM EHECD_Client WHERE sRegCode=@sRegCode)", new { sRegCode = temp.ID });

                            //当前会员的下级的下级都不能是分享链接的上级
                            foreach (var item in allclienttoclient)
                            {
                                if (shareClient.sRegCode == item.ID)
                                {
                                    havaLower = true;
                                    break;
                                }
                            }

                            if (havaLower == false)
                            {//跟新信息成为下级

                                sNickName = temp.sNickName;
                                int res = excute.UpdateSingle<EHECD_ClientDTO>(new EHECD_ClientDTO()
                                {
                                    dBirthday = DateTime.Now,
                                    sRegCode = client.sRegCode
                                }, string.Format(" WHERE ID='{0}'", temp.ID.ToString()));
                                return res;
                            }
                            else
                            {//造成三级分销循环依赖
                                return -3;
                            }
                        }
                    }
                    else
                    { //当前分享链接的会员没有上级

                        sNickName = temp.sNickName;

                        int res = excute.UpdateSingle<EHECD_ClientDTO>(new EHECD_ClientDTO()
                        {
                            dBirthday = DateTime.Now,
                            sRegCode = client.sRegCode
                        }, string.Format(" WHERE ID='{0}'", temp.ID.ToString()));
                        return res;
                    }

                }
            }
        }
        

        /// <summary>
        /// 分享客的提现
        /// </summary>
        /// <param name="cash"></param>
        /// <param name="sResult">返回的提示信息</param>
        /// <returns></returns>
        public override int WithdrawCashHandle(EHECD_WithdrawCashDTO cash, out string sResult)
        {
            StringBuilder sSql = new StringBuilder();
            sResult = string.Empty;
            var setting = query.SingleQuery<EHECD_WithdrawSettingDTO>(@"SELECT * FROM EHECD_WithdrawSetting WHERE sRoleName='分享客'", null);
            //分享客提现的时间间隔
            int iIntervalDays = setting.iIntervalDays.ToInt32();

            var client = query.SingleQuery<Dictionary<string, object>>(@"SELECT dIncome FROM EHECD_Client WHERE ID=@ID", new { ID = cash.sWithdrawMemberID });
            decimal drawcash = client["dIncome"].ToDecimal();


            var record = query.SingleQuery<EHECD_WithdrawCashDTO>(@"SELECT TOP 1 * FROM EHECD_WithdrawCash WHERE sWithdrawMemberID=@ClientID Order By dApplyTime Desc",
                                                    new { ClientID = cash.sWithdrawMemberID });
            if (record != null)
            {
                DateTime pretime =DateTime.Parse(record.dApplyTime.Value.ToString("yyyy-MM-dd"));//上次的提现时间

                var now = DateTime.Now.ToString("yyyy-MM-dd");
                //求相差的时间间隔
                TimeSpan d3 = DateTime.Parse(now).Subtract(pretime);
                if (d3.Days > iIntervalDays)
                {//可提现
                    if (cash.iWithdrawMoney >= setting.iMinimumMoney)
                    {//大于最低提现金额

                        if (cash.iWithdrawMoney > drawcash)
                        {//余额不足
                            sResult = "可提现余额不足";
                            return -3;
                        }
                        else
                        {
                            //添加提现记录
                            cash.ID = GuidHelper.GetSecuentialGuid();
                            cash.dApplyTime = DateTime.Now;
                            cash.iState = 0;
                            cash.bIsDeleted = false;
                            cash.sWithdrawNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            cash.sWithdrawMemberType = 2;

                            sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_WithdrawCashDTO>(cash));
                            //编辑会员余额
                            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO()
                            {
                                dIncome = drawcash - cash.iWithdrawMoney
                            },
                            string.Format("WHERE ID='{0}'", cash.sWithdrawMemberID)));

                            return excute.ExcuteTransaction(sSql.ToString());
                        }
                    }
                    else
                    {
                        sResult = string.Format("每次最低提现金额{0}元", setting.iMinimumMoney);
                        return -2;
                    }
                }
                else
                {//小于设置的时间间隔不能提现

                    sResult = string.Format("每{0}天可提现一次", setting.iIntervalDays+1);
                    return -1;
                }
            }
            else
            {//第一次提现
                if (cash.iWithdrawMoney >= setting.iMinimumMoney)
                {//大于最低提现金额
                    if (cash.iWithdrawMoney > drawcash)
                    {//余额不足
                        sResult = "可提现余额不足";
                        return -3;
                    }
                    else
                    {
                        //添加提现记录
                        cash.ID = GuidHelper.GetSecuentialGuid();
                        cash.dApplyTime = DateTime.Now;
                        cash.iState = 0;
                        cash.bIsDeleted = false;
                        cash.sWithdrawNumber = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        cash.sWithdrawMemberType = 2;

                        sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_WithdrawCashDTO>(cash));
                        //编辑会员余额
                        sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_ClientDTO>(new EHECD_ClientDTO()
                        {
                            dIncome = drawcash - cash.iWithdrawMoney
                        },
                        string.Format("WHERE ID='{0}'",cash.sWithdrawMemberID)));

                        return excute.ExcuteTransaction(sSql.ToString());
                    }
                }
                else
                {
                    sResult = string.Format("每次最低提现金额{0}元", setting.iMinimumMoney);
                    return -2;
                }
            }
        }
       

        /// <summary>
        /// 分享客分享商品
        /// </summary>
        /// <param name="sClientID"></param>
        /// <param name="sGoodsId"></param>
        /// <returns></returns>
        public override int ShareGoods(Guid sClientID,Guid sGoodsId)
        {
            var good = query.SingleQuery<EHECD_GoodsDTO>(@"SELECT * FROM EHECD_Goods WHERE ID=@ID", new { ID = sGoodsId });

            //查询该用户是否关注过该店铺
            var temp = query.SingleQuery<EHECD_SharedClientInfoDTO>(@"SELECT TOP 1 * FROM EHECD_SharedClientInfo 
                                                                                WHERE sClientID=@sClientID AND sShopID=@sShopID",
                                                new { sClientID = sClientID, sShopID = good.sStoreId });
            if (temp != null && temp.bIsForzen == false)
            {//过滤掉不是该店的分享客，并且没有被冻结

                return 1;
            }
            else
            {
                return -1;//不添加店铺分享商品
            }
        }
        

        #endregion
    }
}
