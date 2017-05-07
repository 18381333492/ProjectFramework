using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ReturnOrdersHistory")]
    public class EHECD_ReturnOrdersHistoryDTO
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
        /// 维权ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReturnID",Required = true,DataLength = 32)]
        public Guid? sReturnID
        {
            get; set;
        }

        /// <summary>
        /// 退货号
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReturnNo",Required = true,DataLength = 50)]
        public String sReturnNo
        {
            get; set;
        }

        /// <summary>
        /// 状态（0-接受申请，1 - 退款审核中，2 - 退款成功，3 - 拒绝退款）
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 4)]
        public Int32? iState
        {
            get; set;
        }

        /// <summary>
        /// 退款金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iTotalMoney",Required = true,DataLength = 9)]
        public Decimal? iTotalMoney
        {
            get; set;
        }

        /// <summary>
        /// 操作人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperatorID",Required = true,DataLength = 32)]
        public Guid? sOperatorID
        {
            get; set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dActionTime",Required = true,DataLength = 8)]
        public DateTime? dActionTime
        {
            get; set;
        }

        /// <summary>
        /// 操作描述
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sDescribe",Required = true,DataLength = 0)]
        public String sDescribe
        {
            get; set;
        }

        /// <summary>
        /// 用户操作时的备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperateDescribe",Required = false,DataLength = 0)]
        public String sOperateDescribe
        {
            get; set;
        }
    }
}
