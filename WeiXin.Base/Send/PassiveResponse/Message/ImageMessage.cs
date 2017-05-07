/***********************************************************************
 * Module:  ImageMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class ImageMessage
 ***********************************************************************/

namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class ImageMessage : BaseMessage
    {
        public ImageMessage() {
            MsgType = MessageType.IMAGE.ToString().ToLower();
        }

        public CImage[] Image;

        public class CImage
        {
            /// <summary>
            /// 通过上传多媒体文件，得到的id。
            /// </summary>
            public string MediaId;
        }
    }
}