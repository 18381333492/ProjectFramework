using WeiXin.Tool;

namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 用户已关注时的事件推送信息
    /// </summary>
    public class ScanEvent : BaseMessage
    {
        /// <summary>
        /// 事件类型，SCAN
        /// </summary>
        public CDATA Event;

        /// <summary>
        /// 事件KEY值，是一个32位无符号整数，即创建二维码时的二维码scene_id
        /// </summary>
        public CDATA EventKey;

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public CDATA Ticket;
    }
}
