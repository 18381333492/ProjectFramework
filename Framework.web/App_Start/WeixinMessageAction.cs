using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXin.Base.Receive;

namespace Framework.web.App_Start
{
    public class WeixinMessageAction : IAction
    {
        public string HandleImage(WeiXin.Base.Receive.Message.ImageMessage message)
        {
            return string.Empty;
        }

        public string HandleLink(WeiXin.Base.Receive.Message.LinkMessage message)
        {
            return string.Empty;
        }

        public string HandleLocation(WeiXin.Base.Receive.Message.LocationMessage message)
        {
            return string.Empty;
        }

        /// <summary>
        /// 处理文本消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string HandleText(WeiXin.Base.Receive.Message.TextMessage message)
        {
            string sResult = string.Empty,
                fromUserName = message.FromUserName.Text,
                toUserName = message.ToUserName.Text,
                keyword = message.Content;

            sResult = WeiXinTool.BuildKeyWordMessage(sResult, fromUserName, toUserName, keyword);
            return sResult;
        }

        public string HandleVideo(WeiXin.Base.Receive.Message.VideoMessage message)
        {
            return string.Empty;
        }

        public string HandleVoice(WeiXin.Base.Receive.Message.VoiceMessage message)
        {
            return string.Empty;
        }

        public string HandleClickEvent(WeiXin.Base.Receive.Event.ClickEvent eventdata)
        {
            string sResult = string.Empty;           
            return sResult;
        }

        public string HandleLocationEvent(WeiXin.Base.Receive.Event.LocationEvent eventdata)
        {
            return string.Empty;
        }

        public string HandleScanEvent(WeiXin.Base.Receive.Event.ScanEvent eventdata)
        {
            return string.Empty;
        }

        /// <summary>
        /// 关注时触发
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        public string HandleSubscribeEvent(WeiXin.Base.Receive.Event.SubscribeEvent eventdata)
        {            
            #region 关注时 触发
            string sResult = string.Empty;
            try
            {
                string fromUserName = eventdata.FromUserName.Text;              //openid
                string toUserName = eventdata.ToUserName.Text;                  //原始id
                string eventKey = eventdata.
                                    EventKey.
                                    Text.
                                    Replace("qrscene_", string.Empty);          //通过二维码扫码关注 获得的

                sResult = WeiXinTool.BuildAttentionAutoReply(fromUserName,toUserName,eventKey);

            }
            catch (Exception ex)
            {
                SystemLog.Logs.GetLog().WriteErrorLog(ex);
            }
            return sResult;
            #endregion
        }

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        public string HandleUnSubscribeEvent(WeiXin.Base.Receive.Event.UnSubscribeEvent eventdata)
        {
            #region 取消关注
            string sResult = string.Empty;            
            return sResult;
            #endregion
        }

        public string HandleViewClick(WeiXin.Base.Receive.Event.ViewClick eventdata)
        {
            return string.Empty;
        }

        /// <summary>
        /// 模板消息发送结果
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        public string HandleTemplateMessageStatus(WeiXin.Base.Send.Template.Message.TemplateMessageStatusEvent eventdata)
        {
            #region 模板消息发送结果
            string sResult = string.Empty;            
            return sResult;
            #endregion
        }
    }
}