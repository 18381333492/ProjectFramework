/// <reference path="E:\项目集合\成都亿合科技\源码\友客分享商城\ProjectFramework\Framework.web\scripts/plug-in/address/baiduaddress/scripts/baidu.map.js" />
$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("待审核店铺", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#shop_check_menu");
        var travelObj = {};
        /*
        *初始化页面绑定
        */
        function init() {
            
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#shop_check_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sShopName', title: '店铺名称', align: 'center', width: 400 },
                 {
                     field: 'iState', title: '店铺状态', align: 'center', width: 200, formatter: function (value, row, index) {
                         var state;
                         
                         switch (row.iState) {
                             case 0:
                                 state="待审核";
                                 break ;
                             case 1:
                                 state="通过";
                                 break;
                             case 2:
                                 state="拒绝";
                                 break ;
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
                     field: 'sDZ', title: '地址', align: 'center', width: 400, formatter: function (value, row, index) {

                         return row.sProvice + row.sCity + row.sCounty + row.sAddress;
                     }
                 },
                 { field: 'dCreateTime', title: '申请时间', align: 'center', width: 200 },
                 

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
                if ($("#shop_check_form").form("validate")) {
                    var param/*查询参数*/ = $("#shop_check_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#search_check_shop").val();
                    f.post("/Admin/CheckShop/CheckPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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
        //绑定时间
        function initEvent() {
            $("#shop_check_form")
            .on("click", "a[data-id='search_check_button']", searchCheck)
            .on("click", "a[data-id='pass_check_button']", passCheck)
            .on("click", "a[data-id='delay_check_button']", delayCheck)
            .on("click", "a[data-id='delete_check_button']", deleteCheck)
            .on("click", "a[data-id='detail_check_button']", detailCheck)
        }
        //查找
        function searchCheck()
        {
            grid.datagrid("getPager").pagination("select");
        }
        //通过
        function passCheck() {
            debugger
            eui.checkSelectedRow(grid, doPass, "请选中您要操作的店铺");
            //eui.confirmDomainByMultiRows(grid, doPass,"", "请至少选择一家店铺",function formatconfirm(rows) {
            //    return "您确定要通过这【{0}】个商店".format(rows.length);
            //});
        }
        //只能选中一行
        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }
        //通过回调
        function doPass(row) {
            if (CheckIsSelectOne()) {
                var rowlong = "";//经度
                var rowlat = "";//纬度
                var address = "";
                var city = "";
                //遍历所有的行创建ID集合

                // ====================根据地址获取经纬度=======================
                address = row.sProvice + row.sCity + row.sCounty + row.sAddress;
                city = row.sCity;
                // 获取具体的经纬度
                // 获取地理编码
                var geocoder = new BMap.Geocoder();
                geocoder.getPoint(address, function (point) {
                    // alert(JSON.stringify(point)); point.lng 经度  point.lat 纬度
                    rowlong = point.lng;
                    rowlat = point.lat;
                    f.post("/Admin/CheckShop/PassCheck", { ID: row.ID, sLONG: rowlong, sLat: rowlat, sCity: city }, function () {
                        eui.alertInfo("店铺通过审核");
                        reloadData();
                        grid.datagrid("clearSelections");
                    }, function () {
                        eui.alertInfo("审核失败");
                    });
                }, city);
            }
           
          
               
        }
        //拒绝
        function delayCheck()
        {
            eui.confirmDomainByMultiRows(grid, function delay(row) {
                var rowIDs = [];

                //遍历所有的行创建ID集合
                for (var i = 0; i < row.length; i++) {
                    rowIDs.push(row[i].ID);
                }

                f.post("/Admin/CheckShop/DelayCheck", { ID: rowIDs.toString() }, function () {
                    eui.alertInfo("拒绝成功");
                    reloadData();
                    grid.datagrid("clearSelections");
                }, function () {
                    eui.alertInfo("拒绝失败");
                })
            }, "", "请至少选择一家店铺", function formatconfirm(rows) {
                return "您确定要拒绝这【{0}】个商店".format(rows.length);
            });
        }
        //删除
        function deleteCheck() 
        {
            eui.confirmDomainByMultiRows(grid, deletecc, "", "请至少选择一家店铺", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个商店".format(rows.length);
            });
        }
        function deletecc(row) {
            debugger
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            
              f.post("/Admin/CheckShop/DeleteCheck", {  ID: rowIDs.toString() }, function () {
                  eui.alertInfo("删除成功");
                  reloadData();
                  grid.datagrid("clearSelections");
              }, function () {
                  eui.alertInfo("删除失败");
              })
        }

        //只能选中一行
        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }

        function detailCheck()
        {
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
                href: '/Admin/CheckShop/Detail?ID=' + row.ID,
                width: 550,
                height: 400,
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
            initEvent();
            init();
        } catch (e) {
            eui.alertErr(e.message);
        }



    });
});