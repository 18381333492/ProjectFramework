using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using WeiXin.Tool;
namespace WeiXin.Base.Send.Template
{
    /// <summary>
    ///1、所有服务号都可以在功能->添加功能插件处看到申请模板消息功能的入口，但只有认证后的服务号才可以申请模板消息的使用权限并获得该权限；
    ///2、需要选择公众账号服务所处的2个行业，每月可更改1次所选行业；
    ///3、在所选择行业的模板库中选用已有的模板进行调用；
    ///4、每个账号可以同时使用15个模板。
    ///5、当前每个模板的日调用上限为10万次【2014年11月18日将接口调用频率从默认的日1万次提升为日10万次，可在MP登录后的开发者中心查看】。
    ///<remarks>
    ///  接口地址：http://mp.weixin.qq.com/wiki/17/304c1885ea66dbedf7dc170d84999a9d.html#.E5.8F.91.E9.80.81.E6.A8.A1.E6.9D.BF.E6.B6.88.E6.81.AF
    /// </remarks>
    /// </summary>
    public class BaseMessage
    {
        /// <summary>
        /// 发送模板消息地址
        /// 接口调用 URL地址
        /// http请求方式: POST
        /// "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";
        /// </summary>
        [JsonIgnore]
        private string PostUrl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

        /// <summary>
        /// 调用接口凭证
        /// </summary>
        [JsonIgnore]
        public string access_token { get; set; }


        /// <summary>
        /// 接收者用户的OPENID
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// 模板消息ID
        /// </summary>
        public string template_id { get; set; }

        /// <summary>
        /// 链接地址,点击消息的链接地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 顶部颜色
        /// </summary>
        public string topcolor { get; set; }

        [JsonIgnore]
        public Data content { set; get; }

        /// <summary>
        /// 参数数据，  一一对应
        /// </summary>
        public JObject data { get { return content.ToString(); } }


        /// <summary>
        /// 推送内容
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 发送消息
        /// <remarks>
        /// 在模版消息发送任务完成后，微信服务器会将是否送达成功作为通知，发送到开发者中心中填写的服务器配置地址中。
        /// 微信发送的通知信息内容来检测模板消息，用户是否收到
        /// </remarks>
        /// </summary>
        /// <param name="msgid">消息id</param>
        /// <returns></returns>
        public bool Send(out string msgid)
        {
            string sResult = HttpHelper.Post(string.Format(PostUrl, access_token), this.ToString());
            JObject jResult = JObject.Parse(sResult);
            /*信息成功发出后收到的信息：{
               "errcode":0,
               "errmsg":"ok",
               "msgid":200228332
           }*/
            var isSuccess = jResult["errcode"].ToString().Equals("0");
            msgid = isSuccess ? jResult["msgid"].ToString() : string.Empty;
            return isSuccess;
        }

        #region 设置行业

        /// <summary>
        /// 设置所属行业,post地址
        /// </summary>
        [JsonIgnore]
        private string sSetUrl = "https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token={0}";
        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="industry_id1">公众号模板消息所属行业编号</param>
        /// <param name="industry_id2">公众号模板消息所属行业编号</param>
        /// <returns></returns>
        public bool Set(int industry_id1, int industry_id2)
        {
            JObject jParams = new JObject();
            jParams.Add(new JProperty("industry_id1", industry_id1));
            jParams.Add(new JProperty("industry_id2", industry_id2));

            string sResult = HttpHelper.Post(string.Format(PostUrl, access_token), JsonConvert.SerializeObject(jParams));
            JObject jResult = JObject.Parse(sResult);
            return jResult["errcode"].ToString().Equals("0");
        }
        #endregion

        #region 获取模板ID
        /// <summary>
        /// 获取模板id，post地址
        /// </summary>
        [JsonIgnore]
        private string sTemplateIdUrl = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token={0}";

        /// <summary>
        /// 获取模板ID
        /// <remarks>template_id_short:模板库中模板的编号，有“TM**”和“OPENTMTM**”等形式</remarks>
        /// </summary>
        /// <param name="code">模板编号</param>
        /// <param name="template_id">模板ID</param>
        /// <returns></returns>
        public bool Get(string code,out string template_id)
        {
            JObject jParams = new JObject();
            jParams.Add(new JProperty("template_id_short", code));
            string sResult = HttpHelper.Post(string.Format(PostUrl, access_token), JsonConvert.SerializeObject(jParams));
            JObject jResult = JObject.Parse(sResult);
            bool isSuccess = jResult["errcode"].ToString().Equals("0");
            template_id = isSuccess ? jResult["template_id"].ToString() : string.Empty;
            return isSuccess;
        }
        #endregion
    }

    public class Data
    {
        [JsonIgnore]
        private IList<Data> list = new List<Data>();

        /// <summary>
        /// 模板消息关键字
        /// </summary>
        public string key { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">内容</param>
        /// <param name="color">字体颜色</param>
        public void Add(string key, string value, string color)
        {
            list.Add(new Data() { key = key, value = value, color = color });
        }

        /// <summary>
        /// 返回指定格式的字符串
        /// </summary>
        /// <returns></returns>
        public new JObject ToString()
        {
            int len = list.Count; if (len <= 0) return null;

            JObject jResult = new JObject();
            try
            {
                foreach (var item in list)
                {
                    var ValueColor = getItem(item);
                    jResult.Add(new JProperty(item.key, ValueColor));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return jResult;
        }

        public JObject getItem(Data data)
        {
            JObject jResult = new JObject();
            jResult.Add(new JProperty("value", data.value));
            jResult.Add(new JProperty("color", data.color));
            return jResult;
        }
    }
}
