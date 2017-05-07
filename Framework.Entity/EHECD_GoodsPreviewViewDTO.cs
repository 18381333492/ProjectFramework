using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_GoodsPreviewView")]
    public class EHECD_GoodsPreviewViewDTO
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
        /// 商品名称
        /// </summary>
        [FieldInfo(DataFieldLength = 510,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsName",Required = true,DataLength = 255)]
        public String sGoodsName
        {
            get; set;
        }

        /// <summary>
        /// 商品分类(1--客房，2-票务，3--周边)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "sGoodsCategory",Required = true,DataLength = 4)]
        public Int32? sGoodsCategory
        {
            get; set;
        }

        /// <summary>
        /// 房型
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHouseSize",Required = false,DataLength = 32)]
        public Guid? sHouseSize
        {
            get; set;
        }

        /// <summary>
        /// 房间数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iHouseCount",Required = false,DataLength = 4)]
        public Int32? iHouseCount
        {
            get; set;
        }

        /// <summary>
        /// 床的数量
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iHouseBedCount",Required = false,DataLength = 4)]
        public Int32? iHouseBedCount
        {
            get; set;
        }

        /// <summary>
        /// 适宜人数
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iHousePerson",Required = false,DataLength = 4)]
        public Int32? iHousePerson
        {
            get; set;
        }

        /// <summary>
        /// 客房简述/票务简述
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sHouseOrTicketDetail",Required = false,DataLength = 0)]
        public String sHouseOrTicketDetail
        {
            get; set;
        }

        /// <summary>
        /// 商品图片
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsPictures",Required = true,DataLength = 0)]
        public String sGoodsPictures
        {
            get; set;
        }

        /// <summary>
        /// 佣金类型(1--固定金额，2--商品价格比例)
        /// </summary>
        [FieldInfo(DataFieldLength = 4,DataFieldPrecision = 10,DataFieldScale = 0,FiledName = "iCommissionType",Required = true,DataLength = 4)]
        public Int32? iCommissionType
        {
            get; set;
        }

        /// <summary>
        /// 佣金
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dMoney",Required = true,DataLength = 9)]
        public Decimal? dMoney
        {
            get; set;
        }

        /// <summary>
        /// 商品介绍
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsIntroduce",Required = true,DataLength = 0)]
        public String sGoodsIntroduce
        {
            get; set;
        }

        /// <summary>
        /// 商品价格1
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dGoodsFisrtPrice",Required = true,DataLength = 9)]
        public Decimal? dGoodsFisrtPrice
        {
            get; set;
        }

        /// <summary>
        /// 商品价格2
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dGoodsSecPrice",Required = true,DataLength = 9)]
        public Decimal? dGoodsSecPrice
        {
            get; set;
        }

        /// <summary>
        /// 商品价格3
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dGoodsThirdPrice",Required = true,DataLength = 9)]
        public Decimal? dGoodsThirdPrice
        {
            get; set;
        }

        /// <summary>
        /// 是否上架
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bShelves",Required = true,DataLength = 1)]
        public Boolean? bShelves
        {
            get; set;
        }

        /// <summary>
        /// 是否秒杀
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bSeckill",Required = true,DataLength = 1)]
        public Boolean? bSeckill
        {
            get; set;
        }

        /// <summary>
        /// 是否特卖
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bSpecialSale",Required = true,DataLength = 1)]
        public Boolean? bSpecialSale
        {
            get; set;
        }

        /// <summary>
        /// 秒杀价格
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dSeckillPrices",Required = false,DataLength = 9)]
        public Decimal? dSeckillPrices
        {
            get; set;
        }

        /// <summary>
        /// 特卖价格
        /// </summary>
        [FieldInfo(DataFieldLength = 9,DataFieldPrecision = 12,DataFieldScale = 2,FiledName = "dSpecialSalePrices",Required = false,DataLength = 9)]
        public Decimal? dSpecialSalePrices
        {
            get; set;
        }

        /// <summary>
        /// 秒杀时间段(开始时间，结束时间)
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSeckillTime",Required = false,DataLength = 100)]
        public String sSeckillTime
        {
            get; set;
        }

        /// <summary>
        /// 特卖时间段(开始时间，结束时间)
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSpecialSaleTime",Required = false,DataLength = 100)]
        public String sSpecialSaleTime
        {
            get; set;
        }

        /// <summary>
        /// 逻辑删除标识
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 商品编号(备用字段)
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNo",Required = false,DataLength = 100)]
        public String sGoodsNo
        {
            get; set;
        }

        /// <summary>
        /// 商品所属店铺
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreId",Required = false,DataLength = 32)]
        public Guid? sStoreId
        {
            get; set;
        }

        /// <summary>
        /// 上架时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dShelvesTime",Required = true,DataLength = 8)]
        public DateTime? dShelvesTime
        {
            get; set;
        }
    }
}
