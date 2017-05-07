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
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override EHECD_ClientDTO Login(Dictionary<string, string> param)
        {
            var ret = query.SingleQuery<EHECD_ClientDTO>("SELECT * FROM EHECD_Client WHERE sPhone = @sPhone AND sPassWord = @sPassWord;", new
            {
                sPhone = param["phoneNumber"],
                sPassWord = Security.GetMD5Hash(param["pwd"])
            });

            return ret;
        }
    }
}
