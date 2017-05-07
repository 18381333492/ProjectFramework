/// <reference path="E:\项目集合\成都亿合科技\源码\友客分享商城\ProjectFramework\Framework.web\scripts/lib/easyui.module.js" />

$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("评论管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var pageNumber = 1, pageSize = 10;
        var grid = $("#comment_index_datagrid");
        /**
          * 初始化数据
          */
        function initDataGrid() {

            $("#comment_show_big_pic").dialog({
                title: "查看大图",
                //width: 650,
                //height: 600,
                cache: false,                
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                maximized: true,                
                buttons: [{
                    text: '关闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        $("#comment_show_big_pic").dialog("close");
                    }
                }],
                onClose: function () {                   
                },
                onLoad: function () {                    
                }
            }).dialog("close");

            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#comment_index_tool_div",
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
                { field: 'sGoodsName', title: '商品名称', align: 'center', width: 100 },
                { field: 'sOrderNo', title: '订单号', align: 'center', width: 100 },
                { field: 'sStoreName', title: '店铺名称', align: 'center', width: 100 },
                { field: 'IsStore', title: '是否是店铺登录', align: 'center', width: 100, hidden: true },
                {
                    field: 'sCommentContent', title: '评价内容', align: 'center', width: 100, formatter: function (value, row, index) {
                        return res = value.length > 20 ? value.substr(0, 20) + "..." : value;
                    }
                },
                { field: 'dCommentTime', title: '评价时间', align: 'center', width: 100 },
                { field: 'sCommenterName', title: '评价人', align: 'center', width: 100 },
                {
                    field: 'bIsReplay', title: '状态', align: 'center', width: 100, formatter: function (value, row, index) {
                        return value = value === false ? "未回复" : "已回复";
                    }
                }
                ]],
                onLoadSuccess: function (data) {
                    if (data.total > 0) {
                        if (data.rows[0].IsStore === 'true') {
                            grid.datagrid('hideColumn', 'sStoreName'); // 设置隐藏列    
                        }
                    }
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
                if ($("#comment_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#comment_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/Comment/LoadData", param, function (r) {
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
         * 格式化提示信息的函数
         */
        function formatConfirmMSGCallBack(selectedRow) {
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
            f.post("/Admin/Comment/DoDelete", { rowIDs: rowIDs.toString() }, function (res) {
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
         * 展示大图
         */
        function showBigImgeEvent() {
            $("#comment_show_big_pic img").attr("src", $(this).attr("src"));
            $("#comment_show_big_pic").dialog("open");
        }

        /**
        * 显示编辑窗口
        */
        function showEditView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑",
                width: 650,
                height: 500,
                cache: false,
                href: '/Admin/Comment/Edit?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '确认',
                    iconCls: 'icon-save',
                    handler: function () {
                        var isReplay = $("#comment_edit_bIsReplay").val();
                        if (isReplay == "true") {
                            $(div).dialog("close");
                        } else {
                            if ($("#comment_edit_form").form("validate")) {
                                var sReplayContent = $("#comment_edit_sReplayContent").val();
                                //验证
                                if (sReplayContent == "") {
                                    return eui.alertInfo("请输入回复信息");
                                }
                                if (sReplayContent.length>150) {
                                    return eui.alertInfo("回复信息最多150个字符");
                                }
                                if ($("<span/>").html(sReplayContent).text() != sReplayContent) {
                                    return eui.alertInfo("禁止输入非法的html字符");
                                }

                                //ID
                                var ID = $("#comment_edit_ID").val();
                                //执行编辑
                                f.post("/Admin/Comment/DoEdit", { ID: ID, sReplayContent: sReplayContent }, function (res) {
                                    eui.alertInfo("回复成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (res) {
                                    eui.alertErr(res.Msg);
                                });
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
                    //展示大图
                    $("#comment_edit_imges_div").find("img[name='comment_edit_imges']").on("click", showBigImgeEvent);
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
        function editEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, editCallBack, "请至少选中一行");
            }
        }

        /**
         * 查询【事件】
         */
        function searchEvent() {
            searchCallBack(pageNumber, pageSize);
        }

        /**
         * 绑定事件
         */
        function bindEvent() {
            $("#comment_index_form")
                .on("click", "#comment_index_btn_seach", searchEvent)
                .on("click", "a[data-id='Comment_Delete']", deleteEvent)
                .on("click", "a[data-id='Comment_Edit']", editEvent)
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
});