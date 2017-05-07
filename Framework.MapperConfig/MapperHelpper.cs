using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.MapperConfig
{
    public class MapperHelpper
    {
        /// <summary>
        /// 映射对象
        /// </summary>
        /// <typeparam name="T">要从数据源映射成的类型</typeparam>
        /// <param name="value">源数据</param>
        /// <returns>映射结果</returns>
        public static T Map<T>(Object value)
        {
            return AutoMapper.Mapper.Map<T>(value);
        }
    }
}
