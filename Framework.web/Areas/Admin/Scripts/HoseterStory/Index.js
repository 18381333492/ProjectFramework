
$(function () {
        modules.get(enums.Modules.CACHE).setMenuDomain("店主故事", new function () {
            var eui = modules.get(enums.Modules.JQUERY_EASYUI);
            var f = modules.get(enums.Modules.FUNC);
            var ue = modules.get(enums.Modules.BAIDU_EDITOR);
            var travelObj = {};

            function init()
            {
                travelObj.editorEE = ue.initUE('hoster_story_sContent_script',$("#hostory_story").val());
            }

            function bindEvent()
            {
               
                $("#hoster_story_btn_look").bind("click", Story);
                //事件绑定
                $("#hoster_story_btn_save").bind("click", function hoster() {
                    if (travelObj.editorEE.getContent() == "") {
                        return eui.alertInfo("请输入内容");
                    } else {
                        $("#hostory_story").val(travelObj.editorEE.getContent());
                    }
                    f.post("/Admin/HosterMessage/UpdateHoster", $("#hoster_story_index_form").serializeObject(), function () {
                        eui.alertInfo("保存成功");
                    }, function () { eui.alertInfo("保存失败"); })
                });
            }
            //预览
            function Story() {
                if (travelObj.editorEE.getContent() == "") {
                    return eui.alertInfo("请输入内容");
                } else {
                    $("#hostory_story").val(travelObj.editorEE.getContent());
                }
                f.post("/Admin/HosterMessage/UpdateHoster", $("#hoster_story_index_form").serializeObject(), function () {
                    var id = $("#host_id_story").val();
                    PreviewGoods(id);
                }, function () { eui.alertInfo("保存失败"); })
            }
            //预览
            function PreviewGoods(ID) {
                var div = $("<div/>");
                div.dialog({
                    title: "故事预览",
                    width: 300,
                    height: 300,
                    href: '/Admin/HosterStory/LookDetail',
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
                        f.post("/Admin/HosterMessage/UpdateHoster", { sHeadStory: $("#hostory_story").val() }, function () {          
                        }, function () { });
                        $(div).dialog("destroy");
                        div = null;
                    },
                    onLoad: function () {
                        $("#host_sotry_dord").qrcode({
                            width: 160, //宽度 
                            height: 160, //高度 
                            text: 'http://' + window.location.host + '/Client/ShopHome/HostStory?id=' ,//+ ID, //任意内容 
                        });
                    }
                });
            }




            try {
                init();
                bindEvent();
            }
            catch (e)
            { }


        });
    });
