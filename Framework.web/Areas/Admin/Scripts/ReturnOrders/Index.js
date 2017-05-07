$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("维权订单管理", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var grid = $("#return_order_manager_grid");

        /**
         * 编辑事件
         */
        function editEvent(selectRow) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑",
                width: 650,
                height: 400,
                cache: false,
                href: '/Admin/ReturnOrders/Edit',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#return_order_manager_edit_form").form("validate")) {

                            //过滤备注
                            var sRemark = $("#return_order_manager_edit_sRemark").val();
                            if ($("<span/>").html(sRemark).text() != sRemark) {
                                return eui.alertInfo("禁止输入非法的html字符");
                            }

                            var json = $("#return_order_manager_edit_form").serializeObject();
                            //执行编辑
                            f.post("/Admin/ReturnOrders/DoEdit", json, function (res) {
                                eui.alertInfo("编辑成功");
                                $(div).dialog("close");
                                eui.search(grid, false);
                            }, function (res) {
                                eui.alertErr(res.Msg);
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
                    //绑定页面数据

                    $("#return_order_manager_edit_ID").val(selectRow.ID);
                    $("#return_order_manager_td_number").text(selectRow.sReturnNo);
                   // $("#return_order_manager_edit_sRemark").text(selectRow.sDescribe);

                    //绑定状态选择事件
                    var iState = selectRow.iState;
                    $("#return_order_manager_edit_select").combobox({
                        onSelect: function (row) {
                            //0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款
                            //1.退款审核中→接受申请→退款成功 
                            //2.退款审核中→拒绝退款
                            switch (iState) {
                                case 0:
                                    if (row.value != 0 && row.value != "1" && row.value != "3") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("只能将当前状态改为【接受申请】或【拒绝退款】");
                                    }
                                    break;
                                case 1:
                                    if (row.value != 1 && row.value != "2") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("只能将当前状态改为【退款成功】");
                                    }
                                    break;
                                case 2:
                                    if (row.value != "2") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("不能修改当前状态");
                                    }
                                    break;
                                case 3:
                                    if (row.value != "3") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("不能修改当前状态");
                                    }
                                    break;

                            }
                        }
                    });
                    //设置选中项
                    $("#return_order_manager_edit_select").combobox("select", iState);

                }
            });
        }

        /**
         * 基础数据绑定
         * @param {type} data
         */
        function setBaseData(data) {
            var value = "";
            switch (data.iState) {
                case 0:
                    value = "退款审核中";
                    break;
                case 1:
                    value = "接受申请";
                    break;
                case 2:
                    value = "退款成功";
                    break;
                case 3:
                    value = "拒绝退款";
                    break;
            }
            //只有 待使用 的订单能维权退款
            $("#return_order_manager_detail_no").text(data.sOrderNo);
            $("#return_order_manager_detail_pay_time").text(data.dPayTime);
            $("#return_order_manager_detail_status").text(value);
            $("#return_order_manager_detail_book_time").text(data.dBookTime);
            $("#return_order_manager_detail_client_name").text(data.sReceiver);
            if (data.sPhone)
            $("#return_order_manager_detail_client_mobile_number").text(data.sPhone);
            var couponStr = "";
            if (data.iCoiCouponPrice === null || data.iCoiCouponPrice === "" || data.iUsePrice === null || data.iUsePrice === "") {
                couponStr = "该订单没有使用优惠劵";
            } else {
                couponStr = "{0}元，（满{1}可用）".format(data.iCoiCouponPrice, data.iUsePrice);
            }
            $("#return_order_manager_detail_coupon").text(couponStr);
            $("#return_order_manager_detail_realPayMoney").text(data.iTotalPrice + "元");
            $("#return_order_manager_detail_remark").text(data.sDescribe);
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
                    { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                    { field: 'iTotalPrice', title: '总价', align: 'center', width: 100 },
                    { field: 'dStartTime', title: '开始时间', align: 'center', width: 100, hidden: true },
                    { field: 'sEndTime', title: '结束时间', align: 'center', width: 100, hidden: true },
                    {
                        field: 'iTotalTime', title: '入住时间', align: 'center', width: 300, formatter: function (value, row, index) {

                            
                            return "{0}入住，{1}退房".format(new Date(row.dStartTime.replace(/-/g, "/")).Format("MM/dd"), new Date((new Date(row.sEndTime.replace(/-/g, "/")).getTime()+24*60*60*1000)).Format("MM/dd"));
                        }
                    }];
                    break;
                case 1:
                    columns = [
                     { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                     { field: 'iAmount', title: '数量', align: 'center', width: 100 },
                     {
                         field: 'iTotalPrice', title: '金额', align: 'center', width: 100
                     },
                     { field: 'dStartTime', title: '票务时间', align: 'center', width: 100 }];
                    break;
                case 2:
                    columns = [
                   { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                   { field: 'iTotalPrice', title: '总价', align: 'center', width: 100 }];
                    break;
            }
            return columns;
        }

        /**
         * 加载商品数据
         */
        function initGoodsGrid(row, data) {
            //获取的要显示商品的列
            var columnsData = getGoodsColumns(row.iType);
            //初始化组件
            $("#return_order_manager_detail_goods_grid").datagrid({
                columns: [columnsData]
            });
            //加载数据
            $("#return_order_manager_detail_goods_grid").datagrid("loadData", data);
            //设置样式【样式兼容问题】
            $($(".datagrid-htable")[3]).css({ "width": "774px" });
            $($(".datagrid-btable")[2]).css({ "width": "774px" });
         
           
        }

        /**
         * 详细事件
         * @param {type} selectRow
         */
        function detailsEvent(row) {
            var div = $("<div/>");
            div.dialog({
                title: "订单详情",
                width: 800,
                height: 500,
                cache: false,
                href: '/Admin/ReturnOrders/Detail',
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
                    f.post('/Admin/ReturnOrders/LoadOrderDetailById', { ID: row.ID, iType: row.iType },
                        function (r) {
                            //绑定基础数据
                            setBaseData(r.Data.order);
                            //加载商品列表数据
                            initGoodsGrid(row, r.Data.orderGoods);
                        }, function (r) {
                            eui.alertInfo(r.Msg);
                        });
                }
            });
        }

        /**
         * 绑定事件
         */
        function initEvent() {
            $("#return_order_manager_form")
            .on("click", "#return_order_manager_btn_seach", function () {
                //搜索
                var dStartTime = $("#return_order_manager_input_startTime").val();
                var dEndTime = $("#return_order_manager_input_endTime").val();
                if (dStartTime != "" && dEndTime != "") {
                    if (new Date(dStartTime) > new Date(dEndTime)) {
                        return eui.alertInfo("开始时间不能大于结束时间");
                    }
                }
                //搜索
                eui.search(grid, false);
            })
            .on("click", "a[data-id='Adults_Details']", function () {
                //详情
                eui.checkSelectedRow(grid, detailsEvent, "请至少选中一行");
            })
            .on("click", "a[data-id='Adults_Edit']", function () {
                //操作
                eui.checkSelectedRow(grid, editEvent, "请至少选中一行");
            })
        }

        /**
         * 初始化数据
         */
        function initGrid() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#return_order_manager_grid_tool",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                    { field: 'ID', title: '主键', align: 'center', width: 100, hidden: true },
                    { field: 'sDescribe', title: '备注', align: 'center', width: 100, hidden: true },
                    { field: 'iType', title: '订单类型', align: 'center', width: 100, hidden: true },
                    { field: 'sOrderNo', title: '订单号', align: 'center', width: 100 },
                    { field: 'sReturnNo', title: '退款号', align: 'center', width: 100 },
                    { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                    { field: 'dInsertTime', title: '申请时间', align: 'center', width: 100 },
                    { field: 'iTotalPrice', title: '实付金额', align: 'center', width: 100 },
                    { field: 'sShopName', title: '店铺名称', align: 'center', width: 100 },
                    {
                        field: 'iState', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                            //0- 退款审核中，1 -接受申请，2 - 退款成功，3 - 拒绝退款
                            switch (value) {
                                case 0:
                                    value = "退款审核中";
                                    break;
                                case 1:
                                    value = "接受申请";
                                    break;
                                case 2:
                                    value = "退款成功";
                                    break;
                                case 3:
                                    value = "拒绝退款";
                                    break;
                            }
                            return value;
                        }
                    }]],
                onLoadSuccess: function (data) {
                    if (data.total > 0) {
                        var flag = data.rows[0].isStore;
                        if (flag === false) {
                            //平台用户
                        } else {
                            //店铺用户
                            grid.datagrid('hideColumn', 'sShopName'); // 设置隐藏列
                        }
                    }
                }
            }, loadDatagridPagingData).pagination("select");
        }

        /**
         * 加载数据回调
         * @param {type} pageNumber
         * @param {type} pageSize
         */
        function loadDatagridPagingData(pageNumber, pageSize) {
            try {
                if ($("#return_order_manager_form").form("validate")) {
                    var param/*查询参数*/ = $("#return_order_manager_form").serializeObject();
                    param.pageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/ReturnOrders/LoadReturnOrderData", param, function (r) {
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


        try {
            //初始化datagrid
            initGrid();
            initEvent();//绑定事件
        } catch (e) {
            eui.alertErr(e.message);
        }

        /**
         * 释放资源的方法
         */
        function destroy() {
        }

        return {
            //释放资源
            destroy: destroy
        }

    });
});