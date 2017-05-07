using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Categories")]
    public class EHECD_CategoriesDTO
    {

        /// <summary>
        /// 上级ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "PID",Required = false,DataLength = 32)]
        public Guid? PID
        {
            get; set;
        }

        /// <summary>
        /// 商品种类名
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCategoryName",Required = true,DataLength = 255)]
        public String sCategoryName
        {
            get; set;
        }

        /// <summary>
        /// 商品种类简述
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCategoryCaption",Required = true,DataLength = 255)]
        public String sCategoryCaption
        {
            get; set;
        }

        /// <summary>
        /// 排序
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iOrder",Required = true,DataLength = 4)]
        public Int32? iOrder
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dInsertTime",Required = true,DataLength = 8)]
        public DateTime? dInsertTime
        {
            get; set;
        }

        /// <summary>
        /// 分类图片
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sImgUri",Required = true,DataLength = -1)]
        public String sImgUri
        {
            get; set;
        }

        /// <summary>
        /// ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }
    }
}
