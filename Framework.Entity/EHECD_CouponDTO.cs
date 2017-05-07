using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Coupon")]
    public class EHECD_CouponDTO
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
        /// 优惠劵所属店铺（null代表属于平台)
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreID",Required = false,DataLength = 32)]
        public Guid? sStoreID
        {
            get; set;
        }

        /// <summary>
        /// 优惠劵金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iCoiCouponPrice",Required = true,DataLength = 9)]
        public Decimal? iCoiCouponPrice
        {
            get; set;
        }

        /// <summary>
        /// 使用优惠劵的条件金额,(无门槛0)
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iUsePrice",Required = true,DataLength = 9)]
        public Decimal? iUsePrice
        {
            get; set;
        }

        /// <summary>
        /// 投放数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iCouponCount",Required = true,DataLength = 4)]
        public Int32? iCouponCount
        {
            get; set;
        }

        /// <summary>
        /// 有效期-开始
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dValidDateStart",Required = true,DataLength = 8)]
        public DateTime? dValidDateStart
        {
            get; set;
        }

        /// <summary>
        /// 有效期-结束
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dValidDateEnd",Required = true,DataLength = 8)]
        public DateTime? dValidDateEnd
        {
            get; set;
        }

        /// <summary>
        /// 是否删除(0否1是)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 插入时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dInsertTime",Required = true,DataLength = 8)]
        public DateTime? dInsertTime
        {
            get; set;
        }

        /// <summary>
        /// 平台优惠券才会填
        /// </summary>
        [FieldInfo(DataFieldLength = 255,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCouponName",Required = false,DataLength = 255)]
        public String sCouponName
        {
            get; set;
        }
    }
}
