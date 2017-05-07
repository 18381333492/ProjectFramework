using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_FullHouseTime")]
    public class EHECD_FullHouseTimeDTO
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 商品主键ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsId",Required = true,DataLength = 32)]
        public Guid? sGoodsId
        {
            get; set;
        }

        /// <summary>
        /// 满房开始时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dStartTime",Required = true,DataLength = 8)]
        public DateTime? dStartTime
        {
            get; set;
        }

        /// <summary>
        /// 满房结束时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dEndTime",Required = true,DataLength = 8)]
        public DateTime? dEndTime
        {
            get; set;
        }

        /// <summary>
        /// 满房数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iFullHouseCount",Required = true,DataLength = 4)]
        public Int32? iFullHouseCount
        {
            get; set;
        }
    }
}
