using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using System.Threading;

namespace Framework.BLL
{
    public class SendMessage : ISendMessage
    {


        /// <summary>
        /// 获取已发送短信详情
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_SendMeaasgeDTO> GetPageList(PageInfo page, Dictionary<string, object> dic, EHECD_SystemUserDTO user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select ID,sPeople,sContent,dSendTime from EHECD_SendMeaasge where bIsDeleted=0 and sSendID='" + user.ID + "'");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                sb.AppendFormat("and (sPeople like '%{0}%' or sContent like '%{0}%')", dic["sKeyword"].ToString());
            }
            return query.PaginationQuery<EHECD_SendMeaasgeDTO>(sb.ToString(), page, null);
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int SendShortMessage(EHECD_SendMeaasgeDTO dto, EHECD_SystemUserDTO user)
        {
            StringBuilder builder = new StringBuilder();
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.dSendTime = DateTime.Now;
            dto.sSendPeople = user.sUserName;
            dto.sSendID = user.ID;
            var di = DI.DIEntity.GetInstance().GetImpl<IClientManager>();
            var msgInterface = DI.DIEntity.GetInstance().GetImpl<Framework.Validate.IMessager>();
            if (dto.sPeople == "全部会员")
            {
                IList<EHECD_ClientDTO> temp = di.SearchPeople();
                foreach (var item in temp)
                {
                    msgInterface.SendMessage(item.sPhone, dto.sContent);
                    Thread.Sleep(10);
                }
            }
            else if (dto.sPeople == "全部分享客")
            {
                IList<EHECD_ClientDTO> temp = di.SerachVipPhone();
                foreach (var item in temp)
                {
                    msgInterface.SendMessage(item.sPhone, dto.sContent);
                    Thread.Sleep(10);
                }
            }
            else if (dto.sPeople == "本店所有分享客")
            {
              List<EHECD_ClientDTO> temp = DI.DIEntity.GetInstance().GetImpl<ISendMessage>().ClientPhoneByShare(user);
                foreach (var item in temp)
                {
                    msgInterface.SendMessage(item.sPhone, dto.sContent);
                    Thread.Sleep(10);
                }
            }



           

            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SendMeaasgeDTO>(dto)).Append(";");
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO() { sMessage = user.sMessage }, string.Format("where ID IN ('{0}')", user.ID)));
            return excute.ExcuteTransaction(builder.ToString());
        }
        public override EHECD_SendMeaasgeDTO GetDetail(string ID)
        {
            return query.SingleQuery<EHECD_SendMeaasgeDTO>("select ID,sPeople,sContent,dSendTime from EHECD_SendMeaasge where bIsDeleted=0 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 所有会员的数量
        /// </summary>
        /// <returns></returns>
        public override int AllCount()
        {
            var obj = query.SingleQuery<Dictionary<string, object>>("SELECT COUNT(iClientType) [count] FROM EHECD_Client", null);
            return obj["count"].ToInt32();
        }
        /// <summary>
        /// 平台所有分享客的数量
        /// </summary>
        /// <returns></returns>
        public override int AllShareCount()
        {
            var obj = query.SingleQuery<Dictionary<string, object>>("SELECT COUNT(iClientType) [count] FROM EHECD_Client where iClientType=1", null);
            return obj["count"].ToInt32();
        }
        /// <summary>
        /// 店铺所有分享客的数量
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override Dictionary<string, object> AllShopShareCount(EHECD_SystemUserDTO dto)
        {
            var obj = query.SingleQuery<Dictionary<string, object>>("SELECT COUNT(ID) [count] FROM EHECD_SharedClientInfo where sShopID=@sShopID", new { sShopID = dto.ID });
            return obj;
        }
        /// <summary>
        /// 查找本店所有分享客的ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override IList<EHECD_SharedClientInfoDTO> SearchAllShareID(EHECD_SystemUserDTO dto)
        {
            
            return query.QueryList<EHECD_SharedClientInfoDTO>("SELECT sClientID FROM EHECD_SharedClientInfo WHERE bIsDeleted=0 AND sShopID=@sShopID", new { sShopID = dto.ID });
        }
        /// <summary>
        /// 查号本店所有分享客的电话
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override List<EHECD_ClientDTO> ClientPhoneByShare(EHECD_SystemUserDTO dto)
        {
            var temp = DI.DIEntity.GetInstance().GetImpl<ISendMessage>().SearchAllShareID(dto);
            List<EHECD_ClientDTO> share = new List<EHECD_ClientDTO>();
            foreach (var item in temp)
            {
                EHECD_ClientDTO helper = query.SingleQuery<EHECD_ClientDTO>("select sPhone from EHECD_Client where bIsDeleted=0 and ID=@ID", new { ID = item.sClientID.ToString() });
                share.Add(helper);
            }
            return share;
        }
    }
}
