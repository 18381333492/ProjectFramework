/***********************************************************************
 * Module:  LocationMessage.cs
 * Author:  潘小刚
 * Purpose: Definition of the Class LocationMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class LocationMessage : BaseMessage
    {
        /// <summary>
        /// 地理位置维度
        /// </summary>
        public string Location_X;

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public string Location_Y;

        /// <summary>
        /// 地图缩放大小
        /// </summary>
        public string Scale;

        /// <summary>
        /// 地理位置信息
        /// </summary>
        public string Label;

    }
}