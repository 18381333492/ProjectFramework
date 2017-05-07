/// <reference path="E:\项目集合\成都亿合科技\源码\友客分享商城\ProjectFramework\Framework.web\scripts/lib/easyui.module.js" />
$(function () {

    modules.get(enums.Modules.CACHE).setMenuDomain("优惠劵管理", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var pageNumber = 1, pageSize = 10;
        var grid = $("#coupon_index_datagrid");

        /**
         * 初始化数据
         */
        function initDataGrid() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#coupon_index_tool_div",
                fit: true,
                pageNumber: pageNumber,
                pageSize: pageSize,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'ID', title: '主键', align: 'center', width: 100, hidden: true },
                { field: 'iCoiCouponPrice', title: '优惠劵金额', align: 'center', width: 100, hidden: true },
                { field: 'iUsePrice', title: '优惠劵使用限制', align: 'center', width: 100, hidden: true },
                {
                    field: 'sCouponName', title: '优惠劵名称', align: 'center', width: 100, formatter: function (value, row, index) {
                        var where = row.iUsePrice == 0 ? "无使用门槛" : "满" + row.iUsePrice + "元可用";
                        return row.iCoiCouponPrice + "元" + "(" + where + ")";
                    }
                },
                { field: 'iCouponCount', title: '投放数量', align: 'center', width: 100 },
                { field: 'iCouponGetedCount', title: '领取数量', align: 'center', width: 100 },
                { field: 'dValidDateStart', title: '有效期开始', align: 'center', width: 100, hidden: true },
                { field: 'dValidDateEnd', title: '有效期结束', align: 'center', width: 100, hidden: true },
                {
                    field: 'dVilidateDate', title: '有效期', align: 'center', width: 100, formatter: function (value, row, index) {
                        return new Date(row.dValidDateStart.replace(/-/g, "/")).Format("yyyy-MM-dd") + "至" + new Date(row.dValidDateEnd.replace(/-/g, "/")).Format("yyyy-MM-dd");
                    }
                }
                ]]
            }, searchCallBack).pagination("select");
        }

        /**
         * 查询数据的回调方法
         * @param {type} pageNumber
         * @param {type} pageSize
         */
        function searchCallBack(pageNumber, pageSize) {
            try {
                if ($("#coupon_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#coupon_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/Coupon/LoadData", param, function (r) {
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

        /**
         * 确认是否值选中一行(编辑使用)
         */
        function CheckIsSelectOne() {
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }

        /**
         * 显示添加窗口
         */
        function showAddView() {
            var div = $("<div/>");
            div.dialog({
                title: "添加",
                width: 650,
                height: 400,
                cache: false,
                href: '/Admin/Coupon/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        var startTime = $("#coupon_add_dValidDateStart").val();
                        var endTime = $("#coupon_add_dValidDateEnd").val();
                        //时间判断
                        if (startTime == "" || endTime == "") {
                            return eui.alertInfo("请填写时间");
                        } else if (new Date(startTime) > new Date(endTime)) {
                            return eui.alertInfo("结束时间不能小于开始时间");
                        }

                        if ($("#coupon_add_form").form("validate")) {
                            var json = $("#coupon_add_form").serializeObject();
                            if (parseFloat(json.iUsePrice) != 0) {
                                if (parseFloat(json.iUsePrice) <=parseFloat(json.iCoiCouponPrice)) {
                                    eui.alertErr("使用条件必须要大于优惠券的价格");
                                    return;
                                }
                            }
                            f.post("/Admin/Coupon/DoAdd", json, function (res) {
                                eui.alertInfo("添加成功");
                                $(div).dialog("close");
                                eui.search(grid,false);
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
                }
            });
        }

        /**
         * 显示编辑窗口
         */
        function showEditView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑",
                width: 650,
                height: 400,
                cache: false,
                href: '/Admin/Coupon/Edit?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#coupon_edit_form").form("validate")) {
                            //var json = $("#coupon_edit_form").serializeObject();
                            var iCouponCount = $("#coupon_edit_input_iCouponCount").val();
                            var ID = $("#coupon_edit_ID").val();
                            //修改数量不能小于已经领取的数量
                            if (iCouponCount < row.iCouponGetedCount) {
                                return eui.alertInfo("不能小于已领取数量");
                            }
                            //执行编辑
                            f.post("/Admin/Coupon/DoEdit", { ID: ID, iCouponCount: iCouponCount }, function (res) {
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
                }
            });
        }

        /**
         * 显示详情窗口
         */
        function showDetailView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "详细",
                width: 650,
                height: 400,
                cache: false,
                href: '/Admin/Coupon/Detail?ID=' + row.ID,
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
                    /**
                    * 如果dialog里有对全局变量的操作并有驻留，这里记得要清除
                    */
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                }
            });
        }

        /**
         * 详细回调
         * @param {type} selectRow 选中行
         */
        function detailCallBack(selectRow) {
            showDetailView(selectRow);
        }

        /**
         * 查看详细
         */
        function lookDetailEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, detailCallBack, "请至少选中一行");
            }
        }

        /**
         * 查询【事件】
         */
        function searchEvent() {
            var startTime = $("#coupon_index_dValidDateStart").val();
            var endTime = $("#coupon_index_dValidDateEnd").val();
            //时间判断
            if (new Date(startTime) > new Date(endTime)) {
                eui.alertInfo("结束时间不能小于开始时间");
            }
            else {
                searchCallBack(pageNumber, pageSize);
            }
            
        }
        
        /**
         * 添加【事件】
         */
        function addEvent() {
            showAddView();
        }

        /**
         * 编辑回调
         * @param {type} selectRow 选中行
         */
        function editCallBack(selectRow){
            showEditView(selectRow);
        }

        /**
         * 编辑【事件】
         */
        function editEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, editCallBack,"请至少选中一行");
            }
        }

        /**
         * 格式化提示信息的函数
         */
        function formatConfirmMSGCallBack(selectedRow){
            return "你确定要删除这" + selectedRow.length + "条数据吗？"
        }

        /**
         * 删除回调
         */
        function deleteCallBack(selectedRows) {
            var rowIDs = [];
            for (var i = 0; i < selectedRows.length; i++) {
                rowIDs.push(selectedRows[i].ID);
            }
            //执行操作
            f.post("/Admin/Coupon/DoDelete", { rowIDs: rowIDs.toString() }, function (res) {
                //searchCallBack(pageNumber, pageSize);
                //从新加载数据 并清除选中
                eui.search(grid, false);
                eui.alertInfo("删除成功");
            }, function (res) {
                eui.alertInfo(res.Msg);
            });
        }

        /**
         * 删除
         */
        function deleteEvent() {
            eui.confirmDomainByMultiRows(grid, deleteCallBack, "", "请至少选中一行", formatConfirmMSGCallBack);
        }

        /**
         * 绑定事件
         */
        function bindEvent() {
            $("#coupon_index_form")
                .on("click", "#coupon_index_btn_seach", searchEvent)
                .on("click", "a[data-id='Coupon_Add']", addEvent)
                .on("click", "a[data-id='Coupon_Edit']", editEvent)
                .on("click", "a[data-id='Coupon_Delete']", deleteEvent)
                .on("click", "a[data-id='Coupon_Detail']",lookDetailEvent)
        }

        try {
            initDataGrid();
            bindEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }

        function destroy() {

        }

        /**
        * 如果你要暴露域接口，请使用以下方式，其他域如果要互动，可以
        * 通过modules.get("cache").getMenuDomain("菜单标题（中文）")
        * 的方式来获取对应的菜单域，并调用他们提供的公共方法，但有一
        * 点请注意，如果提供方法的菜单已经关闭，则无法取到他的操作域
        * */
        return {
            destroy: destroy
        };
    });
});