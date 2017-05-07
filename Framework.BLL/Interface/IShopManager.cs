using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IShopManager:BaseBll
    {
        /// <summary>
        /// 店铺短信页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetPageList(PageInfo page, Dictionary<String, object> dic);
        /// <summary>
        /// 已通过页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetPassList(PageInfo page, Dictionary<string, object> dic);
        /// <summary>
        /// 待审核页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> CheckPageList(PageInfo page,Dictionary<string,object> dic);
        /// <summary>
        /// 获取店铺短信条数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetMessageCount(string ID);
        /// <summary>
        /// 修改短信票数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract int UpdateCount(EHECD_SystemUserDTO dto);
        /// <summary>
        /// 添加商铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int AddShop(Dictionary<string, object> dir);
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract int PassCheck(Dictionary<string, object> dic);
        /// <summary>
        /// 拒绝通过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int DelayCheck(Dictionary<string, object> dic);
        /// <summary>
        /// 删除店铺的请求
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int DeleteCheck(Dictionary<string, object> dic);
        /// <summary>
        /// 冻结店铺
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
       
        public abstract int FreezeCheck(Dictionary<string, object> obj);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int UpDatePassword(EHECD_SystemUserDTO dto);
        /// <summary>
        /// 根据ID查看信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetUser(string ID);
        /// <summary>
        /// 删除未审核的店铺
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int DeleteUnCheck(Dictionary<string, object> dic);
        /// <summary>
        /// 根据申请人的ID查看申请人的详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ApplyDTO ApplyUser(string ID);
        /// <summary>
        /// 获取所有的合伙人的姓名，ID
        /// </summary>
        /// <returns></returns>
        public abstract string GetAllPartner();
        /// <summary>
        /// 修改商家的合伙人
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public abstract int UdatePartner(EHECD_SystemUserDTO sys);
        /// <summary>
        /// 获取平台的短信数量
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO GetTerraceCount(EHECD_SystemUserDTO sys);
        /// <summary>
        /// 修改平台的短信数
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public abstract int UpdateTerraceCount(EHECD_SystemUserDTO sys,EHECD_SystemUserDTO dto);
        /// <summary>
        /// 合伙人店铺页面数据绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetPartnerShop(PageInfo page, Dictionary<string, object> dic,EHECD_SystemUserDTO user);
        /// <summary>
        /// 判断是否有相同的手机号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO TheSeamPhone(EHECD_SystemUserDTO user);
        /// <summary>
        /// 判断是否有相同的登录名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract EHECD_SystemUserDTO TheSeamLoginName(EHECD_SystemUserDTO user);
    }
}
