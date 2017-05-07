using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_CouponDetails")]
    public class EHECD_CouponDetailsDTO
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
        /// 优惠劵ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCouponID",Required = true,DataLength = 32)]
        public Guid? sCouponID
        {
            get; set;
        }

        /// <summary>
        /// 领取用户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserID",Required = true,DataLength = 32)]
        public Guid? sUserID
        {
            get; set;
        }

        /// <summary>
        /// 领取用户名称
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserName",Required = true,DataLength = 10)]
        public String sUserName
        {
            get; set;
        }

        /// <summary>
        /// 是否使用(0-否 1-是)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsUsed",Required = true,DataLength = 1)]
        public Boolean? bIsUsed
        {
            get; set;
        }

        /// <summary>
        /// 领取时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dGetTime",Required = true,DataLength = 8)]
        public DateTime? dGetTime
        {
            get; set;
        }

        /// <summary>
        /// 是否删除(0-否 1-是)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
