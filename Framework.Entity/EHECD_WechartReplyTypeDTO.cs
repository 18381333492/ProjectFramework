using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WechartReplyType")]
    public class EHECD_WechartReplyTypeDTO
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
        /// 状态(回复功能开启1或关闭0)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "sState",Required = true,DataLength = 1)]
        public Boolean? sState
        {
            get; set;
        }

        /// <summary>
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopID",Required = true,DataLength = 32)]
        public Guid? sShopID
        {
            get; set;
        }

        /// <summary>
        /// 回复类型(纯文本0、图文混合1)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "sContentType",Required = true,DataLength = 1)]
        public Boolean? sContentType
        {
            get; set;
        }

        /// <summary>
        /// 原始id
        /// </summary>
        [FieldInfo(DataFieldLength = 80,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOriginalID",Required = true,DataLength = 80)]
        public String sOriginalID
        {
            get; set;
        }

        /// <summary>
        /// 回复类型(关注回复0、关键字1、自动回复2)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sReplyType",Required = true,DataLength = 4)]
        public Int32? sReplyType
        {
            get; set;
        }
    }
}
