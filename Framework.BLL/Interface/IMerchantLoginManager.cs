using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
    /// <summary>
    /// 商户端登录接口
    /// </summary>
    public abstract class IMerchantLoginManager:BaseBll
    {
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="param">包含手机号和密码</param>
        /// <returns></returns>
        public abstract Dictionary<string,object> Login(Dictionary<string,object> param);
    }
}
