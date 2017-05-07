using Framework.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;

namespace Framework.BLL
{
    public abstract class IRoleManager : BaseBll
    {
        /// <summary>
        /// 获取角色列表（分页）
        /// </summary>
        /// <param name="where">分页条件</param>
        /// <returns>分页结果对象</returns>
        public abstract PagingRet<EHECD_RoleDTO> LoadRoles(dynamic where);

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="data">创建角色要用到的参数信息</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>添加结果</returns>
        public abstract int AddRole(dynamic data,dynamic p);
        
        /// <summary>
        /// 编辑角色信息
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>编辑结果</returns>
        public abstract int EditRole(EHECD_RoleDTO role, dynamic p);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ID">角色ID</param>
        /// <param name="p">插入系统操作日志需要的动态参数</param>
        /// <returns>删除结果</returns>
        public abstract int DeleteRole(string ID, dynamic p);
        
        /// <summary>
        /// 载入所有角色
        /// </summary>
        /// <returns>角色集合</returns>
        public abstract IList<EHECD_RoleDTO> LoadAllRoles();
        
        /// <summary>
        /// 载入用户的角色
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns>用户的角色</returns>
        public abstract IList<EHECD_RoleDTO> LoadUserRole(EHECD_SystemUserDTO user);
        
        /// <summary>
        /// 载入分配角色的菜单tree数据
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>tree数据</returns>
        public abstract dynamic LoadDistributionMenu(EHECD_RoleDTO role);

        /// <summary>
        /// 载入分配角色的菜单按钮tree数据
        /// </summary>
        /// <param name="role">角色</param>
        /// <returns>tree数据</returns>
        public abstract dynamic LoadDistributionMenuButton(EHECD_RoleDTO role);

        /// <summary>
        /// 分配角色菜单
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="dynamicData">数据</param>
        /// <returns>分配结果</returns>
        public abstract int DistributionMenu(EHECD_RoleDTO role, dynamic dynamicData);

        /// <summary>
        /// 分配角色菜单按钮
        /// </summary>
        /// <param name="role">角色</param>
        /// <param name="dynamicData">数据</param>
        /// <returns>分配结果</returns>
        public abstract int DistributionMenuButton(EHECD_RoleDTO role, dynamic dynamicData);
    }
}
