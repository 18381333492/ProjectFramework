using Framework.DTO;
using Framework.web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WeiXin.Base;
using WeiXin.Base.Receive;
using WeiXin.Tool;

namespace Framework.web.Controllers
{
    public class WeixinResultController : Controller
    {
        public void ResultFn(string echoStr, string signature, string timestamp, string nonce)
        {
            //string echoStr = ControllerContext.Request["echoStr"].ToString();
            //string signature = request["signature"].ToString();
            //string timestamp = request["timestamp"].ToString();
            //string nonce = request["nonce"].ToString();
                       
            var method = Request.HttpMethod.ToLower();

            if (method == "get")
            {
                var token = config.WebConfig.LoadDynamicJson("weixin").Token;

                string[] ArrTmp = { token, timestamp, nonce };
                Array.Sort(ArrTmp);//字典排序
                string sRandomStr = string.Join("", ArrTmp);
                sRandomStr = Helper.Security.GetSHA1Hash(sRandomStr);
                sRandomStr = sRandomStr.ToLower();
                
                if (sRandomStr == signature)
                {
                    Response.Write(echoStr);
                }
                else
                {
                    Response.Write("Url验证失败!");
                }
            }
            else
            {
                var result = RequestMesssage(Request, Response, new WeixinMessageAction());
                Response.Write(result);
                Response.End();                
            }
        }

        /// <summary>
        /// 处理接收过来的消息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="Action"></param>
        /// <returns></returns>
        public string RequestMesssage(HttpRequestBase request, HttpResponseBase response, IAction Action)
        {
            #region 处理接收过来的消息
            string sXmlContent = string.Empty,  //微信发送的xml内容
                sResult = string.Empty;         //返回的xml内容
            try
            {
                using (var stream = request.InputStream)
                {
                    Byte[] byteData = new Byte[stream.Length];
                    stream.Read(byteData, 0, (Int32)stream.Length);

                    // 获取微信发过来的xml字符串
                    sXmlContent = Encoding.UTF8.GetString(byteData);
                }

                // 获取微信发送来的消息类型
                string sMsgType = XmlHelper.getTextByNode(sXmlContent, "MsgType");

                // 找到对应的消息类型
                MessageType msgType = (MessageType)Enum.Parse(typeof(MessageType), sMsgType.ToUpper());
                
                var handle = new HandleMessage(Action);

                if (msgType == MessageType.EVENT)
                {
                    // 获取事件类型
                    string sEventType = XmlHelper.getTextByNode(sXmlContent, "Event");

                    // 事件类型
                    EventType eventType = (EventType)Enum.Parse(typeof(EventType), sEventType);

                    // 获取处理结果
                    sResult = handle.ProcessEvent(eventType, sXmlContent);
                }
                else
                {
                    // 获取其他类型的响应
                    sResult = handle.ProcessMessage(msgType, sXmlContent);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sResult;
            #endregion
        }

        //public List<EHECD_WeCharReplyDTO> GetKeyWordMessage()
        //{
        //    int no = 0;
        //    var ret =new  BLL.WechartManager().GetKeyWordMessage("gh_99642e5393c2", "哈哈",out no);

        //    return ret;
        //}
    }
}
