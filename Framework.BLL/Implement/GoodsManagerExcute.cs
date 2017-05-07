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
    public partial class GoodsManager : IGoodsManager
    {

        #region 商品的操作实现

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="dto">商品Model</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override int Insert(EHECD_GoodsDTO dto, List<EHECD_FullHouseTimeDTO> list)
        {
            decimal rate = GetRate().ToDecimal();//获取商品的佣金的最高比例
            if (dto.iCommissionType == 2)
            {//比例
                if(dto.dMoney.Value> rate)
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            else
            {//固定金恩
                if(dto.dMoney>dto.dGoodsFisrtPrice.Value* rate* (0.01.ToDecimal()))
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            dto.ID = Helper.GuidHelper.GetSecuentialGuid();
            dto.bIsDeleted = false;
            dto.bShelves = true;
            dto.dShelvesTime = DateTime.Now;
            if(dto.sGoodsCategory != 1)
            {
                dto.sHouseSize = null;
            }
            StringBuilder sSql = new StringBuilder();
            sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_GoodsDTO>(dto));
            if(dto.sGoodsCategory != 3)
            {
                foreach (var m in list)
                {
                    m.sGoodsId = dto.ID;
                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_FullHouseTimeDTO>(m));
                }
            }
            return excute.ExcuteTransaction(sSql.ToString());
        }


        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override int Update(EHECD_GoodsDTO dto, List<EHECD_FullHouseTimeDTO> list)
        {
            decimal rate = GetRate().ToDecimal();//获取商品的佣金的最高比例
            if (dto.iCommissionType == 2)
            {//比例
                if (dto.dMoney.Value > rate)
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            else
            {//固定金恩
                if (dto.dMoney > dto.dGoodsFisrtPrice.Value * rate*(0.01.ToDecimal()))
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            StringBuilder sSql = new StringBuilder();
            if (dto.sGoodsCategory != 1)
            {
                dto.sHouseSize = null;
            }
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsDTO>(dto,string.Format("where ID='{0}'", dto.ID)));
            if (dto.sGoodsCategory != 3)
            {//处理价格时间段信息
                /*
                 **先删除
                 */
                sSql.AppendFormat("DELETE [EHECD_FullHouseTime] where sGoodsId='{0}'", dto.ID);
                //在添加
                foreach (var m in list)
                {
                    m.sGoodsId = dto.ID;
                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_FullHouseTimeDTO>(m));
                }
            }
            return excute.ExcuteTransaction(sSql.ToString());
        }


        /// <summary>
        /// 获取平台设置的商品佣金最高限制比例
        /// </summary>
        /// <returns></returns>
        public override int GetRate()
        {
            var rate = query.SingleQuery<EHECD_BaseSettingDTO>(@"SELECT TOP 1 * FROM EHECD_BaseSetting",null);
            return rate.iHighestCommissionPrecent.Value;
        }


        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="dic">商品ID集合</param>
        /// <returns></returns>
        public override int Delete(Dictionary<string, object> dic)
        {

            StringBuilder sSql = new StringBuilder();
            string[] IDs = dic["Ids"].ToString().Split(',');
            foreach(var m in IDs)
            {
                //删除商品相关满房时间段信息
                sSql.AppendFormat("DELETE [EHECD_FullHouseTime] where sGoodsId={0}", m);
                //删除商品的时间段价格信息
                sSql.AppendFormat("DELETE [EHECD_GoodsTimePrice] where sGoodsId={0}", m);
            }
            //删除
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsDTO>(new EHECD_GoodsDTO() { bIsDeleted = true },
                string.Format("where ID in ({0}) ", dic["Ids"].ToString())));
            return excute.ExcuteTransaction(sSql.ToString());
        }

        /// <summary>
        /// 上下架商品
        /// </summary>
        /// <param name="ID">商品主键ID</param>
        /// <param name="type">1-上架,2-下架</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        public override int bShelves(Guid ID,int type, EHECD_SystemUserDTO user)
        {
            StringBuilder sSql = new StringBuilder();
            var good = query.SingleQuery<Dictionary<string,object>>(@"SELECT sGoodsName FROM EHECD_Goods WHERE ID=@ID AND bIsDeleted=0", new { ID=ID });
            if (type == 1)
            {//上架

                var log = query.SingleQuery<EHECD_OperatDTO>(@"SELECT TOP 1 * FROM EHECD_Operat WHERE sGoodsId=@sGoodsId ORDER BY dOperatTime DESC", 
                    new { sGoodsId = ID });
                var admin = query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT * FROM EHECD_SystemUser WHERE ID=@ID AND bIsDeleted=0", new { ID = log.sOperatorID });

                if (admin.tUserType==1||user.tUserType==0)
                {//店铺自己下架过
                    //添加商品的上架的日志
                    sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OperatDTO>(new EHECD_OperatDTO()
                    {
                        ID = GuidHelper.GetSecuentialGuid(),
                        iState = 0,//商品上架
                        sGoodsId = ID,//商品ID
                        dOperatTime = DateTime.Now,
                        bIsDeleted = false,
                        sContent = string.Format("{0}上架了商品{1}", user.sUserName, good["sGoodsName"].ToString()),
                        sOperator = user.sUserName,
                        sOperatorID = user.ID
                    }));
                    sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsDTO>(new EHECD_GoodsDTO() { bShelves = true },
                          string.Format("where ID='{0}'", ID.ToString())));

                    return excute.ExcuteTransaction(sSql.ToString());
                }
                else
                {//商品被平台用户下架过不能上架
                    return -10;
                }
            }
            else
            {//下架

                //添加商品的下架的日志
                sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_OperatDTO>(new EHECD_OperatDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    iState = 1,//商品下架
                    sGoodsId = ID,//商品ID
                    dOperatTime = DateTime.Now,
                    bIsDeleted = false,
                    sContent = string.Format("{0}下架了商品{1}", user.sUserName, good["sGoodsName"].ToString()),
                    sOperator = user.sUserName,
                    sOperatorID = user.ID

                }));
                sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsDTO>(new EHECD_GoodsDTO() { bShelves = false },
                        string.Format("where ID='{0}'", ID.ToString())));

                return excute.ExcuteTransaction(sSql.ToString());
            }
            
        }


        /// <summary>
        /// 设置/取消秒杀活动
        /// </summary>
        /// <param name="GoodsId">商品主键ID</param>
        /// <param name="type">1-设置秒杀,2-取消秒杀</param>
        /// <param name="sSeckillTime">秒杀时间段</param>
        /// <param name="dSeckillPrices">秒杀价格</param>
        /// <returns></returns>
        public override int SetOrCancelSeckill(Guid GoodsId, int type, string sSeckillTime=null,decimal dSeckillPrices=0,string sActivityUseTime=null)
        {
            if (type == 1)
            {//设置秒杀活动
                return excute.UpdateSingle<EHECD_GoodsDTO>(new EHECD_GoodsDTO()
                {
                    sSeckillTime = sSeckillTime,
                    bSeckill = true,
                    dSeckillPrices = dSeckillPrices,
                    sActivityUseTime= sActivityUseTime,
                }, string.Format("where ID in ('{0}') ", GoodsId.ToString()));
            }
            else
            {//取消秒杀活动
                return excute.UpdateSingle<EHECD_GoodsDTO>(new EHECD_GoodsDTO() {
                    bSeckill = false,
                    dSeckillPrices= dSeckillPrices,
                    sSeckillTime= sSeckillTime,
                    sActivityUseTime = sActivityUseTime,
                }, string.Format("where ID in ('{0}') ", GoodsId.ToString()));
            }
        }

        /// <summary>
        /// 设置/取消特价活动
        /// </summary>
        /// <param name="GoodsId">商品主键ID</param>
        /// <param name="type">1-设置特价,2-取消特价</param>
        /// <param name="sSpecialSaleTime">特价时间段</param>
        /// <param name="dSpecialSalePrices">特价价格</param>
        /// <returns></returns>
        public override int SetOrCancelSpecialSale(Guid GoodsId, int type, string sSpecialSaleTime = null, decimal dSpecialSalePrices = 0, string sActivityUseTime = null)
        {
            if (type == 1)
            {//设置特价活动
                return excute.UpdateSingle<EHECD_GoodsDTO>(new EHECD_GoodsDTO()
                {
                    sSpecialSaleTime = sSpecialSaleTime,
                    bSpecialSale = true,
                    dSpecialSalePrices = dSpecialSalePrices,
                    sActivityUseTime= sActivityUseTime

                }, string.Format("where ID in ('{0}') ", GoodsId.ToString()));
            }
            else
            {//取消特价活动
                return excute.UpdateSingle<EHECD_GoodsDTO>(new EHECD_GoodsDTO()
                {
                    bSpecialSale = false,
                    dSpecialSalePrices = dSpecialSalePrices,
                    sSpecialSaleTime = sSpecialSaleTime,
                    sActivityUseTime = sActivityUseTime
                }, string.Format("where ID in ('{0}') ", GoodsId.ToString()));
            }
        }

        /// <summary>
        /// 统一设置佣金类型和佣金
        /// </summary>
        /// <param name="sStoreId">商品所属店铺</param>
        /// <param name="iCommissionType">佣金类型（1-固定金额,2-商品价格比例）</param>
        /// <param name="dMoney">固定金额/价格比例</param>
        /// <returns></returns>
        public override int SetAllMoney(Guid? sStoreId,int iCommissionType,decimal dMoney)
        {
            decimal rate = GetRate();////获取商品的佣金的最高比例
               //获取该店铺的所有商品
            var goolist = query.QueryList<EHECD_GoodsDTO>(@"SELECT * FROM EHECD_Goods WHERE bIsDeleted=0 AND sStoreId=@sStoreId",new { sStoreId = sStoreId });

            foreach(var good in goolist)
            {
                if (iCommissionType == 2)
                {//比例
                    if (dMoney > rate)
                    {
                        return -10;//佣金设置超过平台佣金设置的最高限制
                    }
                }
                else
                {//固定金恩
                    if (dMoney > good.dGoodsFisrtPrice.Value * rate* (0.01.ToDecimal()))
                    {
                        return -10;//佣金设置超过平台佣金设置的最高限制
                    }
                }
            }
            return excute.UpdateSingle<EHECD_GoodsDTO>(new EHECD_GoodsDTO()
            {
               iCommissionType= iCommissionType,
               dMoney= dMoney
            }, string.Format("where sStoreId ='{0}' AND bIsDeleted=0 ", sStoreId.ToString()));
        }

        /// <summary>
        /// 设置商品的时间段价格
        /// </summary>
        /// <param name="sGoodsId"></param>
        /// <param name="sTimePrices"></param>
        /// <returns></returns>
        public override int SetPrices(Guid sGoodsId,string sTimePrices)
        {

           var item=query.SingleQuery<EHECD_GoodsTimePriceDTO>(@"select * from EHECD_GoodsTimePrice where sGoodsId=@sGoodsId", new { sGoodsId = sGoodsId });

            if (item == null)
            {//新增
                EHECD_GoodsTimePriceDTO dto = new EHECD_GoodsTimePriceDTO();
                dto.ID = Helper.GuidHelper.GetSecuentialGuid();
                dto.sGoodsId = sGoodsId;
                dto.sFirstTime = sTimePrices;
                //这两字段没有用,设默认值空
                dto.sSecTime = string.Empty;
                dto.sThirdTime = string.Empty;
                return excute.InsertSingle<EHECD_GoodsTimePriceDTO>(dto);
            }
            else
            {//编辑
                return  excute.Update(@"Update EHECD_GoodsTimePrice 
                                            set sFirstTime=@sFirstTime where sGoodsId=@sGoodsId",
                                            new { sFirstTime = sTimePrices, sGoodsId = sGoodsId, });
            }
        }



        /// <summary>
        /// 预览商品返回商品主键ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override string PreviewGoods(EHECD_GoodsPreviewViewDTO dto)
        {
            dto.ID = Helper.GuidHelper.GetSecuentialGuid();
            dto.bIsDeleted = false;

            var ret = excute.InsertSingle<EHECD_GoodsPreviewViewDTO>(dto);
            if (ret > 0)
            {
                return dto.ID.ToString();
            }
            return string.Empty;
        }

        #endregion
    }
}
