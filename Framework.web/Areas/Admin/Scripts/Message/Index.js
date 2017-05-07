$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("已发送记录", new function () { 
    var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
    var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
    var grid = $("#message_manager_menu");

    //初始化datagid
    function init() {
        $("#send_message_sy_system").text($("#send_message_system").val());
        eui.bindPaginationEvent(grid, {
            loadMsg: "正在加载...",
            toolbar: "#message_manager_tool",
            pagination: true,
            rownumbers: true,
            fit: true,
            columns: [[
            { field: 'checkbox', checkbox: true },
             { field: 'ID', title: 'ID', align: 'center', width: 260, hidden: true },
             { field: 'sPeople', title: '收件人', align: 'center', width: 200 },
             { field: 'dSendTime', title: '发送时间', align: 'center', width: 300 },
             {
                 field: 'sContent', title: '短信内容', align: 'center', width: 400, formatter: function (value) {
                     var a = value.length >= 20 ? value.substr(0, 20) + "..." : value;
                     return "<span style='color:red'>" + a + " </span>";
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
    function loadDate(pageNumber, pageSize, sKeyword) {
        try {
            if ($("#message_manage_form").form("validate")) {
                var param/*查询参数*/ = $("#message_manage_form").serializeObject();
                param.PageIndex/*当前页码*/ = pageNumber;
                param.pageSize/*每页显示条数*/ = pageSize;
                var keyword = $("#search_message_Name").val();
                f.post("/Admin/Message/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: keyword }, function (r) {

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

        
        //var keyword = $("#messageName").val();
        //f.post("/Admin/Message/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: keyword}, function (r) {
        //    grid.datagrid("loadData", r.Data.Result);
        //    grid.datagrid("getPager").pagination({
        //        pageNumber: pageNumber,
        //        pageSize: pageSize,
        //        total: r.Data.MaxCount
        //    });
        //}, function (r) {
        //    eui.alertErr(r.Msg);
        //});
    }
    
        //按钮事件
    function initEvent() {
        $("#message_manage_form")
            .on("click", "a[data-id='send_message_button']", sendmessage)
            .on("click", "a[data-id='serach_meaasge_button']", searchMessage)
            .on("click", "a[data-id='detail_message_button']", detailMessage)
            .on("click", "a[data-id='send_share_message_button']", shareMessage)
    }
        //重新加载数据
    function reloadData() {
        grid.datagrid("getPager").pagination("select");
    }
    var overplus = $("#message_form_overplus").val();
        //发送窗口
    var div = null
    function showDialog() {
        var name = "全部会员";
        div = $("<div/>");
        div.dialog({
            title: '发送短信',
            width: 900,
            height: 450,
            cache: false,
            href: '/Admin/Message/Add',
            modal: true,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            buttons: [{
                text: '发送',
                iconCls: 'icon-save',
                handler: function () {
                    var look="";
                    if ( name == "全部会员") {
                        f.post('/Admin/Message/AllCount', null, function (res) {
                            
                            look = res.Data;//所有会员数量
                            if (parseInt(look) > parseInt(overplus)) {//如果剩余短信数量小于要发送短信的数量无法发送
                                eui.alertInfo("剩余短信条数不够，无法发送");
                                $(div).dialog("close");
                            }
                            else {
                                var sContent = $("#sed_sMessage_Content").val();
                                if (sContent == null || sContent == "") { return eui.alertInfo("请输入短信内容"); }
                                if ($("#sed_sMessage_Content").val().length > 70) {
                                    return eui.alertInfo("短信内容不能超过70字");
                                }
                                f.post('/Admin/Message/SendShortMessage', { sPeople: name, sContent: sContent, difference:look}, function (r) {
                                    eui.alertInfo("发送成功");
                                    $("#send_message_sy_system").text(parseInt($("#send_message_sy_system").text()) - look);
                                    $(div).dialog("close");
                                    reloadData();
                                })
                            }
                        }, function (res) { });
                    }
                    else {
                        f.post('/Admin/Message/AllShareCount', null, function (res) {
                            
                            look = res.Data;//所有分销客的数量
                            if (parseInt(look) > parseInt(overplus)) {//如果剩余短信数量小于要发送短信的数量无法发送
                                eui.alertInfo("剩余短信条数不够，无法发送");
                                $(div).dialog("close");
                            }
                            else {
                                var sContent = $("#sed_sMessage_Content").val();
                                if (sContent == null || sContent == "") { return eui.alertInfo("请输入短信内容"); }
                                if ($("#sed_sMessage_Content").val().length > 70) {
                                    return eui.alertInfo("短信内容不能超过70字");
                                }
                                name = "全部分享客";
                                debugger
                                f.post('/Admin/Message/SendShortMessage', { sPeople: name, sContent: sContent, difference: look }, function (r) {
                                    eui.alertInfo("发送成功");
                                    
                                    $("#send_message_sy_system").text(parseInt($("#send_message_sy_system").text()) - look);
                                    $(div).dialog("close");
                                    reloadData();
                                });
                            }
                        }, function (res) { });

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
            },
            onLoad: function () {
                $('input:radio[name=choseVip]').click(function () {
                    if ($(this).val() === "1") {
                        name = "全部会员";
                    } else {
                        name = "全部分销客";
                    }
                });
            }
        });
    }
        //发送短信
         function sendmessage()
         {
             showDialog();
             
         }
        //发送给本店所有的分销客
         function shareMessage()
         {
             
             var div = $("<div/>");
             div.dialog({
                  title: '发送短信',
                  width: 900,
                  height: 450,
                  cache: false,
                  href: '/Admin/Message/AddShare',
                  modal: true,
                  collapsible: false,
                  minimizable: false,
                  maximizable: false,
                  resizable: false,
                  buttons: [{
                      text: '发送',
                      iconCls: 'icon-save',
                      handler: function () {
                      var look = "";
                          //查询所有分销客的数量
                      f.post('/Admin/Message/AllShopShareCount', null, function (res) {
                          debugger
                          if (res.Data.count == 0) {
                              $(div).dialog("close");
                              return eui.alertInfo("本店没有分享客");
                            
                          }
                          look = res.Data.count;//所有会员数量
                                if (parseInt(look) > parseInt(overplus)) {//如果剩余短信数量小于要发送短信的数量无法发送
                                 eui.alertInfo("剩余短信条数不够，无法发送");
                                 $(div).dialog("close");
                                  }
                                else {
                                    var name = "本店所有分享客";
                                    var sContent = $("#share_send_message").val();
                                    if ($("#share_send_message").val().length > 70) {
                                        return eui.alertInfo("短信内容不能超过70字");
                                    }
                                    f.post('/Admin/Message/SendShortMessage', { sPeople: name, sContent: sContent, difference: look }, function (r) {
                                        eui.alertInfo("发送成功");
                                        $("#send_message_sy_system").text(parseInt($("#send_message_sy_system").text()) - look);
                                             $(div).dialog("close");
                                             reloadData();
                                         })
                                     }
                      }, function (res) {
                      });
                            
                             
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
                       
                     }
                 });
             
         }
        //查找
         function searchMessage() {
             grid.datagrid("getPager").pagination("select");

         }

        //详情窗口
         function detailShowDialog(row) {
             
            var div = $("<div/>");
             div.dialog({
                 title: '短信详情',
                 width: 900,
                 height: 450,
                 cache: false,
                 href: '/Admin/Message/Detail?ID='+row.ID,
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
                     $(div).dialog("destroy"); div = null;
                 },
                 onLoad: function () {
                 }
             });
         }

        //只能选中一行
         function CheckIsSelectOne() {
             var rows = grid.datagrid("getSelections");
             if (rows.length > 1) {
                 return eui.alertInfo("只能选中一行");
             }
             return true;
         }
        //查看详情
         function detailMessage() {
             
             if (CheckIsSelectOne()) {
                 eui.checkSelectedRow(grid, function detailM(row) {
                     detailShowDialog(row);
                 }, "请至少选择一行");
             }
             
         }


        try {
            init();
            initEvent();
        } catch (e) {
            eui.alertErr(e.message);
        }


    });


   
});