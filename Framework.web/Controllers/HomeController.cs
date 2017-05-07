using Framework.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return Redirect("/Admin/Main");
        }

        /// <summary>
        /// 重新读取配置文件
        /// </summary>
        [HttpGet]
        public void RSJC()
        {
            Framework.web.config.WebConfig.ReLoadJsonConfig();
        }
    }
}