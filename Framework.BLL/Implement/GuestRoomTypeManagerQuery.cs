using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;

namespace Framework.BLL
{
    public partial class GuestRoomTypeManager : IGuestRoomTypeManager
    {
        //查询客房类型
        public override PagingRet<EHECD_GuestRoomTypeDTO> GetPageList(PageInfo info)
        {
            return query.PaginationQuery<EHECD_GuestRoomTypeDTO>("SELECT * FROM EHECD_GuestRoomType WHERE bIsDeleted = 0", info, null);
        }

        //获取所有可用房间类型
        public override List<EHECD_GuestRoomTypeDTO> GetAllUsedRoomType()
        {
            return query.QueryList<EHECD_GuestRoomTypeDTO>("SELECT * FROM EHECD_GuestRoomType WHERE bIsDeleted = 0", null).ToList();
        }
    }
}
