using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ShopSet")]
    public class EHECD_ShopSetDTO
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
        /// 店铺id
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopID",Required = true,DataLength = 32)]
        public Guid? sShopID
        {
            get; set;
        }

        /// <summary>
        /// 店铺名
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sShopName",Required = true,DataLength = 50)]
        public String sShopName
        {
            get; set;
        }

        /// <summary>
        /// 民宿介绍
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sIntroduce",Required = true,DataLength = 100)]
        public String sIntroduce
        {
            get; set;
        }

        /// <summary>
        /// 是否删除
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDelete",Required = true,DataLength = 1)]
        public Boolean? bIsDelete
        {
            get; set;
        }

        /// <summary>
        /// 店主头像
        /// </summary>
        [FieldInfo(DataFieldLength = 800,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHeadPicture",Required = true,DataLength = 400)]
        public String sHeadPicture
        {
            get; set;
        }

        /// <summary>
        /// 店主姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHeadName",Required = true,DataLength = 20)]
        public String sHeadName
        {
            get; set;
        }

        /// <summary>
        /// 民宿签名
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sAutograph",Required = true,DataLength = 0)]
        public String sAutograph
        {
            get; set;
        }

        /// <summary>
        /// 店主故事
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHeadStory",Required = true,DataLength = 0)]
        public String sHeadStory
        {
            get; set;
        }

        /// <summary>
        /// 所在省
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sProvice",Required = true,DataLength = 20)]
        public String sProvice
        {
            get; set;
        }

        /// <summary>
        /// 所在市
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCity",Required = true,DataLength = 20)]
        public String sCity
        {
            get; set;
        }

        /// <summary>
        /// 所在地区
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCounty",Required = true,DataLength = 20)]
        public String sCounty
        {
            get; set;
        }

        /// <summary>
        /// 详细地址
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sAddress",Required = true,DataLength = 30)]
        public String sAddress
        {
            get; set;
        }

        /// <summary>
        /// 手机号码
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMobileNum",Required = true,DataLength = 25)]
        public String sMobileNum
        {
            get; set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLONG",Required = true,DataLength = 100)]
        public String sLONG
        {
            get; set;
        }

        /// <summary>
        /// 纬度
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLat",Required = true,DataLength = 100)]
        public String sLat
        {
            get; set;
        }

        /// <summary>
        /// 首字母
        /// </summary>
        [FieldInfo(DataFieldLength = 20,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sFristLetter",Required = true,DataLength = 10)]
        public String sFristLetter
        {
            get; set;
        }
    }
}
