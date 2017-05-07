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
    public abstract class IBaseSettingManager : BaseBll
    {
        /// <summary>
        /// 获取客服列表
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_CustomServiceDTO> GetCustomers();

        /// <summary>
        /// 获取轮播图列表
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_ImagesDTO> GetImageList();

        /// <summary>
        /// 获取基础设置
        /// </summary>
        /// <returns></returns>
        public abstract EHECD_BaseSettingDTO GetBaseSetting();

        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public abstract int DoEdit(Dictionary<string,object> data);
    }
}
