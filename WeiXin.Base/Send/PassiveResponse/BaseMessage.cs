/***********************************************************************
 * Module:  BaseMessage.cs
 * Author:  潘小刚
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
        /// 接收方账号（收到的OpenId）
        /// </summary>
        public string ToUserName;
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string FromUserName;

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public int CreateTime;

        /// <summary>
        /// 消息类别
        /// </summary>
        public string MsgType;

        public override string ToString()
        {
            return XmlHelper.ObjectToXml(this, "xml");
        }
    }
}