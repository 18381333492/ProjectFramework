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
    public abstract class ICountAnalysisManager : BaseBll
    {

        #region 统计分析的查询接口



        /// <summary>
        /// 分页获取订单统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        ///  <param name="data"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetOrderList(PageInfo page, Dictionary<string, object> where, int tUserType, Guid? sStoreID, out string data);



        /// <summary>
        /// 分页获取销售金额统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        ///  <param name="data"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetSalesList(PageInfo page, Dictionary<string, object> where, int tUserType, Guid? sStoreID, out string data);


        /// <summary>
        /// 分页获取退货统计数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="tUserType">用户类型（0：平台用户，1：店铺，2：合伙人）</param>
        /// <param name="sStoreID">所属店铺</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetReturnList(PageInfo page, Dictionary<string, object> where, int tUserType, Guid? sStoreID, out string data);

        #endregion




    }
}
