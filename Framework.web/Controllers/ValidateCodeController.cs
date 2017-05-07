using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Validate;
using System.Threading.Tasks;

namespace Framework.web.Controllers
{
    public class ValidateCodeController : SuperController
    {
        /// <summary>
        /// 该方法用于获取验证码图片
        /// </summary>        
        public async Task VCode()
        {
            await Task.Run(() =>
            {
                SessionInfo session = (SessionInfo)Session[SessionInfo.USER_SESSION_NAME];

                if (session == null) session = new SessionInfo();

                //获取session理的code对象
                VImgCode vc = session.ImgCode;

                if (vc == null)
                {
                    //初始化session的图片验证信息
                    vc = new VImgCode
                    {
                        CheckCodeRet = false,
                        LastVCTime = DateTime.Now,
                        VCodeCount = 0
                    };
                }

                vc = ValidateSession.ValidateImgCodeSessionInfo(vc).Result;

                if (vc.CheckCodeRet)
                {
                    //本来这里是准备传byte数组的，但是由于想实现ajax请求，重复提交会有提示，因此后来改用base64
                    string vVerificationCode = ValidateCodeImgGenerator.RandomCode().Result;
                    var codes = string.Concat("data:image/png;base64,", Convert.ToBase64String(ValidateCodeImgGenerator.CreateCodeImage(vVerificationCode).Result));

                    vc.VCodeContent = vVerificationCode;
                    vc.LastVCTime = DateTime.Now;
                    vc.VCodeCount++;

                    session.ImgCode = vc;
                    Session[SessionInfo.USER_SESSION_NAME] = session;                    

                    result.Data = codes;
                    result.Succeeded = true;
                    result.Msg = "suc";
                }
                else
                {
                    result.Data = null;
                    result.Succeeded = false;
                    result.Msg = "请勿频繁重复请求图片验证码，请稍后再试";
                }
            });
        }
    }
}