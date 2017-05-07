$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("操作日志", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var pageNumber = 1, pageSize = 10;
        var grid = $("#operateLog_index_datagrid");

        /**
        * 初始化数据
        */
        function initDataGrid() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#operateLog_index_tool_div",
                fit: true,
                fitColumns: true,
                pagination: true,
                rownumbers: true,
                sortOrder: "desc",
                //singleSelect: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'ID', title: '主键', align: 'center', width: 100, hidden: true },
                {
                    field: 'iState', title: '类型', align: 'center', width: 100, formatter: function (value, row, index) {
                        //类型(0-商品上架 1-商品下架 2-冻结用户)
                        var type = "";
                        switch (value) {
                            case 0:
                                type = "商品上架";
                                break;
                            case 1:
                                type = "商品上架";
                                break;
                            case 2:
                                type = "冻结用户";
                                break;

                        }
                        return type;
                    }
                },
                {
                    field: 'dOperatTime', title: '操作时间', align: 'center', width: 100, formatter: function (value, row, index) {
                        return new Date(value).Format("yyyy-MM-dd");
                    }
                },
                { field: 'sOperator', title: '操作人', align: 'center', width: 100 },
                { field: 'sContent', title: '内容', align: 'center', width: 100 }
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
                if ($("#operateLog_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#operateLog_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/OperateLog/LoadData", param, function (r) {
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
            var startTime=$("#operateLog_index_dValidDateStart").val();
            var endTime=$("#operateLog_index_dValidDateEnd").val();
            if (new Date(startTime) > new Date(endTime)) {
                return eui.alertInfo("开始时间不能大于结束时间");
            }
            searchCallBack(pageNumber, pageSize);
        }

        /**
         * 绑定事件
         */
        function bindEvent() {
            $("#operateLog_index_form")
            .on("click", "#operateLog_index_btn_seach", searchEvent);
        }


        try {
            initDataGrid();
            bindEvent();
        } catch (e) {

        }

    });
});