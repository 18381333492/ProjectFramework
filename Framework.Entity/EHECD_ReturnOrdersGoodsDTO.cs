using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ReturnOrdersGoods")]
    public class EHECD_ReturnOrdersGoodsDTO
    {

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 客户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }

        /// <summary>
        /// 维权ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReturnID",Required = true,DataLength = 32)]
        public Guid? sReturnID
        {
            get; set;
        }

        /// <summary>
        /// 维权号
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReturnNo",Required = true,DataLength = 50)]
        public String sReturnNo
        {
            get; set;
        }

        /// <summary>
        /// 维权商品ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsPrimaryKey",Required = true,DataLength = 32)]
        public Guid? sGoodsPrimaryKey
        {
            get; set;
        }

        /// <summary>
        /// 维权商品名称
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsName",Required = true,DataLength = 255)]
        public String sGoodsName
        {
            get; set;
        }

        /// <summary>
        /// 维权商品货号（该项目暂时未用）
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNo",Required = true,DataLength = 32)]
        public String sGoodsNo
        {
            get; set;
        }

        /// <summary>
        /// 维权商品订单所属子类（该项目暂时未用）
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBelongSubclass",Required = true,DataLength = 32)]
        public String sBelongSubclass
        {
            get; set;
        }

        /// <summary>
        /// 维权商品数量
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iAmount",Required = true,DataLength = 1)]
        public Byte? iAmount
        {
            get; set;
        }

        /// <summary>
        /// 维权商品单价
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iSinglePrice",Required = true,DataLength = 9)]
        public Decimal? iSinglePrice
        {
            get; set;
        }

        /// <summary>
        /// 商品规格ID(暂时不用)
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStandardID",Required = true,DataLength = 32)]
        public String sStandardID
        {
            get; set;
        }

        /// <summary>
        /// 商品颜色(暂时不用)
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sColorValue",Required = true,DataLength = 255)]
        public String sColorValue
        {
            get; set;
        }

        /// <summary>
        /// 商品颜色图标(暂时不用)
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sColorIcon",Required = true,DataLength = 255)]
        public String sColorIcon
        {
            get; set;
        }

        /// <summary>
        /// 商品尺寸(暂时不用)
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSizeName",Required = true,DataLength = 255)]
        public String sSizeName
        {
            get; set;
        }

        /// <summary>
        /// 总价
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dTotalPrice",Required = true,DataLength = 9)]
        public Decimal? dTotalPrice
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
