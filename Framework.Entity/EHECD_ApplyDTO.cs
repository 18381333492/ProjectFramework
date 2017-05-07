using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Apply")]
    public class EHECD_ApplyDTO
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
        /// 主营业务/合伙人的个人描述
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMainBusiness",Required = true,DataLength = 0)]
        public String sMainBusiness
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
        /// 申请类型(0-合伙人 1-商家)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "iType",Required = true,DataLength = 1)]
        public Boolean? iType
        {
            get; set;
        }

        /// <summary>
        /// 真实姓名/店主姓名
        /// </summary>
        [FieldInfo(DataFieldLength = 40,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sName",Required = true,DataLength = 20)]
        public String sName
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
        /// 0未通过 1已通过
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 1)]
        public Byte? iState
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
        /// 申请客户的ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }
        /// <summary>
        /// 申请时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8, DataFieldPrecision = 23, DataFieldScale = 3, FiledName = "dCreateTime", Required = true, DataLength = 8)]
        public DateTime? dCreateTime
        {
            get; set;
        }
        /// <summary>
        /// 登录密码
        /// </summary>
        [FieldInfo(DataFieldLength = 100, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sPassWord", Required = true, DataLength = 50)]
        public String sPassWord
        {
            get; set;
        }


        /// <summary>
        /// 分享客推荐码
        /// </summary>
        [FieldInfo(DataFieldLength = 100, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sIDCard", Required = false, DataLength = 50)]
        public String sIDCard
        {
            get; set;
        }
    }
}
