using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class PasswordController : SuperController
    {
        // GET: Admin/Password
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }

        public ActionResult Edit()
        {
            var ID = Request.QueryString["ID"];
            EHECD_ScannerDTO user = LoadInterface<IPasswordManager>().GetScannerById(ID);
            return PartialView(user);
        }
        public ActionResult EditPS()
        {
            var ID = Request.QueryString["ID"];
            EHECD_ScannerDTO user = LoadInterface<IPasswordManager>().GetScannerById(ID);
            return PartialView(user);
        }
        /// <summary>
        /// 获取页面绑定信息
        /// </summary>
        public void GetList()
        {
            PageInfo info = LoadParam<PageInfo>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IPasswordManager>().GetPageList(info,SessionUser.User,dic);
            result.Data = ret;
            result.Succeeded = true;
        }
        /// <summary>
        /// 添加扫面员
        /// </summary>
        public void AddScanner()
        {
            EHECD_ScannerDTO dto = LoadParam<EHECD_ScannerDTO>();
            var ret = LoadInterface<IPasswordManager>().AddScanner(dto, SessionUser.User);
            result.Succeeded = ret>0;
            result.Msg = result.Succeeded ? "" : "添加失败，请联系管理员";
        }
        /// <summary>
        /// 删除扫描员 
        /// </summary>
        public void DeleteScanner()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IPasswordManager>().DeleteScanner(dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加失败，请联系管理员";
        }
        /// <summary>
        /// 修改扫描员信息
        /// </summary>
        public void UpdateScanner()
        {
            EHECD_ScannerDTO user = LoadParam<EHECD_ScannerDTO>();
            
            var ret = LoadInterface<IPasswordManager>().EditScanner(user);
            result.Succeeded = ret !=null ;
            result.Msg = result.Succeeded ? "" : "修改失败，请联系系统管理员";
        }

        public void SearchName()
        {
            Dictionary<string,object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IPasswordManager>().SearchName(dic["name"].ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "操作失败，联系管理员";
            }
        }

    }
}