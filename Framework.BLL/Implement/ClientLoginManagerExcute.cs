using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.DI;
using Framework.MapperConfig;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class ClientLoginManager : IClientLoginManager
    {
        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="npwd"></param>
        /// <returns></returns>
        public override bool ChangePassword(string phoneNumber, string npwd)
        {
            var exis = query.SingleQuery<Dictionary<string, object>>("SELECT 1 ret FROM EHECD_Client WHERE sPhone = @sPhone", new { sPhone = phoneNumber });

            var ret = false;

            if(exis != null)
            {
                ret = excute.UpdateSingle<EHECD_ClientDTO>(new EHECD_ClientDTO { sPhone = phoneNumber, sPassWord = Framework.Helper.Security.GetMD5Hash(npwd) }, string.Format("where sPhone = '{0}'",phoneNumber)) > 0;
            }

            return ret;
        }
    }
}
