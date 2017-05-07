using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Dapper
{
    /// <summary>
    /// 分页条件
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string OrderBy { get; set; } = string.Empty;

        /// <summary>
        /// 排序类型
        /// </summary>
        public OrderType orderType { get; set; } = OrderType.ASC;
    }

    /// <summary>
    /// 排序的枚举
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 降序
        /// </summary>
        DESC = 1,

        /// <summary>
        /// 升序
        /// </summary>
        ASC = 0
    }
}
