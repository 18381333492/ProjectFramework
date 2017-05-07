using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    /// <summary>
    /// 优惠劵 【抽象】
    /// author 王其
    /// </summary>
    public abstract class ICommentManager : BaseBll
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetList(Dapper.PageInfo pageInfo,Guid? guid,dynamic where);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public abstract int DoEdit(EHECD_CommentDTO dto);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract EHECD_CommentDTO GetSingle(Guid? id);

        /// <summary>
        /// 获取对应的图片数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract IList<EHECD_ImagesDTO> GetImageList(Guid? id);

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public abstract int DoDelete(string ids);
    }
}
