using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using Framework.Validate;

namespace Framework.Validate
{
    public class LoginCheckHttpModule : IHttpModule, IRequiresSessionState
    {
        void IHttpModule.Init(HttpApplication context)
        {
            context.AcquireRequestState += new EventHandler(context_AuthenticateRequest);
        }

        /// <summary>
        /// Session验证拦截
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        private void context_AuthenticateRequest(object obj, EventArgs e)
        {
            HttpApplication context = (HttpApplication)obj;

            //拦截页面
            if (Regex.IsMatch(context.Request.Url.LocalPath.ToLower(), "^/admin/"))
            {
                //登陆验证Session
                AdminLoginValidate(context);
            }
        }


        /// <summary>
        /// Admin后台登录验证
        /// </summary>
        /// <param name="context"></param>
        private void AdminLoginValidate(HttpApplication context)
        {
            //跳过登录页
            if (Regex.IsMatch(context.Request.Url.LocalPath.ToLower(), "^/admin/login"))
            {
                return;
            }

            //获取用户会话
            SessionInfo session = (SessionInfo)context.Session[SessionInfo.USER_SESSION_NAME];

            if (session == null || session.SessionUser == null || session.SessionUser.User == null)
            {
                context.CompleteRequest();
                context.Response.Redirect("/Admin/Login");
            }
        }
        

        
        /// <summary>
        /// Api登陆
        /// </summary>
        /// <param name="context"></param>
        private void ApiLoginValidate(HttpApplication context)
        {            
            ////要求验证
            //if (!context.Request.QueryString.ToString().Contains("token"))
            //{
            //    RequestSession.SessionUser.UserInfo.Client = null;
            //    return;
            //}
            ////通过token获取用户信息
            //EHECD_Client client = new ClientManager().LoadClientSign(context.Request.QueryString["token"].ToString().ToUpper());
            //if (client != null)
            //{
            //    RequestSession.SessionUser.UserInfo.Client = client;
            //}
            //else
            //{
            //    context.CompleteRequest();
            //}
        }

        /// <summary>
        /// 微信登录验证
        /// </summary>
        /// <param name="context"></param>
        private void WeiXinLoginValidate(HttpApplication context)
        {
            //如果不是微信浏览器则
            //if (!context.Request.UserAgent.ToLower().Contains("micromessenger"))
            //{
            //    return;
            //}
            //if (RequestSession.SessionUser.UserInfo.Client == null)
            //{
            //    context.CompleteRequest();
            //    context.Response.Redirect("/WeChat/Oauth");
            //}
        }
        
        void IHttpModule.Dispose()
        { }
    }
}