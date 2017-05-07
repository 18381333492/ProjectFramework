using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_SysMessageDetail")]
    public class EHECD_SysMessageDetailDTO
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
        /// 接收者
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiverID",Required = true,DataLength = 32)]
        public Guid? sReceiverID
        {
            get; set;
        }

        /// <summary>
        /// 接收者名称
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiver",Required = true,DataLength = 50)]
        public String sReceiver
        {
            get; set;
        }

      
      

        /// <summary>
        /// 接收状态：0.未读 1.已读  2.已经签收 3.拒绝签收
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iRecStatus",Required = true,DataLength = 1)]
        public Byte? iRecStatus
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
        /// 拒收原因
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRejReason",Required = false,DataLength = 0)]
        public String sRejReason
        {
            get; set;
        }

        /// <summary>
        /// 邮件ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sMailID", Required = true, DataLength = 32)]
        public Guid? sMailID
        {
            get; set;
        }


        /// <summary>
        /// 昵称
        /// </summary>
        [FieldInfo(DataFieldLength = 60, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sNickName", Required = true, DataLength = 30)]
        public String sNickName
        {
            get; set;
        }

    }
}
