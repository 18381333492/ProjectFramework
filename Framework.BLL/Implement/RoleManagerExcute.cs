using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Dapper;
using Framework.DTO;
using Framework.Helper;
using System.Collections;

namespace Framework.BLL
{
    public partial class RoleManager : IRoleManager
    {                
        //添加角色
        public override int AddRole(dynamic data, dynamic p)
        {
            //1.创建角色对象
            EHECD_RoleDTO role = new EHECD_RoleDTO
            {
                ID = GuidHelper.GetSecuentialGuid(),
                bEnable = Convert.ToBoolean(data.bEnable.Value),
                bIsDeleted = false,
                dCreateTime = DateTime.Now,
                dModifyTime = DateTime.Now,
                iOrder = Convert.ToInt32(data.iOrder.Value),
                sRoleName = data.sRoleName.Value
            };

            var sqlIf = @"IF EXISTS(SELECT 1 FROM EHECD_Role WHERE sRoleName = @sRoleName AND bIsDeleted=0)
                        BEGIN
	                        SELECT -1 RET;
                        END
                        ELSE
                        BEGIN
	                        {0}
                        END;";

            sqlIf = string.Format(sqlIf, DBSqlHelper.GetInsertSQL<EHECD_RoleDTO>(role));

            //2.插入角色信息
            var ret = excute.Insert(sqlIf, new { sRoleName = role.sRoleName });

            //3.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.ADD | SYSTEM_LOG_TYPE.ROLE),
                "系统用户添加角色" + role.ID,
                role.ID.ToString(),
                ret > 0);

            return ret;
        }

        //编辑角色
        public override int EditRole(EHECD_RoleDTO role, dynamic p)
        {
            //1.完善角色信息
            role.dModifyTime = DateTime.Now;

            //2.更新角色信息
            var ret = excute.UpdateSingle<EHECD_RoleDTO>(role, string.Format("where ID = '{0}'", role.ID.ToString()));

            //3.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.MODIFY | SYSTEM_LOG_TYPE.ROLE),
                "系统用户编辑角色" + role.ID,
                role.ID.ToString(), ret > 0);

