using System.Collections.Generic;
using System.IO;
using System.Web;
using Framework.Helper;

namespace Framework.web
{
    public static class ParameterLoader
    {
        /// <summary>
        /// 载入AJax提交的请求参数
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static RequestData LoadAjaxPostParameters(Stream stream)
        {
            RequestData r = new RequestData();

            using (StreamReader sr = new StreamReader(stream))
            {
                // 获取源数据
                var sourceData = HttpUtility.UrlDecode(sr.ReadToEnd());

                // 获取数据字典
                var requstDic = JSONHelper.GetModel<IDictionary<string, object>>(sourceData);
                                
                object rdata = "";

                if (requstDic!=null && requstDic.TryGetValue("data", out rdata))
                {
                    if (rdata != null)
                    {
                        // 元数据字符串
                        r.dataStr = rdata.ToString();
                        // 数据对象（JObject）
                        r.data = JSONHelper.GetModel<object>(r.dataStr);
                        // 源数据动态对象
                        r.dynamicData = JSONHelper.GetModel<dynamic>(sourceData);                       
                    }
                }
            }
            return r;
        }

        /// <summary>
        /// 获取json响应
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public static string LoadResponseJSONStr(object responseData)
        {
            return JSONHelper.GetJsonString(responseData);
        }
    }
}
