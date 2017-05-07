using Newtonsoft.Json;

namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 图片消息
    /// </summary>
    public class VoiceMessage:BaseMessage
    {
        public VoiceMessage()
        {
            msgtype = MessageType.VOICE.ToString().ToLower();
        }
        /// <summary>
        /// 语音信息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Content voice;
    }
}
