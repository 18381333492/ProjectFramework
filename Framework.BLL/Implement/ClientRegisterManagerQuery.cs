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

    using Framework.DTO;

    public partial class ClientRegisterManager : IClientRegisterManager
    {
        /// <summary>
        /// 载入用户协议
        /// </summary>
        /// <returns></returns>
        public override EHECD_ArticleDTO LoadUserProtocal()
        {
            return query.SingleQuery<EHECD_ArticleDTO>("SELECT sTitle,sContent FROM EHECD_Article WHERE sTitle = @title AND bIsDeleted=0", new { title= "用户协议" });
        }

        /// <summary>
        /// 判断注册用户的账号是否已注册
        /// </summary>
        /// <param name="phoneNumber">注册的电话号码</param>
        /// <returns>校验结果</returns>
        public override bool ExistsClientRegistPhoneNumber(string phoneNumber)
        {
            return query.SingleQuery<Dictionary<string, object>>("SELECT 1 RET FROM EHECD_Client WHERE sPhone = @sPhone",new { sPhone = phoneNumber}) == null;
        }

        /// <summary>
        /// 判断注册用户的微信号是否已经绑定过手机号
        /// </summary>
        /// <returns></returns>
        public override bool ExistsClientRegistWeiXin(string openId)
        {
            return query.SingleQuery<Dictionary<string,object>>(@"SELECT * FROM EHECD_Client WHERE sOpenId=@sOpenId AND sPassWord IS NOT NULL AND sPassWord!=''",new { sOpenId = openId })==null;
        }
    }
}
