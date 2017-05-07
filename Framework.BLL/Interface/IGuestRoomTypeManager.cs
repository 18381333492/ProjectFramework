using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Dapper;

namespace Framework.BLL
{
    public abstract class IGuestRoomTypeManager : BaseBll
    {
        /// <summary>
        /// 获取客房类型
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public abstract PagingRet<EHECD_GuestRoomTypeDTO> GetPageList(PageInfo info);

        /// <summary>
        /// 添加客房类型
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public abstract bool AddGuestRoomType(EHECD_GuestRoomTypeDTO room);

        /// <summary>
        /// 获取所有可用房间类型
        /// </summary>
        /// <returns></returns>
        public abstract List<EHECD_GuestRoomTypeDTO> GetAllUsedRoomType();

        /// <summary>
        /// 删除客房类型
        /// </summary>
        /// <param name="roomType"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public abstract bool DeleteRoomType(EHECD_GuestRoomTypeDTO roomType, EHECD_SystemUserDTO user);
    }
}
