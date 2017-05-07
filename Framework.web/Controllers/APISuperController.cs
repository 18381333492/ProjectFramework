
namespace Framework.web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Framework.Domain;
    using Helper;
    using System.Collections.Generic;
    using System.Web;
    using Newtonsoft.Json;
    using System.Text;
    using DTO;
    public class APISuperController : Controller
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
            identity = ""
        };

        /// <summary>
        /// 处理前
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(RequestContext requestContext)
        {
            try
            {
                if (requestContext.HttpContext.Request.IsAjaxRequest() && requestContext.HttpContext.Request.RequestType.ToLower() == "post")
                {
                    RequestParameters = ParameterLoader.LoadAjaxPostParameters(requestContext.HttpContext.Request.InputStream);
                }
                base.Initialize(requestContext);
            }
            catch (Exception e)
            {
                SystemLog.Logs.GetLog().WriteErrorLog(e);
            }
            base.Initialize(requestContext);
        }

        /// <summary>
        /// 处理后
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {

            if (filterContext.Exception != default(Exception))
            {
                if (filterContext.Exception.GetType() == typeof(DomainInfoException))
                {
                    var ex = filterContext.Exception as DomainInfoException;
                    filterContext.ExceptionHandled = true;
                    if (ex.IsLog)
                    {
                        SystemLog.Logs.GetLog().WriteErrorLog(ex);
                        result.Succeeded = false;
                        result.Msg = ex.Message;
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                    else
                    {
                        result.Succeeded = false;
                        result.Msg = ex.Message;
                        filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                    }
                }
                else
                {
                    //处理controller的异常
                    SystemLog.Logs.GetLog().WriteErrorLog(filterContext.Exception);
                    filterContext.ExceptionHandled = false;                   
                }
            }
            else
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.HttpContext.Request.RequestType.ToLower() == "post")
                {
                    filterContext.Result = Content(ParameterLoader.LoadResponseJSONStr(result));
                }
            }
        }
        

        /// <summary>
        /// 载入指定会话内容
        /// </summary>
        /// <param name="sessionName"></param>
        /// <returns></returns>
        protected object GetSessionInfo(string sessionName)
        {
            var data = Session[sessionName];
            if(data==null&&sessionName == "_client")
            {
                try
                {
                    data = JsonConvert.DeserializeObject<EHECD_ClientDTO>(HttpUtility.UrlDecode(Request.Cookies[sessionName].Value, Encoding.UTF8));
                }
                catch
                {
                    data = null;
                }
               
            }
            return data;
           // return Session[sessionName];
        }

        /// <summary>
        /// 获取Sesssion中的数据并从会话中移除
        /// </summary>
        /// <param name="sessionName">session的key</param>
        /// <returns></returns>
        protected object GetSessionInfoNoResident(string sessionName)
        {
            var ret = GetSessionInfo(sessionName);
            Session[sessionName] = null;
            return ret;
        }

        /// <summary>
        /// 设置指定session内容
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="sessionInfo"></param>
        protected void SetSessionInfo(string sessionName, object sessionInfo)
        {
            Session[sessionName] = sessionInfo;
            if (sessionName == "_client")
            {//保存登陆信息
                Response.Cookies[sessionName].Value = HttpUtility.UrlEncode(JsonConvert.SerializeObject(sessionInfo), Encoding.UTF8);
                Response.Cookies[sessionName].Expires = DateTime.Now.AddDays(7);//保存7天
            }
           // Session[sessionName] = sessionInfo;
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
        /// 载入动态参数
        /// </summary>
        /// <returns></returns>
        protected dynamic LoadDynamicParam()
        {
            var ret = JSONHelper.GetModel<dynamic>(RequestParameters.dataStr);
            if (ret != null) return ret;
            return null;
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
    }
}