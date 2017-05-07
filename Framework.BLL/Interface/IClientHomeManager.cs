using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    /// <summary>
    /// 客户端的首页查询接口
    /// </summary>
    public abstract class IClientHomeManager : BaseBll
    {
        /// <summary>
        /// 获取首页数据
        /// </summary>
        /// <returns></returns> 
        public abstract object QueryClietHomeData(Dictionary<string,object> param);

        /// <summary>
        /// 选择城市
        /// </summary>
        /// <returns></returns>
        public abstract IList<Dictionary<string, object>> QuerySelectCity(Dictionary<string,object> param);

        /// <summary>
        /// 查询【名宿】
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> QueryByStore(PageInfo pageInfo,Dictionary<string,object> param);

        /// <summary>
        /// 查询【票务】
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> QueryByTiket(PageInfo pageInfo, Dictionary<string, object> param);

        /// <summary>
        /// 查询【周边】
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> QueryByAround(PageInfo pageInfo, Dictionary<string,object> param);

        /// <summary>
        /// 获取秒杀专区列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> QuerySecKillList(PageInfo pageInfo);


        /// <summary>
        /// 获取热卖专区列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> QuerySpecialSaleList(PageInfo pageInfo);

        /// <summary>
        /// 获取优惠劵列表（返回null或长度为0表示暂无优惠劵可领）
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<EHECD_CouponDTO> QueryCouponList(PageInfo pageInfo,Dictionary<string,object> param);

        /// <summary>
        /// 领取优惠劵
        /// </summary>
        /// <returns></returns>
        public abstract int ExcuteGetCoupon(Dictionary<string,object> param);

        /// <summary>
        /// 根据当前定位获取该城市下面的所有区
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract IList<Dictionary<string, object>> QueryCountryByCity(Dictionary<string,object> param);

        /// <summary>
        /// 获取所有房型
        /// </summary>
        /// <returns></returns>
        public abstract IList<Dictionary<string, object>> QueryRoomTypeList();

        /// <summary>
        /// 获取展示Banner详情
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract EHECD_ImagesDTO QueryShowBannerDetail(Dictionary<string,object> dic);

        /// <summary>
        /// 根据搜索框的输入实时查询商品列表
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract IList<Dictionary<string, object>> QueryGoodsByWhere(Dictionary<string,object> dic);

        /// <summary>
        /// 查看用户是否是分享客
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO IsShareClient(string ClientID);
    }
}
