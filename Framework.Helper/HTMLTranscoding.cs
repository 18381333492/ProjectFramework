using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Framework.Helper
{
    public class HTMLTranscoding
    {
        /// <summary>
        /// 对字典中的字符串类型的值进行HTML解码
        /// </summary>
        /// <param name="dicl">要解码的字典</param>
        /// <returns>解码后的字典</returns>
        public static IDictionary<string, object> DictionaryDecodeHtml(IDictionary<string, object> dicl)
        {
            List<string> keys = new List<string>(dicl.Keys);
            for (int j = 0; j < keys.Count; j++)
            {
                if (dicl[keys[j]] is String)
                {
                    dicl[keys[j]] = HttpUtility.HtmlDecode(dicl[keys[j]].ToString());
                }
            }
            return dicl;
        }

        /// <summary>
        /// 对字典中的字符串类型的值进行HTML解码
        /// </summary>
        /// <param name="dicl">要解码的字典</param>
        /// <returns>解码后的字典</returns>
        public static IDictionary<string, object> DictionaryEncodeHtml(IDictionary<string, object> dicl)
        {
            List<string> keys = new List<string>(dicl.Keys);
            for (int j = 0; j < keys.Count; j++)
            {
                if (dicl[keys[j]] is String)
                {
                    dicl[keys[j]] = HttpUtility.HtmlEncode(dicl[keys[j]].ToString());
                }
            }
            return dicl;
        }

        /// <summary>
        /// 对字典集合中的字符串类型的值进行HTML解码
        /// </summary>
        /// <param name="dicl">要解码的字典集合</param>
        /// <returns>解码后的字典集合</returns>
        public static IList<IDictionary<string, object>> DictionaryListDecodeHtml(IList<IDictionary<string, object>> dicl)
        {
            for (int i = 0; i < dicl.Count; i++)
            {
                List<string> keys = new List<string>(dicl[i].Keys);
                for (int j = 0; j < keys.Count; j++)
                {
                    if (dicl[i][keys[j]] is String)
                    {
                        dicl[i][keys[j]] = HttpUtility.HtmlDecode(dicl[i][keys[j]].ToString());
                    }
                }
            }
            return dicl;
        }

        /// <summary>
        /// 对字典集合中的字符串类型的值进行HTML解码
        /// </summary>
        /// <param name="dicl">要解码的字典集合</param>
        /// <returns>解码后的字典集合</returns>
        public static IList<Dictionary<string, object>> DictionaryListEncodeHtml(IList<Dictionary<string, object>> dicl)
        {
            for (int i = 0; i < dicl.Count; i++)
            {
                List<string> keys = new List<string>(dicl[i].Keys);
                for (int j = 0; j < keys.Count; j++)
                {
                    if (dicl[i][keys[j]] is String)
                    {
                        dicl[i][keys[j]] = HttpUtility.HtmlEncode(dicl[i][keys[j]].ToString());
                    }
                }
            }
            return dicl;
        }
        
        /// <summary>
        /// 对字符串进行HTML编码
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string EncodeHtml(string t)
        {
            return HttpUtility.HtmlEncode(t);
        }

        /// <summary>
        /// 为对象的字符串字段进行HTML编码
        /// </summary>
        /// <typeparam name="T">对象实例类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>编码后的对象</returns>
        public static T EncodeHtml<T>(T t)
        {
            Type type = t.GetType();
            foreach (var pro in type.GetProperties())
            {
                if (pro.GetValue(t) != null && pro.GetValue(t) is String)
                {
                    pro.SetValue(t, HttpUtility.HtmlEncode(pro.GetValue(t).ToString()));
                }
            }
            return t;
        }

        /// <summary>
        /// 为对象的字符串字段进行HTML解码
        /// </summary>
        /// <typeparam name="T">对象实例类型</typeparam>
        /// <param name="t">对象实例</param>
        /// <returns>解码后的对象</returns>
        public static T DecodeHtml<T>(T t)
        {
            Type type = t.GetType();
            foreach (var pro in type.GetProperties())
            {
                if (pro.GetValue(t) != null && pro.GetValue(t) is String)
                {
                    pro.SetValue(t, HttpUtility.HtmlDecode(pro.GetValue(t).ToString()));
                }
            }
            return t;
        }

        /// <summary>
        /// 对泛型集合的字符串字段进行HTML解码
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">集合</param>
        /// <returns>结果</returns>
        public static IList<T> ListDecodeHtml<T>(IList<T> t)
        {
            for (int i = 0; i < t.Count; i++)
            {
                t[i] = DecodeHtml<T>(t[i]);
            }
            return t;
        }

        /// <summary>
        /// 对泛型集合的字符串字段进行HTML解码
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="t">集合</param>
        /// <returns>结果</returns>
        public static IList<T> ListEncodeHtml<T>(IList<T> t)
        {
            for (int i = 0; i < t.Count; i++)
            {
                t[i] = EncodeHtml<T>(t[i]);
            }
            return t;
        }
    }
}
