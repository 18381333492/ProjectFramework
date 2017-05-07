$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("游记管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var gal = modules.get(enums.Modules.CACHE);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);

        var pageNumber = 1, pageSize = 10;
        var grid = $("#travelNote_index_datagrid");
        var travelObj = {};

        /**
          * 初始化数据
          */
        function initDataGrid() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#travelNote_index_tool_div",
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
                { field: 'sStoreID', title: '店铺ID', align: 'center', width: 100, hidden: true },
                { field: 'IsStore', title: '是否是店铺登录', align: 'center', width: 100, hidden: true },
                { field: 'sTitle', title: '游记标题', align: 'center', width: 100 },
                { field: 'sAuthor', title: '作者', align: 'center', width: 100 },
                { field: 'dPublishTime', title: '发布时间', align: 'center', width: 100 },
                { field: 'iOrder', title: '排序编号（数字越小越靠前）', align: 'center', width: 100 },
                { field: 'sStoreName', title: '店铺名称', align: 'center', width: 100 }
                ]],
                onLoadSuccess: function (data) {
                    if (data.total > 0) {
                        if (data.rows[0].IsStore === 'true') {
                            //店铺用户登录
                            grid.datagrid('hideColumn', 'sStoreName'); // 设置隐藏列    
                        } else {
                            //平台用户登录
                            grid.datagrid('hideColumn', 'iOrder'); // 设置隐藏列  
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
            debugger
            try {
                if ($("#travelNote_index_form").form("validate")) {
                    var param/*查询参数*/ = $("#travelNote_index_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    f.post("/Admin/TravelNote/LoadData", param, function (r) {
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
         * 查询【事件】
         */
        function searchEvent() {
            searchCallBack(pageNumber, pageSize);
        }

        /**
         * 显示添加窗口
         */
        function showAddView() {
            var div = $("<div/>");
            div.dialog({
                title: "添加",
                width: 850,
                height: 600,
                cache: false,
                href: '/Admin/TravelNote/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        //添加数据
                        if ($("#travelNote_add_form").form("validate")) {

                            //设置头像路径
                            if (travelObj.uploadHeadImage.imageList.length <= 0) {
                                return eui.alertInfo("请上传头像");
                            } else {
                                $("#travelNote_add_sHeadImages").val(travelObj.uploadHeadImage.imageList[0].filePath);
                            }

                            //设置游记图片
                            if (travelObj.uploadTravelImage.imageList.length <= 0) {
                                return eui.alertInfo("请至少上传一张游记图片");
                            } else {
                                var temps = [];
                                for (var i = 0; i < travelObj.uploadTravelImage.imageList.length; i++) {
                                    temps.push(travelObj.uploadTravelImage.imageList[i].filePath);
                                }
                                $("#travelNote_add_sTravelImges").val(temps.join());
                            }
                            //设置游记内容
                            var content=ue.getTextContent(travelObj.editor);
                            var htmlcontent=ue.getHtmlContent(travelObj.editor);
                           
                            if (travelObj.editor.getContent().length == "") {
                                return eui.alertInfo("请输入游记内容");
                            } else if (travelObj.editor.getContent().length > 3000) {
                                return eui.alertInfo("游记内容不能超过3000字");
                            }else{
                                $("#travelNote_add_sContent").val(travelObj.editor.getContent());
                            }
                                
                            var json = $("#travelNote_add_form").serializeObject();
                            f.post("/Admin/TravelNote/DoAdd", json, function (res) {
                                eui.alertInfo("添加成功");
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

                    //初始化图片上传（上传头像）
                    travelObj.uploadHeadImage = new UploadImage({
                        target: "travelNote_add_sHeadImages_upload", maxFileCount: 1
                    });

                    //初始化图片上传（上传游记图片）
                    travelObj.uploadTravelImage = new UploadImage({
                        target: "travelNote_add_sTravelImges_upload", maxFileCount: 6
                    });

                    //初始化编辑器
                    travelObj.editor = ue.initUE('travelNote_add_sContent_script');
                }
            });
        }

        /**
         * 添加事件
         */
        function addEvent() {
            showAddView();
        }

        /**
        * 编辑事件
        */
        function editEvent() {
            if (CheckIsSelectOne() == true) {
                var rows = grid.datagrid("getSelections");
                if (rows.length == 0) { return eui.alertInfo("请选择一行进行操作!"); }
                var div = $("<div/>");
                div.dialog({
                    title: "编辑",
                    width: 850,
                    height: 600,
                    cache: false,
                    href: '/Admin/TravelNote/Edit?ID='+rows[0].ID,
                    modal: true,
                    collapsible: false,
                    minimizable: false,
                    maximizable: false,
                    resizable: false,
                    buttons: [{
                        text: '保存',
                        iconCls: 'icon-save',
                        handler: function () {
                            //添加数据
                            if ($("#travelNote_edit_form").form("validate")) {
                                //设置头像路径
                                if (travelObj.uploadHeadImage.imageList.length <= 0) {
                                    return eui.alertInfo("请上传头像");
                                } else {
                                    $("#travelNote_add_sHeadImages").val(travelObj.uploadHeadImage.imageList[0].filePath);
                                }
                                //设置游记图片
                                if (travelObj.uploadTravelImage.imageList.length <= 0) {
                                    return eui.alertInfo("请至少上传一张游记图片");
                                } else {
                                    var temps = [];
                                    for (var i = 0; i < travelObj.uploadTravelImage.imageList.length; i++) {
                                        temps.push(travelObj.uploadTravelImage.imageList[i].filePath);
                                    }
                                    $("#travelNote_add_sTravelImges").val(temps.join());
                                }
                                //设置游记内容
                                var content = ue.getTextContent(travelObj.editor);
                                var htmlcontent = ue.getHtmlContent(travelObj.editor);

                                if (travelObj.editor.getContent().length == "") {
                                    return eui.alertInfo("请输入游记内容");
                                } else if (travelObj.editor.getContent().length > 3000) {
                                    return eui.alertInfo("游记内容不能超过3000字");
                                } else {
                                    $("#travelNote_add_sContent").val(travelObj.editor.getContent());
                                }
                                var json = $("#travelNote_edit_form").serializeObject();
                                f.post("/Admin/TravelNote/DoEdit", json, function (res) {
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
                        var imgLst = [];
                        if ($('#sTravelImges').val() != "") {
                            var picture = $('#sTravelImges').val().split(',');//游记图片
                            $(picture).each(function () {
                                imgLst.push({ filePath: this });
                            });
                        }
                        //初始化图片上传（游记图片）
                        travelObj.uploadTravelImage = new UploadImage({
                            target: "travelNote_add_sTravelImges_upload", maxFileCount: 6, imgLst: imgLst, removeExistsImageHandle: function (file) {
                                var order;
                              //  var src = goods.uploadImage.imageList[0].filePath;
                                for (var i = 0; i < travelObj.uploadTravelImage.imageList.length; i++) {
                                    if (travelObj.uploadTravelImage.imageList[i].filePath == file) {
                                        order = i;
                                        travelObj.uploadTravelImage.imageList.splice(order, 1)
                                        break;
                                    }
                                }
                            }
                        });

                        //初始化图片上传（上传头像）
                        travelObj.uploadHeadImage = new UploadImage({
                            target: "travelNote_add_sHeadImages_upload", maxFileCount: 1, imgLst: [{ filePath: $('#sHeadImges').val() }]
                        });
                        //初始化编辑器
                        travelObj.editor = ue.initUE('travelNote_add_sContent_script', $('#sContent').val());
                    }
                });
            }
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
            f.post("/Admin/TravelNote/DoDelete", { rowIDs: rowIDs.toString() }, function (res) {
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
        * 编辑回调
        * @param {type} selectRow 选中行
        */
        function editOrderCallBack(selectRow) {
            showEditOrderView(selectRow);
        }

        /**
         * 编辑操作
         */
        function editOrderEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, editOrderCallBack, "请至少选中一行");
            }
        }

        /**
        * 显示编辑排序窗口
        */
        function showEditOrderView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑排序",
                width: 450,
                height: 400,
                cache: false,
                href: '/Admin/TravelNote/EditOrder?ID=' + row.ID,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [
                    {
                        text: '保存',
                        iconCls: 'icon-save',
                        handler: function () {
                            if ($("#travelNote_edit_order_form").form("validate")) {
                                var iOrder = $("#travelNote_edit_order_iOrder").textbox("getText");
                                var ID = $("#travelNote_edit_order_ID").val();
                                f.post("/Admin/TravelNote/DoEditOrder", { ID: ID, iOrder: iOrder }, function (res) {
                                    eui.alertInfo("编辑成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (res) {
                                    eui.alertInfo(res.Msg);
                                });
                            }
                        }
                    }
                    , {
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
        function showDetailView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑",
                width: 500,
                height: 450,
                cache: false,
                href: '/Admin/TravelNote/Detail?ID=' + row.ID,
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
        * 编辑回调
        * @param {type} selectRow 选中行
        */
        function detailCallBack(selectRow) {
            showDetailView(selectRow);
        }

        /**
         * 编辑操作
         */
        function detailsEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, detailCallBack, "请至少选中一行");
            }
        }

        /**
        * 编辑回调
        * @param {type} selectRow 选中行
        */
        function settingCallBack(selectRow) {
            showSettingView(selectRow);
        }

        /**
         * 编辑操作
         */
        function settingEvent() {
            if (CheckIsSelectOne()) {
                eui.checkSelectedRow(grid, settingCallBack, "请至少选中一行");
            }
        }

        /**
         * 添加店铺事件
         */
        function DoAddShopEvent() {
            //添加选择的店铺<input name="Fruit" type="checkbox" value="" />苹果
            var value = $(this).parents("tr").find("td:first").children("div").text();
            var name = $(this).parents("tr").find("td:first").next().children("div").text();
            var html = '&emsp;&emsp;<input name="CheckedShop" checked="checked" type="checkbox" value="{0}" /><span>{1}</span>'.format(value, name);
            $("#travelNote_setting_shops_div").append(html);
            //按钮禁用
            $(this).removeAttr("name");
            $(this).attr("style", "color: #afafaf");
        }

        var flag = true;//标志是否是第一次  避免多次绑定 按钮事件

        /**
         * 初始化【店铺】Combogrid
         */
        function initShopCombogrid(row) {
            flag = true;
            $('#travelNote_setting_shops_input').combogrid({
                delay: 100,
                mode: 'remote',
                idField: 'sShopID',
                textField: 'sShopName',
                columns: [[
                    { field: 'sShopID', title: '主键', hidden: true },
                    { field: 'sShopName', title: '店铺名称', width: 200 },
                    {
                        field: 'btnAdd', width: 50, title: '添加', formatter: function (value, row, index) {
                            return '<a href="#" name="travelNote_setting_addShop_btn">添加</a>';
                        }
                    }
                ]],
                keyHandler: {
                    query: function (keyword) {
                        GetShopCombogridData(keyword, row.sStoreID);
                    }
                }
            });

            GetShopCombogridData("", row.sStoreID);
        }

        /**
         * 加载Combogrid数据
         */
        function GetShopCombogridData(keyword, sStoreID) {
            f.post("/Admin/TravelNote/GetShops", { keyword: keyword, sStoreID: sStoreID }, function (r) {

                $('#travelNote_setting_shops_input').combogrid("grid").datagrid({
                    onLoadSuccess: function (data) {
                        if (flag) {
                            $('#travelNote_setting_shops_input').combogrid("grid").parent().on("click", "a[name='travelNote_setting_addShop_btn']", DoAddShopEvent);
                            flag = false;
                        }
                    }
                });
                $('#travelNote_setting_shops_input').combogrid("grid").datagrid("loadData", r.Data);
            }, function (r) {
                eui.alertInfo(r.Msg);
            });
        }

        /**
        * 显示【将游记布置到】窗口
        */
        function showSettingView(row) {
            var div = $("<div/>");
            div.dialog({
                title: "将游记布置到",
                width: 650,
                height: 500,
                cache: false,
                href: '/Admin/TravelNote/Setting',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [
                    {
                        text: '保存',
                        iconCls: 'icon-save',
                        handler: function () {
                            //获取用户选中店铺的信息
                            var shops = $("#travelNote_setting_shops_div").find("input[name='CheckedShop']:checked");
                            if(shops.length<=0){
                                return eui.alertInfo("至少选择一个店铺");
                            }
                            var json = [];//上传数据
                            for (var index = 0; index < shops.length; index++) {
                                var sShopID = $(shops[index]).val();
                                var value=$(shops[index]).next().text();
                                json.push({ sShopID: sShopID, sShopName: value, TravelID: row.ID });
                            }
                            f.post("/Admin/TravelNote/DoSetting", json, function (res) {
                                eui.alertInfo("操作成功");
                                $(div).dialog("close");
                                eui.search(grid, false);
                            }, function (res) {
                                eui.alertInfo(res.Msg);
                            });
                        }
                    }
                    , {
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
                    initShopCombogrid(row);
                }
            });
        }

        /**
         * 绑定事件
         */
        function bindEvent() {
            $("#travelNote_index_form")
            .on("click", "#travelNote_index_btn_seach", searchEvent)
            .on("click", "a[data-id='TravelNote_Add']", addEvent)
            .on("click", "a[data-id='TravelNote_Delete']", deleteEvent)
            .on("click", "a[data-id='TravelNote_Setting']", settingEvent)
            .on("click", "a[data-id='TravelNote_Detail']", detailsEvent)
            .on("click", "a[data-id='TravelNote_EditOrder']", editOrderEvent)
            .on("click", "a[data-id='TravelNote_Edit']", editEvent)//编辑
        }

        try {
            initDataGrid();
            bindEvent();
        } catch (e) {

        }

        function destroy() {
            travelObj.uploadTravelImage.destroy();
            travelObj.uploadHeadImage.destroy();
            travelObj.editor.destroy();
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