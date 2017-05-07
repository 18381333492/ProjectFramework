using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Collect")]
    public class EHECD_CollectDTO
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "iGoodsID",Required = true,DataLength = 32)]
        public Guid? iGoodsID
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
        /// 用户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "iClientID",Required = true,DataLength = 32)]
        public Guid? iClientID
        {
            get; set;
        }

        /// <summary>
        /// 是否收藏
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsCollect",Required = true,DataLength = 1)]
        public Boolean? bIsCollect
        {
            get; set;
        }

        /// <summary>
        /// 收藏类型(0-店铺，1-客房，2-票务，3-周边)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iCollectType",Required = true,DataLength = 4)]
        public Int32? iCollectType
        {
            get; set;
        }
    }
}
