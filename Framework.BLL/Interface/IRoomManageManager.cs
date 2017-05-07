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
    public abstract class IRoomManageManager : BaseBll
    {

        #region 房态管理的查询接口


        /// <summary>
        /// 分页查询商品草稿列表数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="sUserID">商户ID</param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_GoodsDTO> GetList(PageInfo page, Dictionary<string, object> where,Guid sUserID);


        /// <summary>
        /// 根据时间段获取房间数详情
        /// </summary>
        /// <param name="sGoodsId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public abstract List<int> GetRoomDetail(Guid sGoodsId, int year, int month);

        #endregion




    }
}
