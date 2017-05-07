using Framework.BLL;
using Framework.DTO;
using Framework.Validate;
using Framework.web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Helper;

namespace Framework.web.Areas.Admin.Controllers
{
    public class MenuManageController : SuperController
    {
        // GET: Admin/MenuManage
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 载入菜单
        /// </summary>
        public void LoadAllMenu()
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            result.Succeeded = userRoleMenu != null ? true : false;

            //开始组装菜单的数据
            result.Data = userRoleMenu != null ? CreateMenuData(userRoleMenu.AllMenu) : new object();
        }

        #region 跳转的Action，用于获取部分视图
        /// <summary>
        /// 获取添加菜单的页面
        /// </summary>
        public PartialViewResult ToAddMenu()
        {
            return PartialView("AddMenu");
        }

        /// <summary>
        /// 获取添加按钮的页面
        /// </summary>        
        public PartialViewResult ToAddButton()
        {
            return PartialView("AddButton");
        }

        /// <summary>
        /// 获取编辑按钮的页面
        /// </summary> 
        public PartialViewResult ToEditButton(string id)
        {
            UserMenuButton btn = LoadMenuButtonById(id);
            return PartialView("EditButton", btn);
        }

        /// <summary>
        /// 获取编辑菜单的页面
        /// </summary>
        public PartialViewResult ToEditMenu(string id)
        {
            UserMenu menu = LoadMenuById(id);
            return PartialView("EditMenu", menu);
        }
        #endregion

        #region 操作方法（编辑、添加）

