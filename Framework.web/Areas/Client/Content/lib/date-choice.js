/*2016-10-26 毛宇浩*/
/*时间选择控件，需要的空元素上加date-choice属性即可*/

(function($){

    $.extend({  
        'timeChoice':function(obj){
            var room_con_json = {
                "normal_price": [],//普通价格
                "special_offer": [],//特价
                "no_choice": [],//满房
            };

            var choice_style = "checkbox";
            
            var datetime = "";

            if (obj) {
                room_con_json.normal_price=obj.normal_price;
                room_con_json.special_offer = obj.special_offer;
                room_con_json.no_choice = obj.no_choice;
                choice_style = obj.choice_style;//单选多选的样式
            }

           

            //  是否是连续的时间
            function IsContinueDays(arr_days) {
              //  alert(JSON.stringify(arr_days));
                // //先排序，然后转时间戳
                //var days = arr_days.sort().map((d, i) => {
                //    var  dt = new Date(d)
                //    dt.setDate(dt.getDate() + 4 - i) // 处理为相同日期
                //    // 抹去 时 分 秒 毫秒
                //    dt.setHours(0)
                //    dt.setMinutes(0)
                //    dt.setSeconds(0)
                //    dt.setMilliseconds(0)
                //    return +dt
                //});
                //var ret = true;
                //days.forEach(d => {
                //    if (days[0] !== d) {
                //        ret = false;
                //    }
               //})
                //  alert(arr_days);
                
                for (var k = 0; k < arr_days.length; k++) {
                    arr_days[k] = new Date(arr_days[k]).Format("yyyy/MM/dd");
                }
                var New = [];
                var days = arr_days.sort();
               ////alert(days)
                for (var i = 0; i < days.length; i++) {
                    var dt = new Date(days[i]);
                    dt.setDate(dt.getDate() + 4 - i) // 处理为相同日期
                    // 抹去 时 分 秒 毫秒
                    dt.setHours(0)
                    dt.setMinutes(0)
                    dt.setSeconds(0)
                    dt.setMilliseconds(0);
                    New.push(dt.getTime());
                }
                var ret = true;
                for (var j = 0; j < New.length; j++) {
                    if (New[0] != New[j]) {
                        ret = false;
                    }
                }
            
                for (var k = 0; k < window.timeSelect.length; k++) {
                    window.timeSelect[k] = new Date(arr_days[k]).Format("yyyy/M/d");
                }
                return ret;
            }


            var set_choiceStyle = {
                "start_year": 1949,//设置起始年
                "today_start": "yes",//设置是否以今天为起始时间
                "limit_days": "no",//设置以今天为起始时间的往后限定限定天数，可以为'no'或者具体数字
                "choice_style": "radio",//设置选择日期为单选或者多选，值为'radio'或者'checkbox'
                "date_relativeCon": undefined,//设置日期相关的信息，默认无，默认格式为数组[],每个元素和日期一一对应
                "special_date_relativeCon": undefined,//设置特殊日期的相关信息，默认无，默认数组格式[2016/11/20-500,2016/11/27-350]一种
                "disabled_date": undefined,//单独设置的不能选择的日期，默认无，默认格式为数组例如[2016/11/20,2016/12/1,2017/1/15],[2016-11-20,2016-12-1,2017-1-15]二种格式
                "callback": undefined,//设置点击"确定"按钮后的回调函数，默认无
                "date_choice": function (doms, set_json) {
                    /*点击生成选择时间界面*/
                    var that = this;
                    date_obj = new Date();
                    var the_year = date_obj.getFullYear();
                    var the_month = date_obj.getMonth() + 1;
                    var the_weekDay = date_obj.getDay();
                    var show_time = function (dom, year, month, day) {
                        //text()一般元素模式
                        if (dom.prop("type") == undefined) {
                            //一般元素-单选模式
                            if (set_json.choice_style == "radio") {


                                if (set_json.disabled_date) {
                                    var disabled_date_ary = set_json.disabled_date.slice(0);
                                    var date_no = true;
                                    var start = new Date().valueOf();
                                    while (date_no) {
                                        date_no = false;
                                        var start_date = new Date(start);
                                        dom.text(start_date.getFullYear() + "-" + (start_date.getMonth() + 1) + "-" + start_date.getDate());
                                        var item_ary = disabled_date_ary.slice(0);
                                        while (item_ary.length) {
                                            var item_ary_val = item_ary.pop();
                                            if (item_ary_val == (start_date.getFullYear() + "/" + (start_date.getMonth() + 1) + "/" + start_date.getDate()) || item_ary_val == (start_date.getFullYear() + "-" + (start_date.getMonth() + 1) + "-" + start_date.getDate())) {

                                                if (set_json.today_start && set_json.limit_days > 0) {
                                                    var day_item = new Date();
                                                    day_item = new Date(day_item.getFullYear(), day_item.getMonth(), day_item.getMonth());
                                                    var endDay = new Date(day_item.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000);
                                                    if (endDay - start_date > 0) {

                                                        start += 24 * 60 * 60 * 1000;
                                                        date_no = true;
                                                    }

                                                } else {
                                                    start += 24 * 60 * 60 * 1000;
                                                    date_no = true;

                                                }
                                            }


                                        }
                                    }
                                }


                                //一般元素-多选模式
                            } else if (set_json.choice_style == "checkbox") {
                                if (set_json.disabled_date) {


                                }
                                else {
                                    var next_day = new Date(new Date(year, month - 1, day).valueOf() + 24 * 60 * 60 * 1000);
                                    dom.text(year + "-" + month + "-" + day + "," + next_day.getFullYear() + "-" + (next_day.getMonth() - 0 + 1) + "-" + next_day.getDate());
                                }





                            }

                            //VAL() input 文本输入框模式		
                        } else {

                            //input文本输入框-单选模式
                            if (set_json.choice_style == "radio") {

                                //有设置禁选日期
                                if (set_json.disabled_date) {


                                    var disabled_date_ary = set_json.disabled_date.slice(0);
                                    var date_no = true;
                                    var start = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDay()).valueOf();
                                    while (date_no) {


                                        date_no = false;
                                        var start_date = new Date(start);
                                        dom.val(start_date.getFullYear() + "-" + (start_date.getMonth() + 1) + "-" + start_date.getDate());
                                        var item_ary = disabled_date_ary.slice(0);

                                        while (item_ary.length) {

                                            var item_ary_val = item_ary.pop();
                                            if (item_ary_val == (start_date.getFullYear() + "/" + (start_date.getMonth() + 1) + "/" + start_date.getDate()) || item_ary_val == (start_date.getFullYear() + "-" + (start_date.getMonth() + 1) + "-" + start_date.getDate())) {

                                                if (set_json.today_start && set_json.limit_days > 0) {

                                                    var day_item = new Date();
                                                    day_item = new Date(day_item.getFullYear(), day_item.getMonth(), day_item.getDate());
                                                    var endDay = new Date(day_item.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000);
                                                    if (endDay - start_date > 0) {

                                                        alert(11);
                                                        start += 24 * 60 * 60 * 1000;
                                                        date_no = true;
                                                    } else {
                                                        alert(222);
                                                        dom.val("在限定的天数内没有可选日期");

                                                    }

                                                } else {
                                                    alert(333);
                                                    start += 24 * 60 * 60 * 1000;
                                                    date_no = true;



                                                }
                                            }


                                        }



                                    }


                                }
                                    //没有设置禁选日期
                                else {
                                    dom.val(year + "-" + month + "-" + day);

                                }

                                //input文本输入框-多选模式
                            } else if (set_json.choice_style == "checkbox") {

                                var day_item = new Date();
                                day_item = new Date(day_item.getFullYear(), day_item.getMonth(), day_item.getDate());


                                if (set_json.disabled_date) {
                                    //			 	    				var date_string=new Date().getFullYear()+"-"+(new Date().getMonth()+1)+"-"+new Date().getDate();
                                    var date_string = "";
                                    var disabled_date_ary = set_json.disabled_date.slice(0);
                                    var date_no = true;
                                    var start = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate()).valueOf();

                                    while (date_no) {

                                        date_no = false;
                                        var prev_start = start;
                                        var start_date = new Date(start);
                                        var item_ary = disabled_date_ary.slice(0);
                                        while (item_ary.length) {
                                            var item_ary_val = item_ary.pop();
                                            if (item_ary_val == (start_date.getFullYear() + "/" + (start_date.getMonth() + 1) + "/" + start_date.getDate()) || item_ary_val == (start_date.getFullYear() + "-" + (start_date.getMonth() + 1) + "-" + start_date.getDate())) {

                                                if (set_json.today_start == "yes" && set_json.limit_days > 0) {

                                                    var day_item = new Date();
                                                    day_item = new Date(day_item.getFullYear(), day_item.getMonth(), day_item.getDate());
                                                    var endDay = new Date(day_item.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000);
                                                    if (endDay - start_date > 0) {

                                                        start += 24 * 60 * 60 * 1000;
                                                    } else {
                                                        dom.val("在限定的天数内没有可选日期");
                                                    }

                                                } else if (set_json.today_start == "yes" && set_json.limit_days == "no") {

                                                    start += 24 * 60 * 60 * 1000;


                                                } else {
                                                    start += 24 * 60 * 60 * 1000;
                                                }
                                            }

                                        }
                                        if (start > prev_start) {
                                            date_no = true;
                                        }

                                    };

                                    var date_no = true;
                                    var end = start + 24 * 60 * 60 * 1000;
                                    while (date_no) {

                                        date_no = false;
                                        var prev_end = end;
                                        var second_date = new Date(end);
                                        var item_ary = disabled_date_ary.slice(0);
                                        while (item_ary.length) {
                                            var item_ary_val = item_ary.pop();
                                            if (item_ary_val == (second_date.getFullYear() + "/" + (second_date.getMonth() + 1) + "/" + second_date.getDate()) || item_ary_val == (second_date.getFullYear() + "-" + (second_date.getMonth() + 1) + "-" + second_date.getDate())) {

                                                if (set_json.today_start == "yes" && set_json.limit_days > 0) {

                                                    var day_item = new Date();
                                                    day_item = new Date(day_item.getFullYear(), day_item.getMonth(), day_item.getDate());
                                                    var endDay = new Date(day_item.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000);


                                                    if (endDay - second_date > 0) {

                                                        end += 24 * 60 * 60 * 1000;
                                                    } else {
                                                        dom.val("在限定的天数内没有可选日期");
                                                    }


                                                } else if (set_json.today_start == "yes" && set_json.limit_days == "no") {

                                                    end += 24 * 60 * 60 * 1000;

                                                } else {
                                                    end += 24 * 60 * 60 * 1000;
                                                }
                                            }


                                        }
                                        if (end > prev_end) {
                                            date_no = true;
                                        }



                                    }
                                    //    	            			
                                    dom.val(new Date(start).getFullYear() + "-" + (new Date(start).getMonth() + 1) + "-" + new Date(start).getDate() + "," + new Date(end).getFullYear() + "-" + (new Date(end).getMonth() + 1) + "-" + new Date(end).getDate());



                                } else {

                                    if (set_json.limit_days == "no" || set_json.limit_days > 1) {
                                        var next_day = new Date(new Date(year, month - 1, day).valueOf() + 24 * 60 * 60 * 1000);
                                        dom.val(year + "-" + month + "-" + day + "," + next_day.getFullYear() + "-" + (next_day.getMonth() - 0 + 1) + "-" + next_day.getDate());
                                    } else {
                                        dom.val(year + "-" + month + "-" + day);

                                    }


                                }


                            }

                        };

                    };

                    doms.each(function () {
                        var dom = $(this);
                        show_time(dom, the_year, the_month, date_obj.getDate());
                        dom.click(function () {   
                            if (datetime == "") date_obj = new Date();
                            else date_obj = datetime;
                            the_year = date_obj.getFullYear();
                            the_month = date_obj.getMonth() + 1;
                            if (!$(".date_click").length) {
                                $("body").append('<div class="date_click back-fff"><div class="year_month font0"><select class="year"></select><select class="month"></select></div><div class="week"><span>周日</span><span>周一</span><span>周二</span><span>周三</span><span>周四</span><span>周五</span><span>周六</span></div><div class="days"><ul></ul></div><div class="dothing"><span class="ok">确定</span class="cancel"><em>取消</em></div></div>');
                                /*初始化赋值*/
                                if (set_json == undefined) {
                                    set_json = { "start_year": this.start_year, "today_start": this.today_start, "limit_days": this.limit_days, "choice_style": this.choice_style, "date_relativeCon": undefined, "disabled_date": undefined }

                                } else {
                                    set_json.start_year = (set_json.start_year != undefined) ? set_json.start_year : that.start_year;
                                    set_json.today_start = (set_json.today_start != undefined) ? set_json.today_start : that.today_start;
                                    set_json.limit_days = (set_json.limit_days != undefined) ? set_json.limit_days : that.limit_days;
                                    set_json.choice_style = (set_json.choice_style != undefined) ? set_json.choice_style : that.choice_style;
                                    set_json.date_relativeCon = (set_json.date_relativeCon != undefined) ? set_json.date_relativeCon : that.date_relativeCon;
                                    set_json.special_date_relativeCon = (set_json.special_date_relativeCon != undefined) ? set_json.special_date_relativeCon : that.special_date_relativeCon;
                                    set_json.disabled_date = (set_json.disabled_date != undefined) ? set_json.disabled_date : that.disabled_date;
                                    set_json.callback == (set_json.callback != undefined) ? set_json.disabled_date : that.callback;
                                };

                                /*设置开始的时间年限，可以设置具体某一年，也可以设置限定为当前时间的 年份，以及往后2年*/
                                var setYear = function (start) {
                                    $(".date_click .year_month .year").empty();
                                    var year = start;
                                    var end_year = the_year + 2;
                                    if (set_json.today_start == "yes" && set_json.limit_days >= 0) {

                                        var end_dateVal = new Date(date_obj.valueOf() + (set_json.limit_days - 1) * 24 * 60 * 60 * 1000);
                                        end_year = end_dateVal.getFullYear();

                                    }

                                    var year_t = new Date().getFullYear();
                                    if (start > year_t) year_t = new Date().getFullYear();
                                    else year_t = start;
                                    while (year_t <= end_year) {

                                        $(".date_click .year_month .year").prepend('<option value="' + year_t + '">' + year_t + '</option>');
                                        ++year_t;
                                    }
                                    if (dom.prop("type") == undefined) {
                                        $(".date_click .year_month .year").find("[value=" + dom.text().split("-")[0] + "]").prop("selected", "selected");

                                    } else {
                                       // $(".date_click .year_month .year").find("[value=" + dom.val().split("-")[0] + "]").prop("selected", "selected");
                                        $(".date_click .year_month .year").find("[value=" + start + "]").prop("selected", "selected");
                                    };
                                };
                                /*设置选择月份，可以为全年12月，也可以以当前时间月份未开始的月份至本年12月份*/
                                var setMonth = function (start, end) {
                                    $(".date_click .year_month .month").empty();
                                    var month = start;
                                    end_month = 12;
                                    if (end != undefined) {
                                        var end_month = end;
                                    }
                                    var month_t = 1;
                                    while (month_t <= end_month) {

                                        $(".date_click .year_month .month").append('<option value="' + month_t + '">' + month_t + "月" + '</option>');
                                        ++month_t;
                                    };
                                    if (dom.prop("type") == undefined) {

                                        $(".date_click .year_month .month").find("[value=" + dom.text().split("-")[1] + "]").prop("selected", "selected");
                                    } else {
                                       // $(".date_click .year_month .month").find("[value=" + dom.val().split("-")[1] + "]").prop("selected", "selected");
                                        $(".date_click .year_month .month").find("[value=" + month + "]").prop("selected", "selected");
                                    };

                                };
                                /*设置每个年月具体当月天数*/
                                var setDays = function (year, month, limit) {
                                    var num = 0;
                                    var curr_month = new Date(year, month - 1, 1);
                                    if (month == 12) {
                                        num = 31;
                                    }
                                    else {
                                        var next_month = new Date(year, month, 1);
                                        num = (next_month - curr_month) / (24 * 60 * 60 * 1000);

                                    }
                                    $(".date_click .days ul").empty();
                                    var that_year = $(".date_click .year_month .year").val();
                                    var that_month = $(".date_click .year_month .month").val();
                                    while (num) {
                                        var i = num;
                                        if (set_json.date_relativeCon) {
                                            if (set_json.special_date_relativeCon) {

                                                var relative_con = set_json.special_date_relativeCon.slice(0);
                                                var same = false;
                                                while (relative_con.length) {
                                                    var item_con = relative_con.pop().split("-");
                                                    if ((that_year + "/" + that_month + "/" + num) == item_con[0]) {
                                                        same = true;
                                                        break;
                                                    }
                                                }
                                                if (same) {

                                                    $(".date_click .days ul").prepend('<li class="date"><label><input type="' + set_json.choice_style + '" name="choice_day" value="' + num + '"/><span><i>' + (num--) + '<i><b>&yen;' + item_con[1] + '</b></span></label></li>');
                                                }
                                                else {
                                                    $(".date_click .days ul").prepend('<li class="date"><label><input type="' + set_json.choice_style + '" name="choice_day" value="' + num + '"/><span><i>' + num + '<i><b>&yen;' + (set_json.date_relativeCon[--num] == undefined ? "" : set_json.date_relativeCon[num]) + '</b></span></label></li>');

                                                }

                                            } else {

                                                $(".date_click .days ul").prepend('<li class="date"><label><input type="' + set_json.choice_style + '" name="choice_day" value="' + num + '"/><span><i>' + num + '<i><b>&yen;' + (set_json.date_relativeCon[--num] == undefined ? "" : set_json.date_relativeCon[num]) + '</b></span></label></li>');

                                            }

                                        } else {

                                            $(".date_click .days ul").prepend('<li class="date"><label><input type="' + set_json.choice_style + '" name="choice_day" value="' + num + '"/><span><i>' + (num--) + '<i></span></label></li>');


                                        }
                                        //特殊设置的不可选的日期
                                        if (set_json.disabled_date) {
                                            var no_choiced_days = set_json.disabled_date.slice(0);
                                            while (no_choiced_days.length) {
                                                var item_date = no_choiced_days.pop();
                                                if (item_date == new Date((that_year + "/" + that_month + "/" + i)).Format("yyyy/MM/dd")) {
                                                    $(".date_click .days ul li:first-child").find("input").attr("disabled", "disabled");
                                                    break;
                                                }
                                            }
                                            
                                        }

                                    }
                                    for (var i = 0; i < curr_month.getDay() ; i++) {
                                        $(".date_click .days ul").prepend("<li></li>");
                                    }
                                    var all_days = 42 - $(".date_click .days ul li").length;
                                    for (var n = 0; n < all_days; n++) {
                                        $(".date_click .days ul").append("<li></li>");
                                    }
                                    //如果限定天数有设置，执行下面代码，限定每月可选天数
                                    if (set_json.today_start == "yes" && limit > 0) {
                                        var the_month_days = $(".date_click .days ul li.date").length;
                                        var curr_day = date_obj.getDate();
                                        var start_ind = $(".date_click .days ul li").index($(".date_click .days ul li.date").eq(curr_day - 1));
                                        var end_dateVal = date_obj.valueOf() + (limit - 1) * 24 * 60 * 60 * 1000;
                                        var end_date = new Date(end_dateVal);
                                        var end_month = date_obj.getMonth() + 1;


                                        if (new Date(date_obj.getFullYear(), date_obj.getMonth() + 1) - new Date(year, month) == 0) {

                                            $(".date_click .days ul li:lt(" + start_ind + ")").find("input").attr("disabled", "disabled");

                                        }

                                        if (end_date.getFullYear() == year && (end_date.getMonth() + 1 == month)) {


                                            var end_ind = $(".date_click .days ul li").index($(".date_click .days ul li.date").eq(end_date.getDate() - 1));
                                            $(".date_click .days ul li:gt(" + end_ind + ")").find("input").attr("disabled", "disabled");

                                        }

                                    } else if (set_json.today_start == "yes" && (limit == "no" || limit == 0)) {
                                        var end_dateVal = date_obj.valueOf() + (limit - 1) * 24 * 60 * 60 * 1000;
                                        var end_date = new Date(end_dateVal);
                                        if (datetime == "") datetime = new Date();

                                        if (datetime.getFullYear() == new Date().getFullYear() && (datetime.getMonth() + 1) == new Date().getMonth() + 1) {
                                            var curr_day = new Date().getDate();
                                            var start_ind = $(".date_click .days ul li").index($(".date_click .days ul li.date").eq(curr_day - 1));
                                            $(".date_click .days ul li:lt(" + start_ind + ")").find("input").attr("disabled", "disabled");
                                        }
                                    }

                                };


                                
                                /*设置点击输入框后默认的已选择天数样式*/ //----------------------业务处理--------------------
                                var set_defaultDay = function (year, month) {
                                    // debugger
                                    //var that_year = dom.text().split("-")[0];
                                    //var that_month = dom.text().split("-")[1];
                                    //if (dom.prop("type") == undefined) {
                                    //    var default_val = dom.text()
                                    //} else {
                                    //    var default_val = dom.val()

                                    //}

                                    ////判断是否选择模式为多天多个默认值，设置一一对应日期为选中,还需要判断是否为设置的不能选择的日期，如果是，则往后顺延一天
                                    //if (set_json.choice_style == "radio") {
                                    //  //  $(".date_click .days ul").find("span:contains(" + default_val.split("-")[2] + "):first").siblings("input").prop("checked", true);

                                    //} else {

                                    //    var date_string = $(".date_click .year_month .year").val() + "-" + $(".date_click .year_month .month").val() + "-"
                                    //    $(".date_click .days li input").each(function () {
                                    //        var date = date_string + $(this).val();
                                    //        var ary = default_val.split(",");
                                    //        while (ary.length) {
                                    //            if (ary.pop() == date) {
                                    //                $(this).prop("checked", true);
                                    //                break;
                                    //            }
                                    //        }
                                    //    })
                                    //}

                                    //汤台添加
                                    if (obj.gtype == 1) {
                                        //商品为客房
                                        var selected = window.timeSelect;
                                        $(selected).each(function () {
                                            var time = this.split('/');
                                            if (time[0] == year && time[1] == month) {
                                                //设置选中
                                                $(".date_click .days li input[value=" + time[2] + "]").prop("checked", true);
                                            }
                                        });
                                    }
                                    
                                    
                                
                                };

                                /*初始化绑定点击时间选择事件*/
                                var click_change = function () {
                                    $(".date_click .days li input").click(function () {
                                        if (dom.prop("type") == undefined) {
                                            if (set_json.choice_style == "radio") {
                                                dom.text($(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + $(".date_click .days ul li input:checked").siblings("span").text());
                                            } else if (set_json.choice_style.trim() == "checkbox") {
                                                var date_string = dom.text();
                                                var date_ary = date_string.split(",");
                                                var date_num = $(this).val();

                                                if ($(this).prop("checked")) {

                                                    dom.text((date_string == "" ? "" : date_string + ",") + $(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + date_num);

                                                } else {
                                                    if ($(".date_click .days li input:checked").length == 0) {
                                                        $(this).prop("checked", true);
                                                        return;
                                                    }
                                                    var selected_date = $(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + date_num;
                                                    var new_ary = [];
                                                    while (date_ary.length) {
                                                        var item = date_ary.pop();
                                                        if (item.trim() != selected_date.trim()) {
                                                            new_ary.push(item);

                                                        }
                                                    }
                                                    dom.text(new_ary.reverse().join(","));
                                                }
                                            }
                                        } else {
                                            if (set_json.choice_style.trim() == "radio") {
                                                //为单选的时候
                                                if (obj.gtype == 2) {
                                                    //商品为票务的时候
                                                    var year = $('.year').val();//年份
                                                    var month = $('.month').val();//月份
                                                    var day = $(this).val();
                                                    //添加选中的时间
                                                    window.timeSelect = [];
                                                    window.timeSelect.push('' + year + '/' + month + '/' + day + '');
                                                    var price = parseFloat($(this).next().children('i').children('i').children('b').text().slice(1))//获取选中的价格
                                                    window.Allprices =price;
                                                }
                                                dom.val($(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + $(".date_click .days ul li input:checked").val());
                                            } else if (set_json.choice_style.trim() == "checkbox") {
                                                var date_string = dom.val();
                                                var date_ary = date_string.split(",");

                                                var date_num = $(this).val();

                                                if ($(this).prop("checked"))
                                                {//选中 
                                                    dom.val((date_string == "" ? "" : date_string + ",") + $(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + date_num);

                                                    if (obj.gtype == 1) {//商品是客房
                                                        //选中的业务处理
                                                        var year = $('.year').val();//年份
                                                        var month = $('.month').val();//月份
                                                        var day = $(this).val();
                                                        //添加选中的时间
                                                        window.timeSelect.push('' + year + '/' + month + '/' + day + '')
                                                        var price = parseFloat($(this).next().children('i').children('i').children('b').text().slice(1))//获取选中的价格
                                                        window.Allprices = window.Allprices + price;
                                                        //选中的业务处理
                                                    }
                                                } else {
                                                    //取消选中
                                                    //if ($(".date_click .days li input:checked").length == 0) {
                                                    //    $(this).prop("checked", true);
                                                    //    return;
                                                    //}
                                                    var selected_date = $(".date_click .year_month select:first").val() + "-" + $(".date_click .year_month select:last").val() + "-" + date_num;
                                                    var new_ary = [];
                                                    while (date_ary.length) {
                                                        var item = date_ary.pop();

                                                        if (item.trim() != selected_date.trim()) {
                                                            new_ary.push(item);

                                                        }
                                                    }
                                                    dom.val(new_ary.reverse().join(","));
                                                    if (obj.gtype == 1) {//商品是客房
                                                        //取消选中的业务处理
                                                        var year = $('.year').val();//年份
                                                        var month = $('.month').val();//月份
                                                        var day = $(this).val();
                                                        //删除取消时间    
                                                        var index = window.timeSelect.indexOf('' + year + '/' + month + '/' + day + '');
                                                        window.timeSelect.splice(index, 1);
                                                        var price = parseFloat($(this).next().children('i').children('i').children('b').text().slice(1))//获取选中的价格
                                                        window.Allprices = window.Allprices - price;
                                                        //取消选中的业务处理
                                                    }
                                                }
                                            }
                                        };
                                    });
                                }

                                /*具体年月日生成函数调用*/
                                if (set_json.today_start == "no") {
                                    setYear(set_json.start_year);
                                    setMonth(1, 12);
                                    setDays(the_year, the_month);
                                    set_defaultDay(the_year, the_month);
                                    click_change();
                                } else if (set_json.today_start == "yes") {

                                    setYear(the_year);
                                    if (set_json.limit_days >= 0) {

                                        var end_dateVal = date_obj.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000;
                                        if ((new Date(end_dateVal)).getFullYear() > the_year) {

                                            setMonth(the_month, 12);
                                        }
                                        else {

                                            var end_month = (new Date(end_dateVal)).getMonth() + 1;

                                            setMonth(the_month, end_month);
                                        }

                                    } else {

                                        if ($(".date_click .year_month .year").val() > date_obj.getFullYear()) {
                                            setMonth(1, 12);

                                        } else {
                                            setMonth(the_month, 12);

                                        }
                                    }

                                    setDays(the_year, the_month, set_json.limit_days);
                                    set_defaultDay(the_year, the_month);
                                    click_change();
                                };

                                /*选择年月后，刷新下面的天数*/
                                $(".date_click .year_month select").change(function () {


                                    if (set_json.today_start == "yes" && $(this).hasClass("year") && set_json.limit_days >= 0) {

                                        var end_date = new Date(date_obj.valueOf() + set_json.limit_days * 24 * 60 * 60 * 1000);
                                        if (end_date.getFullYear() == date_obj.getFullYear()) {


                                            setMonth(date_obj.getMonth() + 1, end_date.getMonth() + 1)
                                        }
                                        else {

                                            if ($(this).val() == end_date.getFullYear()) {

                                                setMonth(1, end_date.getMonth() + 1)

                                            } else if ($(this).val() > date_obj.getFullYear()) {

                                                setMonth(1, 12);
                                            } else if ($(this).val() == date_obj.getFullYear()) {

                                                setMonth(date_obj.getMonth() + 1, 12);

                                            }
                                        }

                                    } else if (set_json.today_start == "yes" && $(this).hasClass("year")) {

                                        if ($(this).val() == the_year) {
                                            setMonth(the_month, 12);
                                        } else {
                                            setMonth(1, 12);

                                        }
                                    }

                                    var year = $(".date_click .year_month select:first").val();
                                    var month = $(".date_click .year_month select:last").val();
                                
                                    datetime = new Date(year + "/" + month + "/1");


                                  ///选择年月日刷新数据
                                    var json = {};
                                    var nowtime = "" + year + "/"+ month + "";
                                    common.post("/Client/OrderConfirm/LoadOrderInfo", {
                                        gtype: obj.gtype,
                                        gid: obj.gid,
                                        nowtime: nowtime
                                    }, function (r) {
                                        json.normal_price = r.Data.normal_price;
                                        json.special_offer = r.Data.special_offer;
                                        json.no_choice = r.Data.no_choice;

                                        set_json.date_relativeCon = json.normal_price;//普通价格
                                        set_json.special_date_relativeCon = json.special_offer;//特殊价格
                                        set_json.disabled_date = json.no_choice;//不能选择的日期

                                        setDays(year, month, set_json.limit_days);
                                        set_defaultDay(year, month);
                                        click_change();
                                    
                                    },null,false);

                                   // obj.callback();//回调函数
                                })

                                /*选择日期后，点击"确认"和"取消"的操作*/
                                if (dom.prop("type") == undefined) {
                                    var curr_date = dom.text();

                                } else {
                                    var curr_date = dom.val();

                                };
                                //****************************************时间复杂处理**************************************//
                                //确定取消按钮事件处理
                                $(".date_click .dothing >*").click(function () {
                                    if ($(this).text() == "取消") {
                                        if (dom.prop("type") == undefined) {
                                            dom.text(curr_date);
                                        } else {
                                            dom.val(curr_date);
                                        };
                                        $(".date_click").remove();
                                    }
                                    else
                                    {//确定按钮          
                                        var year = $('.year').val();//年份
                                        var month = $('.month').val();//月份
                                        var checked = $('.date input[name=choice_day]:checked');//选中的日期  
                                        if (obj.gtype == 1)
                                        {//客房
                                            if (window.timeSelect.length == 0) {
                                                alert("请选择入住的时间!", null, 800);
                                                return;
                                            }
                                            else {
                                               
                                                var res = IsContinueDays(window.timeSelect);//判断所选时间是否连续
                                                if (res == false) {
                                                    alert("只能选择连续的时间入住!");
                                                    return;
                                                }
                                                set_json.callback();
                                                setTimeout(function () {
                                                    $(".date_click").remove();
                                                }, 400)
                                            }
                                        }
                                        else
                                        {//票务
                                            if (window.timeSelect.length == 0) {
                                                alert("请选择票务的时间!", null, 800);
                                                return;
                                            }
                                            set_json.callback();
                                            setTimeout(function () {
                                                $(".date_click").remove();
                                            }, 400)
                                        }
                                    }
                                });
                                /*设定图片选择框位置*/
                                var top = dom.offset().top;
                                var date_clickTop = top + dom.height() + 10;
                                $(".date_click").css("top", date_clickTop + "px");
                            }
                        });
                    });
                }
            }

            
           // 价格的展示
            var show_price = function () {
                var selected = window.timeSelect;
                var index = selected.length;
                if (obj.gtype == 1) {//客房
                    if (selected.length == 1) {
                        window.timeData.stime = selected[0] + " 00:00:00";
                        window.timeData.etime = selected[0] + " 23:59:59";
                    }
                    else {
                        window.timeData.stime = selected[0] + " 00:00:00";
                        window.timeData.etime = selected[index - 1] + " 23:59:59";
                    }

                    //var ary = $("[date-choice]").val().split(",");
                    //var new_ary = [];
                    //while (ary.length) {
                    //    var item = ary.pop().split("-");
                    //    new_ary.push(new Date(item[0], item[1], item[2]).valueOf())

                    //}
                    //new_ary.sort();
                    var start = new Date(selected[0]);
                    var end = new Date((new Date(selected[index - 1]).getTime() + 1 * 24 * 60 * 60 * 1000));

                    $(".sureRoom-tittle span i").text(start.getMonth()+1 + "/" + start.getDate())
                    $(".sureRoom-tittle span b").text(end.getMonth()+1 + "/" + end.getDate())
                    $(".sureRoom-tittle span em").text(index);
                    $('#data').show();
                    
                    window.CountAndPrice.price = window.Allprices;//复制
                   // window.Allprices = 0;//清零
                    // window.timeSelect = [];//清选择

                    //判断优惠卷满不满足使用条件
                    var totalprice = window.CountAndPrice.price * window.CountAndPrice.count;//付款的额总价
                    if ((totalprice - window.CountAndPrice.top) >= 0 && totalprice > window.CountAndPrice.coupon) {
                        ////重新计算价格
                        $('#total_price').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));
                        $('#shold_to_pay').text(((window.CountAndPrice.price * window.CountAndPrice.count) - window.CountAndPrice.coupon).toFixed(2));

                    }
                    else {
                        //不满足适用条件 不使用优惠卷
                        $('#total_price').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));
                        $('#shold_to_pay').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));

                        //清空优惠卷信息
                        window.CountAndPrice.coupon=0,//优惠卷价格
                        window.CountAndPrice.top=0,//优惠卷的使用条件
                        window.CountAndPrice.couponid = null//优惠卷ID

                        //清除文本信息
                        $('#coupon_selected i').text('不使用优惠券');
                    }
                }
                else {//票务
                    if (selected.length == 1) {
                        window.timeData.stime = selected[0] + " 00:00:00";
                        window.timeData.etime = selected[0] + " 23:59:59";

                        var start = new Date(selected[0]);
                        var end = new Date((new Date(selected[index - 1]).getTime() + 1 * 24 * 60 * 60 * 1000));
                        $('#data').html("<i>" + (start.getMonth()+1) + "/" + start.getDate() + "</i>")
                        $('#data').show();

                        window.CountAndPrice.price = window.Allprices;//复制

                        //判断优惠卷满不满足使用条件
                        var totalprice = window.CountAndPrice.price * window.CountAndPrice.count;//付款的额总价
                        if ((totalprice - window.CountAndPrice.top) >= 0 && totalprice > window.CountAndPrice.coupon) {
                            ////重新计算价格
                            $('#total_price').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));
                            $('#shold_to_pay').text(((window.CountAndPrice.price * window.CountAndPrice.count) - window.CountAndPrice.coupon).toFixed(2));

                        }
                        else {
                            //不满足适用条件 不使用优惠卷
                            $('#total_price').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));
                            $('#shold_to_pay').text((window.CountAndPrice.price * window.CountAndPrice.count).toFixed(2));

                            //清空优惠卷信息
                            window.CountAndPrice.coupon = 0,//优惠卷价格
                            window.CountAndPrice.top = 0,//优惠卷的使用条件
                            window.CountAndPrice.couponid = null//优惠卷ID

                            //清除文本信息
                            $('#coupon_selected i').text('不使用优惠券');
                        }
                        //window.Allprices = 0;//清零
                        //window.timeSelect = [];
                    }
                    else {
                        alert("票务只能选择一天的", null, 800);
                        return;
                    }
                }
            }

            set_choiceStyle.date_choice($("[date-choice]"), {
                "today_start": "yes",
                "limit_days": "no",
                "choice_style": choice_style,
                "date_relativeCon": room_con_json.normal_price,
                "special_date_relativeCon": room_con_json.special_offer,
                "disabled_date": room_con_json.no_choice,
                "callback": show_price
            });
        }
    });

})(jQuery)


   

