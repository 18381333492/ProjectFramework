using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ReturnOrders")]
    public class EHECD_ReturnOrdersDTO
    {

        /// <summary>
        /// 维权订单ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 维权单号
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReturnNo",Required = true,DataLength = 32)]
        public String sReturnNo
        {
            get; set;
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
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
        /// 退货总数
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iTotalAmount",Required = true,DataLength = 1)]
        public Byte? iTotalAmount
        {
            get; set;
        }

        /// <summary>
        /// 实际退款金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iTotalMoney",Required = true,DataLength = 9)]
        public Decimal? iTotalMoney
        {
            get; set;
        }

        /// <summary>
        /// 申请退款原金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iOriginalTotaMoney",Required = true,DataLength = 9)]
        public Decimal? iOriginalTotaMoney
        {
            get; set;
        }

        /// <summary>
        /// 退货处理人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperatorUserID",Required = true,DataLength = 32)]
        public Guid? sOperatorUserID
        {
            get; set;
        }

        /// <summary>
        /// 退货处理人
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperatorUserName",Required = true,DataLength = 50)]
        public String sOperatorUserName
        {
            get; set;
        }

        /// <summary>
        /// 退货处理时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dOperatorTime",Required = true,DataLength = 8)]
        public DateTime? dOperatorTime
        {
            get; set;
        }

        /// <summary>
        /// 状态（0-接受申请，1 - 退款审核中，2 - 退款成功，3 - 拒绝退款）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 1)]
        public Byte? iState
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [FieldInfo(DataFieldLength = 500,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sDescribe",Required = true,DataLength = 250)]
        public String sDescribe
        {
            get; set;
        }

        /// <summary>
        /// 维权原因
        /// </summary>
        [FieldInfo(DataFieldLength = 500,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReason",Required = true,DataLength = 250)]
        public String sReason
        {
            get; set;
        }

        /// <summary>
        /// 物流公司名称（该项目暂未使用）
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLogisticsName",Required = true,DataLength = 50)]
        public String sLogisticsName
        {
            get; set;
        }

        /// <summary>
        /// 物流编号（该项目暂未使用）
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLogisticsNo",Required = true,DataLength = 50)]
        public String sLogisticsNo
        {
            get; set;
        }

        /// <summary>
        /// 数据来源(1-PC,2-网页，3-Android,4-IOS，该项目暂未使用)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "sSource",Required = true,DataLength = 1)]
        public Byte? sSource
        {
            get; set;
        }

        /// <summary>
        /// 申请维权时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dInsertTime",Required = true,DataLength = 8)]
        public DateTime? dInsertTime
        {
            get; set;
        }

        /// <summary>
        /// 维权完成时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dFinishTime",Required = true,DataLength = 8)]
        public DateTime? dFinishTime
        {
            get; set;
        }

        /// <summary>
        /// 删除标记（0-正常，1-删除)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 物流费用（该项目暂未使用）
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iLogisticsPrice",Required = true,DataLength = 9)]
        public Decimal? iLogisticsPrice
        {
            get; set;
        }
    }
}
