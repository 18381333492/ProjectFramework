using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.BLL
{
   public abstract class ISharePosterManager:BaseBll
    {
        /// <summary>
        /// 上传海报分享
       /// </summary>
       /// <param name="dto"></param>
       /// <param name="image"></param>
       /// <returns></returns>
        public abstract int SharePosterEdit(EHECD_SystemUserDTO dto,EHECD_ImagesDTO image);
        /// <summary>
        /// 查看数据库中是否有图片
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public abstract EHECD_ImagesDTO GetImage(EHECD_SystemUserDTO dto);
    }
}
