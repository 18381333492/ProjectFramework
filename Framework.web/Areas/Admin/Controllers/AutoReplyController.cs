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
    public class AutoReplyController : SuperController
    {
        // GET: Admin/AutoReply
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 自动回复设置
        /// </summary>
        public void AutoReplySet() {
            EHECD_WeCharReplyDTO sto = LoadParam<EHECD_WeCharReplyDTO>();
            var ret = LoadInterface<IWechartManager>().AutoReplySet(sto, SessionUser.User);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系系统管理员";
        }
        /// <summary>
        /// 根据ID获取自动回复的信息
        /// </summary>
        public void GetAutoReply()
        {
            var ret = LoadInterface<IWechartManager>().GetAutoReply(SessionUser.User.ID.ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取数据失败";
            }
        }
        /// <summary>
        /// 是否开启以及本文类型
        /// </summary>
        public void GetOn()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["type"] = 2;
            dic["ID"] = SessionUser.User.ID;
            var ret = LoadInterface<IWechartManager>().GetOn(dic);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
            }
        }

        /// <summary>
        /// 获取自动回复的信息
        /// </summary>
        public void GetReply()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().GetFollowReply(SessionUser.User.ID.ToString(), dic);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }
    }
}