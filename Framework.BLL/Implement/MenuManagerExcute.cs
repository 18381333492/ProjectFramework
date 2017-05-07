using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DTO;
using Framework.Helper;

namespace Framework.BLL
{
    public partial class MenuManager : IMenuManager
    {
        //添加菜单
        public override EHECD_FunctionMenuDTO AddMenu(EHECD_FunctionMenuDTO dto, dynamic p)
        {
            //1.完善菜单信息
            dto.bIsDeleted = false;
            dto.ID = Helper.GuidHelper.GetSecuentialGuid();

            //2.添加菜单
            var ret = excute.InsertSingle<EHECD_FunctionMenuDTO>(dto);

            //3.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.ADD | SYSTEM_LOG_TYPE.MENU),
                "系统用户创建菜单" + dto.sMenuName, dto.ID.ToString(),
                ret > 0);

            return ret > 0 ? dto : null;
        }

        //添加按钮
        public override EHECD_MenuButtonDTO AddButton(EHECD_MenuButtonDTO dto, string menuID, dynamic p)
        {
            //1.完善按钮信息
            dto.bIsDeleted = false;
            dto.ID = Helper.GuidHelper.GetSecuentialGuid();

            //2.创建按钮和菜单的特权信息
            EHECD_PrivilegeDTO pri = new EHECD_PrivilegeDTO
            {
                bIsDeleted = false,
                bPrivilegeOperation = false,
                ID = GuidHelper.GetSecuentialGuid(),
                sBelong = "menu",
                sBelongValue = Guid.Parse(menuID),
                sPrivilegeAccess = "button",
                sPrivilegeAccessValue = dto.ID,
                sPrivilegeMaster = "menu",
                sPrivilegeMasterValue = Guid.Parse(menuID)
            };

            StringBuilder sb = new StringBuilder();

            sb.Append(/*插入按钮SQL*/Framework.Dapper.DBSqlHelper.GetInsertSQL<EHECD_MenuButtonDTO>(dto));
            sb.Append(/*插入对应按钮菜单特权SQL*/Framework.Dapper.DBSqlHelper.GetInsertSQL<EHECD_PrivilegeDTO>(pri));

            var ret = excute.ExcuteTransaction(sb.ToString());

            //3.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.ADD | SYSTEM_LOG_TYPE.BUTTON),
                "系统用户在菜单" + menuID + "下添加按钮" + dto.sButtonName,
                string.Concat(dto.ID, ",", menuID),
                ret > 0);

