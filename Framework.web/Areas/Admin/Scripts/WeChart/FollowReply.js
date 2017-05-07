$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("关注回复设置", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);
        var followUpload = {};
        var sState = true;

        function init() {
            //初始化数据
            f.post("/Admin/FollowReply/GetOn", null, function (res) {
                var sContentType=0;
                if (res.Data.sContentType == false) {
                    sContentType = 0;
                }
                else {
                    sContentType = 1;
                }
                f.post("/Admin/FollowReply/GetReply", { sContentType: sContentType, sReplyType: 0 }, function (e) {
                    if (sContentType === 0) {//如果是纯文本
                        
                        $("#wechart_follow_reply_text").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'none');
                        $("#text_mix_div").css('display', 'block');
                        $("#wechart_sContent").val(e.Data[0].sContent);
                    }

                    if (sContentType === 1) {
                        //如果是图文混合
                        var photo = [];
                        for (var c = 0; c < e.Data.length; c++) {
                            photo.push({ filePath: e.Data[c].sPictureUrl, sTitle: e.Data[c].sTitle, sShopUrl: e.Data[c].sShopUrl })
                        }
                        $("#wechart_follow_reply_imgtext").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'block');
                        $("#text_mix_div").css('display', 'none');
                        followUpload.UploadReplyImage = new UploadImage({
                            
                            target: "wechart_follow_reply_upload", maxFileCount: 8,
                            imgLst: photo,
                            addPic: function (file) {
                                $("#follow_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#follow_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    required: true,
                                    validType: ['isHtmlValidate'],
                                    width: 130
                                });
                            },
                            removePic: function (file) {
                                //删除链接input followUpload.UploadReplyImage.imageList
                                for (var i = 0; i < followUpload.UploadReplyImage.imageList.length; i++) {
                                    if (followUpload.UploadReplyImage.imageList[i].title == file.name) {
                                        followUpload.UploadReplyImage.imageList.splice(i, 1);
                                        $($("#follow_reply_title [data-span-id='{0}']".format(file.id))[0]).remove();
                                        $($("#follow_reply_shopurl [data-span-id='{0}']".format(file.id))[0]).remove();
                                    }
                                }
                            }
                            , createExistsImageHandle: function (exfile) {
                                
                                //初始化图片数据，添加链接
                                $("#follow_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}"/></span>'.format(exfile.id));
                                $("#follow_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}"/></span>'.format(exfile.id));
                                $($("#follow_reply_shopurl [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    validType: ['isHtmlValidate'],
                                    value: exfile.sShopUrl,
                                    required: true,
                                    width: 130
                                });
                                $($("#follow_reply_title [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    value: exfile.sTitle,
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });
                            }, removeExistsImageHandle: function (file) {
                                debugger
                                followUpload.UploadReplyImage.imageList;
                                $("#follow_reply_shopurl [data-span-id='{0}']".format(file[0].id)).remove();
                                $("#follow_reply_title [data-span-id='{0}']".format(file[0].id)).remove();
                            }
                        });
                    }



                }, function () { });
                
               
                   
                    
                  
                if (res.Data.sState == true) {
                    //开启关注回复
                    $("#wechart_follow_reply_on").attr("checked", true);
                    sState = 1;
                }
                else {
                    //关闭自动回复
                    $("#wechart_follow_reply_down").attr("checked", true);
                    sState = 0;
                }
                $("#wechart_sContent").val(res.Data.sContent);

                $('input:radio[name=sContentType]').click(function () {
                    if ($(this).val() === "0") {
                        //选择文本类型
                        $("#wechart_follow_reply_down").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'none');
                        $("#text_mix_div").css('display', 'block');
                    }
                    else {
                        //选择图文混合类型
                        $("#wechart_follow_reply_imgtext").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'block');
                        $("#text_mix_div").css('display', 'none');
                        followUpload.UploadReplyImage = new UploadImage({
                            target: "wechart_follow_reply_upload", maxFileCount: 8,
                            addPic: function (file) {
                                $("#follow_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#follow_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    required: true,
                                    validType: ['isHtmlValidate'],
                                    width: 130
                                });
                            },
                            removePic: function (file) {
                                //删除链接input
                                $("#follow_reply_title [data-span-id='{0}']".format(file.id))[0].remove();
                                $("#follow_reply_shopurl [data-span-id='{0}']".format(file.id))[0].remove();
                            }
                        });
                        
                    }
                });
                $('input:radio[name=sReplyState]').click(function () {
                    //选择自动回复关闭或开启
                    if ($(this).val() === "1") {
                        sState = true;
                    } else {
                        sState = false;
                    }
                });

            }, function () {
                //如果数据库中没有设置,进行初始化
                $('input:radio[name=sContentType]').click(function () {
                    if ($(this).val() === "0") {
                        $("#wechart_follow_reply_down").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'none');
                        $("#text_mix_div").css('display', 'block');
                    }
                    else {
                        $("#wechart_follow_reply_imgtext").attr("checked", "checked");
                        $("#picture_text_mix_div").css('display', 'block');
                        $("#text_mix_div").css('display', 'none');
                        followUpload.UploadReplyImage = new UploadImage({
                            target: "wechart_follow_reply_upload", maxFileCount: 8,
                            addPic: function (file) {
                                $("#follow_reply_title").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sTitle" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_title [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入标题",
                                    validType: ['isHtmlValidate'],
                                    required: true,
                                    width: 130
                                });

                                $("#follow_reply_shopurl").append('&emsp;&emsp;<span data-span-id="{0}"><input name="sShopUrl" data-link-id="{0}" width="50px"/></span>'.format(file.id));
                                $($("#follow_reply_shopurl [data-link-id='{0}']".format(file.id))[0]).textbox({
                                    missingMessage: "请输入链接地址",
                                    required: true,
                                    validType: ['isHtmlValidate'],
                                    width: 130
                                });
                            },
                            removePic: function (file) {
                                //删除链接input
                                $("#follow_reply_title [data-span-id='{0}']".format(file.id))[0].remove();
                                $("#follow_reply_shopurl [data-span-id='{0}']".format(file.id))[0].remove();
                            }
                        });
                    }
                });
                $('input:radio[name=sReplyState]').click(function () {
                    if ($(this).val() === "1") {
                        sState = true;
                    } else {
                        sState = false;
                    }
                });
               
            })
        }
        //事件绑定
        function bindEvent() {
          
            $("#wechart_follow_reply_save_button").bind('click', function () {
                f.post("/Admin/FollowReply/GetsOriginalID", null, function (res) {
                    debugger
                    var json = $("#wechart_follow_reply_form").serializeObject();
                    if (json.sContentType === "0") {
                        json.sContent = $("#wechart_sContent").val();
                        if ($("#wechart_sContent").val() == null || $("#wechart_sContent").val() == "") {
                            return eui.alertInfo("请输入文本内容");
                        }
                        json.sContentType = 0;
                        json.sPictureUrl = "";
                        json.sShopUrl = "";
                        json.sTitle = "";

                    }
                    else {
                        if (!$("#wechart_follow_reply_form").form('validate')) { return false; }
                        if (followUpload.UploadReplyImage.imageList.length < 1) {
                            return eui.alertInfo("请上传图片");
                        }

                        else {
                            if (followUpload.UploadReplyImage.imageList.length == 1) {
                                json.sPictureUrl = followUpload.UploadReplyImage.imageList[0].filePath;
                                json.sShopUrl = json.sShopUrl;
                                json.sTitle = json.sTitle;
                                json.sContent = "";
                            }
                            else {
                                var url = "";
                                var title = "";
                                var sShopUrl = "";
                                debugger
                                for (var i = 0; i < followUpload.UploadReplyImage.imageList.length; i++) {
                                    url += followUpload.UploadReplyImage.imageList[i].filePath + ',';
                                    title += json.sTitle[i] + ',';
                                    sShopUrl += json.sShopUrl[i] + ',';
                                }
                                json.sPictureUrl = url.substr(0, url.length - 1);
                                json.sShopUrl = sShopUrl.substr(0, sShopUrl.length - 1);
                                json.sTitle = title.substr(0, title.length - 1);
                                json.sContent = "";
                            }
                           
                            json.sContentType = 1;
                        }

                    }
                    json.sState = sState;
                    json.sReplyType = 0;
                    json.sOriginalID = res.Data.sOriginalID;
                    f.post("/Admin/FollowReply/FollowReplySet", json, function (r) {
                        eui.alertInfo("保存成功");
                    }, function (e) { });

                }, function () {
                    return eui.alertInfo("请先完成微信绑定设置");
                })

                
               
                

            })
        }
        try {
            init();
            bindEvent();
        }
        catch (e) { }
       
    });
});