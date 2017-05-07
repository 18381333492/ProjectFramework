using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
  public abstract class ISendMessage:BaseBll
    {
        /// <summary>
        /// 获取已发送短信详情
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_SendMeaasgeDTO> GetPageList(PageInfo page, Dictionary<String, object> dic, EHECD_SystemUserDTO user);
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int SendShortMessage(EHECD_SendMeaasgeDTO dto, EHECD_SystemUserDTO user);
        /// <summary>
        /// 根据ID查看详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SendMeaasgeDTO GetDetail(string ID);
        /// <summary>
        /// 获取会员的总数
        /// </summary>
        /// <returns></returns>
        public abstract int AllCount();
        /// <summary>
        /// 获取分享客的总数
        /// </summary>
        /// <returns></returns>
        public abstract int AllShareCount();
        /// <summary>
        /// 获取本店分享客的总数
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string,object> AllShopShareCount(EHECD_SystemUserDTO dto);

        /// <summary>
        /// 查出所有本店所有分销客的ID
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract IList<EHECD_SharedClientInfoDTO> SearchAllShareID(EHECD_SystemUserDTO dto);

        public abstract List<EHECD_ClientDTO> ClientPhoneByShare(EHECD_SystemUserDTO dto);
    }
}
