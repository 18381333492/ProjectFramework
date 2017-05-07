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
    public abstract class IGoodsPreviewViewManager : BaseBll
    {

        #region 商品草稿查询接口


        /// <summary>
        /// 分页查询商品草稿列表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_GoodsPreviewViewDTO> GetList(PageInfo page, Dictionary<string, object> where);


        /// <summary>
        /// 根据商品ID获取详情
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract EHECD_GoodsPreviewViewDTO GetGoodsPreviewView(Guid GoodsId, out List<EHECD_FullHouseTimeDTO> list);

        #endregion



        #region 商品草稿操作接口
        /// <summary>
        /// 添加商品草稿
        /// </summary>
        /// <param name="dto">商品Model</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract int Insert(EHECD_GoodsPreviewViewDTO dto, List<EHECD_FullHouseTimeDTO> list);


        /// <summary>
        /// 编辑商品草稿
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract int Update(EHECD_GoodsPreviewViewDTO dto, List<EHECD_FullHouseTimeDTO> list);


        /// <summary>
        /// 删除商品草稿
        /// </summary>
        /// <param name="dic">商品ID集合</param>
        /// <returns></returns>
        public abstract int Delete(Dictionary<string, object> dic);


        /// <summary>
        /// 删除商品草稿箱,添加到商品列表中
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract int DeleteAndIndert(Guid ID);

        #endregion
    }
}
