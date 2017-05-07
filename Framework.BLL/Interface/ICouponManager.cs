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
    /// 优惠劵 【抽象】
    /// author 王其
    /// </summary>
    public abstract class ICouponManager:BaseBll
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string, object>> GetList(Dapper.PageInfo pageInfo,Guid? guid,dynamic where);

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public abstract int DoAdd(EHECD_CouponDTO dto);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public abstract int DoEdit(EHECD_CouponDTO dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public abstract int DoDelete(string ids);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract EHECD_CouponDTO GetSingle(Guid? id);
    }
}
