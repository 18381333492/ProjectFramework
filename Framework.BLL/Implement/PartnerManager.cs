using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using Framework.DI;
using Framework.MapperConfig;

namespace Framework.BLL
{
    public class PartnerManager : IPartnerManager
    {
        /// <summary>
        /// 已通过审核的合伙人
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetPassPartner(PageInfo page, Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID,tUserState,dCreateTime,sUserName,sMobileNum,sProvice,sCity,sCounty FROM EHECD_SystemUser  WHERE bIsDeleted = 0 AND tUserType = 2 AND  tUserState = 0");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (sUserName like  '%{0}%')", dic["sKeyword"].ToString());
            }

            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
        }
        /// <summary>
        /// 未通过审核的合伙人
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetUnPassPartner(PageInfo page, Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID,sName,sMobileNum,dCreateTime,sShopName,sProvice,sCity,sCounty,sAddress,iState FROM EHECD_Apply WHERE bIsDeleted = 0 and iType=0 and (iState=0 or iState=2)");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (sShopName like  '%{0}%')", dic["sKeyword"].ToString());
            }

            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
        }
        /// <summary>
        /// 添加合伙人
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public override int AddPartner(Dictionary<string, object> dir)
        {
            StringBuilder builder = new StringBuilder();
            EHECD_SystemUserDTO user = new EHECD_SystemUserDTO()
            {
                ID = GuidHelper.GetSecuentialGuid(),
                sLoginName = dir["sLoginName"].ToString(),
                sMobileNum = dir["sMobileNum"].ToString(),
                sUserName = dir["sUserName"].ToString(),
                sProvice = dir["sProvice"].ToString(),
                sCity = dir["sCity"].ToString(),
                sCounty = dir["sCounty"].ToString(),
                sMainBusiness = dir["sMainBusiness"].ToString(),
                sPassWord = Helper.Security.GetMD5Hash(dir["sPassWord"].ToString()),
                tUserState = 0,
                tUserType = 2,
                sUserNickName = "合伙人"
            };

            string travelSql = DBSqlHelper.GetInsertSQL(user);
            builder.Append(travelSql).Append(";");
            var partnerID = query.SingleQuery<EHECD_RoleDTO>("SELECT ID FROM EHECD_Role WHERE bIsDeleted=0 AND sRoleName='合伙人'", null);
            EHECD_SystemUser_R_RoleDTO role = new EHECD_SystemUser_R_RoleDTO();
            role.ID = GuidHelper.GetSecuentialGuid();
            role.sRoleID = partnerID.ID;
            role.sUserID = user.ID;
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUser_R_RoleDTO>(role)).Append(";");

            //添加身份证的图片
            foreach (string item in dir["sImagePath"].ToString().Split(','))
            {
                EHECD_ImagesDTO img = new EHECD_ImagesDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sImageName = "身份证",
                    sBelongID = user.ID,
                    sImagePath = item,
                    iState=5,
                    bIsDeleted = false
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(img)).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeletePartner(Dictionary<string, object> dic)
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
            StringBuilder sSql = new StringBuilder();
            var list = query.QueryList<EHECD_SystemUserDTO>(string.Format("SELECT * FROM EHECD_SystemUser WHERE ID IN ({0})", string.Join(",", builder)), null);

            List<string> sPhoneList = new List<string>();
            foreach (var item in list)
            {
                sPhoneList.Add("'" + item.sLoginName + "'");
            }
            var data = query.QueryList<EHECD_ApplyDTO>(string.Format("SELECT * FROM EHECD_Apply WHERE sMobileNum IN ({0}) AND iType=0", string.Join(",", sPhoneList)), null);


            List<string> IDList = new List<string>();
            foreach (var item in data)
            {
                IDList.Add("'" + item.ID + "'");
            }
            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_ApplyDTO>(
             new EHECD_ApplyDTO()
             { bIsDeleted = true },
             string.Format("where ID in ({0})", string.Join(",", IDList))));

            sSql.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(
             new EHECD_SystemUserDTO()
             { bIsDeleted = true },
            string.Format("where ID in ({0})", string.Join(",", builder))));

            return helper.ExcuteTransaction(sSql.ToString());
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int UpDatePartnerPassword(EHECD_SystemUserDTO dto)
        {
            var obj = MapperHelpper.Map<EHECD_SystemUserDTO>(dto);
            obj.sPassWord = Helper.Security.GetMD5Hash(dto.sPassWord.ToString());
            return excute.UpdateSingle<EHECD_SystemUserDTO>(obj, string.Format(" Where ID='{0}'", dto.ID));
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DelayPartner(Dictionary<string, object> dic)
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
            return helper.UpdateSingle<EHECD_ApplyDTO>(new EHECD_ApplyDTO() { iState = 2 }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }
        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int PassPartner(Dictionary<string, object> dic)
        {
            StringBuilder builder = new StringBuilder();
            var ID =dic["ID"].ToString().Split(',');
            for (int i = 0; i < ID.Count(); i++)
            {
               var user=query.SingleQuery<EHECD_ApplyDTO>("select ID,sPassWord,sProvice,sCity,sCounty,sMainBusiness,sAddress,sMobileNum,sName,sShopName,sClientID from EHECD_Apply WHERE bIsDeleted = 0  and iType=0 and ID=@ID", new { ID = ID[i] });

                //查询用户的密码
                var userPassword = query.SingleQuery<EHECD_ClientDTO>("SELECT sPassWord FROM EHECD_Client WHERE ID=@sClientID", new { sClientID = user.sClientID });
                EHECD_SystemUserDTO sys = new EHECD_SystemUserDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sLoginName = user.sMobileNum,
                    sPassWord = userPassword.sPassWord,
                    sUserName = user.sName,
                    tUserState = 0,
                    tUserType = 2,
                    sUserNickName = "合伙人",
                    dCreateTime = DateTime.Now,
                    sProvice = user.sProvice,
                    sCity = user.sCity,
                    sCounty = user.sCounty,
                    sAddress = user.sAddress,
                    bIsDeleted = false,
                    sMobileNum = user.sMobileNum,
                    sMessage = 0,
                    sMainBusiness = user.sMainBusiness,
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUserDTO>(sys)).Append(";");
                //分配合伙人角色
                var partnerID = query.SingleQuery<EHECD_RoleDTO>("SELECT ID FROM EHECD_Role WHERE bIsDeleted=0 AND sRoleName='合伙人'", null);
                EHECD_SystemUser_R_RoleDTO role = new EHECD_SystemUser_R_RoleDTO();
                role.ID = GuidHelper.GetSecuentialGuid();
                role.sRoleID =partnerID.ID;
                role.sUserID = sys.ID;
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUser_R_RoleDTO>(role)).Append(";");

                //发送站内信
                EHECD_SysMessageDTO message = new EHECD_SysMessageDTO() {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sSender ="系统管理员",
                    sMsgTitle="申请合伙人成功",
                    bIsDeleted=false,
                    dInsertTime=DateTime.Now,
                    sMsgContent= "您申请成为合伙人已通过审核，请登录后台，账号为注册手机号，初始密码为商城登录密码",
                    iRecevierType=1

                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDTO>(message)).Append(";");
                //保存站内信的内容
                EHECD_SysMessageDetailDTO detail = new EHECD_SysMessageDetailDTO() {
                    ID=GuidHelper.GetSecuentialGuid(),
                    sReceiverID=user.sClientID,
                    dInsertTime=DateTime.Now,
                    sMailID=message.ID,
                    sReceiver = user.sName,
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDetailDTO>(detail)).Append(";");
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ApplyDTO>(new EHECD_ApplyDTO() { iState = 1 }, string.Format("where ID IN ('{0}')", user.ID)));
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { sBelongID=new Guid(sys.ID.ToString())},string.Format("where sBelongID IN ('{0}')",user.ID)));

            }

            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 根据ID获取登录名
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO GetPartner(string ID)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select ID,sLoginName from EHECD_SystemUser WHERE bIsDeleted = 0 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO GetUserByID(string ID)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>(@"SELECT
                                                            a.ID,
                                                            a.sMobileNum,
                                                            a.sUserName,
                                                            b.sProvice,
                                                            b.sCity,
                                                            b.sCounty,
                                                            b.sAddress,
                                                            a.sMainBusiness,
                                                            a.sPartnerName,
                                                            a.sPartnerID
                                                        FROM
                                                            EHECD_SystemUser a, EHECD_ShopSet b
                                                        WHERE

                                                            a.bIsDeleted = 0 and b.bIsDelete = 0
                                                        AND a.ID =@ID AND a.ID = b.sShopID", new { ID=ID});
        }
        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override IList<EHECD_ImagesDTO> SerachName(string ID)
        {
            return query.QueryList<EHECD_ImagesDTO>("select ID,sImagePath from EHECD_Images where bIsDeleted=0 and iState=5 AND sBelongID=@sBelongID", new { sBelongID = ID });
        }
        /// <summary>
        /// 删除没有通过的合伙人
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteUnPassPartner(Dictionary<string, object> dic)
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
            return helper.UpdateSingle<EHECD_ApplyDTO>(new EHECD_ApplyDTO() { bIsDeleted = true }, string.Format("where ID in ({0})", string.Join(",", builder)));
        }

        public override EHECD_SystemUserDTO GetPartnerDetail(string ID)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>(@"select ID,
	                                                        sMobileNum,
	                                                        sUserName,
	                                                        sProvice,
	                                                        sCity,
	                                                        sCounty,
	                                                        sAddress,
	                                                        sMainBusiness,
	                                                        sPartnerName,
	                                                        sPartnerID
                                                        FROM
	                                                        EHECD_SystemUser
                                                        WHERE
	                                                        bIsDeleted = 0
                                                        AND ID =@ID", new { ID = ID });
        }
    }
}
