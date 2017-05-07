using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXin.Base.Receive.Message;
using WeiXin.Base.Receive.Event;
using WeiXin.Base.Send.Template.Message;

namespace WeiXin.Base.Receive
{
    /// <summary>
    /// 接收信息处理接口
    /// </summary>
    public interface IAction
    {
        /// <summary>
        /// 处理图片消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleImage(ImageMessage message);

        /// <summary>
        /// 处理链接消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleLink(LinkMessage message);

        /// <summary>
        /// 处理地理位置消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleLocation(LocationMessage message);

        /// <summary>
        /// 处理文本消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleText(TextMessage message);

        /// <summary>
        /// 处理视频消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleVideo(VideoMessage message);

        /// <summary>
        /// 处理语音消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        string HandleVoice(VoiceMessage message);

        /// <summary>
        /// 处理菜单 Click事件消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleClickEvent(ClickEvent eventdata);

        /// <summary>
        /// 处理获取地理位置消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleLocationEvent(LocationEvent eventdata);

        /// <summary>
        /// 处理二维码扫码消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleScanEvent(ScanEvent eventdata);

        /// <summary>
        /// 处理关系消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleSubscribeEvent(SubscribeEvent eventdata);

        /// <summary>
        /// 用户取消关注
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleUnSubscribeEvent(UnSubscribeEvent eventdata);

        /// <summary>
        /// 处理菜单View类型消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleViewClick(ViewClick eventdata);

        /// <summary>
        /// 处理接收模板消息
        /// </summary>
        /// <param name="eventdata"></param>
        /// <returns></returns>
        string HandleTemplateMessageStatus(TemplateMessageStatusEvent eventdata);
    }
}
