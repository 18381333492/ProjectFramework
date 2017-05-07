using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WeCharReply")]
    public class EHECD_WeCharReplyDTO
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
        /// 回复类型(关注回复0、关键字1、自动回复2)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sReplyType",Required = true,DataLength = 4)]
        public Int32? sReplyType
        {
            get; set;
        }

        /// <summary>
        /// 回复内容类型(纯文本0、图文混合1)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sContentType",Required = true,DataLength = 4)]
        public Int32? sContentType
        {
            get; set;
        }

        /// <summary>
        /// 关键字
        /// </summary>
        [FieldInfo(DataFieldLength = 600,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sKeyword",Required = true,DataLength = 300)]
        public String sKeyword
        {
            get; set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [FieldInfo(DataFieldLength = 600,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sTitle",Required = true,DataLength = 300)]
        public String sTitle
        {
            get; set;
        }

        /// <summary>
        /// 商城链接
        /// </summary>
        [FieldInfo(DataFieldLength = 1000,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopUrl",Required = true,DataLength = 500)]
        public String sShopUrl
        {
            get; set;
        }

        /// <summary>
        /// 内容/备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sContent",Required = true,DataLength = 0)]
        public String sContent
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
        /// 是否删除(否0，是1)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
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
        /// 图片外链
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPictureUrl",Required = true,DataLength = 0)]
        public String sPictureUrl
        {
            get; set;
        }
    }
}
