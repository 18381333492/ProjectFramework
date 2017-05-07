using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class CheckShopController : SuperController
    {
        // GET: Admin/CheckShop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail() {
            var ID = Request.QueryString["ID"];
            EHECD_ApplyDTO user = LoadInterface<IShopManager>().ApplyUser(ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (user != null)
            {
                ViewBag.picture = LoadInterface<IPartnerManager>().SerachName(ID);
            }
            return PartialView(user);
        }

        //未通过审核页面绑定
        public void CheckPageList()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            PageInfo info = LoadParam<PageInfo>();
            //PageInfo info = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            result.Data = LoadInterface<IShopManager>().CheckPageList(info, dic);
            if (result.Data != null)
            {
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取数据失败";
            }
                
        }
        public void GetPassList()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            PageInfo info = LoadParam<PageInfo>();
            //var dic = LoadParam<Dictionary<string, object>>();
            //PageInfo info = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            result.Data = LoadInterface<IShopManager>().GetPassList(info, dic);
            if (result.Data != null)
            {
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取数据失败";
            }
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        public void PassCheck()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var city= dic["sCity"].ToString().Split(',');
            StringBuilder sb = new StringBuilder();
            dic["city"] = "";
            for (int i = 0; i < city.Length; i++) {
                sb.Append(PinYinHelper.GetPinYin(city[i], true).Substring(0, 1)).Append(',');
            }
            dic["city"] = sb.ToString().Substring(0, sb.ToString().Length - 1);
            var res = LoadInterface<IShopManager>().PassCheck(dic);
            result.Succeeded = res > 0;
            result.Msg = result.Succeeded ? "" : "审核失败，联系管理员";
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        public void DelayCheck()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var res = LoadInterface<IShopManager>().DelayCheck(ID);
            result.Succeeded = res > 0;
            result.Msg = result.Succeeded ? "" : "拒绝失败，联系管理员";
        }
        /// <summary>
        /// 删除未审核店铺
        /// </summary>
        public void DeleteCheck()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var ret = LoadInterface<IShopManager>().DeleteUnCheck(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";
            
        }
    }
}