using Newtonsoft.Json;
namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class TextMessage:BaseMessage
    {
        public TextMessage() {
            msgtype = MessageType.TEXT.ToString().ToLower();
        }

        /// <summary>
        /// 文本消息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Content text { get; set; }       
    }
}
