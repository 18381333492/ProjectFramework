using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ClientBalanceDetail")]
    public class EHECD_ClientBalanceDetailDTO
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
        /// 会员ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }

        /// <summary>
        /// 0 普通客户 1.分享客,2:店铺，3合伙人
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iClientType",Required = true,DataLength = 1)]
        public Byte? iClientType
        {
            get; set;
        }

        /// <summary>
        /// 会员名
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserName",Required = true,DataLength = 255)]
        public String sUserName
        {
            get; set;
        }

        /// <summary>
        /// 变动方式( 1 支出，2 收入 )
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iMethod",Required = true,DataLength = 1)]
        public Byte? iMethod
        {
            get; set;
        }

        /// <summary>
        /// 1提现，2充值，3退款，4购买 5收入佣金,6服务费,7订单收入,8.佣金支出9.服务费支出,10合伙人提成
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iType",Required = true,DataLength = 1)]
        public Byte? iType
        {
            get; set;
        }

        /// <summary>
        /// 变动时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dChangeTime",Required = true,DataLength = 8)]
        public DateTime? dChangeTime
        {
            get; set;
        }

        /// <summary>
        /// 变化前金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iBeforePrice",Required = true,DataLength = 9)]
        public Decimal? iBeforePrice
        {
            get; set;
        }

        /// <summary>
        /// 变动金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iPrice",Required = true,DataLength = 9)]
        public Decimal? iPrice
        {
            get; set;
        }

        /// <summary>
        /// 变化后金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iAfterPrice",Required = true,DataLength = 9)]
        public Decimal? iAfterPrice
        {
            get; set;
        }

        /// <summary>
        /// 删除标识
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [FieldInfo(DataFieldLength = 1000,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRemark",Required = true,DataLength = 500)]
        public String sRemark
        {
            get; set;
        }

        /// <summary>
        /// 支出收入关系订单ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderID",Required = true,DataLength = 32)]
        public Guid? sOrderID
        {
            get; set;
        }

        /// <summary>
        /// 支出收入关系订单编号
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 50)]
        public String sOrderNo
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
        /// 佣金费用
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iCommissionPrice",Required = true,DataLength = 9)]
        public Decimal? iCommissionPrice
        {
            get; set;
        }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "iShopID",Required = false,DataLength = 32)]
        public Guid? iShopID
        {
            get; set;
        }


        /// <summary>
        /// 合伙人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "PartnerID", Required = false, DataLength = 32)]
        public Guid? PartnerID
        {
            get; set;
        }
    }
}
