using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Orders")]
    public class EHECD_OrdersDTO
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
        /// 使用的优惠劵ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sCouponID", Required = true, DataLength = 32)]
        public Guid? sCouponID
        {
            get; set;
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [FieldInfo(DataFieldLength = 32,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 32)]
        public String sOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 购买人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientID",Required = true,DataLength = 32)]
        public Guid? sClientID
        {
            get; set;
        }

        /// <summary>
        /// 客户登录名
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sClientLoginName",Required = true,DataLength = 255)]
        public String sClientLoginName
        {
            get; set;
        }

        /// <summary>
        /// 订单支付总价格（包含所有附加费用，如运费、优惠券积分抵扣的费用等）
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 15,DataFieldScale = 2,FiledName = "iTotalPrice",Required = true,DataLength = 9)]
        public Decimal? iTotalPrice
        {
            get; set;
        }

        /// <summary>
        /// 订单支付原价（原始价格，只有订单商品的价格）
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 15,DataFieldScale = 2,FiledName = "iOriginalTotalPrice",Required = true,DataLength = 9)]
        public Decimal? iOriginalTotalPrice
        {
            get; set;
        }

        /// <summary>
        /// 订单物流占用价格（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 5,DataFieldPrecision = 8,DataFieldScale = 2,FiledName = "iLogisticsPrice",Required = true,DataLength = 5)]
        public Decimal? iLogisticsPrice
        {
            get; set;
        }

        /// <summary>
        /// 积分抵扣的金额（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iScopeDeductiblePrice",Required = true,DataLength = 4)]
        public Int32? iScopeDeductiblePrice
        {
            get; set;
        }

        /// <summary>
        /// 订单完成后获取的积分数量（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iGetScope",Required = true,DataLength = 4)]
        public Int32? iGetScope
        {
            get; set;
        }

        /// <summary>
        /// 订单下单时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dBookTime",Required = true,DataLength = 8)]
        public DateTime? dBookTime
        {
            get; set;
        }

        /// <summary>
        /// 付款时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dPayTime",Required = true,DataLength = 8)]
        public DateTime? dPayTime
        {
            get; set;
        }

        /// <summary>
        /// 发货时间（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dSendTime",Required = true,DataLength = 8)]
        public DateTime? dSendTime
        {
            get; set;
        }

        /// <summary>
        /// 订单完成时间（该项目中体现为核销时间）
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dFinishTime",Required = true,DataLength = 8)]
        public DateTime? dFinishTime
        {
            get; set;
        }

        /// <summary>
        /// 订单下所有商品(客房、门票、周边)的名称，以逗号分隔
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNameList",Required = true,DataLength = 0)]
        public String sGoodsNameList
        {
            get; set;
        }

        /// <summary>
        /// 订单所属商户ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreID",Required = true,DataLength = 32)]
        public Guid? sStoreID
        {
            get; set;
        }

        /// <summary>
        /// 收货人姓名（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiver",Required = true,DataLength = 255)]
        public String sReceiver
        {
            get; set;
        }

        /// <summary>
        /// 收货人电话
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiverPhone",Required = true,DataLength = 50)]
        public String sReceiverPhone
        {
            get; set;
        }

        /// <summary>
        /// 收货地区 - 省（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiveProvince",Required = true,DataLength = 255)]
        public String sReceiveProvince
        {
            get; set;
        }

        /// <summary>
        /// 收货地区 - 市（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiveCity",Required = true,DataLength = 255)]
        public String sReceiveCity
        {
            get; set;
        }

        /// <summary>
        /// 收货地区- 区县（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiveRegion",Required = true,DataLength = 255)]
        public String sReceiveRegion
        {
            get; set;
        }

        /// <summary>
        /// 收货详细地址（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReceiverAddress",Required = true,DataLength = 255)]
        public String sReceiverAddress
        {
            get; set;
        }

        /// <summary>
        /// 物流配送名称（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLogisticsName",Required = true,DataLength = 50)]
        public String sLogisticsName
        {
            get; set;
        }

        /// <summary>
        /// 物流配送编码（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLogisticsCode",Required = true,DataLength = 50)]
        public String sLogisticsCode
        {
            get; set;
        }

        /// <summary>
        /// 物流单号（该项目暂时没有用）
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sLogisticsOrderNo",Required = true,DataLength = 50)]
        public String sLogisticsOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 订单下单备注
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sDescribe",Required = true,DataLength = 0)]
        public String sDescribe
        {
            get; set;
        }

        /// <summary>
        /// 订单类别 0客房 1门票 2周边产品
   
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iType",Required = true,DataLength = 4)]
        public Int32? iType
        {
            get; set;
        }

        /// <summary>
        /// 订单状态 0待付款 1待使用 2-已核销   
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iState",Required = true,DataLength = 4)]
        public Int32? iState
        {
            get; set;
        }

        /// <summary>
        /// 是否删除标志
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 数据来源(1-PC,2-网页，3-Android,4-IOS)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iSource",Required = true,DataLength = 4)]
        public Int32? iSource
        {
            get; set;
        }


        /// <summary>
        /// 订单的合伙人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sPartnerID", Required = false, DataLength = 32)]
        public Guid? sPartnerID
        {
            get; set;
        }
    }
}
