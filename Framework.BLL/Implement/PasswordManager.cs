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
    public class PasswordManager : IPasswordManager
    {
      /// <summary>
      /// 扫面员页面绑定
      /// </summary>
      /// <param name="info"></param>
      /// <param name="user"></param>
      /// <param name="dic"></param>
      /// <returns></returns>
        public override PagingRet<EHECD_ScannerDTO> GetPageList(PageInfo info, EHECD_SystemUserDTO user, Dictionary<string, object> dic)
        {
           
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID,sLoginName,sReallyName FROM EHECD_Scanner WHERE bIsDeleted = 0 and sShopID='" + user.ID+"'");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                sb.AppendFormat("and (sLoginName like  '%{0}%' or sReallyName like '%{0}%')", dic["sKeyword"].ToString());
            }

            return query.PaginationQuery<EHECD_ScannerDTO>(sb.ToString(),info, null);
        }
        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_ScannerDTO GetScannerById(string ID)
        {
            return query.SingleQuery<EHECD_ScannerDTO>("select ID,sLoginName,sReallyName,sPhone,sPassword from EHECD_Scanner WHERE bIsDeleted = 0 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 添加扫描员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int AddScanner(EHECD_ScannerDTO dto, EHECD_SystemUserDTO user)
        {
            dto.ID = GuidHelper.GetSecuentialGuid();
            dto.sShopID = user.ID;
            dto.sPassword = Helper.Security.GetMD5Hash(dto.sPassword);
            return excute.InsertSingle<EHECD_ScannerDTO>(dto);
        }
        /// <summary>
        /// 删除扫描员
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteScanner(Dictionary<string, object> dic)
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
            return helper.UpdateSingle<EHECD_ScannerDTO>(new EHECD_ScannerDTO() { bIsDeleted = true }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 编辑扫描员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int EditScanner(EHECD_ScannerDTO dto)
        {
            dto.sPassword = Helper.Security.GetMD5Hash(dto.sPassword);
            StringBuilder sb = new StringBuilder();
            if (dto.sPhone == "" || dto.sPhone == null) {
                sb.Append("UPDATE [EHECD_Scanner] SET sPhone=NULL WHERE	ID = '"+dto.ID+"'").Append(";");
            }
            sb.Append(DBSqlHelper.GetUpdateSQL<EHECD_ScannerDTO>(dto, string.Format(" Where ID='{0}'", dto.ID)));
            return excute.ExcuteTransaction(sb.ToString());
        }
        /// <summary>
        /// 查看是否有相同的登录名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override Dictionary<string, object> SearchName(string name)
        {
            Dictionary<string,object> isHaveName=new Dictionary<string, object>() ;
            var sname= query.SingleQuery<Dictionary<string, object>>("SELECT a.sLoginName FROM EHECD_Scanner a,EHECD_SystemUser b WHERE a.bIsDeleted=0 AND b.bIsDeleted=0 AND (a.sLoginName=@name OR b.sLoginName=@name)", new { name = name });
            if (sname != null) {
                isHaveName["name"] = "1";//有相同的登录名
            }
            else {
                isHaveName["name"] = "0";//没有相同的登录名
            }

            return isHaveName;
        }
    }
}
