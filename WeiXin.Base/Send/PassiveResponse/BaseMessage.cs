/***********************************************************************
 * Module:  BaseMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class BaseMessage
 ***********************************************************************/

using System;
using WeiXin.Tool;

namespace WeiXin.Base.Send
{
    public class BaseMessage
    {
        public BaseMessage()
        {
            this.CreateTime = (int)(DateTime.Now.ToFileTimeUtc());
        }

        /// <summary>
        /// ���շ��˺ţ��յ���OpenId��
        /// </summary>
        public string ToUserName;
        /// <summary>
        /// ������΢�ź�
        /// </summary>
        public string FromUserName;

        /// <summary>
        /// ��Ϣ����ʱ��
        /// </summary>
        public int CreateTime;

        /// <summary>
        /// ��Ϣ���
        /// </summary>
        public string MsgType;

        public override string ToString()
        {
            return XmlHelper.ObjectToXml(this, "xml");
        }
    }
}