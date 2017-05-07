using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    /// <summary>
    /// 财务 【抽象】
    /// author 王其
    /// </summary>
    public abstract class IFinanceManager : BaseBll
    {
        /// <summary>
        /// 获取平台用户财务列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetFinanceBackgroundList(Dapper.PageInfo pageInfo, dynamic where,out decimal iServicePrice);

        /// <summary>
        /// 获取店铺用户财务列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetFinanceStoreList(Dapper.PageInfo pageInfo, dynamic where);

        /// <summary>
        /// 获取合伙人财务
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetFinancePartnerList(Dapper.PageInfo pageInfo, dynamic where);

        /// <summary>
        /// 获取【后台 财务管理】【店铺详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetBackgroundStoreDetails(Dapper.PageInfo pageInfo, dynamic where);


        /// <summary>
        ///  获取店铺的财务数据列表
        /// [汤台重写]
        /// </summary>
        /// <param name="pageInfo">分页查参数</param>
        /// <param name="where">查询条件</param>
        /// <param name="PartnerID">合伙人主键ID</param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetBackgroundStoreDetails(PageInfo pageInfo, dynamic where, Guid sShopID);


        /// <summary>
        ///  获取合伙人财务
        /// [汤台重写]
        /// </summary>
        /// <param name="pageInfo">分页查参数</param>
        /// <param name="where">查询条件</param>
        /// <param name="PartnerID">合伙人主键ID</param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetFinancePartnerList(PageInfo pageInfo, dynamic where, Guid PartnerID);


        /// <summary>
        /// 获取平台用户财务列表
        /// [汤台重写]
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetFinanceBackgroundListByTT(Dapper.PageInfo pageInfo, dynamic where, out decimal iServicePrice);


        /// <summary>
        /// 获取【后台 财务管理】【合伙人详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetBackgroundPartnerDetails(Dapper.PageInfo pageInfo, dynamic where);

        /// <summary>
        /// 获取【后台 财务管理】【分享客详情页列表】
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetBackgroundShareDetails(Dapper.PageInfo pageInfo, dynamic where);

        /// <summary>
        /// 申请提现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract int ApplyWithdraw(EHECD_WithdrawCashDTO dto,EHECD_SystemUserDTO user);

        /// <summary>
        /// 获取提现人的提现信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract EHECD_WithdrawCashDTO GetApplierInfo(Guid? id);

        /// <summary>
        /// 获取店铺可提现金额
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetStoreAviliableMoney(Guid? id);

        //********************************************************************************************//

        /// <summary>
        /// 获取店铺可提现金额
        /// [汤台重写]
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetStoreAviliableMoneyByTT(Guid? id);



        /// <summary>
        /// 获取合伙人的可提现金额
        /// [汤台重写]
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetPartnerAviliableMoneyByTT(Guid? id);


        /// <summary>
        /// 获取合伙人可提现金额和申请中的金额
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string,object> GetPartnerAviliableMoney(Guid? id);


        /// <summary>
        /// 获取提现设置，店铺/分享客/合伙人 的提现设置（间隔天数，最低提现额度）
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string, object> GetWithdrawCash(string roleName);
    }
}
