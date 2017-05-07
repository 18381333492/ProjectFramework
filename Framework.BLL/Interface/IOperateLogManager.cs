using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    /// <summary>
    /// 优惠劵 【抽象】
    /// author 王其
    /// </summary>
    public abstract class IOperateLogManager : BaseBll
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetList(Dapper.PageInfo pageInfo,dynamic where);

        /// <summary>
        /// 写入操作日志
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public abstract int WirteOperateLog(EHECD_OperatDTO dto);
    }
}
