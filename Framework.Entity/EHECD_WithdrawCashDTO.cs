using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WithdrawCash")]
    public class EHECD_WithdrawCashDTO
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
        /// 提现编号(:TX0001)
        /// </summary>
        [FieldInfo(DataFieldLength = 64,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sWithdrawNumber",Required = true,DataLength = 32)]
        public String sWithdrawNumber
        {
            get; set;
        }

        /// <summary>
        /// 提现人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sWithdrawMemberID",Required = true,DataLength = 32)]
        public Guid? sWithdrawMemberID
        {
            get; set;
        }

        /// <summary>
        /// 提现人名称
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sWithdrawMemberName",Required = true,DataLength = 25)]
        public String sWithdrawMemberName
        {
            get; set;
        }

        /// <summary>
        /// 提现人类型(0-店铺 1-合伙人 2-分享客)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "sWithdrawMemberType",Required = true,DataLength = 1)]
        public Byte? sWithdrawMemberType
        {
            get; set;
        }

        /// <summary>
        /// 提现金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iWithdrawMoney",Required = true,DataLength = 9)]
        public Decimal? iWithdrawMoney
        {
            get; set;
        }

        /// <summary>
        /// 申请时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dApplyTime",Required = true,DataLength = 8)]
        public DateTime? dApplyTime
        {
            get; set;
        }

        /// <summary>
        /// 提现方式（0-线下 1-银联卡）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "sMethod",Required = true,DataLength = 1)]
        public Byte? sMethod
        {
            get; set;
        }

        /// <summary>
        /// 状态(0-提现审核中 1-通过审核 2-提现成功)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 1)]
        public Byte? iState
        {
            get; set;
        }

        /// <summary>
        /// 审核人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sChecker",Required = true,DataLength = 32)]
        public Guid? sChecker
        {
            get; set;
        }

        /// <summary>
        /// 审核人
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCheckerName",Required = true,DataLength = 25)]
        public String sCheckerName
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRemark",Required = true,DataLength = 0)]
        public String sRemark
        {
            get; set;
        }

        /// <summary>
        /// 数据来源(1-PC,2-微信，3-Android,4-IOS)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iSource",Required = true,DataLength = 1)]
        public Byte? iSource
        {
            get; set;
        }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBankCardNo",Required = true,DataLength = 30)]
        public String sBankCardNo
        {
            get; set;
        }

        /// <summary>
        /// 开户行
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBankName",Required = true,DataLength = 100)]
        public String sBankName
        {
            get; set;
        }

        /// <summary>
        /// 持卡人姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBankCardUserName",Required = true,DataLength = 25)]
        public String sBankCardUserName
        {
            get; set;
        }

        /// <summary>
        /// 是否删除（0-否 1-是）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
