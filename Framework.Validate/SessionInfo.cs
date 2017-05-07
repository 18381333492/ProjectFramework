using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.Validate
{
    /// <summary>
    /// 图片验证码信息对象
    /// </summary>
    [Serializable]
    public class VImgCode
    {
        /// <summary>
        /// 校验结果
        /// </summary>
        public bool CheckCodeRet { get; set; }

        /// <summary>
        /// 上次获取验证码时间
        /// </summary>
        public DateTime LastVCTime { get; set; }

        /// <summary>
        /// 已获取过验证码次数
        /// </summary>
        public int VCodeCount { get; set; } = 0;

        /// <summary>
        /// 获取的验证码
        /// </summary>
        public string VCodeContent { get; set; } = "";
    }

    /// <summary>
    /// 会员信息
    /// </summary>    
    [Serializable]
    public class UserInfo
    {
        public Framework.DTO.EHECD_SystemUserDTO User { get; set; }
    }

    /// <summary>
    /// session信息
    /// </summary>
    [Serializable]
    public class SessionInfo
    {
        public static readonly string USER_SESSION_NAME = "$-@k-76^";
        public static readonly string APISESSION_NAME = "$-@k-71^";
        public static readonly string USER_MENUS = "$-@k-72^";

        /// <summary>
        /// 获取或设置会话用户信息
        /// </summary>
        public UserInfo SessionUser { get; set; } = new UserInfo();

        /// <summary>
        /// 获取或设置图片验证对象
        /// </summary>
        public VImgCode ImgCode { get; set; }
    }
}