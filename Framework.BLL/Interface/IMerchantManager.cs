using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IMerchantManager:BaseBll
    {

        /// <summary>
        /// 本店所有订单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetAllOrder(PageInfo page, string ID);
        /// <summary>
        /// 没付款
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetNotPay(PageInfo page, string ID);
        /// <summary>
        /// 未使用
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetNotUsed(PageInfo page, string ID);
        /// <summary>
        /// 已核销
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetHaveUsed(PageInfo page, string ID);
        /// <summary>
        /// 本店所有分销客
        /// </summary>
        /// <param name="page"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_ClientDTO> GetStoreShare(PageInfo page, string ID);
        /// <summary>
        /// 分销客详情
        /// </summary>
        /// <param name="page"></param>
        /// <param name="shopID"></param>
        /// <param name="sClientID"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetShareDetail(PageInfo page, string shopID, string sClientID);
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetDetail(string ID);
        /// <summary>
        /// 获取分销客的基本信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO GetShareClient(string ID);
    }
}
