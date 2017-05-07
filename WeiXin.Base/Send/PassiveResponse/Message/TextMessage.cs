/***********************************************************************
 * Module:  TextMessage.cs
 * Author:  ��С��
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
        /// �ظ�����Ϣ���ݣ����У���content���ܹ����У�΢�ſͻ��˾�֧�ֻ�����ʾ��
        /// </summary>
        public string Content;

    }
}