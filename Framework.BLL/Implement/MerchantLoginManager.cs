using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Helper;

namespace Framework.BLL
{
    /// <summary>
    /// 商户端 登录
    /// </summary>
    public class MerchantLoginManager : IMerchantLoginManager
    {
        /// <summary>
        /// 登录:结果为null登录失败
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public override Dictionary<string,object> Login(Dictionary<string, object> param)
        {
            var name = CommonHelper.GetDictionaryValue("name",param,typeof(string));
            var pwd = Security.GetMD5Hash(CommonHelper.GetDictionaryValue("password", param, typeof(string)).ToString());

            Dictionary<string, object> result = new Dictionary<string, object>();
            result["message"] = "";
            if (!string.IsNullOrEmpty(name.ToString()) && !string.IsNullOrEmpty(pwd)) {
                //查询数据库数据
              Dictionary<string,object>  login = query.SingleQuery<Dictionary<string, object>>(@" IF EXISTS (
	                                                                        SELECT
		                                                                        1
	                                                                        FROM
		                                                                        EHECD_Scanner
	                                                                        WHERE
		                                                                        bIsDeleted = 0
	                                                                        AND sLoginName = @name
	                                                                        AND sPassword = @pwd
                                                                        )
                                                                        BEGIN
	                                                                        --扫描员
	                                                                        SELECT
		                                                                        roleType = 0,tUserState=0,
		                                                                        sShopID
	                                                                        FROM
		                                                                        EHECD_Scanner
	                                                                        WHERE
		                                                                        bIsDeleted = 0
	                                                                        AND sLoginName = @name
	                                                                        AND sPassword = @pwd ;
	                                                                        END
	                                                                        ELSE

	                                                                        BEGIN
		                                                                        --管理员
		                                                                        SELECT
			                                                                        roleType = 1,
			                                                                        ID sShopID,tUserState
		                                                                        FROM
			                                                                        EHECD_SystemUser
		                                                                        WHERE
			                                                                        bIsDeleted = 0 
		                                                                        AND sLoginName = @name
		                                                                        AND sPassWord = @pwd ;
		                                                                        END", new { name = name, pwd = pwd });
                if (login == null)
                {
                    result["message"] = "登录名或密码错误";
                }
                else if (login["tUserState"].ToInt32() == 1)
                {
                    result["message"] = "该账号已被冻结";
                }
                else {
                    result["message"] = "登录成功";
                    result["roleType"] = login["roleType"];
                    result["sShopID"] = login["sShopID"];
                }
            }

            return result;
        }
    }
}
