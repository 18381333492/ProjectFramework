using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;


namespace Framework.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //初始化对象映射配置
            MapperConfig.MapConfiguration.Configure();

            // 微信商户支付配置信息
            Task.Factory.StartNew(() =>
            {
                //加载配置信息
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wxpay.config");
                WeiXin.Base.Pay.Lib.WxPayConfig.Initialize(configPath);
            });

        }

        //protected void Session_Start(object sender, EventArgs e)
        //{

        //}

        /// <summary>
        /// WebApiSession启动
        /// </summary>
        //public override void Init()
        //{
        //    this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        //    base.Init();
        //}
    }
}
