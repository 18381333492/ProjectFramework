using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    public abstract class ISystemLogManager
    {        
        /// <summary>
        /// 插入系统日志
        /// </summary>
        /// <param name="loginfo">系统操作日志</param>
        /// <returns>插入结果</returns>
        public abstract int InsertSystemLog(EHECD_SystemLogDTO loginfo, Dapper.ExcuteHelper excute);
    }
}
