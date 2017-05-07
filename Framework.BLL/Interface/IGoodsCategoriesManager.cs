using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;
using Framework.Helper;

namespace Framework.BLL
{
    public abstract class IGoodsCategoriesManager:BaseBll
    {
        /// <summary>
        /// 载入商品分类
        /// </summary>
        /// <param name="c">商品分类</param>
        /// <returns></returns>
        public abstract EHECD_CategoriesDTO LoadGoodsCategorie(EHECD_CategoriesDTO c);

        /// <summary>
        /// 载入商品种类（分页）
        /// </summary>
        /// <param name="pageInfo">分页信息</param>
        /// <returns>载入结果</returns>
        public abstract string LoadGoodsCategories();

        /// <summary>
        /// 添加商品种类
        /// </summary>
        /// <param name="c">商品种类</param>
        /// <param name="p">记录日志对象</param>
        /// <returns></returns>
        public abstract EHECD_CategoriesDTO AddGoodsCategories(EHECD_CategoriesDTO c,dynamic p);

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="c">商品种类</param>
        /// <param name="p">记录日志对象</param>
        /// <returns></returns>
        public abstract int DeleteGoodsCategory(EHECD_CategoriesDTO c, dynamic p);
    }
}
