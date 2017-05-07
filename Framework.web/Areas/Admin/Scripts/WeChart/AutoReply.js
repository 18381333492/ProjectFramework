$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("自动回复", new function () {

        var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
        var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
        var ue = modules.get("ue");//UEditor模块
        var autoReply = {};
        var sContentType = 0;
        var sState = 1;
        //初始化
        function init() {
            //判断是否开启以及类型
            f.post("/Admin/AutoReply/GetOn", null, function (res) {
                var sContentType = 0;
                if (res.Data.sContentType == false) {
                    sContentType = 0;
                }
                else {
                    sContentType = 1;
                }
                f.post("/Admin/AutoReply/GetReply", { sContentType: sContentType, sReplyType: 2 }, function (e) {
                    if (res.Data.sContentType == false) {
                        //根据文章类型判断div的显示,并给相应的文本框赋值
                        $("#dd_auto_reply_text_content_div").css('display', 'block');
                        $("#add_auto_reply_picture_content_div").css('display', 'none');
                        $("#add_keyword_reply_on").attr("checked", true);
                        $("#add_auto_reply_picture").attr("checked", false);
                        sContentType = 0;
                        $("#add_auto_reply_picture_scontent_add").val(e.Data[0].sContent);//如果是文本类型赋值到文本框
                    }
                    else {
                    //隐藏或显示
                    $("#dd_auto_reply_text_content_div").css('display', 'none');
                    $("#add_auto_reply_picture_content_div").css('display', 'block');
                    $("#add_auto_reply_picture").attr("checked", true);
                    $("#add_keyword_reply_on").attr("checked", false);
                    var photo = [];
                    for (var c = 0; c < e.Data.length; c++) {
                        photo.push({ filePath: e.Data[c].sPictureUrl, sTitle: e.Data[c].sTitle, sShopUrl: e.Data[c].sShopUrl, sContent: e.Data[c].sContent })
                    }
                    autoReply.uploadlImage = new UploadImage({
                        target: "add_auto_reply_upload", maxFileCount: 8, imgLst: photo,
                        addPic: function (file) {
                            
                            $("#auto_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                            $($("#auto_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                missingMessage: "请输入标题",
                                validType: ['isHtmlValidate'],
                                required: true,
                                width: 130
                            });
                            $("#auto_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                            $($("#auto_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                missingMessage: "请输入内容",
                                validType: ['isHtmlValidate'],
                                required: true,
                                width: 130
                            });

                            $("#auto_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                            $($("#auto_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                missingMessage: "请输入链接地址",
                                required: true,
                                validType: ['isHtmlValidate'],
                                width: 130
                            });
                        },
                        removePic: function (file) {
                            //删除链接input
                            debugger
                            for (var i = 0; i < autoReply.uploadlImage.imageList.length; i++) {
                                if (autoReply.uploadlImage.imageList[i].title == file.name) {
                                    autoReply.uploadlImage.imageList.splice(i, 1);
                                   $($("#auto_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                   $($("#auto_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                   $($("#auto_reply_content [data-span-id='{0}']".format(file.id))[0]).remove();
                                }
                            }
                        }
                         , createExistsImageHandle: function (exfile) {
                                //初始化图片数据，添加链接
                                $("#auto_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(exfile.id));
                                $("#auto_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}"/></span>'.format(exfile.id));
                                $("#auto_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}"/></span>'.format(exfile.id));
                                $($("#auto_reply_content [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                    missingMessage: "请输入内容",
                                    validType: ['isHtmlValidate'],
                                    value: exfile.sContent,
                                    required: true,
                                    width: 130
                                });
                                $($("#auto_reply_shopurl [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    validType: ['isHtmlValidate'],
                                    value: exfile.sShopUrl,
                                    required: true,
                                    width: 130
                                });
                                $($("#auto_reply_title [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    value: exfile.sTitle,
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });
                            }, removeExistsImageHandle: function (exfile) {
                                $("#auto_reply_shopurl [data-span-id='{0}']".format(exfile[0].id)).remove();
                                $("#auto_reply_title [data-span-id='{0}']".format(exfile[0].id)).remove();
                                $("#auto_reply_content [data-span-id='{0}']".format(exfile[0].id)).remove();
                            }

                    });

                    sContentType = 1;
                    
                }
                if (res.Data.sState == false) {
                    $("#add_auto_reply_down").attr("checked", true);
                    sState = 0;
                }
                else {
                    $("#add_auto_reply_on").attr("checked", true);
                    sState = 1;
                }
                $('input:radio[name=sReplyState]').click(function () {
                    if ($(this).val() === "0") {
                        $("#dd_auto_reply_text_content_div").css('display', 'block');
                        $("#add_auto_reply_picture_content_div").css('display', 'none');
                        $("#add_keyword_reply_on").attr("add_auto_reply_text", true);
                        $("#add_auto_reply_picture").attr("checked", false);
                        sContentType = 0;
                    } else {
                        debugger
                        sContentType = 1;
                        $("#dd_auto_reply_text_content_div").css('display', 'none');
                        $("#add_auto_reply_picture_content_div").css('display', 'block');
                        $("#add_auto_reply_picture").attr("checked", true);
                        $("#add_keyword_reply_on").attr("checked", false);
                        autoReply.uploadlImage = new UploadImage({
                            target: "add_auto_reply_upload", maxFileCount: 8,
                            addPic: function (file) {
                                $("#auto_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#auto_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入内容",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#auto_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    required: true,
                                    validType: ['isHtmlValidate'],
                                    width: 130
                                });
                            },
                            removePic: function (file) {
                                //删除链接input
                                for (var i = 0; i < autoReply.uploadlImage.imageList.length; i++) {
                                    if (autoReply.uploadlImage.imageList[i].title == file.name) {
                                        autoReply.uploadlImage.imageList.splice(i, 1);
                                        $($("#auto_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                        $($("#auto_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                        $($("#auto_reply_content [data-span-id='{0}']".format(file.id))[0]).remove();
                                    }
                                }
                            }
                        });
                        
                    }
                });
                $('input:radio[name=reply_on]').click(function () {
                    if ($(this).val() === "0") {
                        //状态关闭
                        sState = 0;
                    } else {
                        //状态开启
                        sState = 1;
                    }
                });
                }, function () { });
            }, function () {
                //如果数据库中没有数据，初始化整个页面
                $('input:radio[name=sReplyState]').click(function () {
                    if ($(this).val() === "0") {
                        $("#dd_auto_reply_text_content_div").css('display', 'block');
                        $("#add_auto_reply_picture_content_div").css('display', 'none');
                        $("#add_keyword_reply_on").attr("add_auto_reply_text", true);
                        sContentType = 0;
                    } else {
                        $("#dd_auto_reply_text_content_div").css('display', 'none');
                        $("#add_auto_reply_picture_content_div").css('display', 'block');
                        $("#add_auto_reply_picture").attr("checked", true);
                        autoReply.uploadlImage = new UploadImage({
                            target: "add_auto_reply_upload", maxFileCount: 8,
                            addPic: function (file) {
                                $("#auto_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });
                                $("#auto_reply_content").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sContent" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_content [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入内容",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#auto_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#auto_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    required: true,
                                    validType: ['isHtmlValidate'],
                                    width: 130
                                });
                            },
                            removePic: function (file) {
                                //删除链接input
                                for (var i = 0; i < autoReply.uploadlImage.imageList.length; i++) {
                                    if (autoReply.uploadlImage.imageList[i].title == file.name) {
                                        autoReply.uploadlImage.imageList.splice(i, 1);//删除图片信息
                                        $($("#auto_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                        $($("#auto_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                        $($("#auto_reply_content [data-span-id='{0}']".format(file.id))[0]).remove();
                                    }
                                }
                            }

                        });
                        sContentType = 1;
                    }
                });
                $('input:radio[name=reply_on]').click(function () {
                    if ($(this).val() === "0") {
                        sState = 0;
                    } else {
                        sState = 1;
                    }
                });
            })

        }

       
        function initEvent() {
            var url = "";
            var title = "";
            var sShopUrl = "";
            var sContent = "";
            //保存
            $("#add_auto_reply_btn_save").bind("click", function () {
                debugger
                f.post("/Admin/FollowReply/GetsOriginalID", null, function (res) {
                    //如果是文字
                    var json = $("#add_auto_reply_form").serializeObject();
                    if ($('input:radio[name=sReplyState]:checked').val() == 0) {
                        //判断是否输入了内容
                        if ($("#add_auto_reply_picture_scontent_add").val() == null || $("#add_auto_reply_picture_scontent_add").val() == "") {
                            return eui.alertInfo("请输入内容");
                        }
                        else {
                            json.sContent = $("#add_auto_reply_picture_scontent_add").val();
                            json.sPictureUrl = "";
                            json.sShopUrl = "";
                            json.sTitle = "";
                            json.sContentType=0
                        }
                    }
                    else {
                        //如果是图文
                        if (!$("#add_auto_reply_form").form('validate')) { return false; }
                        if (autoReply.uploadlImage.imageList.length < 1) {
                            return eui.alertInfo("请上传图片");
                        }

                        else {
                            
                            if (autoReply.uploadlImage.imageList.length == 1) {
                                debugger
                                json.sPictureUrl = autoReply.uploadlImage.imageList[0].filePath;
                                json.sShopUrl =json.sShopUrl;
                                json.sTitle = json.sTitle;
                                json.sContent = json.sContent;
                            }
                            else {
                                for (var i = 0; i < autoReply.uploadlImage.imageList.length; i++) {
                                    url += autoReply.uploadlImage.imageList[i].filePath + ',';
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
                    json.sReplyType = 2;
                    json.sOriginalID = res.Data.sOriginalID;
                    json.sState = sState;
                    //执行添加
                    f.post("/Admin/FollowReply/FollowReplySet", json, function (r) {
                        debugger
                        eui.alertInfo("保存成功");
                    }, function (e) { });
                },
                function (res) {
                    return eui.alertInfo("请先完成微信绑定设置");
                });

               

            })
        }
        init();
        initEvent();
       
    });
});