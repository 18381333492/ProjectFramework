using WeiXin.Tool;

namespace WeiXin.Base.Receive.Event
{
    /// <summary>
    /// 上报地理位置事件信息
    /// </summary>
    public class LocationEvent : BaseMessage
    {
        /// <summary>
        /// 事件类型  LOCATION
        /// </summary>
        public CDATA Event;

        /// <summary>
        /// 地理位置纬度
        /// </summary>
        public float Latitude;

        /// <summary>
        /// 地理位置经度
        /// </summary>
        public float Longitude;

        /// <summary>
        /// 地理位置精度
        /// </summary>
        public float Precision;
    }
}
