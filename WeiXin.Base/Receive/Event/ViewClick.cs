using WeiXin.Tool;

namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 点击菜单跳转链接时的事件推送信息
    /// </summary>
    public class ViewClick : BaseMessage
    {
        /// <summary>
        /// 事件类型, VIEW
        /// </summary>
        public CDATA Event;

        /// <summary>
        /// 事件KEY值，设置的跳转URL
        /// </summary>
        public CDATA EventKey;
    }
}
