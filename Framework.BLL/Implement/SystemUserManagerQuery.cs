using Framework.Dapper;
using Framework.DTO;
using System.Text;

namespace Framework.BLL
{
    public partial class SystemUserManager:ISystemUserManager
    {
        //获取用户信息
        public override EHECD_SystemUserDTO GetSystemUserInfoById(EHECD_SystemUserDTO user)
        {
            user = query.SingleQuery<EHECD_SystemUserDTO>("SELECT * FROM EHECD_SystemUser WHERE ID = @ID", new { ID = user.ID });
            return user == default(EHECD_SystemUserDTO) ? null : user;            
        }

        //分页系统用户
        public override PagingRet<EHECD_SystemUserDTO> LoadSystemUsers(PageInfo page, dynamic where)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(@"SELECT
	                            ID,
	                            sLoginName,
	                            sUserName,
	                            tUserState,
	                            tUserType,
	                            sUserNickName,
	                            dCreateTime,
	                            dLastLoginTime,
	                            sMobileNum
                            FROM
	                            EHECD_SystemUser
                            WHERE
	                            bIsDeleted = 0 ");

            // 获取参数
            if (!string.IsNullOrEmpty(where.data.sLoginName.Value.ToString())) builder.Append(string.Format(@" AND (sLoginName LIKE '%{0}%' OR sUserName LIKE '%{0}%')", where.data.sLoginName.Value.ToString()));
            if (!string.IsNullOrEmpty(where.data.sStartTime.Value.ToString())) builder.Append(string.Format(@" AND dCreateTime>='{0}' ", where.data.sStartTime.Value.ToString()));
            if (!string.IsNullOrEmpty(where.data.sEndTime.Value.ToString())) builder.Append(string.Format(@" AND dCreateTime<='{0}' ", where.data.sEndTime.Value.ToString()));

            return query.PaginationQuery<EHECD_SystemUserDTO>(builder.ToString(),page,null);
        }
    }


}
