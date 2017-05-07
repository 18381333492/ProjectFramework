using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using WeiXin.Tool;
using WeiXin.Base.Receive;

namespace WeiXin.Base
{
    /// <summary>
    /// 处理来自微信的信息
    /// </summary>
    public class HttpMessageTool
    {
        private const string VALID_URL_ERROR_MESSAGE = "验证URL出错";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="sToken">开发者验证页上的Token</param>
        /// <returns></returns>
        public static string ValidUrl(HttpRequestBase request,HttpResponseBase response,string sToken)
        {
            if (string.IsNullOrEmpty(sToken))
            {
                throw new ArgumentNullException("sToken为空");
            }
            string sEchostr = request["echoStr"].ToString();
            string sSignature = request["signature"].ToString();
            string sTimestamp = request["timestamp"].ToString();
            string sNonce = request["nonce"].ToString();

            return UrlVerify.Valid(sToken, sEchostr, sSignature, sTimestamp, sNonce) ? sEchostr : VALID_URL_ERROR_MESSAGE;
        }

        /// <summary>
        /// 处理接收过来的消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public static string RequestMesssage(HttpRequestBase request, HttpResponseBase response, IAction Action)
        {
            string sXmlContent = string.Empty, sResult = string.Empty;
            using (var stream = request.InputStream)
            {
                Byte[] byteData = new Byte[stream.Length];
                stream.Read(byteData, 0, (Int32)stream.Length);
                sXmlContent = Encoding.UTF8.GetString(byteData);
            }

            string sMsgType = XmlHelper.getTextByNode(sXmlContent, "MsgType");        

            MessageType msgType = (MessageType)Enum.Parse(typeof(MessageType),sMsgType.ToUpper());
            var handle = new HandleMessage(Action);
            if (msgType == MessageType.EVENT)
            {
                string sEventType = XmlHelper.getTextByNode(sXmlContent, "Event");   
                EventType eventType = (EventType)Enum.Parse(typeof(EventType), sEventType);
                sResult = handle.ProcessEvent(eventType, sXmlContent);
            }
            else {
                sResult = handle.ProcessMessage(msgType, sXmlContent);
            }
            return sResult;
        }
    }
}
