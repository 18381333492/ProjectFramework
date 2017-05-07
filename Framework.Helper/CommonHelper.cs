using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Helper
{
    public class CommonHelper
    {
        /// <summary>
        /// 创建系统日志信息
        /// </summary>
        /// <param name="dyP">动态参数</param> 
        /// <param name="Ip">ip地址</param>
        /// <param name="sLoginName">登录名</param>
        /// <param name="sUserName">用户名</param>
        public static void CreateSyslogInfo(ref dynamic dyP,string Ip,string sLoginName,string sUserName)
        {
            if (dyP == null) dyP = new { };
            dyP.dynamicData.sLoginName = sLoginName;
            dyP.dynamicData.sUserName = sUserName;
            dyP.dynamicData.IP = Ip == "::1" ? "127.0.0.1" : Ip;
        }
    }
}
