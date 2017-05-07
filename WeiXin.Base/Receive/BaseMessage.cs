/***********************************************************************
 * Module:  BaseMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class BaseMessage
 ***********************************************************************/

using System;
using WeiXin.Tool;

namespace WeiXin.Base.Receive
{
    public class BaseMessage
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public CDATA ToUserName;

        /// <summary>
        /// 发送方帐号（一个OpenID）
        /// </summary>
        public CDATA FromUserName;

        /// <summary>
        /// 消息创建时间 （整型）
        /// </summary>
        public int CreateTime;

        /// <summary>
        /// 消息类型
        /// </summary>
        public CDATA MsgType;
        
        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgID;

        public override string ToString()
        {
            return XmlHelper.ObjectToXml(this);
        }
    }
}