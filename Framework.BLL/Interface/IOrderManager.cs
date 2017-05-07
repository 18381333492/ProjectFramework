using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
   public abstract class IOrderManager : BaseBll
    {
        /// <summary>
        /// 分页查询订单数据
        /// </summary>
        /// <param name="info">分页条件</param>
        /// <param name="param">查询条件</param>
        /// <returns>分页结果</returns>
        public abstract PagingRet<IDictionary<string, object>> LoadOrderData(PageInfo info,Guid? id,IDictionary<string, object> param);

        /// <summary>
        /// 根据ID获取订单详情
        /// </summary>
        /// <param name="order">订单</param>
        /// <returns>完整订单信息</returns>
        public abstract object LoadOrderDetailById(EHECD_OrdersDTO order);
    }
}
