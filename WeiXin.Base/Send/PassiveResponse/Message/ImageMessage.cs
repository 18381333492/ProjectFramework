/***********************************************************************
 * Module:  ImageMessage.cs
 * Author:  ��С��
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
            /// ͨ���ϴ���ý���ļ����õ���id��
            /// </summary>
            public string MediaId;
        }
    }
}