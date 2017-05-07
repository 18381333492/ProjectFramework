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
    public class FollowReplyController : SuperController
    {
        // GET: Admin/FollowReply
        public ActionResult Index()
        {
            return View();
        }

        public void GetReply() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().GetFollowReply(SessionUser.User.ID.ToString(),dic);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取失败";
            }
        }

        /// <summary>
        /// 获取原始ID
        /// </summary>
        public void GetsOriginalID(){
             var let = LoadInterface<IWechartManager>().GetsOriginalID(SessionUser.User.ID.ToString());
            if (let == null)
            {
                result.Succeeded = false;
                result.Msg = "未完善店铺设置";
            }
            else {
                result.Succeeded = true;
                result.Data = let;
            }
         }

        /// <summary>
        /// 保存
        /// </summary>
        public void FollowReplySet() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();

            EHECD_WeCharReplyDTO reply = LoadParam<EHECD_WeCharReplyDTO>();
            var ret = LoadInterface<IWechartManager>().FollowReplySet(reply, SessionUser.User,dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系系统管理员";
        }

        /// <summary>
        /// 是否开启以及本文类型
        /// </summary>
        public void GetOn() {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic["type"] = 0;
            dic["ID"] = SessionUser.User.ID;
            var ret = LoadInterface<IWechartManager>().GetOn(dic);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
            }
        }

        
    }
}