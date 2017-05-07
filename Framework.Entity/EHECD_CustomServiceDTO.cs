using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_CustomService")]
    public class EHECD_CustomServiceDTO
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
        /// 店铺ID(为null则表示是平台的)
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreID",Required = false,DataLength = 32)]
        public Guid? sStoreID
        {
            get; set;
        }

        /// <summary>
        /// 客服QQ号
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sQQ",Required = true,DataLength = 50)]
        public String sQQ
        {
            get; set;
        }

        /// <summary>
        /// 客服QQ昵称
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sQQName",Required = true,DataLength = 50)]
        public String sQQName
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

        /// <summary>
        /// 是否删除(0-否 1-是)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
