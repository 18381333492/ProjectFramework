using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    /// <summary>
    /// 优惠劵 【实现】
    /// author 王其
    /// </summary>
    public class CouponManager : ICouponManager
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetList(Dapper.PageInfo pageInfo,Guid? guid, dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dInsertTime";
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format(@"SELECT
	                                        ID,
	                                        iCoiCouponPrice,
	                                        iUsePrice,
	                                        iCouponCount,
	                                        dValidDateStart,
	                                        dValidDateEnd,
                                            dInsertTime,
	                                        (
		                                        SELECT
			                                        COUNT (1)
		                                        FROM
			                                        EHECD_CouponDetails
		                                        WHERE
			                                        a.ID = sCouponID
	                                        ) AS iCouponGetedCount
                                        FROM
	                                        EHECD_Coupon AS a WHERE bIsDeleted=0 AND ID!='{0}' AND  sCouponName is NULL", new Guid()));// 排除商家入驻平台返还的给分享客的优惠劵
            //guid为null则加载全部数据，否则加载对应数据
            if (guid == null)
            {
                builder.Append(" AND a.sStoreID IS NULL ");
            }
            else {
                builder.Append(string.Format(" AND a.sStoreID='{0}' ", guid));
            }
            //设置查询条件
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString())&&!string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {
                //设置开始时间和结束时间
                builder.Append(string.Format(@" AND '{0}' >= dValidDateStart AND '{1}' <= dValidDateEnd", where.dValidDateStart.Value.ToString(), where.dValidDateEnd.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.iCoiCouponPrice.Value.ToString()))
            {
                //优惠劵名称
                builder.Append(string.Format(@" AND iCoiCouponPrice={0} ",where.iCoiCouponPrice.Value.ToString()));
            }
            //查询数据
            return query.PaginationQuery<Dictionary<string,object>>(builder.ToString(),pageInfo,null);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public override int DoAdd(EHECD_CouponDTO dto)
        { 
            return excute.InsertSingle<EHECD_CouponDTO>(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public override int DoDelete(string ids)
        {
            List<string> idArr = new List<string>();
            foreach (var item in ids.Split(','))
            {
                idArr.Add("'" + item + "'");
            }
            return excute.UpdateSingle<EHECD_CouponDTO>(new EHECD_CouponDTO() { bIsDeleted=true},string.Format(@" WHERE ID IN ({0}) ",string.Join(",", idArr)));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public override int DoEdit(EHECD_CouponDTO dto)
        {
            return excute.UpdateSingle<EHECD_CouponDTO>(dto,string.Format(" where ID='{0}' ",dto.ID));
        }

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override EHECD_CouponDTO GetSingle(Guid? id)
        {
            return query.SingleQuery<EHECD_CouponDTO>(@" SELECT
	                                                        TOP (1) *
                                                        FROM
	                                                        EHECD_Coupon
                                                        WHERE
	                                                        ID =@ID",new { ID= id });
        }
    }
}
