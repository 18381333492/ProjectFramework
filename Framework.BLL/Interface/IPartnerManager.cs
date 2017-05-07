using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IPartnerManager:BaseBll
    {
        /// <summary>
        /// 已通过合伙人
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetPassPartner(PageInfo page, Dictionary<string, object> dic);
        /// <summary>
        /// 未通过合伙人页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetUnPassPartner(PageInfo page, Dictionary<string, object> dic);

        public abstract int AddPartner(Dictionary<string, object> dir);

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int DeletePartner(Dictionary<string, object> dic);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int UpDatePartnerPassword(EHECD_SystemUserDTO dto);

        /// <summary>
        /// 拒绝通过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int DelayPartner(Dictionary<string, object> dic);
        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int PassPartner(Dictionary<string, object> dic);
        /// <summary>
        /// 根据ID查看信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetPartner(string ID);
        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetUserByID(string ID);
        /// <summary>
        /// 根据ID获取身份证路径
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract IList<EHECD_ImagesDTO> SerachName(string ID);
        public abstract int DeleteUnPassPartner(Dictionary<string, object> dic);
        /// <summary>
        /// 合伙人详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetPartnerDetail(string ID);
    }
}
