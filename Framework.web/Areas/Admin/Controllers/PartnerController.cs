using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class PartnerController : SuperController
    {
        // GET: Admin/Partner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add()
        {
            return PartialView();
        }

        public ActionResult Detail()
        {
            var ID = Request.QueryString["ID"];
            EHECD_SystemUserDTO user = LoadInterface<IPartnerManager>().GetPartnerDetail(ID);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            //IList<EHECD_ImagesDTO> temp = LoadInterface<IPartnerManager>().SerachName(ID);
            if (user != null)
            {
                ViewBag.picture = LoadInterface<IPartnerManager>().SerachName(ID);
            }
            return PartialView(user);
        }

        public ActionResult Edit()
        {
            var ID = Request.QueryString["ID"];
            EHECD_SystemUserDTO user = LoadInterface<IPartnerManager>().GetPartner(ID);
            return PartialView(user);
        }
        /// <summary>
        /// 添加合伙人
        /// </summary>
        public void AddPartner()
        {
            Dictionary<string, object> data = LoadParam<Dictionary<string, object>>();
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            IPartnerManager partner = LoadInterface<IPartnerManager>();
            IShopManager shop = LoadInterface<IShopManager>();
           
            if (shop.TheSeamLoginName(user) != null)
            {
                result.Msg = "已有相同的登录名";
            }
            else
            {

                var res = partner.AddPartner(data);
                result.Succeeded = res > 0;
                result.Msg = result.Succeeded ? "" : "新增店铺失败，请联系系统管理员";
            }
            
        }
        /// <summary>
        /// 已通过合伙人页面绑定
        /// </summary>
        public void GetPassPartner()
        {
            var dic = LoadParam<Dictionary<string, object>>();
            PageInfo info = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            result.Data = LoadInterface<IPartnerManager>().GetPassPartner(info, dic);
            result.Succeeded = true;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        public void UpDatePassword()
        {
            EHECD_SystemUserDTO user = LoadParam<EHECD_SystemUserDTO>();
            var ret = LoadInterface<IPartnerManager>().UpDatePartnerPassword(user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "修改密码失败，请联系系统管理员";
        }
        public void DeleteCheck()
        {
            var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            var ret = LoadInterface<IPartnerManager>().DeletePartner(ID);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";

        }
    }
}