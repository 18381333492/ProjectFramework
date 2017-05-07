using Framework.BLL;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.Validate;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Framework.web.Areas.Admin.Controllers
{
    public class MailController : SuperController
    {
        // GET: Admin/Mail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return PartialView();
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            //var ID = Request.QueryString["ID"];
            //EHECD_SysMessageDTO dto = LoadInterface<IMailManager>().Detail(ID);
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //string names = "";
            //if (dto.iRecevierType == 1) {
            //    IList<EHECD_SysMessageDetailDTO> sys = LoadInterface<IMailManager>().SerachName(ID);
            //    List<string> temp = new List<string>();
            //    foreach (var ab in sys)
            //    {
            //        temp.Add(ab.sReceiver);
            //    }
            //    names = string.Join(",", temp);
            //    //names = sys.ToString();
            //}
            //if (dto.iRecevierType == 0)
            //{
            //    names = "全部会员";
            //}
            //ViewBag.Names = names;
            //return PartialView(dto);
            return PartialView();
            
        }
        /// <summary>
        /// 页面绑定
        /// </summary>
        public void GetPageList()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            result.Data = LoadInterface<IMailManager>().GetPageList(page,dic);
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
        /// 用户页面绑定
        /// </summary>
        public void GetVipList()
        {
            PageInfo page = JSONHelper.GetModel<PageInfo>(RequestParameters.dataStr);
            var dic = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            result.Data = LoadInterface<IClientManager>().GetPageList(page, dic);
            result.Succeeded = true;
            result.Msg = "成功";
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        public void SendMail()
        {
            Dictionary<string, object> obj = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            EHECD_SysMessageDTO sys = new EHECD_SysMessageDTO();
            sys.ID = GuidHelper.GetSecuentialGuid();
            sys.dInsertTime = DateTime.Now;
            
            //获取登录名
            EHECD_SystemUserDTO user = (GetSessionInfo(SessionInfo.USER_SESSION_NAME) as SessionInfo).SessionUser.User;
            string name = user.sUserName;
            //站内信详情
            EHECD_SysMessageDetailDTO dto = new EHECD_SysMessageDetailDTO();
            
            dto.sMailID = sys.ID;
            sys.sSender = name;
            sys.sMsgTitle = obj["sMsgTitle"].ToString();
            sys.sMsgContent = obj["sMsgContent"].ToString();
            sys.iRecevierType = obj["iRecevierType"].ToByte();
            if (sys.iRecevierType == 1) { 
                 string[] ids = obj["sReceiverID"].ToString().Split(',');
                 string[] sReceivers = obj["sNickName"].ToString().Split(',');
                string[] sNickName = obj["sNickName"].ToString().Split(',');
                 for (int i = 0; i < ids.Length; i++)
                 {
                    dto.ID = GuidHelper.GetSecuentialGuid();
                    dto.sReceiverID =new Guid(ids[i]);
                    dto.sReceiver = sReceivers[i];
                    dto.sNickName = sNickName[i];
                    if (LoadInterface<IMailManager>().SendAllPeople(dto) > 0)
                    { result.Msg = "haha"; }
                    else { result.Msg = "hahahaha"; }

                    
               }
            }
            if (sys.iRecevierType == 0) {
                IList<EHECD_ClientDTO> abc = LoadInterface<IClientManager>().SearchPeople();
                
                foreach (var sv in abc) {
                    dto.sReceiverID = sv.ID;
                    dto.sReceiver = sv.sNickName;
                    dto.sNickName = sv.sNickName;
                    dto.ID = GuidHelper.GetSecuentialGuid();
                    LoadInterface<IMailManager>().SendAllPeople(dto);
                }
            }

            result.Data = LoadInterface<IMailManager>().SendMail(sys);
            result.Succeeded = true;
            result.Msg = "成功";
        }
        /// <summary>
        /// 删除站内信
        /// </summary>
        public void DeleteMail()
        {
           var ID = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            if (LoadInterface<IMailManager>().DeleteMail(ID) > 0)
            {
                result.Data = LoadInterface<IMailManager>().DeleteAll(ID);
                result.Succeeded = true;
                result.Msg = "成功";
            }
            
        }


        public void GetDetailById()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            var ret = LoadInterface<IMailManager>().Detail(dic["ID"].ToString());
            if (ret != null)
            {
                result.Data = ret;
                result.Succeeded = true;
            }
            else {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }

        public void SerachName()
        {
            Dictionary<string, object> dic = LoadParam<Dictionary<string, object>>();
            IList<EHECD_SysMessageDetailDTO> sys = LoadInterface<IMailManager>().SerachName(dic["ID"].ToString());
            if (sys != null) {
                result.Data = sys;
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "失败";
            }
        }
    }

       
    }
