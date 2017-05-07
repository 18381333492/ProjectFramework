using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IClientManager: BaseBll
    {
        /// <summary>
        /// 页面数据初始化绑定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_ClientDTO> GetPageList(PageInfo info, Dictionary<string, object> dic);
        /// <summary>
        /// 查出所有会员的信息
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_ClientDTO> SearchPeople();
        /// <summary>
        /// 查出所有分销客的电话
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_ClientDTO> SerachVipPhone();

        /// <summary>
        /// 载入分享客
        /// </summary>
        /// <param name="info"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> LoadSharedClient(PageInfo info, Dictionary<string, object> param);

        /// <summary>
        /// 载入客户【总后台客户管理】
        /// </summary>
        /// <param name="info"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> LoadClient(PageInfo info, Dictionary<string, object> param);

        /// <summary>
        /// 冻结/解冻用户
        /// </summary>
        /// <param name="param"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract bool ForzenClients(List<EHECD_ClientDTO> param, EHECD_SystemUserDTO user);

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO QueryClientInfo(EHECD_ClientDTO param);

        /// <summary>
        /// 载入普通用户的订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_OrdersDTO> LoadClientOrders(PageInfo info, Guid ID);

        /// <summary>
        /// 载入店铺用户的订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_OrdersDTO> LoadClientOrdersByShopID(PageInfo info, Guid ID,Guid sShopID);


        /// <summary>
        /// 冻结/解冻分享客
        /// </summary>
        /// <param name="param"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract bool ForzenShardClients(List<EHECD_SharedClientInfoDTO> param, EHECD_SystemUserDTO user);

        /// <summary>
        /// 载入分享客分享的商品
        /// </summary>
        /// <param name="info">参数</param>
        /// <param name="guid">分享客的ID</param>
        /// <returns>查询结果</returns>
        public abstract PagingRet<Dictionary<string,object>> LoadSharedGoods(PageInfo info, Guid guid);

        ///汤台重写
        /// <summary>
        /// 载入分享客分享的商品
        /// </summary>
        /// <param name="info">参数</param>
        /// <param name="guid">分享客的ID</param>
        /// <returns>查询结果</returns>
        public abstract PagingRet<Dictionary<string, object>> LoadSharedGoodsList(PageInfo info, Guid guid,Guid sShopId);


        /// <summary>
        /// 查询该电话号码是否注册过
        /// </summary>
        /// <param name="sPhone"></param>
        public abstract bool IsExit(string sPhone);
    }
}
