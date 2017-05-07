using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_RoomDetail")]
    public class EHECD_RoomDetailDTO
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
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsId",Required = true,DataLength = 32)]
        public Guid? sGoodsId
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreId",Required = true,DataLength = 32)]
        public Guid? sStoreId
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dStartTime",Required = true,DataLength = 8)]
        public DateTime? dStartTime
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "sEndTime",Required = true,DataLength = 8)]
        public DateTime? sEndTime
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderId",Required = true,DataLength = 32)]
        public Guid? sOrderId
        {
            get; set;
        }

        /// <summary>
        /// 订单商品数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4, DataFieldPrecision = 10, DataFieldScale = 0, FiledName = "iAmount", Required = true, DataLength = 4)]
        public Int32? iAmount
        {
            get; set;
        }
    }
}
