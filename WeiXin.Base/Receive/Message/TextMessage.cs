/***********************************************************************
 * Module:  TextMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class TextMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class TextMessage : BaseMessage
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content;
    }
}