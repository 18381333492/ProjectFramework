using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Framework.SystemLog;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Framework.Dapper
{
    /// <summary>
    /// 查询的基类
    /// </summary>
    public abstract class QueryHelperBase : QueryHelper
    {
        //连接字符串
        private string connectionStr = null;

        // 获取数据库连接        
        private SqlConnection GetSqlConnection()
        {
            try
            {               
                if (connectionStr == null) connectionStr = web.config.WebConfig.LoadElement("connectionString");
                SqlConnection conn = new SqlConnection(connectionStr);
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
                return null;
            }
        }

        //关闭连接
        private void CloseConnect(SqlConnection conn)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
            }
        }

        /// <summary>
        /// 单条查询
        /// </summary>
        /// <remarks>
        ///     这部分代码由于实现了对异常的处理，并且开放给外部的接口
        ///     就是通过调用该部分，因此该方法不允许被重写
        /// </remarks>
        /// <typeparam name="T">查询的对象类型</typeparam>
        /// <param name="sqlCommand">sql命令</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        public override sealed T SingleQuery<T>(string sqlCommand, Object parameter)
        {
            SqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                if (conn == null) throw new ApplicationException("未获取到连接对象。");
                return DoSingleQuery<T>(conn, sqlCommand, parameter);
            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
                return default(T);
            }
            finally
            {
                CloseConnect(conn);
            }
        }

        /// <summary>
        /// 查询多条数据
        /// </summary>
        /// <remarks>
        ///     这部分代码由于实现了对异常的处理，并且开放给外部的接口
        ///     就是通过调用该部分，因此该方法不允许被重写
        /// </remarks>
        /// <typeparam name="T">查询的对象类型</typeparam>
        /// <param name="sqlCommand">sql命令</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        public override sealed IList<T> QueryList<T>(string sqlCommand, Object parameter)
        {
            SqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                if (conn == null) throw new ApplicationException("未获取到连接对象。");
                return DoQueryList<T>(conn, sqlCommand, parameter);
            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
                return null;
            }
            finally
            {
                CloseConnect(conn);
            }
        }
                
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <remarks>
        ///     这部分代码由于实现了对异常的处理，并且开放给外部的接口
        ///     就是通过调用该部分，因此该方法不允许被重写
        /// </remarks>
        /// <typeparam name="T">查询的对象类型</typeparam>
        /// <param name="sqlCommand">sql命令</param>
        /// <param name="pageInfo">分页查询条件对象</param>
        /// <param name="parameter">参数</param>
        /// <returns></returns>
        public override sealed PagingRet<T> PaginationQuery<T>(string sqlCommand, PageInfo pageInfo, Object parameter)
        {
            SqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                if (conn == null) throw new ApplicationException("未获取到连接对象。");
                return DoPaginationQuery<T>(conn, sqlCommand, pageInfo, parameter);
            }
            catch (Exception ex)
            {
                Logs.GetLog().WriteErrorLog(ex);
                return null;
            }
            finally
            {
                CloseConnect(conn);
            }
        }

        /// <summary>
        /// 子类真正应该实现的查询单条数据的方法
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        protected abstract T DoSingleQuery<T>(SqlConnection conn, string sqlCommand, Object parameter) where T : new();

        /// <summary>
        /// 子类真正应该实现的查询多条数据方法
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        protected abstract IList<T> DoQueryList<T>(SqlConnection conn, string sqlCommand, Object parameter) where T : new();

        /// <summary>
        /// 子类真正应该实现的分页查询数据方法
        /// </summary>
        /// <typeparam name="T">映射类型</typeparam>
        /// <param name="conn">数据库链接对象</param>
        /// <param name="sqlCommand">sql</param>
        /// <param name="parameter">参数</param>
        /// <returns>查询结果</returns>
        protected abstract PagingRet<T> DoPaginationQuery<T>(SqlConnection conn, string sqlCommand, PageInfo pageInfo, Object parameter);
    }
}
