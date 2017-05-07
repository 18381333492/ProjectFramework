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
    /// 游记
    /// author 王其
    /// </summary>
    public abstract class ITravelNoteManager : BaseBll
    {
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public abstract PagingRet<Dictionary<string,object>> GetList(Dapper.PageInfo pageInfo,Guid? guid,dynamic where);

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public abstract int DoAdd(Dictionary<string,object> dir);

        /// <summary>
        /// 编辑游记
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="traveImg"></param>
        /// <returns></returns>
        public abstract int DoEdit(EHECD_TravelsNotesDTO dto, string traveImg);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public abstract int DoEditOrder(EHECD_TravelsNotesDTO dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public abstract int DoDelete(string ids);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract EHECD_TravelsNotesDTO GetSingle(Guid? id);

        /// <summary>
        /// 获取对应的图片数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract IList<EHECD_ImagesDTO> GetImageList(Guid? id);

        /// <summary>
        /// 获取店铺列表
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_ShopSetDTO> GetShopList(string ShopName,Guid id);

        /// <summary>
        /// 将游记布置到
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public abstract int DoSetting(List<Dictionary<string, object>> data);

        
    }
}
