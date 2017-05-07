using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WithdrawSetting")]
    public class EHECD_WithdrawSettingDTO
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
        /// 角色名称(分享客.商   户,合伙人)
        /// </summary>
        [FieldInfo(DataFieldLength = 20,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRoleName",Required = true,DataLength = 10)]
        public String sRoleName
        {
            get; set;
        }

        /// <summary>
        /// 提现间隔天数
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iIntervalDays",Required = true,DataLength = 4)]
        public Int32? iIntervalDays
        {
            get; set;
        }

        /// <summary>
        /// 最低提现金额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "iMinimumMoney",Required = true,DataLength = 9)]
        public Decimal? iMinimumMoney
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
