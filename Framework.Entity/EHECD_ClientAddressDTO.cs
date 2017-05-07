using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_ClientAddress")]
    public class EHECD_ClientAddressDTO
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
        /// 客户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }

        /// <summary>
        /// 联系人
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserName",Required = true,DataLength = 100)]
        public String sUserName
        {
            get; set;
        }

        /// <summary>
        /// 联系电话
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPhone",Required = true,DataLength = 30)]
        public String sPhone
        {
            get; set;
        }

        /// <summary>
        /// 省
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sProvince",Required = true,DataLength = 20)]
        public String sProvince
        {
            get; set;
        }

        /// <summary>
        /// 市
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCity",Required = true,DataLength = 20)]
        public String sCity
        {
            get; set;
        }

        /// <summary>
        /// 区县
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCounty",Required = true,DataLength = 20)]
        public String sCounty
        {
            get; set;
        }

        /// <summary>
        /// 具体地址
        /// </summary>
        [FieldInfo(DataFieldLength = 300,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sAddress",Required = true,DataLength = 150)]
        public String sAddress
        {
            get; set;
        }

        /// <summary>
        /// 是否三环内
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsInThreeRound",Required = true,DataLength = 1)]
        public Boolean? bIsInThreeRound
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 是否是默认地址
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDefault",Required = true,DataLength = 1)]
        public Boolean? bIsDefault
        {
            get; set;
        }

        /// <summary>
        /// 是否送达乡镇
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsToVillage",Required = true,DataLength = 1)]
        public Boolean? bIsToVillage
        {
            get; set;
        }

        /// <summary>
        /// 维度
        /// </summary>
        [FieldInfo(DataFieldLength = 5,DataFieldPrecision = 9,DataFieldScale = 6,FiledName = "dLatitude",Required = true,DataLength = 5)]
        public Decimal? dLatitude
        {
            get; set;
        }

        /// <summary>
        /// 经度
        /// </summary>
        [FieldInfo(DataFieldLength = 5,DataFieldPrecision = 9,DataFieldScale = 6,FiledName = "dLongtidude",Required = true,DataLength = 5)]
        public Decimal? dLongtidude
        {
            get; set;
        }
    }
}
