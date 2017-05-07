$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("用户管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI); //easyui模块
        var f = modules.get(enums.Modules.FUNC);            //公用模块
        var grid = $("#systemuser_grid");                   //系统用户grid

        /**
         * 初始化事件
         */
        function initEvent() {
            $("#systemuser_grid_tool")
                .on("click", "a[data-id='search_user_button']", function () { eui.search(grid, true); })
                .on("click", "a[data-id='add_systemuser_button']", addSystemUser)
                .on("click", "a[data-id='edit_systemuser_button']", editSystemUser)
                .on("click", "a[data-id='del_systemuser_button']", delSystemUser)
                .on("click", "a[data-id='frozen_systemuser_button']", frozenSystemUser)
                .on("click", "a[data-id='distribution_role_to_systemuser']", distributionRoleToSystemuser);
        }

        /**
         * 载入系统用户列表
         * @param {Number} pageNumber 页码
         * @param {Number} pageSize 每页显示条数
         */
        function loadSystemUser(pageNumber, pageSize) {
            try {
                if ($("#systemuser_form").form("validate")) {
                    var param/*查询参数*/ = $("#systemuser_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/SystemUser/LoadSystemUser", param, function (r) {
                        grid.datagrid("loadData", r.Data.Result);
                        grid.datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });
                    }, function (r) {
                        eui.alertErr(r.Msg);
                    });
                }
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 添加用户
         */
        function addSystemUser() {
            var div = $("<div/>");
            div.dialog({
                title: "添加用户",
                width: 500,
                height: 400,
                cache: false,
                href: '/Admin/SystemUser/ToAddSystemUser',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_systemuser_form").form("validate")) {

                            //序列化提交数据
                            var json = $("#add_systemuser_form").serializeObject();

                            //验证密码是否两次输入一致
                            if (json.sPassWord !== json.sPassWord2) {
                                return eui.alertErr("两次输入的密码不一致");
                            }

                            delete json.sPassWord2;

                            /*
                            * 初始化区域数据
                            */
                            var region = json.region.split("/");

                            if (region.length > 0) {

                                delete json.region;

                                if (region.length === 1) {
                                    json.sProvice = region[0];
                                    json.sCity = "";
                                    json.sCounty = "";
                                } else if (region.length === 2) {
                                    json.sProvice = region[0];
                                    json.sCity = region[1];
                                    json.sCounty = "";
                                } else {
                                    json.sProvice = region[0];
                                    json.sCity = region[1];
                                    json.sCounty = region[2];
                                }

                            } else {
                                return eui.alertErr("请选择系统用户所在区域");
                            }
                            debugger
                            var type = $("#add_userType").combobox("getText");
                            if (type == "平台用户") {
                                json.tUserType = 0;
                            }
                            if (type == "店铺") {
                                json.tUserType = 1;
                            }
                            if (type == "合伙人") {
                                json.tUserType = 2;
                            }
                            f.post("/Admin/SystemUser/AddSystemUser", json,
                                function (ret) {
                                    eui.alertInfo("添加用户成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (ret) {
                                    eui.alertErr(ret.Msg);
                                });
                        }
                    }
                }, {
                    text: '关闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $(div).dialog("close");
                    }
                }],
                onClose: function () {
                    modules.get(enums.Modules.CACHE).cleanCache(enums.VARIABLE.BOOTSTRAP_ADDRESS_DATA);
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                    
                }
            });
        }

        /**
         * 编辑用户
         */
        function editSystemUser() {
            eui.checkSelectedRow(grid, function (selectedRow) {
                var div = $("<div/>");
                div.dialog({
                    title: "编辑用户信息",
                    width: 400,
                    height: 400,
                    cache: false,
                    href: '/Admin/SystemUser/ToEditSystemUser?ID=' + selectedRow.ID,
                    modal: true,
                    collapsible: false,
                    minimizable: false,
                    maximizable: false,
                    resizable: false,
                    buttons: [{
                        text: '保存',
                        iconCls: 'icon-save',
                        handler: function () {
                            if ($("#edit_systemuser_form").form("validate")) {

                                //序列化提交数据
                                var json = $("#edit_systemuser_form").serializeObject();
                                json.ID = selectedRow.ID;
                                /*
                                * 初始化区域数据
                                */
                                var region = json.region.split("/");

                                if (region.length > 0) {

                                    delete json.region;

                                    if (region.length === 1) {
                                        json.sProvice = region[0];
                                        json.sCity = "";
                                        json.sCounty = "";
                                    } else if (region.length === 2) {
                                        json.sProvice = region[0];
                                        json.sCity = region[1];
                                        json.sCounty = "";
                                    } else {
                                        json.sProvice = region[0];
                                        json.sCity = region[1];
                                        json.sCounty = region[2];
                                    }

                                } else {
                                    return eui.alertErr("请选择系统用户所在区域");
                                }

                                f.post("/Admin/SystemUser/EditSystemUser", json,
                                    function (ret) {
                                        eui.alertInfo("编辑用户成功");
                                        $(div).dialog("close");
                                        eui.search(grid, true);
                                    }, function (ret) {
                                        eui.alertErr(ret.Msg);
                                    });
                            }
                        }
                    }, {
                        text: '关闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $(div).dialog("close");
                        }
                    }],
                    onClose: function () {
                        modules.get(enums.Modules.CACHE).cleanCache(enums.VARIABLE.BOOTSTRAP_ADDRESS_DATA);
                        $(div).dialog("destroy"); div = null;
                    },
                    onLoad: function () {
                        var form = $("#nodata");
                        if (form.length > 0) {
                            div.parent().find("a>span")[0].remove();
                        }
                    }
                });
            }, "请选择您要编辑的角色");
        }

        /**
         * 删除用户
         */
        function delSystemUser() {
            try {
                eui.confirmDomain(grid, function (selectedRow) {
                    f.post("/Admin/SystemUser/DeleteSystemUser", { ID: selectedRow.ID }, function (r) {
                        eui.alertInfo("删除角色成功");
                        eui.search(grid, false);
                    }, function (r) {
                        eui.aler(r.Msg);
                    });
                }, undefined, "请选择您要删除的系统用户。",
                function (selectedRow) {
                    return "您是否确认要删除【{0}】？<span style='color:red'>该操作是不可逆的！将删除该用户及分配给该用户的所有菜单和按钮。</span>".format(selectedRow.sUserName);
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 冻结用户
         */
        function frozenSystemUser() {
            try {
                eui.confirmDomain(grid, function (selectedRow) {
                    f.post("/Admin/SystemUser/FrozenSystemUser",
                        {
                            ID: selectedRow.ID,
                            tUserState: selectedRow.tUserState
                        }, function (r) {
                            if (selectedRow.tUserState === 0) {
                                eui.alertInfo("冻结用户成功");
                            } else {
                                eui.alertInfo("解冻用户成功");
                            }
                            eui.search(grid, true);
                        }, function (r) {
                            eui.alertErr(r.Msg);
                        });
                }, undefined, "请选择您要冻结或解冻的系统用户。",
                function (selectedRow) {
                    if (selectedRow.tUserState === 0) {
                        return "您是否确认要冻结【{0}】？<span style='color:red'>用户冻结后将不能登录系统。</span>".format(selectedRow.sUserName);
                    } else {
                        return "您是否确认要解冻【{0}】？".format(selectedRow.sUserName);
                    }
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 分配角色
         */
        function distributionRoleToSystemuser() {
            try {
                eui.checkSelectedRow(grid, function (selectedRow) {
                    var div = $("<div/>");
                    div.dialog({
                        title: "编辑用户信息",
                        width: 400,
                        height: 400,
                        cache: false,
                        href: '/Admin/SystemUser/ToDistributionRole?ID=' + selectedRow.ID,
                        modal: true,
                        collapsible: false,
                        minimizable: false,
                        maximizable: false,
                        resizable: false,
                        buttons: [{
                            text: '保存',
                            iconCls: 'icon-save',
                            handler: function () {
                                if ($("#distribution_role_form").form("validate")) {
                                    var rolesIds = "";
                                    var selected = $("input[data-checkbox='box']:checked");
                                    if (selected.length > 1) {
                                        eui.alertErr("每个用户只能分配一种角色");
                                        return;
                                    }

                                    $("input[data-checkbox='box']:checked")
                                        .each(function () {
                                            rolesIds += $(this).val() + ",";
                                        });

                                    if (rolesIds.length > 0) rolesIds = rolesIds.slice(0, rolesIds.length - 1);
                                   

                                    f.post("/Admin/SystemUser/DistributionRole?ID=" + selectedRow.ID, { ids: rolesIds },
                                        function (ret) {
                                            eui.alertInfo("分配用户角色成功");
                                            $(div).dialog("close");
                                            eui.search(grid, true);
                                        }, function (ret) {
                                            eui.alertErr(ret.Msg);
                                        });
                                }
                            }
                        }, {
                            text: '关闭',
                            iconCls: 'icon-cancel',
                            handler: function () {
                                $(div).dialog("close");
                            }
                        }],
                        onClose: function () {
                            $(div).dialog("destroy"); div = null;
                        },
                        onLoad: function () {
                            var form = $("#nodata");
                            if (form.length > 0) {
                                div.parent().find("a>span")[0].remove();
                            }
                        }
                    });
                }, "请选择要分配角色的用户");
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 初始化grid数据
         */
        function initData() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#systemuser_grid_tool",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'sLoginName', title: '登录名', align: 'center', width: 100 },
                { field: 'sUserName', title: '用户名', align: 'center', width: 100 },
                {
                    field: 'tUserState', title: '用户状态', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (row.tUserState) {
                            case 0:
                                return "正常";
                            case 1:
                                return "冻结";
                            default:
                                break;
                        }
                    }
                },
                {
                    field: 'tUserType', title: '用户类型', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.tUserType === 0) {
                            return "平台用户";
                        } else if (row.tUserType === 1) {
                            return "店铺用户";
                        } else if (row.tUserType === 2) {
                            return "合伙人用户";
                        }
                    }
                },
                { field: 'sUserNickName', title: '用户昵称', align: 'center', width: 100 },
                { field: 'sMobileNum', title: '手机号', align: 'center', width: 100 },
                { field: 'dCreateTime', title: '创建时间', align: 'center', width: 100 },
                { field: 'dLastLoginTime', title: '最后登录时间', align: 'center', width: 100 }
                ]],
                onLoadSuccess: function () {
                    $(".datagrid-header-check input[type=checkbox]").remove();
                }, rowStyler: function (index, row) {
                    if (row.tUserState === 1) {
                        return 'background-color:gray;color:white';
                    }
                }
            }, loadSystemUser).pagination("select");
        }
        
        try {
            initData();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }
        
        function destroy() {
           
        }

        /**
        * 如果你要暴露域接口，请使用以下方式，其他域如果要互动，可以
        * 通过modules.get("cache").getMenuDomain("菜单标题（中文）")
        * 的方式来获取对应的菜单域，并调用他们提供的公共方法，但有一
        * 点请注意，如果提供方法的菜单已经关闭，则无法取到他的操作域
        * */
        return {
            destroy: destroy
        };
    });
});