using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
   public abstract class IOrderConfirmManager : BaseBll
    {
        /// <summary>
        /// 载入订单需要的数据
        /// </summary>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public abstract room_con_json LoadOrderInfo(dynamic param);

        /// <summary>
        /// 载入可用优惠券
        /// </summary>
        /// <param name="eHECD_ClientDTO"></param>
        /// <param name="dynamicData"></param>
        /// <returns></returns>
        public abstract IList<object> LoadCoupons(EHECD_ClientDTO eHECD_ClientDTO, dynamic dynamicData);



        /// <summary>
        /// 检查商品库存
        /// </summary>
        /// <param name="param"></param>
        public abstract bool checkCount(dynamic param);


        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name=""></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract int MakeOrder(EHECD_OrdersDTO order, dynamic param);
    }
}
