
/*=====================================================================
*   下面是创建带分页功能的dialog模板代码，可以直接复制去修改一下
*======================================================================*/
easyui模块.bindPaginationEvent(grid, {
    idField: "ID字段",
    loadMsg: "正在加载...",
    toolbar: "工具栏的ID",
    fit: true,
    fitColumns: true,
    pagination: true,
    rownumbers: true,
    singleSelect: true,
    //下面的列都是一些示例，用的时候直接替换
    columns: [[
    { field: 'checkbox', checkbox: true },
    { field: 'sLoginName', title: '登录名', align: 'center', width: 100 },
    { field: 'sUserName', title: '用户名', align: 'center', width: 100 },
    {
        field: 'tUserState', title: '用户状态', align: 'center', width: 100, formatter: function (value, row, index) {
            switch (row.tUserState) {
                case 0:
                    return "正常";
                case 1:
                    return "冻结";
                default:
                    break;
            }
        }
    },
    {
        field: 'tUserType', title: '用户类型', align: 'center', width: 100, formatter: function (value, row, index) {
            if (row.tUserType === 0) {
                return "平台用户";
            }
        }
    },
    { field: 'sUserNickName', title: '用户昵称', align: 'center', width: 100 },
    { field: 'sMobileNum', title: '手机号', align: 'center', width: 100 },
    { field: 'dCreateTime', title: '创建时间', align: 'center', width: 100 },
    { field: 'dLastLoginTime', title: '最后登录时间', align: 'center', width: 100 }
    ]],
    onLoadSuccess: function () {
        $(".datagrid-header-check input[type=checkbox]").remove();
    }, rowStyler: function (index, row) {
        if (row.tUserState === 1) {
            return 'background-color:gray;color:white';
        }
    }
}, 查询数据的回调方法).pagination("select");

function 查询数据的回调方法(pageNumber, pageSize) {
    try {
        if ($("#systemuser_form").form("validate")) {
            var param/*查询参数*/ = $("#systemuser_form").serializeObject();
            param.PageIndex/*当前页码*/ = pageNumber;
            param.pageSize/*每页显示条数*/ = pageSize;
            f.post("/Admin/SystemUser/LoadSystemUser", param, function (r) {
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