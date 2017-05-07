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
    public partial class GoodsPreviewViewManager : IGoodsPreviewViewManager
    {

        #region 商品草稿的操作实现

        /// <summary>
        /// 添加商品草稿
        /// </summary>
        /// <param name="dto">商品Model</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override int Insert(EHECD_GoodsPreviewViewDTO dto, List<EHECD_FullHouseTimeDTO> list)
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
                if (dto.dMoney > dto.dGoodsFisrtPrice.Value * rate * (0.01.ToDecimal()))
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            dto.bIsDeleted = false;
            dto.bShelves = true;
            dto.dShelvesTime = DateTime.Now;
            if (dto.sGoodsCategory != 1)
            {
                dto.sHouseSize = null;
            }
            StringBuilder sSql = new StringBuilder();
            sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_GoodsPreviewViewDTO>(dto));
            if (dto.sGoodsCategory != 3)
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
        /// 编辑商品草稿
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override int Update(EHECD_GoodsPreviewViewDTO dto, List<EHECD_FullHouseTimeDTO> list)
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
                if (dto.dMoney > dto.dGoodsFisrtPrice.Value * rate * (0.01.ToDecimal()))
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }

            StringBuilder sSql = new StringBuilder();
            if (dto.sGoodsCategory != 1)
            {
                dto.sHouseSize = null;
            }
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsPreviewViewDTO>(dto, string.Format("where ID='{0}'", dto.ID)));
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
        /// 删除商品草稿
        /// </summary>
        /// <param name="dic">商品ID集合</param>
        /// <returns></returns>
        public override int Delete(Dictionary<string, object> dic)
        {

            StringBuilder sSql = new StringBuilder();
            string[] IDs = dic["Ids"].ToString().Split(',');
            foreach (var m in IDs)
            {
                //删除商品相关满房时间段信息
                sSql.AppendFormat("DELETE [EHECD_FullHouseTime] where sGoodsId={0}", m);
                //删除商品的时间段价格信息
                sSql.AppendFormat("DELETE [EHECD_GoodsTimePrice] where sGoodsId={0}", m);
            }
            //删除
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsPreviewViewDTO>(new EHECD_GoodsPreviewViewDTO() { bIsDeleted = true },
                string.Format("where ID in ({0}) ", dic["Ids"].ToString())));
            return excute.ExcuteTransaction(sSql.ToString());
        }

        /// <summary>
        /// 删除商品草稿箱,添加到商品列表中
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override int DeleteAndIndert(Guid ID)
        {


            var history = query.SingleQuery<EHECD_GoodsPreviewViewDTO>(@"SELECT * FROM EHECD_GoodsPreviewView WHERE ID=@ID", new { ID = ID });

            decimal rate = GetRate().ToDecimal();//获取商品的佣金的最高比例
            if (history.iCommissionType == 2)
            {//比例
                if (history.dMoney.Value > rate)
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }
            else
            {//固定金恩
                if (history.dMoney > history.dGoodsFisrtPrice.Value * rate * (0.01.ToDecimal()))
                {
                    return -10;//佣金设置超过平台佣金设置的最高限制
                }
            }

            var good = new EHECD_GoodsDTO();
            good.ID = GuidHelper.GetSecuentialGuid();
            good.iCommissionType = history.iCommissionType;
            good.iHouseBedCount = history.iHouseBedCount;
            good.bIsDeleted = false;
            good.bShelves = true;//上架
            good.dGoodsFisrtPrice = history.dGoodsFisrtPrice;
            good.dGoodsSecPrice = history.dGoodsSecPrice;
            good.dGoodsThirdPrice = history.dGoodsThirdPrice;
            good.dMoney = history.dMoney;
            good.dShelvesTime = DateTime.Now;//上架时间
            good.iHouseCount = history.iHouseCount;
            good.iHousePerson = history.iHousePerson;
            good.sGoodsCategory = history.sGoodsCategory;
            good.sGoodsIntroduce = history.sGoodsIntroduce;
            good.sGoodsName = history.sGoodsName;
            good.sGoodsNo = history.sGoodsNo;
            good.sGoodsPictures = history.sGoodsPictures;
            good.sHouseOrTicketDetail = history.sHouseOrTicketDetail;
            good.sHouseSize = history.sHouseSize;
            good.sStoreId = history.sStoreId.ToString();//店铺ID
            good.sSeckillTime = history.sSeckillTime;
            good.sSpecialSaleTime = history.sSpecialSaleTime;
            good.bSeckill = history.bSeckill;
            good.bSpecialSale = history.bSpecialSale;

            StringBuilder sSql = new StringBuilder();

            sSql.Append(DBSqlHelper.GetInsertSQL<EHECD_GoodsDTO>(good));//获取商品添加的SQL语句

            //满房时间短的SQL语句
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_FullHouseTimeDTO>(new EHECD_FullHouseTimeDTO()
            {
                sGoodsId = good.ID
            }, string.Format(" WHERE sGoodsId='{0}'", history.ID)));

            //满房商品时间价格段的SQL语句
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsTimePriceDTO>(new EHECD_GoodsTimePriceDTO()
            {
                sGoodsId = good.ID
            }, string.Format(" WHERE sGoodsId='{0}'", history.ID)));

            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_GoodsPreviewViewDTO>(new EHECD_GoodsPreviewViewDTO() { bIsDeleted = true },
                string.Format("where ID in ('{0}') ", history.ID)));
            return excute.ExcuteTransaction(sSql.ToString());

        }


        /// <summary>
        /// 获取平台设置的商品佣金最高限制比例
        /// </summary>
        /// <returns></returns>
        private int GetRate()
        {
            var rate = query.SingleQuery<EHECD_BaseSettingDTO>(@"SELECT TOP 1 * FROM EHECD_BaseSetting", null);
            return rate.iHighestCommissionPrecent.Value;
        }
        #endregion
    }
}
