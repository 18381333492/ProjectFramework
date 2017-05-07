$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("短信分配", new function () {
        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var grid = $("#shopmessage_menu");
       

        function init() {
            
            $("#shengyu_message").text($("#sql_sy_message").val());
            showModifyDialog();
            terraceDialog();
            eui.bindPaginationEvent(grid, {
                idField: "ID",
                loadMsg: "正在加载...",
                toolbar: "#shopmessage_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
                 { field: 'sShopName', title: '店铺名称', align: 'center', width: 200 },
                 { field: 'tUserState', title: '店铺状态', align: 'center', width: 200, hidden: true },
                 { field: 'sProvice', title: '省', align: 'center', width: 100,hidden:true},
                 { field: 'sCity', title: '市', align: 'center', width: 100, hidden: true },
                 { field: 'sCounty', title: '县', align: 'center', width: 100, hidden: true },
                 { field: 'sAddress', title: '具体地址', align: 'center', width: 100, hidden: true },
                 {
                     field: 'sDZ', title: '地址', align: 'center', width: 200, formatter: function (value, row, index) {
                        
                         return row.sProvice + row.sCity + row.sCounty + row.sAddress;
                     }
                 },
                 { field: 'dCreateTime', title: '成立时间', align: 'center', width: 200 },
                 { field: 'sMessage', title: '剩余短信', align: 'center', width: 200 },
                 
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
        //重新加载数据
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        function showModifyDialog() {
            $("#edit_message_dialog_container").dialog({
                title: '设置短信条数',
                width: 220,
                height: 150,
                buttons: [
                    {
                        text: '修改',
                        iconCls: 'icon-save',
                        handler: function () {
                            
                            if (!$("#edit_message_dialog_form").form('validate')) { return false; }
                            var shengyuMessage = "";
                            f.post("/Admin/ShopMessage/GetMessageCount", null, function (res) {
                                
                                shengyuMessage = res.Data.sMessage;
                                var ID = $("#edit_message_id").val();
                                var eMessage = $("#edit_message_count").textbox('getText');//设置后的短信数量
                                var exMessage = $("#jilu_message_count").val();//设置前的短信数量
                                var difference=parseInt(eMessage) - parseInt(exMessage);//增加了多少的短信
                                if (parseInt(shengyuMessage) < difference) {//计算短信数量是否足够
                                    eui.alertInfo("短信数量不够，请重新分配");
                                }
                                else {
                                    f.post("/Admin/ShopMessage/UpdateTerraceCount", { ID: ID, sMessage: eMessage, difference: difference }, function () {
                                       
                                        $("#shengyu_message").text(parseInt($("#shengyu_message").text()) - difference);
                                        $("#edit_message_dialog_container").dialog('close');
                                        eui.alertInfo("修改成功");
                                        grid.datagrid("getPager").pagination("select");
                                        
                                    })
                                }
                            }, function (res) {
                                })
                        }
                    },
                    {
                        text: '关闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $("#edit_message_dialog_container").dialog('close');
                        }
                    }],
                onClose: function () {
                },
                onLoad: function () {
                   
                }
            }).dialog("close");

            $("#edit_message_dialog_form [name='sMessage']").numberbox({
                required: true,
                width: 160,
                validType: ['isNumber', 'maxInt[99999]', 'minInt[0]'],
                missingMessage: "请输入短信条数"
            });
        }

        //查询数据的回调方法
        function loadDate(pageNumber, pageSize) {
            var sKeyword = $("#shop_message_name_search").val();
            f.post("/Admin/ShopMessage/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
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

        /*
        *按钮事件
        */
        function initEvent() {            
            $("#shopmessage_form")
                .on("click", "a[data-id='edit_message_button']", editCount)
                .on("click", "a[data-id='update_terrace_message']", editTerraceCount)
                .on("click", "a[data-id='shop_message_search_button']", searchShop)
        }
        function searchShop()
        {
            grid.datagrid("getPager").pagination("select");
        }
        /*
        * 修改短信条数点击事件
        */
        function editCount() {
            
            eui.checkSelectedRow(grid, editshort, "请选中您要操作的店铺");
            
        }

        /*
        * 打开编辑短信条数窗口
        */
        function editshort(row) {
            if (row.tUserState === 1) {
                eui.alertInfo("冻结用户不允许修改短信票数");
            }
            else{
                $("#edit_message_dialog_container").dialog("setTitle", "设置短信条数").dialog("open");
               f.post('/Admin/ShopMessage/GetCount',{ ID: row.ID }, function (res) {
                $("#edit_message_id").val(res.Data.ID) ;
                $("#edit_message_count").textbox("setText", res.Data.sMessage);
                $("#jilu_message_count").val(res.Data.sMessage);
            }, function () { })
        }
        }
        //修改平台短信窗口
        function terraceDialog() {
            $("#edit_TerraceCount_dialog_container").dialog({
                title: '设置平台短信条数',
                width: 220,
                height: 150,
                buttons: [
                    {
                        text: '修改',
                        iconCls: 'icon-save',
                        handler: function () {
                            var json = $("#edit_TerraceCount_dialog_container").serializeObject();
                            var eMessage = $("#edit_float_message_count").textbox('getText');
                            var exMessage = $("#ex_float_message_count").val();
                            if (parseInt(eMessage) < parseInt(exMessage)) {
                                eui.alertInfo("店铺短信不能减少");
                             }
                            else {
                                json.sMessage = eMessage;
                                f.post("/Admin/ShopMessage/UpdateMessageCount", json, function () {
                                    debugger
                                    $("#shengyu_message").text(eMessage);
                                    $("#edit_TerraceCount_dialog_container").dialog('close');
                                        eui.alertInfo("修改成功");
                                        reloadData();
                                      
                                    })
                                }
                            }

                        
                    },
                    {
                        text: '关闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            $("#edit_TerraceCount_dialog_container").dialog('close');
                        }
                    }],
                onClose: function () {
                },
                onLoad: function () {


                }
            }).dialog("close");

            $("#edit_TerraceCount_dialog_form [name='sMessage']").numberbox({
                required: true,
                width: 160,
                validType: ['isNumber', 'maxInt[99999]', 'minInt[0]'],
                missingMessage: "请输入短信条数"
            });
        }
        //修改平台的短信数
        function editTerraceCount()
        {
            $("#edit_TerraceCount_dialog_container").dialog("setTitle", "设置平台短信条数").dialog("open");

            f.post('/Admin/ShopMessage/GetMessageCount', null, function (res) {
                
                $("#ex_float_message_count").val(res.Data.sMessage);
                $("#edit_float_message_count").textbox("setText", res.Data.sMessage);
            }, function (res) { })

            
        }

        try {
            init();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }
    });
});