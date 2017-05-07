$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("店铺管理", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#partner_self_shop_menu");
        
        /*
        *初始化页面绑定
        */
        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#partner_self_shop_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sLoginName', title: '登录名', align: 'center', width: 260, hidden: true },
                 { field: 'sShopName', title: '店铺名称', align: 'center', width: 200 },
                 {
                     field: 'tUserState', title: '店铺状态', align: 'center', width: 200, formatter: function (value, row, index) {
                         var state;
                         switch (row.tUserState) {
                             case 0:
                                 state = "正常";
                                 break;
                             case 1:
                                 state = "冻结";
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
                 { field: 'sAddress', title: '具体地址', align: 'center', width: 100, hidden: true },
                 {
                     field: 'sDZ', title: '地址', align: 'center', width: 200, formatter: function (value, row, index) {
                         return row.sProvice + row.sCity + row.sCounty + row.sAddress;
                     }
                 },
                 { field: 'dCreateTime', title: '成立时间', align: 'center', width: 200 },
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
                if ($("#partner_self_shop_form").form("validate")) {
                    var param/*查询参数*/ = $("#partner_self_shop_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#partner_self_shop_search").val();
                    f.post("/Admin/PartnerShop/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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
            $("#partner_self_shop_form")
            .on("click", "a[data-id='partner_search_selfshop_button']", function () {
                grid.datagrid("getPager").pagination("select");
            })
             .on("click", "a[data-id='detail_partner_shop_button']", detailShop)

        }
        //只能选中一行
        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }
        function detailShop() {
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
                href: '/Admin/PartnerShop/Detail?ID=' + row.ID,
                width: 550,
                height: 330,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [
                    {
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
        }
        catch(e){}
    });
});