            return ret > 0 ? dto : null;
        }

        //物理删除逻辑删除的数据
        public override bool DelDeletedData(dynamic dynamicData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DELETE EHECD_FunctionMenu WHERE bIsDeleted = 1;");
            sb.AppendLine("DELETE EHECD_MenuButton WHERE bIsDeleted = 1;");
            sb.AppendLine("DELETE EHECD_Privilege WHERE bIsDeleted = 1;");
            sb.AppendLine("DELETE EHECD_Role WHERE bIsDeleted = 1;");
            sb.AppendLine("DELETE EHECD_SystemUser WHERE bIsDeleted = 1;");
            sb.AppendLine("DELETE EHECD_SystemUser_R_Role WHERE bIsDeleted = 1;");

            var ret = excute.ExcuteTransaction(sb.ToString()) >= 0;

            InsertSystemLog(
                dynamicData.sLoginName.ToString(),
                dynamicData.sUserName.ToString(),
                dynamicData.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.BUTTON | SYSTEM_LOG_TYPE.MENU | SYSTEM_LOG_TYPE.ROLE | SYSTEM_LOG_TYPE.SYSTEMUSER),
                "系统用户清除了权限系统的逻辑删除数据",
                "",
                ret);

            return ret;
        }

        //删除菜单
        public override int DeleteMenu(EHECD_FunctionMenuDTO menu, dynamic p)
        {
            StringBuilder sb = new StringBuilder();

            //1.查询当前菜单的层级关系（它和它的所有下级菜单的ID）            
            string withCTE = string.Format(@"WITH CTE (ID, sPID) AS (
	                                                        SELECT
		                                                        ID,
		                                                        sPID
	                                                        FROM
		                                                        EHECD_FunctionMenu
	                                                        WHERE
		                                                        ID = @ID
	                                                        AND	bIsDeleted = 0
	                                                        UNION ALL
		                                                        SELECT
			                                                        B.ID,
			                                                        B.sPID
		                                                        FROM
			                                                        CTE,
			                                                        EHECD_FunctionMenu B
		                                                        WHERE
			                                                        CTE.ID = B.sPID
		                                                        AND B.bIsDeleted = 0
                                                        ) SELECT
	                                                        CTE.ID
                                                        FROM
	                                                        CTE;", menu.ID);

            var ids = query.QueryList<Dictionary<string, object>>(withCTE, new { ID = menu.ID.ToString() }).Select(m => m["ID"].ToString()).ToList();

            foreach (var menuId in ids)
            {
                //2.删除菜单数据
                //注释的代码是物理删除：sb.Append(Dapper.DBSqlHelper.GetDeleteSQL<EHECD_FunctionMenuDTO>(menu, string.Format("where ID = '{0}'", item)));
                sb.Append(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_FunctionMenuDTO>(new EHECD_FunctionMenuDTO { bIsDeleted = true }, string.Format("where ID = '{0}'", menuId)));

                //3.找到菜单的按钮信息
                var menus = query.QueryList<EHECD_MenuButtonDTO>(@"WITH CTE AS (
	                                                                            SELECT
		                                                                            sPrivilegeAccessValue
	                                                                            FROM
		                                                                            [EHECD_Privilege]
	                                                                            WHERE
		                                                                            sPrivilegeMaster = 'menu'
	                                                                            AND sPrivilegeAccess = 'button'
	                                                                            AND sBelong = 'menu'
	                                                                            AND sBelongValue = @ID
                                                                            ) SELECT
	                                                                            EHECD_MenuButton.ID
                                                                            FROM
	                                                                            EHECD_MenuButton,
	                                                                            CTE
                                                                            WHERE
	                                                                            ID = CTE.sPrivilegeAccessValue;", new { ID = menuId });

                foreach (var button in menus)
                {
                    //4.删除对应的菜单按钮
                    //注释的代码是物理删除：sb.Append(Dapper.DBSqlHelper.GetDeleteSQL<EHECD_MenuButtonDTO>(button, string.Format("where ID = '{0}'", button.ID.ToString())));
                    sb.Append(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_MenuButtonDTO>(new EHECD_MenuButtonDTO { bIsDeleted = true }, string.Format("where ID = '{0}'", button.ID.ToString())));

                    //5.删除对应这个按钮在特权表中分发给其他所有者的特权信息（如分发给角色和指定用户的按钮特权）
                    //注释的代码是物理删除：sb.Append(Dapper.DBSqlHelper.GetDeleteSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO(), string.Format("where ((sPrivilegeMaster = 'role' AND sBelong = 'role'/*分发给角色的*/) or (sPrivilegeMaster = 'user' AND sBelong = 'user'/*分发给用户的*/)) AND sPrivilegeAccess = 'button' and sPrivilegeAccessValue = '{0}'", button.ID.ToString())));
                    sb.Append(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO { bIsDeleted = true }, string.Format("where ((sPrivilegeMaster = 'role' AND sBelong = 'role'/*分发给角色的*/) or (sPrivilegeMaster = 'user' AND sBelong = 'user'/*分发给用户的*/)) AND sPrivilegeAccess = 'button' and sPrivilegeAccessValue = '{0}'", button.ID.ToString())));
                }

                //6.解除菜单对应的特权信息
                //注释的代码是物理删除：sb.Append(Dapper.DBSqlHelper.GetDeleteSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO(), string.Format("where ((sPrivilegeMaster = 'menu' AND sPrivilegeMasterValue = '{0}') or (sPrivilegeAccess = 'menu' and sPrivilegeAccessValue = '{0}') or (sBelong = 'menu' and sBelongValue = '{0}'))", item)));
                sb.Append(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO { bIsDeleted = true }, string.Format("where ((sPrivilegeMaster = 'menu' AND sPrivilegeMasterValue = '{0}') or (sPrivilegeAccess = 'menu' and sPrivilegeAccessValue = '{0}') or (sBelong = 'menu' and sBelongValue = '{0}'))", menuId)));
            }

            //执行删除
            var ret = excute.ExcuteTransaction(sb.ToString());

            //7.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.MENU),
                "系统用户删除菜单" + menu.ID.ToString(),
                menu.ID.ToString(),
                ret > 0);

            return ret;
        }

        //删除菜单按钮
        public override int DeleteMenuButton(EHECD_MenuButtonDTO btn, dynamic p)
        {
            StringBuilder sb = new StringBuilder();

            //1.删除当前菜单按钮
            sb.AppendLine(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_MenuButtonDTO>(btn, string.Format("where ID = '{0}'", btn.ID.ToString())));

            //2.解除这个按钮的所有特权信息
            sb.AppendLine(Dapper.DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO { bIsDeleted = true }, string.Format("where sPrivilegeAccessValue = '{0}' AND sPrivilegeAccess = 'button'", btn.ID.ToString())));

            var ret = excute.ExcuteTransaction(sb.ToString());

            //3.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.BUTTON),
                "系统用户删除菜单按钮" + btn.ID,
                btn.ID.ToString(),
                ret > 0);

            return ret;
        }

        //更新按钮
        public override EHECD_MenuButtonDTO EditButton(EHECD_MenuButtonDTO dto, dynamic p)
        {
            //1.更新按钮信息
            var ret = excute.UpdateSingle<EHECD_MenuButtonDTO>(dto, string.Format("WHERE [ID] = '{0}'", dto.ID.ToString()));

            //2.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.MODIFY | SYSTEM_LOG_TYPE.BUTTON),
                "系统用户更新菜单按钮" + dto.ID,
                dto.ID.ToString(),
                ret > 0);

            return ret > 0 ? dto : null;
        }

        //更新菜单
        public override EHECD_FunctionMenuDTO EditMenu(EHECD_FunctionMenuDTO menu, dynamic p)
        {
            //1.更新菜单信息
            var ret = excute.UpdateSingle<EHECD_FunctionMenuDTO>(menu, string.Format("WHERE [ID] = '{0}'", menu.ID.ToString()));

            //2.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.MODIFY | SYSTEM_LOG_TYPE.MENU),
                "系统用户更新菜单" + menu.ID,
                menu.ID.ToString(),
                ret > 0);

            return ret > 0 ? menu : null;
        }
    }
}
