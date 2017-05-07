using WeiXin.Tool;
namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 点击菜单拉取消息时的事件推送信息
    /// </summary>
    public class ClickEvent:BaseMessage
    {
        /// <summary>
        /// 事件类型,CLICK
        /// </summary>
        public CDATA Event;

        /// <summary>
        /// 事件KEY值，与自定义菜单接口中KEY值对应
        /// </summary>
        public CDATA EventKey;
    }
}
