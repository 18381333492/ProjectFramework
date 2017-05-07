/***********************************************************************
 * Module:  VideoMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class VideoMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class VideoMessage : BaseMessage
    {
        /// <summary>
        /// 视频消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId;

        /// <summary>
        /// 视频消息缩略图的媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string ThumbMediaId;

    }
}