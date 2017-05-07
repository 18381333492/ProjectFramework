using Newtonsoft.Json;

namespace WeiXin.Base.Send.CustomerService.Message
{
    /// <summary>
    /// 音乐消息
    /// </summary>
    public class MusicMessage:BaseMessage
    {
        public MusicMessage()
        {
            msgtype = MessageType.MUSIC.ToString().ToLower();
        }
        /// <summary>
        /// 音乐消息
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Content music;
    }
}
