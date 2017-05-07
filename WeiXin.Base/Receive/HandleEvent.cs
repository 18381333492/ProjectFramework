using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXin.Base.Receive
{
    public class HandleEvent
    {
        /// <summary>
        /// 处理事件消息
        /// </summary>
        /// <param name="xmlcontent"></param>
        /// <returns></returns>
        public bool ProcessMessage(EventType type, string xmlcontent)
        {
            return true;
        }
    }
}
