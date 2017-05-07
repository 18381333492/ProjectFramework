$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("首页图文管理", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var grid = $("#home_picture_menu");
        var travelObj = {};

        function init() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#home_picture_tool",
                pagination: true,
                rownumbers: true,

                fit: true,
                //下面的列都是一些示例，用的时候直接替换
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'ID', title: 'ID', align: 'center', width: 260,hidden:true },
                {
                    field: 'sImagePath', title: '图片', align: 'center', width: 300, formatter: function (value, row, index) {
                        return "<img src='" + row.sImagePath + "'  style='height:50px;width:50px'/>"
                    }
                },
                { field: 'dPublishTime', title: '发布时间', align: 'center', width: 200 },
                { field: 'iOrder', title: '排序编号(越小越靠前)', align: 'center', width: 200 },
                {
                    field: 'bDisplay', title: '是否显示', align: 'center', width: 200, formatter: function (value) {
                        if (value == 0) return '不显示';
                        if (value == 1) return '显示';
                    }
                },
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
                if ($("#home_picture_form").form("validate")) {
                    var param/*查询参数*/ = $("#home_picture_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/HomePicture/GetList", { PageIndex: pageNumber, PageSize: pageSize }, function (r) {
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
        function initEvent() {
            $("#home_picture_form")
            .on("click", "a[data-id='add_picture_home_button']", addpicture)
            .on("click", "a[data-id='delete_picture_home_button']", deletepicture)
            .on("click", "a[data-id='edit_picture_home_button']", editepicture)
            .on("click", "a[data-id='display_picture_home_button']", display)
        }
        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        function addpicture()
        {
            var div = $("<div/>");
            div.dialog({
                title: "添加",
                width: 850,
                height: 600,
                cache: false,
                href: '/Admin/HomePicture/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_homepicture_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDCardImage.imageList.length <= 0) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDCardImage.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDCardImage.imageList[i].filePath);
                                }
                                $("#home_add_picture_banner").val(temps.join());
                            }
                            if (travelObj.editor.getContent() == "") {
                                return eui.alertInfo("请输入内容");
                            } else {
                                 debugger
                                $("#homepicture_sContent").val(travelObj.editor.getContent());
                            }

                            var json = $("#add_homepicture_form").serializeObject();
                            f.post("/Admin/HomePicture/AddPicture", json,
                                function (ret) {
                                    eui.alertInfo("添加成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (ret) {
                                    eui.alertErr(ret.Msg);
                                });
                        }
                    }
                },
                {
                    text: '预览',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_homepicture_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDCardImage.imageList.length <= 0) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDCardImage.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDCardImage.imageList[i].filePath);
                                }
                                $("#home_add_picture_banner").val(temps.join());
                            }
                            if (travelObj.editor.getContent() == "") {
                                return eui.alertInfo("请输入内容");
                            } else {
                                $("#homepicture_sContent").val(travelObj.editor.getContent());
                            }

                            var json = $("#add_homepicture_form").serializeObject();
                            f.post("/Admin/HomePicture/AddPicture", json, function () { }, function () { });
                        }

                        look();
                    }
                },
                {
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
                    travelObj.UploadIDCardImage = new UploadImage({
                        target: "home_add_picture_banner_upload", maxFileCount: 1
                    });

                    travelObj.editor = ue.initUE('homepicture_add_sContent_script');
                }
            });
        }
        //获取预览的ID
        function look() {
            
            f.post("/Admin/HomePicture/lookImage", null, function (res) {
                PreviewGoods(res.Data.ID);
            }, function () { });
        }
        //预览
        function PreviewGoods(ID) {
            var div = $("<div/>");
            div.dialog({
                title: "图文预览",
                width: 300,
                height: 300,
                href: '/Admin/HomePicture/PicturePreview',
                modal: true,
                buttons: [
                    {//关闭
                        text: '关闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $(div).dialog("close");
                        }
                    },
                ],
                onClose: function () {
                    f.post("/Admin/HomePicture/deleteHomePicture", { ID: ID },
                    function (ret) {
                        $(div).dialog("destroy");
                        div = null;
                    }, function (ret) {
                        eui.alertErr(ret.Msg);
                    });
                },
                onLoad: function () {
                    $("#home_picture_code").qrcode({
                        //render: "html",
                        width: 160, //宽度 
                        height: 160, //高度 
                        text: '  http://' + window.location.host + '/Client/ClientHome/GetHomeShowBannerDetailView?ID=' + ID //任意内容 
                    });
                }
            });
        }

        //删除
        function deletepicture()
        {
            eui.confirmDomainByMultiRows(grid, deletess, "", "请至少选择一行", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个图文".format(rows.length);
            });
        }
        //删除回调
        function deletess(row) {
            debugger
            var rowIDs = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < row.length; i++) {
                rowIDs.push(row[i].ID);
            }
            f.post("/Admin/HomePicture/DeletePicture", { ID: rowIDs.toString() }, function () {
                eui.alertInfo("删除成功");
                reloadData();
                grid.datagrid("clearSelections");
            }, function () {
                eui.alertInfo("删除失败");
            })
        }

        //只能选中一行
        function CheckIsSelectOne() {
           
            var rows = grid.datagrid("getSelections");
            if (rows.length > 1) {
                return eui.alertInfo("只能选中一行");
            }
            return true;
        }
        function editepicture() {
            
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, function detailA(row) {
                    mailDetail(row);
                },
                  "请选择一行");
            }
        }

        //编辑
        function mailDetail(row)
        {
            var div = $("<div/>");
            div.dialog({
                title: "编辑",
                width: 850,
                height: 600,
                cache: false,
                href: '/Admin/HomePicture/Edit?ID='+row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                       
                        if ($("#edit_homepicture_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDPicture.imageList.length <= 0) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDPicture.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDPicture.imageList[i].filePath);
                                }
                                $("#home_edit_picture_banner").val(temps.join());
                            }
                            if (travelObj.editorEE.getContent() == "") {
                                return eui.alertInfo("请输入内容");
                            } else {
                                $("#homepicture_edit_sContent").val(travelObj.editorEE.getContent());
                            }

                            var json = $("#edit_homepicture_form").serializeObject();
                            f.post("/Admin/HomePicture/EditPicture", json,
                                function (ret) {
                                    eui.alertInfo("编辑成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (ret) {
                                    eui.alertErr(ret.Msg);
                                });
                        }
                    }
                },
                {
                    text: '预览',
                    iconCls: 'icon-save',
                    handler: function () {
                        
                        if ($("#edit_homepicture_form").form("validate")) {
                            //序列化提交数据
                            if (travelObj.UploadIDPicture.imageList.length <= 0) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                var temps = [];
                                for (var i = 0; i < travelObj.UploadIDPicture.imageList.length; i++) {
                                    temps.push(travelObj.UploadIDPicture.imageList[i].filePath);
                                }
                                $("#home_edit_picture_banner").val(temps.join());
                            }
                            if (travelObj.editorEE.getContent() == "") {
                                return eui.alertInfo("请输入内容");
                            } else {
                                $("#homepicture_edit_sContent").val(travelObj.editorEE.getContent());
                            }

                            var json = $("#edit_homepicture_form").serializeObject();
                            f.post("/Admin/HomePicture/AddPicture", json, function () { }, function () { });
                        }

                        look();
                    }
                },
                {
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
                    travelObj.UploadIDPicture = new UploadImage({
                        target: "home_edit_picture_banner_upload", maxFileCount: 1, imgLst: [
                            {
                                filePath: $("#home_edit_picture_banner").val(),
                            }
                        ]
                    });

                    travelObj.editorEE = ue.initUE('homepicture_edit_sContent_script', $("#homepicture_edit_sContent").val());
                }
            });
        }

        function display()
        {
            eui.confirmDomainByMultiRows(grid, function delay(row) {
                var rowIDs = [];
                var bDisplay = [];
                debugger
                //遍历所有的行创建ID集合
                for (var i = 0; i < row.length; i++) {
                    rowIDs.push(row[i].ID);
                    bDisplay.push(row[i].bDisplay);
                }
                f.post("/Admin/HomePicture/Display", { ID: rowIDs.toString(), bDisplay: bDisplay.toString() }, function () {
                    eui.alertInfo("显示/隐藏成功");
                    reloadData();
                    grid.datagrid("clearSelections");
                }, function () {
                    eui.alertInfo("显示/隐藏失败");
                })


            }, "", "请至少选择一行", function formatconfirm(rows) {
                return "您确定要显示/隐藏这【{0}】个图文".format(rows.length);
            });
        }
       
        try {
            init();
            initEvent();
        } catch (e) {

        }

        function destroy() {
            travelObj.UploadIDPicture.destroy();
            travelObj.UploadIDCardImage.destroy();
            travelObj.editor.destroy();
        }

       
        return {
            destroy: destroy
        };
    });
});