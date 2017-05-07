using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Dapper
{
    public abstract class QueryHelper
    {        
        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        public virtual T SingleQuery<T>(string sqlCommand,Object parameter) where T : new()
        {
            return new T();
        }

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        public virtual IList<T> QueryList<T>(string sqlCommand, Object parameter) where T : new()
        {
            return new List<T>();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        public virtual PagingRet<T> PaginationQuery<T>(string sqlCommand, PageInfo pageInfo, Object parameter)
        {
            return new PagingRet<T>();
        }
    }
}
