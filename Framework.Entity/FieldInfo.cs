using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DTO
{
    /// <summary>
    /// 数据字段信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldInfo:Attribute
    {       
        /// <summary>
        /// 数据字段字节长度(字节)
        /// </summary>
        public int DataFieldLength { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLength { get; set; }

        /// <summary>
        /// 数据字段精度
        /// </summary>
        public int DataFieldPrecision { get; set; }

        /// <summary>
        /// 数据字段小数位
        /// </summary>
        public int DataFieldScale { get; set; }

        /// <summary>
        /// 数据字段是否是必填(不能为空)
        /// </summary>
        public Boolean Required { get; set; }

        /// <summary>
        /// 数据字段在数据库的名称
        /// </summary>
        public String FiledName { get; set; }
    }
    /// <summary>
    /// 数据表信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableInfo : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

    }
}
