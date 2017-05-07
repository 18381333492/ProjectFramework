$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("站内信管理", new function () { });
    var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
    var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
    var grid = $("#mail_manager_menu");
    var iRecevierType=0;


    //初始化datagid
    function init() {
        eui.bindPaginationEvent(grid, {
            loadMsg: "正在加载...",
            toolbar: "#mail_manager_tool",
            pagination: true,
            rownumbers: true,
            fit: true,
            columns: [[
            { field: 'checkbox', checkbox: true },
             { field: 'ID', title: 'ID', align: 'center', width: 10 ,hidden:true},
              {
                  field: 'iRecevierType', title: '收件人', align: 'center', width: 200, formatter: function (value, row, index) {
                      switch (row.iRecevierType) {
                          case 0:
                              return "所有会员";
                          case 1:
                              return "部分会员";
                          default:
                              break;
                      }
                  }
              },
            { field: 'sMsgTitle', title: '标题', align: 'center', width: 230 },
            { field: 'dInsertTime', title: '发送时间', align: 'center', width: 300 },
            { field: 'sSender', title: '发送人', align: 'center', width: 200 },
            {
                field: 'sMsgContent', title: '内容', align: 'center', width: 500, formatter: function (value, row, index)
                  {  var a=value.length >= 20 ? value.substr(0, 20) + "..." : value;
                 return "<span style='color:red'>"+a+" </span>" }
            }         
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
    function loadDate(pageNumber, pageSize, sKeyword, dStartTime, dEndTime) {
        
        var keyword = $("#search_mail_userid").val();
        var dStartTime = $("#start_send_mail").val();//开始时间
        var dEndTime = $("#end_send_mail").val();//结束时间
        f.post("/Admin/Mail/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: keyword, dStartTime: dStartTime, dEndTime: dEndTime }, function (r) {
                    
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
    //绑定事件
    function initEvent() {
        $("#mail_manager_form")
            .on("click", "a[data-id='send_button']", mailAdd)
            .on("click", "a[data-id='search_button']", mailSearch)
            .on("click", "a[data-id='detail_button']", lookMail)
            .on("click", "a[data-id='delete_button']", deleteMail)
    }


    //只能选中一行
    function CheckIsSelectOne() {
        var rows = grid.datagrid("getSelections");
        if (rows.length > 1) {
            return eui.alertInfo("只能选中一行");
        }
        return true;
    }
    //重新加载数据
    function reloadData() {
        grid.datagrid("getPager").pagination("select");
    }
    //添加窗口
    var div =null
    function showDialog(title, url, func) {
        div = $("<div/>");
        div.dialog({
            title: title,
            width: 900,
            height:500,
            cache: false,
            href: url,
            modal: true,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            buttons: [{
                text: '发送',
                iconCls: 'icon-save',
                handler: function () {
                    func();
                    //$(div).dialog("close");
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
               
                loadCombo();
                $("#searchMail_button").bind("click", searchVip);
            }
        });
    }

    //详情窗口
    function mailDetail(row)
    {
        
        var showmail = $("<div/>");
        showmail.dialog({
            title: '详情',
            //href: '/Admin/Mail/Detail?ID='+row.ID,
            href: '/Admin/Mail/Detail',
            width: 700,
            height: 400,
            modal: true,
            collapsible: false,
            minimizable: false,
            maximizable: false,
            resizable: false,
            buttons: [{
                text: '关闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    $(showmail).dialog("close");
                }
            }],
           
        onClose: function () {

            $(showmail).dialog("destroy"); showmail = null;
        },
        onLoad: function () {
           //加载详细信息
            f.post("/Admin/Mail/GetDetailById", { ID: row.ID.toString() }, function (res) {
                var mail_type = "<table class='styletable' style='height:300px'>";
                mail_type = mail_type.concat("<tr><td>");
                mail_type = mail_type.concat("<span>发件人:</span>");
                mail_type = mail_type.concat("</td>");
                mail_type = mail_type.concat("<td>");
                mail_type = mail_type.concat("<span>" + res.Data.sSender + "</span>");
                mail_type = mail_type.concat("</td></tr>");
                mail_type = mail_type.concat("<tr><td>");
                mail_type = mail_type.concat("<span>标题:</span>");
                mail_type = mail_type.concat("</td>");
                mail_type = mail_type.concat("<td>");
                mail_type = mail_type.concat("<span>" + res.Data.sMsgTitle + "</span>");
                mail_type = mail_type.concat("</td></tr>");
                mail_type = mail_type.concat("<tr><td>");
                mail_type = mail_type.concat("<span>收件人:</span>");
                mail_type = mail_type.concat("</td>");
                mail_type = mail_type.concat("<td>");
                if (res.Data.iRecevierType == 0) {
                    //当发送类型是全部会员时，显示全部会员
                    mail_type = mail_type.concat("<span>全部会员</span>");
                    mail_type = mail_type.concat("</td></tr>");
                    mail_type = mail_type.concat("<tr><td>");
                    mail_type = mail_type.concat("<span>发送时间:</span>");
                    mail_type = mail_type.concat("</td>");
                    mail_type = mail_type.concat("<td>");
                    mail_type = mail_type.concat("<span>" + res.Data.dInsertTime + "</span>");
                    mail_type = mail_type.concat("</td></tr>");
                    mail_type = mail_type.concat("<tr style='height:150px'><td>");
                    mail_type = mail_type.concat("<span>文章内容:</span>");
                    mail_type = mail_type.concat("</td>");
                    mail_type = mail_type.concat("<td>");
                    mail_type = mail_type.concat("<span>" + res.Data.sMsgContent + "</span>");
                    mail_type = mail_type.concat("</td></tr>");

                    mail_type = mail_type.concat("</table>");

                    $("#getDetail_mail").append(mail_type);
                }
                else {
                    //当是部分会员时，判断是否有四个以上成员，如果有隐藏四个会员后面的名字，如果没有直接显示
                    var cc = "";
                    f.post("/Admin/Mail/SerachName", { ID: row.ID.toString() }, function (r) {
                        if (r.Data.length > 4) {
                            for (var i = 4; i < r.Data.length; i++) {
                                cc += r.Data[i].sReceiver + ',';
                            }
                            var jj = ',' + cc.substring(0, cc.length - 1);
                            mail_type = mail_type.concat("<div><span>" + r.Data[0].sReceiver + "," + r.Data[1].sReceiver + "," + r.Data[2].sReceiver + ", " + r.Data[3].sReceiver + "<div id='search_ex'><a id='click_me' href='#'>展开</a></div><div id='mail_type_yincang' style='display:none'>" + jj + "</div></span></div>");
                            
                        }
                        else {
                            
                            
                            for (var j = 0; j < r.Data.length; j++) {
                                cc += r.Data[j].sReceiver + ',';
                            }
                           var receiver = cc.substring(0, cc.length - 1);
                            mail_type = mail_type.concat("<span>" + receiver + "</span>");
                           
                        }
                        mail_type = mail_type.concat("</td></tr>");
                        mail_type = mail_type.concat("<tr><td>");
                        mail_type = mail_type.concat("<span>发送时间:</span>");
                        mail_type = mail_type.concat("</td>");
                        mail_type = mail_type.concat("<td>");
                        mail_type = mail_type.concat("<span>" + res.Data.dInsertTime + "</span>");
                        mail_type = mail_type.concat("</td></tr>");
                        mail_type = mail_type.concat("<tr  style='height:150px'><td>");
                        mail_type = mail_type.concat("<span>文章内容:</span>");
                        mail_type = mail_type.concat("</td>");
                        mail_type = mail_type.concat("<td>");
                        mail_type = mail_type.concat("<span>" + res.Data.sMsgContent + "</span>");
                        mail_type = mail_type.concat("</td></tr>");

                        mail_type = mail_type.concat("</table>");

                        $("#getDetail_mail").append(mail_type);
                        //点开展开时，显示隐藏的会员姓名
                        $("#click_me").bind("click", function () {
                            $("#mail_type_yincang").css('display', 'block');
                            $("#search_ex").css('display', 'none');
                        });

                    })
                }
                
            })

          
        }
    });
    }

    //详情
    function lookMail() {
        if (CheckIsSelectOne())
        {
            eui.checkSelectedRow(grid, function detailA(row) {
                mailDetail(row);
            },
            "请选择一行数据");
        }
    }
    //初始化用户数据
    function vipInit(vipgrid,isclose) {
        eui.bindPaginationEvent(vipgrid, {
            loadMsg: "正在加载...",
            pagination: true,
            rownumbers: true,
            fit: false,
            closed: isclose,
            height:400,
            //下面的列都是一些示例，用的时候直接替换
            columns: [[
            { field: 'checkbox', checkbox: true },
            { field: 'ID', title: 'ID', align: 'center',hidden:true },
            { field: 'sName', title: '姓名', align: 'center', hidden: true },
            { field: 'sNickName', title: '昵称', width: 200, align: 'center' },
            {
                field: 'iClientType', title: '角色', align: 'center', width: 200, formatter: function (value, row, index) {
                    switch (row.iClientType) {
                        case 0:
                            return "会员";
                        case 1:
                            return "分享客";
                        default:
                            break;
                    }
                }
            },
            { field: 'sPhone', title: '电话', width: 200, align: 'center' },
            { field: 'dAddTime', title: '添加时间', width: 200, align: 'center' },
            
            ]],
            onLoadSuccess: function () {
                $(".datagrid-header-check input[type=checkbox]").remove();
            }, rowStyler: function (index, row) {
                if (row.tUserState === 1) {
                    return 'background-color:gray;color:white';
                }
            }
        }, loadDateVip).pagination("select");
    }

    //查询数据的回调方法
    function loadDateVip(pageNumber, pageSize) {
        
        var sKeyword = $("#search_vipname_mail").val();
        f.post("/Admin/Mail/GetVipList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword }, function (r) {
                    
            $("#vip_Message_part").datagrid("loadData", r.Data.Result);
            $("#vip_Message_part").datagrid("getPager").pagination({
                pageNumber: pageNumber,
                pageSize: pageSize,
                total: r.Data.MaxCount
            });
        }, function (r) {
            eui.alertErr(r.Msg);
        });
    }
    //发送邮件
    function mailAdd() {
        showDialog("发送邮件", "/Admin/Mail/Add", function doAdd() {
            
            if (!$("#mail_add_form").form('validate')) { return false; }
            if (iRecevierType == 1) {
                eui.confirmDomainByMultiRows($("#vip_Message_part"), domail, "", "请至少选择一条数据", function email(rows) {
                    return "您确定给这【{0}】人发站内信".format(rows.length);
                });
            }
            else {
                
                var sMsgTitle = $("#add_sMsgTitle_mail").val();
                var sMsgContent = $("#add_sMsgContent_mail").val();
                if (sMsgContent == null || sMsgContent == "") { return eui.alertInfo("请输入您要发送的内容");}
                f.post("/Admin/Mail/SendMail", { iRecevierType: iRecevierType, sMsgTitle: sMsgTitle, sMsgContent: sMsgContent }, function () {

                    eui.alertInfo("发送成功");
                    $(div).dialog("close");
                    loadDate();

                })
            }

        })
    }
    //发送
    function domail(rows) {
        
        
        var sMsgTitle = $("#add_sMsgTitle_mail").val();
        var sMsgContent = $("#add_sMsgContent_mail").val();
        var rowID = [];
        var rowName = [];
        var sNick = [];
        for (var i = 0; i < rows.length; i++) {
            rowID.push(rows[i].ID);
            rowName.push(rows[i].sName);
            sNick.push(rows[i].sNickName);
        }
        if (sMsgContent == null || sMsgContent == "") { return eui.alertInfo("请输入您要发送的内容"); }
        f.post("/Admin/Mail/SendMail", { iRecevierType: iRecevierType, sMsgTitle: sMsgTitle, sMsgContent: sMsgContent, sReceiverID: rowID.toString(), sName: rowName.toString(), sNickName: sNick.toString() }, function () {
            reloadData();
            eui.alertInfo("发送成功");
            $(div).dialog("close");
            loadDate();
            
        })
    }
    //下拉菜单绑定的数据
    var localData = [{
        "value": 0,
        "text": "所有会员",
        "selected": true
    }, {
        "value": 1,
        "text": "部分会员"
    }];
    //下拉菜单初始化
    function loadCombo() {
        $("#iRecevierType_mail").combobox({
            data: localData,
            textField: "text",
            valueField: "value",
            editable: false,
            onSelect: function (res) {
                
                var vipgrid = $("#vip_Message_part");
                if (res.value == 1) {
                    iRecevierType = 1;
                    vipInit(vipgrid,false);
                }
                else {
                    iRecevierType = 0;
                    vipInit(vipgrid, true);
                }
            }
        });
    }
    //查询
    function mailSearch() {
        grid.datagrid("getPager").pagination("select");
    }
    //删除邮件
    function deleteMail() {
        eui.confirmDomainByMultiRows(grid,dodel,"", "请至少选择一条数据", function del(rows) {
            return "您确定删除这【{0}】封站内信?".format(rows.length);
        })
    }
    //删除邮件回调
    function dodel(rows) {
        var rowIDs = [];
        
        //遍历所有的行创建ID集合
        for (var i = 0; i < rows.length; i++) {
            rowIDs.push(rows[i].ID);
        }
            
        f.post('/Admin/Mail/DeleteMail', { ID: rowIDs.toString() }, function (res) {
            reloadData();
            grid.datagrid("clearSelections");
            eui.alertInfo("删除成功");
        }, function (res) {
            eui.alertErr("删除失败");
        });
    }
    //查找用户
    function searchVip()
    {
        var vipgrid = $("#vip_Message_part");
        vipInit(vipgrid);
    }

    try {
        initEvent();
        init();
    } catch (e) {
        eui.alertErr(e.message);
    }
    

});