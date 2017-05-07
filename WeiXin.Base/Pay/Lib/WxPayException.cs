using System;
using System.Runtime.Serialization;

namespace WeiXin.Base.Pay.Lib
{
    /// <summary>
    /// 微信支付相关异常信息
    /// </summary>
    [Serializable]
    internal class WxPayException : Exception
    {
        public WxPayException()
        {
        }

        public WxPayException(string message) : base(message)
        {
        }

        public WxPayException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WxPayException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}