using Framework.BLL;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class WeChartSetController : SuperController
    {
        // GET: Admin/WeChartSet
        public ActionResult Index()
        {
            EHECD_WeChatSetDTO set = LoadInterface<IWechartManager>().GetSet(SessionUser.User.ID.ToString());
            if (set != null)
            {
                return View(set);
            }
            return View(new EHECD_WeChatSetDTO());
        }

        public void WeChartSet() {
            EHECD_WeChatSetDTO set = LoadParam<EHECD_WeChatSetDTO>();
            var ret = LoadInterface<IWechartManager>().WeChartSet(set, SessionUser.User);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系系统管理员";
        }
    }
}