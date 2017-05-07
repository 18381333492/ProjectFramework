using Framework.Dapper;
using Framework.DTO;

namespace Framework.BLL
{
    public abstract class ISystemUserManager:BaseBll
    {       
        /// <summary>
        /// 分页载入系统用户
        /// </summary>
        /// <param name="page">分页对象</param>
        /// <param name="where">查询条件</param>
        /// <returns>分页结果</returns>
        public abstract PagingRet<EHECD_SystemUserDTO> LoadSystemUsers(PageInfo page,dynamic where);

        /// <summary>
        /// 冻结系统用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <param name="p">用来生成系统日志的动态类型</param>
        /// <returns>冻结结果</returns>
        public abstract int FrozenSystemUser(EHECD_SystemUserDTO user, dynamic p);

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <param name="p">用来生成系统日志的动态类型</param>
        /// <returns>删除结果</returns>
        public abstract int DeleteSystemUser(EHECD_SystemUserDTO user, dynamic p);

        /// <summary>
        /// 添加系统用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <param name="p">用来生成系统日志的动态类型</param>
        /// <returns>添加结果</returns>
        public abstract int AddSystemUser(EHECD_SystemUserDTO user,dynamic p);

        /// <summary>
        /// 编辑系统用户
        /// </summary>
        /// <param name="user">系统用户</param>
        /// <param name="p">用来生成系统日志的动态类型</param>
        /// <returns>编辑结果</returns>
        public abstract int EditSystemUser(EHECD_SystemUserDTO user, dynamic p);

        /// <summary>
        /// 根据ID获取用户完整信息
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>完整用户信息</returns>
        public abstract EHECD_SystemUserDTO GetSystemUserInfoById(EHECD_SystemUserDTO user);

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="user"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public abstract int DistributionRole(DTO.EHECD_SystemUserDTO user,dynamic p);
    }
}
