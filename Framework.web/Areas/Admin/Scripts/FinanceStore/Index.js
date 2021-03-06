﻿$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("店铺财务管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);

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
         * 查询数据的回调方法
         * @param {type} pageNumber
         * @param {type} pageSize
         */
        function searchStoreCallBack(pageNumber, pageSize) {
            try {
                if ($("#store_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#store_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/FinanceStore/GetDetailStoreList", param, function (r) {
                        $("#store_index_datagrid").datagrid("loadData", r.Data.list.Result);
                        $("#store_index_datagrid").datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.list.MaxCount
                        });
                        // 可提现金额显示 tStoreAviliableMoney
                        $("#store_index_span_withdrawMoney").text(r.Data.tStoreAviliableMoney.iprice.toFixed(2));
                        $("#store_index_span_applayMoney").text(r.Data.tStoreAviliableMoney.applayMoney.toFixed(2));

                        //服务费的显示和总价显示
                        $("#store_index_span_amountMoney").text(r.Data.totalPrices.toFixed(2));
                        $("#store_index_span_serviceMoney").text(r.Data.totalServer.toFixed(2));

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

            //绑定提现金额
            var withdrawMoney = $("#store_index_span_withdrawMoney").text();
            if (withdrawMoney == "" || Number(withdrawMoney) <= 0) $("#store_index_span_withdrawMoney").text("0.00");

            //绑定申请中金额
            var applyMoney = $("#store_index_span_applayMoney").text();
            if (applyMoney == "" || Number(applyMoney) <= 0) $("#store_index_span_applayMoney").text("0.00");

            //绑定数据
            eui.bindPaginationEvent($("#store_index_datagrid"), {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#store_index_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                singleSelect: true,
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'sOrderNo', title: '订单号', align: 'center', width: 100 },
                {
                    field: 'dChangeTime', title: '订单时间', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = row.sType == 3 ? "" : value;
                    }
                },
                { field: 'sUserName', title: '用户', align: 'center', width: 100 },
                {
                    field: 'iType', title: '类别', align: 'center', width: 100, formatter: function (value, row, index) {
                        //3：退款 4：收入 1：店铺提现
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
                        if (row.iType == 4 || row.iType == 3) {
                            return "<span style='color:red'>+￥" + (parseFloat(row.iPrice) + parseFloat(row.iServicePrice) + parseFloat(row.iCommissionPrice)).toFixed(2) + "</span>"
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
                            return "<span style='color:red'>+￥" + price + "</span>";
                        }
                        else {
                            return "<span style='color:green'>-￥" + price + "</span>";
                        }
                    }
                }
                ]],
                onLoadSuccess: function (data) {
                }
            }, searchStoreCallBack).pagination("select");
        }

        /**
         * 申请提现
         */
        function applyWithdawCash() {
            var div = $("<div/>");
            div.dialog({
                title: "申请提现",
                width: 500,
                height: 400,
                cache: false,
                href: '/Admin/FinanceStore/Apply',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '提交',
                    iconCls: 'icon-save',
                    handler: function () {
                        var sCode = modules.get(enums.Modules.CACHE).getCache("_financeStoreGetmcode");
                        if (true || sCode === true) {
                            if ($("#store_apply_form").form("validate")) {
                                //想提现的金额
                                var wantMoney = $("#store_apply_input_iWithdrawMoney").val();
                                //判断提现金额是否大于最低可提现额度
                                var store_apply_iMinimumMoney = $("#store_apply_iMinimumMoney").val();
                                if (parseFloat(wantMoney) < parseFloat(store_apply_iMinimumMoney)) {
                                    return eui.alertInfo("提现金额不能小于最低可提现额度");
                                }
                                //判断提现金额是否大于可提现金额

                                var aviliableMoney = $("#store_apply_span_valiableMoney").text();
                                if (parseFloat(wantMoney) > parseFloat(aviliableMoney)) {
                                    return eui.alertInfo("提现金额不能高于可提现金额");
                                }

                                //序列化提交数据
                                var json = $("#store_apply_form").serializeObject();
                                f.post("/Admin/FinanceStore/DoApply", json,
                                    function (ret) {
                                        modules.get(enums.Modules.CACHE).cleanCache("_financeStoreGetmcode");
                                        eui.alertInfo("提现申请已提交");
                                        $(div).dialog("close");
                                        store_index_datagrid.datagrid('getPager').pagination("select");
                                    }, function (ret) {
                                        //modules.get(enums.Modules.CACHE).cleanCache("_financeStoreGetmcode");
                                        eui.alertErr(ret.Msg);
                                    });
                            }
                        } else {
                            if (sCode === null) {
                                eui.alertErr("请获取短信验证码");
                            } else {
                                eui.alertErr("短信验证码已过期，请重新获取");
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
                    //设置短信验证码
                    $("#store_apply_a_getCode").on("click", getVCode);
                }
            });
        }

        /**
         * 获取短信验证码
         */
        function getVCode() {
            modules.get(enums.Modules.CACHE).setCache("_financeStoreGetmcode", null);
            f.post("/Admin/FinanceStore/SendMessage", null, function (r) {
                modules.get(enums.Modules.CACHE).setCache("_financeStoreGetmcode", true);
                $("#store_apply_a_getCode").off("click");
                $("#store_apply_a_getCode").linkbutton("disable");
                countDown(120);
            }, function (r) {
                eui.alertErr(r.Msg);
            });
        }

        /**
         * 获取验证码倒数显示
         * @param {type} ls
         */
        function countDown(ls) {
            setTimeout(function () {
                if (ls > 0) {
                    ls--;
                    $("#store_apply_a_getCode").linkbutton({ text: "获取验证码({0})".format(ls) });
                    countDown(ls);
                } else {
                    $("#store_apply_a_getCode").linkbutton({ text: "获取验证码" });
                    $("#store_apply_a_getCode").linkbutton("enable");
                    $("#store_apply_a_getCode").on("click", getVCode);
                    modules.get(enums.Modules.CACHE).setCache("_financeStoreGetmcode", false);
                }
            }, 1000);
        }

        /**
         * 绑定 店铺 详细事件
         */
        function bindStoreDetailEvent() {
            //绑定事件
            var store_index_datagrid = $("#store_index_datagrid");
            var sStartStr = "#store_index_dValidDateStart", sEndStr = "#store_index_dValidDateEnd";
            $("#store_index_form")
                .on("click", "#store_index_btn_seach", function () {
                    //搜索
                    var selectStartTime = $("#store_index_input_dValidDateStart").val();
                    var selectEndTime = $("#store_index_input_dValidDateEnd").val();
                    if (selectStartTime != "" && selectEndTime != "") {
                        if (new Date(selectStartTime) > new Date(selectEndTime)) {
                            return eui.alertInfo("开始时间不能大于结束时间");
                        }
                    }
                    if (selectStartTime != "") selectStartTime = selectStartTime + " 00:00:00";
                    if (selectEndTime != "") selectEndTime = selectEndTime + "  23:59:59";
                    setTimeParam(selectStartTime, selectEndTime, sStartStr, sEndStr);
                    store_index_datagrid.datagrid('getPager').pagination("select");

                })
            .on("click", "#store_index_a_all", function () {
                //全部
                setTimeParam("", "2080-12-31", sStartStr, sEndStr);
                store_index_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#store_index_a_today", function () {
                //今天
                setTodayParam(sStartStr, sEndStr);
                store_index_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#store_index_a_yesterday", function () {
                //昨天
                setYesterdayParam(sStartStr, sEndStr);
                store_index_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#store_index_a_week", function () {
                //最近一周
                setCurrentParam(sStartStr, sEndStr);
                store_index_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#store_index_a_month", function () {
                //最近一个月
                setCurrentMonth(sStartStr, sEndStr);
                store_index_datagrid.datagrid('getPager').pagination("select");
            })
            .on("click", "#store_index_a_withdraw", function () {

                //判断现在是否能够提现
                f.post("/Admin/FinanceStore/IsWithdrawAble", null, function (res) {
                    //申请提现
                    applyWithdawCash();
                }, function (res) {
                    eui.alertInfo(res.Msg);
                });
            });
        }

        try {
            initStoreDataGrid();
            bindStoreDetailEvent();
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