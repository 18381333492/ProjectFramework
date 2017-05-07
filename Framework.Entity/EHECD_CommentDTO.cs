using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_Comment")]
    public class EHECD_CommentDTO
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
        /// 评价人ＩＤ
        /// </summary>
        [FieldInfo(DataFieldLength = 16, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sCommenterID", Required = true, DataLength = 32)]
        public Guid? sCommenterID
        {
            get; set;
        }

        /// <summary>
        /// 订单ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderID",Required = true,DataLength = 32)]
        public Guid? sOrderID
        {
            get; set;
        }

        /// <summary>
        /// 订单号
        /// </summary>
        [FieldInfo(DataFieldLength = 200,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sOrderNo",Required = true,DataLength = 100)]
        public String sOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 商品ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsID",Required = true,DataLength = 32)]
        public Guid? sGoodsID
        {
            get; set;
        }

        /// <summary>
        /// 商品号
        /// </summary>
        [FieldInfo(DataFieldLength = 64,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sGoodsNo",Required = false,DataLength = 32)]
        public String sGoodsNo
        {
            get; set;
        }

        /// <summary>
        /// 商品名称
        /// </summary>
        [FieldInfo(DataFieldLength = 510, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sGoodsName", Required = true, DataLength = 255)]
        public String sGoodsName
        {
            get; set;
        }


        /// <summary>
        /// 店铺ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sStoreID",Required = true,DataLength = 32)]
        public Guid? sStoreID
        {
            get; set;
        }

      

        /// <summary>
        /// 评价时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8,DataFieldPrecision = 23,DataFieldScale = 3,FiledName = "dCommentTime",Required = true,DataLength = 8)]
        public DateTime? dCommentTime
        {
            get; set;
        }

        /// <summary>
        /// 评价内容
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCommentContent",Required = true,DataLength = 0)]
        public String sCommentContent
        {
            get; set;
        }

        /// <summary>
        /// 评价人
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sCommenterName",Required = true,DataLength = 25)]
        public String sCommenterName
        {
            get; set;
        }

        /// <summary>
        /// 评价分数
        /// </summary>
        [FieldInfo(DataFieldLength = 9, DataFieldPrecision = 12, DataFieldScale = 2, FiledName = "iCommentScore", Required = true, DataLength = 9)]
        public Decimal? iCommentScore
        {
            get; set;
        }

        ///// <summary>
        ///// 评论星级
        ///// </summary>
        //[FieldInfo(DataFieldLength = 4, DataFieldPrecision = 10, DataFieldScale = 0, FiledName = "iCommentStar", Required = true, DataLength = 4)]
        //public Int32? iCommentStar
        //{
        //    get; set;
        //}

        /// <summary>
        /// 是否回复（0-否 1-是）
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsReplay",Required = true,DataLength = 1)]
        public Boolean? bIsReplay
        {
            get; set;
        }

        /// <summary>
        /// 回复内容
        /// </summary>
        [FieldInfo(DataFieldLength = -1,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReplayContent",Required = true,DataLength = 0)]
        public String sReplayContent
        {
            get; set;
        }

        /// <summary>
        /// 回复人ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReplayerID",Required = true,DataLength = 32)]
        public Guid? sReplayerID
        {
            get; set;
        }

        /// <summary>
        /// 回复人
        /// </summary>
        [FieldInfo(DataFieldLength = 50,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sReplayer",Required = true,DataLength = 25)]
        public String sReplayer
        {
            get; set;
        }

        /// <summary>
        /// 回复时间
        /// </summary>
        [FieldInfo(DataFieldLength = 8, DataFieldPrecision = 23, DataFieldScale = 3, FiledName = "dReplayTime", Required = true, DataLength = 8)]
        public DateTime? dReplayTime
        {
            get; set;
        }

        /// <summary>
        /// 是否删除(0-否 1-是)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsDeleted",Required = true,DataLength = 1)]
        public Boolean? bIsDeleted
        {
            get; set;
        }

        /// <summary>
        /// 评论图片地址
        /// </summary>
        [FieldInfo(DataFieldLength = -1, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sCommentImgPath", Required = false, DataLength = 0)]
        public String sCommentImgPath
        {
            get; set;
        }
    }
}
