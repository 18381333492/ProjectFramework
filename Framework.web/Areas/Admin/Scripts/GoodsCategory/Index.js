$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("商品分类管理", new function () {

        var eui = modules.get("eui");
        var f = modules.get("func");

        /**
         * 初始化事件
         */
        function initEvent() {
            $("#goods_category_form")
                .on("click", "a[data-id='add_goods_category']", function () {
                    eui.checkSelectedRow($("#goods_category_tree"), addGoodsCategory, "请选择所属分类");
                })
                .on("click", "a[data-id='edit_goods_category']", function () {
                    eui.checkSelectedRow($("#goods_category_tree"), editGoodsCategory, "请选择要编辑的分类");
                })
                .on("click", "a[data-id='del_goods_category']", function () {
                    eui.confirmDomain($("#goods_category_tree"), deleteGoodsCategory, null, "请选中你要删除的分类", function (r) {
                        return "你是否确认要删除{0}这个商品分类，这样做将造成<span style='color:red;'>被删除的分类及其子分类都将被删除，其对应的商品也会被删除，请谨慎操作</span>".format(r.text);
                    });
                });
        }

        /**
         * 添加商品分类
         */
        function addGoodsCategory(selectedRow) {
            var div = $("<div/>");
            div.dialog({
                title: "添加商品分类",
                width: 500,
                height: 400,
                cache: false,
                href: '/Admin/GoodsCategory/ToAddGoodsCategory',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        if ($("#add_goods_category_form").form("validate")) {

                            //序列化提交数据
                            var json = $("#add_goods_category_form").serializeObject();
                            json.PID = selectedRow.id;

                            f.post("/Admin/GoodsCategory/AddGoodsCategory", json,
                                function (ret) {
                                    eui.alertInfo("添加成功");
                                    $(div).dialog("close");
                                    $("#goods_category_tree").treegrid("append", {
                                        parent: selectedRow.id,
                                        data: [ret.Data]
                                    });
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
                    $(div).dialog("destroy"); div = null;
                    /* 如果套用了iframe就回收内存 */
                    var frame = $('iframe', this);
                    if (frame.length > 0) {
                        frame[0].contentWindow.document.write('');
                        frame[0].contentWindow.close();
                        frame.remove();
                    }
                },
                onLoad: function () {
                }
            });
        }

        /**
         * 编辑商品分类
         */
        function editGoodsCategory(selectedRow) {
            var div = $("<div/>");
            div.dialog({
                title: "编辑商品分类",
                width: 500,
                height: 400,
                cache: false,
                href: '/Admin/GoodsCategory/ToEditGoodsCategory?ID=' + selectedRow.id,
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        //if ($("#add_goods_category_form").form("validate")) {

                        //    //序列化提交数据
                        //    var json = $("#add_goods_category_form").serializeObject();

                        //    f.post("/Admin/GoodsCategory/AddGoodsCategory", json,
                        //        function (ret) {
                        //            eui.alertInfo("添加成功");
                        //            $(div).dialog("close");
                        //            $("#goods_category_tree").treegrid("append", {
                        //                parent: selectedRow.id,
                        //                data: [ret.Data]
                        //            });
                        //        }, function (ret) {
                        //            eui.alertErr(ret.Msg);
                        //        });
                        //}
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
                    /* 如果套用了iframe就回收内存 */
                    var frame = $('iframe', this);
                    if (frame.length > 0) {
                        frame[0].contentWindow.document.write('');
                        frame[0].contentWindow.close();
                        frame.remove();
                    }
                },
                onLoad: function () {
                    var form = $("#nodata");
                    if (form.length > 0) {
                        div.parent().find("a>span")[0].remove();
                    }
                    form = null;
                }
            });
        }

        /**
         * 删除商品分类
         */
        function deleteGoodsCategory(selectedRow) {
            f.post("/Admin/GoodsCategory/DeleteGoodsCategory",
                { ID: selectedRow.id },
                function (r) {
                    eui.alertInfo("您已经删除了商品分类{0}".format(selectedRow.text));
                    $("#goods_category_tree").treegrid("remove", selectedRow.id);
                }, function (r) {
                    eui.alertErr(r.Msg);
                });
        }

        /**
         * 初始化tree数据
         */
        function initData() {
            $("#goods_category_tree").treegrid({
                idField: "id",
                fit: true,
                treeField: "text",
                animate: true,
                border: false,
                method: 'post',
                fitColumns: true,
                rownumbers: true,
                columns: [[
                    { field: 'sPID', hidden: true },
                    { title: '商品分类名称', field: 'text', width: 100 },
                    { title: '商品分类简述', field: 'sCategoryCaption', align: 'center', width: 100 },
                    {
                        title: '分类图标', field: 'sImgUri', align: 'center', width: 100, formatter: function (value, row, index) {
                            var ret = "<img src='" + (value === "" ? "../Content/img/noimg.png" : value) + "' style='height:40px;width:100px;'></img>";
                            return ret;
                        }
                    },
                    { title: '添加时间', field: 'addDate', align: 'center', width: 100 },
                    { title: '排序号', field: 'iOrder', align: 'center', width: 100 }
                ]]
            });
            try {
                f.post("/Admin/GoodsCategory/LoadGoodsCategory", null, function (r) {
                    $("#goods_category_tree").treegrid("loadData", r.Data);
                }, function (r) {
                    eui.alertErr(r.Msg);
                }, true, true);
            } catch (e) {
                eui.alertErr(e.message);
            }
        }

        try {
            initData();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }
    });
});