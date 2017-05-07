using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_GoodsTimePrice")]
    public class EHECD_GoodsTimePriceDTO
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
        /// 价格1的时间
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sFirstTime",Required = true,DataLength = 0)]
        public String sFirstTime
        {
            get; set;
        }

        /// <summary>
        /// 价格2的时间
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSecTime",Required = true,DataLength = 0)]
        public String sSecTime
        {
            get; set;
        }

        /// <summary>
        /// 价格3的时间
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sThirdTime",Required = true,DataLength = 0)]
        public String sThirdTime
        {
            get; set;
        }
    }
}
