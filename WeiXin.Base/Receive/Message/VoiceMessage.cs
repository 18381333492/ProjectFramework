/***********************************************************************
 * Module:  VoiceMessage.cs
 * Author:  ��С��
 * Purpose: Definition of the Class VoiceMessage
 ***********************************************************************/

using System;
namespace WeiXin.Base.Receive.Message
{
    public class VoiceMessage : BaseMessage
    {
        /// <summary>
        /// ������Ϣý��id�����Ե��ö�ý���ļ����ؽӿ���ȡ����
        /// </summary>
        public string MediaId;

        /// <summary>
        /// ������ʽ����amr��speex��
        /// </summary>
        public string Format;

    }
}