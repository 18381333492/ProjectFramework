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
    public partial class GoodsPreviewViewManager :IGoodsPreviewViewManager
    {


        /// <summary>
        /// 分页获取商品草稿数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_GoodsPreviewViewDTO> GetList(PageInfo page, Dictionary<string, object> where)
        {
            var sStoreId = where["sStoreId"].ToString();
            int sGoodsCategory = where["sGoodsCategory"] != null ? where["sGoodsCategory"].ToInt32() : 0;
            string sKeyWord = where["sKeyWord"] != null ? where["sKeyWord"].ToString() : string.Empty;
            string dStartTime = where["dStartTime"] != null ? where["dStartTime"].ToString() : string.Empty;
            string dEndTime = where["dEndTime"] != null ? where["dEndTime"].ToString() : string.Empty;
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT ID,sGoodsName,
                                    sGoodsCategory,
                                    dGoodsFisrtPrice,     
                                    dShelvesTime
                                    FROM EHECD_GoodsPreviewView WHERE bIsDeleted = 0 AND sStoreId=@sStoreId");
            if (sGoodsCategory > 0)
            {//商品分类查询
                sql.Append(" AND sGoodsCategory=@sGoodsCategory");
            }
            if (!string.IsNullOrEmpty(sKeyWord))
            {//商品名称查询
                sql.AppendFormat(" AND sGoodsName Like '%{0}%' ", sKeyWord);
            }
            if (!string.IsNullOrEmpty(dStartTime))
            {//上架时间查询
                sql.AppendFormat(" AND dShelvesTime>='{0}' ", dStartTime);
            }
            if (!string.IsNullOrEmpty(dEndTime))
            {
                sql.AppendFormat(" AND dShelvesTime<='{0}' ", dEndTime);
            }
            return query.PaginationQuery<EHECD_GoodsPreviewViewDTO>(sql.ToString(), page, new { sStoreId= sStoreId, sGoodsCategory = sGoodsCategory});
        }


        /// <summary>
        /// 根据商品ID获取详情
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public override EHECD_GoodsPreviewViewDTO GetGoodsPreviewView(Guid GoodsId, out List<EHECD_FullHouseTimeDTO> list)
        {
            list = new List<EHECD_FullHouseTimeDTO>();
            var Goods = query.SingleQuery<EHECD_GoodsPreviewViewDTO>(@"select * from EHECD_GoodsPreviewView where ID=@ID", new { ID = GoodsId });
            if (Goods.sGoodsCategory != 3)
            {//商品类型为客房和票务时获取时间段的数据

                list = query.QueryList<EHECD_FullHouseTimeDTO>(@"select * from EHECD_FullHouseTime 
                                                                            where sGoodsId=@sGoodsId order by dStartTime ",
                                                                            new { sGoodsId = GoodsId }).ToList();
            }
            return Goods;
        }




    }
}
