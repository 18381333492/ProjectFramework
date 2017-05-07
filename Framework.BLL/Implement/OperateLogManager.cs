using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;

namespace Framework.BLL
{
    public class OperateLogManager : IOperateLogManager
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="guid"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetList(PageInfo pageInfo,dynamic where)
        {
            pageInfo.orderType = OrderType.DESC;
            pageInfo.OrderBy = "dOperatTime";
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format(@"select *  from   EHECD_Operat WHERE  bIsDeleted = 0 "));
            
            //设置查询条件
            if (!string.IsNullOrEmpty(where.dValidDateStart.Value.ToString()) && !string.IsNullOrEmpty(where.dValidDateEnd.Value.ToString()))
            {
                //设置开始时间和结束时间
                builder.Append(string.Format(@" AND '{0} 00:00:00' <=dOperatTime AND '{1} 23:59:59' >= dOperatTime", where.dValidDateStart.Value.ToString(), where.dValidDateEnd.Value.ToString()));
            }
            if (!string.IsNullOrEmpty(where.sName.Value.ToString()))
            {
                //用户名或者是昵称
                builder.Append(string.Format(@" AND sOperator LIKE '%{0}%' ", where.sName.Value.ToString()));
            }
            //查询数据
            return query.PaginationQuery<Dictionary<string, object>>(builder.ToString(), pageInfo, null);
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int WirteOperateLog(EHECD_OperatDTO dto)
        {
            return excute.InsertSingle<EHECD_OperatDTO>(dto);
        }
    }
}
