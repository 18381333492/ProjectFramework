using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;

namespace Framework.Dapper
{
    public class DapperExcuteHelper : ExcuteHelperBase
    {
        protected override int DoExcuteTransaction(SqlConnection conn, IDbTransaction tran, string sql)
        {
            int ret = conn.Execute(sql, null, tran, null, CommandType.Text);
            if (ret > 0)
            {
                tran.Commit();
            }
            else
            {
                tran.Rollback();
            }
            return ret;
        }

        protected override int DoInsert(SqlConnection conn, string sql, object param)
        {
            CommandDefinition command = new CommandDefinition(sql, param,null,30, CommandType.Text, CommandFlags.NoCache);           
            return conn.Execute(command);
        }

        protected override int DoInsertMultiple<T>(SqlConnection conn, IDbTransaction tran, IList<T> t)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in t)
            {
                sb.Append(DBSqlHelper.GetInsertSQL(item));
            }
            
            //考虑到有的时候批量插入需要按照顺序来插入，所以这个地方暂时不用并行运算了
            //Parallel.ForEach(t, item =>
            //{
            //    sb.Append(DBSqlHelper.GetInsertSQL(item));
            //});

            int row = conn.Execute(sb.ToString(), null, tran, null, CommandType.Text);

            if (row > 0)
            {
                tran.Commit();
            }
            else
            {
                tran.Rollback();
            }

            return row;
        }

        protected override int DoInsertSingle<T>(SqlConnection conn, T t)
        {
            string sql = DBSqlHelper.GetInsertSQL<T>(t);
            return conn.Execute(sql);
        }

        protected override int DoUpdate(SqlConnection conn, string sql, object param)
        {
            return conn.Execute(sql,param);
        }
        
        protected override int DoUpdateSingle<T>(SqlConnection conn, T t, string where)
        {
            string sql = DBSqlHelper.GetUpdateSQL<T>(t, where);
            return conn.Execute(sql);
        }
    }
}
