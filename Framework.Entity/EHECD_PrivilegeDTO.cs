using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Privilege")]
    public class EHECD_PrivilegeDTO
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
        /// 配置该特权所属的对象
   
   
        /// </summary>
        [FieldInfo(DataFieldLength = 15,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPrivilegeMaster",Required = true,DataLength = 15)]
        public String sPrivilegeMaster
        {
            get; set;
        }

        /// <summary>
        /// 对应的特权所有者的唯一标识
 
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPrivilegeMasterValue",Required = true,DataLength = 32)]
        public Guid? sPrivilegeMasterValue
        {
            get; set;
        }

        /// <summary>
        /// 特权类型标识
   
        /// </summary>
        [FieldInfo(DataFieldLength = 15,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPrivilegeAccess",Required = true,DataLength = 15)]
        public String sPrivilegeAccess
        {
            get; set;
        }

        /// <summary>
        /// 对应的特权
   
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPrivilegeAccessValue",Required = true,DataLength = 32)]
        public Guid? sPrivilegeAccessValue
        {
            get; set;
        }

        /// <summary>
        /// 标识这个特权所有者的类型
  
        /// </summary>
        [FieldInfo(DataFieldLength = 15,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBelong",Required = true,DataLength = 15)]
        public String sBelong
        {
            get; set;
        }

        /// <summary>
        /// 标识这个特权是属于哪个的
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBelongValue",Required = true,DataLength = 32)]
        public Guid? sBelongValue
        {
            get; set;
        }

        /// <summary>
        /// 是否要禁用该特权 0为不禁用 1为禁用
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bPrivilegeOperation",Required = true,DataLength = 1)]
        public Boolean? bPrivilegeOperation
        {
            get; set;
        }

        /// <summary>
        /// 是否删除 0 未删除 1删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
