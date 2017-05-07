$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("文章管理", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#article_manage_menu");
        var travelObj = {};
       

        //初始化datagid
        function init(){
            eui.bindPaginationEvent(grid, {
                
           
            loadMsg: "正在加载...",
            toolbar: "#article_manager_tool",
            pagination: true,
            rownumbers: true,
            
            fit:true,
            //下面的列都是一些示例，用的时候直接替换
            columns: [[
            { field: 'checkbox', checkbox: true },
            { field: 'ID', title: 'ID', align: 'center', width: 260 ,hidden:true},
            { field: 'sTitle', title: '标题', align: 'center', width: 300 },
            { field: 'dPublishTime', title: '发布时间', align: 'center', width: 300},
            ]],
            onLoadSuccess: function () {
                $(".datagrid-header-check input[type=checkbox]").remove();
            }, rowStyler: function (index, row) {
                if (row.tUserState === 1) {
                    return 'background-color:gray;color:white';
                }
            }
        }, loadDate).pagination("select");
    }

        //查询数据的回调方法
        function loadDate(pageNumber, pageSize) {
            try {
                if ($("#article_manager_form").form("validate")) {
                    var param/*查询参数*/ = $("#article_manager_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/Article/GetList", { PageIndex: pageNumber, PageSize: pageSize }, function (r) {
                        
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
        //绑定按钮事件
        function initEvent() {
            $("#article_manager_form")
                .on("click", "a[data-id='add_article_button']", articleAdd)
                .on("click", "a[data-id='edit_article_button']", articleEdit)
                .on("click", "a[data-id='delete_article_button']", articleDelete)
              
        }


        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        var div ="";
        function showDialog(title,url,func) {
            div = $("<div/>");
            div.dialog({
                title: title,
                width: 900,
                height: 600,
                cache: false,
                href:url,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [
                    {
                        text: '预览',
                        iconCls: 'icon-save',
                        handler: function () {
                            func(true);  
                        }
                    },
                    {
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        func();      
                    }
                }, {
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
                    if (title == "添加文章") {
                        travelObj.editorEE = ue.initUE('add_Article_sContent_script');
                    }
                    else {
                        travelObj.editorEE = ue.initUE('edit_Article_sContent_script', $("#edit_articl_Content").val());
                    }
                }
            });
        }

        //预览文章
        function Preview(ID) {
            var div = $("<div/>");
            div.dialog({
                title: "文章预览",
                width: 300,
                height: 300,
                href: '/Admin/Article/Preview',
                modal: true,
                buttons: [
                    {//关闭
                        text: '关闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $(div).dialog("destroy");
                        }
                    },
                ],
                onLoad: function () {
                    $("#code").qrcode({
                        //render: "html",
                        width: 160, //宽度 
                        height: 160, //高度 
                        text: 'http://' + window.location.host + '/Client/ClientCenter/HelpCenterDeatil?ID=' + ID //任意内容 
                    });
                }
            });
        }

        //添加文章
        function articleAdd() {
            showDialog("添加文章",'/Admin/Article/Add', addwz);
        }
        //添加文章回调
        function addwz(type) {
            if (!$("#add_Article_form").form('validate')) { return false; }
            var json = $("#add_Article_form").serializeObject();
            if (travelObj.editorEE.getContent() == "") {
                return eui.alertInfo("请输入内容");
            } else {
                json.sContent = travelObj.editorEE.getContent();
                if (type == true) {
                    f.post('/Admin/Article/PreviewArticle', json, function (res) {
                        Preview(res.Data);
                    });
                }
                else {
                    f.post('/Admin/Article/Insert', json, function (res) {
                        reloadData();
                        eui.alertInfo("已成功添加");
                        $(div).dialog("close");
                    });
                }
            }
           
        }
        //修改文章
        function articleEdit() {
            eui.checkSelectedRow(grid, edit, "请选择一条您需要修改的数据");
            
        }
        //修改文章回调
        function edit(row)
        {
            var selectedRow = grid.datagrid("getSelections");
            if (selectedRow.length > 1) {
                eui.alertInfo("只能选择一条数据")
                return false;
             }
            showDialog("编辑文章", '/Admin/Article/Edit?ID=' + row.ID, function editwz(type) {
                if (!$("#edit_Article_form").form('validate')) { return false; }
                var json = $("#edit_Article_form").serializeObject();
                if (travelObj.editorEE.getContent() == "") {
                    return eui.alertInfo("请输入内容");
                } else {
                    json.sContent = travelObj.editorEE.getContent();
                    f.post('/Admin/Article/UpdateText', json, function (res) {
                        reloadData();
                        if (type == true) {
                            Preview(res.Data);
                        }
                        else {
                            eui.alertInfo("已成功修改");
                            $(div).dialog("close");
                        }
                    })
                }
             });
            
        }
        //删除文章
        function articleDelete()
        {
            eui.confirmDomainByMultiRows(grid, deleteText, "", "请选择至少一条数据", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个文章".format(rows.length);
            });
        }
        function deleteText(rows)
        {
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < rows.length; i++) {
                rowIDs.push(rows[i].ID);
                if (rows[i].sTitle == "关于分享客" || rows[i].sTitle == "关于合伙人" || rows[i].sTitle == "商家入驻须知" || rows[i].sTitle == "用户协议") {
                    return eui.alertInfo("不能删除");
                }
            }
            f.post('/Admin/Article/DeleteText', { ID: rowIDs.toString() }, function (res) {
                reloadData();
                grid.datagrid("clearSelections");

                eui.alertInfo("删除成功");
            }, function (res) {
                eui.alertErr("删除失败");
            });
        }
        try {
            initEvent();
            init();
        } catch (e) {
            eui.alertErr(e.message);
        }
        

    });
});