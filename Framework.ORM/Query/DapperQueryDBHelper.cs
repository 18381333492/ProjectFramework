using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Reflection;
using Framework.Helper;

namespace Framework.Dapper
{
    public class DapperQueryDBHelper : QueryHelperBase
    {
        #region 分页语句
        private const string PAGINATION_SQL = @"DECLARE	@mc INT 
                                        SELECT	@mc = COUNT (1)	FROM ({0}) AS forMaxCount--最大条数

                                        SELECT	*,@mc MaxCount
		                                        FROM(
				                                        SELECT
					                                        ROW_NUMBER () OVER (ORDER BY {1}) AS ROWNUM ,*
				                                        FROM
					                                        ({0}) AS ret
			                                        ) AS query
		                                        WHERE
			                                        query.ROWNUM BETWEEN {2}
		                                        AND {3}";
        #endregion

        protected override PagingRet<T> DoPaginationQuery<T>(SqlConnection conn, string sqlCommand, PageInfo pageInfo, Object parameter)
        {
            //计算分页参数
            int startPage = pageInfo.PageIndex == 0 ? 1 : pageInfo.PageSize * (pageInfo.PageIndex - 1) + 1;
            int endPage = startPage + pageInfo.PageSize - 1;
            var result = new PagingRet<T>();

            //分页查询语句
            string sql = string.Format(PAGINATION_SQL,
                                               sqlCommand,
                                               pageInfo.OrderBy == string.Empty ? "(SELECT 0)" : (string.Format("{0} {1}", pageInfo.OrderBy, pageInfo.orderType == OrderType.ASC ? "ASC" : "DESC")),
                                               startPage,
                                               endPage);


            //获取查询结果（DapperRow[类型是IEnumerable<dynamic>]）,并将其转换为字典
            var ret = conn.Query(sql, parameter, null, true, null, CommandType.Text).Select(m => ((IDictionary<string, object>)m).ToDictionary(pi => pi.Key, pi => pi.Value)).ToList<IDictionary<string, object>>();

            if (ret.Count > 0)
            {
                //随意抽一条做最大条数记录
                result.MaxCount = ret[0]["MaxCount"].ToInt32();

                //如果泛型类型是字典，分页结果里面的泛型集合就是字典集合，否则就是设定的泛型类型集合
                var interfaceType = typeof(T).GetInterface("IDictionary");

                if (typeof(T).Name.IndexOf("IDictionary") >= 0 || interfaceType != null)
                {
                    result.Result = ret.Select(m => (T)m).ToList();
                }
                else
                {
                    result.Result = ret.Select(kv =>
                    {
                        var properties = typeof(T).GetProperties();
                        var item = (T)(typeof(T).GetConstructor(new Type[] { }).Invoke(default(Type[])));
                        //反射给泛型对象赋值
                        //TODO:现在没得时间，有时间了这里不会用反射的方式来赋值，有时间改为表达式树来生成赋值的方法委托然后调用
                        Parallel.ForEach(properties, property =>
                    {
                        object value = null;
                        if (kv.TryGetValue(property.Name, out value))
                        {
                            property.SetValue(item, value);
                        }
                    });
                        return item;
                    }).ToList<T>();
                }
            }
            else
            {
                result.Result = new List<T>();
            }
            return result;
        }

        protected override IList<T> DoQueryList<T>(SqlConnection conn, string sqlCommand, Object parameter)
        {
            var ret = conn.Query(sqlCommand, parameter, null, true, null, CommandType.Text).Select(m => ((IDictionary<string, object>)m).ToDictionary(pi => pi.Key, pi => pi.Value)).ToList<IDictionary<string, object>>();

            //var ret = conn.Query("select @PageSize + @PageNumber", new { PageSize = 5, PageNumber = 5 });

            //如果泛型类型是字典，泛型集合就是字典集合，否则就是设定的泛型类型集合
            var interfaceType = typeof(T).GetInterface("IDictionary");

            if (typeof(T).Name.IndexOf("IDictionary") >= 0 || interfaceType != null)
            {
                return ret.Select(m => (T)m).ToList();
            }
            else
            {
                return ret.Select(kv =>
                {
                    var properties = typeof(T).GetProperties();
                    var item = new T();
                    //反射给泛型对象赋值
                    //TODO:现在没得时间，有时间了这里不会用反射的方式来赋值，有时间改为表达式树来生成赋值的方法委托然后调用
                    Parallel.ForEach(properties, property =>
                {
                    object value = null;
                    if (kv.TryGetValue(property.Name, out value))
                    {
                        property.SetValue(item, value);
                    }
                });
                    return item;
                }).ToList<T>();
            }
        }

        protected override T DoSingleQuery<T>(SqlConnection conn, string sqlCommand, Object parameter)
        {
            var ret = 
                        // 从数据查询结果集
                        conn.Query(
                                sqlCommand,
                                parameter,
                                null,
                                true,
                                null,
                            CommandType.Text).
                          // 从结果集
                          Select(
                                m => ((IDictionary<string, object>)m).
                                                        ToDictionary(
                                                                pi => pi.Key, 
                                                                pi => pi.Value)).
                          
                          FirstOrDefault<IDictionary<string, object>>();

            if (ret == default(IDictionary<string, object>)) return default(T);

            //如果泛型类型是字典，结果就是字典，否则就是设定的泛型类型
            var interfaceType = typeof(T).GetInterface("IDictionary");

            if (typeof(T).Name.IndexOf("IDictionary") >= 0 || interfaceType != null)
            {
                return (T)ret;
            }
            else
            {
                var properties = typeof(T).GetProperties();
                var item = new T(); //(T)(typeof(T).GetConstructor(new Type[] { }).Invoke(default(Type[]))); 本来是选择通过构造函数创建对象，但由于要规避泛型传接口进来，所以对泛型类型进行了new约束
                //反射给泛型对象赋值
                //TODO:现在没得时间，有时间了这里不会用反射的方式来赋值，有时间改为表达式树来生成赋值的方法委托然后调用
                Parallel.ForEach(properties, property =>
                {
                    object value = null;
                    if (ret.TryGetValue(property.Name, out value))
                    {
                        property.SetValue(item, value);
                    }
                });
                return item;
            }
        }
    }
}
