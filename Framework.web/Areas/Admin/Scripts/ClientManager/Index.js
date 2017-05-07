$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("会员管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var grid = $("#client_manager_grid");
        var sort = 0;
        var sortable = false;
        var selectedRowId = null;

        /**
         * 初始化事件
         */
        function initEvent() {
            $("#client_manager_form")
                .on("click", "a[data-id='search_client_manager']", function () {
                    sortable = false;
                    eui.search(grid, false);
                })
                .on("click", "a[data-id='forzen_client_manager']", forzenClient)
                .on("click", "a[data-id='sort_by_commission_client_manager']", function () {
                    sortable = true;
                    sort = sort === 0 ? 1 : 0;
                    eui.search(grid, false);
                });
        }

        /**
         * 冻结/解冻客户
         */
        function forzenClient() {
            try {
                eui.confirmDomainByMultiRows(grid, function (selectedRows) {
                    var param = [];
                    for (var i = 0; i < selectedRows.length; i++) {
                        param.push({
                            ID: selectedRows[i].ID,
                            iState: selectedRows[i].iState === 1 ? 0 : 1,
                            sPhone: selectedRows[i].sPhone
                        });
                    }
                    f.post("/Admin/ClientManager/ForzenClients", param, function (r) {
                        eui.alertInfo("操作成功");
                        eui.search(grid, true);
                    }, function (r) {
                        eui.alertErr(r.Msg);
                    });
                },
                "您是否要对选中的用户进行冻结/解冻操作？", "请至少选中一行信息以进行操作", null);
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
                toolbar: "#client_manager_grid_tool",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: false,
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'sNickName', title: '昵称', align: 'center', width: 100 },
                { field: 'sPhone', title: '手机号', align: 'center', width: 100 },
                {
                    field: 'iClientType', title: '角色', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (row.iClientType) {
                            case 0:
                                return "普通会员";
                            case 1:
                                return "分享客";
                            default:
                                break;
                        }
                    }
                },
                {
                    field: 'iState', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.iState === 1) {
                            return "正常";
                        } else {
                            return "冻结";
                        }
                    }
                },
                { field: 'dAddTime', title: '注册时间', align: 'center', width: 100 },
                {
                    field: 'iTotalPrice', title: '累计佣金', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.iTotalPrice === null && row.iClientType === 0) {
                            // 普通会员
                            return "N/A";
                        } else if (row.iClientType ===0) {
                            // 普通会员
                            return "N/A";
                        }
                        else if (row.iTotalPrice === null && row.iClientType === 1) {
                            // 分销客（但是此时还未分销商品）
                            return "0";
                        } else {
                            // 分销客（已经有分销的商品）
                            return row.iTotalPrice;

                        }
                    }
                },
                {
                    field: 'operator', title: '操作', align: 'center', width: 100, formatter: function (value, row, index) {
                        return "<a href='javascript:;' onclick='modules.get(enums.Modules.CACHE).getMenuDomain(\"会员管理\").clientDetail(\"" + row.ID + "\"," + row.iClientType + ");'>详情</a>";
                    }
                }
                ]],
                onLoadSuccess: function () {
                }, rowStyler: function (index, row) {
                    if (row.iState === 0) {
                        return 'background-color:gray;color:white';
                    }
                }
            }, loadClient).pagination("select");
        }

        /**
         * 查看客户详情
         * @param {type} id
         * @param {type} iClientType
         */
        function clientDetail(id, iClientType) {
            selectedRowId = id;
            var div = $("<div/>");
            div.dialog({
                title: iClientType === 0 ? "普通用户详情" : "分享客详情",
                width: 800,
                height: 400,
                cache: false,
                href: '/Admin/ClientManager/ClientDetail',
                modal: true,
                queryParams: { ID: id, iClientType: iClientType },
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
                    initClientOrdersGrid();
                    if (iClientType === 1) {
                        initSharedGoodsGrid();
                    }
                }
            });
        }

        /**
         * 载入分享的商品
         */
        function initSharedGoodsGrid() {
            eui.bindPaginationEvent($("#client_shared_goods"), {
                idField: "ID",
                loadMsg: "正在加载...",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'sClientID', hidden: true },
                {
                    field: 'sGoodsPictures', title: '商品图', align: 'center', width: 80, height: 80, formatter: function (value, row, index) {
                        var pics = value.split(",");
                        if (pics.length > 0) {
                            return "<img src=" + pics[0]+ " style='height:80px;width:80px;'>";
                        } else {
                            return "<img src='../Content/img/noimg.png' style='height:80px;width:80px;'></img>";
                        }
                    }
                },
                { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                { field: 'iPirce', title: '价格', align: 'center', width: 100 },
                { field: 'sShopName', title: '所属店铺', align: 'center', width: 100 },                
                { field: 'dInsertTime', hidden: true }
                ]],
                onLoadSuccess: function () {
                    $(".datagrid-header-check input[type=checkbox]").remove();
                }, rowStyler: function (index, row) {
                }
            }, loadSharedGoods).pagination("select");
        }

        /**
        * 载入分享客分享的商品
        * @param {Number} pageNumber 页码
        * @param {Number} pageSize 每页显示条数
        */
        function loadSharedGoods(pageNumber, pageSize) {
            try {                
                var param/*查询参数*/ = {};
                param.PageIndex/*当前页码*/ = pageNumber;
                param.pageSize/*每页显示条数*/ = pageSize;
                param.ID = selectedRowId;
                f.post("/Admin/ClientManager/LoadSharedGoods", param, function (r) {
                    if (r.Data !== null) {
                        $("#client_shared_goods").datagrid("loadData", r.Data.Result);
                        $("#client_shared_goods").datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });
                    }
                }, function (r) {
                    eui.alertErr(r.Msg);
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 初始化普通会员的订单列表
         */
        function initClientOrdersGrid() {
            eui.bindPaginationEvent($("#client_his_orders"), {
                idField: "ID",
                loadMsg: "正在加载...",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'sOrderNo', title: '订单号', align: 'center', width: 150 },
                { field: 'sReceiverPhone', title: '购买人', align: 'center', width: 100 },
                { field: 'iTotalPrice', title: '订单总价', align: 'center', width: 100 },
                {
                    field: 'iType', title: '订单分类', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (value) {
                            case 0:
                                return "民宿";
                            case 1:
                                return "票务";
                            case 2:
                                return "周边";
                            default:
                                break;
                        }
                    }
                },
                { field: 'dBookTime', title: '下单时间', align: 'center', width: 100 },
                {
                    field: 'iState', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                        debugger
                        switch (value) {
                            case 0:
                                return "待付款";
                            case 1:
                                return "待使用";
                            case 2:
                                return "已核销";
                            case 3:
                                return "退款";
                            default:
                                break;
                        }
                    }
                },
                {
                    field: 'operator', title: '操作', align: 'center', width: 100, formatter: function (value, row, index) {
                        // "<a href='javascript:;' onclick='modules.get(enums.Modules.CACHE).getMenuDomain(\"会员管理\").clientDetail(\"" + row.ID + "\"," + row.iClientType + ");'>详情</a>";
                        return '<a class="OrderDetail" data-ID=' + row.ID + ' data-iType=' + row.iType + '>详情</a>'
                    }
                }
                ]],
                onLoadSuccess: function () {
                    //绑定详情事件
                    $('.OrderDetail').on("click", function () {
                        var ID = $(this).attr("data-ID");
                        var iType = Number($(this).attr("data-iType"));
                        OrderDetail(ID, iType);
                    });

                    $(".datagrid-header-check input[type=checkbox]").remove();
                }, rowStyler: function (index, row) {
                }
            }, loadClientOrders).pagination("select");
        }

        /**
     * 查看订单ID
     * @param {type} orderID 订单ID
     */
        function OrderDetail(ID, iType) {
            var div = $("<div/>");
            div.dialog({
                title: "订单详情",
                width: 800,
                height: 500,
                cache: false,
                href: '/Admin/ClientManager/OrderDetail',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                singleSelect: true,
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
                    f.post('/Admin/OrderManager/LoadOrderDetailById', { ID: ID, iType: iType },
                        function (r) {
                            //绑定基础数据                            
                            setBaseData(r.Data.order);
                            //加载商品列表数据
                            initGoodsGrid(iType, r.Data.orderGoods);
                        }, function (r) {
                            eui.alertInfo(r.Msg);
                        });
                }
            });
        }

        /**
         * 基础数据绑定
        * @param {type} data
        */
        function setBaseData(data) {
            //0待付款 1待使用  2-已核销 
            var timeTR = $("#order_manager_order_time");
            var value = "";
            switch (data.iState) {
                case 0:
                    value = "待付款";
                    break;
                case 1:
                    value = "待使用";
                    timeTR.html('<td>下单时间</td><td id="order_manager_order_book_time"></td><td>付款时间</td><td>{0}</td>'.format(data.dPayTime));
                    break;
                case 2:
                    value = "已核销";
                    var newHtml = '<tr id="order_manager_order_pay_time"><td>付款时间</td><td>{0}</td><td>核销时间</td><td>{1}</td></tr>'.format(data.dPayTime, data.dFinishTime);
                    $("#order_manager_order_form").append(newHtml);
                    $("#order_manager_order_pay_time").insertAfter("#order_manager_order_time");
                    break;
                case 3://（0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款）
                    if (data.returniState == 0) value = "退款(退款审核中)";
                    if (data.returniState == 1) value = "退款(接受申请)";
                    if (data.returniState == 2) value = "退款(退款成功)";
                    if (data.returniState == 3) value = "退款(拒绝退款)";
                    break;
            }
            $("#order_manager_order_no").text(data.sOrderNo);
            $("#order_manager_order_status").text(value);
            $("#order_manager_order_book_time").text(data.dBookTime);
            $("#order_manager_order_client_name").text(data.sReceiver);
            $("#order_manager_order_client_mobile_number").text(data.sReceiverPhone);
            var couponStr = "";
            if (data.iCoiCouponPrice === null || data.iCoiCouponPrice === "" || data.iUsePrice === null || data.iUsePrice === "") {
                couponStr = "该订单没有使用优惠劵";
            } else {
                couponStr = "{0}元，（满{1}可用）".format(data.iCoiCouponPrice, data.iUsePrice);
            }
            $("#order_manager_order_coupon").text(couponStr);
            $("#order_manager_order_realPayMoney").text(data.iTotalPrice + "元");
            $("#order_manager_order_remark").text(data.sDescribe);
        }

        /**
        * 获取显示商品详情的列数组
        * @param {type} type 类型（0客房 1门票 2周边产品）
        */
        function getGoodsColumns(type) {
            var columns = [];
            switch (type) {
                case 0:
                    columns = [
                    { field: 'sGoodsName', title: '商品名称', align: 'center', width: 150},
                    { field: 'iTotalPrice', title: '总价', align: 'center', width: 100 },
                    { field: 'dStartTime', title: '开始时间', align: 'center', width: 100, hidden: true },
                    { field: 'sEndTime', title: '结束时间', align: 'center', width: 100, hidden: true },
                    {
                        field: 'iTotalTime', title: '入住时间', align: 'center', width: 300, formatter: function (value, row, index) {

                            var endTime = new Date(row.sEndTime.replace(/-/g, "/"));
                            //new Date("2016-11-29".replace(/-/g, "/")).setDate(new Date("2016-11-29".replace(/-/g, "/")).getDate()+1)
                            return "{0}入住，{1}退房".format(new Date(row.dStartTime.replace(/-/g, "/")).Format("MM/dd"), new Date(endTime.setDate(endTime.getDate() + 1)).Format("MM/dd"));
                        }
                    }];
                    break;
                case 1:
                    columns = [
                     { field: 'sGoodsName', title: '商品名称', align: 'center', width: 150 },
                     { field: 'iAmount', title: '数量', align: 'center', width: 100 },
                     {
                         field: 'iTotalPrice', title: '金额', align: 'center', width: 100
                     },
                     { field: 'dStartTime', title: '票务时间', align: 'center', width: 100 }];
                    break;
                case 2:
                    columns = [
                   { field: 'sGoodsName', title: '商品名称', align: 'center', width: 150},
                   { field: 'iTotalPrice', title: '总价', align: 'center', width: 100 }];
                    break;
            }
            return columns;
        }

        /**
         * 加载商品数据
         */
        function initGoodsGrid(iType, data) {

            //获取的要显示商品的列
            var columnsData = getGoodsColumns(iType);
            //初始化组件
            $("#order_manager_order_goods_grid").datagrid({
                columns: [columnsData],
                fitColumns: true,
            });
            //加载数据
            $("#order_manager_order_goods_grid").datagrid("loadData", data);

            //设置样式【样式兼容问题】
            $($(".datagrid-htable")[3]).css({ "width": "774px" });
            $($(".datagrid-btable")[2]).css({ "width": "774px" });
        }





        /**
        * 载入普通客户订单
        * @param {Number} pageNumber 页码
        * @param {Number} pageSize 每页显示条数
        */
        function loadClientOrders(pageNumber, pageSize) {
            try {                
                var param/*查询参数*/ = {};
                param.PageIndex/*当前页码*/ = pageNumber;
                param.pageSize/*每页显示条数*/ = pageSize;
                param.ID = selectedRowId;
                f.post("/Admin/ClientManager/LoadClientOrders", param, function (r) {
                    if (r.Data !== null) {
                        $("#client_his_orders").datagrid("loadData", r.Data.Result);
                        $("#client_his_orders").datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });
                    }
                }, function (r) {
                    eui.alertErr(r.Msg);
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }


        /**
        * 载入系统用户列表
        * @param {Number} pageNumber 页码
        * @param {Number} pageSize 每页显示条数
        */
        function loadClient(pageNumber, pageSize) {
            try {
                if ($("#client_manager_form").form("validate")) {
                    var param/*查询参数*/ = $("#client_manager_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    param.sort = sort;
                    param.sortable = sortable;
                    f.post("/Admin/ClientManager/LoadClients", param, function (r) {
                        if (r.Data !== null) {
                            grid.datagrid("loadData", r.Data.Result);
                            grid.datagrid("getPager").pagination({
                                pageNumber: pageNumber,
                                pageSize: pageSize,
                                total: r.Data.MaxCount
                            });
                        }
                    }, function (r) {
                        eui.alertErr(r.Msg);
                    });
                }
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        try {
            initData();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }

        function destroy() {

        }

        return {
            destroy: destroy,
            clientDetail: clientDetail
        };
    });
});