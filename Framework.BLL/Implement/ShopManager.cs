using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Dapper;
using Framework.DTO;
using Framework.MapperConfig;
using Framework.Helper;
using Framework.DI;

namespace Framework.BLL
{
    public class ShopManager : IShopManager
    {
        /// <summary>
        /// 店铺页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string,object>> GetPageList(PageInfo page, Dictionary<String, object> dic)
        {
           
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT a.ID,a.tUserState,a.dCreateTime,b.sShopName,a.sMessage,b.sProvice,b.sCity,b.sCounty,b.sAddress FROM EHECD_SystemUser a, EHECD_ShopSet b WHERE a.bIsDeleted = 0 AND b.bIsDelete = 0 AND a.tUserType = 1 AND a.ID = b.sShopID AND( tUserState = 0 OR tUserState = 1)");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                sb.AppendFormat("and (b.sShopName like '%{0}%')", dic["sKeyword"].ToString());
            }
          
            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
        }
        /// <summary>
        /// 根据ID获取短信票数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO GetMessageCount(string ID)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select ID,sMessage from EHECD_SystemUser where bIsDeleted=0 and ID=@ID", new { ID = ID });
        }

        /// <summary>
        /// 修改票数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override int UpdateCount(EHECD_SystemUserDTO dto)
        {
            var sys = MapperHelpper.Map<EHECD_SystemUserDTO>(dto);
            return excute.UpdateSingle<EHECD_SystemUserDTO>(sys, string.Format(" Where ID='{0}'", dto.ID));
        }
        /// <summary>
        /// 添加商店
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public override int AddShop(Dictionary<String,object> dir)
        {
            StringBuilder builder = new StringBuilder();
           
            var partner = query.SingleQuery<EHECD_SystemUserDTO>("select TOP 1 ID,sUserName from EHECD_SystemUser where tUserType=2 and bIsDeleted=0 and sProvice like'" + dir["sProvice"].ToString().Split('市')[0] + "' ", null);

            EHECD_SystemUserDTO user = new EHECD_SystemUserDTO();
            user.ID = GuidHelper.GetSecuentialGuid();
            user.sLoginName = dir["sLoginName"].ToString();
            user.sMobileNum = dir["sMobileNum"].ToString();
            user.sUserName = dir["sHeadName"].ToString();
            user.sAddress = dir["sAddress"].ToString();
            user.sProvice = dir["sProvice"].ToString();
            user.sCity = dir["sCity"].ToString();
            user.sCounty = dir["sCounty"].ToString();
            user.sMainBusiness = dir["sMainBusiness"].ToString();
            user.sPassWord = Helper.Security.GetMD5Hash(dir["sPassWord"].ToString());
            user.sUserNickName = "商家";
            user.sMessage = 0;
            user.tUserState = 0;
            user.tUserType = 1;
            user.dLastLoginTime = DateTime.Now;
            if (partner != null) {
                user.sPartnerID = partner.ID;
                user.sPartnerName = partner.sUserName;
            }
           
            string travelSql = DBSqlHelper.GetInsertSQL(user);
            builder.Append(travelSql).Append(";");

            var roleID = query.SingleQuery<EHECD_RoleDTO>("SELECT ID FROM EHECD_Role WHERE bIsDeleted=0 AND sRoleName='店铺'", null);
            EHECD_SystemUser_R_RoleDTO role = new EHECD_SystemUser_R_RoleDTO();

            role.sRoleID = roleID.ID;
            role.sUserID = user.ID;
            role.ID = GuidHelper.GetSecuentialGuid();
            builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUser_R_RoleDTO>(role)).Append(";");

            if (dir["sCity"].ToString() == "重庆市")
            {
                dir["sFristLetter"] = "c";
            }

            EHECD_ShopSetDTO shop = new EHECD_ShopSetDTO()
            {
                ID = GuidHelper.GetSecuentialGuid(),
                sShopName = dir["sShopName"].ToString(),
                sHeadName = dir["sHeadName"].ToString(),
                sAddress = dir["sAddress"].ToString(),
                sProvice = dir["sProvice"].ToString(),
                sCity = dir["sCity"].ToString(),
                sCounty = dir["sCounty"].ToString(),
                sShopID = user.ID,
                sFristLetter= dir["sFristLetter"].ToString(),
                sLONG=dir["sLONG"].ToString(),
                sLat=dir["sLat"].ToString()

            };
            string shopsql = DBSqlHelper.GetInsertSQL(shop);
            builder.Append(shopsql).Append(";");

            //添加身份证的图片
            foreach (string item in dir["sImagePath"].ToString().Split(','))
            {
                EHECD_ImagesDTO img = new EHECD_ImagesDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sImageName = "身份证",
                    iState=5,
                    sBelongID =user.ID,
                    sImagePath = item,
                    bIsDeleted = false
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ImagesDTO>(img)).Append(";");
            }

            //执行事务
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 待审核页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string,object>> CheckPageList(PageInfo page, Dictionary<string, object> dic)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ID,dCreateTime,sShopName,sProvice,sCity,sCounty,sAddress,iState FROM EHECD_Apply WHERE bIsDeleted = 0 and iType=1 and (iState=0 or iState=2)");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (sShopName like  '%{0}%')", dic["sKeyword"].ToString());
            }
           
            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
        }
        /// <summary>
        /// 通过审核
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int PassCheck(Dictionary<string, object> dic)
        {
            var user = new EHECD_ApplyDTO();
            var clientPW = new EHECD_ClientDTO();
            StringBuilder builder = new StringBuilder();
            var ID = dic["ID"].ToString().Split(',');
            var sLONG = dic["sLONG"].ToString().Split(',');
            var sLat = dic["sLat"].ToString().Split(',');
            var sFristLetter = dic["city"].ToString().Split(',');
            for (int i = 0; i < ID.Count(); i++)
            {
                user = query.SingleQuery<EHECD_ApplyDTO>("select ID,sPassWord,sProvice,sCity,sCounty,sMainBusiness,sAddress,sMobileNum,sName,sShopName,sClientID,sIDCard from EHECD_Apply WHERE bIsDeleted = 0  and iType=1 and ID=@ID", new { ID = ID[i] });

                clientPW = query.SingleQuery<EHECD_ClientDTO>("SELECT sPassWord FROM EHECD_Client WHERE ID=@ID", new { ID = user.sClientID });
                //如果商家填写了推荐码，给分销客发送优惠券
                if (user.sIDCard != null&& !string.IsNullOrEmpty(user.sIDCard.ToString())) {
                    var couponID = query.SingleQuery<EHECD_ClientDTO>("SELECT ID FROM EHECD_Client WHERE sIDCard=@sIDCard", new { sIDCard = user.sIDCard });
                    if (couponID != null) {
                        //如果推荐码正确
                        var idCoupon = query.SingleQuery<EHECD_CouponDTO>("SELECT  TOP 1* FROM EHECD_Coupon WHERE bIsDeleted=0 AND sCouponName='平台优惠券' ORDER BY dInsertTime DESC", null);
                        EHECD_CouponDetailsDTO coupon = new EHECD_CouponDetailsDTO();
                        coupon.sUserID = couponID.ID;
                        coupon.ID = GuidHelper.GetSecuentialGuid();
                        coupon.dGetTime = DateTime.Now;
                        coupon.bIsUsed = false;
                        coupon.sCouponID = idCoupon.ID;

                        builder.Append(DBSqlHelper.GetInsertSQL<EHECD_CouponDetailsDTO>(coupon)).Append(";");
                    }
                    
                }
                var client = query.SingleQuery<EHECD_SystemUserDTO>("select top 1 ID,sUserName FROM EHECD_SystemUser WHERE  bIsDeleted = 0 and tUserState=0 AND tUserType=2 AND sProvice=@sProvice",new { sProvice =user.sProvice});
                EHECD_SystemUserDTO sys = new EHECD_SystemUserDTO();
                sys.ID = GuidHelper.GetSecuentialGuid();
                sys.sLoginName = user.sMobileNum;
                sys.sPassWord = clientPW.sPassWord;
                sys.sUserName = user.sName;
                sys.tUserState = 0;
                sys.tUserType = 1;
                sys.sUserNickName = "商家";
                sys.dCreateTime = DateTime.Now;
                sys.sProvice = user.sProvice;
                sys.sCity = user.sCity;
                sys.sCounty = user.sCounty;
                sys.sAddress = user.sAddress;
                sys.bIsDeleted = false;
                sys.sMobileNum = user.sMobileNum;
                sys.sMessage = 0;
                sys.sMainBusiness = user.sMainBusiness;
                sys.dLastLoginTime = DateTime.Now;
                if (client != null) {
                    sys.sPartnerID = client.ID;
                    sys.sPartnerName = client.sUserName;
                }
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUserDTO>(sys)).Append(";");
                //角色ID
                var roleID = query.SingleQuery<EHECD_RoleDTO>("SELECT ID FROM EHECD_Role WHERE bIsDeleted=0 AND sRoleName='店铺'", null);
                //赋予角色
                EHECD_SystemUser_R_RoleDTO role = new EHECD_SystemUser_R_RoleDTO();
                role.sRoleID = roleID.ID;
                role.sUserID = sys.ID;
                role.ID = GuidHelper.GetSecuentialGuid();
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SystemUser_R_RoleDTO>(role)).Append(";");

                if (user.sCity.ToString() == "重庆市") {
                    sFristLetter[i] = "c";
                }
                //店铺设置
                EHECD_ShopSetDTO shop = new EHECD_ShopSetDTO() {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sShopID=sys.ID,
                    sShopName=user.sShopName,
                    sHeadName=user.sName,
                    sProvice=user.sProvice,
                    sCity=user.sCity,
                    sCounty=user.sCounty,
                    sAddress=user.sAddress,
                    sMobileNum=user.sMobileNum,
                    bIsDelete=false,
                    sLONG= sLONG[i],
                    sLat= sLat[i],
                    sFristLetter= sFristLetter[i],
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_ShopSetDTO>(shop)).Append(";");

                //发送站内信
                EHECD_SysMessageDTO message = new EHECD_SysMessageDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sSender = "系统管理员",
                    sMsgTitle = "申请店铺成功",
                    bIsDeleted = false,
                    dInsertTime = DateTime.Now,
                    sMsgContent = "您的店铺已通过审核，请登录网站http://ykfx.server74.ehecd.com/Admin/Login，账号为注册手机号，初始密码为商城登录密码",
                    iRecevierType = 1

                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDTO>(message)).Append(";");
                //站内信详情
                EHECD_SysMessageDetailDTO detail = new EHECD_SysMessageDetailDTO()
                {
                    ID = GuidHelper.GetSecuentialGuid(),
                    sReceiverID = user.sClientID,
                    sReceiver= user.sName,
                    dInsertTime = DateTime.Now,
                    sMailID = message.ID
                };
                builder.Append(DBSqlHelper.GetInsertSQL<EHECD_SysMessageDetailDTO>(detail)).Append(";");
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ApplyDTO>(new EHECD_ApplyDTO() { iState = 1 }, string.Format("where ID IN ('{0}')", user.ID)));
                builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_ImagesDTO>(new EHECD_ImagesDTO() { sBelongID = new Guid(sys.ID.ToString()) }, string.Format("where sBelongID IN ('{0}')", user.ID)));

            }

            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 拒绝通过
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int DelayCheck(Dictionary<string, object> dic)
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
        /// 删除已审核过的店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int DeleteCheck(Dictionary<string, object> dic)
        {
            List<string> builder = new List<string>();
            var helper = DIEntity.GetInstance().GetImpl<Dapper.ExcuteHelper>();
            var query = DIEntity.GetInstance().GetImpl<Dapper.QueryHelper>();

            StringBuilder sSql = new StringBuilder();
            if (dic["ID"] != null)
            {
                foreach (var item in (dic["ID"].ToString().Split(',')))
                {
                    builder.Add("'" + item + "'");
                }
            }
            var list = query.QueryList<EHECD_SystemUserDTO>(string.Format("SELECT * FROM EHECD_SystemUser WHERE ID IN ({0})", string.Join(",", builder)), null);

            List<string> sPhoneList = new List<string>();
            foreach (var item in list)
            {
                sPhoneList.Add("'" + item.sLoginName + "'");
            }
            var data = query.QueryList<EHECD_ApplyDTO>(string.Format("SELECT * FROM EHECD_Apply WHERE sMobileNum IN ({0}) AND iType=1", string.Join(",", sPhoneList)), null);


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
        /// 冻结、解冻
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int FreezeCheck(Dictionary<string, object> obj)
        {
            StringBuilder builder = new StringBuilder();
            EHECD_SystemUserDTO user = new EHECD_SystemUserDTO();
            string[] id = obj["ID"].ToString().Split(',');
            string[] tUserState = obj["tUserState"].ToString().Split(',');
            for (int i = 0; i < id.Length; i++) {
                user.ID = new Guid(id[i]);
                user.tUserState =(tUserState[i].ToByte() == 0 ? 1 : 0).ToByte();
                //return excute.UpdateSingle<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO(){ tUserState=user.tUserState},string.Format("where ID IN ({0})",user.ID));
                string sql=DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO() { tUserState = user.tUserState }, string.Format("where ID = '{0}'", user.ID));
                builder.Append(sql).Append(";");
            }
            return excute.ExcuteTransaction(builder.ToString());
            
        }
        /// <summary>
        /// 已通过页面绑定
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetPassList(PageInfo page, Dictionary<string, object> dic)
        {
            page.orderType = OrderType.DESC;
            page.OrderBy = "dCreateTime";
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT a.ID,a.sLoginName,a.tUserState,a.dCreateTime,b.sShopName,a.sMessage,b.sProvice,b.sCity,b.sCounty,b.sAddress FROM EHECD_SystemUser a, EHECD_ShopSet b WHERE a.bIsDeleted = 0 AND b.bIsDelete = 0 AND a.tUserType = 1 AND a.ID = b.sShopID AND( tUserState = 0 OR tUserState = 1)");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {

                sb.AppendFormat("and (b.sShopName like  '%{0}%')", dic["sKeyword"].ToString());
            }
            if (dic["tUserState"] != null && !string.IsNullOrEmpty(dic["tUserState"].ToString()))
            {
                sb.AppendFormat("and (a.tUserState like  '%{0}%')", dic["tUserState"].ToString());
            }
            if (dic["sProvice"] != null && !string.IsNullOrEmpty(dic["sProvice"].ToString()))
            {
                sb.AppendFormat("and (b.sProvice like  '%{0}%')", dic["sProvice"].ToString());
            }
            if (dic["sCity"] != null && !string.IsNullOrEmpty(dic["sCity"].ToString()))
            {
                sb.AppendFormat("and (b.sCity like  '%{0}%')", dic["sCity"].ToString());
            }
            if (dic["sCounty"] != null && !string.IsNullOrEmpty(dic["sCounty"].ToString()))
            {
                sb.AppendFormat("and (b.sCounty like  '%{0}%')", dic["sCounty"].ToString());
            }
            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
           
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public override int UpDatePassword(EHECD_SystemUserDTO dto)
        {
            var obj = MapperHelpper.Map<EHECD_SystemUserDTO>(dto);
            obj.sPassWord = Helper.Security.GetMD5Hash(dto.sPassWord.ToString());
            return excute.UpdateSingle<EHECD_SystemUserDTO>(obj, string.Format(" Where ID='{0}'", dto.ID));
        }
        /// <summary>
        /// 根据ID查看用户信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO GetUser(string ID)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select ID,sLoginName from EHECD_SystemUser WHERE bIsDeleted = 0 and ID=@ID", new { ID = ID });
        }
        /// <summary>
        /// 删除未审核的店铺
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public override int DeleteUnCheck(Dictionary<string, object> dic)
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

        public override EHECD_ApplyDTO ApplyUser(string ID)
        {
            return query.SingleQuery<EHECD_ApplyDTO>("SELECT * FROM EHECD_Apply WHERE ID=@ID",new { ID=ID});
        }
        /// <summary>
        /// 获取所有的合伙人的姓名，ID
        /// </summary>
        /// <returns></returns>
        public override string GetAllPartner()
        {
              var Partner=query.QueryList<EHECD_SystemUserDTO>("select ID,sUserName from EHECD_SystemUser where tUserState=0 and tUserType=2 and bIsDeleted=0", null);
              var data = from m in Partner
                       select new
                           {
                             id = m.ID,
                             text = m.sUserName
                       };
            return JSONHelper.GetJsonString(data);

        }
        /// <summary>
        /// 修改商家的合伙人
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public override int UdatePartner(EHECD_SystemUserDTO sys)
        {
            sys.dLastLoginTime = DateTime.Now;
            return excute.UpdateSingle<EHECD_SystemUserDTO>(sys, string.Format(" Where ID='{0}'", sys.ID));
        }
        /// <summary>
        /// 获取平台的短信数量
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO GetTerraceCount(EHECD_SystemUserDTO sys)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select ID,sMessage from EHECD_SystemUser where ID=@ID", new { ID=sys.ID});
        }
        /// <summary>
        /// 修改平台的短信数
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        public override int UpdateTerraceCount(EHECD_SystemUserDTO sys,EHECD_SystemUserDTO dto)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO() { sMessage = sys.sMessage }, string.Format("where ID IN ('{0}')", sys.ID)));
            builder.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO() { sMessage =dto.sMessage}, string.Format("where ID IN ('{0}')", dto.ID)));
            return excute.ExcuteTransaction(builder.ToString());
        }
        /// <summary>
        /// 合伙人页面数据绑定
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dic"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public override PagingRet<Dictionary<string, object>> GetPartnerShop(PageInfo page, Dictionary<string, object> dic,EHECD_SystemUserDTO user)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT a.ID,a.tUserState,a.dCreateTime,b.sShopName,
                               a.sMessage,b.sProvice,b.sCity,b.sCounty,b.sAddress 
                        FROM
                               EHECD_SystemUser a, EHECD_ShopSet b 
                        WHERE 
                              a.bIsDeleted = 0 AND b.bIsDelete = 0 AND a.tUserType = 1 AND a.ID = b.sShopID AND( a.tUserState = 0 OR a.tUserState = 1) and sPartnerID='"+user.ID+"'");
            if (dic["sKeyword"] != null && !string.IsNullOrEmpty(dic["sKeyword"].ToString()))
            {
                sb.AppendFormat("and (b.sShopName like '%{0}%')", dic["sKeyword"].ToString());
            }

            return query.PaginationQuery<Dictionary<string, object>>(sb.ToString(), page, null);
        }

        /// <summary>
        /// 寻找是否有相同的手机号
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>

        public override EHECD_SystemUserDTO TheSeamPhone(EHECD_SystemUserDTO user)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select * from EHECD_SystemUser   where bIsDeleted=0 and  sMobileNum='" + user.sMobileNum + "'",null);
          
        }
        /// <summary>
        /// 查找是否有相同的用户名
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override EHECD_SystemUserDTO TheSeamLoginName(EHECD_SystemUserDTO user)
        {
            return query.SingleQuery<EHECD_SystemUserDTO>("select * from EHECD_SystemUser   where bIsDeleted=0 and  sLoginName='" + user.sLoginName + "'", null);
        }
    }
}
