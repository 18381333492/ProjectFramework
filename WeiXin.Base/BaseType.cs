
namespace WeiXin.Base
{
    /// <summary>
    /// 菜单类型
    /// </summary>
    public enum ButtonType
    {
        view, click
    }

    /// <summary>
    /// 事件类型
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 订阅
        /// </summary>
        subscribe,

        /// <summary>
        /// 取消订阅
        /// </summary>
        unsubscribe,

        /// <summary>
        /// 用户已关注时的事件推送
        /// </summary>
        SCAN,

        /// <summary>
        /// 上报地理位置事件
        /// </summary>
        LOCATION,

        /// <summary>
        /// 自定义菜单事件
        /// </summary>
        CLICK,

        /// <summary>
        /// 点击菜单跳转链接时的事件推送
        /// </summary>
        VIEW,

        /// <summary>
        /// 发送模板信息结果
        /// </summary>
        TEMPLATESENDJOBFINISH
    }


    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 事件消息
        /// </summary>
        EVENT,

        /// <summary>
        /// 文本消息
        /// </summary>
        TEXT,

        /// <summary>
        /// 图片消息
        /// </summary>
        IMAGE,

        /// <summary>
        /// 语音消息
        /// </summary>
        VOICE,

        /// <summary>
        /// 视频消息
        /// </summary>
        VIDEO,

        /// <summary>
        /// 音乐消息
        /// </summary>
        MUSIC,

        /// <summary>
        /// 图文消息
        /// </summary>
        NEWS,

        /// <summary>
        /// 链接消息
        /// </summary>
        LINK,

        /// <summary>
        /// 地理位置消息
        /// </summary>
        LOCATION
    }
}
