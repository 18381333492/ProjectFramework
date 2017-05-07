using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Role")]
    public class EHECD_RoleDTO
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 角色名称
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRoleName",Required = true,DataLength = 20)]
        public String sRoleName
        {
            get; set;
        }

        /// <summary>
        /// 是否可用
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bEnable",Required = true,DataLength = 1)]
        public Boolean? bEnable
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dCreateTime",Required = true,DataLength = 8)]
        public DateTime? dCreateTime
        {
            get; set;
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dModifyTime",Required = true,DataLength = 8)]
        public DateTime? dModifyTime
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 排序编号
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iOrder",Required = true,DataLength = 4)]
        public Int32? iOrder
        {
            get; set;
        }
    }
}
