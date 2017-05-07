using Framework.Dapper;
using Framework.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class RoleManager: IRoleManager
    {
        //载入角色
        public override PagingRet<EHECD_RoleDTO> LoadRoles(dynamic where)
        {
            PageInfo pageinfo = new PageInfo
            {
                OrderBy = "iOrder",
                orderType = OrderType.ASC,
                PageIndex = Convert.ToInt32(where.pageNumber.Value),
                PageSize = Convert.ToInt32(where.pageSize.Value)
            };
            return query.PaginationQuery<EHECD_RoleDTO>("select * from EHECD_Role where bIsDeleted = 0 and sRoleName like @sRoleName", pageinfo, new { sRoleName = "%" + where.sRoleName.Value + "%" });
        }

        //载入所有角色
        public override IList<EHECD_RoleDTO> LoadAllRoles()
        {
            return query.QueryList<EHECD_RoleDTO>("SELECT ID,sRoleName FROM EHECD_Role WHERE bIsDeleted = 0 AND bEnable = 1", null);
        }

        //载入用户权限
        public override IList<EHECD_RoleDTO> LoadUserRole(EHECD_SystemUserDTO user)
        {
            return query.QueryList<EHECD_RoleDTO>("SELECT r.ID FROM EHECD_Role r,EHECD_SystemUser_R_Role srr WHERE r.bIsDeleted = 0 AND r.bEnable = 1 AND r.ID = srr.sRoleID AND srr.bIsDeleted = 0 AND srr.sUserID = @ID", new { ID = user.ID.ToString() });
        }

        //载入分配角色菜单的tree数据
        public override dynamic LoadDistributionMenu(EHECD_RoleDTO role)
        {
            try
            {
                //1.获取所有菜单
                var allMenus = query.QueryList<UserMenu>("SELECT * FROM EHECD_FunctionMenu WHERE bIsDeleted = 0;", null);

                //2.获取角色的菜单
                var roleMenus = query.QueryList<UserMenu>(/*分配给角色的菜单特权*/
                                                            @"SELECT
	                                                            sPrivilegeAccessValue ID
                                                            FROM
	                                                            EHECD_Privilege
                                                            WHERE
	                                                            sPrivilegeMaster = 'role'
                                                            AND sPrivilegeAccess = 'menu'
                                                            AND sBelong = 'role'
                                                            AND bIsDeleted = 0 --未删除的
                                                            AND bPrivilegeOperation = 0 --未禁用的
                                                            AND sPrivilegeMasterValue = @ID;", new { ID = role.ID });


                //载入用户菜单层级关系
                for (int i = 0; i < allMenus.Count; i++)
                {
                    if (allMenus[i].sPID == null)
                    {
                        allMenus[i].ChildMenu = LoadMenuData(allMenus, allMenus[i]).OrderBy(m => m.iOrder).ToList();
                    }
                }

                //排个序
                allMenus = allMenus.OrderBy(x => x.iOrder).ToList();

                //创建菜单数据
                var ret = allMenus.Where(m => m.sPID == null).Select(o => new
                {
                    id = o.ID,
                    text = o.sMenuName,
                    children = CreateChidrenMenuTreeData(o.ChildMenu, roleMenus)
                }).ToList();

                return JSONHelper.GetJsonString<dynamic>(ret);
            }
            catch (Exception)
            {
                return null;
            }
        }

        //载入分配菜单按钮的tree数据
        public override dynamic LoadDistributionMenuButton(EHECD_RoleDTO role)
        {
            try
            {
                #region 1.获取角色的菜单
                //1.获取角色的菜单
                var roleMenus = query.QueryList<UserMenu>(
                                                            @"WITH CTE (ID) AS (
	                                                            SELECT
		                                                            sPrivilegeAccessValue ID
	                                                            FROM
		                                                            EHECD_Privilege
	                                                            WHERE
		                                                            sPrivilegeMaster = 'role'
	                                                            AND sPrivilegeAccess = 'menu'
	                                                            AND sBelong = 'role'
	                                                            AND bIsDeleted = 0
	                                                            AND bPrivilegeOperation = 0
	                                                            AND sPrivilegeMasterValue = @ID
                                                            )SELECT A.ID,A.sMenuName,A.sPID,A.iOrder FROM EHECD_FunctionMenu A,CTE WHERE CTE.ID = A.ID;", new { ID = role.ID });
                #endregion

                #region 2.获取角色菜单按钮
                //2.获取角色菜单按钮
                var roleMenuButtons = query.QueryList<UserMenuButton>(
                    @"WITH CTE (ID) AS (
	                    SELECT
		                    sPrivilegeAccessValue ID
	                    FROM
		                    EHECD_Privilege
	                    WHERE
		                    sPrivilegeMaster = 'role'
	                    AND sPrivilegeAccess = 'button'
	                    AND sBelong = 'menu'
	                    AND bIsDeleted = 0
	                    AND bPrivilegeOperation = 0
	                    AND sPrivilegeMasterValue = @ID
                    ) SELECT
	                    A.ID,
	                    A.sButtonName
                    FROM
	                    EHECD_MenuButton A,
	                    CTE
                    WHERE
	                    CTE.ID = A.ID;", new { ID = role.ID.ToString() }
                    );
                #endregion

                #region 3.获取所有菜单的按钮
                //4.获取所有菜单的按钮
                var allMenuButtons = query.QueryList<Dictionary<string, object>>(
                    @"WITH CTE (ID,sBelongValue) AS (
	                    SELECT
		                    sPrivilegeAccessValue ID,--按钮ID
                            sBelongValue
	                    FROM
		                    EHECD_Privilege
	                    WHERE
		                    sPrivilegeMaster = 'menu'
	                    AND sPrivilegeAccess = 'button'
	                    AND sBelong = 'menu'
	                    AND bIsDeleted = 0
	                    AND bPrivilegeOperation = 0	                    
                    ) SELECT
	                    A.ID,
	                    A.sButtonName,
                        A.iOrder,
                        A.sIcon,
                        CTE.sBelongValue sDataID--临时用sDataID装一下
                    FROM
	                    EHECD_MenuButton A,
	                    CTE
                    WHERE
	                    CTE.ID = A.ID;", null
                    );
                #endregion

                #region 4.整理菜单的层级关系
                //5.整理菜单的层级关系
                for (int i = 0; i < roleMenus.Count; i++)
                {
                    //添加按钮
                    for (int j = 0; j < allMenuButtons.Count; j++)
                    {
                        if (roleMenus[i].ID.ToString() == allMenuButtons[j]["sDataID"].ToString())
                        {
                            roleMenus[i].Buttons.Add(new UserMenuButton
                            {
                                ID = Guid.Parse(allMenuButtons[j]["ID"].ToString()),
                                sButtonName = allMenuButtons[j]["sButtonName"].ToString(),
                                iOrder = allMenuButtons[j]["iOrder"].ToInt32(),
                                sIcon = allMenuButtons[j]["sIcon"].ToString()
                            });
                        }
                    }
                    //整理菜单层级关系
                    if (roleMenus[i].sPID == null)
                    {
                        roleMenus[i].ChildMenu = LoadMenuData(roleMenus, roleMenus[i]).OrderBy(m => m.iOrder).ToList();
                    }
                }
                #endregion

                #region 5.创建菜单的树结构数据
                var ret = roleMenus.Where(m => m.sPID == null).Select(o => new
                {
                    id = o.ID,
                    text = o.sMenuName,
                    @checked = false,
                    attributes = new { isLeaf = false },
                    children = CreateChidrenMenuButtonTreeData(o.ChildMenu, roleMenuButtons)
                }).ToList();
                #endregion

                return JSONHelper.GetJsonString(ret);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #region 组装菜单数据的私有方法

        //创建子菜单的树结构数据
        private List<dynamic> CreateChidrenMenuTreeData(IList<UserMenu> childMenu, IList<UserMenu> roleMenus)
        {
            var ret = new List<dynamic>();
            for (int i = 0; i < childMenu.Count; i++)
            {
                ret.Add(new
                {
                    id = childMenu[i].ID,
                    text = childMenu[i].sMenuName,
                    @checked = roleMenus.Count > 0 ? roleMenus.Where(m => m.ID == childMenu[i].ID).FirstOrDefault() != default(UserMenu) : false,
                    children = CreateChidrenMenuTreeData(childMenu[i].ChildMenu, roleMenus)
                });
            }
            return ret;
        }

        //载入菜单的层级关系
        private List<UserMenu> LoadMenuData(IList<UserMenu> menu, UserMenu parent)
        {
            var ret = new List<UserMenu>();
            for (int i = 0; i < menu.Count; i++)
            {
                if (parent.ID == menu[i].sPID)
                {
                    //parent.ChildMenu
                    ret.Add(menu[i]);
                    menu[i].ChildMenu = LoadMenuData(menu, menu[i]);
                }
            }
            return ret;
        }

        //创建菜单的树结构数据(分配按钮用)
        private List<dynamic> CreateChidrenMenuButtonTreeData(IList<UserMenu> childMenu, IList<UserMenuButton> roleMenuButtons)
        {
            var ret/*结果集*/ = new List<dynamic>();
            var buttonCompare/*自定义对比器*/ = new ButtonsCompare();

            for (int i = 0; i < childMenu.Count; i++)
            {
                //如果还有子菜单就创建子菜单，作为父菜单是没有菜单按钮的
                if (childMenu[i].ChildMenu != null && childMenu[i].ChildMenu.Count > 0)
                {
                    ret.Add(new
                    {
                        id = childMenu[i].ID,
                        attributes = new { isLeaf = false },
                        text = childMenu[i].sMenuName,
                        @checked = false,
                        children = CreateChidrenMenuButtonTreeData(childMenu[i].ChildMenu, roleMenuButtons)
                    });
                }
                //没有子菜单就创建按钮
                else
                {
                    if (childMenu[i].Buttons != null && childMenu[i].Buttons.Count > 0)
                    {
                        ret.Add(new
                        {
                            id = childMenu[i].ID,
                            text = childMenu[i].sMenuName,
                            attributes = new { isLeaf = false },
                            @checked = false,
                            children = childMenu[i].Buttons.Select(m =>
                            {
                                var btn = new
                                {
                                    id = m.ID,
                                    text = m.sButtonName,
                                    iconCls = m.sIcon,
                                    attributes = new { isLeaf = true ,munuid = childMenu[i].ID },
                                    @checked = roleMenuButtons != null && roleMenuButtons.Count > 0 ? roleMenuButtons.Contains(m, buttonCompare) : false,
                                };
                                return btn;
                            }).ToList()
                        });
                    }
                }
            }
            return ret;
        }

        #endregion
    }
}
