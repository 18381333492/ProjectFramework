using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Operat")]
    public class EHECD_OperatDTO
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
        /// 主键
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sOperatorID", Required = true, DataLength = 32)]
        public Guid? sOperatorID
        {
            get; set;
        }

        /// <summary>
        /// 类型(0-商品上架 1-商品下架 2-冻结用户)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 1)]
        public Byte? iState
        {
            get; set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dOperatTime",Required = true,DataLength = 8)]
        public DateTime? dOperatTime
        {
            get; set;
        }

        /// <summary>
        /// 操作人
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOperator",Required = true,DataLength = 50)]
        public String sOperator
        {
            get; set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sContent",Required = true,DataLength = 255)]
        public String sContent
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

        /// <summary>
        /// 商品上下级用的字段
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sGoodsId", Required = false, DataLength = 32)]
        public Guid? sGoodsId
        {
            get; set;
        }
    }
}
