using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Client")]
    public class EHECD_ClientDTO
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
        /// 客户姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sName",Required = true,DataLength = 255)]
        public String sName
        {
            get; set;
        }

        /// <summary>
        /// 电话
        /// </summary>
        [FieldInfo(DataFieldLength = 22,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPhone",Required = true,DataLength = 11)]
        public String sPhone
        {
            get; set;
        }

        /// <summary>
        /// 密码
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPassWord",Required = true,DataLength = 50)]
        public String sPassWord
        {
            get; set;
        }

        /// <summary>
        /// 头像地址
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHeadPic",Required = true,DataLength = 255)]
        public String sHeadPic
        {
            get; set;
        }

        /// <summary>
        /// 1-正常   0-冻结
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 4)]
        public Int32? iState
        {
            get; set;
        }

        /// <summary>
        /// 删除标志
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 添加时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dAddTime",Required = true,DataLength = 8)]
        public DateTime? dAddTime
        {
            get; set;
        }

        /// <summary>
        /// 账户余额
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 18,DataFieldScale = 2,FiledName = "dAccountBalance",Required = true,DataLength = 9)]
        public Decimal? dAccountBalance
        {
            get; set;
        }

        /// <summary>
        /// 0-普通客户,1-分销客
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iClientType",Required = true,DataLength = 4)]
        public Int32? iClientType
        {
            get; set;
        }

        /// <summary>
        /// 支付密码
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPayPassWord",Required = true,DataLength = 50)]
        public String sPayPassWord
        {
            get; set;
        }

        /// <summary>
        /// 支付密码安全等级 0.无  1.弱  2.中 3.强
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sPayPassWordGrade",Required = true,DataLength = 4)]
        public Int32? sPayPassWordGrade
        {
            get; set;
        }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMail",Required = true,DataLength = 50)]
        public String sMail
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sRemark",Required = true,DataLength = 0)]
        public String sRemark
        {
            get; set;
        }

        /// <summary>
        /// 注册邀请码
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sRegCode", Required = false, DataLength = 32)]
        public Guid? sRegCode
        {
            get; set;
        }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dLoginEndTime",Required = true,DataLength = 8)]
        public DateTime? dLoginEndTime
        {
            get; set;
        }

        /// <summary>
        /// 数据来源（1-PC，2-微信，3-安卓，4-IOS）
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iSource",Required = true,DataLength = 4)]
        public Int32? iSource
        {
            get; set;
        }

        /// <summary>
        /// 昵称
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sNickName",Required = true,DataLength = 30)]
        public String sNickName
        {
            get; set;
        }

        /// <summary>
        /// 性别:1男,2女
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iSex",Required = true,DataLength = 4)]
        public Int32? iSex
        {
            get; set;
        }

        /// <summary>
        /// qq
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sQQ",Required = true,DataLength = 30)]
        public String sQQ
        {
            get; set;
        }

        /// <summary>
        /// 生日
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dBirthday",Required = true,DataLength = 8)]
        public DateTime? dBirthday
        {
            get; set;
        }

        /// <summary>
        /// 年龄
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iAge",Required = true,DataLength = 4)]
        public Int32? iAge
        {
            get; set;
        }

        /// <summary>
        /// 学历 （文盲 小学 中学 高中  学士  硕士 博士）
        /// </summary>
        [FieldInfo(DataFieldLength = 60,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sEduBackground",Required = true,DataLength = 30)]
        public String sEduBackground
        {
            get; set;
        }

        /// <summary>
        /// 职业
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sPofessional",Required = true,DataLength = 50)]
        public String sPofessional
        {
            get; set;
        }

        /// <summary>
        /// 收入
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dIncome",Required = true,DataLength = 9)]
        public Decimal? dIncome
        {
            get; set;
        }

        /// <summary>
        /// 一级会员标识,不能被发展为下线  1-一级会员 -1-非一级会员
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sIDType",Required = true,DataLength = 50)]
        public String sIDType
        {
            get; set;
        }

        /// <summary>
        /// 证件号码
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sIDCard",Required = true,DataLength = 50)]
        public String sIDCard
        {
            get; set;
        }

        /// <summary>
        /// 积分
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iIntegral",Required = true,DataLength = 4)]
        public Int32? iIntegral
        {
            get; set;
        }

        /// <summary>
        /// 客户登录名
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLoginName",Required = true,DataLength = 50)]
        public String sLoginName
        {
            get; set;
        }

        /// <summary>
        /// 客户的OpenId
        /// </summary>
        [FieldInfo(DataFieldLength = 100, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sOpenId", Required = true, DataLength = 50)]
        public String sOpenId
        {
            get; set;
        }
    }
}
