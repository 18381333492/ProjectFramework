$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("基础设置", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var baseSettingObj = {};
        /**
         * 添加客服事件
         */
        function addEvent() {
            var html = "<li style='line-height:3'>";
            html = html.concat('<span>客服：</span>')
            html = html.concat('&nbsp;<span>QQ号：<input name="sQQ"></span>')
            html = html.concat('&emsp;&emsp;&emsp;&nbsp;&nbsp;');
            html = html.concat('<span>QQ昵称：<input name="sQQName"></span>')
            html = html.concat('&nbsp;<span><a data-id="baseSetting_index_customer_delete" href="#">删除</a></span>');
            html = html.concat("</li>");

            $("#baseSetting_index_customer_contenter_div ul").append(html);
            var ul = $("#baseSetting_index_customer_contenter_div ul li");
            $($(ul[ul.length - 1]).find("input[name='sQQ']")[0]).textbox({
                missingMessage: "请输入QQ号",
                required: true,
                validType: ['isHtmlValidate', 'isNumber'],
                width: 200
            });

            $($(ul[ul.length - 1]).find("input[name='sQQName']")[0]).textbox({
                missingMessage: "请输入QQ昵称",
                required: true,
                validType: ['chartLengthBetween([1,6])', 'isHtmlValidate'],
                width: 200
            });

            $($(ul[ul.length - 1]).find("a[data-id='baseSetting_index_customer_delete']")[0]).linkbutton({
                iconCls: 'icon-remove',
            });
        }

        /**
         * 删除客服事件
         */
        function deleteEvent() {
            $(this).parents("li").remove();
        }

        /**
         * 保存事件
         */
        function saveEvent() {
            if ($("#baseSetting_index_form").form("validate")) {

                //判断是否加起来100%
                var one = $("#baseSetting_index_iLevelOneCommissionPrecent_id").textbox("getText");
                var two = $("#baseSetting_index_iLevelTwoCommissionPrecent_id").textbox("getText");
                var three = $("#baseSetting_index_iLevelThreeCommissionPrecent_id").textbox("getText");
                if ((parseInt(one) + parseInt(two) + parseInt(three)) != 100) {
                    return eui.alertInfo("三级佣金百分比加起来必须等于100%");
                }

                var json = $("#baseSetting_index_form").serializeObject();
                debugger
                //至少上传一张图片
                json.imageList = baseSettingObj.uploadCarouselImage.imageList;
                if (json.imageList.length < 1) {
                    return eui.alertInfo("至少上传一张图片");
                }
                debugger
                json.customerCount = $("#baseSetting_index_form").find("input[name='sQQ']").length;

                if (parseFloat(json.iUserMoney) != 0) {
                    if (parseFloat(json.iUserMoney) <= parseFloat(json.iReturnMoney)) {
                        eui.alertErr("使用条件必须要大于优惠券的价格");
                        return;
                    }
                }
                //发送数据
                f.post("/Admin/BaseSetting/DoEdit", json, function (res) {
                    eui.alertInfo("保存成功");
                }, function (res) {
                    eui.alertInfo(res.Msg);
                });
            }
        }

        /**
         * 事件绑定
         */
        function bindEvent() {
            $("#baseSetting_index_form").on("click", "#baseSetting_index_btn_add_customer", addEvent)
            .on("click", "[data-id='baseSetting_index_customer_delete']", deleteEvent)
            .on("click", "#baseSetting_index_btn_save", saveEvent)
        }

        /**
         * 初始化图片数据
         */
        function initImages() {
            f.post("/Admin/BaseSetting/GetImages", null, function (res) {
                //绑定页面数据
                var imagePaths = [];
                for (var item in res.Data) {
                    imagePaths.push({ filePath: res.Data[item].sImagePath, sLink: res.Data[item].sLink });
                }

                //初始化图片上传（轮播图）
                baseSettingObj.uploadCarouselImage = new UploadImage({
                    target: "baseSetting_index_upload", maxFileCount: 5,
                    imgLst: imagePaths,
                    addPic: function (file) {
                        //添加 链接input
                        //missingMessage="请输入链接地址" data-options="required:true" style="width:180px"
                        $("#baseSetting_index_carousel_links").append('&emsp;&emsp;<span data-span-id="{0}">链接：<input name="sLinks" data-link-id="{0}"/></span>'.format(file.id));
                        $($("#baseSetting_index_carousel_links [data-link-id='{0}']".format(file.id))[0]).textbox({
                            missingMessage: "请输入链接地址",
                            required: true,
                            validType: ['isHtmlValidate'],
                            width: 180
                        });
                    },
                    removePic: function (file) {
                        //删除链接input
                        $($("#baseSetting_index_carousel_links [data-span-id='{0}']".format(file.id))[0]).remove();
                    }, createExistsImageHandle: function (exfile) {
                        //初始化图片数据，添加链接
                        $("#baseSetting_index_carousel_links").append('&emsp;&emsp;<span data-span-id="{0}">链接：<input name="sLinks" data-link-id="{0}"/></span>'.format(exfile.id));
                        $($("#baseSetting_index_carousel_links [data-link-id='{0}']".format(exfile.id))[0]).textbox({
                            missingMessage: "请输入链接地址",
                            validType: ['isHtmlValidate'],
                            value: exfile.sLink,
                            required: true,
                            width: 180
                        });
                    }, removeExistsImageHandle: function (exfile) {
                        $($("#baseSetting_index_carousel_links [data-span-id='{0}']".format(exfile[0].id))[0]).remove();
                    }
                });
            }, function (res) {
                eui.alertInfo(res.Msg);
            })
        }

        try {
            bindEvent();
            initImages();
        } catch (e) {

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
            destroy: destroy
        };

    });
});