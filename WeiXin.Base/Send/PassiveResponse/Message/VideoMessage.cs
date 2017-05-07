/***********************************************************************
 * Module:  VideoMessage.cs
 * Author:  ��С��
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
            /// ͨ���ϴ���ý���ļ����õ���id
            /// </summary>
            public string MediaId;

            /// <summary>
            /// ��Ƶ��Ϣ�ı���
            /// </summary>
            public string Title;

            /// <summary>
            /// ��Ƶ��Ϣ������
            /// </summary>
            public string Description;
        }

    }
}