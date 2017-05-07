/***********************************************************************
 * Module:  VoiceMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class VoiceMessage
 ***********************************************************************/

namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class VoiceMessage : BaseMessage
    {
        public VoiceMessage() {
            MsgType = MessageType.VOICE.ToString().ToLower();
        }
        public CVoice[] Voice;

        public class CVoice
        {
            /// <summary>
            /// ͨ���ϴ���ý���ļ����õ���id��
            /// </summary>
            public string MediaId;
        }
    }
}