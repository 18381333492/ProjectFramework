using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IShopDetailManager:BaseBll
    {
        /// <summary>
        /// 跟新当前用户上级
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="superID"></param>
        /// <returns></returns>
        public abstract bool UpdateClientSuper(string userID, string superID);

        /// <summary>
        /// 判断当前用户是否有上级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract bool IsHaveSuper(string ID);

        /// <summary>
        /// 插入分销的商品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract int InsertShareGoods(Dictionary<string,object> param);

        /// <summary>
        /// 检查当前分享的人员【是否已经注册】和【是否是分销客】
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> CheckUser(Dictionary<string,object> param);

        /// <summary>
        /// 手机端根据ID查看游记详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetTravelByID(string ID);
        /// <summary>
        /// 手机端店铺所有游记
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetAllTraveNote(PageInfo page, string ID);
        /// <summary>
        /// 店铺首页信息绑定
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetShopHome(Dictionary<string,object> dic,PageInfo page);
        /// <summary>
        /// 获取本店的客房
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_GoodsDTO> GetRoom(PageInfo page, string ID, Dictionary<string, object> dic);
        /// <summary>
        /// 查看房间的详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract object HomeDetail(string ID,string ClientID);
        /// <summary>
        /// 获取所有评论
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetAllComment(PageInfo page, string ID);
       
        /// <summary>
        /// 店铺首页
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract object ShopHome(string ID,string clientID);
        /// <summary>
        /// 票务首页
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> TicketHome(PageInfo page, Dictionary<string, object> dic);
        /// <summary>
        /// 周边
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> AroundHome(PageInfo page, Dictionary<string, object> dic);
        /// <summary>
        /// 店主故事
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ShopSetDTO HosterStory(string ID);
        /// <summary>
        /// 获取商品的评分
        /// </summary>
        /// <param name="ID">商品ID</param>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetCommentScore(string ID);
        //收藏
        public abstract int CollectionIn(EHECD_CollectDTO dto);
        /// <summary>
        /// 判断用户是否已经收藏
        /// </summary>
        /// <param name="goodID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public abstract EHECD_CollectDTO IsCollect(string goodID, string clientID);
        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="goodID"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public abstract int CancelCollect(string goodID, string clientID);
        /// <summary>
        /// 写游记
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int WriteTravel(EHECD_TravelsNotesDTO dto,Dictionary<string,object> dic);
        /// <summary>
        /// 领取店铺优惠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract string GetShopCoupon(EHECD_CouponDetailsDTO dto);

        public abstract EHECD_CouponDetailsDTO IsGet(EHECD_CouponDetailsDTO dto);
        /// <summary>
        /// 根据openid查询客户表中是否存在
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO SearchOpernID(string ID);
        /// <summary>
        /// 向client表中插入新的用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int CreateNewClient(EHECD_ClientDTO dto);
        /// <summary>
        /// 根据用户进入页面后获取的ID寻找他的上级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO GetClient(string ID);
        /// <summary>
        /// 核销订单
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> CancelOrder(string sOrderNo,string shopID);
        /// <summary>
        /// 是否是本店的分享客
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> IsShareBelong(Dictionary<string, object> dic);
        /// <summary>
        /// 是否冻结
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO IsFrozen(string ID);
        /// <summary>
        /// 是否是分享客
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int IsShare(Dictionary<string, object> dic);
        /// <summary>
        /// 根据ID查出分享客的上级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO SearchUpShare(string ID);
        /// <summary>
        /// 所有的下级
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract IList<EHECD_ClientDTO> AllLower(string ID);
    }
}
