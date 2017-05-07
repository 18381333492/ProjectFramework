using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Web;
using Framework.Helper;


namespace Framework.web.config
{
    public class WebConfig
    {

        private static JObject configJson = new JObject();

        //初始化配置
        static WebConfig()
        {
            using (TextReader tr = new StreamReader(Path.Combine(HttpRuntime.BinDirectory, "frame.config.json")))
            {
                string configStr = tr.ReadToEnd();
                configJson = JSONHelper.GetModel<JObject>(configStr);
            }
        }

        /// <summary>
        /// 重新载入配置文件
        /// </summary>
        public static void ReLoadJsonConfig()
        {
            using (TextReader tr = new StreamReader(Path.Combine(HttpRuntime.BinDirectory, "frame.config.json")))
            {
                string configStr = tr.ReadToEnd();
                configJson = JSONHelper.GetModel<JObject>(configStr);
            }
        }

        /// <summary>
        /// 载入json配置
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>实际返回的是jobject</returns>
        /// <exception cref="null">出现异常返回的就是null</exception>
        public static dynamic LoadDynamicJson(string key)
        {
            try
            {                
                return (JObject)configJson.GetValue(key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static String LoadElement(string key)
        {
            var temp = ParseDictionary(key);
            if (temp.Count > 0)
            {
                string ret = null;
                if (temp.TryGetValue(key, out ret))
                {
                    return ret;
                }
                else
                {
                    throw new ApplicationException(string.Format("给定的关键字{0}未在配置中找到", key));
                }
            }
            else
            {
                throw new ApplicationException(string.Format("给定的关键字{0}未在配置中找到", key));
            }
        }

        /// <summary>
        /// 根据键载入token
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static JToken LoadJToken(string key)
        {
            JToken ret = null;
            configJson.TryGetValue(key, out ret);
            return ret;
        }

        public static IDictionary<string, string> ParseDictionary(string key)
        {
            var obj = LoadJToken(key);
            IDictionary<string, string> ret = new Dictionary<string, string>();
            if (obj == null) { return ret; }

            string nodeType = obj.GetType().Name;

            //节点是对象
            if (nodeType == "JObject")
            {
                var ele = (JObject)obj;
                ret.Add(ele.Path, ele.ToString());
                return ret;
            }

            //节点直接是键值方式
            if (nodeType == "JValue")
            {
                var val = (JValue)obj;
                ret.Add(val.Path, val.Value.ToString());
                return ret;
            }

            //节点是数组
            if (nodeType == "JArray")
            {
                var arr = (JArray)obj;
                ret.Add(arr.Path, arr.ToString());
                return ret;
            }

            return ret;
        }
    }
}
