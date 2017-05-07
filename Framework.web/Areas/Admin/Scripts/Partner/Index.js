$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("已审核合伙人", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#partner_pass_menu");
        var travelObj = {};
        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#partner_pass_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sLoginName', title: '登录名', align: 'center', width: 260, hidden: true },
                 { field: 'sUserName', title: '合伙人姓名', align: 'center', width: 200 },
                 {
                     field: 'tUserState', title: '状态', align: 'center', width: 200, formatter: function (value, row, index) {
                         var state;
                         switch (row.tUserState) {
                             case 0:
                                 state = "正常";
                                 break;
                             case 2:
                                 state = "待审核";
                                 break;
                             case 3:
                                 state = "拒绝";
                                 break;
                             default:
                                 break;

                         }
                         return state;
                     }
                 },
                 { field: 'sProvice', title: '省', align: 'center', width: 100, hidden: true },
                 { field: 'sCity', title: '市', align: 'center', width: 100, hidden: true },
                 { field: 'sCounty', title: '县', align: 'center', width: 100, hidden: true },
                 {
                     field: 'sDZ', title: '代理区域', align: 'center', width: 200, formatter: function (value, row, index) {

                         return row.sProvice + row.sCity + row.sCounty;
                     }
                 },
                 { field: 'dCreateTime', title: '通过时间', align: 'center', width: 200 },
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
                if ($("#partner_pass_form").form("validate")) {
                    var param/*查询参数*/ = $("#partner_pass_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#partner_name_search").val();
                    f.post("/Admin/Partner/GetPassPartner", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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

        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }

        function initEvent() {
            $("#partner_pass_form")
                .on("click", "a[data-id='add_partner_button']", addPartner)
                .on("click", "a[data-id='delete_partner_button']", deletePartner)
                .on("click", "a[data-id='update_ps_partner_button']",updatePartnerPS)
                .on("click", "a[data-id='search_partner_button']", searchPartner)
                .on("click", "a[data-id='detail_partner_button']", detailPartner)
        }
        //查找合伙人
        function searchPartner() {
            grid.datagrid("getPager").pagination("select");
        }
        //添加合伙人
        function addPartner() {
            var div = $("<div/>");
            div.dialog({
                title: "添加合伙人",
                width: 800,
                height: 600,
                cache: false,
                href: '/Admin/Partner/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_partner_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDCardImage.imageList.length <= 1) {
                                return eui.alertInfo("请上传身份证正反两面");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDCardImage.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDCardImage.imageList[i].filePath);
                                }
                                $("#partner_add_idCardImges").val(temps.join());

                                var json = $("#add_partner_form").serializeObject();
                                //验证密码是否两次输入一致
                                if (json.sPassWord !== json.sPassWord2) {
                                    return eui.alertErr("两次输入的密码不一致");
                                }
                                delete json.sPassWord2;
                                
                                if ($('#choose_address_add').val() == "" || $('.placeholder').val() == null) {
                                    return eui.alertInfo("请输入意向区域");
                                }
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
                            }
                            f.post("/Admin/Partner/AddPartner", json,
                                function (ret) {
                                    eui.alertInfo("添加成功");
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
                    /**
                    * 如果dialog里有对全局变量的操作并有驻留，这里记得要清除
                    */
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                    travelObj.UploadIDCardImage = new UploadImage({
                        target: "partner_add_idCardImges_upload", maxFileCount: 2
                    });
                }
            });
        }
        //删除合伙人
        function deletePartner() {
            eui.confirmDomainByMultiRows(grid, deletess, "", "请至少选择一个合伙人", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个合伙人".format(rows.length);
            });
        }
        //删除回调
        function deletess(row) {
            
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            f.post("/Admin/Partner/DeleteCheck", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("删除成功");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("删除失败");
            })
        }
        //修改密码
        function updatePartnerPS() {
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
                width: 500,
                height: 250,
                cache: false,
                href: '/Admin/Partner/Edit?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if (!$("#update_partner_password").form('validate')) { return false; }
                        var json = $("#update_partner_password").serializeObject();
                        //验证密码是否两次输入一致
                        if (json.sPassWord !== json.sPassWord2) {
                            return eui.alertErr("两次输入的密码不一致");
                        }
                        delete json.sPassWord2;
                        debugger
                        f.post("/Admin/Partner/UpDatePassword", json, function (res) {
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

        //只能选中一行
        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }

        //查看详情
        function detailPartner()
        {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, function detailA(row) {
                    debugger
                    mailDetail(row);
                },
                  "请选择一行");
            }
            
        }
        function mailDetail(row) {

            var div = $("<div/>");
            div.dialog({
                title: '详情',
                href: '/Admin/Partner/Detail?ID=' + row.ID,
                width: 550,
                height: 330,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
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