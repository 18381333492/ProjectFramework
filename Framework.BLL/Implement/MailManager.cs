using Framework.Dapper;
using System.Collections.Generic;
using Framework.DTO;
using System;
using Framework.DI;
using System.Text;

namespace Framework.BLL
{
    public class MailManager : IMailManager
    {
        /// <summary>
        /// 获取邮件信息数据绑定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_SysMessageDTO> GetPageList(PageInfo info, Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select  ID,sSender,sMsgTitle,dInsertTime,sMsgContent,iRecevierType FROM EHECD_SysMessage  where bIsDeleted=0 ");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (ID in (select sMailID from EHECD_SysMessageDetail where sReceiver like '%{0}%' or sNickName like '%{0}%'))", dic["sKeyword"].ToString());
            }
            if (dic["dStartTime"] != null && !string.IsNullOrEmpty(dic["dStartTime"].ToString()))
            {
                sb.AppendFormat("and (dInsertTime >= '{0}')", dic["dStartTime"].ToString());
            }
            if (dic["dEndTime"] != null && !string.IsNullOrEmpty(dic["dEndTime"].ToString()))
            {
                sb.AppendFormat("and (dInsertTime <= '{0}')", dic["dEndTime"].ToString());
            }
            return query.PaginationQuery<EHECD_SysMessageDTO>(sb.ToString(), info, null);
        }
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override int SendMail(EHECD_SysMessageDTO info)
        {
           
            return excute.InsertSingle<EHECD_SysMessageDTO>(info);
        }
        /// <summary>
        ///发送明细
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override int SendAllPeople(EHECD_SysMessageDetailDTO info)
        {
            return excute.InsertSingle<EHECD_SysMessageDetailDTO>(info);
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SysMessageDTO Detail(string ID)
        {
            return query.SingleQuery<EHECD_SysMessageDTO>("select ID,sSender,dInsertTime,iRecevierType,sMsgTitle,sMsgContent from EHECD_SysMessage where bIsDeleted=0 and ID=@ID", new { ID =ID});
        }
        /// <summary>
        /// 根据ID查找收件人
        /// </summary>
        /// <param name="sMailID"></param>
        /// <returns></returns>
        public override IList<EHECD_SysMessageDetailDTO> SerachName(string sMailID)
        {
            return query.QueryList<EHECD_SysMessageDetailDTO>("select sReceiver from EHECD_SysMessageDetail where bIsDeleted=0 and sMailID=@sMailID", new { sMailID = sMailID });
        }
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteMail(Dictionary<string, object> dic)
        {
            List<string> builder = new List<string>();
            var helper = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
            if (dic["ID"] != null)
            {
                foreach (var item in (dic["ID"].ToString().Split(',')))
                {
                    builder.Add("'" + item + "'");
                }
            }
            return helper.UpdateSingle<EHECD_SysMessageDTO>(new EHECD_SysMessageDTO() { bIsDeleted = true }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteAll(Dictionary<string, object> dic)
        {
            List<string> builder = new List<string>();
            var helper = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
            if (dic["ID"] != null)
            {
                foreach (var item in (dic["ID"].ToString().Split(',')))
                {
                    builder.Add("'" + item + "'");
                }
            }
            return helper.UpdateSingle<EHECD_SysMessageDetailDTO>(new EHECD_SysMessageDetailDTO() { bIsDeleted = true }, string.Format("where sMailID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 查找第一个收件人的姓名
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SysMessageDetailDTO OneMail(string ID)
        {
            return query.SingleQuery<EHECD_SysMessageDetailDTO>("select sReceiver from EHECD_SysMessageDetail where bIsDeleted=0 and sMailID=@sMailID", new { sMailID = ID });
        }
    }
}
