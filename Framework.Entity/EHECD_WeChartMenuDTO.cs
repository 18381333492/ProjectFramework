using System;
using System.Text;
using System.Collections.Generic;
using System.Data;

namespace Framework.DTO
{
    [Serializable]
    [TableInfo(TableName = "EHECD_WeChartMenu")]
    public class EHECD_WeChartMenuDTO
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
        /// 菜单名
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sMenuName",Required = true,DataLength = 50)]
        public String sMenuName
        {
            get; set;
        }

        /// <summary>
        /// 所属菜单
        /// </summary>
        [FieldInfo(DataFieldLength = 100,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSubmenu",Required = false,DataLength = 50)]
        public String sSubmenu
        {
            get; set;
        }

        /// <summary>
        /// 所属菜单ID
        /// </summary>
        [FieldInfo(DataFieldLength = 16,DataFieldPrecision = 0,DataFieldScale = 0,FiledName = "sSubmenuID",Required = false,DataLength = 32)]
        public Guid? sSubmenuID
        {
            get; set;
        }

        /// <summary>
        /// 触发类型(0-关键字，1-url)
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iTouchType",Required = true,DataLength = 1)]
        public Byte? iTouchType
        {
            get; set;
        }

        /// <summary>
        /// 排序编号
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 3,DataFieldScale = 0,FiledName = "iOrderNo",Required = true,DataLength = 1)]
        public Byte? iOrderNo
        {
            get; set;
        }

        /// <summary>
        /// 是否显示
        /// </summary>
        [FieldInfo(DataFieldLength = 1,DataFieldPrecision = 1,DataFieldScale = 0,FiledName = "bIsVisible",Required = true,DataLength = 1)]
        public Boolean? bIsVisible
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
        /// 关键字/url
        /// </summary>
        [FieldInfo(DataFieldLength = 100, DataFieldPrecision = 0, DataFieldScale = 0, FiledName = "sKeyword", Required = false, DataLength = 50)]
        public String sKeyword
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
    }
}
