using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_SysMessage")]
    public class EHECD_SysMessageDTO
    {

        /// <summary>
        /// ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 发送者
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSender",Required = true,DataLength = 50)]
        public String sSender
        {
            get; set;
        }

        /// <summary>
        /// 消息标题
        /// </summary>
        [FieldInfo(DataFieldLength = 2000,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMsgTitle",Required = true,DataLength = 1000)]
        public String sMsgTitle
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
        /// 消息正文
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMsgContent",Required = true,DataLength = 0)]
        public String sMsgContent
        {
            get; set;
        }

        /// <summary>
        /// 接收者类型(0表示所有 1表示指定用户)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iRecevierType",Required = true,DataLength = 1)]
        public Byte? iRecevierType
        {
            get; set;
        }
    }
}