            return ret;
        }

        //删除角色
        public override int DeleteRole(string ID, dynamic p)
        {
            StringBuilder sb = new StringBuilder();

            //1.逻辑删除这个角色的所有特权
            sb.Append(DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(new EHECD_PrivilegeDTO { bIsDeleted = true }, string.Format("WHERE ((sPrivilegeMaster = 'role' AND sPrivilegeMasterValue = '{0}'/*类别是角色*/) OR (sBelong = 'role' AND sBelongValue = '{0}'/*所有者是角色*/) OR (sPrivilegeAccess = 'role' AND sPrivilegeAccessValue = '{0}'/*提供者是角色*/))", ID)));

            //2.逻辑删除这个角色
            sb.Append(DBSqlHelper.GetUpdateSQL<EHECD_RoleDTO>(new EHECD_RoleDTO { ID = Guid.Parse(ID), bIsDeleted = true }, String.Format("WHERE ID = '{0}'", ID)));

            //3.逻辑删除这个角色绑定的客户
            sb.Append(DBSqlHelper.GetUpdateSQL<EHECD_SystemUser_R_RoleDTO>(new EHECD_SystemUser_R_RoleDTO { sRoleID = Guid.Parse(ID), bIsDeleted = true }, String.Format("WHERE sRoleID = '{0}'", ID)));

            var ret = excute.ExcuteTransaction(sb.ToString());

            //4.记录系统日志
            InsertSystemLog(
                p.sLoginName.ToString(),
                p.sUserName.ToString(),
                p.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.ROLE),
                "系统用户删除角色" + ID,
                ID,
                ret > 0);

            return ret;
        }
        
        //给角色分配菜单
        public override int DistributionMenu(EHECD_RoleDTO role, dynamic dynamicData)
        {
            var ret = 0;
            var menusDynamic = dynamicData.data;
            if (menusDynamic.Count > 0)
            {
                //1.获取选中的菜单ID
                List<string> menus = JSONHelper.GetModel<List<string>>(menusDynamic.Parent.Value.ToString());

                //2.获取角色已有菜单ID
                var roleMenus = query.QueryList<EHECD_FunctionMenuDTO>(
                                                            @"SELECT
	                                                            sPrivilegeAccessValue ID
                                                            FROM
	                                                            EHECD_Privilege
                                                            WHERE
	                                                            sPrivilegeMaster = 'role'
                                                            AND sPrivilegeAccess = 'menu'
                                                            AND sBelong = 'role'
                                                            AND bIsDeleted = 0
                                                            AND bPrivilegeOperation = 0
                                                            AND sPrivilegeMasterValue = @ID;", new { ID = role.ID })
                                                            .Select(m => m.ID.ToString()).ToList();

                //3.选取选中的和已有的交集用以更新或插入
                var jm = menus.Union(roleMenus).ToList();

                //4.选取已有的和选中的差集用以解除没有选中的菜单权限
                var cm = roleMenus.Except(menus).ToList();

                StringBuilder sb = new StringBuilder();

                //分配权限
                sb.Append(CreateDistributionMenuSql(jm, role.ID.ToString()));

                //移除取消了得权限
                sb.Append(CreateUnDistributionMenuSql(cm, role.ID.ToString()));

                ret = excute.ExcuteTransaction(sb.ToString());
            }
            else
            {
                //既然一个都没选中，那就取消这个角色的所有权限
                ret = excute.Update("UPDATE EHECD_Privilege SET bIsDeleted = 1 WHERE sPrivilegeMaster = 'role' AND sPrivilegeMasterValue = @RID AND sBelong = 'role' AND sBelongValue = @RID AND sPrivilegeAccess = 'menu';", new { RID = role.ID });
            }

            //记录系统日志
            InsertSystemLog(
                dynamicData.sLoginName.ToString(),
                dynamicData.sUserName.ToString(),
                dynamicData.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.MODIFY | SYSTEM_LOG_TYPE.ADD | SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.ROLE | SYSTEM_LOG_TYPE.MENU),
                "系统用户分配角色菜单" + role.ID,
                role.ID.ToString(), ret > 0);

            return ret;
        }

        //给角色分配菜单按钮
        public override int DistributionMenuButton(EHECD_RoleDTO role, dynamic dynamicData)
        {
            var ret = 0;

            var btnData = dynamicData.data;

            if (btnData.Count > 0)
            {
                //1.选中的按钮的ID
                var bts = JSONHelper.GetModel<dynamic>(btnData.Parent.Value.ToString()).GetEnumerator();

                var btnsDy = new List<object>();

                while (bts.MoveNext())
                {
                    btnsDy.Add(new {
                        id = bts.Current.id.ToString(),
                        menuid = bts.Current.menuid.ToString()
                    });
                }

                //var btsid = bts.Select(m => m.id.Value.ToString()).ToList();

                //2.获取角色已有的按钮ID
                var roleBtnIds = query.QueryList<Dictionary<string,object>>(
                    @"
                        SELECT 
                            sPrivilegeAccessValue id,
                            sBelongValue menuid
                        FROM 
                            EHECD_Privilege 
                        WHERE sPrivilegeMaster = 'role' 
                        AND sBelong = 'menu' 
                        AND sPrivilegeAccess = 'button'
                        AND bIsDeleted = 0
                        AND sPrivilegeMasterValue = @ID;", new { ID = role.ID }
                    ).Select(m=>new {
                        id= m["id"].ToString(),
                        menuid = m["menuid"].ToString()
                    }).ToList();

                //3.获取交集用以更新和插入
                var jb = btnsDy.Union(roleBtnIds,new ButtonMenuRoleCompare()).ToList();

                ////4.获取差集用以解除没有选中的按钮权限
                var cb = roleBtnIds.Except(btnsDy, new ButtonMenuRoleCompare()).ToList();

                StringBuilder sb = new StringBuilder();

                //分配权限
                sb.Append(CreateDistributionButtonSql(jb, role.ID.ToString()));

                ////移除取消了得权限
                sb.Append(CreateUnDistributionButtonSql(cb, role.ID.ToString()));

                ret = excute.ExcuteTransaction(sb.ToString());
            }
            else
            {
                //没有选中只能认为是要取消所有按钮权限了
                ret = excute.Update("UPDATE EHECD_Privilege SET bIsDeleted = 1 WHERE sPrivilegeMaster = 'role' AND sPrivilegeMasterValue = @RID AND sBelong = 'role' AND sBelongValue = @RID AND sPrivilegeAccess = 'button';", new { RID = role.ID });
            }

            //记录系统日志
            InsertSystemLog(
                dynamicData.sLoginName.ToString(),
                dynamicData.sUserName.ToString(),
                dynamicData.IP.ToString(),
                (Int16)(SYSTEM_LOG_TYPE.MODIFY | SYSTEM_LOG_TYPE.ADD | SYSTEM_LOG_TYPE.DELETE | SYSTEM_LOG_TYPE.ROLE | SYSTEM_LOG_TYPE.BUTTON),
                "系统用户分配角色菜单按钮" + role.ID,
                role.ID.ToString(), ret > 0);

            return ret;
        }
        
        #region 分配菜单用到的私有方法
        /// <summary>
        /// 创建解除角色菜单的sql
        /// </summary>
        /// <param name="cm">要解除的菜单集合</param>
        /// <param name="v">要解除的角色</param>
        /// <returns>sql</returns>
        private string CreateUnDistributionMenuSql(List<string> cm, string v)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cm.Count; i++)
            {
                sb.AppendLine(
                        DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(
                             new EHECD_PrivilegeDTO { bIsDeleted = true },
                            string.Format(
                                    "WHERE sPrivilegeMaster = 'role' AND sBelong = 'role' AND sPrivilegeMasterValue = '{0}' AND sBelongValue = '{0}' AND sPrivilegeAccess = 'menu' AND sPrivilegeAccessValue = '{1}'",
                                    v, cm[i]
                                )
                            )
                    );
            }
            return sb.ToString();
        }

        /// <summary>
        /// 创建分配角色菜单的sql
        /// </summary>
        /// <param name="jm">要分配的菜单集合</param>
        /// <param name="roleId">要分配的角色</param>
        /// <returns>sql</returns>
        private string CreateDistributionMenuSql(List<string> jm, string roleId)
        {
            StringBuilder sb = new StringBuilder();

            //赋予角色菜单权限的sql
            string dsql =
                        @"IF EXISTS (
	                            SELECT
		                            1
	                            FROM
		                            EHECD_Privilege
	                            WHERE
		                            sPrivilegeMaster = 'role'
	                            AND sBelong = 'role'
	                            AND sPrivilegeMasterValue = '{0}'--角色ID
	                            AND sBelongValue = '{0}'--角色ID
	                            AND sPrivilegeAccess = 'menu'
	                            AND sPrivilegeAccessValue = '{1}'--菜单ID
                            )
                            BEGIN
	                            --存在直接更新
                                {2}
                            END
                            ELSE

                            BEGIN
	                            --不存在就插入一条
                                {3}
                            END;";

            for (int i = 0; i < jm.Count; i++)
            {
                sb.AppendFormat(dsql, roleId, jm[i],
                        DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(
                            new EHECD_PrivilegeDTO { bIsDeleted = false },
                            string.Format(
                                    "WHERE sPrivilegeMaster = 'role' AND sBelong = 'role' AND sPrivilegeMasterValue = '{0}' AND sBelongValue = '{0}' AND sPrivilegeAccess = 'menu' AND sPrivilegeAccessValue = '{1}'",
                                    roleId, jm[i]
                                )
                            ),
                        DBSqlHelper.GetInsertSQL<EHECD_PrivilegeDTO>(
                                new EHECD_PrivilegeDTO
                                {
                                    bIsDeleted = false,
                                    bPrivilegeOperation = false,
                                    ID = GuidHelper.GetSecuentialGuid(),
                                    sBelong = "role",
                                    sBelongValue = Guid.Parse(roleId),
                                    sPrivilegeAccess = "menu",
                                    sPrivilegeAccessValue = Guid.Parse(jm[i]),
                                    sPrivilegeMaster = "role",
                                    sPrivilegeMasterValue = Guid.Parse(roleId)
                                }
                            )
                    );
            }

            return sb.ToString();
        }

        /// <summary>
        /// 创建分配角色按钮权限的sql
        /// </summary>
        /// <param name="jb"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private string CreateDistributionButtonSql(List<dynamic> jb, string v)
        {
            StringBuilder sb = new StringBuilder();

            //赋予角色菜单按钮权限的sql
            string dsql =
                        @"IF EXISTS (
	                            SELECT
		                            1
	                            FROM
		                            EHECD_Privilege
	                            WHERE
		                            sPrivilegeMaster = 'role'--分配给角色的
                                AND sPrivilegeMasterValue = '{0}'--角色ID
                                AND sPrivilegeAccess = 'button'--特权类型是按钮
                                AND sPrivilegeAccessValue = '{1}'--按钮ID
	                            AND sBelong = 'menu'--属于菜单的	                            
	                            AND sBelongValue = '{4}'--菜单ID
                            )
                            BEGIN
	                            --存在直接更新
                                {2}
                            END
                            ELSE

                            BEGIN
	                            --不存在就插入一条
                                {3}
                            END;";

            for (int i = 0; i < jb.Count; i++)
            {
                sb.AppendFormat(dsql, v, jb[i].id.ToString(),
                        DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(
                            new EHECD_PrivilegeDTO { bIsDeleted = false },
                            string.Format(
                                    @"WHERE 
                                        sPrivilegeMaster = 'role'--分配给角色的
                                        AND sPrivilegeMasterValue = '{0}'--角色ID
                                        AND sPrivilegeAccess = 'button'--特权类型是按钮
                                        AND sPrivilegeAccessValue = '{1}'--按钮ID
                                        AND sBelong = 'menu'--属于菜单的
                                        AND sBelongValue = '{2}'--菜单ID",
                                    v, jb[i].id.ToString(), jb[i].menuid.ToString()
                                )
                            ),
                        DBSqlHelper.GetInsertSQL<EHECD_PrivilegeDTO>(
                                new EHECD_PrivilegeDTO
                                {
                                    bIsDeleted = false,
                                    bPrivilegeOperation = false,
                                    ID = GuidHelper.GetSecuentialGuid(),
                                    sBelong = "menu",
                                    sBelongValue = Guid.Parse(jb[i].menuid.ToString()),
                                    sPrivilegeAccess = "button",
                                    sPrivilegeAccessValue = Guid.Parse(jb[i].id.ToString()),
                                    sPrivilegeMaster = "role",
                                    sPrivilegeMasterValue = Guid.Parse(v)
                                }
                            ),jb[i].menuid.ToString()
                    );
            }
            return sb.ToString();
        }

        /// <summary>
        /// 创建解除角色菜单按钮权限的sql
        /// </summary>
        /// <param name="cb"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        private string CreateUnDistributionButtonSql(List<dynamic> cb, string v)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cb.Count; i++)
            {
                sb.AppendLine(
                        DBSqlHelper.GetUpdateSQL<EHECD_PrivilegeDTO>(
                             new EHECD_PrivilegeDTO { bIsDeleted = true },
                            string.Format(
                                    @"WHERE 
                                        sPrivilegeMaster = 'role' --特权分配给角色的
                                        AND sPrivilegeMasterValue = '{0}' --角色ID
                                        AND sBelong = 'menu' --属于菜单的
                                        AND sBelongValue = '{2}' --菜单ID
                                        AND sPrivilegeAccess = 'button' --特权类型是按钮
                                        AND sPrivilegeAccessValue = '{1}'--按钮ID",
                                    v, cb[i].id.ToString(),cb[i].menuid.ToString()
                                )
                            )
                    );
            }
            return sb.ToString();
        }
        #endregion        
    }    
}
