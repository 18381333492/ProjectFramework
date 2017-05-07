using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WeChatSet")]
    public class EHECD_WeChatSetDTO
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
        /// 公众号名称
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sWeChatName",Required = true,DataLength = 50)]
        public String sWeChatName
        {
            get; set;
        }

        /// <summary>
        /// 微信号
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sWeChatNo",Required = true,DataLength = 100)]
        public String sWeChatNo
        {
            get; set;
        }

        /// <summary>
        /// 微信Token
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sToken",Required = true,DataLength = 100)]
        public String sToken
        {
            get; set;
        }

        /// <summary>
        /// 微信绑定url
        /// </summary>
        [FieldInfo(DataFieldLength = 400,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUrl",Required = true,DataLength = 200)]
        public String sUrl
        {
            get; set;
        }

        /// <summary>
        /// sAppId
        /// </summary>
        [FieldInfo(DataFieldLength = 80, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sAppId", Required = true, DataLength = 80)]
        public string sAppId
        {
            get; set;
        }

        /// <summary>
        /// sAppSecret
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sAppSecret",Required = true,DataLength = 100)]
        public String sAppSecret
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
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopID",Required = true,DataLength = 32)]
        public Guid? sShopID
        {
            get; set;
        }

        /// <summary>
        /// 原始ID
        /// </summary>
        [FieldInfo(DataFieldLength = 80,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOriginalID",Required = true,DataLength = 80)]
        public string sOriginalID
        {
            get; set;
        }

        /// <summary>
        /// API密钥
        /// </summary>
        [FieldInfo(DataFieldLength = 500,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPaySignKey",Required = true,DataLength = 250)]
        public String sPaySignKey
        {
            get; set;
        }
    }
}
