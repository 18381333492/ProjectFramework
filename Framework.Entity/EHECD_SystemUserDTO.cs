using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_SystemUser")]
    public class EHECD_SystemUserDTO
    {

        /// <summary>
        /// 唯一标识
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "ID",Required = true,DataLength = 32)]
        public Guid? ID
        {
            get; set;
        }

        /// <summary>
        /// 登录名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLoginName",Required = true,DataLength = 20)]
        public String sLoginName
        {
            get; set;
        }

        /// <summary>
        /// 登录密码
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPassWord",Required = true,DataLength = 50)]
        public String sPassWord
        {
            get; set;
        }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 30,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserName",Required = true,DataLength = 15)]
        public String sUserName
        {
            get; set;
        }

        /// <summary>
        /// 用户状态 0：正常 1：冻结
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "tUserState",Required = true,DataLength = 1)]
        public Byte? tUserState
        {
            get; set;
        }

        /// <summary>
        /// 用户类型 0：平台用户，1：店铺，2：合伙人
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "tUserType",Required = true,DataLength = 1)]
        public Byte? tUserType
        {
            get; set;
        }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sUserNickName",Required = true,DataLength = 20)]
        public String sUserNickName
        {
            get; set;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dCreateTime",Required = true,DataLength = 8)]
        public DateTime? dCreateTime
        {
            get; set;
        }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dLastLoginTime",Required = true,DataLength = 8)]
        public DateTime? dLastLoginTime
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
        /// 性别 0:女 1:男
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "tSex",Required = true,DataLength = 1)]
        public Byte? tSex
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
        /// 手机号码
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMobileNum",Required = true,DataLength = 25)]
        public String sMobileNum
        {
            get; set;
        }

        /// <summary>
        /// 账户余额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 18,DataFieldScale = 2,FiledName = "iAccountNalance",Required = true,DataLength = 9)]
        public Decimal? iAccountNalance
        {
            get; set;
        }

        /// <summary>
        /// 剩余短信
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sMessage",Required = true,DataLength = 4)]
        public Int32? sMessage
        {
            get; set;
        }

        /// <summary>
        /// 主营业务/合伙人的个人描述
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMainBusiness",Required = true,DataLength = 0)]
        public String sMainBusiness
        {
            get; set;
        }

        /// <summary>
        /// 所属合伙人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPartnerID",Required = false,DataLength = 32)]
        public Guid? sPartnerID
        {
            get; set;
        }

        /// <summary>
        /// 所属合伙人姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPartnerName",Required = false,DataLength = 20)]
        public String sPartnerName
        {
            get; set;
        }
    }
}
