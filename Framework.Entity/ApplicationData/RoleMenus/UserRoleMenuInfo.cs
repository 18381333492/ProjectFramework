using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DTO
{
    /// <summary>
    /// 用户权限信息
    /// </summary>
    [Serializable]
    public class UserRoleMenuInfo
    {
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <remarks>
        /// 请注意，只有当系统配置了启用角色功能的时候，才会初始化该属性
        /// </remarks>
        public IList<UserRole> UserRole { get; set; } = new List<UserRole>();

        /// <summary>
        /// 用户菜单
        /// </summary>        
        public IList<UserMenu> UserMenu { get; set; } = new List<UserMenu>();

        /// <summary>
        /// 用户所有的菜单（没有层级关系）
        /// </summary>
        public IList<UserMenu> AllMenu { get; set; } = new List<UserMenu>();

        /// <summary>
        /// 载入结果
        /// </summary>
        public bool LoadSuccess { get; set; } = false;
    }
}
