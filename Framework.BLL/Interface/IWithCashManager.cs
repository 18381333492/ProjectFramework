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
    /// 提现 【抽象】
    /// author 王其
    /// </summary>
    public abstract class IWithCashManager : BaseBll
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<EHECD_WithdrawCashDTO> GetList(Dapper.PageInfo pageInfo,dynamic where);

        /// <summary>
        /// 提现(-1：更新数据库用户信息失败 -2：微信转账失败 1：操作成功)
        /// </summary>
        /// <param name="dto">当前提现用户</param>
        /// <returns></returns>
        public abstract int DoWithdrawCash(EHECD_WithdrawCashDTO dto,out string message);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract EHECD_WithdrawCashDTO GetSingle(Guid? id);

        /// <summary>
        /// 获取设置列表数据
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_WithdrawSettingDTO> GetSettingList();

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public abstract int DoSetting(List<EHECD_WithdrawSettingDTO> list);
    }
}
