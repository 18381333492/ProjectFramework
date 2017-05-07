using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
   public abstract class IArticle:BaseBll
    {
        /// <summary>
        /// 获取全部文章列表
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_ArticleDTO> GetPageList(PageInfo info);
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="addtext"></param>
        /// <returns></returns>
        public abstract int Insert(Article addtext);
        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="editText"></param>
        /// <returns></returns>
        public abstract int Update(Article editText);
        /// <summary>
        /// 根据ID查找文章
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ArticleDTO searchText(string ID);
        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int Delete(Dictionary<string, object> dic);




        /// <summary>
        /// 预览文章
        /// </summary>
        /// <param name="addtext"></param>
        /// <returns></returns>
        public abstract int Preview(Article addtext);
    }
}
