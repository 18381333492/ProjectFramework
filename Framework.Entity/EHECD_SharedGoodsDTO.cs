using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_SharedGoods")]
    public class EHECD_SharedGoodsDTO
    {

        /// <summary>
        /// ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 分享客ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsID",Required = true,DataLength = 32)]
        public Guid? sGoodsID
        {
            get; set;
        }

        /// <summary>
        /// 分享时的价格
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iPirce",Required = true,DataLength = 9)]
        public Decimal? iPirce
        {
            get; set;
        }

        /// <summary>
        /// 分享的时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dInsertTime",Required = true,DataLength = 8)]
        public DateTime? dInsertTime
        {
            get; set;
        }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopID",Required = true,DataLength = 32)]
        public Guid? sShopID
        {
            get; set;
        }
    }
}
