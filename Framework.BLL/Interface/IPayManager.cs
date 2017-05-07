using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    public abstract class IPayManager : BaseBll
    {
        /// <summary>
        /// 根据订单ID检查订单状态
        /// </summary>
        /// <param name="sOrderId"></param>
        /// <returns></returns>
        public abstract int CheckOrder(Guid sOrderId);

        /// <summary>
        /// 支付成功修改订单状态
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public abstract int AlterOrderState(string sOrderNo);


        /// <summary>
        /// 根据订单编号获取订单所属店铺的Openid
        /// </summary>
        /// <param name="sOrderNo"></param>
        /// <returns></returns>
        public abstract string GetOpenid(string sOrderNo);
    }
}
