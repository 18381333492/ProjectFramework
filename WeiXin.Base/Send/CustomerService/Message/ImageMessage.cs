using Newtonsoft.Json;

namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class ImageMessage:BaseMessage
    {
        public ImageMessage()
        {
            msgtype = MessageType.IMAGE.ToString().ToLower();
        }
        /// <summary>
        /// 图片信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Content image;
    }
}
