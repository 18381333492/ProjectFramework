using Framework.DTO;
using Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.web.config;

namespace Framework.BLL
{
    public class UserLogin : ILogin
    {
        //载入权限菜单
        public override UserRoleMenuInfo LoadUserRoleMenuInfo(EHECD_SystemUserDTO t)
        {
            //用户的权限和菜单、菜单按钮信息
            UserRoleMenuInfo ret = new UserRoleMenuInfo();

            //从配置中查看启不启用权限
            if (WebConfig.LoadElement("UseUserRole") == "1")
            {
                //获取用户和用户角色下的菜单
                ret.UserMenu = LoadUserAndRolesMenu(t.ID);

                //获取用户的角色
                ret.UserRole = query.QueryList<UserRole>(@"SELECT
	                                                            r.ID,
	                                                            r.sRoleName,
	                                                            r.dModifyTime,
	                                                            r.iOrder
                                                            FROM
	                                                            EHECD_SystemUser_R_Role srr,
	                                                            EHECD_Role r
                                                            WHERE
	                                                            srr.sRoleID = r.ID
                                                            AND r.bIsDeleted = 0
                                                            AND srr.bIsDeleted = 0
                                                            AND r.bEnable = 1
                                                            AND srr.sUserID = @id ORDER BY r.iOrder;", new { id = t.ID });

                //判断是否开启绑定到菜单按钮
                if (WebConfig.LoadElement("UseMenuBottn") == "1")
                {
                    //获取这个用户所有的角色ID
                    string userRoles = string.Join(",", ret.UserRole.Select(m => string.Concat("'", m.ID, "'")));

                    //获取用户和用户角色的菜单按钮
                    for (int i = 0; i < ret.UserMenu.Count; i++)
                    {
                        ret.UserMenu[i].Buttons = LoadUserAndRolesMenuButton(userRoles, ret.UserMenu[i].ID, t.ID);
                    }
                }
                else
                {
                    //如果不启用菜单按钮和角色与客户绑定的话，则获取每个菜单的按钮
                    for (int i = 0; i < ret.UserMenu.Count; i++)
                    {
                        ret.UserMenu[i].Buttons = query.QueryList<UserMenuButton>(@"SELECT
	                                                                                        *
                                                                                        FROM
	                                                                                        EHECD_MenuButton
                                                                                        WHERE
	                                                                                        ID IN (
		                                                                                        SELECT DISTINCT
			                                                                                        sPrivilegeAccessValue
		                                                                                        FROM
			                                                                                        EHECD_Privilege
		                                                                                        WHERE
			                                                                                        sBelong = 'menu'
		                                                                                        AND sBelongValue = @ID
                                                                                                AND sPrivilegeMaster = 'menu'
                                                                                                AND sPrivilegeMasterValue = @ID
		                                                                                        AND sPrivilegeAccess = 'button'
	                                                                                        )
                                                                                        AND bIsDeleted = 0
                                                                                        ORDER BY iOrder;", new { ID = ret.UserMenu[i].ID });
                    }
                }
                ret.AllMenu = ret.UserMenu;
                //初始化菜单使其具有层级关系   
                ret.UserMenu = InitMenu(ret.UserMenu);

                ret.LoadSuccess = true;
            }
            else
            {
                //如果不启用权限，就获取所有菜单
                ret.UserMenu = query.QueryList<UserMenu>("SELECT ID,sMenuName,sPID,sUrl,iOrder from EHECD_FunctionMenu WHERE bIsDeleted = 0 ORDER BY iOrder;", null);

                //判断是否开启菜单按钮配置：在不开启权限的情况下，这里的菜单按钮只获取绑定到个人身上的，不再获取绑定到权限的
                if (WebConfig.LoadElement("UseMenuBottn") == "1")
                {
                    //获取用户菜单按钮
                    for (int i = 0; i < ret.UserMenu.Count; i++)
                    {
                        ret.UserMenu[i].Buttons = LoadUserMenuButton(ret.UserMenu[i].ID, t.ID);
                    }
                }
                else
                {
                    //获取所有菜单按钮
                    for (int i = 0; i < ret.UserMenu.Count; i++)
                    {
                        ret.UserMenu[i].Buttons = query.QueryList<UserMenuButton>(@"SELECT
	                                                                                        *
                                                                                        FROM
	                                                                                        EHECD_MenuButton
                                                                                        WHERE
	                                                                                        ID IN (
		                                                                                        SELECT DISTINCT
			                                                                                        sPrivilegeAccessValue
		                                                                                        FROM
			                                                                                        EHECD_Privilege
		                                                                                        WHERE
			                                                                                        sBelong = 'menu'
		                                                                                        AND sBelongValue = @ID
                                                                                                AND sPrivilegeMaster = 'menu'
                                                                                                AND sPrivilegeMasterValue = @ID
		                                                                                        AND sPrivilegeAccess = 'button'
	                                                                                        )
                                                                                        AND bIsDeleted = 0
                                                                                        ORDER BY iOrder;", new { ID = ret.UserMenu[i].ID });
                    }
                }

                ret.AllMenu = ret.UserMenu;
                //初始化菜单使其具有层级关系   
                ret.UserMenu = InitMenu(ret.UserMenu);

                ret.LoadSuccess = true;
            }

            return ret;
        }

        #region 获取权限信息的方法

        //获取用户和用户角色下的菜单
        private IList<UserMenu> LoadUserAndRolesMenu(Guid? ID)
        {
            return query.QueryList<UserMenu>(@"SELECT * FROM EHECD_FunctionMenu WHERE ID IN(
                                                            SELECT
	                                                            a.sPrivilegeAccessValue
                                                            FROM
	                                                            EHECD_Privilege a
                                                            WHERE
	                                                            a.sPrivilegeMaster = 'role' /*指定特权所属对象是角色*/
                                                            AND a.sPrivilegeAccess = 'menu'/*指定特权类型是菜单特权*/
                                                            AND a.sPrivilegeMasterValue IN (
	                                                            SELECT
		                                                            x.sRoleID
	                                                            FROM
		                                                            EHECD_SystemUser_R_Role x,
                                                                    EHECD_Role y
	                                                            WHERE
																	x.sRoleID = y.ID
																AND y.bEnable = 1
																AND y.bIsDeleted = 0
		                                                        AND x.sUserID = @ID
	                                                            AND x.bIsDeleted = 0																															
                                                            )/*根据用户ID获取到他的角色以指定他的角色所拥有的特权*/
                                                            AND a.bIsDeleted = 0
                                                            AND a.bPrivilegeOperation = 0
                                                            UNION
	                                                            SELECT
		                                                            m.sPrivilegeAccessValue
	                                                            FROM
		                                                            EHECD_Privilege m,
																	EHECD_FunctionMenu n
	                                                            WHERE
		                                                            m.sPrivilegeMaster = 'user' /*指定特权所属对象是客户*/	
	                                                            AND m.sPrivilegeAccess = 'menu'/*指定特权类型是菜单特权*/
	                                                            AND m.sPrivilegeMasterValue = @ID/*指定要查找的特权所有者的ID*/
	                                                            AND m.bIsDeleted = 0
	                                                            AND m.bPrivilegeOperation = 0
																AND n.ID = m.sPrivilegeAccessValue
																AND n.bIsDeleted = 0
                                                            ) ORDER BY iOrder;", new { ID = ID });
        }

        //获取用户和用户角色的菜单按钮
        private IList<UserMenuButton> LoadUserAndRolesMenuButton(string userRoles, Guid? menuID, Guid? userID)
        {
            return query.QueryList<UserMenuButton>(string.Format(@"SELECT * from EHECD_MenuButton WHERE ID IN (
                                                                   SELECT
	                                                                   a.sPrivilegeAccessValue
                                                                   FROM
	                                                                   EHECD_Privilege a,
	                                                                   EHECD_MenuButton b
                                                                   WHERE
	                                                                   a.sPrivilegeAccessValue = b.ID
                                                                   AND b.bIsDeleted = 0
                                                                   AND a.sPrivilegeMaster = 'role' /*指定该特权属于角色特权*/
                                                                   AND a.sBelong = 'menu' /*指定该特权是属于菜单的*/
                                                                   AND a.sPrivilegeAccess = 'button' /*指定该特权类型是一个按钮特权*/
                                                                   AND a.sPrivilegeMasterValue IN (
	                                                                   {0}
                                                                   ) /*指定该特权所属的具体角色*/
                                                                   AND a.sBelongValue = @ID /*指定该特权属于哪个菜单*/
                                                                   AND a.bIsDeleted = 0
                                                                   AND a.bPrivilegeOperation = 0
                                                                   UNION
	                                                               SELECT
		                                                               a.sPrivilegeAccessValue
	                                                               FROM
		                                                               EHECD_Privilege a,
		                                                               EHECD_MenuButton b
	                                                               WHERE
		                                                               a.sPrivilegeAccessValue = b.ID
	                                                               AND b.bIsDeleted = 0
	                                                               AND a.sPrivilegeMaster = 'user' /*指定该特权属于用户特权*/
	                                                               AND a.sBelong = 'menu' /*指定该特权是属于菜单的*/
	                                                               AND a.sPrivilegeAccess = 'button' /*指定该特权类型是一个按钮特权*/
	                                                               AND a.sPrivilegeMasterValue = @userID /*指定该特权所属的用户*/
	                                                               AND a.sBelongValue = @ID /*指定该特权属于哪个菜单*/
	                                                               AND a.bIsDeleted = 0
	                                                               AND a.bPrivilegeOperation = 0) ORDER BY iOrder;", userRoles), new { ID = menuID, userID = userID });
        }

        //获取用户的菜单按钮
        private IList<UserMenuButton> LoadUserMenuButton(Guid? menuID, Guid? userID)
        {
            return query.QueryList<UserMenuButton>(string.Format(@"SELECT * from EHECD_MenuButton WHERE ID IN (
                                                                   SELECT
	                                                                   a.sPrivilegeAccessValue
                                                                   FROM
	                                                                   EHECD_Privilege a,
	                                                                   EHECD_MenuButton b
                                                                   WHERE
	                                                                   a.sPrivilegeAccessValue = b.ID
                                                                   AND b.bIsDeleted = 0
                                                                   AND a.sPrivilegeMaster = 'user' /*指定该特权属于角色特权*/
                                                                   AND a.sBelong = 'menu' /*指定该特权是属于菜单的*/
                                                                   AND a.sPrivilegeAccess = 'button' /*指定该特权类型是一个按钮特权*/
                                                                   AND a.sPrivilegeMasterValue = @userID /*指定该特权所属的用户*/
                                                                   AND a.sBelongValue = @menuID /*指定该特权属于哪个菜单*/
                                                                   AND a.bIsDeleted = 0
                                                                   AND a.bPrivilegeOperation = 0) ORDER BY iOrder;"), new { menuID = menuID, userID = userID });
        }

        #endregion

        /// <summary>
        /// 初始化菜单，返回一个带层级关系的菜单集合
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private IList<UserMenu> InitMenu(IList<UserMenu> t)
        {
            var userMs = new List<UserMenu>();

            //初始化菜单层级关系
            foreach (var item in t)
            {
                //顶层菜单
                if (item.sPID == null)
                {
                    LoadLevelUserMenu(t, item);
                    userMs.Add(item);
                }
            }
            return userMs.OrderBy(m => m.iOrder).ToList();
        }

        /// <summary>
        /// 递归从集合中载入指定菜单的下级菜单，直到没有下级菜单
        /// </summary>
        /// <param name="t"></param>
        /// <param name="m"></param>
        private void LoadLevelUserMenu(IList<UserMenu> t, UserMenu m)
        {
            Parallel.For(0, t.Count, index =>
            {
                var temp = t[index];
                if (m.ID == temp.sPID)
                {
                    m.ChildMenu.Add(temp);
                    LoadLevelUserMenu(t, temp);
                }
            });
            m.ChildMenu = m.ChildMenu != null ? m.ChildMenu.OrderBy(x => x.iOrder).ToList() : new List<UserMenu>();
        }

        //后台登录
        public override EHECD_SystemUserDTO Login(EHECD_SystemUserDTO t)
        {
            var IP = t.sAddress;

            //1.查询用户数据
            t = query.SingleQuery<EHECD_SystemUserDTO>("SELECT ID,sLoginName,sUserName,tUserState,tUserType,sUserNickName,dLastLoginTime,sProvice,sCity,sCounty,sAddress,tSex,sMobileNum FROM EHECD_SystemUser WHERE sLoginName = @name and sPassWord = @pwd AND bIsDeleted = 0;", new { name = t.sLoginName, pwd = Security.GetMD5Hash(t.sPassWord) });

            if (t != null)
                //2.记录系统日志
                InsertSystemLog(
                    t.sLoginName,
                    t.sUserName == null ? "用户" : t.sUserName,
                    IP,
                    (Int16)(SYSTEM_LOG_TYPE.LOGON | SYSTEM_LOG_TYPE.SYSTEMUSER),
                    "系统用户登录",
                    t.ID.ToString() == null ? "" : t.ID.ToString(),
                    t != default(EHECD_SystemUserDTO) && t.tUserState == 0);

            if (t != default(EHECD_SystemUserDTO))
            {
                //3.更新最后登录时间
                excute.ExcuteTransaction(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_SystemUserDTO>(new EHECD_SystemUserDTO { dLastLoginTime = DateTime.Now }, string.Format("where ID = '{0}'", t.ID.ToString())));
                //登录成功                
                return t;
            }
            else
            {
                return null;
            }
        }
    }
}
