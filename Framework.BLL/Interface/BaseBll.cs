using Framework.DI;
using Framework.DTO;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    public class BaseBll
    {

        protected Dapper.QueryHelper query = DIEntity.GetInstance().GetImpl<Dapper.QueryHelper>();
        protected Dapper.ExcuteHelper excute = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
        protected ISystemLogManager sysLog = DIEntity.GetInstance().GetImpl<ISystemLogManager>();

        /// <summary>
        /// 插入系统操作日志
        /// </summary>
        /// <param name="ulname">操作者登录名</param>
        /// <param name="uname">操作者名字</param>
        /// <param name="ip">IP地址</param>
        /// <param name="type">操作类型</param>
        /// <param name="detail">操作简短描述</param>
        /// <param name="doid">操作涉及的ID</param>
        /// <param name="doret">操作结果，根据此结果生成对应信息</param>
        protected void InsertSystemLog(string ulname, string uname, string ip, short type, string detail, string doid, bool doret)
        {            
               var log = doret ?
                new EHECD_SystemLogDTO
                {
                    bIsDeleted = false,
                    dInsertTime = DateTime.Now,
                    ID = GuidHelper.GetSecuentialGuid(),
                    sIPAddress = ip,
                    sLoginName = ulname,
                    sUserName = uname,
                    sDomainDetail = detail,
                    sDoMainId = doid,
                    tDoType = type
                } :
                new EHECD_SystemLogDTO
                {
                    bIsDeleted = false,
                    dInsertTime = DateTime.Now,
                    ID = GuidHelper.GetSecuentialGuid(),
                    sIPAddress = ip,
                    sLoginName = ulname,
                    sUserName = uname,
                    sDomainDetail = detail + "失败",
                    sDoMainId = doid,
                    tDoType = type
                };
            sysLog.InsertSystemLog(log, excute);
        }
    }
}
