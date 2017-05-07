using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_BaseSetting")]
    public class EHECD_BaseSettingDTO
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
        /// 平台电话
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMallPhone",Required = true,DataLength = 20)]
        public String sMallPhone
        {
            get; set;
        }

        /// <summary>
        /// 时限内未付款自动取消订单
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iHours",Required = true,DataLength = 4)]
        public Int32? iHours
        {
            get; set;
        }

        /// <summary>
        /// 佣金最高百分比限制
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iHighestCommissionPrecent",Required = true,DataLength = 4)]
        public Int32? iHighestCommissionPrecent
        {
            get; set;
        }

        /// <summary>
        /// 佣金比例:1级
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iLevelOneCommissionPrecent",Required = true,DataLength = 4)]
        public Int32? iLevelOneCommissionPrecent
        {
            get; set;
        }

        /// <summary>
        /// 佣金比例:2级
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iLevelTwoCommissionPrecent",Required = true,DataLength = 4)]
        public Int32? iLevelTwoCommissionPrecent
        {
            get; set;
        }

        /// <summary>
        /// 佣金比例:3级
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iLevelThreeCommissionPrecent",Required = true,DataLength = 4)]
        public Int32? iLevelThreeCommissionPrecent
        {
            get; set;
        }

        /// <summary>
        /// 服务费比例
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iServicePrecent",Required = true,DataLength = 4)]
        public Int32? iServicePrecent
        {
            get; set;
        }

        /// <summary>
        /// 合伙人提成比例
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iPartnerCommissionPrecent",Required = true,DataLength = 4)]
        public Int32? iPartnerCommissionPrecent
        {
            get; set;
        }

        /// <summary>
        /// 返还优惠劵金额
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iReturnMoney",Required = true,DataLength = 4)]
        public Int32? iReturnMoney
        {
            get; set;
        }

        /// <summary>
        /// 使用优惠劵需要达到的条件金额（无门槛为0）
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iUserMoney",Required = true,DataLength = 9)]
        public Decimal? iUserMoney
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
