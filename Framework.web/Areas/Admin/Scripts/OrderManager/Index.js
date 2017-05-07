$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("订单管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI); //easyui模块
        var f = modules.get(enums.Modules.FUNC);            //公用方法模块
        var grid = $("#order_manager_grid");                         //页面用到的grid，如果没用请删除

        /**
         * 初始化datagrid
         */
        function initGrid() {
            //设置界面
            var roleType = $("#order_manager_input_RoleType").val();
            if (roleType == 0) {
                //平台用户
                $("#order_manager_input_where_store").attr("style", "display:none");
            } else {
                //店铺用户
                $("#order_manager_input_where_background").attr("style", "display:none");
            }

            try {
                eui.bindPaginationEvent(grid, {
                    idField: "ID",
                    loadMsg: "正在加载...",
                    toolbar: "#order_manager_grid_tool",
                    fit: true,
                    fitColumns: true,
                    pagination: true,
                    rownumbers: true,
                    singleSelect: true,
                    columns: [[
                    //{ field: 'checkbox', checkbox: true },
                    { field: 'ID', title: '主键', align: 'center', width: 100, hidden: true },
                    { field: 'sOrderNo', title: '订单号', align: 'center', width: 100 },
                    { field: 'sReceiver', title: '购买人', align: 'center', width: 100 },
                    { field: 'iTotalPrice', title: '订单总价', align: 'center', width: 100 },
                    {
                        field: 'iType', title: '订单分类', align: 'center', width: 100, formatter: function (value, row, index) {
                            //--0客房 1门票 2周边
                            switch (value) {
                                case 0:
                                    value = "客房";
                                    break;
                                case 1:
                                    value = "门票";
                                    break;
                                case 2:
                                    value = "周边";
                                    break;
                            }
                            return value;
                        }
                    },
                    { field: 'sShopName', title: '店铺名称', align: 'center', width: 100 },
                    { field: 'dBookTime', title: '下单时间', align: 'center', width: 100 },
                    {
                        field: 'iState', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                            //--0待付款 1待使用 2-已核销
                            switch (value) {
                                case 0:
                                    value = "待付款";
                                    break;
                                case 1:
                                    value = "待使用";
                                    break;
                                case 2:
                                    value = "已核销";
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
                    }, rowStyler: function (index, row) {
                        if (row.iState === 0) {
                            return 'background-color:gray;color:white';
                        }
                    }
                }, loadDatagridPagingData).pagination("select");
            } catch (e) {
                eui.alertErr(e.Msg);
            }
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
                    { field: 'sGoodsName', title: '商品名称', align: 'center', width: 150 },
                    { field: 'iTotalPrice', title: '总价', align: 'center', width: 100 },
                    { field: 'dStartTime', title: '开始时间', align: 'center', width: 100, hidden: true },
                    { field: 'sEndTime', title: '结束时间', align: 'center', width: 100, hidden: true },
                    {
                        field: 'iTotalTime', title: '入住时间', align: 'center', width: 300, formatter: function (value, row, index) {

                            var endTime = new Date(row.sEndTime.replace(/-/g, "/"));
                            endTime=new Date(endTime.setDate(endTime.getDate() + 1));
                            var startTime = new Date(row.dStartTime.replace(/-/g, "/"))
                            var time = endTime.getTime() - startTime.getTime() ; //日期的long型值之差

                            var count = Math.floor(time / (24 * 60 * 60 * 1000));
                            //new Date("2016-11-29".replace(/-/g, "/")).setDate(new Date("2016-11-29".replace(/-/g, "/")).getDate()+1)
                            return "{0}入住,{1}退房,共{2}晚".format(startTime.Format("MM/dd"), endTime.Format("MM/dd"), count);
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
                   { field: 'sGoodsName', title: '商品名称', align: 'center', width: 150 },
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
         * 查看订单ID
         * @param {type} orderID 订单ID
         */
        function detailCallBack(row) {
            var div = $("<div/>");
            div.dialog({
                title: "订单详情",
                width: 800,
                height: 500,
                cache: false,
                href: '/Admin/OrderManager/ToOrderDetail',
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
                    f.post('/Admin/OrderManager/LoadOrderDetailById', { ID: row.ID, iType: row.iType },
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
        * 载入datagrid数据
        * @param {Number} pageNumber 页码
        * @param {Number} pageSize 每页显示条数
        */
        function loadDatagridPagingData(pageNumber, pageSize) {
            try {
                if ($("#order_manager_form").form("validate")) {

                    //封装查询要用到的数据

                    var param/*查询参数*/ = $("#order_manager_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.PageSize/*每页显示条数*/ = pageSize;

                    f.post("/Admin/OrderManager/LoadOrderData", param, function (r) {
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

        /**
         * 初始化事件
         */
        function initEvent() {
            $("#order_manager_form")
                .on("click", "#order_manager_btn_seach", function () {
                    //判断时间
                    var startTime = $("#order_manager_input_startTime").val();
                    var endTime = $("#order_manager_input_endTime").val();
                    if (startTime != "" && endTime != "") {
                        if (new Date(startTime) > new Date(endTime)) {
                            return eui.alertInfo("开始时间不能大于结束时间");
                        }
                    }
                    eui.search(grid, false);
                })
            .on("click", "#order_manager_a_detail", function () {
                eui.checkSelectedRow(grid, detailCallBack, "请至少选中一行");
            });
        }

        try {

            //初始化datagrid
            initGrid();

            //初始化事件
            initEvent();

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
        };
    });
});
