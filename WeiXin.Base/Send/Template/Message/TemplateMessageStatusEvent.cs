using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using WeiXin.Tool;

namespace WeiXin.Base.Send.Template.Message
{
    /// <summary>
    /// 发送状态类别
    /// </summary>
    public enum StatusType
    {
        /// <summary>
        /// 发送成功
        /// </summary>
        SUCCESS,

        /// <summary>
        /// 发送失败(用户拒绝接收)
        /// </summary>
        USER_BLOCK,

        /// <summary>
        /// 发送失败(非用户拒绝)
        /// </summary>
        SYSTEM_FAILD
    }

    public class TemplateMessageStatusEvent
    {
        /// <summary>
        /// 公众号微信号
        /// </summary>
        public CDATA ToUserName { get; set; }

        /// <summary>
        /// 接收模板消息的用户的openid
        /// </summary>
        public CDATA FromUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public CDATA CreateTime { get; set; }

        /// <summary>
        /// 消息类型是事件
        /// </summary>
        public CDATA MsgType { get; set; }

        /// <summary>
        /// 事件为模板消息发送结束
        /// </summary>
        public CDATA Event { get; set; }

        /// <summary>
        /// 消息id
        /// </summary>
        public int MsgID { get; set; }

        /// <summary>
        /// 发送状态为成功
        /// </summary>
        public CDATA Status { get; set; }

        /// <summary>
        /// 发送状态类型
        /// </summary>
        [JsonIgnore]
        public StatusType StatusType
        {
            get
            {
                if (this.Status.Text.Equals("success"))
                {
                    return StatusType.SUCCESS;
                }
                else if (this.Status.Text.Equals("failed:user block"))
                {
                    return StatusType.USER_BLOCK;
                }
                else
                {
                    return StatusType.SYSTEM_FAILD;
                }
            }
        }

    }
}
