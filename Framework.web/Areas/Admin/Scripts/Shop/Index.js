/// <reference path="E:\项目集合\成都亿合科技\源码\友客分享商城\ProjectFramework\Framework.web\scripts/plug-in/address/baiduaddress/scripts/baidu.map.js" />

$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("已通过的店铺", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#shop_menu");
        var travelObj = {};
        /*
        *初始化页面绑定
        */
        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#shop_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sLoginName', title: '登录名', align: 'center', width: 260, hidden: true },
                 { field: 'sShopName', title: '店铺名称', align: 'center', width: 260 },
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
                 { field: 'sAddress', title: '具体地址', align: 'center', width: 100, hidden: true },
                 {
                     field: 'sDZ', title: '地址', align: 'center', width: 400, formatter: function (value, row, index) {

                         return row.sProvice + row.sCity + row.sCounty + row.sAddress;
                     }
                 },
                 { field: 'dCreateTime', title: '成立时间', align: 'center', width: 300 },
                 { field: 'sMessage', title: '剩余短信', align: 'center', width: 200, hidden: true },

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
                if ($("#shop_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#shop_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#search_pass_shop").val();
                    var region = $("#shop_index_form").serializeObject().region.split("/");
                    var sProvice = "";
                    var sCity = "";
                    var sCounty = "";
                    if (region.length === 1) {
                        sProvice = region[0];
                        sCity = "";
                        sCounty = "";
                    } else if (region.length === 2) {
                        sProvice = region[0];
                        sCity = region[1];
                        sCounty = "";
                    } else {
                        sProvice = region[0];
                        sCity = region[1];
                        sCounty = region[2];
                    }
                    var tUserState = "";
                    if ($("#shaop_pass_State").combobox("getText") === "全部") {
                        tUserState = "";
                    }
                    else if ($("#shaop_pass_State").combobox("getText") === "正常") {
                        tUserState = 0;
                    }
                    else if ($("#shaop_pass_State").combobox("getText") === "冻结") {
                        tUserState = 1;
                    }

                    f.post("/Admin/CheckShop/GetPassList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword, tUserState: tUserState, sProvice: sProvice, sCity: sCity, sCounty: sCounty }, function (r) {
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

        //下拉菜单绑定的数据
        var localData = [{
            "value": 0,
            "text": "全部",
            "selected": true
        }, {
            "value": 1,
            "text": "正常"
        }, {
            "value": 2,
            "text": "冻结"
        }
        ];
        //下拉菜单初始化
        function loadCombo() {
            $("#shaop_pass_State").combobox({
                data: localData,
                textField: "text",
                valueField: "value",
                editable: false,
                onSelect: function (res) {
                }
            });
        }

        //绑定事件
        function initEvent() {
            $("#shop_index_form")
            .on("click", "a[data-id='add_shop_button']", addShop)
            .on("click", "a[data-id='delete_shop_button']", deletepassShop)
            .on("click", "a[data-id='unfreeze_shop_button']", frezzeshop)
            .on("click", "a[data-id='search_shop_button']", searchshop)
            .on("click", "a[data-id='reset_ps_button']", updateShopPS)
            .on("click", "a[data-id='detail_shop_button']", detailPartner)
        }
        //修改密码
        function updateShopPS() {
            eui.checkSelectedRow(grid, function doUpdate(row) {
                var selectedRow = grid.datagrid("getSelections");
                if (selectedRow.length > 1) {
                    eui.alertInfo("只能选择一条数据")
                    return false;
                }
                psDialog(row);
            }, "请选择一行数据");
        }

        function psDialog(row) {

            var div = $("<div/>");
            div.dialog({
                title: '修改密码',
                width: 500,
                height: 250,
                cache: false,
                href: '/Admin/Shop/Edit?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if (!$("#update_shop_password").form('validate')) { return false; }
                        var json = $("#update_shop_password").serializeObject();
                        //验证密码是否两次输入一致
                        if (json.sPassWord !== json.sPassWord2) {
                            return eui.alertErr("两次输入的密码不一致");
                        }
                        delete json.sPassWord2;
                        debugger
                        f.post("/Admin/Shop/UpDatePassword", json, function (res) {
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
        //查找
        function searchshop() {
            init();
        }
        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        //删除
        function deletepassShop() {
            eui.confirmDomainByMultiRows(grid, deletess, "", "请至少选择一家店铺", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个商店".format(rows.length);
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
            f.post("/Admin/Shop/DeleteCheck", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("删除成功");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("删除失败");
            })
        }
        //冻结
        function frezzeshop() {
            eui.confirmDomainByMultiRows(grid, function delay(row) {
                var rowIDs = [];
                var rowState = [];
                debugger
                //遍历所有的行创建ID集合
                for (var i = 0; i < row.length; i++) {
                    rowIDs.push(row[i].ID);
                    rowState.push(row[i].tUserState);
                }
                f.post("/Admin/Shop/FreezeCheck", { ID: rowIDs.toString(), tUserState: rowState.toString() }, function () {
                    eui.alertInfo("冻结/解冻成功");
                    reloadData();
                    grid.datagrid("clearSelections");
                }, function () {
                    eui.alertInfo("冻结/解冻失败");
                })


            }, "", "请至少选择一家店铺", function formatconfirm(rows) {
                return "您确定要冻结/解冻这【{0}】个商店".format(rows.length);
            });
        }

        function addShop() {
            var div = $("<div/>");
            div.dialog({
                title: "添加店铺",
                width: 800,
                height: 600,
                cache: false,
                href: '/Admin/Shop/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_shop_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDCardImage.imageList.length <= 1) {
                                return eui.alertInfo("请上传身份证正反两面");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDCardImage.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDCardImage.imageList[i].filePath);
                                }
                                $("#shop_add_idCardImges").val(temps.join());

                                var json = $("#add_shop_form").serializeObject();
                                //验证密码是否两次输入一致
                                if (json.sPassWord !== json.sPassWord2) {
                                    return eui.alertErr("两次输入的密码不一致");
                                }
                                delete json.sPassWord2;

                                if ($("#add_shop_pass_address").val() == "" || $("#add_shop_pass_address").val() == null) {
                                    return eui.alertInfo("请选择区域");
                                }

                                /*
                                * 初始化区域数据
                                */
                                var region = json.region.split("/");

                                if (region.length > 0) {

                                    delete json.region;


                                    if (region.length === 1) {
                                        return eui.alertInfo("请选择市");
                                    } else if (region.length === 2) {
                                        json.sProvice = region[0];
                                        json.sCity = region[1];
                                        json.sCounty = "";
                                    } else {
                                        json.sProvice = region[0];
                                        json.sCity = region[1];
                                        json.sCounty = region[2];
                                    }
                                    //详细地址
                                    debugger
                                    var address = json.sProvice + json.sCity + json.sCounty + json.sAddress;
                                    
                                    // ====================根据选择的地址获取经纬度=======================

                                    // 获取地理编码
                                    var geocoder = new BMap.Geocoder();

                                    // 获取具体的经纬度
                                    geocoder.getPoint(address, function (point) {

                                        // alert(JSON.stringify(point)); point.lng 经度  point.lat 纬度
                                        json.sLONG = point.lng;
                                        json.sLat = point.lat;
                                        // 提交保存数据
                                        f.post("/Admin/Password/SearchName", { name: $("#add_shop_name").val() }, function () {
                                            f.post("/Admin/Shop/AddShop", json,
                                           function (ret) {
                                               eui.alertInfo("添加成功");
                                               $(div).dialog("close");
                                               eui.search(grid, false);
                                           }, function (ret) {
                                               eui.alertErr(ret.Msg);
                                           });
                                        }, function () {
                                            return eui.alertInfo("已存在相同的登录名，请重新输入登录名");
                                        });


                                    }, json.sCity);

                                } else {
                                    return eui.alertErr("请选择系统用户所在区域");
                                }
                            }
                            
                            
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
                        target: "shop_add_idCardImges_upload", maxFileCount: 2
                    });
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
        //详情
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
                href: '/Admin/Shop/Detail?ID=' + row.ID,
                width: 550,
                height: 430,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [
                    {
                        text: '保存',
                        iconCls: 'icon-add',
                        handler: function () {
                            debugger
                            var json = $("#detail_pass_shop").serializeObject();
                            json.sPartnerID = $("#shop_pass_partner").combobox("getValue");
                            json.sPartnerName = $("#shop_pass_partner").combobox("getText");
                            f.post('/Admin/Shop/UpdatePartner', json, function (res) {
                                eui.alertInfo("保存成功");
                            }, function (res) {
                                eui.alertInfo("保存失败");
                            })

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
                    loadComboPartnrt();
                }
            });
        }

        function loadComboPartnrt() {
            $("#shop_pass_partner").combobox({
                textField: "text",
                valueField: "id",
                onSelect: function (res) {

                }
            });
            f.post("/Admin/Shop/GetAllPartner", null, function (res) {

                $("#shop_pass_partner").combobox("loadData", res.Data);
            }, function (res) {

            })


        }

        try {
            loadCombo();
            init();
            initEvent();
        }

        catch (e)
        { eui.alertErr(e.message); }



    });
});