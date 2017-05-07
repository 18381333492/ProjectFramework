$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("客房类型管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var grid = $("#guestroom_type_grid");

        /**
         * 添加客房类型
         */
        function addGuestRoomType() {
            var div = $("<div/>");
            div.dialog({
                title: "添加客房类型",
                width: 300,
                height: 127,
                cache: false,
                href: '/Admin/GuestRoom/ToAddGuestRoomType',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_guestroom_type_form").form("validate")) {

                            //序列化提交数据
                            var json = $("#add_guestroom_type_form").serializeObject();

                            f.post("/Admin/GuestRoom/AddGuestRoomType", json,
                                function (ret) {
                                    eui.alertInfo("添加成功");
                                    $(div).dialog("close");
                                    eui.search(grid, false);
                                }, function (ret) {
                                    eui.alertErr(ret.Msg);
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
         * 初始化事件
         */
        function initEvent() {
            $("#guestroom_type_grid_tool")
                .on("click", "a[data-id='add_guestroom_type']", addGuestRoomType);
        }

        /**
         * 载入客房类型
         * @param {Number} pageNumber 页码
         * @param {Number} pageSize 每页显示条数
         */
        function loadGuestRoomTypes(pageNumber, pageSize) {
            try {
                var param = {};
                param.PageIndex/*当前页码*/ = pageNumber;
                param.pageSize/*每页显示条数*/ = pageSize;
                f.post("/Admin/GuestRoom/LoadGuestRoomTypes", param, function (r) {
                    if (r.Data !== null) {
                        grid.datagrid("loadData", r.Data.Result);
                        grid.datagrid("getPager").pagination({
                            pageNumber: pageNumber,
                            pageSize: pageSize,
                            total: r.Data.MaxCount
                        });
                    }
                }, function (r) {
                    eui.alertErr(r.Msg);
                });
            } catch (e) {
                eui.alertErr(e.message);
            }
        }
                
        /**
         * 初始化grid数据
         */
        function initData() {
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#guestroom_type_grid_tool",
                fit: true,
                fitColumns: false,
                pagination: true,
                rownumbers: true,
                singleSelect: true,
                columns: [[
                { field: 'checkbox', checkbox: true },
                { field: 'sRoomTypeName', title: '客房类型', align: 'center', width: 250 },
                {
                    field: 'operate', title: '操作', align: 'center', width: 100, formatter: function (index, row) {

                        return "<a style='cursor:pointer;' onclick='modules.get(enums.Modules.CACHE).getMenuDomain(\"客房类型管理\").deleteRoomType(\"" + row.ID + "\");'>删除</a>";
                    }
                }
                ]],
                onLoadSuccess: function () {
                     $(".datagrid-header-check input[type=checkbox]").remove();
                }, rowStyler: function (index, row) {
                }
            }, loadGuestRoomTypes).pagination("select");
        }

        /*初始化页面数据*/
        try {
            initData();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }

        /**
         * 删除客房类型
         * @param {type} id 客房类型的ID
         */
        function deleteRoomType(id) {
            try {

                eui.confirm(function () {
                    f.post("/Admin/GuestRoom/DeleteGuestRoomTypes", { ID: id }, function (r) {
                        eui.alertInfo("该客房类型已删除成功");
                        eui.search(grid, false);
                    }, function (r) {
                        eui.alertErr("该房型下面绑定有商品不能删除!");
                    });
                }, "您确认要删除当前房间类型吗？请您确认操作！");

            } catch (e) {
                eui.alertErr(e.Msg);
            }
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
            destroy: destroy,
            deleteRoomType: deleteRoomType
        };
    });
});