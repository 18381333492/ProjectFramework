using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.DI;

namespace Framework.BLL
{
    public class HomePictureManager : IHomePictureManager
    {
      
        public override PagingRet<EHECD_ImagesDTO> GetPageList(PageInfo info)
        {
            return query.PaginationQuery<EHECD_ImagesDTO>("select ID,sImagePath,iOrder,bDisplay,dPublishTime from EHECD_Images WHERE bIsDeleted = 0 and iState=1", info, null);
        }
        /// <summary>
        /// 修改首页图文
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int Update(EHECD_ImagesDTO dto)
        {
            return excute.UpdateSingle<EHECD_ImagesDTO>(dto, string.Format(" where ID='{0}' ", dto.ID));
        }
        /// <summary>
        /// 添加首页图文
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int AddPicture(EHECD_ImagesDTO dto)
        {
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.dPublishTime = DateTime.Now;
            dto.iState = 1;
            return excute.InsertSingle<EHECD_ImagesDTO>(dto);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int Delete(Dictionary<string, object> dic)
        {
            List<string> builder = new List<string>();
            var helper = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
            if (dic["ID"] != null)
            {
                foreach (var item in (dic["ID"].ToString().Split(',')))
                {
                    builder.Add("'" + item + "'");
                }
            }
            return helper.UpdateSingle<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { bIsDeleted = true }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ImagesDTO GetPicture(string ID)
        {
            return query.SingleQuery<EHECD_ImagesDTO>("select ID,sImagePath,iOrder,sTitle,sContent from EHECD_Images WHERE bIsDeleted = 0 and iState=1 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 显示/隐藏
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int DisplatPicture(Dictionary<string, object> obj)
        {
            StringBuilder builder = new StringBuilder();
            EHECD_ImagesDTO user = new EHECD_ImagesDTO();
            string[] id = obj["ID"].ToString().Split(',');
            string[] bDisplay = obj["bDisplay"].ToString().Split(',');
            for (int i = 0; i < id.Length; i++)
            {
                user.ID = new Guid(id[i]);
                user.bDisplay = (bDisplay[i].ToBoolean() == true ? false: true);
                string sql = DBSqlHelper.GetUpdateSQL<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { bDisplay = user.bDisplay }, string.Format("where ID = '{0}'", user.ID));
                builder.Append(sql).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 获取扫码查看数据的ID
        /// </summary>
        /// <returns></returns>
        public override EHECD_ImagesDTO lookImage()
        {
            return query.SingleQuery<EHECD_ImagesDTO>("SELECT TOP 1 ID FROM EHECD_Images WHERE iState=1 AND bIsDeleted=0 ORDER BY dPublishTime DESC ", null);
        }

        public override int deletePicture(string ID)
        {
            return excute.UpdateSingle<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { bIsDeleted = true }, string.Format("where ID = '{0}'", ID));
        }
    }
}
