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
    /// 商品查询相关
    /// </summary>
    public partial class GoodsManager : IGoodsManager
    {
        #region 商品的查询实现

        /// <summary>
        /// 分页获取商品数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetList(PageInfo page, Dictionary<string,object> where)
        {
            var sStoreId = where["sStoreId"].ToString();
            int tUserType = where["tUserType"].ToInt32();  //用户类型 0：平台用户，1：店铺，2：合伙人
            int sGoodsCategory = where["sGoodsCategory"] != null ? where["sGoodsCategory"].ToInt32() : 0;
            int bShelves = where["bShelves"] != null ? where["bShelves"].ToInt32() : -1;
            string sKeyWord = where["sKeyWord"] != null ? where["sKeyWord"].ToString() : string.Empty;
            string dStartTime = where["dStartTime"] != null ? where["dStartTime"].ToString() : string.Empty;
            string dEndTime = where["dEndTime"] != null ? where["dEndTime"].ToString() : string.Empty;
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT A.ID,A.sGoodsName,
                                    A.sGoodsCategory,
                                    A.dGoodsFisrtPrice,
                                    A.dGoodsSecPrice,
                                    A.dGoodsThirdPrice,
                                    A.bShelves,
                                    A.bSeckill,
                                    A.bSpecialSale,
                                    A.dSeckillPrices,
                                    A.dSpecialSalePrices,
                                    A.sSeckillTime,
                                    A.sSpecialSaleTime,
                                    A.dShelvesTime,
                                    B.sShopName    
                                    FROM EHECD_Goods AS A 
                                    LEFT JOIN EHECD_ShopSet AS B
                                    ON A.sStoreId=B.sShopID
                                    LEFT JOIN EHECD_SystemUser AS C 
                                    ON A.sStoreId=C.ID
                                    WHERE A.bIsDeleted = 0 AND C.bIsDeleted=0 ");
            if (tUserType ==1 )
            {//类型为店铺
                sql.Append(string.Format(" AND A.sStoreId='{0}'", sStoreId));
            }
            if (sGoodsCategory > 0)
            {//商品分类查询
                sql.Append(" AND A.sGoodsCategory=@sGoodsCategory");
            }
            if (bShelves >=0)
            {//状态查询
                sql.Append(" AND A.bShelves=@bShelves");
            }
            if (!string.IsNullOrEmpty(sKeyWord) && tUserType == 1)
            {//商品名称查询
                sql.AppendFormat(" AND A.sGoodsName Like '%{0}%' ", sKeyWord);
            }
            if (!string.IsNullOrEmpty(sKeyWord)&& tUserType==0)
            {//店铺名称查询 (平台才能按店铺查询)
                sql.AppendFormat(" AND (A.sGoodsName Like '%{0}%' OR B.sShopName Like '%{0}%') ", sKeyWord);
            }
            if (!string.IsNullOrEmpty(dStartTime))
            {//上架时间查询
                sql.AppendFormat(" AND A.dShelvesTime>='{0}' ", dStartTime);
            }
            if (!string.IsNullOrEmpty(dEndTime))
            {
                sql.AppendFormat(" AND A.dShelvesTime<='{0}' ", dEndTime);
            }
            return query.PaginationQuery<Dictionary<string, object>>(sql.ToString(), page, new { sGoodsCategory= sGoodsCategory, bShelves= bShelves });
        }



        /// <summary>
        /// 根据商品ID获取详情
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override EHECD_GoodsDTO GetGoods(Guid GoodsId ,out List<EHECD_FullHouseTimeDTO> list)
        {
            list = new List<EHECD_FullHouseTimeDTO>();
            var Goods=query.SingleQuery<EHECD_GoodsDTO>(@"select * from EHECD_Goods where ID=@ID",new { ID= GoodsId });
            if (Goods.sGoodsCategory != 3)
            {//商品类型为客房和票务时获取时间段的数据

                list = query.QueryList<EHECD_FullHouseTimeDTO>(@"select * from EHECD_FullHouseTime 
                                                                            where sGoodsId=@sGoodsId order by dStartTime ",
                                                                            new { sGoodsId = GoodsId }).ToList();
            }
            return Goods;
        }

        /// <summary>
        /// 根据商品ID获取该商品的选择的时间段的价格
        /// </summary>
        /// <param name="sGoodsId"></param>
        public override string GetSelectTimePricesByGoodsId(Guid sGoodsId)
        {
            var item= query.SingleQuery<EHECD_GoodsTimePriceDTO>(@"select * from 
                                                         EHECD_GoodsTimePrice where sGoodsId=@sGoodsId",new { sGoodsId= sGoodsId });
            if (item != null)
            {
                return item.sFirstTime;
            }
            return string.Empty;
        }

        #endregion

    }
}
