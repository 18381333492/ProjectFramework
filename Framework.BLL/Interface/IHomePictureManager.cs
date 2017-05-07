using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
 public abstract class IHomePictureManager:BaseBll
    {
        /// <summary>
        /// 获取banner列表数据绑定
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_ImagesDTO> GetPageList(PageInfo info);
        /// <summary>
        /// 添加首页Banner图
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public abstract int AddPicture(EHECD_ImagesDTO dto);
        /// <summary>
        /// 删除首页Banner图
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public abstract int Delete(Dictionary<string, object> dic);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="editText"></param>
        /// <returns></returns>
        public abstract int Update(EHECD_ImagesDTO dto);
        /// <summary>
        /// 根据ID获取Banner信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public abstract EHECD_ImagesDTO GetPicture(string ID);
        /// <summary>
        /// 显示/隐藏
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract int DisplatPicture(Dictionary<string, object> obj);
        /// <summary>
        /// 查看插入的ID
        /// </summary>
        /// <returns></returns>
        public abstract EHECD_ImagesDTO lookImage();

        public abstract int deletePicture(string ID);

    }
}
