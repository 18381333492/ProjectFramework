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

    /// <summary>
    /// 房态管理的业务处理
    /// </summary>
    public class RoomManageManager:IRoomManageManager
    {

        /// <summary>
        /// 分页获取房态管理数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="where"></param>
        /// <param name="sUserID">商户ID</param>
        /// <returns></returns>
        public override PagingRet<EHECD_GoodsDTO> GetList(PageInfo page, Dictionary<string, object> where, Guid sUserID)
        { 
            string sKeyWord = where["sKeyWord"] != null ? where["sKeyWord"].ToString() : string.Empty;
            StringBuilder sql = new StringBuilder();
            sql.Append(@"SELECT ID,sGoodsName,
                                    dGoodsFisrtPrice,     
                                    iHouseCount
                                    FROM EHECD_Goods WHERE bIsDeleted = 0 AND sGoodsCategory=1 AND sStoreId=@sUserID");
            if (!string.IsNullOrEmpty(sKeyWord))
            {//商品名称查询
                sql.AppendFormat(" AND sGoodsName Like '%{0}%' ", sKeyWord);
            }
            return query.PaginationQuery<EHECD_GoodsDTO>(sql.ToString(), page, new { sUserID = sUserID });
        }


        /// <summary>
        /// 根据时间段获取房间数详情
        /// </summary>
        /// <param name="sGoodsId"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public override List<int> GetRoomDetail(Guid sGoodsId, int year,int month)
        {
            string dStartTime = string.Format("{0}-{1}-1 00:00:00", year, month);
            string sEndTime = string.Format("{0}-{1}-{2} 23:59:59", year, month, DateTime.DaysInMonth(year, month));

            //住房明细
            var roomDetail = query.QueryList<EHECD_RoomDetailDTO>(@"SELECT * FROM EHECD_RoomDetail 
                                                                             WHERE sGoodsId = @ID 
                                                                             AND 
                                                                      ((dStartTime >= @dStartTime AND sEndTime <= @dEndTime)   --第一种情况包含关系
                                                                      OR (dStartTime<@dStartTime AND sEndTime>=@dStartTime)  --第二种情况跨月的开始时间情况
                                                                      OR (dStartTime<=@dEndTime AND sEndTime>@dEndTime)      --第三种情况跨月的结束时间
                                                                      OR (dStartTime<=@dStartTime AND sEndTime>=@dEndTime)   --第四种情况被包含关系
                                                                     );",
                       new
                       {
                           ID = sGoodsId,
                           dStartTime = dStartTime,
                           dEndTime = sEndTime
                       }).Select(m =>
                       {
                           m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                           m.sEndTime = DateTime.Parse(m.sEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                           return m;
                       }).ToList();

            //查询该商品的满房时间段
            var fullTime = query.QueryList<EHECD_FullHouseTimeDTO>(@"SELECT * FROM 
                                                                  EHECD_FullHouseTime WHERE sGoodsId = @ID",
                new
                {
                    ID = sGoodsId,
                }).Select(m =>
                {
                    m.dStartTime = DateTime.Parse(m.dStartTime.ToDateTime().ToString("yyyy-MM-dd ") + "00:00:00");
                    m.dEndTime = DateTime.Parse(m.dEndTime.ToDateTime().ToString("yyyy-MM-dd ") + "23:59:59");
                    return m;
                }).ToList();

            // 获取商品的信息
            var goodInfo = query.SingleQuery<EHECD_GoodsDTO>("SELECT * FROM EHECD_Goods WHERE ID = @ID;", new { ID = sGoodsId });

            //结果统计
            List<int> RoomCount = new List<int>();
            for (var i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                DateTime sta = DateTime.Parse((new DateTime(year, month, i).ToString("yyyy-MM-dd") + " 00:00:00")),//当天的开始时间
                 end = DateTime.Parse((new DateTime(year, month, i).ToString("yyyy-MM-dd") + " 23:59:59"));//当天的结束时间

                //查询当天所属于的满房时间段
                var time = fullTime.Where(m => m.dStartTime <= sta && m.dEndTime >= end);
                if (time != null && time.Count() > 0)
                {//在满房时间段内

                    // 获取当天的订房总数
                    var todayBookedRoomTotalAmount = roomDetail.Where(de => de.dStartTime <= sta && de.sEndTime >= end).Sum(de => de.iAmount);
                    //剩余房间数量
                    RoomCount.Add(goodInfo.iHouseCount.Value - todayBookedRoomTotalAmount.Value - time.First().iFullHouseCount.Value);
                }
                else
                {
                    // 获取当天的订房总数
                    var todayBookedRoomTotalAmount = roomDetail.Where(de => de.dStartTime <= sta && de.sEndTime >= end).Sum(de => de.iAmount);
                    RoomCount.Add(goodInfo.iHouseCount.Value - todayBookedRoomTotalAmount.Value);
                }
            }
            return RoomCount;
        }
    }
}
