using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Framework.web.config;

namespace Framework.Validate
{
    public class ValidateSession
    {
        /// <summary>
        /// 最大验证次数
        /// </summary>
        private static int VCodeMaxCount = 0;

        /// <summary>
        /// 图片验证码超时时间（秒）
        /// </summary>
        private static int VImgCodeTimeOut = 0;

        static ValidateSession()
        {
            VCodeMaxCount/*最大验证次数*/ = Int32.Parse(WebConfig.LoadElement("VImgCodeMaxCount"));
            VImgCodeTimeOut/*图片验证码超时时间（秒）*/ = Int32.Parse(WebConfig.LoadElement("VImgCodeTimeOut"));
        }

        /// <summary>
        /// 验证图片验证码合法性
        /// </summary>
        /// <param name="vc"></param>
        /// <returns></returns>
        public static async Task<VImgCode> ValidateImgCodeSessionInfo(VImgCode vc)
        {
            return await Task<VImgCode>.Run<VImgCode>(() =>
               {
                   var timeNow = DateTime.Now;

                   //验证是否超时
                   if ((timeNow - vc.LastVCTime).TotalSeconds <= VImgCodeTimeOut)
                   {
                       //验证是否超过最大请求图片次数
                       if (vc.VCodeCount <= VCodeMaxCount)
                       {
                           vc.CheckCodeRet = true;
                           return vc;
                       }
                       else
                       {
                           vc.CheckCodeRet = false;
                           return vc;
                       }
                   }
                   else
                   {
                       vc.CheckCodeRet = true;
                       vc.VCodeCount = 0;
                       return vc;
                   }
               });
        }
    }
}
