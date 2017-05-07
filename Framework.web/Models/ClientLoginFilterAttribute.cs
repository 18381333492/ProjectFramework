using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Framework.web.Models
{
    using BLL;
    using Framework.DTO;

    /// <summary>
    /// 验证商城端客户是否登录
    /// </summary>
    public class ClientLoginFilterAttribute : System.Web.Mvc.ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var method = filterContext.HttpContext.Request.HttpMethod.ToLower();

            // var client = filterContext.HttpContext.Session["_client"] as EHECD_ClientDTO;
            if (filterContext.HttpContext.Request.Cookies["_client"] != null)
            {
                var data = HttpUtility.UrlDecode(filterContext.HttpContext.Request.Cookies["_client"].Value, Encoding.UTF8);
                var client = JsonConvert.DeserializeObject<EHECD_ClientDTO>(data);
                if (client == null)
                {
                    if (method == "get")
                    {
                        filterContext.Result = new RedirectResult("/Client/ClientLogin");
                    }
                    else if (filterContext.HttpContext.Request.IsAjaxRequest() && method == "post")
                    {
                        throw new Domain.DomainInfoException("1");
                    }
                }
                else
                {
                    var bll = DI.DIEntity.GetInstance().GetImpl<IShopDetailManager>();
                    var user = bll.IsFrozen(client.ID.Value.ToString());
                    if (user.iState.Value == 0)
                    {//该用户已被冻结
                        if (method == "get")
                        {
                            filterContext.Result = new RedirectResult("/Client/ClientLogin");
                        }
                        else if (filterContext.HttpContext.Request.IsAjaxRequest() && method == "post")
                        {
                            //ContentResult res = new ContentResult();
                            //JObject job = new JObject();
                            //job.Add(new JProperty("Succeeded", false));
                            //job.Add(new JProperty("Msg", "你已被冻结,不能进行其他操作!"));
                            //res.Content = job.ToString();
                            //filterContext.Result = res;
                            throw new Domain.DomainInfoException("你已被冻结,不能进行其他操作");
                        }
                    }
                    else
                    {
                        base.OnActionExecuting(filterContext);
                    }
                }
            }
            else
            {//登录过期
                if (method == "get")
                {
                    filterContext.Result = new RedirectResult("/Client/ClientLogin");
                }
                else if (filterContext.HttpContext.Request.IsAjaxRequest() && method == "post")
                {
                    throw new Domain.DomainInfoException("1");
                }
            }
        }

    }
}