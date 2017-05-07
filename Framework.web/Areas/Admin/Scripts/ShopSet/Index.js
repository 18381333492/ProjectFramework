$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("店铺设置", new function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var baseSettingObj = {};
        //初始化图片
        function initImages() {
            f.post("/Admin/ShopSet/GetImages", null, function (res) {
                $("#shop_introduce_Content").val($("#shop_introduce_Content_hidden").val());
                //绑定页面数据
                var imagePaths = [];
                for (var item in res.Data) {
                    imagePaths.push({ filePath: res.Data[item].sImagePath });
                }
                

                //初始化
                baseSettingObj.uploadCarouselImage = new UploadImage({
                    target: "shopSet_index_upload", maxFileCount: 15,
                    imgLst: imagePaths,
                });
            }, function (res) {
                eui.alertInfo(res.Msg);
            })
        }
        //按钮绑定
        function bindEvent() {
            $("#shopset_index_form").on("click", "#shopset_index_btn_add_customer", addEvent)
            .on("click", "[data-id='shopSet_index_customer_delete']", deleteEvent)
            .on("click", "#shopSet_index_btn_save", saveEvent)
        }
        //添加客服
        function addEvent() {
            var html = "<li style='line-height:3'>";
            html = html.concat('<span>客服：</span>')
            html = html.concat('&nbsp;<span>QQ号：<input name="sQQ"></span>')
            html = html.concat('&emsp;&emsp;&emsp;&nbsp;&nbsp;');
            html = html.concat('<span>QQ昵称：<input name="sQQName"></span>')
            html = html.concat('&nbsp;<span><a data-id="shopSet_index_customer_delete" href="#">删除</a></span>');
            html = html.concat("</li>");

            $("#shopSet_index_customer_contenter_div ul").append(html);
            var ul = $("#shopSet_index_customer_contenter_div ul li");
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

            $($(ul[ul.length - 1]).find("a[data-id='shopSet_index_customer_delete']")[0]).linkbutton({
                iconCls: 'icon-remove',
            });
        }


        //删除客服
        function deleteEvent() {
            $(this).parents("li").remove();
        }

        function saveEvent() {
            if ($("#shopset_index_form").form("validate")) {
                
                var json = $("#shopset_index_form").serializeObject();
                if ($("#shop_introduce_Content").val().length > 100) {
                    return eui.alertInfo("店铺介绍不能超过100个字");
                }
              else  if (baseSettingObj.uploadCarouselImage.imageList <= 0) {
                    return eui.alertInfo("请上传至少一张图片");
                }
                else
                {
                    var readaddress = window.parent.modules.get(window.parent.enums.Modules.CACHE);
                    var c = readaddress.getCache(window.parent.enums.VARIABLE.BAIDU_MAP_ADDRESS_CHOOSER);
                   debugger
                   if (c.a != null && c.a != "") {//是否有选择位置
                       json.sProvice = c.s;//省
                       json.sCity = c.c;//市
                       var country = c.q;//区
                       if (country == "请选择县区") {
                           json.sCounty = "";//如果没有选择县区，直接进行搜索
                       }
                       else {
                           json.sCounty = country;
                       }
                       //经纬度
                       json.sLONG = c.lng;
                       json.sLat = c.lat;
                   }
                       //如果没有选择位置则默认为之前的地址
                   else {
                       json.sProvice=$("#shopset_ex_provice").val();
                       json.sCity = $("#shopset_ex_sCity").val();
                       json.sCounty = $("#shopset_ex_sCounty").val();
                       json.sLONG = $("#shopset_ex_sLONG").val();
                       json.sLat = $("#shopset_ex_sLat").val();
                   }
                    
                json.imageList = baseSettingObj.uploadCarouselImage.imageList;
                json.customerCount = $("#shopset_index_form").find("input[name='sQQ']").length;
                    //保存
                    
                json.sIntroduce = $("#shop_introduce_Content").val();
                f.post("/Admin/ShopSet/SetShopMessage", json, function (res) {
                    eui.alertInfo("保存成功");
                }, function (res) {
                    eui.alertInfo(res.Msg);
                });
            }
            }
        }
        try {
            initImages();
            bindEvent();
        }
        catch (e)
        {

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