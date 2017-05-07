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
    public class SharePosterManager : ISharePosterManager
    {
        /// <summary>
        /// 查看数据库中是否有图片
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override EHECD_ImagesDTO GetImage(EHECD_SystemUserDTO dto)
        {
            return query.SingleQuery<EHECD_ImagesDTO>(" SELECT sImagePath FROM EHECD_Images WHERE iState=2 and sBelongID=@sBelongID", new { sBelongID = dto.ID });
        }
        /// <summary>
        /// 上传海报
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public override int SharePosterEdit(EHECD_SystemUserDTO dto, EHECD_ImagesDTO image)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(@"DELETE FROM EHECD_Images where  iState=2").Append(";");
            EHECD_ImagesDTO photo = new EHECD_ImagesDTO() {
                ID = GuidHelper.GetSecuentialGuid(),
                sBelongID=dto.ID,
                sImagePath=image.sImagePath,
                iState=2
            };
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(photo)).Append(";");
            return excute.ExcuteTransaction(builder.ToString());
        }
    }
}
