$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("密码设置", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#password_manage_menu");


        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#password_manage_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                
                { field: 'sLoginName', title: '登录名', align: 'center', width: 200 },
                { field: 'sReallyName', title: '真实姓名', align: 'center', width: 200 },
                { field: '扫描员', title: '角色', align: 'center', width: 200  ,formatter: function (value, row, index) {
                    return "<span>扫描员</span>";
                }},
                ]],
                onLoadSuccess: function () {
                    $(".datagrid-header-check input[type=checkbox]").remove();
                }, rowStyler: function (index, row) {
                    if (row.tUserState === 1) {
                        return 'background-color:gray;color:white';
                    }
                }
            }, loadDate).pagination("select");
        }

        //查询数据的回调方法
        function loadDate(pageNumber, pageSize) {
            try {
                if ($("#password_manage_form").form("validate")) {
                    var param/*查询参数*/ = $("#password_manage_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#search_password_user").val();
                    
                    f.post("/Admin/Password/GetList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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
        //按钮事件绑定
        function initEvent() {
            $("#password_manage_form")
            .on("click", "a[data-id='search_scanner_button']", searchScanner)
            .on("click", "a[data-id='add_scanner_button']", addScanner)
            .on("click", "a[data-id='delete_scanner_button']", deleteScanner)
            .on("click", "a[data-id='edit_scanner_button']", editScanner)
            .on("click", "a[data-id='updateps_scanner_button']", updateScanner)
        }
        //查找扫描员
        function searchScanner() {
            grid.datagrid("getPager").pagination("select");
        }
        //添加扫描员
        function addScanner(){
            var div = $("<div/>");
            div.dialog({
                title: "添加扫描员",
                width: 700,
                height: 310,
                cache: false,
                href: '/Admin/Password/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if (!$("#add_scanner_form").form('validate')) { return false; }
                        var json = $("#add_scanner_form").serializeObject();
                        if (json.sPassWord !== json.sPassWord2) {
                            return eui.alertErr("两次输入的密码不一致");
                        }
                        delete json.sPassWord2;

                        f.post("/Admin/Password/SearchName", { name: $("#add_scanner_name").val() }, function (res) {
                            debugger
                            if (res.Data.name == 0) {//如果没有相同的用户名,添加扫描员
                                f.post("/Admin/Password/AddScanner", json,
                            function (ret) {
                                eui.alertInfo("添加成功");
                                $(div).dialog("close");
                                eui.search(grid, false);
                            }, function (ret) {
                                eui.alertErr(ret.Msg);
                            });
                            }
                            else {
                                //如果有相同的用户名
                                return eui.alertInfo("已存在相同的登录名，请重新输入登录名");
                            }
                          
                        }, function () {
                           
                        });

                      
                    }
                }, {
                    text: '关闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $(div).dialog("close");
                    }
                }],
                onClose: function () {
                    /**
                    * 如果dialog里有对全局变量的操作并有驻留，这里记得要清除
                    */
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                }
            });
        }

        function deleteScanner() {
            eui.confirmDomainByMultiRows(grid, deletess, "", "请至少选择一个扫描员", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个扫描员".format(rows.length);
            });
        }

        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }

        //删除回调
        function deletess(row) {
            debugger
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            f.post("/Admin/Password/DeleteScanner", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("删除成功");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("删除失败");
            })
        }
        //修改
        function editScanner() {
            eui.checkSelectedRow(grid, edit, "请选择一条您需要修改的数据");

        }
        //修改回调
        function edit(row) {
            var selectedRow = grid.datagrid("getSelections");
            if (selectedRow.length > 1) {
                eui.alertInfo("只能选择一条数据")
                return false;
            }
            //修改窗口
            var div = $("<div/>");
            div.dialog({
                title: "编辑扫描员",
                width: 400,
                height: 230,
                cache: false,
                href: '/Admin/Password/Edit?ID='+row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if (!$("#edit_scanner_form").form('validate')) { return false; }
                        var json = $("#edit_scanner_form").serializeObject();
                        //判断是否修改了登录名
                        debugger
                        var exloginname = $("#ex_scanner_loginname").val();//修改之前的登录名
                        var loginName = json.sLoginName;//现在的登录名
                        if (exloginname == loginName) {
                            f.post("/Admin/Password/UpdateScanner", json,
                               function (ret) {
                                   eui.alertInfo("修改成功");
                                   $(div).dialog("close");
                                   eui.search(grid, false);
                               }, function (ret) {
                                   eui.alertErr(ret.Msg);
                               });
                        }

                        else {
                            f.post("/Admin/Password/SearchName", { name: loginName }, function (res) {
                                if (res.Data.name == 0) {//如果没有相同的用户名,添加扫描员
                                 f.post("/Admin/Password/UpdateScanner", json,
                                      function (ret) {
                                         eui.alertInfo("修改成功");
                                         $(div).dialog("close");
                                         eui.search(grid, false);
                                     }, function (ret) {
                                         eui.alertErr(ret.Msg);   });
                                }
                                else {
                                    //如果有相同的用户名
                                    return eui.alertInfo("已存在相同的登录名，请重新输入登录名");
                                }

                            }, function (res) { });
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
                }
            });
        }
        //修改密码
        function updateScanner() {
            eui.checkSelectedRow(grid, function doUpdate(row) {
                var selectedRow = grid.datagrid("getSelections");
                if (selectedRow.length > 1) {
                    eui.alertInfo("只能选择一条数据")
                    return false;
                }
                psDialog(row);
            }, "请选择一行数据");
        }
        //修改密码回调
        function psDialog(row) {

            var div = $("<div/>");
            div.dialog({
                title: '修改密码',
                width: 400,
                height: 250,
                cache: false,
                href: '/Admin/Password/EditPS?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if (!$("#edit_scanner_password_form").form('validate')) { return false; }
                        var json = $("#edit_scanner_password_form").serializeObject();
                        //验证密码是否两次输入一致
                        if (json.sPassWord !== json.sPassWord2) {
                            return eui.alertErr("两次输入的密码不一致");
                        }
                        delete json.sPassWord2;
                        
                        f.post("/Admin/Password/UpdateScanner", json, function (res) {
                            eui.alertInfo("修改成功");
                        }, function () { eui.alertInfo("修改失败"); })
                        $(div).dialog("close");
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
                }
            });
        }

        

        try {
            init();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }
    });
});
