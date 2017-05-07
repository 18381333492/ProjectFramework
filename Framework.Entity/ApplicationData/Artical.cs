using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    
    public class Article
    {

        /// <summary>
        /// 主键
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "ID", Required = true, DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 文章标题
        /// </summary>
        [FieldInfo(DataFieldLength = 400, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sTitle", Required = true, DataLength = 200)]
        public String sTitle
        {
            get; set;
        }

        /// <summary>
        /// 发布时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8, DataFieldPrecision = 23, DataFieldScale = 3, FiledName = "dPublishTime", Required = true, DataLength = 8)]
        public DateTime? dPublishTime
        {
            get; set;
        }

        /// <summary>
        /// 文章内容
        /// </summary>
        [FieldInfo(DataFieldLength = -1, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sContent", Required = true, DataLength = 0)]
        public String sContent
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1, DataFieldPrecision = 1, DataFieldScale = 0, FiledName = "bIsDeleted", Required = true, DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }
    }
}
