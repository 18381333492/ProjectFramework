/***********************************************************************
 * Module:  ImageMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class ImageMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class ImageMessage : BaseMessage
    {
        /// <summary>
        /// ͼƬ����
        /// </summary>
        public string PicUrl;

        /// <summary>
        /// ͼƬ��Ϣý��id�����Ե��ö�ý���ļ����ؽӿ���ȡ����
        /// </summary>
        public string MediaId;

    }
}