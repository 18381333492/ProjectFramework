$(function () {

    modules.get(enums.Modules.CACHE).setMenuDomain("关键字回复设置", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);
        var grid = $("#wechart_keyword_reply_menu");
        var keyword = {};
        var sContentType = 0;
        var sType = "";
        //页面数据绑定
        function init() {
            eui.bindPaginationEvent(grid, {
                loadMsg: "正在加载...",
                toolbar: "#wechart_keyword_reply_tool",
                pagination: true,
                rownumbers: true,
                fit: true,
                columns: [[
                 { field: 'checkbox', checkbox: true },
                 
                 { field: 'sKeyword', title: '关键字', align: 'center', width: 260, },
                 {
                   field: 'sContentType', title: '回复类型', align: 'center', width: 200, formatter: function (value, row, index) {
                         var state;
                         switch (row.sContentType) {
                             case 0:
                                 state = "纯文本";
                                 break;
                             case 1:
                                 state = "图文混合";
                                 break;
                             default:
                                 break;

                         }
                         return state;
                     }
                 },
                 {
                     field: 'content', title: '内容', align: 'center', width: 300,
                     formatter: function (value, row, index)
                     {
                         return value.length >= 20 ? value.substr(0, 20) + "..." : value
                     }
                 },
                ]],
                onLoadSuccess: function () {
                    $(".datagrid-header-check input[type=checkbox]").remove();
                }
            }, loadDate).pagination("select");
        }

        //查询数据的回调方法
        function loadDate(pageNumber, pageSize) {
            try {
                if ($("#wechart_keyword_reply_form").form("validate")) {
                    var param/*查询参数*/ = $("#wechart_keyword_reply_form").serializeObject();
                    param.PageIndex/*当前页码*/ = pageNumber;
                    param.pageSize/*每页显示条数*/ = pageSize;
                    var sKeyword = $("#wechart_keyword_reply_search").val();
                    f.post("/Admin/KeyWordReply/GetPageList", { PageIndex: pageNumber, PageSize: pageSize, sKeyword: sKeyword, sContentType: sType }, function (r) {
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

        //下拉菜单绑定的数据
        var localData = [{
            "value": 0,
            "text": "全部",
            "selected": true
        }, {
            "value": 1,
            "text": "纯文本"
        }, {
            "value": 2,
            "text": "图文混合"
        }
        ];
        //下拉菜单初始化
        function loadCombo() {
            $("#wechart_keyword_reply_type").combobox({
                data: localData,
                textField: "text",
                valueField: "value",
                editable: false,
                onSelect: function (res) {
                    
                    if (res.value === 0) {
                        sType = "";
                    }
                    else if (res.value === 1) {
                        sType = 0;
                    }
                    else {
                        sType = 1;
                    }
                }
            });
        }
        //绑定事件
        function initEvent() {
            $("#wechart_keyword_reply_form")
                .on("click", "a[data-id='add_keyword_reply_button']", addKey)
                .on("click", "a[data-id='edit_keyword_reply_button']", editKey)
                .on("click", "a[data-id='delete_keyword_reply_button']", deleteKey)
                .on("click", "a[data-id='search_keyword_reply_button']", reloadData)
            //打开关闭绑定事件
            $('input:radio[name=sReplyState]').click(function () {
                var states = true;
                if ($(this).val() === "0") {
                    states = false;
                }
                else {
                    states = true;
                }
                f.post("/Admin/FollowReply/GetsOriginalID", null, function (res) {
                    var ContentType = true;
                    var sContentType = $('input:radio[name=replyType]:checked').val();
                    if (sContentType == 1) {
                        ContentType = true;
                    }
                    else {
                        ContentType = false;
                    }
                    var sOriginalID = res.Data.sOriginalID;
                    f.post("/Admin/KeyWordReply/ChangeContentType", { sState: states, sContentType: ContentType, sOriginalID: sOriginalID }, function () {
                        eui.alertInfo("设置已保存");
                    }, function () {
                        eui.alertInfo("保存失败");
                    });

                }, function () {
                    eui.alertInfo("请完善微信绑定设置");
                });

               
            });

            //回复类型绑定
            $('input:radio[name=replyType]').click(function () {
                var ContentType = true;
                if ($(this).val() === "0") {
                    ContentType = false;
                }
                else {
                    ContentType = true;
                }

                f.post("/Admin/FollowReply/GetsOriginalID", null, function (res) {
                    debugger
                    var stateType = true;
                    var onstate = $('input:radio[name=sReplyState]:checked').val();
                    if (onstate == 1) {
                        stateType = true;
                    }
                    else {
                        stateType = false;
                    }
                    var sContentType = $('input:radio[name=replyType]:checked').val();
                    var sOriginalID = res.Data.sOriginalID;
                    f.post("/Admin/KeyWordReply/ChangeContentType", { sState: stateType, sContentType: ContentType, sOriginalID: sOriginalID }, function () {
                        eui.alertInfo("设置已保存");
                    }, function () {
                        eui.alertInfo("保存失败");
                    });

                }, function () {
                    eui.alertInfo("请完善微信绑定设置");
                });
            });
        }
        //重新加载
        function reloadData() {
            grid.datagrid("getPager").pagination("select");
        }
        //添加关键字
        function addKey() {
            var div = $("<div/>");
            div.dialog({
                title: "新增关键字",
                width: 750,
                height: 600,
                cache: false,
                href: '/Admin/KeyWordReply/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        
                        var json = $("#add_keyword_reply_form").serializeObject();
                        if ($('input:radio[name=sReplyStateType]:checked').val() == 0) {
                            if ($("#text_scontent_add").val() == null || $("#text_scontent_add").val()=="") {
                                return eui.alertInfo("请输入内容");
                            } else if ($("#keyword_apply_key").val() == null || $("#keyword_apply_key").val() == "") {
                                return eui.alertInfo("请输入关键字");
                            }
                            else {
                               
                                json.sContent = $("#text_scontent_add").val();
                                json.sPictureUrl = "";
                                json.sShopUrl = "";
                                json.sTitle = "";
                                json.sContentType = 0
                            }
                        }
                        else {

                            if (keyword.uploadlImage.imageList.length < 1) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                if (!$("#add_keyword_reply_form").form('validate')) { return false; }
                                if (keyword.uploadlImage.imageList.length == 1) {
                                    
                                    json.sPictureUrl = keyword.uploadlImage.imageList[0].filePath;
                                    json.sShopUrl = json.sShopUrl;
                                    json.sTitle = json.sTitle;
                                    json.sContent = json.sContent;
                                }
                                else {
                                    var url = "";
                                    var title = "";
                                    var sShopUrl = "";
                                    var sContent = "";
                                    for (var i = 0; i < keyword.uploadlImage.imageList.length; i++) {
                                        
                                        url += keyword.uploadlImage.imageList[i].filePath + ',';
                                        title += json.sTitle[i] + ',';
                                        sShopUrl += json.sShopUrl[i] + ',';
                                        sContent += json.sContent[i] + ',';
                                    }
                                    json.sPictureUrl = url.substr(0, url.length - 1);
                                    json.sShopUrl = sShopUrl.substr(0, sShopUrl.length - 1);
                                    json.sTitle = title.substr(0, title.length - 1);
                                    json.sContent = sContent.substr(0, sContent.length - 1);
                                }
                                json.sContentType = 1;
                            }
                        }
                        json.sKeyword = $("#keyword_apply_key").val()
                        //判断是否重名
                        f.post("/Admin/KeyWordReply/SearchKeyName", { sKeyword: $("#keyword_apply_key").val(), sContentType: sContentType }, function () {
                            return eui.alertInfo("已有同名的关键字，请重新输入");
                        }, function () {
                            //没有查到重名就执行添加
                            debugger
                            f.post("/Admin/KeyWordReply/AddKeyReply", json, function () {
                                eui.alertInfo("添加成功");
                                reloadData();
                                $(div).dialog("close");
                            });
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
                    /**
                    * 如果dialog里有对全局变量的操作并有驻留，这里记得要清除
                    */
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                    $('input:radio[name=sReplyStateType]').click(function () {
                        debugger
                        if ($(this).val() === "0") {
                            $("#text_content_div").css('display', 'block');
                            $("#picture_text_content_div").css('display', 'none');
                            $("#add_keyword_reply_on").attr("checked", true);
                            sContentType = 0;
                        } else {
                            $("#text_content_div").css('display', 'none');
                            $("#picture_text_content_div").css('display', 'block');
                            $("#add_keyword_reply_down").attr("checked", true);
                            keyword.uploadlImage = new UploadImage({
                                target: "picture_text_upload", maxFileCount: 8,
                                addPic: function (file) {
                                    var inputCount = $("#keyword_reply_title > span").length+1;
                                    $("#keyword_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(file.id));
                                    $($("#keyword_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入标题",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });
                                    $("#keyword_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入备注",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });

                                    $("#keyword_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入链接地址",
                                        required: true,
                                        validType: ['isHtmlValidate'],
                                        width: 130
                                    });
                                },
                                    removePic: function (file) {
                                        //删除链接input
                                        
                                        for (var i = 0; i < keyword.uploadlImage.imageList.length; i++) {
                                            if (keyword.uploadlImage.imageList[i].title == file.name) {
                                                keyword.uploadlImage.imageList.splice(i, 1);
                                                $($("#keyword_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                                $($("#keyword_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                                $($("#keyword_reply_content [data-span-id='{0}']".format(file.id))[0]).remove();
                                                return;
                                            }
                                        }
                                      
                                      
                                       
                                    }

                            });


                            sContentType = 1;
                        }
                    });
                }
            });
        
        }
        //编辑关键字
        function editKey() {
            eui.checkSelectedRow(grid, showDialog, "请选择一条您需要修改的数据");
        }

        //编辑关键字窗口
        function showDialog(row)
        {
            var selectedRow = grid.datagrid("getSelections");
            if (selectedRow.length > 1) {
                eui.alertInfo("只能选择一条数据")
                return false;
            }
            var div = $("<div/>");
            div.dialog({
                title: "编辑关键字",
                width: 750,
                height: 600,
                cache: false,
                href: '/Admin/KeyWordReply/Add',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '保存',
                    iconCls: 'icon-save',
                    handler: function () {
                        var json = $("#add_keyword_reply_form").serializeObject();
                        
                        if ($('input:radio[name=sReplyStateType]:checked').val() == 0) {
                            if ($("#text_scontent_add").val()== null || $("#text_scontent_add").val()== "") {
                                return eui.alertInfo("请输入内容");
                            } else if ($("#keyword_apply_key").textbox("getText") == null || $("#keyword_apply_key").textbox("getText") == "") {
                                return eui.alertInfo("请输入关键字");
                            }
                            else {
                                json.sContent = $("#text_scontent_add").val();
                                json.sContentType = 0;
                                json.sPictureUrl = "";
                                json.sShopUrl = "";
                                json.sTitle = "";
                            }
                        }
                        else {
                            if (keyword.uploadlImage.imageList.length < 1) {
                                return eui.alertInfo("请上传图片");
                            }
                            else {
                                if (!$("#add_keyword_reply_form").form('validate')) { return false; }
                                if (keyword.uploadlImage.imageList.length == 1) {
                                    debugger
                                    json.sPictureUrl = keyword.uploadlImage.imageList[0].filePath;
                                    json.sShopUrl = json.sShopUrl;
                                    json.sTitle = json.sTitle;
                                    json.sContent = json.sContent;
                                }
                                else {
                                    debugger
                                    var url = "";
                                    var title = "";
                                    var sShopUrl = "";
                                    var sContent = "";
                                    for (var i = 0; i < keyword.uploadlImage.imageList.length; i++) {
                                        url += keyword.uploadlImage.imageList[i].filePath + ',';
                                        title += json.sTitle[i] + ',';
                                        sShopUrl += json.sShopUrl[i] + ',';
                                        sContent += json.sContent[i] + ',';
                                    }
                                    json.sPictureUrl = url.substr(0, url.length - 1);
                                    json.sShopUrl = sShopUrl.substr(0, sShopUrl.length - 1);
                                    json.sTitle = title.substr(0, title.length - 1);
                                    json.sContent = sContent.substr(0, sContent.length - 1);
                                }
                                json.sContentType = 1;
                            }
                        }
                        json.sKeyword = $("#keyword_apply_key").textbox("getText");
                        json.exType = $("#keyword_type_ex").val();
                        json.exKeyword = $("#ex_keyword_apply_key").val();
                        debugger
                        if ($("#ex_keyword_apply_key").val() != $("#keyword_apply_key").textbox("getText")) {
                            f.post("/Admin/KeyWordReply/SearchKeyName", { sKeyword: $("#keyword_apply_key").val(), sContentType: json.sContentType}, function () {
                                return eui.alertInfo("已有同名的关键字，请重新输入");
                            }, function () {
                                f.post("/Admin/KeyWordReply/EditKeyReply", json, function () {
                                    eui.alertInfo("编辑成功");
                                    reloadData();
                                    $(div).dialog("close");
                                });
                            });
                        }
                        else {
                            f.post("/Admin/KeyWordReply/EditKeyReply", json, function () {
                                eui.alertInfo("编辑成功");
                                reloadData();
                                $(div).dialog("close");
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
                    f.post("/Admin/KeyWordReply/GetKeyWordByType", { sContentType: row.sContentType, sKeyword: row.sKeyword }, function (res) {
                        if (res.Data[0].sContentType === 0) {
                            //加载时隐藏或者显示div，给form赋值
                            $("#text_content_div").css('display', 'block');
                            $("#picture_text_content_div").css('display', 'none');
                            $("#add_keyword_reply_on").attr("checked", true);
                            $("#text_scontent_add").val(res.Data[0].sContent);
                            $("#keyword_apply_key").textbox("setText", res.Data[0].sKeyword);
                            $("#ex_keyword_apply_key").val(res.Data[0].sKeyword);
                            sContentType = 0;
                        }
                        else {
                            var photo = [];
                            for (var c = 0; c < res.Data.length; c++) {
                                photo.push({ filePath: res.Data[c].sPictureUrl, sTitle: res.Data[c].sTitle, sShopUrl: res.Data[c].sShopUrl, sContent: res.Data[c].sContent })
                            }

                            $("#text_content_div").css('display', 'none');
                            $("#picture_text_content_div").css('display', 'block');
                            $("#add_keyword_reply_down").attr("checked", true);
                            keyword.uploadlImage = new UploadImage({
                                target: "picture_text_upload", maxFileCount: 8,
                                imgLst: photo,
                                addPic: function (file) {
                                    var inputCount = $("#keyword_reply_title > span").length + 1;
                                    $("#keyword_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(file.id));
                                    $($("#keyword_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入标题",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });
                                    $("#keyword_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入备注",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });

                                    $("#keyword_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入链接地址",
                                        required: true,
                                        validType: ['isHtmlValidate'],
                                        width: 130
                                    });
                                },
                                removePic: function (file) {
                                    //删除链接input
                                    for (var i = 0; i < keyword.uploadlImage.imageList.length; i++) {
                                        if (keyword.uploadlImage.imageList[i].title == file.name) {
                                            keyword.uploadlImage.imageList.splice(i, 1);
                                            $($("#keyword_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                            $($("#keyword_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                            $($("#keyword_reply_content [data-span-id='{0}']".format(file.id))[0]).remove();
                                            return;
                                        }
                                    }

                                },
                                createExistsImageHandle: function (exfile) {

                                    //初始化图片数据，添加链接
                                    $("#keyword_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(exfile.id));
                                    $("#keyword_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}"/></span>'.format(exfile.id));
                                    $("#keyword_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}"/></span>'.format(exfile.id));
                                    $($("#keyword_reply_content [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                        missingMessage: "请输入内容",
                                        validType: ['isHtmlValidate'],
                                        value: exfile.sContent,
                                        required: true,
                                        width: 130
                                    });
                                    $($("#keyword_reply_shopurl [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                        missingMessage: "请输入链接地址",
                                        validType: ['isHtmlValidate'],
                                        value: exfile.sShopUrl,
                                        required: true,
                                        width: 130
                                    });
                                    $($("#keyword_reply_title [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                        missingMessage: "请输入标题",
                                        value: exfile.sTitle,
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });
                                }, removeExistsImageHandle: function (exfile) {
                                    $($("#keyword_reply_title [data-span-id='{0}']".format(exfile[0].id))[0]).remove();
                                    $($("#keyword_reply_shopurl [data-span-id='{0}']".format(exfile[0].id))[0]).remove();
                                    $($("#keyword_reply_content [data-span-id='{0}']".format(exfile[0].id))[0]).remove();
                                }


                            });
                            $("#keyword_apply_key").textbox("setText",res.Data[0].sKeyword);
                            $("#ex_keyword_apply_key").val(res.Data[0].sKeyword);
                            $("#keyword_type_ex").val(res.Data[0].sContentType);

                        }
                    }, function (res) {
                        debugger
                    })
                    $('input:radio[name=sReplyStateType]').click(function () {
                        if ($(this).val() === "0") {
                            $("#text_content_div").css('display', 'block');
                            $("#picture_text_content_div").css('display', 'none');
                            $("#add_keyword_reply_on").attr("checked", true);
                            sContentType = 0;
                        } else {
                            $("#text_content_div").css('display', 'none');
                            $("#picture_text_content_div").css('display', 'block');
                            $("#add_keyword_reply_down").attr("checked", true);
                            keyword.uploadlImage = new UploadImage({
                                target: "picture_text_upload", maxFileCount: 8,
                                addPic: function (file) {
                                    var inputCount = $("#keyword_reply_title > span").length + 1;
                                    $("#keyword_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(file.id));
                                    $($("#keyword_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入标题",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });
                                    $("#keyword_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入备注",
                                        validType: ['isHtmlValidate'],
                                        required: true,
                                        width: 130
                                    });

                                    $("#keyword_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                    $($("#keyword_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                        missingMessage: "请输入链接地址",
                                        required: true,
                                        validType: ['isHtmlValidate'],
                                        width: 130
                                    });
                                },
                                removePic: function (file) {
                                    //删除链接input
                                    $("#keyword_reply_title [data-span-id='{0}']".format(file.id))[0].remove();
                                    $("#keyword_reply_shopurl [data-span-id='{0}']".format(file.id))[0].remove();
                                    $("#keyword_reply_content [data-span-id='{0}']".format(file.id))[0].remove();
                                }

                            });
                            sContentType = 1;
                        }
                    });
                }
            });
        }
        //删除关键字
        function deleteKey() {
            eui.confirmDomainByMultiRows(grid, doDelete, "", "请选择至少一条数据", function formatconfirm(rows) {
                return "您确定要删除这【{0}】个关键字".format(rows.length);
            });
        }
        //删除关键字回调
        function doDelete(rows) {
            var sKeyword = [];
            var sContentType = [];
            //遍历所有的行创建ID集合
            for (var i = 0; i < rows.length; i++) {
                sKeyword.push(rows[i].sKeyword);
                sContentType.push(rows[i].sContentType);
            }
            f.post('/Admin/KeyWordReply/DeleteKeyReply', { sKeyword: sKeyword.toString(), sContentType: sContentType.toString() }, function (res) {
                reloadData();
                grid.datagrid("clearSelections");

                eui.alertInfo("删除成功");
            }, function (res) {
                eui.alertErr("删除失败");
            });
        }
       
        function statesBind() {
            
            f.post("/Admin/KeyWordReply/GetStates", null, function (res) {
                debugger
                if (res.Data.sState == 0) {
                    $("#wechart_keyword_reply_down").attr("checked", true);
                }
                else {
                    $("#wechart_keyword_reply_on").attr("checked", true);
                }
                if (res.Data.sContentType == 0) {
                    $("#wechart_keyword_replyType_down").attr("checked", true);
                }
                else {
                    $("#wechart_keyword_replyType_on").attr("checked", true);
                }
            })
        }

        try {
            init();
            initEvent();
            loadCombo();
            statesBind();
        }
        catch (e) { }
    });
});