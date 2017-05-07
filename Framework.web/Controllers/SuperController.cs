using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Framework.Domain;
using Framework.DTO;
using Framework.Validate;
using Framework.Helper;

namespace Framework.web.Controllers
{
    public class SuperController : Controller
    {
        //响应的对象封装对象
        protected ResponseData result = new ResponseData
        {
            Data = null,
            Succeeded = false,
            Msg = ""
        };

        //ajax请求的参数封装对象
        protected RequestData RequestParameters = new RequestData
        {
            data = null,
            identity = "",
            dataStr = ""
        };

        //SessionUser
        protected UserInfo SessionUser = null;

        /// <summary>
        /// 这个地方是标识继承自该controller的菜单是否需要从session中筛选对应button
        /// </summary>
        protected Boolean NeedMenuButton = false;

        /// <summary>
        /// 初始化的时候
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            //ajax提交上来的请求
            if (requestContext.HttpContext.Request.IsAjaxRequest() && requestContext.HttpContext.Request.RequestType.ToLower() == "post")
            {
                //封装一下ajax的数据
                RequestParameters = ParameterLoader.LoadAjaxPostParameters(requestContext.HttpContext.Request.InputStream);
            }
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 处理action前
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionInfo session/*载入后台的用户session信息*/ = (SessionInfo)GetSessionInfo(SessionInfo.USER_SESSION_NAME);

            if (session == null || session.SessionUser == null || session.SessionUser.User == null)
            {
                var crtl/*获取请求的控制器路由字符串*/ = filterContext.HttpContext.Request.RequestContext.RouteData.Values["controller"].ToString();

                //排除掉登录和获取验证码的控制器
                if (crtl != "Login" && crtl != "ValidateCode")
                {
                    string url = new UrlHelper(filterContext.RequestContext).Action("Index", "Login");
                    //防止ajax调用分部视图出现登陆超时，在局部跳转URL的问题   
                    filterContext.HttpContext.Response.ContentType = "text/html";                    
                    filterContext.HttpContext.Response.Write("<script> alert('抱歉，您已登录超时，系统将于3秒后返回登录页重新登录');setTimeout(function(){ window.location.href='" + url + "';},3000); </script>");
                    filterContext.HttpContext.Response.End();
                }
            }
            else
            {
                SessionUser = session != null && session.SessionUser != null ? session.SessionUser : null;
            }
        }

        /// <summary>
        /// 处理后
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var isAjaxRequest/*是否是ajax异步请求*/ = filterContext.HttpContext.Request.IsAjaxRequest();
            var requestType/*请求类型*/ = filterContext.HttpContext.Request.RequestType.ToLower();

            //如果响应遇到异常
            if (filterContext.Exception != default(Exception))
            {
                //DomainInfoException表示这个是自定义的信息
                if (filterContext.Exception is DomainInfoException)
                {
                    var ex = filterContext.Exception as DomainInfoException;
                    if (ex != null && ex.IsLog)
                    {
                        SystemLog.Logs.GetLog().WriteErrorLog(ex);/*记录日志*/
                        result.Succeeded = false;
                        result.Msg = ex.Message;
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Msg = ex != null ? ex.Message : "后台执行中出现异常";
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                    filterContext.ExceptionHandled = true;
                }
                else
                {
                    //处理controller的异常
                    SystemLog.Logs.GetLog().WriteErrorLog(filterContext.Exception);
                    result.Succeeded = false;
                    result.Msg = filterContext.Exception.Message;
                    filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    filterContext.ExceptionHandled = true;
                }
            }
            else if (isAjaxRequest && requestType == "post")
            {
                //请求是脚本 ajax POST 的请求
                filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
            }
            else if (isAjaxRequest && requestType == "get")
            {
                //这里一般是通过easyui的panel或者dialog的get请求请求部分数据的处理
                var controller = filterContext.RequestContext.RouteData.Values["controller"].ToString().ToLower();
                var action = filterContext.RequestContext.RouteData.Values["action"].ToString().ToLower();

                //排除登录和菜单、首页还有不需要按钮的标识，如果非登录和菜单、首页，
                //你需要载入按钮的话，那么把NeedMenuButton置为true就可以了
                if (controller != "login" && controller != "menu" && controller != "main" && action == "index" || NeedMenuButton)
                {
                    //请求是get请求，一般都是打开某个菜单，获取会话中的用户权限菜单信息
                    var menuinfo = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;

                    if (menuinfo != null)
                    {
                        //找到当前控制器对应的菜单信息
                        var userMenu = menuinfo.AllMenu.Where(m =>
                        {
                            return m.sUrl.Split(new char[] { '/' }).Last().ToLower() == controller;
                        }).FirstOrDefault();

                        if (userMenu != default(UserMenu))
                        {
                            //添加这个菜单有的btn,这里是会排序的，如果编辑了菜单按钮的排序，刷新就可以了
                            filterContext.Controller.ViewData.Add("btns", userMenu.Buttons.OrderBy(m => m.iOrder).ToList());
                        }
                    }
                }
            }
            else
            {
                //其他方式的请求：暂无处理
            }
        }

        /// <summary>
        /// 载入指定会话内容
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        protected object GetSessionInfo(string sessionName)
        {
            return Session[sessionName];
        }

        /// <summary>
        /// 设置指定session内容
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="sessionInfo"></param>
        protected void SetSessionInfo(string sessionName, object sessionInfo)
        {
            Session[sessionName] = sessionInfo;
        }

        /// <summary>
        /// 将传入的jsonstr转为指定的对象
        /// </summary>
        /// <typeparam name="T">转换的对象类型</typeparam>
        /// <returns>转换类型</returns>
        protected T LoadParam<T>()
        {
            return JSONHelper.GetModel<T>(RequestParameters.dataStr);
        }

        /// <summary>
        /// 载入操作对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T LoadInterface<T>()
        {
            return DI.DIEntity.GetInstance().GetImpl<T>();
        }

        /// <summary>
        /// 创建系统日志信息
        /// </summary>
        protected void CreateSyslogInfo()
        {
            if (RequestParameters.dynamicData == null)
            {
                RequestParameters.dynamicData =
                    new
                    {
                        sLoginName = SessionUser.User.sLoginName,
                        sUserName = SessionUser.User.sUserName,
                        IP = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress
                    };

                //如果不使用mono.cecil来对程序集中的匿名类型修改，
                //将其指定为访问权限为public（默认是internal）,那
                //就用下面的json插件的类型来装，不然匿名类传到其他
                //程序集就无法访问了

                //JObject.Parse(JSONHelper.GetJsonString(new
                //{
                //    sLoginName = SessionUser.User.sLoginName,
                //    sUserName = SessionUser.User.sUserName,
                //    IP = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress
                //}));
            }
            else
            {
                RequestParameters.dynamicData.sLoginName = SessionUser.User.sLoginName;
                RequestParameters.dynamicData.sUserName = SessionUser.User.sUserName;
                RequestParameters.dynamicData.IP = Request.UserHostAddress == "::1" ? "127.0.0.1" : Request.UserHostAddress;
            }
        }
    }
}