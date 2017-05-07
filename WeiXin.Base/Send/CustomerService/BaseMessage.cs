using Newtonsoft.Json;
using WeiXin.Tool;
using Newtonsoft.Json.Linq;

namespace WeiXin.Base.Send.CustomerService
{
    /// <summary>
    /// 开发者在一段时间内（目前修改为48小时）可以调用客服消息接口，
    /// 通过POST一个JSON数据包来发送消息给普通用户，
    /// 在48小时内不限制发送次数
    /// </summary>
    public class BaseMessage
    {
        /// <summary>
        /// 接口调用 URL地址
        /// http请求方式: POST
        /// "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";
        /// </summary>
        [JsonIgnore]
        private string PostUrl = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}";

        /// <summary>
        /// 调用接口凭证
        /// </summary>
        [JsonIgnore]
        public string access_token;

        /// <summary>
        /// 普通用户openid
        /// </summary>
        public string touser;

        /// <summary>
        /// 消息类型 text,image,voice,video,music,news
        /// </summary>
        public string msgtype;

        /// <summary>
        /// 返回JSON字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
           return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <returns></returns>
        public bool Send() {
           string sResult = HttpHelper.Post(string.Format(PostUrl, access_token), this.ToString());
           JObject jResult = JObject.Parse(sResult);
           return jResult["errcode"].ToString().Equals("0");
        }
    }

    #region 其他信息类
    public class Content
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string content { get; set; }

        /// <summary>
        /// 发送的媒体ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string media_id { get; set; }

        /// <summary>
        /// 消息的标题
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string title { get; set; }

        /// <summary>
        /// 消息的描述
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string description { get; set; }

        /// <summary>
        /// 音乐链接
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string musicurl { get; set; }

        /// <summary>
        /// 高品质音乐链接，wifi环境优先使用该链接播放音乐
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string hqmusicurl { get; set; }

        /// <summary>
        /// 缩略图的媒体ID
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string thumb_media_id { get; set; }

        /// <summary>
        /// 点击后跳转的链接
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string url { get; set; }

        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640*320，小图80*80
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string picurl { get; set; }
    }
    #endregion
}
