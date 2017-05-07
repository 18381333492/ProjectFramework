/***********************************************************************
 * Module:  VideoMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class VideoMessage
 ***********************************************************************/

namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class VideoMessage : BaseMessage
    {
        public VideoMessage()
        {
            MsgType = MessageType.VIDEO.ToString().ToLower();
        }

        public CVideo[] Video;

        public class CVideo
        {
            /// <summary>
            /// 通过上传多媒体文件，得到的id
            /// </summary>
            public string MediaId;

            /// <summary>
            /// 视频消息的标题
            /// </summary>
            public string Title;

            /// <summary>
            /// 视频消息的描述
            /// </summary>
            public string Description;
        }

    }
}