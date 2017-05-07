using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.Dapper
{
    public static class DBSqlHelper
    {
        /// <summary>
        /// 传入实体类获取插入sql
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>sql</returns>
        public static string GetInsertSQL<T>(T t)
        {
            StringBuilder mySQL = new StringBuilder();

            List<string> fs/*装字段名的集合*/ = new List<string>();
            List<string> vs/*装字段值的集合*/ = new List<string>();

            PropertyInfo[] pros = t.GetType().GetProperties();

            var tbName/*表名*/ = t.GetType().GetCustomAttribute<Framework.DTO.TableInfo>();

            mySQL.Append(string.Format("INSERT INTO [{0}]", tbName.TableName));

            foreach (var pro in pros)
            {
                var field/*字段对应的数据信息*/ = pro.GetCustomAttribute<Framework.DTO.FieldInfo>();

                var fvalue/*获取传入对象字段的值*/ = pro.GetValue(t);

                var value/*没有值就不插入，这个值是根据字段值来的*/ = fvalue != null ? fvalue.ToString() : "";

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    //加入字段名称
                    fs.Add(string.Format("[{0}]", field.FiledName));

                    var pName/*获取字段属性类型的名称*/ = pro.PropertyType.FullName.ToLower();

                    //加入value值
                    if (pName.IndexOf("guid") >= 0 || pName.IndexOf("datetime") >= 0 || pName.IndexOf("string") >= 0)
                    {
                        vs.Add(string.Format("'{0}'", value));
                    }
                    else if (pName.IndexOf("int") >= 0 || pName.IndexOf("byte") >= 0 || pName.IndexOf("decimal") >= 0)
                    {
                        vs.Add(string.Format("{0}", value));
                    }
                    else if (pName.IndexOf("boolean") >= 0)
                    {
                        vs.Add(string.Format("{0}", value.ToLower() == "false" ? "0" : "1"));
                    }
                }
            }
            mySQL.Append(string.Format("({0}) VALUES({1});", string.Join(",", fs), string.Join(",", vs)));
            return mySQL.ToString();
        }

        /// <summary>
        /// 传入实体类获取更新sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string GetUpdateSQL<T>(T t, string where)
        {
            StringBuilder mySQL = new StringBuilder();

            List<string> setStrArray/*更新值的set集合*/ = new List<string>();

            PropertyInfo[] pros/*属性集合*/ = t.GetType().GetProperties();

            var tbName/*表名*/ = t.GetType().GetCustomAttribute<Framework.DTO.TableInfo>();

            mySQL.Append(string.Format("UPDATE [{0}] SET ", tbName.TableName));

            foreach (var pro in pros)
            {

                var field/*字段对应的数据信息*/ = pro.GetCustomAttribute<Framework.DTO.FieldInfo>();

                var fvalue/*获取传入对象字段的值*/ = pro.GetValue(t);

                var value/*没有值就不更新，这个值是根据字段值来的*/ = fvalue != null ? fvalue.ToString() : "";

                if (!string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value))
                {
                    var pName/*字段名称*/ = pro.PropertyType.FullName.ToLower();

                    if (pName.IndexOf("guid") >= 0 || pName.IndexOf("datetime") >= 0 || pName.IndexOf("string") >= 0)
                    {
                        setStrArray.Add(string.Format("[{0}] = '{1}'", field.FiledName, value));
                    }
                    else if (pName.IndexOf("int") >= 0 || pName.IndexOf("byte") >= 0 || pName.IndexOf("decimal") >= 0)
                    {
                        setStrArray.Add(string.Format("[{0}] = {1}", field.FiledName, value));
                    }
                    else if (pName.IndexOf("boolean") >= 0)
                    {
                        setStrArray.Add(string.Format("[{0}] = {1}", field.FiledName, value.ToLower() == "true" ? 1 : 0));
                    }
                }
            }
            mySQL.Append(string.Join(",", setStrArray) + (where.StartsWith(" ") ? "" : " "));
            mySQL.Append(where.EndsWith(";") ? where : where + ";");
            return mySQL.ToString();
        }

        /// <summary>
        /// 获取物理删除的语句
        /// </summary>
        /// <typeparam name="T">要删除的对象类型</typeparam>
        /// <param name="t">要删除的对象</param>
        /// <param name="where">删除的条件</param>
        /// <returns>删除语句</returns>
        public static string GetDeleteSQL<T>(T t, string where)
        {
            StringBuilder mySQL = new StringBuilder();

            var tbName/*获取表名*/ = t.GetType().GetCustomAttribute<Framework.DTO.TableInfo>().TableName;

            mySQL.AppendFormat("DELETE [{0}]", tbName);
            mySQL.Append(where.StartsWith(" ") ? "" : " ");
            mySQL.Append(where.EndsWith(";") ? where : where + ";");

            return mySQL.ToString();
        }
    }
}
