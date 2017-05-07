using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Helper;
using Framework.BLL;
using Framework.DI;

namespace Framework.web.Areas.Client.Controllers
{
    /// <summary>
    /// 商户端登录
    /// </summary>
    public class MerchantLoginController : APISuperController
    {
        #region 视图
        /// <summary>
        /// 登录视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 显示菜单页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Menu()
        {
            ViewBag.sType = (GetSessionInfo("_merchant") as Dictionary<string, object>)["roleType"];
            return View();
        }

        #endregion

        #region 操作

        /// <summary>
        /// 登录操作
        /// </summary>
        public void Login()
        {
            var param = LoadParam<Dictionary<string, object>>();
            Dictionary<string, object> resultDic = LoadInterface<IMerchantLoginManager>().Login(param);
            bool flag = resultDic != null;
            Dictionary<string, object> dic = resultDic;
            if (flag&& dic["message"].ToString()== "登录成功")
            {
                result.Data = resultDic;
                // 将登陆信息存入Session:
                // 查询结果：
                //      roleType:0→扫描员  1→管理员
                //      sShopID：店铺ID
                SetSessionInfo("_merchant", resultDic);
            }
            else
            {
                result.Data = resultDic;
            }
            result.Msg = flag ? "登录成功":"登录名或密码错误";
            result.Succeeded = flag;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void signOut()
        {
            SetSessionInfo("_merchant",null);
            result.Data = "";
            result.Msg = "ok";
            result.Succeeded = true;
        }

        #endregion
    }
}