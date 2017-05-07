using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DTO
{
    [Serializable]
    public class UserRole
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [FieldInfo(DataFieldLength = 16, FiledName = "ID", Required = true)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [FieldInfo(DataFieldLength = 20, FiledName = "sRoleName", Required = true)]
        public String sRoleName
        {
            get; set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8, FiledName = "dModifyTime", Required = true)]
        public DateTime? dModifyTime
        {
            get; set;
        }

        /// <summary>
        /// 排序编号
        /// </summary>
        [FieldInfo(DataFieldLength = 4, FiledName = "iOrder", Required = true)]
        public Int32? iOrder
        {
            get; set;
        }        
    }
}
