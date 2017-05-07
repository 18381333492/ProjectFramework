using WeiXin.Tool;

namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 用户取消关注时的事件推送信息
    /// </summary>
    public class UnSubscribeEvent : BaseMessage
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public CDATA Event;
    }
}
