using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    public abstract class IClientLoginManager : BaseBll
    {
        /// <summary>
        /// 用户前端登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO Login(Dictionary<string,string> param);

        /// <summary>
        /// 用户修改密码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="npwd"></param>
        /// <returns></returns>
        public abstract bool ChangePassword(string phoneNumber, string npwd);
    }
}
