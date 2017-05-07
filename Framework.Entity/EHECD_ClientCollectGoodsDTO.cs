using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ClientCollectGoods")]
    public class EHECD_ClientCollectGoodsDTO
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
        /// 用户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public String sClientID
        {
            get; set;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsPrimaryKey",Required = true,DataLength = 32)]
        public String sGoodsPrimaryKey
        {
            get; set;
        }

        /// <summary>
        /// 商品类别：1.客房 2.门票 3.周边产品
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iGoodsType",Required = true,DataLength = 1)]
        public Byte? iGoodsType
        {
            get; set;
        }

        /// <summary>
        /// 商品货号（暂未使用）
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNo",Required = true,DataLength = 100)]
        public String sGoodsNo
        {
            get; set;
        }

        /// <summary>
        /// 商品收藏时价格
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iCollectPrice",Required = true,DataLength = 9)]
        public Decimal? iCollectPrice
        {
            get; set;
        }

        /// <summary>
        /// 收藏时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dCollectTime",Required = true,DataLength = 8)]
        public DateTime? dCollectTime
        {
            get; set;
        }

        /// <summary>
        /// 数据来源（1-PC，2-微信，3-安卓，4-IOS）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iSource",Required = true,DataLength = 1)]
        public Byte? iSource
        {
            get; set;
        }

        /// <summary>
        /// 删除标记
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
