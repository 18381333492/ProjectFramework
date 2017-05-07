using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IMailManager:BaseBll
    {
        /// <summary>
        /// 获取邮件信息数据绑定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_SysMessageDTO> GetPageList(PageInfo info,Dictionary<string,object> dic);
        /// <summary>
        /// 发送站内信
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract int SendMail(EHECD_SysMessageDTO info);
        /// <summary>
        ///发送明细
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract int SendAllPeople(EHECD_SysMessageDetailDTO info);
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SysMessageDTO Detail(string ID);
        /// <summary>
        /// 根据ID查找收件人
        /// </summary>
        /// <param name="sMailID"></param>
        /// <returns></returns>
        public abstract IList<EHECD_SysMessageDetailDTO> SerachName(string sMailID);
        /// <summary>
        /// 删除邮件
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteMail(Dictionary<string,object> dic);
        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteAll(Dictionary<string,object> dic);
        /// <summary>
        /// 查找第一个收件人的姓名
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SysMessageDetailDTO OneMail(string ID);

    }
}
