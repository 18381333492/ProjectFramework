using WeiXin.Tool;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeiXin.Base.Receive.Event;
using WeiXin.Base.Receive.Message;
using System.Text.RegularExpressions;
using WeiXin.Base.Send.Template.Message;

namespace WeiXin.Base.Receive
{
    /// <summary>
    /// 处理接收消息类
    /// </summary>
    public class HandleMessage
    {
        /// <summary>
        /// 用于处理消息类
        /// </summary>
        private IAction Action { get; set; }

        public HandleMessage(IAction Action)
        {
            this.Action = Action;
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="xmlcontent"></param>
        /// <returns></returns>
        public string ProcessMessage(MessageType type, string xmlcontent)
        {
            if (null == Action) return string.Empty;

            string sResult = string.Empty;

            switch (type)
            {
                case MessageType.IMAGE:
                    sResult = Action.HandleImage(Convert<ImageMessage>(xmlcontent));
                    break;
                case MessageType.LINK:
                    sResult = Action.HandleLink(Convert<LinkMessage>(xmlcontent));
                    break;
                case MessageType.LOCATION:
                    sResult = Action.HandleLocation(Convert<LocationMessage>(xmlcontent));
                    break;
                case MessageType.TEXT:
                    sResult = Action.HandleText(Convert<TextMessage>(xmlcontent));
                    break;
                case MessageType.VIDEO:
                    sResult = Action.HandleVideo(Convert<VideoMessage>(xmlcontent));
                    break;
                case MessageType.VOICE:
                    sResult = Action.HandleVoice(Convert<VoiceMessage>(xmlcontent));
                    break;
            }
            return sResult;
        }


        /// <summary>
        /// 处理事件推送
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xmlcontent"></param>
        /// <returns></returns>
        public string ProcessEvent(EventType type, string xmlcontent)
        {
            if (null == Action) return string.Empty;

            string sResult = string.Empty;

            switch (type)
            {
                //订阅
                case EventType.subscribe:

                    sResult = Action.HandleSubscribeEvent(Convert<SubscribeEvent>(xmlcontent));
                    break;

                //取消订阅（取消关注）
                case EventType.unsubscribe:
                    sResult = Action.HandleUnSubscribeEvent(Convert<UnSubscribeEvent>(xmlcontent));
                    break;

                //用户已关注时的事件推送
                case EventType.SCAN:

                    sResult = Action.HandleScanEvent(Convert<ScanEvent>(xmlcontent));
                    break;

                // 上报地理位置事件
                case EventType.LOCATION:

                    sResult = Action.HandleLocationEvent(Convert<LocationEvent>(xmlcontent));
                    break;

                // 自定义菜单事件
                case EventType.CLICK:

                    sResult = Action.HandleClickEvent(Convert<ClickEvent>(xmlcontent));
                    break;

                // 点击菜单跳转链接时的事件推送
                case EventType.VIEW:

                    sResult = Action.HandleViewClick(Convert<ViewClick>(xmlcontent));
                    break;

                //模板信息结果事件
                case EventType.TEMPLATESENDJOBFINISH:

                    sResult = Action.HandleTemplateMessageStatus(Convert<TemplateMessageStatusEvent>(xmlcontent));
                    break;
            }
            return sResult;
        }

        /// <summary>
        /// xml格式内容转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlcontent"></param>
        /// <returns></returns>
        private T Convert<T>(string xmlcontent)
        {
            var sType = typeof(T).Name;
            var regex = new Regex("xml");
            xmlcontent = regex.Replace(xmlcontent, sType);
            return XmlHelper.XmlToObject<T>(xmlcontent);
        }
    }
}
