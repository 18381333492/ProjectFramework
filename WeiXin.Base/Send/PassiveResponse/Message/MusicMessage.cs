
namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class MusicMessage : BaseMessage
    {
        public MusicMessage()
        {
            MsgType = MessageType.MUSIC.ToString().ToLower();
        }

        public CMusic[] Music;

        public class CMusic
        {
            /// <summary>
            /// 音乐标题
            /// </summary>
            public string Title;

            /// <summary>
            /// 音乐描述
            /// </summary>
            public string Description;

            /// <summary>
            /// 音乐链接
            /// </summary>
            public string MusicURL;

            /// <summary>
            /// 高质量音乐链接，WIFI环境优先使用该链接播放音乐
            /// </summary>
            public string HQMusicUrl;

            /// <summary>
            /// 缩略图的媒体id，通过上传多媒体文件，得到的id
            /// </summary>
            public string ThumbMediaId;
        }
    }
}
