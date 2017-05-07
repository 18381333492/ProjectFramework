/***********************************************************************
 * Module:  VoiceMessage.cs
 * Author:  潘小刚
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
            /// 通过上传多媒体文件，得到的id。
            /// </summary>
            public string MediaId;
        }
    }
}