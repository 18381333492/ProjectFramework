using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Scanner")]
    public class EHECD_ScannerDTO
    {

        /// <summary>
        /// 主键
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
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
        /// 真实姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReallyName",Required = true,DataLength = 20)]
        public String sReallyName
        {
            get; set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPhone",Required = true,DataLength = 25)]
        public String sPhone
        {
            get; set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPassword",Required = true,DataLength = 50)]
        public String sPassword
        {
            get; set;
        }

        /// <summary>
        /// 所属店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopID",Required = true,DataLength = 32)]
        public Guid? sShopID
        {
            get; set;
        }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldInfo(DataFieldLength = 1, DataFieldPrecision = 1, DataFieldScale = 0, FiledName = "bIsDeleted", Required = true, DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
