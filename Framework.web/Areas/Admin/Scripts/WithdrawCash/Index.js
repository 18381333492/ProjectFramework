$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("提现管理", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var pageNumber = 1, pageSize = 10;
        var grid = $("#withdrawCash_index_datagrid");

        var button = true;
        /**
        * 初始化数据
        */
        function initDataGrid() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#withdrawCash_index_tool_div",
                fit: true,
                pageNumber: pageNumber,
                pageSize: pageSize,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                sortOrder: "desc",
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                //{ field: 'checkbox', checkbox: true },
                { field: 'ID', title: '主键', align: 'center', width: 100, hidden: true },
                { field: 'sWithdrawNumber', title: '提现编号', align: 'center', width: 100 },
                { field: 'sWithdrawMemberName', title: '提现会员', align: 'center', width: 100 },
                { field: 'iWithdrawMoney', title: '提现金额', align: 'center', width: 100 },
                { field: 'dApplyTime', title: '申请时间', align: 'center', width: 100 },
                {
                    field: 'iState', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                        //<!--状态(0-提现审核中 1-通过审核 2-提现成功)-->
                        switch (value) {
                            case 0:
                                value = "提现审核中"
                                break;
                            case 1:
                                value = "通过审核"
                                break;
                            case 2:
                                value = "提现成功"
                                break;
                        }
                        return value;
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
                if ($("#withdrawCash_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#withdrawCash_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/WithdrawCash/LoadData", param, function (r) {
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
         * 查询【事件】
         */
        function searchEvent() {
            var startTime = $("#withdrawCash_index_dApplyTimeStart").val();
            var endTime = $("#withdrawCash_index_dApplyTimeEnd").val();
            //时间判断
            if (new Date(startTime) > new Date(endTime)) {
                eui.alertInfo("结束时间不能小于开始时间");
            }
            else {
                searchCallBack(pageNumber, pageSize);
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
         * 显示设置窗口
         */
        function showSettingView() {
            var div = $("<div/>");
            div.dialog({
                title: "设置",
                width: 650,
                height: 400,
                cache: false,
                href: '/Admin/WithdrawCash/Setting',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#withdrawCash_setting_form").form("validate")) {

                            var json = $("#withdrawCash_setting_form").serializeObject();
                            //执行编辑
                            f.post("/Admin/WithdrawCash/DoSetting", json, function (res) {
                                eui.alertInfo("设置成功");
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
                    //f.post("/Admin/WithdrawCash/GetSettingData", null, function (res) {
                    //    //生成绑定模板数据
                    //    var html=template("sectionTemple", { list: res.Data });
                    //    $("#withdrawCash_setting_content_div").html(html);
                    //}, function (res) { 
                    //    eui.alertInfo(res.Msg);
                    //});
                }
            });
        }

        /**
         * 提现设置
         */
        function settingEvent() {
            showSettingView();
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
                href: '/Admin/WithdrawCash/Edit?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#withdrawCash_edit_form").form("validate")&&button==true) {
                            //过滤备注
                            var sRemark=$("#withdrawCash_edit_sRemark").val();
                            if (sRemark == "") {
                                return eui.alertInfo("请输入备注信息");
                            }
                            if ($("<span/>").html(sRemark).text() != sRemark) {
                                return eui.alertInfo("禁止输入非法的html字符");
                            }
                            button = false;//防止重复提交
                            var json = $("#withdrawCash_edit_form").serializeObject();
                            //执行编辑
                            f.post("/Admin/WithdrawCash/DoEdit", json, function (res) {

                                eui.alertInfo("编辑成功");
                                $(div).dialog("close");
                                button = true;
                                eui.search(grid, false);
                            }, function (res) {
                                eui.alertErr(res.Msg);
                                button = false;
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
                    var iState = parseInt($("#withdrawCash_edit_iState").val());
                   // <!--提现人类型(0-店铺 1-合伙人 2-分享客)-->
                    var sWithdrawMemberType = parseInt($("#sWithdrawMemberType").val());
                    //绑定状态选择事件
                    $("#withdrawCash_edit_select").combobox({
                        onSelect: function (row) {
                            //0-提现审核中 1-通过审核 2-提现成功
                            //只能往下修改
                            switch (iState) {
                                case 0:
                                    if (row.value != 0 && row.value != "1"&& sWithdrawMemberType!=2) {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("只能将当前状态改为【通过审核】");
                                    }
                                    break;
                                case 1:
                                    if (row.value != 1 && row.value != "2") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("只能将当前状态改为【提现成功】");
                                    }
                                    break;
                                case 2:
                                    if (row.value != "2") {
                                        $(this).combobox("select", iState);
                                        eui.alertInfo("不能修改当前状态");
                                    }
                                    break;
                            }
                        }
                    });
                    //设置选中项
                    $("#withdrawCash_edit_select").combobox("select", iState);
                    
                }
            });
        }

        /**
        * 编辑回调
        * @param {type} selectRow 选中行
        */
        function editCallBack(selectRow) {
            showEditView(selectRow);
        }

        /**
         * 编辑操作
         */
        function editEvent(){
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, editCallBack, "请至少选中一行");
            }
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
                href: '/Admin/WithdrawCash/Detail?ID=' + row.ID,
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
                    var iState = $("#withdrawCash_detail_iState").text();
                    var res = "提现审核中";
                    switch (iState) {
                        case "0":
                            res = "提现审核中"
                            break;
                        case "1":
                            res = "通过审核"
                            break;
                        case "2":
                            res = "提现成功"
                            break;
                    }
                    $("#withdrawCash_detail_iState").text(res);
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
        function detailEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, detailCallBack, "请至少选中一行");
            }
        }

        /**
         * 绑定事件
         */
        function bindEvent() {
            $("#withdrawCash_index_form")
                .on("click", "#withdrawCash_index_btn_seach", searchEvent)
                .on("click", "a[data-id='WithdrawCash_Setting']", settingEvent)
                .on("click", "a[data-id='WithdrawCash_Edit']", editEvent)
                .on("click", "a[data-id='WithdrawCash_Detail']",detailEvent)
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