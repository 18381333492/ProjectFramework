using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.web.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    /// <summary>
    /// 短信管理
    /// </summary>
    public class MessageController : SuperController
    {
        // GET: Admin/Message
        public ActionResult Index()
        {
            EHECD_SystemUserDTO dto = LoadInterface<IShopManager>().GetMessageCount(SessionUser.User.ID.ToString());
            return View(dto);
        }

        public ActionResult Add()
        {
            return PartialView();
        }
        public ActionResult AddShare()
        {
            return PartialView();
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            var ID = Request.QueryString["ID"];
            EHECD_SendMeaasgeDTO dto = LoadInterface<ISendMessage>().GetDetail(ID);
            return PartialView(dto);
        }
        /// <summary>
        /// 短信信息页面绑定
        /// </summary>

        public void GetPageList()
        {
            var dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var ret = LoadInterface<ISendMessage>().GetPageList(page, dic,SessionUser.User);
            result.Data = ret;
            result.Succeeded = true;
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        public void SendShortMessage()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            EHECD_SendMeaasgeDTO dto = LoadParam<EHECD_SendMeaasgeDTO>();
            EHECD_SystemUserDTO user = new EHECD_SystemUserDTO();
            var session= LoadInterface<IShopManager>().GetMessageCount(SessionUser.User.ID.ToString());
            user.sMessage = session.sMessage - int.Parse(dic["difference"].ToString());
            user.ID = SessionUser.User.ID;
            user.sUserName = SessionUser.User.sUserName;
            var ret = LoadInterface<ISendMessage>().SendShortMessage(dto, user);
            result.Succeeded = ret > 0;
            result.Msg = result.Succeeded ? "" : "发送短信失败，请联系系统管理员";            
        }
        /// <summary>
        /// 平台所有的用户
        /// </summary>
        public void AllCount()
        {
            var ret = LoadInterface<ISendMessage>().AllCount();
            if (ret >0)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Msg = "失败";
            }
        }
        /// <summary>
        /// 平台所有分享客的数量
        /// </summary>
        public void AllShareCount()
        {
            var ret = LoadInterface<ISendMessage>().AllShareCount();
            if (ret > 0)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Msg = "失败";
                result.Succeeded = false;
            }
        }
        /// <summary>
        /// 本店所有的分销客
        /// </summary>
        public void AllShopShareCount()
        {
            var ret = LoadInterface<ISendMessage>().AllShopShareCount(SessionUser.User);
            var let = LoadInterface<ISendMessage>().SearchAllShareID(SessionUser.User);
            if (ret!= null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else
            {
                result.Msg = "失败";
                result.Succeeded = false;
            }
        }
    }
}