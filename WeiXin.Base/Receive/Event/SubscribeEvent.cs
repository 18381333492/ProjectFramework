using WeiXin.Tool;
namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 用户未关注时，进行关注后的事件推送信息
    /// </summary>
    public class SubscribeEvent : BaseMessage
    {
        /// <summary>
        /// 事件类型，subscribe
        /// </summary>
        public CDATA Event;

        /// <summary>
        ///  事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public CDATA EventKey;

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public CDATA Ticket;

    }
}
