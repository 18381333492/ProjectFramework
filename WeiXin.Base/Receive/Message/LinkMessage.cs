/***********************************************************************
 * Module:  LinkMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class LinkMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class LinkMessage : BaseMessage
    {
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Title;

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Description;

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Url;

    }
}