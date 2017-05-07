using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Helper;
using Framework.Dapper;

namespace Framework.BLL
{
    public partial class GuestRoomTypeManager : IGuestRoomTypeManager
    {
        //添加客房类型
        public override bool AddGuestRoomType(EHECD_GuestRoomTypeDTO room)
        {
            room.bIsDeleted = false;

            room.ID = GuidHelper.GetSecuentialGuid();

            return excute.InsertSingle<EHECD_GuestRoomTypeDTO>(room) > 0;
        }

        //删除客房类型
        public override bool DeleteRoomType(EHECD_GuestRoomTypeDTO roomType, EHECD_SystemUserDTO user)
        {
            //1.从商品表查找已存在的该客房类型的客房商品
            var allReadyHaveRooms = query.QueryList<EHECD_GoodsDTO>("SELECT ID FROM EHECD_Goods WHERE sGoodsCategory = 1 AND bIsDeleted = 0 AND sHouseSize = @ID", new { roomType.ID });

            if (allReadyHaveRooms != default(IList<EHECD_GoodsDTO>) && allReadyHaveRooms.Count > 0)
            {
                return false;
                //StringBuilder sb = new StringBuilder();

                ////创建下架和日志的语句
                //foreach (var item in allReadyHaveRooms)
                //{
                //    item.bShelves = false;

                //    sb.AppendLine(DBSqlHelper.GetUpdateSQL<EHECD_GoodsDTO>(item, string.Format("where ID = '{0}'", item.ID)));

                //    sb.AppendLine(DBSqlHelper.GetInsertSQL<EHECD_OperatDTO>(new EHECD_OperatDTO
                //    {
                //        ID = GuidHelper.GetSecuentialGuid(),
                //        bIsDeleted = false,
                //        dOperatTime = DateTime.Now,
                //        iState = 1,
                //        sContent = string.Format("系统用户{0}删除客房类型导致下架对应类型的客房", user.sUserName),
                //        sOperator = user.sUserName
                //    }));

                //    roomType.bIsDeleted = true;

                //    sb.AppendLine(DBSqlHelper.GetUpdateSQL<EHECD_GuestRoomTypeDTO>(roomType, string.Format("WHERE ID = '{0}'", roomType.ID)));
                //}

                //return excute.ExcuteTransaction(sb.ToString()) > 0;
            }
            else
            {
                roomType.bIsDeleted = true;
                return excute.UpdateSingle<EHECD_GuestRoomTypeDTO>(roomType, string.Format("WHERE ID = '{0}'", roomType.ID)) > 0;
            }
        }
    }
}
