/***********************************************************************
 * Module:  VoiceMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class VoiceMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class VoiceMessage : BaseMessage
    {
        /// <summary>
        /// 语音消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId;

        /// <summary>
        /// 语音格式，如amr，speex等
        /// </summary>
        public string Format;

    }
}