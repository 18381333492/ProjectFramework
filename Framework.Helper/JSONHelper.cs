using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using System;

namespace Framework
{
    public class JSONHelper
    {
        /// <summary>
        /// 从json字符串序列化为对象
        /// </summary>
        /// <typeparam name="T">序列化的类型</typeparam>
        /// <param name="sJson">json字符串</param>
        /// <returns>序列化结果</returns>
        public static T GetModel<T>(string sJson)
        {
            return JsonConvert.DeserializeObject<T>(sJson, new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            });
        }
        
        /// <summary>
        /// 对象序列化为json字符串
        /// </summary>
        /// <typeparam name="T">对象的类型</typeparam>
        /// <param name="t">要序列化的对象</param>
        /// <returns>序列化结果</returns>
        public static string GetJsonString<T>(T t)
        {
            return JsonConvert.SerializeObject(t, new JsonSerializerSettings()
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            });
        }
    }
}
