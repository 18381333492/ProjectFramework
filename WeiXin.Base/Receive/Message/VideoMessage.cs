/***********************************************************************
 * Module:  VideoMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class VideoMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class VideoMessage : BaseMessage
    {
        /// <summary>
        /// ��Ƶ��Ϣý��id�����Ե��ö�ý���ļ����ؽӿ���ȡ����
        /// </summary>
        public string MediaId;

        /// <summary>
        /// ��Ƶ��Ϣ����ͼ��ý��id�����Ե��ö�ý���ļ����ؽӿ���ȡ����
        /// </summary>
        public string ThumbMediaId;

    }
}