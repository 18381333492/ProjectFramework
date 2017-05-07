using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class UnpassPartnerController : SuperController
    {
        // GET: Admin/UnpassPartner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            var ID = Request.QueryString["ID"];
            EHECD_ApplyDTO user = LoadInterface<IShopManager>().ApplyUser(ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (user != null)
            {
                ViewBag.picture = LoadInterface<IPartnerManager>().SerachName(ID);
            }
            return PartialView(user);
        }

        public void GetUnPassPartner()
        {
            var dic = LoadParam<Dictionary<string, object>>();
            PageInfo info = LoadParam<PageInfo>();
            result.Data = LoadInterface<IPartnerManager>().GetUnPassPartner(info, dic);
            result.Succeeded = true;
        }

        /// <summary>
        /// 通过审核
        /// </summary>
        public void PassCheck()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            
            var res = LoadInterface<IPartnerManager>().PassPartner(dic);
            result.Succeeded = res > 0;
            result.Msg = result.Succeeded ? "" : "审核失败，联系管理员";
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        public void DelayCheck()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var res = LoadInterface<IPartnerManager>().DelayPartner(ID);
            result.Succeeded = res > 0;
            result.Msg = result.Succeeded ? "" : "拒绝失败，联系管理员";
        }

        public void DeleteCheck()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var ret = LoadInterface<IPartnerManager>().DeleteUnPassPartner(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";

        }
    }
}