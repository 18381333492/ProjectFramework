using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_OrdersPayMethods")]
    public class EHECD_OrdersPayMethodsDTO
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
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 100)]
        public String sOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 支付方式名称( 支付宝，微信支付，银联支付，余额支付，线下支付，包邮卡，免费拿样券，优惠券，积分 )
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPayName",Required = true,DataLength = 255)]
        public String sPayName
        {
            get; set;
        }

        /// <summary>
        /// 支付方式名称( 1.支付宝，2.微信支付，3.银联支付，4.余额支付，5.线下支付，6.包邮卡，7.免费拿样券，8.优惠券，9.积分 )
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sPayType",Required = true,DataLength = 4)]
        public Int32? sPayType
        {
            get; set;
        }

        /// <summary>
        /// 支付金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 18,DataFieldScale = 2,FiledName = "dAmount",Required = true,DataLength = 9)]
        public Decimal? dAmount
        {
            get; set;
        }

        /// <summary>
        /// 支付时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dPayTime",Required = true,DataLength = 8)]
        public DateTime? dPayTime
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 橙币数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iDiscountPrice",Required = false,DataLength = 4)]
        public Int32? iDiscountPrice
        {
            get; set;
        }
    }
}
