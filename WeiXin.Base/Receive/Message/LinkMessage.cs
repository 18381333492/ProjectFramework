/***********************************************************************
 * Module:  LinkMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class LinkMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class LinkMessage : BaseMessage
    {
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description;

        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url;

    }
}