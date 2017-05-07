/*
Ajax 三级省市联动
http://code.ciaoca.cn/
日期：2016-10-18

settings 参数说明
-----
url:省市数据josn文件路径
prov:默认省份
city:默认城市
dist:默认地区（县）
nodata:无数据状态
required:必选项
type:类型(1-PC端(默认)，2-移动端,3-PC前端)
level:联动级数(3-3级联动(默认)，2-二级联动)
ziti:是否是自提(默认false)
------------------------------ */
(function ($) {
    $.fn.citySelect = function (settings) {
        if (this.length < 1) { return; };
        // 默认值
        settings = $.extend({
            url: "/scripts/plug-in/cityselect/city.min.js",
            prov: "四川省",
            city: "成都市",
            dist: "武侯区",
            nodata: "none",
            required: true,
            type: 1,
            level: 3,
            ziti:false
        }, settings);

        var box_obj = this;
        var boxhtml = '';
        if (settings.type == 1) {
            if (settings.level == 3) {
                boxhtml = "<select class=\"prov\" style=\"max-width:80px;\"></select><select class=\"city\" style=\"max-width:120px;margin-left:10px;\" disabled=\"disabled\"></select><select class=\"dist\" style=\"max-width:80px;margin-left:10px;\" disabled=\"disabled\"></select><input class='id' type='hidden'/>";
            } else {
                settings.dist = null;
                boxhtml = "<select class=\"prov\" style=\"max-width:80px;\"></select><select class=\"city\" style=\"max-width:120px;margin-left:10px;\" disabled=\"disabled\"></select><input class='id' type='hidden'/>";
            }
        }
        else if (settings.type == 2) {
            if (settings.level == 3) {
                // boxhtml = '<div class="weui_cell weui_cell_select weui_select_after"><div class="weui_cell_hd">省份/直辖市</div><div class="weui_cell_bd weui_cell_primary"><select class="weui_select prov"></select></div></div><div class="weui_cell weui_cell_select weui_select_after"><div class="weui_cell_hd">城市</div><div class="weui_cell_bd weui_cell_primary"><select class="weui_select city"></select></div></div><div class="weui_cell weui_cell_select weui_select_after"><div class="weui_cell_hd">区域</div><div class="weui_cell_bd weui_cell_primary"><select class="weui_select dist"></select></div></div><input class="id" type="hidden"/>';
                var html = [];
                html.push('<div class="w-height09 w-borderBeee w-paddingTopBottom02 w-position w-flex" style="padding-left:.38rem;">');
                html.push('<h4 class="w-height05 w-color333 w-font14 w-marginRight02">省份</h4>');
                html.push('<select name="" id="select1" class="w-flexitem1 w-height05 w-font14 w-color666 prov" style="-webkit-appearance: none;-moz-appearance: none;appearance: none;background-color:transparent"></select>');
                html.push('<b class="w-iconRight2"></b></div>');
                html.push('<div class="w-height09 w-borderBeee w-paddingTopBottom02 w-position w-flex" style="padding-left:.38rem;">');
                html.push('<h4 class="w-height05 w-color333 w-font14 w-marginRight02">城市</h4>');
                html.push('<select name="" id="select2" class="w-flexitem1 w-height05 w-font14 w-color666 city" style="-webkit-appearance: none;-moz-appearance: none;appearance: none;background-color:transparent"></select>');
                html.push('<b class="w-iconRight2"></b></div>');
                html.push('<div class="w-height09 w-borderBeee w-paddingTopBottom02 w-position w-flex" style="padding-left:.38rem;">');
                html.push('<h4 class="w-height05 w-color333 w-font14 w-marginRight02">区域</h4>');
                html.push('<select name="" id="select3" class="w-flexitem1 w-height05 w-font14 w-color666 dist" style="-webkit-appearance: none;-moz-appearance: none;appearance: none;background-color:transparent"></select>');
                html.push('<b class="w-iconRight2"></b></div>');
                boxhtml = html.join('');
               
            } else {
                settings.dist = null;
                boxhtml = '<div class="weui_cell weui_cell_select weui_select_after"><div class="weui_cell_hd">省份/直辖市</div><div class="weui_cell_bd weui_cell_primary"><select class="weui_select prov"></select></div></div><div class="weui_cell weui_cell_select weui_select_after"><div class="weui_cell_hd">城市</div><div class="weui_cell_bd weui_cell_primary"><select class="weui_select city"></select></div></div><div class="weui_cell weui_cell_select weui_select_after"><input class="id" type="hidden"/>';
            }
        } else {
            if (settings.level == 3) {
                boxhtml = "<select class=\"prov\"></select><select class=\"city\" disabled=\"disabled\"></select><select class=\"dist\" disabled=\"disabled\"></select><input class='id' type='hidden'/>";
            } else {
                settings.dist = null;
                boxhtml = "<select class=\"prov\" ></select><select class=\"city\" disabled=\"disabled\"></select><input class='id' type='hidden'/>";
            }
        }

        $(this).append(boxhtml);
        var prov_obj = box_obj.find(".prov");
        var city_obj = box_obj.find(".city");
        var dist_obj = box_obj.find(".dist");
        var id_obj = box_obj.find(".id");
        var prov_val = settings.prov;
        var city_val = settings.city;
        var dist_val = settings.dist;
        var select_prehtml = (settings.required) ? "" : "<option value=''>请选择</option>";
        var city_json;

        // 赋值市级函数
        var cityStart = function () {
            var prov_id = prov_obj.get(0).selectedIndex;
            if (!settings.required) {
                prov_id--;
            };
            city_obj.empty().attr("disabled", true);
            dist_obj.empty().attr("disabled", true);

            if (prov_id < 0 || city_json.citylist[prov_id].c.length <= 0) {
                
                if (settings.nodata == "none") {
                    if (settings.type == 1 || settings.type == 3) {
                       // city_obj.css("display", "none");
                       // dist_obj.css("display", "none");
                    } else {
                        //city_obj.parent().parent().css("display", "none");
                       // dist_obj.parent().parent().css("display", "none");
                    }
                } else if (settings.nodata == "hidden") {
                    if (settings.type == 1 || settings.type == 3) {
                        city_obj.css("visibility", "hidden");
                        dist_obj.css("visibility", "hidden");
                    }
                    else {
                        city_obj.parent().parent().css("visibility", "hidden");
                        dist_obj.parent().parent().css("visibility", "hidden");
                    }
                };
                return;
            };
           

            // 遍历赋值市级下拉列表
            temp_html = select_prehtml;
            $.each(city_json.citylist[prov_id].c, function (i, city) {
                temp_html += "<option value='" + city.n + "' data-id='" + city.id + "'>" + city.n + "</option>";
                if (i == 0) {
                    id_obj.val(city.id);
                }
            });
            city_obj.html(temp_html).attr("disabled", false);
            if (settings.type == 1 || settings.type == 3) { city_obj.css({ "display": "", "visibility": "" }); }
            else { city_obj.parent().parent().css({ "display": "", "visibility": "" }); }
            distStart();
        };

        // 赋值地区（县）函数
        var distStart = function () {
            if (settings.level == 3) {//三级联动才有
                var prov_id = prov_obj.get(0).selectedIndex;
                var city_id = city_obj.get(0).selectedIndex;
                if (!settings.required) {
                    prov_id--;
                    city_id--;
                };
                dist_obj.empty().attr("disabled", true);
                if (prov_id < 0 || city_id < 0 || city_json.citylist[prov_id].c[city_id].a.length <= 0) {
                    if (settings.nodata == "none") {
                        if (settings.type == 1 || settings.type == 3) {
                            dist_obj.css("display", "none");
                        }
                        else {
                            //dist_obj.parent().parent().css("display", "none");
                        }
                    } else if (settings.nodata == "hidden") {
                        if (settings.type == 1 || settings.type == 3) {
                            dist_obj.css("visibility", "hidden");
                        } else {
                            dist_obj.parent().parent().css("visibility", "hidden");
                        }
                    };
                    return;
                };

                // 遍历赋值市级下拉列表
                temp_html = select_prehtml;
                $.each(city_json.citylist[prov_id].c[city_id].a, function (i, dist) {
                    temp_html += "<option value='" + dist.s + "' data-id='" + dist.id + "'>" + dist.s + "</option>";
                    if (i == 0) {
                        id_obj.val(dist.id);
                    }
                });
                dist_obj.html(temp_html).attr("disabled", false);
                if (settings.type == 1 || settings.type == 3) { dist_obj.css({ "display": "", "visibility": "" }); }
                else { dist_obj.parent().parent().css({ "display": "", "visibility": "" }); }

            }
        };

        var init = function () {
            // 遍历赋值省份下拉列表
            temp_html = select_prehtml;
            $.each(city_json.citylist, function (i, prov) {
                temp_html += "<option value='" + prov.p + "' data-id='" + prov.id + "'>" + prov.p + "</option>";
                if (i == 0) {
                    id_obj.val(prov.id);
                }
            });
            prov_obj.html(temp_html);
            // 若有传入省份与市级的值，则选中。
            if (settings.prov != null) {
                prov_obj.val(settings.prov);
                id_obj.val(prov_obj.find(":selected").attr("data-id"));
                cityStart();
                if (settings.city != null) {
                    city_obj.val(settings.city);
                    id_obj.val(city_obj.find(":selected").attr("data-id"));
                    distStart();
                    if (settings.dist != null) {
                        dist_obj.val(settings.dist);
                        id_obj.val(dist_obj.find(":selected").attr("data-id"));
                    };
                };
            };

            if (settings.ziti)
            {//是自提需要去获取自提地址
                getZitiAddress();
            }


            // 选择省份时发生事件
            prov_obj.bind("change", function () {
                id_obj.val($(this).find(":selected").attr("data-id"));
                cityStart();
                if (settings.ziti) {//是自提需要去获取自提地址
                    getZitiAddress();
                }
            });

            // 选择市级时发生事件
            city_obj.bind("change", function () {
                id_obj.val($(this).find(":selected").attr("data-id"));
                distStart();
                if (settings.ziti) {//是自提需要去获取自提地址
                    getZitiAddress();
                }
            });

            // 选择区级时发生事件
            dist_obj.bind("change", function () {
                id_obj.val($(this).find(":selected").attr("data-id"));
            });
        };

        // 设置省市json数据
        if (typeof (settings.url) == "string") {
            $.ajax({
                url: settings.url,
                type: 'get',
                dataType: 'json',
                success: function (json) {
                    city_json = json;
                    init();
                },
                error: function () {

                }
            });
        } else {
            city_json = settings.url;
            init();
        };
    };
})(jQuery);