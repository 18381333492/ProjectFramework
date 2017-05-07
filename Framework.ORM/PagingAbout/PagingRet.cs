using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Dapper
{
    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">分页承载数据的类型</typeparam>
    public class PagingRet<T>
    {
        /// <summary>
        /// 最大条数
        /// </summary>
        public int MaxCount { get; set; } = 0;

        /// <summary>
        /// 分页的结果
        /// </summary>
        public List<T> Result { get; set; }
    }
}
