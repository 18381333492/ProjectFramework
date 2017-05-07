using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Validate
{
    public interface IMessager
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobileNumber">手机号</param>
        /// <param name="MsgContent">短信内容</param>
        /// <returns></returns>
        bool SendMessage(string mobileNumber,string MsgContent);

        IRandomHelper RandomTool
        {
            get;            
        }

        /// <summary>
        /// 获取注册时的短信验证消息格式化字符串
        /// </summary>
        string RegisteMessage
        {
            get;
        }

        /// <summary>
        /// 获取修改密码时的短信验证消息格式化字符串
        /// </summary>
        string ChangePWDMessage
        {
            get;
        }
    }
}
