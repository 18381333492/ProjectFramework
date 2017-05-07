using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_OrdersGoods")]
    public class EHECD_OrdersGoodsDTO
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
        /// 订单ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderID",Required = true,DataLength = 32)]
        public Guid? sOrderID
        {
            get; set;
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 32)]
        public String sOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 商品ID（客房、门票、周边商品）
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsPrimaryKey",Required = true,DataLength = 32)]
        public Guid? sGoodsPrimaryKey
        {
            get; set;
        }

        /// <summary>
        /// 0 客房 1门票 2周边
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iGoodsType",Required = true,DataLength = 4)]
        public Int32? iGoodsType
        {
            get; set;
        }

        /// <summary>
        /// 商品编号
        /// </summary>
        [FieldInfo(DataFieldLength = 255,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNo",Required = true,DataLength = 255)]
        public String sGoodsNo
        {
            get; set;
        }

        /// <summary>
        /// 商品名称
   
   
   
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsName",Required = true,DataLength = 255)]
        public String sGoodsName
        {
            get; set;
        }

        /// <summary>
        /// 商品单价
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iSinglePrice",Required = true,DataLength = 9)]
        public Decimal? iSinglePrice
        {
            get; set;
        }

        /// <summary>
        /// 里面是  int数字，表示  百分比
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iDiscountSinglePrice",Required = true,DataLength = 4)]
        public Int32? iDiscountSinglePrice
        {
            get; set;
        }

        /// <summary>
        /// 商品数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iAmount",Required = true,DataLength = 4)]
        public Int32? iAmount
        {
            get; set;
        }

        /// <summary>
        /// 可退数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iReturnAmount",Required = true,DataLength = 4)]
        public Int32? iReturnAmount
        {
            get; set;
        }

        /// <summary>
        /// 商品规格(暂未使用)
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodStandID",Required = true,DataLength = 32)]
        public String sGoodStandID
        {
            get; set;
        }

        /// <summary>
        /// 商品颜色(暂未使用)
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sColorValue",Required = true,DataLength = 255)]
        public String sColorValue
        {
            get; set;
        }

        /// <summary>
        /// 商品尺寸(暂未使用)
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSizeName",Required = true,DataLength = 255)]
        public String sSizeName
        {
            get; set;
        }

        /// <summary>
        /// 是否被删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 服务费
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iServicePrice",Required = true,DataLength = 9)]
        public Decimal? iServicePrice
        {
            get; set;
        }

        /// <summary>
        /// 佣金
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iCommission",Required = true,DataLength = 9)]
        public Decimal? iCommission
        {
            get; set;
        }
    }
}
