using Newtonsoft.Json;

namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class VideoMessage:BaseMessage
    {
        public VideoMessage()
        {
            msgtype = MessageType.VIDEO.ToString().ToLower();
        }
        /// <summary>
        /// 视频信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Content video;
    }
}
