using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class IShopSetManager:BaseBll
    {
        /// <summary>
        /// 设置店铺信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int SetShopMessage(EHECD_SystemUserDTO user,Dictionary<string,object> data);
        /// <summary>
        /// 获取本店客服
        /// </summary>
        /// <returns></returns>
        public abstract IList<EHECD_CustomServiceDTO> GetCustomers(EHECD_SystemUserDTO user);
        /// <summary>
        /// 获取店铺设置
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract EHECD_ShopSetDTO GetShopSet(EHECD_SystemUserDTO user);
        /// <summary>
        /// 获取店铺的轮播图
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract IList<EHECD_ImagesDTO> GetImageList(EHECD_SystemUserDTO user);
        /// <summary>
        /// 更改店铺的信息
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract int HosterMessage(EHECD_ShopSetDTO dto,EHECD_SystemUserDTO user);
    }
}
