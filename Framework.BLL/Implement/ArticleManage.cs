using Framework.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.DI;
using Framework.MapperConfig;
using Framework.Helper;

namespace Framework.BLL
{
    public  class ArticleManage : IArticle
    {
        /// <summary>
        /// 获取页面绑定信息
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override PagingRet<EHECD_ArticleDTO> GetPageList(PageInfo info)
        {
            return query.PaginationQuery<EHECD_ArticleDTO>("select ID,sTitle,dPublishTime from EHECD_Article WHERE bIsDeleted = 0 ", info, null);
        }
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="addtext"></param>
        /// <returns></returns>
        public override int Insert(Article addtext)
        {
            var dto = MapperHelpper.Map<EHECD_ArticleDTO>(addtext);
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.dPublishTime = DateTime.Now;
            return excute.InsertSingle<EHECD_ArticleDTO>(dto);
        }

        /// <summary>
        /// 预览文章
        /// </summary>
        /// <param name="addtext"></param>
        /// <returns></returns>
        public override int Preview(Article addtext)
        {
            var dto = MapperHelpper.Map<EHECD_ArticleDTO>(addtext);
            dto.dPublishTime = DateTime.Now;
            dto.bIsDeleted = true;
            return excute.InsertSingle<EHECD_ArticleDTO>(dto);
        }


        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="editText"></param>
        /// <returns></returns>
        public override int Update(Article editText)
        {
            var dto = MapperHelpper.Map<EHECD_ArticleDTO>(editText);
            return excute.UpdateSingle<EHECD_ArticleDTO>(dto, string.Format(" Where ID='{0}'", dto.ID));
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int Delete(Dictionary<string, object> dic)
        {
            List<string> builder = new List<string>();
            var helper = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
            if (dic["ID"] != null)
            {
                foreach (var item in (dic["ID"].ToString().Split(',')))
                {
                    builder.Add("'" + item + "'");
                }
            }
            return helper.UpdateSingle<EHECD_ArticleDTO>(new EHECD_ArticleDTO() { bIsDeleted = true }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 根据ID查找文章
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ArticleDTO searchText(string ID)
        {
            return query.SingleQuery<EHECD_ArticleDTO>("select ID,sTitle,sContent from EHECD_Article WHERE bIsDeleted = 0 and ID=@ID", new { ID = ID });

        }
    }
}
