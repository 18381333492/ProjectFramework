/***********************************************************************
 * Module:  BaseMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class BaseMessage
 ***********************************************************************/

using System;
using WeiXin.Tool;

namespace WeiXin.Base.Receive
{
    public class BaseMessage
    {
        /// <summary>
        /// ������΢�ź�
        /// </summary>
        public CDATA ToUserName;

        /// <summary>
        /// ���ͷ��ʺţ�һ��OpenID��
        /// </summary>
        public CDATA FromUserName;

        /// <summary>
        /// ��Ϣ����ʱ�� �����ͣ�
        /// </summary>
        public int CreateTime;

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public CDATA MsgType;
        
        /// <summary>
        /// ��Ϣid��64λ����
        /// </summary>
        public long MsgID;

        public override string ToString()
        {
            return XmlHelper.ObjectToXml(this);
        }
    }
}