        /// <summary>
        ///物理删除逻辑删除的数据
        /// </summary>
        public void DelDeletedData()
        {
            CreateSyslogInfo();
            result.Succeeded = DI.DIEntity.GetInstance().GetImpl<IMenuManager>().DelDeletedData(RequestParameters.dynamicData);
            result.Msg = result.Succeeded ? "" : "清除失败，请联系系统管理员";
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        public void AddMenu()
        {
            EHECD_FunctionMenuDTO menu/*要添加的菜单*/ = JSONHelper.GetModel<EHECD_FunctionMenuDTO>(RequestParameters.data.ToString());

            //菜单业务对象
            IMenuManager menubll = DI.DIEntity.GetInstance().GetImpl<IMenuManager>();

            CreateSyslogInfo();
            //添加菜单
            var ret = menubll.AddMenu(menu/*要添加的菜单*/, RequestParameters.dynamicData);

            if (ret != null)
            {
                //返回给页面添加好的菜单对象（tree使用的节点）
                result.Data = new
                {
                    id = ret.ID,
                    text = ret.sMenuName,
                    state = "closed",
                    @checked = false,
                    attributes = new { type = "menu", url = ret.sUrl, order = ret.iOrder },
                    children = new object[0]
                };

                //从session获取用户的权限和菜单等信息
                var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;

                if (userRoleMenu != null)
                {
                    //更新添加的菜单到session缓存
                    userRoleMenu.AllMenu.Add(new UserMenu
                    {
                        Buttons = new List<UserMenuButton>(),
                        ChildMenu = new List<UserMenu>(),
                        ID = ret.ID,
                        iOrder = ret.iOrder,
                        sMenuName = ret.sMenuName,
                        sPID = ret.sPID,
                        sUrl = ret.sUrl
                    });
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "会话菜单缓存获取失败";
                    return;
                }

                //重新获取菜单结构
                userRoleMenu.UserMenu = InitMenu(userRoleMenu.AllMenu);
                SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "添加菜单失败，请联系管理员";
            }
        }

        /// <summary>
        /// 添加按钮
        /// </summary>
        public void AddButton()
        {
            var requestData = JSONHelper.GetModel<Dictionary<string, object>>(RequestParameters.dataStr);
            EHECD_MenuButtonDTO addbutton = JSONHelper.GetModel<EHECD_MenuButtonDTO>(requestData["btn"].ToString());
            string menuID = requestData["menuID"].ToString();
            //菜单业务对象
            IMenuManager menubll = DI.DIEntity.GetInstance().GetImpl<IMenuManager>();
            CreateSyslogInfo();
            addbutton = menubll.AddButton(addbutton, menuID, RequestParameters.dynamicData);

            if (addbutton != null)
            {
                //从session获取用户的权限和菜单等信息
                var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;

                if (userRoleMenu != null)
                {
                    //构建返回给界面的节点信息
                    result.Data = new
                    {
                        id = addbutton.ID,
                        attributes = new { type = "btn" },
                        text = addbutton.sButtonName,
                        iconCls = addbutton.sIcon
                    };

                    //从会话用户菜单中找到这个按钮所属的按钮并将其添加进去
                    Parallel.For(0, userRoleMenu.AllMenu.Count, (index, state) =>
                    {
                        if (userRoleMenu.AllMenu[index].ID.ToString() == menuID)
                        {
                            userRoleMenu.AllMenu[index].Buttons.Add(new UserMenuButton
                            {
                                ID = addbutton.ID,
                                iOrder = addbutton.iOrder,
                                sButtonName = addbutton.sButtonName,
                                sDataID = addbutton.sDataID,
                                sIcon = addbutton.sIcon
                            });
                            state.Stop();
                            return;
                        }
                    });

                    //重新获取菜单结构
                    userRoleMenu.UserMenu = InitMenu(userRoleMenu.AllMenu);
                    SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
                    result.Succeeded = true;
                }
                else
                {
                    result.Succeeded = false;
                    result.Msg = "会话菜单缓存获取失败";
                    return;
                }
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "添加菜单按钮失败，请联系管理员";
            }
        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        public void EditButton()
        {
            EHECD_MenuButtonDTO editbutton = JSONHelper.GetModel<EHECD_MenuButtonDTO>(RequestParameters.dataStr);
            CreateSyslogInfo();
            editbutton = DI.DIEntity.GetInstance().GetImpl<IMenuManager>().EditButton(editbutton, RequestParameters.dynamicData);

            if (editbutton != null)
            {
                var ret = new UserMenuButton
                {
                    ID = editbutton.ID,
                    iOrder = editbutton.iOrder,
                    sButtonName = editbutton.sButtonName,
                    sDataID = editbutton.sDataID,
                    sIcon = editbutton.sIcon
                };
                result.Data/*构建返回的按钮节点数据*/ = new
                {
                    id = ret.ID,
                    @checked = true,
                    attributes = new { type = "btn" },
                    text = ret.sButtonName,
                    iconCls = ret.sIcon,
                    state = "closed"
                };
                UpdateSessionMenuButton(ret/*更新用户会话信息中的按钮信息*/);
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "编辑菜单按钮失败，请联系管理员";
            }
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        public void EditMenu()
        {
            EHECD_FunctionMenuDTO menu = LoadParam<EHECD_FunctionMenuDTO>();
            CreateSyslogInfo();
            var editmenu = LoadInterface<IMenuManager>().EditMenu(menu, RequestParameters.dynamicData);
            if (editmenu != null)
            {
                result.Data = new
                {
                    id = editmenu.ID,
                    attributes = new { type = "menu", url = editmenu.sUrl },
                    text = editmenu.sMenuName
                };
                result.Succeeded = true;
                UpdateSessionMenu(editmenu);
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "编辑菜单失败，请联系管理员";
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        public void DeleteMenu()
        {
            EHECD_FunctionMenuDTO menu = LoadParam<EHECD_FunctionMenuDTO>();
            CreateSyslogInfo();
            var editmenu = LoadInterface<IMenuManager>().DeleteMenu(menu, RequestParameters.dynamicData);
            if (editmenu > 0)
            {
                DeleteSessionMenu(menu.ID.ToString());
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "删除菜单失败，请联系管理员";
            }
        }

        /// <summary>
        /// 删除菜单按钮
        /// </summary>
        public void DeleteButton()
        {
            EHECD_MenuButtonDTO button = JSONHelper.GetModel<EHECD_MenuButtonDTO>(RequestParameters.dataStr);
            button.bIsDeleted = true;
            CreateSyslogInfo();
            var ret = DI.DIEntity.GetInstance().GetImpl<IMenuManager>().DeleteMenuButton(button, RequestParameters.dynamicData);

            if (ret > 0)
            {
                DeleteSessionMenuButton(button.ID.ToString());
                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
                result.Msg = "删除菜单按钮失败，请联系管理员";
            }
        }
        #endregion

        #region 创建界面上的菜单树节点数据
        //创建菜单节点数据
        private dynamic CreateMenuData(IList<UserMenu> t)
        {
            var userMs/*组装好的用户菜单*/ = new List<object>();

            //初始化菜单层级关系
            foreach (var item in t)
            {
                //1.判断当前菜单是否是顶层菜单
                if (item.sPID == null)
                {
                    //2.载入当前菜单的层级关系树节点
                    var child = LoadLevelUserMenu(t/*要筛选的菜单*/, item/*当前菜单*/);

                    //3.载入当前菜单的按钮树节点
                    child.AddRange(CreateButtons(item.Buttons));

                    //4.将构建好的节点装入结果集
                    userMs.Add(new
                    {
                        id = item.ID,
                        iconCls = "icon-folder",
                        text = item.sMenuName,
                        attributes = new { type = "menu", url = item.sUrl == null || item.sUrl == "" ? "":  item.sUrl, order = item.iOrder },
                        state = "open",
                        children = child
                    });
                }
            }

            //给整个菜单创建一个根目录
            return new
            {
                text = "根目录",
                iconCls = "icon-folder",
                attributes = new { type = "root", url = "root", order = 0 },
                state = "open",
                children = userMs.OrderBy(m => ((dynamic)((dynamic)m).attributes).order).ToList()
            };
        }

        //载入带层级的菜单节点
        private List<object> LoadLevelUserMenu(IList<UserMenu> t, UserMenu m)
        {
            var userMs/*指定的菜单的下级菜单*/ = new List<object>();

            for (int i = 0; i < t.Count; i++)
            {
                var temp/*从菜单集合中取出操作的菜单*/ = t[i];

                if (m.ID == temp.sPID /*判断该菜单是否是指定菜单的下级菜单*/)
                {
                    //1.载入他的下级节点
                    var child = LoadLevelUserMenu(t, temp);
                    //2.载入他的菜单按钮
                    child.AddRange(CreateButtons(temp.Buttons));

                    var menu = new
                    {
                        id = temp.ID,
                        text = temp.sMenuName,
                        state = "closed",
                        iconCls = string.IsNullOrWhiteSpace(temp.sUrl) ? "icon-folder" : "icon-clipboard",
                        attributes = new { type = "menu", url = temp.sUrl, order = temp.iOrder },
                        children = child
                    };
                    userMs.Add(menu);
                }
            }
            return /*由于这里的都是同级的菜单，返回前对菜单进行一个排序*/userMs.OrderBy(km => ((dynamic)((dynamic)km).attributes).order).ToList();
        }

        //创建按钮节点
        private List<object> CreateButtons(IList<UserMenuButton> t)
        {
            if (t.Count > 0)
            {
                var ret = new List<object>();
                for (int i = 0; i < t.Count; i++)
                {
                    ret.Add(new
                    {
                        id = t[i].ID,
                        attributes = new { type = "btn", order = t[i].iOrder },
                        text = t[i].sButtonName,
                        iconCls = t[i].sIcon
                    });
                }
                return ret.OrderBy(km => ((dynamic)((dynamic)km).attributes).order).ToList();
            }
            else
            {
                return new List<object>();
            }
        }
        #endregion

        #region 私有方法（主要是操作session中的菜单哪一类的）

        //重新构建菜单层级结构
        private IList<UserMenu> InitMenu(IList<UserMenu> t)
        {
            //清除掉原菜单的层级关系
            Parallel.For(0, t.Count, index => t[index].ChildMenu.Clear());

            var userMs/*重构后的层级菜单*/ = new List<UserMenu>();

            //初始化菜单层级关系
            foreach (var item in t)
            {
                if (item.sPID == null/*是否是顶层菜单*/)
                {
                    //载入这个菜单的层级关系，直到没有底层菜单
                    RecursionLoadLevelUserMenu(t, item);
                    userMs.Add(item);
                }
            }
            return userMs.OrderBy(m => m.iOrder).ToList();
        }

        //给指定菜单重新设置其层级关系
        private void RecursionLoadLevelUserMenu(IList<UserMenu> t, UserMenu m)
        {
            Parallel.For(0, t.Count, index =>
            {
                var temp = t[index];
                if (m.ID == temp.sPID)
                {
                    m.ChildMenu.Add(temp);
                    RecursionLoadLevelUserMenu(t, temp);//再载入这个菜单的下级菜单
                }
            });
            m.ChildMenu = m.ChildMenu != null ? m.ChildMenu.OrderBy(x => x.iOrder/*根据排序号排序*/).ToList() : new List<UserMenu>();
        }

        //找到指定的按钮信息
        private UserMenuButton LoadMenuButtonById(string id)
        {
            UserMenuButton ret = null;
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                var isfind = false;
                for (int index = 0; index < userRoleMenu.AllMenu.Count; index++)
                {
                    if (isfind)
                    {
                        break;
                    }
                    else
                    {
                        foreach (var button in userRoleMenu.AllMenu[index].Buttons)
                        {
                            if (button.ID.ToString() == id)
                            {
                                ret = button;
                                isfind = true;
                                break;
                            }
                        }
                    }
                }
            }
            return ret;
        }

        //找到指定的菜单信息
        private UserMenu LoadMenuById(string id)
        {
            UserMenu ret = null;
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                for (int index = 0; index < userRoleMenu.AllMenu.Count; index++)
                {
                    if (userRoleMenu.AllMenu[index].ID.ToString() == id)
                    {
                        ret = userRoleMenu.AllMenu[index];
                        break;
                    }
                }
            }
            return ret;

        }

        //更新会话中的按钮
        private void UpdateSessionMenuButton(UserMenuButton m)
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                var isfind = false;
                for (int index = 0; index < userRoleMenu.AllMenu.Count; index++)
                {
                    if (isfind)
                    {
                        break;
                    }
                    else
                    {
                        for (int innerindex = 0; innerindex < userRoleMenu.AllMenu[index].Buttons.Count; innerindex++)
                        {
                            if (userRoleMenu.AllMenu[index].Buttons[innerindex].ID == m.ID)
                            {
                                userRoleMenu.AllMenu[index].Buttons[innerindex] = m;
                                isfind = true;
                                break;
                            }
                        }
                    }
                }
                SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
            }
            else
            {
                throw new Domain.DomainInfoException("没有从会话中找到对应的按钮，请联系管理员");
            }
        }

        //更新会话中的用户菜单
        private void UpdateSessionMenu(EHECD_FunctionMenuDTO editmenu)
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                for (int index = 0; index < userRoleMenu.AllMenu.Count; index++)
                {
                    if (userRoleMenu.AllMenu[index].ID == editmenu.ID)
                    {
                        userRoleMenu.AllMenu[index].iOrder = editmenu.iOrder;
                        userRoleMenu.AllMenu[index].sMenuName = editmenu.sMenuName;
                        userRoleMenu.AllMenu[index].sUrl = editmenu.sUrl;
                        break;
                    }
                }
                var userMenu = InitMenu(userRoleMenu.AllMenu/*重构菜单的层级关系*/);
                userRoleMenu.UserMenu = userMenu;
                SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
            }
            else
            {
                throw new Domain.DomainInfoException("没有从会话中找到对应的菜单，请联系管理员");
            }
        }

        //删除session中指定ID的menu
        private void DeleteSessionMenu(string v)
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                var temp/*载入要删除的菜单（他的所有有层级关系的下级）*/ = RecursionLoadMenus(userRoleMenu.AllMenu, new UserMenu { ID = Guid.Parse(v) });
                temp.Add(new UserMenu { ID = Guid.Parse(v)/*连要删除的菜单一起放入删除列表*/ });

                for (int i = 0; i < temp.Count; i++)
                {
                    var id = temp[i].ID;

                    //将不要的菜单移除
                    for (int index = 0; index < userRoleMenu.AllMenu.Count; index++)
                    {
                        if (userRoleMenu.AllMenu[index].ID == id)
                        {
                            userRoleMenu.AllMenu.RemoveAt(index);
                        }
                    }
                }
                var userMenu = InitMenu(userRoleMenu.AllMenu/*重构菜单的层级关系*/);
                userRoleMenu.UserMenu = userMenu;
                SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
            }
            else
            {
                throw new Domain.DomainInfoException("没有从会话中找到对应的菜单，请联系管理员");
            }
        }

        //载入指定菜单的所有下级菜单
        private IList<UserMenu> RecursionLoadMenus(IList<UserMenu> t, UserMenu m)
        {
            List<UserMenu> ret = new List<UserMenu>();
            foreach (var item in t)
            {
                if (m.ID == item.sPID)
                {
                    ret.Add(item);
                    ret.AddRange(RecursionLoadMenus(t, item));
                }
            }
            return ret;
        }

        //删除Session中对应的button
        private void DeleteSessionMenuButton(string v)
        {
            var userRoleMenu = GetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/) as UserRoleMenuInfo;
            if (userRoleMenu != null)
            {
                Parallel.For(0, userRoleMenu.AllMenu.Count, (index, state) =>
                {
                    for (int i = 0; i < userRoleMenu.AllMenu[index].Buttons.Count; i++)
                    {
                        if (userRoleMenu.AllMenu[index].Buttons[i].ID.ToString() == v)
                        {
                            userRoleMenu.AllMenu[index].Buttons.RemoveAt(i);
                            state.Stop();
                            return;
                        }
                    }
                });
                var userMenu = InitMenu(userRoleMenu.AllMenu/*重构菜单的层级关系*/);
                userRoleMenu.UserMenu = userMenu;
                SetSessionInfo(SessionInfo.USER_MENUS/*用户的权限和菜单等信息*/, userRoleMenu);
            }
            else
            {
                throw new Domain.DomainInfoException("没有从会话中找到对应的菜单按钮，请联系管理员");
            }
        }
        #endregion
    }
}