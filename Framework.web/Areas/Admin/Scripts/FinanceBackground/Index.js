$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("平台财务管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var grid = $("#background_index_datagrid");

        /**
         * 设置时间参数数据
         */
        function setTimeParam(dStartTime, dEndTime, objStartTime, objEndTime) {
            $(objStartTime).val(dStartTime);
            $(objEndTime).val(dEndTime);
        }

        /**
         * 设置最近一个月的时间【设置参数】
         */
        function setCurrentMonth(objStartTime, objEndTime) {
            var now = new Date();
            var year = now.getFullYear();
            var month = now.getMonth();
            var dStartTime = new Date(year, month, 1).toLocaleDateString() + " 00:00:00";
            var dEndTime = new Date(year, month + 1, 0).toLocaleDateString() + " 23:59:59";
            setTimeParam(dStartTime, dEndTime, objStartTime, objEndTime);
        }

        /**
         * 设置最近一周时间【设置参数】
         */
        function setCurrentParam(objStartTime, objEndTime) {
            var now = new Date();
            var dStartTime = null;
            var dEndTime = null;
            var week = now.getDay(); //(星期为1234560)
            if (week != 0) {

                dStartTime = new Date(now.getTime() - (week - 1) * 24 * 60 * 60 * 1000).toLocaleDateString() + " 00:00:00";
                dEndTime = new Date(now.getTime() - (week - 7) * 24 * 60 * 60 * 1000).toLocaleDateString() + " 23:59:59";
            }
            else {
                dStartTime = new Date((now.getTime() - 6 * 24 * 60 * 60 * 1000)).toLocaleDateString() + " 00:00:00";
                dEndTime = new Date().toLocaleDateString() + " 23:59:59";
            }

            setTimeParam(dStartTime, dEndTime, objStartTime, objEndTime);
        }

        /**
         * 设置昨天时间【设置参数】
         */
        function setYesterdayParam(objStartTime, objEndTime) {
            var now = new Date();
            var dStartTime = new Date(now.getTime() - 1 * 24 * 60 * 60 * 1000).toLocaleDateString() + " 00:00:00";
            var dEndTime = new Date(now.getTime() - 1 * 24 * 60 * 60 * 1000).toLocaleDateString() + " 23:59:59";
            setTimeParam(dStartTime, dEndTime, objStartTime, objEndTime);
        }

        /**
         * 设置今天时间参数
         */
        function setTodayParam(objStartTime, objEndTime) {
            var dStartTime = new Date().toLocaleDateString() + " 00:00:00";
            var dEndTime = new Date().toLocaleDateString() + " 23:59:59";
            setTimeParam(dStartTime, dEndTime, objStartTime, objEndTime);
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
         * 详细【事件】
         */
        function detailEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, detailCallBack, "请至少选中一行");
            }
        }

        /**
         * 统计全部【事件】
         */
        function searchAllEvent() {
            setTimeParam("", "2080-12-31", "#background_index_dValidDateStart", "#background_index_dValidDateEnd");//不设置时间参数
            grid.datagrid('getPager').pagination("select");//查询数据  会去执行searchCallBack 回调函数
        }

        /**
         * 统计今日【事件】
         */
        function searchTodayEvent() {
            setTodayParam("#background_index_dValidDateStart", "#background_index_dValidDateEnd");
            grid.datagrid('getPager').pagination("select");
        }

        /**
         * 统计昨天【事件】
         */
        function searchYesterdayEvent() {
            setYesterdayParam("#background_index_dValidDateStart", "#background_index_dValidDateEnd");
            grid.datagrid('getPager').pagination("select");
        }

        /**
         * 统计最近一周【事件】
         */
        function searchWeekEvent() {
            setCurrentParam("#background_index_dValidDateStart", "#background_index_dValidDateEnd");
            grid.datagrid('getPager').pagination("select");
        }

        /**
         * 统计最近一个月【事件】
         */
        function searchMonthEvent() {
            setCurrentMonth("#background_index_dValidDateStart", "#background_index_dValidDateEnd");
            grid.datagrid('getPager').pagination("select");
        }

        /**
         * 搜索【事件】
         */
        function searchEvent() {
            var selectStartTime = $("#background_index_input_orderDateStart").val();
            var selectEndTime = $("#background_index_input_orderDateEnd").val();
            if (selectStartTime != "" && selectEndTime != "") {
                if (new Date(selectStartTime) > new Date(selectEndTime)) {
                    return eui.alertInfo("开始时间不能大于结束时间");
                }
            }
            if (selectStartTime != "") selectStartTime = selectStartTime + " 00:00:00";
            if (selectEndTime != "") selectEndTime = selectEndTime + "  23:59:59";
            setTimeParam(selectStartTime, selectEndTime, "#background_index_dValidDateStart", "#background_index_dValidDateEnd");
            grid.datagrid('getPager').pagination("select");
        }

        /**
        * 绑定事件
        */
        function bindEvent() {
            $("#background_index_form")
                .on("click", "#background_index_a_detail", detailEvent)//详情进入
                .on("click", "#background_index_a_all", searchAllEvent)
                .on("click", "#background_index_a_today", searchTodayEvent)
                .on("click", "#background_index_a_yesterday", searchYesterdayEvent)
                .on("click", "#background_index_a_week", searchWeekEvent)
                .on("click", "#background_index_a_month", searchMonthEvent)
                .on("click", "#background_index_btn_seach", searchEvent)
        }

        /**
         * 初始化数据
         */
        function initDataGrid() {
            //默认显示最近一个月
            setCurrentMonth();
            //绑定数据
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#background_index_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                singleSelect: true,
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'client', title: '主键', align: 'center', width: 100, hidden: true },
                { field: 'Name', title: '名称', align: 'center', width: 100 },
                {
                    field: 'iClientType', title: '角色', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (value) {
                            case 1:
                                value = "分享客";
                                break;
                            case 2:
                                value = "店铺";
                                break;
                            case 3:
                                value = "合伙人";
                                break;
                        }

                        return value;
                    }
                },
                {
                    field: 'iPrice', title: '金额', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = parseFloat(value) > 0 ?
                            "<span style='color:red'>+￥" + value.toFixed(2) + "</span>" : "<span style='color:green'>-￥" + (-parseFloat(value)).toFixed(2) + "</span>";
                    }
                }
                ]],
                onLoadSuccess: function (data) {

                }
            }, searchCallBack).pagination("select");
        }

        /**
         * 查询数据的回调方法
         * @param {type} pageNumber
         * @param {type} pageSize
         */
        function searchCallBack(pageNumber, pageSize) {
            try {
                if ($("#background_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#background_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/FinanceBackground/LoadData", param, function (r) {
                        grid.datagrid("loadData", r.Data.Result);
                        grid.datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });
                        $("#background_index_span_serviceMoney").text(r.Msg);
                    }, function (r) {
                        eui.alertErr(r.Msg);
                    });
                }
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        /**
         * 绑定 分享客 详细事件
         */
        function bindShareDetailEvent() {
            var background_share_details_datagrid = $("#background_share_details_datagrid");
            var sStartStr = "#background_share_details_dValidDateStart", sEndStr = "#background_share_details_dValidDateEnd";
            $("#background_share_details_form")
                .on("click", "#background_share_details_btn_seach", function () {
                    //搜索
                    debugger
                    var selectStartTime = $("#background_share_details_input_dValidDateStart").val();
                    var selectEndTime = $("#background_share_details_input_dValidDateEnd").val();
                    if (selectStartTime != "" && selectEndTime != "") {
                        if (new Date(selectStartTime) > new Date(selectEndTime)) {
                            return eui.alertInfo("开始时间不能大于结束时间");
                        }
                    }
                    if (selectStartTime != "") selectStartTime = selectStartTime + " 00:00:00";
                    if (selectEndTime != "") selectEndTime = selectEndTime + "  23:59:59";
                    setTimeParam(selectStartTime, selectEndTime, sStartStr, sEndStr);
                    background_share_details_datagrid.datagrid('getPager').pagination("select");

                })
            .on("click", "#background_share_details_a_all", function () {
                //全部
                setTimeParam("", "2080-12-31", sStartStr, sEndStr);
                background_share_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_share_details_a_today", function () {
                //今天
                setTodayParam(sStartStr, sEndStr);
                background_share_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_share_details_a_yesterday", function () {
                //昨天
                setYesterdayParam(sStartStr, sEndStr);
                background_share_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_share_details_a_week", function () {
                //最近一周
                setCurrentParam(sStartStr, sEndStr);
                background_share_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_share_details_a_month", function () {
                //最近一个月
                setCurrentMonth(sStartStr, sEndStr);
                background_share_details_datagrid.datagrid('getPager').pagination("select");
            });
        }

        /**
         * 绑定 合伙人 详细事件
         */
        function bindPartnerDetailEvent() {
            var background_partner_details_datagrid = $("#background_partner_details_datagrid");
            var sStartStr = "#background_partner_details_dValidDateStart", sEndStr = "#background_partner_details_dValidDateEnd";
            $("#background_partner_details_form")
                .on("click", "#background_partner_details_btn_seach", function () {
                    //搜索
                    debugger
                    var selectStartTime = $("#background_partner_details_input_dValidDateStart").val();
                    var selectEndTime = $("#background_partner_details_input_dValidDateEnd").val();
                    if (selectStartTime != "" && selectEndTime != "") {
                        if (new Date(selectStartTime) > new Date(selectEndTime)) {
                            return eui.alertInfo("开始时间不能大于结束时间");
                        }
                    }
                    setTimeParam(selectStartTime, selectEndTime, sStartStr, sEndStr);
                    background_partner_details_datagrid.datagrid('getPager').pagination("select");

                })
            .on("click", "#background_partner_details_a_all", function () {
                //全部
                setTimeParam("", "2080-12-31", sStartStr, sEndStr);
                background_partner_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_partner_details_a_today", function () {
                //今天
                setTodayParam(sStartStr, sEndStr);
                background_partner_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_partner_details_a_yesterday", function () {
                //昨天
                setYesterdayParam(sStartStr, sEndStr);
                background_partner_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_partner_details_a_week", function () {
                //最近一周
                setCurrentParam(sStartStr, sEndStr);
                background_partner_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_partner_details_a_month", function () {
                //最近一个月
                setCurrentMonth(sStartStr, sEndStr);
                background_partner_details_datagrid.datagrid('getPager').pagination("select");
            });
        }

        /**
         * 绑定 店铺 详细事件
         */
        function bindStoreDetailEvent() {
            //绑定事件
            var background_store_details_datagrid = $("#background_store_details_datagrid");
            var sStartStr = "#background_store_details_dValidDateStart", sEndStr = "#background_store_details_dValidDateEnd";
            $("#background_store_details_form")
                .on("click", "#background_store_details_btn_seach", function () {
                    //搜索
                    debugger
                    var selectStartTime = $("#background_store_details_input_dValidDateStart").val();
                    var selectEndTime = $("#background_store_details_input_dValidDateEnd").val();
                    if (selectStartTime != "" && selectEndTime != "") {
                        if (new Date(selectStartTime) > new Date(selectEndTime)) {
                            return eui.alertInfo("开始时间不能大于结束时间");
                        }
                    }
                    setTimeParam(selectStartTime, selectEndTime, sStartStr, sEndStr);
                    background_store_details_datagrid.datagrid('getPager').pagination("select");

                })
            .on("click", "#background_store_details_a_all", function () {
                //全部
                setTimeParam("", "2080-12-31", sStartStr, sEndStr);
                background_store_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_store_details_a_today", function () {
                //今天
                setTodayParam(sStartStr, sEndStr);
                background_store_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_store_details_a_yesterday", function () {
                //昨天
                setYesterdayParam(sStartStr, sEndStr);
                background_store_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_store_details_a_week", function () {
                //最近一周
                setCurrentParam(sStartStr, sEndStr);
                background_store_details_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#background_store_details_a_month", function () {
                //最近一个月
                setCurrentMonth(sStartStr, sEndStr);
                background_store_details_datagrid.datagrid('getPager').pagination("select");
            });
        }

        /**
        * 详细回调
        * @param {type} selectRow 选中行
        */
        function detailCallBack(selectRow) {
          
            var sRole = selectRow.iClientType;
            var url = "";//1 分享客：2 店铺：3合伙人
            switch (sRole) {
                case 1:
                    url = '/Admin/FinanceBackground/DetailShare?ID=' + selectRow.client;//分享客：1
                    break;
                case 2:
                    url = '/Admin/FinanceBackground/DetailStore?ID=' + selectRow.client;//店铺：2
                    break;
                case 3:
                    url = '/Admin/FinanceBackground/DetailPartner?ID=' + selectRow.client;//合伙人：3
                    break;

            }
            //内容框
            var div = $("<div/>");
            div.dialog({
                title: "详细",
                width: 1050,
                height: 700,
                cache: false,
                href: url,
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
                    switch (sRole) {
                        case 1://分享客
                          //  $("#background_share_details_ID").val(selectRow.ID);
                            initShareDataGrid();
                            bindShareDetailEvent();
                            break;
                        case 2://店铺
                          //  $("#background_store_details_ID").val(selectRow.ID);
                            //设置金额总计
                            $("#background_store_details_span_amountMoney").text(selectRow.iPrice);
                            initStoreDataGrid();
                            bindStoreDetailEvent();
                            break;
                        case 3://合伙人
                        //    $("#background_partner_details_ID").val(selectRow.ID);
                            initPartnerDataGrid();
                            bindPartnerDetailEvent();
                            break;
                    }
                }
            });
        }

        //---------------------------------------------------------------------------------------------------------------店铺详情

        /**
         * 查询数据的回调方法
         * @param {type} pageNumber
         * @param {type} pageSize
         */
        function searchStoreCallBack(pageNumber, pageSize) {
            try {
                if ($("#background_store_details_form").form("validate")) {
                    var param/*查询参数*/ = $("#background_store_details_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/FinanceBackground/GetDetailStoreList", param, function (r) {
                        $("#background_store_details_datagrid").datagrid("loadData", r.Data.list.Result);
                        $("#background_store_details_datagrid").datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });

                        $("#background_store_details_span_serviceMoney").text(r.Data.totalServer.toFixed(2));
                        $("#background_store_details_span_amountMoney").text(r.Data.totalPrices.toFixed(2));

                    }, function (r) {
                        eui.alertErr(r.Msg);
                    });
                }
            } catch (e) {
                eui.alertErr(e.message);
            }
        }
        /**
         * 初始化店铺详情
         */
        function initStoreDataGrid() {
            //绑定数据
            eui.bindPaginationEvent($("#background_store_details_datagrid"), {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#background_store_details_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                singleSelect: true,
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                    { field: 'iMethod', title: '变动方式', align: 'center', width: 100, hidden: true },
                { field: 'iShopID', title: '店铺ID', align: 'center', width: 100, hidden: true },
                { field: 'sOrderNo', title: '订单号', align: 'center', width: 100 },
                {
                    field: 'dChangeTime', title: '订单时间', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.sType == 3 ? "" : value;
                    }
                },
                { field: 'sUserName', title: '用户', align: 'center', width: 100 },
                {
                    field: 'iType', title: '类别', align: 'center', width: 100, formatter: function (value, row, index) {
                        //1：退款 2：收入 3：店铺提现
                        switch (value) {
                            case 3:
                                value = "退款";
                                break;
                            case 4:
                                value = "收入";
                                break;
                            case 1:
                                value = "店铺提现";
                                break;
                        }

                        return value;
                    }
                },
                {
                    field: 'ID', title: '实付金额', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.iType == 4 ||row.iType==3) {
                            return "<span style='color:red'>+￥" + (parseFloat(row.iPrice) +parseFloat(row.iServicePrice) +parseFloat(row.iCommissionPrice)).toFixed(2) + "</span>"
                        }
                        else {
                            return "N/A"
                        }
                    }
                },
                {
                    field: 'iServicePrice', title: '服务费', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.iType == 3 || row.iType == 1) return "N/A";
                        else return value;
                    }
                },
                {
                    field: 'iCommissionPrice', title: '佣金', align: 'center', width: 100, formatter: function (value, row, index) {
                        if (row.iType == 3 || row.iType == 1) return "N/A";
                        else return value;
                    }
                },
                {
                    field: 'iPrice', title: '金额', align: 'center', width: 100, formatter: function (value, row, index) {
                        var price = value;
                        if (row.iType == 4) {
                            return "<span style='color:red'>+￥" + price.toFixed(2) + "</span>";
                        }
                        else {
                            return "<span style='color:green'>-￥" + price.toFixed(2) + "</span>";
                        }
                    }
                }
                ]],
                onLoadSuccess: function (data) {
                    //data.rows[0].iServicePrice  服务费总计
                 
                    //if (data.total > 0) {
                    //    var iTotlePrice = "";;
                    //    if (parseFloat(data.rows[0].iTotlePrice) > 0) {
                    //        iTotlePrice = "+" + data.rows[0].iTotlePrice
                    //    } else {
                    //        iTotlePrice = data.rows[0].iTotlePrice;
                    //    }
                    //    $("#background_store_details_span_serviceMoney").text(data.rows[0].iTotleServicePrice);
                    //    $("#background_store_details_span_amountMoney").text(iTotlePrice);
                    //} else {
                    //    $("#background_store_details_span_serviceMoney").text("0.00");
                    //    $("#background_store_details_span_amountMoney").text("0.00");
                    //}
                }
            }, searchStoreCallBack).pagination("select");
        }

        //---------------------------------------------------------------------------------------------------------------合伙人详情

        /**
        * 查询数据的回调方法
        * @param {type} pageNumber
        * @param {type} pageSize
        */
        function searchPartnerCallBack(pageNumber, pageSize) {
            try {
                if ($("#background_partner_details_form").form("validate")) {
                    var param/*查询参数*/ = $("#background_partner_details_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/FinanceBackground/GetDetailPartnerList", param, function (r) {
                        $("#background_partner_details_datagrid").datagrid("loadData", r.Data.list.Result);
                        $("#background_partner_details_datagrid").datagrid("getPager").pagination({
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
         * 初始化合伙人详情
         */
        function initPartnerDataGrid() {
            //绑定数据
            eui.bindPaginationEvent($("#background_partner_details_datagrid"), {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#background_partner_details_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                singleSelect: true,
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                    { field: 'iMethod', title: '变动方式', align: 'center', width: 100, hidden: true },
                {
                    field: 'sOrderNo', title: '订单号', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'dChangeTime', title: '订单时间', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'sUserName', title: '购买用户', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'sShopName', title: '所属店铺', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'iType', title: '类别', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (value) {
                            case 1:
                                value = "提现";
                                break;
                            case 3:
                                value = "退款";
                                break;
                            case 4:
                                value = "提成收入";
                                break;
                        }
                        return value;
                    }
                },
                {
                    field: 'iPrice', title: '金额', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 4 ?
                            "<span style='color:red'>+￥" + value.toFixed(2) + "</span>" : "<span style='color:green'>￥" + value.toFixed(2) + "</span>";
                    }
                }
                ]]
            }, searchPartnerCallBack).pagination("select");
        }

        //---------------------------------------------------------------------------------------------------------------分享客详情
        /**
        * 查询数据的回调方法
        * @param {type} pageNumber
        * @param {type} pageSize
        */
        function searchShareCallBack(pageNumber, pageSize) {
            try {
                if ($("#background_share_details_form").form("validate")) {
                    var param/*查询参数*/ = $("#background_share_details_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/FinanceBackground/GetDetailShareList", param, function (r) {
                        $("#background_share_details_datagrid").datagrid("loadData", r.Data.Result);
                        $("#background_share_details_datagrid").datagrid("getPager").pagination({
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
         * 初始化分享客详情
         */
        function initShareDataGrid() {
            //绑定数据
            eui.bindPaginationEvent($("#background_share_details_datagrid"), {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#background_share_details_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                singleSelect: true,
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                    { field: 'iMethod', title: '变动方式', align: 'center', width: 100, hidden: true },
                {
                    field: 'sOrderNo', title: '订单号', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'dChangeTime', title: '订单时间', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'sReceiver', title: '购买用户', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'sShopName', title: '所属店铺', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iType == 1 ? "N/A" : value;
                    }
                },
                {
                    field: 'iType', title: '类别', align: 'center', width: 100, formatter: function (value, row, index) {
                        switch (value) {
                            case 1:
                                value = "提现";
                                break;
                            case 5:
                                value = "佣金收入";
                                break;
                        }

                        return value;
                    }
                },
                {
                    field: 'iPrice', title: '金额', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.iMethod == 2 ?
                            "<span style='color:red'>+￥" + value.toFixed(2) + "</span>" : "<span style='color:green'>-￥" + (-parseFloat(value)).toFixed(2) + "</span>";
                    }
                }
                ]]
            }, searchShareCallBack).pagination("select");
        }



        try {
            initDataGrid();
            bindEvent();
        } catch (e) {
            eui.alertErr(e);
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
})