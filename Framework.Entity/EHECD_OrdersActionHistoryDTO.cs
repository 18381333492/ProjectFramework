using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_OrdersActionHistory")]
    public class EHECD_OrdersActionHistoryDTO
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
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 100)]
        public String sOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 订单类别 0客房 1门票 2周边产品
   
   
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iType",Required = true,DataLength = 4)]
        public Int32? iType
        {
            get; set;
        }

        /// <summary>
        /// 订单状态 0待付款 1待使用 2待核销
   
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 4)]
        public Int32? iState
        {
            get; set;
        }

        /// <summary>
        /// 操作类型(0-创建订单 1-付款 2-使用 3-核销)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iOperateType",Required = true,DataLength = 4)]
        public Int32? iOperateType
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
        /// 删除标志
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 用户操作时的备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperateDescribe",Required = true,DataLength = 0)]
        public String sOperateDescribe
        {
            get; set;
        }
    }
}
