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
    public abstract class IGoodsManager : BaseBll
    {

        #region 商品的查询接口

        /// <summary>
        /// 分页查询商品列表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetList(PageInfo page, Dictionary<string, object> where);


        /// <summary>
        /// 根据商品ID获取详情
        /// </summary>
        /// <param name="GoodsId"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract EHECD_GoodsDTO GetGoods(Guid GoodsId, out List<EHECD_FullHouseTimeDTO> list);


        /// <summary>
        /// 根据商品ID获取该商品的选择的时间段的价格
        /// </summary>
        /// <param name="sGoodsId"></param>
        public abstract string GetSelectTimePricesByGoodsId(Guid sGoodsId);



        /// <summary>
        /// 获取平台设置的商品佣金最高限制比例
        /// </summary>
        /// <returns></returns>
        public abstract int GetRate();

        #endregion










        #region 商品的操作接口

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="dto">商品Model</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract int Insert(EHECD_GoodsDTO dto, List<EHECD_FullHouseTimeDTO> list);

        /// <summary>
        /// 编辑商品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int Update(EHECD_GoodsDTO dto, List<EHECD_FullHouseTimeDTO> list);


        /// <summary>
        /// 删除商品
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int Delete(Dictionary<string, object> dic);



        /// <summary>
        /// 上下架商品
        /// </summary>
        /// <param name="ID">商品主键ID</param>
        /// <param name="type">1-上架,2-下架</param>
        /// <param name="user">登录用户</param>
        /// <returns></returns>
        public abstract int bShelves(Guid ID, int type, EHECD_SystemUserDTO user);



        /// <summary>
        /// 设置/取消秒杀活动
        /// </summary>
        /// <param name="GoodsId">商品主键ID</param>
        /// <param name="type">1-设置秒杀,2-取消秒杀</param>
        /// <param name="sSeckillTime">秒杀时间段</param>
        /// <param name="dSeckillPrices">秒杀价格</param>
        /// <returns></returns>
        public abstract int SetOrCancelSeckill(Guid GoodsId, int type, string sSeckillTime = null, decimal dSeckillPrices = 0, string sActivityUseTime = null);


        /// <summary>
        /// 设置/取消特价活动
        /// </summary>
        /// <param name="GoodsId">商品主键ID</param>
        /// <param name="type">1-设置特价,2-取消特价</param>
        /// <param name="sSpecialSaleTime">特价时间段</param>
        /// <param name="dSpecialSalePrices">特价价格</param>
        /// <returns></returns>
        public abstract int SetOrCancelSpecialSale(Guid GoodsId, int type, string sSpecialSaleTime = null, decimal dSpecialSalePrices = 0, string sActivityUseTime = null);


        /// <summary>
        /// 统一设置佣金类型和佣金
        /// </summary>
        /// <param name="sStoreId">商品所属店铺</param>
        /// <param name="iCommissionType">佣金类型（1-固定金额,2-商品价格比例）</param>
        /// <param name="dMoney">固定金额/价格比例</param>
        /// <returns></returns>
        public abstract int SetAllMoney(Guid? sStoreId, int iCommissionType, decimal dMoney);



        /// <summary>
        /// 设置商品的时间段价格
        /// </summary>
        /// <param name="sGoodsId"></param>
        /// <param name="sTimePrices"></param>
        /// <returns></returns>
        public abstract int SetPrices(Guid sGoodsId, string sTimePrices);


        /// <summary>
        /// 预览商品返回商品主键ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract string PreviewGoods(EHECD_GoodsPreviewViewDTO dto);

        #endregion

    }
}
