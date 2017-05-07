using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_SystemLog")]
    public class EHECD_SystemLogDTO
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
        /// 操作内容
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sDomainDetail",Required = true,DataLength = 100)]
        public String sDomainDetail
        {
            get; set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLoginName",Required = true,DataLength = 20)]
        public String sLoginName
        {
            get; set;
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 30,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserName",Required = true,DataLength = 15)]
        public String sUserName
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dInsertTime",Required = true,DataLength = 8)]
        public DateTime? dInsertTime
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
        /// 登录IP地址
        /// </summary>
        [FieldInfo(DataFieldLength = 25,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sIPAddress",Required = true,DataLength = 25)]
        public String sIPAddress
        {
            get; set;
        }

        /// <summary>
        /// 操作的ID
        /// </summary>
        [FieldInfo(DataFieldLength = 255,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sDoMainId",Required = true,DataLength = 255)]
        public String sDoMainId
        {
            get; set;
        }

        /// <summary>
        /// 操作类型 short
        /// </summary>
        [FieldInfo(DataFieldLength = 2,DataFieldPrecision = 5,DataFieldScale = 0,FiledName = "tDoType",Required = true,DataLength = 2)]
        public Int16? tDoType
        {
            get; set;
        }
    }
}
