/***********************************************************************
 * Module:  TextMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class TextMessage
 ***********************************************************************/

namespace WeiXin.Base.Send.PassiveResponse.Message
{
    public class TextMessage : BaseMessage
    {
        public TextMessage(){
            MsgType = MessageType.TEXT.ToString().ToLower();
        }

        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        public string Content;

    }
}