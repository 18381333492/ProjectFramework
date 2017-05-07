using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    public abstract class IClientRegisterManager : BaseBll
    {
        /// <summary>
        /// 载入用户协议
        /// </summary>
        /// <returns></returns>
        public abstract EHECD_ArticleDTO LoadUserProtocal();

        /// <summary>
        /// 判断注册用户的账号是否已注册
        /// </summary>
        /// <param name="phoneNumber">注册的电话号码</param>
        /// <returns>校验结果</returns>
        public abstract bool ExistsClientRegistPhoneNumber(string phoneNumber);

        /// <summary>
        /// 注册客户
        /// </summary>
        /// <param name="eHECD_ClientDTO"></param>
        /// <returns></returns>
        public abstract EHECD_ClientDTO RegisteClient(EHECD_ClientDTO eHECD_ClientDTO);

        /// <summary>
        /// 判断注册用户的微信号是否已经绑定过手机号
        /// </summary>
        /// <param name="openId"></param>
        /// <returns></returns>
        public abstract bool ExistsClientRegistWeiXin(string openId);
    }
}
