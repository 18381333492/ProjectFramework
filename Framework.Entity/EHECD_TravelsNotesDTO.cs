using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_TravelsNotes")]
    public class EHECD_TravelsNotesDTO
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
        /// 标题
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sTitle",Required = true,DataLength = 50)]
        public String sTitle
        {
            get; set;
        }

        /// <summary>
        /// 作者
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sAuthor",Required = true,DataLength = 25)]
        public String sAuthor
        {
            get; set;
        }

        /// <summary>
        /// 头像
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHeadImges",Required = true,DataLength = 255)]
        public String sHeadImges
        {
            get; set;
        }

        /// <summary>
        /// 内容
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sContent",Required = true,DataLength = 0)]
        public String sContent
        {
            get; set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dPublishTime",Required = true,DataLength = 8)]
        public DateTime? dPublishTime
        {
            get; set;
        }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreID",Required = true,DataLength = 32)]
        public Guid? sStoreID
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
        /// 排序
        /// </summary>
        [FieldInfo(DataFieldLength = 4, DataFieldPrecision = 10, DataFieldScale = 0, FiledName = "iOrder", Required = true, DataLength = 4)]
        public Int32? iOrder
        {
            get; set;
        }
    }
}
