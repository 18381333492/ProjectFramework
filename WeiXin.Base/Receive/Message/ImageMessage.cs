/***********************************************************************
 * Module:  ImageMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class ImageMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class ImageMessage : BaseMessage
    {
        /// <summary>
        /// 图片链接
        /// </summary>
        public string PicUrl;

        /// <summary>
        /// 图片消息媒体id，可以调用多媒体文件下载接口拉取数据
        /// </summary>
        public string MediaId;

    }
}