/***********************************************************************
 * Module:  LocationMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class LocationMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class LocationMessage : BaseMessage
    {
        /// <summary>
        /// ����λ��ά��
        /// </summary>
        public string Location_X;

        /// <summary>
        /// ����λ�þ���
        /// </summary>
        public string Location_Y;

        /// <summary>
        /// ��ͼ���Ŵ�С
        /// </summary>
        public string Scale;

        /// <summary>
        /// ����λ����Ϣ
        /// </summary>
        public string Label;

    }
}