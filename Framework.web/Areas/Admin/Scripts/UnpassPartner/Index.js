$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("待审核合伙人", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#un_partner_pass_menu");

        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#un_partner_pass_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sName', title: '合伙人姓名', align: 'center', width: 200 },
                 { field: 'sMobileNum', title: '手机号', align: 'center', width: 200 },
                 { field: 'sProvice', title: '省', align: 'center', width: 100, hidden: true },
                 { field: 'sCity', title: '市', align: 'center', width: 100, hidden: true },
                 { field: 'sCounty', title: '县', align: 'center', width: 100, hidden: true },
                 {
                     field: 'sDZ', title: '代理区域', align: 'center', width: 200, formatter: function (value, row, index) {

                         return row.sProvice + row.sCity + row.sCounty;
                     }
                 },
                 { field: 'dCreateTime', title: '申请时间', align: 'center', width: 200 },
                 {
                     field: 'iState', title: '状态', align: 'center', width: 200, formatter: function (value, row, index) {
                         var state;

                         switch (row.iState) {
                             case 0:
                                 state = "待审核";
                                 break;
                             case 1:
                                 state = "通过";
                                 break;
                             case 2:
                                 state = "拒绝";
                                 break;
                             default:
                                 break;

                         }
                         return state;
                     }
                 },
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
                if ($("#un_partner_pass_form").form("validate")) {
                    var param/*查询参数*/ = $("un_#partner_pass_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#un_partner_name_search").val();
                    f.post("/Admin/UnpassPartner/GetUnPassPartner", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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
            $("#un_partner_pass_form")
            .on("click", "a[data-id='pass_partner_button']", passPartner)
            .on("click", "a[data-id='delay_partner_button']", delayPartner)
            .on("click", "a[data-id='delete_unpass_button']", deletePartner)
            .on("click", "a[data-id='search_unpass_button']", searchPartner)
            .on("click", "a[data-id='detail_unpass_pertner_button']", detailPartner)
        }

        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }
        //查看详情
        function detailPartner() {
            
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, function detailA(row) {
                    mailDetail(row);
                },
                  "请选择一行");

            }
        }
        function mailDetail(row) {
            var div = $("<div/>");
            div.dialog({
                title: '详情',
                href: '/Admin/UnpassPartner/Detail?ID=' + row.ID,
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
        //查找
        function searchPartner()
        {
            grid.datagrid("getPager").pagination("select");
        }
        //通过申请
        function passPartner() {
            eui.confirmDomainByMultiRows(grid, doPass, "", "请至少选择一家合伙人", function formatconfirm(rows) {
                return "您确定要通过这【{0}】个合伙人".format(rows.length);
            });
        }

        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }

        //通过回调
        function doPass(row) {
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            f.post("/Admin/UnpassPartner/PassCheck", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("通过审核");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("审核失败");
            })
        }

        //删除合伙人
        function deletePartner() {
            eui.confirmDomainByMultiRows(grid, deletess, "", "请至少选择一个合伙人", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个合伙人".format(rows.length);
            });
        }
        //删除回调
        function deletess(row) {
            debugger
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            f.post("/Admin/UnpassPartner/DeleteCheck", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("删除成功");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("删除失败");
            })
        }
        //拒绝
        function delayPartner() {
            eui.confirmDomainByMultiRows(grid, function delay(row) {
                var rowIDs = [];
                //遍历所有的行创建ID集合
                for (var i = 0; i < row.length; i++) {
                    rowIDs.push(row[i].ID);
                }

                f.post("/Admin/UnpassPartner/DelayCheck", { ID: rowIDs.toString() }, function () {
                    eui.alertInfo("拒绝成功");
                    reloadData();
                    grid.datagrid("clearSelections");
                }, function () {
                    eui.alertInfo("拒绝失败");
                })
            }, "", "请至少选择一个合伙人", function formatconfirm(rows) {
                return "您确定要拒绝这【{0}】个合伙人".format(rows.length);
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