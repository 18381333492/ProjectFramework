$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("自定义菜单", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);
        var grid = $("#wechart_menu_table");
        var iTouchType = 0;
        var bIsVisible = 1;


        //页面数据绑定
        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#wechart_menu_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sMenuName', title: '菜单名', align: 'center', width: 260 },
                 { field: 'iOrderNo', title: '排序', align: 'center', width: 260 },
                 {
                     field: 'iTouchType', title: '触发类型', align: 'center', width: 300, formatter: function (value, row, index) {
                         var state;
                         switch (row.iTouchType) {
                             case 0:
                                 state = "关键字";
                                 break;
                             case 1:
                                 state = "链接";
                                 break;
                             default:
                                 break;
                         }
                         return state;
                     }
                 },
                 { field: 'sKeyword', title: '关键词/URL', align: 'center', width: 260 },
                ]],
                onLoadSuccess: function () {
                    $(".datagrid-header-check input[type=checkbox]").remove();
                }
            }, loadDate).pagination("select");
        }

        //查询数据的回调方法
        function loadDate(pageNumber, pageSize) {
            try {
                if ($("#wechart_menu_form").form("validate")) {
                    var param/*查询参数*/ = $("#wechart_menu_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/WeChartMenu/GetMenuList", { PageIndex: pageNumber, PageSize: pageSize }, function (r) {
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

        function initEvent() {
            $("#wechart_menu_form")
               .on("click", "a[data-id='add_menu_button']", addmenu)
               .on("click", "a[data-id='edit_menu_button']", editmenu)
               .on("click", "a[data-id='delete_menu_button']", menuDelete)
               .on("click", "a[data-id='create_menu_button']", GeneratorMenu);
        }

        /**
         * 生成菜单
         */
        function GeneratorMenu() {
            try {                
                f.post("/Admin/WeChartMenu/GeneratorMenu", null, function (r) {
                    eui.alertInfo("生成成功");
                }, function (r) {
                    eui.alertErr(r.Msg);
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        //添加/编辑窗口
        var div = "";
        function showDialog(title, url, func, row) {
            div = $("<div/>");
            div.dialog({
                title: title,
                width: 900,
                height: 400,
                cache: false,
                href: url,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        func();

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
                    //当编辑时将数据绑定在页面上
                    if (title === "编辑菜单") {
                        f.post("/Admin/WeChartMenu/SearchDetail", { ID: row.ID }, function (res) {
                            $("#wechart_menu_father_id").val(res.Data.ID);
                            $("#weichart_belong_menu").combobox("setText", res.Data.sSubmenu);
                            $("#weichart_belong_menu").combobox("setValue", res.Data.sSubmenuID);
                            //$("#weichart_belong_menu_id").val(res.Data.sSubmenuID);
                            $("#wechart_menu_title").textbox("setText", res.Data.sMenuName);
                            $("#wechart_menu_url").textbox("setText", res.Data.sKeyword);
                            $("#wechart_menu_no").textbox("setText", res.Data.iOrderNo);
                            $("#menu_shop_id").val(res.Data.sShopID);
                            $("#ex_title_wechart").val(res.Data.sMenuName);
                            //根据查出的值来判断单选框哪个应该是checked
                            if (res.Data.iTouchType === 0) {

                                $("#iTouchType_check_on").attr("checked", true);
                                iTouchType = 0;
                            }
                            if (res.Data.iTouchType === 1) {
                                $("#iTouchType_check_off").attr("checked", true);
                                iTouchType = 1;
                            }
                            if (res.Data.bIsVisible === false) {
                                $("#bIsVisible_check_off").attr("checked", true);
                                bIsVisible = 0;
                            }
                            if (res.Data.bIsVisible === true) {
                                $("#bIsVisible_check_on").attr("checked", true);
                                bIsVisible = 1;
                            }


                        })
                    }
                    loadComboPartnrt();//加载下拉菜单
                    $('input:radio[name=iTouchType1]').click(function () {
                        if ($(this).val() === "0") {
                            iTouchType = 0;
                        } else {
                            iTouchType = 1;
                        }
                    });
                    $('input:radio[name=Visible]').click(function () {
                        if ($(this).val() === "0") {

                            bIsVisible = 0;
                        } else {
                            bIsVisible = 1;
                        }
                    });
                }
            });
        }
        //下拉菜单绑定数据
        function loadComboPartnrt() {
            $("#weichart_belong_menu").combobox({
                textField: "text",
                valueField: "id",
                onSelect: function (res) {
                }
            });
            f.post("/Admin/WeChartMenu/GetAllMenu", null, function (res) {

                $("#weichart_belong_menu").combobox("loadData", res.Data);
            }, function (res) {

            })
        }
        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        //添加菜单
        function addmenu() {
            showDialog("添加菜单", "/Admin/WeChartMenu/Add", function () {
                if (!$("#add_wechart_menu").form('validate')) { return false; }
                if (iTouchType == 1) {
                    //判断如果选择的是url就判断输入的是否是正确的网址
                    var strRegex = /(http(s)?:\/\/|^$)([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
                    var patrn = new RegExp(strRegex);
                    if (!patrn.exec($("#wechart_menu_url").val()))
                    { return eui.alertInfo("请输入正确的网址"); }
                }
                var json = $("#add_wechart_menu").serializeObject();
                //如果下拉框有值，给json赋父类菜单ID和name
                if ($("#weichart_belong_menu").combobox("getText") != null || $("#weichart_belong_menu").combobox("getText") != "") {
                    json.sSubmenuID = $("#weichart_belong_menu").combobox("getValue");
                    json.sSubmenu = $("#weichart_belong_menu").combobox("getText");
                }
                json.iTouchType = iTouchType;
                json.bIsVisible = bIsVisible;
               
                //重名判断
                f.post("/Admin/WeChartMenu/SearchMenu", json, function () {
                    if (json.sSubmenuID == null || json.sSubmenuID == "") {//如果是父类
                        debugger
                        f.post("/Admin/WeChartMenu/GetFatherMenuNumber", null, function () {
                            f.post("/Admin/WeChartMenu/AddMenu", json, function () {
                                $(div).dialog("close");
                                eui.alertInfo("添加成功");
                                reloadData();
                            }, function () {
                                eui.alertInfo("添加失败");
                            });
                        }, function () {
                            eui.alertInfo("父类菜单已存在三个，请删除后再添加");
                        });
                    }

                    else {
                        f.post("/Admin/WeChartMenu/AddMenu", json, function () {
                            $(div).dialog("close");
                            eui.alertInfo("添加成功");
                            reloadData();
                        }, function () {
                            eui.alertInfo("添加失败");
                        });
                    }
                }, function () {
                    return eui.alertInfo("菜单名已存在,请重新填写");
                })

               



            }, null)
        }
        //修改菜单
        function editmenu() {
            //判断是否只选择了一条数据
            var selectedRow = grid.datagrid("getSelections");
            if (selectedRow.length > 1) {
                eui.alertInfo("只能选择一条数据")
                return false;
            }
            eui.checkSelectedRow(grid, function dodit(row) {
                showDialog("编辑菜单", "/Admin/WeChartMenu/Add", function () {
                    if (!$("#add_wechart_menu").form('validate')) { return false; }
                    if (iTouchType == 1) {
                        var strRegex = /(http(s)?:\/\/|^$)([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
                        var patrn = new RegExp(strRegex);
                        if (!patrn.exec($("#wechart_menu_url").textbox("getText")))
                        { return eui.alertInfo("请输入正确的网址"); }
                    }
                    var json = $("#add_wechart_menu").serializeObject();
                    if ($("#weichart_belong_menu").combobox("getText") != null || $("#weichart_belong_menu").combobox("getText") != "") {
                        json.sSubmenuID = $("#weichart_belong_menu").combobox("getValue");
                        json.sSubmenu = $("#weichart_belong_menu").combobox("getText");
                    }
                    json.iTouchType = iTouchType;
                    json.bIsVisible = bIsVisible;
                    json.ID = $("#wechart_menu_father_id").val();
                    //$("#weichart_belong_menu_id").val(res.Data.sSubmenuID);
                    json.sMenuName = $("#wechart_menu_title").textbox("getText");
                    json.sKeyword = $("#wechart_menu_url").textbox("getText");
                    json.iOrderNo = $("#wechart_menu_no").textbox("getText");
                    json.sShopID = $("#menu_shop_id").val();
                    if ($("#ex_title_wechart").val() != json.sMenuName) {
                        //如果输入的名字与本来的菜单名不一致，判断菜单中是否有其他的菜单名
                        f.post("/Admin/WeChartMenu/SearchMenu", json, function () {
                            return eui.alertInfo("菜单名已存在,请重新填写");
                        }, function () {
                            f.post("/Admin/WeChartMenu/EditMenu", json, function () {
                                eui.alertInfo("修改成功");
                                $(div).dialog("close");
                                reloadData();
                            }, function () {
                                eui.alertInfo("添加失败");
                            })

                        });
                    }
                    else {
                        //如果输入的名字与本来的菜单名一致，进行修改
                        f.post("/Admin/WeChartMenu/EditMenu", json, function () {
                            eui.alertInfo("修改成功");
                            $(div).dialog("close");
                            reloadData();
                        }, function () {
                            eui.alertInfo("添加失败");
                        })
                    }


                }, row)
            }, "请选择一条您需要修改的数据");

        }
        //删除菜单
        function menuDelete() {
            eui.confirmDomainByMultiRows(grid, dodelete, "", "请选择至少一条数据", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个菜单?删除后子菜单也将删除".format(rows.length);
            });
        }
        //删除回调
        function dodelete(rows) {
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < rows.length; i++) {
                rowIDs.push(rows[i].ID);
            }
            f.post('/Admin/WeChartMenu/DeleteMenu', { ID: rowIDs.toString() }, function (res) {
                reloadData();
                grid.datagrid("clearSelections");
                eui.alertInfo("删除成功");
            }, function (res) {
                eui.alertErr("删除失败");
            });
        }
        try {
            init();
            initEvent();
        }
        catch (e) {

        }
    });
});