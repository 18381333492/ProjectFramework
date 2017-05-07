using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Images")]
    public class EHECD_ImagesDTO
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
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sBelongID",Required = true,DataLength = 32)]
        public Guid? sBelongID
        {
            get; set;
        }

        /// <summary>
        /// 图片名称
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sImageName",Required = true,DataLength = 100)]
        public String sImageName
        {
            get; set;
        }

        /// <summary>
        /// 路径
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sImagePath",Required = true,DataLength = 0)]
        public String sImagePath
        {
            get; set;
        }

        /// <summary>
        /// 类型(0-游记图片，1-首页Banner,2-分享海报，3-首页轮播，4-评价,5-身份证)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 1)]
        public Byte? iState
        {
            get; set;
        }

        /// <summary>
        /// 图片链接地址
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLink",Required = true,DataLength = 0)]
        public String sLink
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
        /// 是否删除（0-否 1-是）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
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
        /// 是否显示(0不显示，1显示)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bDisplay",Required = true,DataLength = 1)]
        public Boolean? bDisplay
        {
            get; set;
        }

        /// <summary>
        /// 标题
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sTitle",Required = true,DataLength = 100)]
        public String sTitle
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
    }
}
