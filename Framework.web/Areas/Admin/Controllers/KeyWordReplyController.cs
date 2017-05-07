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
    public class KeyWordReplyController : SuperController
    {
        // GET: Admin/KeyWordReply
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }

       
        /// <summary>
        /// 获取页面绑定数据
        /// </summary>
        public void GetPageList() {
            PageInfo info = LoadParam<PageInfo>();
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().GetPageList(info, dic, SessionUser.User);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取页面数据失败";
            }
        }

        /// <summary>
        /// 添加关键字回复
        /// </summary>
        public void AddKeyReply() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            EHECD_WeCharReplyDTO dto = LoadParam<EHECD_WeCharReplyDTO>();
            var ret = LoadInterface<IWechartManager>().KeyApplyAdd(dto, SessionUser.User, dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加失败，请联系系统管理员";
        }
        /// <summary>
        /// 根据ID查看关键字回复的信息
        /// </summary>
        public void GetKeyReply()
        {
            Dictionary<string,object> dto = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().GetKeyReply(dto["ID"].ToString());
            if (ret != null) {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 修改关键字回复
        /// </summary>
        public void EditKeyReply()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            EHECD_WeCharReplyDTO dto = LoadParam<EHECD_WeCharReplyDTO>();
            var ret = LoadInterface<IWechartManager>().EditKeyReply(dto, SessionUser.User, dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "添加失败，请联系系统管理员";
        }

        public void DeleteKeyReply() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            dic["shopID"] = SessionUser.User.ID;
            var ret = LoadInterface<IWechartManager>().DeleteKeyReply(dic);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "删除失败，请联系系统管理员";
        }
        /// <summary>
        /// 开启或关闭关键字回复
        /// </summary>
        public void ChangeStates()
        {
            Dictionary<string,object> states = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().ChangeStates(bool.Parse(states["states"].ToString()), SessionUser.User.ID.ToString());
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系系统管理员";
        }
        /// <summary>
        /// 获取状态
        /// </summary>
        public void GetStates() {
            var ret = LoadInterface<IWechartManager>().GetStates(SessionUser.User.ID.ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 同名判断
        /// </summary>
        public void SearchKeyName() {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IWechartManager>().SearchKeyName(dic, SessionUser.User);
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "无同名";
            }
        }

        /// <summary>
        /// 更改回复的内容是文本或者是图文
        /// </summary>
        public void ChangeContentType()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            EHECD_WechartReplyTypeDTO type = LoadParam<EHECD_WechartReplyTypeDTO>();
            var ret = LoadInterface<IWechartManager>().ChangeContentType(type, SessionUser.User.ID.ToString());
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "保存失败，请联系系统管理员";
        }


        public void GetKeyWordByType()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            dic["sShopID"] = SessionUser.User.ID;
            var let = LoadInterface<IWechartManager>().GetKeyWordByType(dic);
            if (let != null)
            {
                result.Data = let;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "获取失败，请联系管理员";
            }

        }
    }
}