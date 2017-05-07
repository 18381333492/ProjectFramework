//后退操作
//监听物理返回按钮    
setTimeout(function () {
    window.addEventListener("popstate", function (event) {
        if (gp && gp.back) {
            gp.back();
            pushHistory();
        }
        return false;
    });
    //alert(JSON.stringify(window.history.length));
    pushHistory();
}, 100);
function pushHistory() {
    window.history.pushState("state", "", "");
}

(function ($) {
    $(function () {
        $.initCache();
        $.getRequest();
        $.reMethod();
        $.InitOnLoad();
    });

    window.gp = {
    }; //全局对象
    window.gp.cache = {};   //本地缓存
    //访问历史缓存地址
    window.gp.history = new Array();
    //是否启用localstorage,true=localstorage,false=cookie
    window.gp.cache.isLocalStorage = true;

    $.initCache = function () {
        //设置本地缓存
        gp.cache.set = function (name, value) {
            if (typeof value != "string") {
                value = JSON.stringify(value);
            }
            if (gp.cache.isLocalStorage) {
                localStorage[name] = value;
            } else {
                $.cookie(name, value, { expires: 30, path: "/" });
            }
        };
        //读取本地缓存
        gp.cache.get = function (name) {
            if (gp.cache.isLocalStorage) {
                return localStorage[name];
            } else {
                return $.cookie(name);
            }
        };
        //清除本地指定缓存
        gp.cache.remove = function (name) {
            if (gp.cache.isLocalStorage) {
                localStorage.removeItem(name);
            } else {
                $.cookie(name, "", { expires: -1, path: "/" });
            }
        };
    };

    $.reMethod = function () //重写部分函数
    {
        //移除历史中的某页
        //pathname不输入或者输入null，则代表删除当前页
        //isOnce，默认为false，如果为true则代表删除所有页面名称相同的历史页面，false则只删除最近的相同页面
        Array.prototype.remove = function (pathname, isOnce) {
            if (!pathname) {
                pathname = document.location.pathname.toLowerCase();
            }
            var wHistory = gp.history;
            for (var j = wHistory.length - 1; j >= 0; j--) {
                var page = wHistory[j];
                if (page.pathname.toLowerCase() == pathname.toLowerCase()) {
                    wHistory.splice(j, 1);
                    if (isOnce) {
                        break;
                    }
                }
            }
            gp.cache.set("wHistory", wHistory);
        };

        //清除除了自己页面的所有历史记录，仅保留自己页面
        Array.prototype.clear = function () {
            var wHistory = gp.history;
            if (wHistory.length > 0) {
                wHistory.splice(0, wHistory.length - 1);
            }
            gp.history = wHistory;
            gp.cache.set("wHistory", wHistory);
        };

        //添加页面进入历史记录
        Array.prototype.add = function (pathname) {
            var wHistory = null;
            if (gp.cache.get("wHistory")) {
                wHistory = JSON.parse(gp.cache.get("wHistory"));
            } else {
                wHistory = [];
            }
            if (wHistory.length >= 15) {
                var wNewArray = [];
                for (var i = 0; i < 5; i++) {
                    wNewArray.push(wHistory.pop());
                }
                wHistory = wNewArray;
            }

            //如果是不存页面名称，则取当前页面名称，否则取传递过来的页面名称
            if (!pathname) {
                //相同页面则跳过
                if (wHistory.length > 0) {
                    if (wHistory[wHistory.length - 1].href.toLowerCase() == window.location.href.toLowerCase()) {
                        return;
                    }
                }
                wHistory.push(JSON.parse(JSON.stringify({
                    href: window.location.href,
                    request: window.location.request,
                    pathname: window.location.pathname.toLowerCase()
                })));
            } else {
                wHistory.push({
                    href: pathname,
                    request: pathname,
                    pagename: pathname
                });
            }
            gp.cache.set("wHistory", wHistory);
            gp.history = wHistory;
        };
        gp.gotoPage = function (url, aniId, type) {
            $.stopEventBubble();

            //alert(url);
            location.href = url;

        };
        /**
         * 返回
         * @param {isSameBack} isSameBack true:允许跳过相同页面并且参数不同
         * @param {urlParms} urlParms:url附加参数,主要APP使用
         */
        gp.back = function (isSameBack, urlParms) {
            $.stopEventBubble();
            //gp.browser.version.weixin
            if (true) {
                if (gp.history) {
                    while (gp.history.length > 0) {
                        var prew = gp.history.pop();
                        if (prew && prew.href.indexOf("SystemError") < 0) {
                            if (isSameBack == true && location.pathname.toLowerCase() == prew.pathname.toLowerCase()) {
                                continue;
                            }
                            if (location.href != prew.href || location.request != prew.request) {
                                gp.cache.set("wHistory", gp.history);
                                gp.gotoPage(prew.href, 1);
                                return;
                            }
                        }
                    }
                }
            } else {
                if (urlParms) {
                    gp.gotoPage("back.html" + urlParms);
                } else {
                    gp.gotoPage("back.html");
                }
            }
        };
    };

    //URL参数
    $.getRequest = function () {
        if (gp.cache.get("wHistory")) {
            window.gp.history = JSON.parse(gp.cache.get("wHistory"));
            //记录下参数
            window.location.request = gp.cache.get("request");
        }
    };

    //禁止冒泡
    $.stopEventBubble = function (event) {
        var e = event || window.event;

        if (e && e.stopPropagation) {
            e.stopPropagation();
        }
        else {
            if (e && e.cancelBubble) {
                e.cancelBubble = true;
            }
        }
    }

    //初始化加载事件
    $.InitOnLoad = function () {
        //页面打开事件
        var pageshow = function () {
            gp.history.add();
        };
        pageshow();
    };
})(jQuery);


/// <reference path="jquery.cookie.js" />

/**
 * dropload插件
 * @param {type} o
 * @returns {type} 
 */
!function (o) {
    "use strict"; function t(o) { o.touches || (o.touches = o.originalEvent.touches) } function s(o, t) { t._startY = o.touches[0].pageY, t.opts.scrollArea == window ? (t._meHeight = p.height(), t._childrenHeight = l.height()) : (t._meHeight = t.$element.height(), t._childrenHeight = t.$element.children().height()), t._scrollTop = t.$scrollArea.scrollTop() } function e(t, s) { s._curY = t.touches[0].pageY, s._moveY = s._curY - s._startY, s._moveY > 0 ? s.direction = "down" : s._moveY < 0 && (s.direction = "up"); var e = Math.abs(s._moveY); "" != s.opts.loadUpFn && s._scrollTop <= 0 && "down" == s.direction && (t.preventDefault(), s.insertDOM || (s.$element.prepend('<div class="' + s.opts.domUp.domClass + '"></div>'), s.insertDOM = !0), s.$domUp = o("." + s.opts.domUp.domClass), i(s.$domUp, 0), e <= s.opts.distance ? (s._offsetY = e, s.$domUp.html("").append(s.opts.domUp.domRefresh)) : e > s.opts.distance && e <= 2 * s.opts.distance ? (s._offsetY = s.opts.distance + .5 * (e - s.opts.distance), s.$domUp.html("").append(s.opts.domUp.domUpdate)) : s._offsetY = s.opts.distance + .5 * s.opts.distance + .2 * (e - 2 * s.opts.distance), s.$domUp.css({ height: s._offsetY })), "" != s.opts.loadDownFn && s._childrenHeight <= s._meHeight + s._scrollTop && "up" == s.direction && (t.preventDefault(), s.insertDOM || (s.$element.find('.' + s.opts.domDown.domClass).length === 0 ? s.$element.append('<div class="' + s.opts.domDown.domClass + '"></div>') : '', s.insertDOM = !0), s.$domDown = o("." + s.opts.domDown.domClass), i(s.$domDown, 0), e <= s.opts.distance ? (s._offsetY = e, s.$domDown.html("").append(s.opts.domDown.domRefresh)) : e > s.opts.distance && e <= 2 * s.opts.distance ? (s._offsetY = s.opts.distance + .5 * (e - s.opts.distance), s.$domDown.html("").append(s.opts.domDown.domUpdate)) : s._offsetY = s.opts.distance + .5 * s.opts.distance + .2 * (e - 2 * s.opts.distance), s.$domDown.css({ height: s._offsetY }), s.$scrollArea.scrollTop(s._offsetY + s._scrollTop)) } function d(t) { var s = Math.abs(t._moveY); t.insertDOM && ("down" == t.direction ? (t.$domResult = t.$domUp, t.domLoad = t.opts.domUp.domLoad) : "up" == t.direction && (t.$domResult = t.$domDown, t.domLoad = t.opts.domDown.domLoad), i(t.$domResult, 300), s > t.opts.distance ? (t.$domResult.css({ height: t.$domResult.children().height() }), t.$domResult.html("").append(t.domLoad), n(t)) : t.$domResult.css({ height: "0" }).on("webkitTransitionEnd", function () { t.insertDOM = !1, o(this).remove() }), t._moveY = 0) } function n(o) { o.loading = !0, "" != o.opts.loadUpFn && "down" == o.direction ? o.opts.loadUpFn(o) : "" != o.opts.loadDownFn && "up" == o.direction && o.opts.loadDownFn(o) } function i(o, t) { o.css({ "-webkit-transition": "all " + t + "ms", transition: "all " + t + "ms" }) } var a, p = o(window), l = o(document); o.fn.dropload = function (o) { return new a(this, o) }, a = function (t, s) { var e = this; e.$element = o(t), e.insertDOM = !1, e.loading = !1, e.isLock = !1, e.init(s) }, a.prototype.init = function (n) { var i = this; i.opts = o.extend({}, { scrollArea: i.$element, domUp: { domClass: "dropload-up", domRefresh: '<div class="dropload-refresh">↓下拉刷新</div>', domUpdate: '<div class="dropload-update">↑释放更新</div>', domLoad: '<div class="dropload-load"><span class="loading"></span>加载中...</div>' }, domDown: { domClass: "dropload-down", domRefresh: '<div class="dropload-refresh">↑上拉加载更多</div>', domUpdate: '<div class="dropload-update">↓释放加载</div>', domLoad: '<div class="dropload-load"><span class="loading"></span>加载中...</div>' }, distance: 50, loadUpFn: "", loadDownFn: "" }, n), i.$scrollArea = i.opts.scrollArea == window ? p : i.opts.scrollArea, i.$element.on("touchstart", function (o) { i.loading || i.isLock || (t(o), s(o, i)) }), i.$element.on("touchmove", function (o) { i.loading || i.isLock || (t(o, i), e(o, i)) }), i.$element.on("touchend", function () { i.loading || i.isLock || d(i) }) }, a.prototype.lock = function () { var o = this; o.isLock = !0 }, a.prototype.unlock = function () { var o = this; o.isLock = !1 }, a.prototype.resetload = function () { var t = this; t.$domResult && t.$domResult.css({ height: "0" }).on("webkitTransitionEnd", function () { t.loading = !1, t.insertDOM = !1, o(this).remove() }) }
}(window.Zepto || window.jQuery);

/*!
 * jQuery Cookie Plugin v1.4.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */
(function (n) {
    typeof define == "function" && define.amd ? define(["jquery"], n) : typeof exports == "object" ? n(require("jquery")) : n(jQuery)
})(function (n) {
    function i(n) {
        return t.raw ? n : encodeURIComponent(n)
    } function f(n) {
        return t.raw ? n : decodeURIComponent(n)
    } function e(n) {
        return i(t.json ? JSON.stringify(n) : String(n))
    } function o(n) {
        n.indexOf('"') === 0 && (n = n.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, "\\")); try { return n = decodeURIComponent(n.replace(u, " ")), t.json ? JSON.parse(n) : n } catch (i) { }
    } function r(i, r) { var u = t.raw ? i : o(i); return n.isFunction(r) ? r(u) : u } var u = /\+/g, t = n.cookie = function (u, o, s) {
        var y, a, h, v, c, p; if (o !== undefined && !n.isFunction(o)) return s = n.extend({}, t.defaults, s), typeof s.expires == "number" && (y = s.expires, a = s.expires = new Date, a.setTime(+a + y * 864e5)), document.cookie = [i(u), "=", e(o), s.expires ? "; expires=" + s.expires.toUTCString() : "", s.path ? "; path=" + s.path : "", s.domain ? "; domain=" + s.domain : "", s.secure ? "; secure" : ""].join(""); for (h = u ? undefined : {
        }, v = document.cookie ? document.cookie.split("; ") : [], c = 0, p = v.length; c < p; c++) { var w = v[c].split("="), b = f(w.shift()), l = w.join("="); if (u && u === b) { h = r(l, o); break } u || (l = r(l)) === undefined || (h[b] = l) } return h
    }; t.defaults = {
    }; n.removeCookie = function (t, i) {
        return n.cookie(t) === undefined ? !1 : (n.cookie(t, "", n.extend({}, i, { expires: -1 })), !n.cookie(t))
    }
});

/**
 * 创建全局公用对象
 */
(function () {
    //全局对象
    window.common = {
    };
    //通过url请求的参数
    window.common.requestParam = null;
    //全局配置对象
    window.config = {
        //localstore记录的history的键名字
        historyLocalStoreName: "_HISTORY_C4_M16",
        //ajax post 超时时间
        postTimeOut: 10000,
        //ajax post 的遮罩层对象
        maskmsg: null,
        //ajax提交的上下文对象
        ajaxPostContext: null,
        //alert的弹出框divHTML
        alertDivHTMLString: "<div class='alert_box' style='position:fixed;z-index:1500;width:100%;opacity:1;height:100%;left:0;top:0px;background-color:rgba(68,68,68,0.4);transition:all 0.3s linear'><div class='content' style='width:260px;text-align:center;position:absolute;font-size:14px;left:50%;top:50%; transform:translateY(-50%) translateX(-50%);-webkit-transform:translateY(-50%) translateX(-50%);background-color:#fff;border-radius:10px'><P style='padding:20px 15px; line-height:25px; color:#333;font-size:16px'>{0}<P><div><div>"
    };

    /**
     * 一些扩展的操作
     */
    (function initExtend() {

        /**
        * 
        * 格式化字符串
        * 
        * @method format     
        * @author [杨瑜堃]
        * @version 1.0.1
        * @param {Stringp[]} args 要格式化的替换值数组
        * @returns {String} 格式化结果 
        * 
        * @example
        * 
        * "这个就是格式化字符串的列子:{0}".format("列子")
        */
        String.prototype.format = function (args) {
            var result = this;
            if (arguments.length > 0) {
                if (arguments.length == 1 && typeof (args) == "object") {
                    for (var key in args) {
                        if (args[key] != undefined) {
                            var reg = new RegExp("({)" + key + "(})", "g");
                            result = result.replace(reg, args[key]);
                        }
                    }
                }
                else {
                    for (var i = 0; i < arguments.length; i++) {
                        if (arguments[i] != undefined) {
                            var reg = new RegExp("({)" + i + "(})", "g");
                            result = result.replace(reg, arguments[i]);
                        }
                    }
                }
            }
            return result;
        };

        /**
        * 
        * 对Date的扩展，将 Date 转化为指定格式的String
        * 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
        * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
        * 
        * @for Date
        * @author [杨瑜堃]
        * @version 1.0.1
        * @param {String} fmt 格式化字符串
        * @returns {String} 结果 
        * 
        * @example
        *  
        * (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
        * (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18 
        */
        Date.prototype.Format = function (fmt) {
            var o = {
                "M+": this.getMonth() + 1, //月份 
                "d+": this.getDate(), //日 
                "h+": this.getHours(), //小时 
                "m+": this.getMinutes(), //分 
                "s+": this.getSeconds(), //秒 
                "q+": Math.floor((this.getMonth() + 3) / 3), //季度 
                "S": this.getMilliseconds() //毫秒 
            };
            for (var time in o) {
                if (isNaN(o[time])) {
                    return "";
                }
            }
            if (/(y+)/.test(fmt))
                fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            for (var k in o)
                if (new RegExp("(" + k + ")").test(fmt))
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
            return fmt;
        };

        /* 
        *  删除数组元素:Array.removeArr(index) 
        */
        Array.prototype.removeAt = function (index) {
            if (isNaN(index) || index >= this.length) {
                return false;
            }
            this.splice(index, 1);
        };

        /* 
        *  插入数组元素:Array.insertArr(dx) 
        */
        Array.prototype.insertAt = function (index, item) {
            this.splice(index, 0, item);
        };

        /**
        * 弹出消息框
        * @param {String} msg 消息
        * @param {Function} fn 可选：显示后回调的函数
        * @param {Number} duration 显示的时间
        */
        window.alert = function AlertWindow(msg, fn, duration) {

            duration = isNaN(duration) ? 2000 : duration;

            if ($(".alert_box").length) {
                $(".alert_box .content p:first").text(msg).parents(".alert_box").css({
                    "opacity": "1", "width": "100%", "height": "100%"
                });
            }
            else {
                var win = $(window.config.alertDivHTMLString.format(msg));
                $("body").append(win);
                $("body").css({ 'position': 'fixed', 'width': '100%', 'height': '100%', 'top': '0', 'left': '0' });
            }
            setTimeout(function () {
                if ($(".alert_box").length > 0) {
                    var d = 0.7;
                    $(".alert_box")[0].style.webkitTransition = '-webkit-transform ' + d + 's ease-in, opacity ' + d + 's ease-in';
                    $(".alert_box")[0].style.opacity = '0';
                    if (fn) fn();
                    setTimeout(function () {
                        $(".alert_box").remove();
                        $("body").css({ 'position': 'static', 'width': 'auto', 'height': 'auto', 'top': '0', 'left': '0' });
                    }, d * 1000);
                }
            }, duration);
        };

        /**
         * 带确认取消按钮的确认框
         * @param {String} msg 提示的信息
         * @param {Object} okOptopn 确认按钮配置，格式如下：{"text":"确定按钮",fn:function(){ //这里是按了确定后触发的事件 }}
         * @param {Object} canclOption 取消按钮配置，格式如下：{"text":"取消按钮",fn:function(){ //这里是按了取消后触发的事件 }}
         * @param {Function} callback 可选：按了确定后，调了确定方法后的回调事件
         */
        window.confirm = function (msg, okOptopn, canclOption, callback) {
            //创建背景DIV
            var divBackground = document.createElement("div");

            //设置背景DIV样式
            divBackground.style.cssText = "position:fixed;top: 0;left:0;width: 100%;height: 100%;background: rgba(0,0,0,0.5);z-index:9999999999;";

            //创建内容DIV
            var divContent = document.createElement("div");

            //设置DIV样式
            divContent.style.cssText = "width: 72%;background: #ffffff;border-radius: 8px;margin:70% auto;position: relative;text-align: center;overflow:hidden;font-size:14px;";

            //创建消息SPAN
            var span = document.createElement("span");

            //设置消息提示内容
            span.innerHTML = msg;

            //设置消息span样式
            span.style.cssText = "margin: 20px;display:block;";

            //创建下方两个按钮的ul容器
            var ul = document.createElement("ul");

            //设置ulstyle
            ul.style.cssText = "margin-top:10px;border-top: 1px solid #dcdcdc;";

            //#region 取消按钮（默认）
            var li1 = document.createElement("li");
            //设置按钮样式（默认）
            li1.style.cssText = "text-align: center;line-height: 40px;float:left;width:50%;border-right: 1px solid gainsboro;margin-left:-1px;font-size:12px;color:red;";
            li1.innerHTML = "取消";
            //检查是否自定义取消按钮文本
            if (canclOption && canclOption.text) {
                li1.innerHTML = canclOption.text;
            }
            //绑定取消按钮事件
            li1.onclick = function () {
                if (canclOption.fn) canclOption.fn();
                document.body.removeChild(divBackground);
            };
            //#endregion

            //#region 确认按钮（默认）
            var li2 = document.createElement("li");
            //设置按钮样式（默认）
            li2.style.cssText = "text-align: center;line-height: 40px;float:left;width:50%;font-size:12px;color:red;";
            li2.innerHTML = "确定";
            //检查是否自定义确认按钮文本
            if (okOptopn && okOptopn.text) {
                li2.innerHTML = okOptopn.text;
            }
            //绑定确认按钮事件
            li2.onclick = function () {
                if (okOptopn.fn) okOptopn.fn();
                if (callback) callback();
                document.body.removeChild(divBackground);
            };
            //#endregion

            ul.appendChild(li1);
            ul.appendChild(li2);
            divContent.appendChild(span);
            divContent.appendChild(ul);
            divBackground.appendChild(divContent);
            document.body.appendChild(divBackground);
        };
    })();

    //提供浏览历史的相关方法
    window.common.history = new function () {
        //是否支持LocalStorage
        var isSupportLocalStorage = window.localStorage != undefined && window.localStorage != null;

        //指定系统键的返回地址（主要针对的是安卓的返回键）
        var backUrl = null;

        //是否指定backUrl
        var isBackUrl = false;

        /**
         * 获取上一个浏览记录
         * @returns {String} 浏览记录 
         */
        function getPrev() {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                if (_history.length > 1) {
                    return _history[_history.length - 2];
                } else {
                    return null;
                }
            }
        }

        /**
         * 获取第一个浏览记录
         * @returns {String} 浏览记录 
         */
        function getFirst() {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                if (_history.length > 0) {
                    return _history[0];
                } else {
                    return null;
                }
            }
        }

        /**
         * 获取最后一次访问记录
         * @returns {type} 
         */
        function getLast() {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                if (_history.length > 0) {
                    return _history[_history.length - 1];
                } else {
                    return null;
                }
            }
        }

        /**
         * 移除最后一次浏览记录
         */
        function removeLast() {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                if (_history.length > 0) {
                    _history.pop();
                    resetLocalStorage(_history);
                }
            }
        }

        /**
         * 更新历史记录的值
         * @param {Array<String>} obj
         */
        function resetLocalStorage(obj) {
            if (isSupportLocalStorage) {
                window.localStorage.setItem(window.config.historyLocalStoreName, JSON.stringify(obj));
            }
        }

        /**
         * 获取历史记录的值
         * @returns {Array<String>} 
         */
        function getLocalStorage() {
            if (isSupportLocalStorage) {
                var items = window.localStorage.getItem(window.config.historyLocalStoreName);
                if (items) {
                    return JSON.parse(items);
                } else {
                    return new Array();
                }
            }
        }

        /**
         * 返回上一次浏览记录
         */
        function back() {
            if (isSupportLocalStorage) {
                var url/*上次浏览*/ = getPrev();
                if (url != null && window.location.href.indexOf("/Client/MerchantLogin/Index") <= -1) {
                    removeLast();//移除最后浏览（当前页）                    
                    window.location.href = url;
                }
            }
        }

        /**
         * 获取当前的URL的下标
         * @param {String} url 当前的地址
         * @returns {Number} 下标
         */
        function getUrlIndex(url) {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                if (_history.length > 0) {
                    for (var i = 0; i < _history.length; i++) {
                        if (_history[i] === url) {
                            return i;
                        }
                    }
                } else {
                    return -1;
                }
            } else {
                return alert("浏览器版本太老，请升级浏览器");
            }
        }

        /**
         * 移除指定url和url之后的历史记录
         * @param {String} url 指定的url         
         */
        function removeSpecifyUrl(url) {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                var index = getUrlIndex(url);
                var newHistory = [];
                for (var i = 0; i < index; i++) {
                    newHistory.push(_history[i]);
                }
                resetLocalStorage(newHistory);
            } else {
                return alert("浏览器版本太老，请升级浏览器");
            }
        }

        /**
         * 记录一条浏览信息
         * @param {String} url 网址
         */
        function pushHistory(url) {
            if (isSupportLocalStorage) {
                var _history = getLocalStorage();
                var newUrl = window.location.pathname;//商城首页
                if (newUrl === "/Client/ClientHome/Index" || url === "/Client/MerchantLogin?clear=true") {//第一次进入清除浏览记录
                    _history = [];//清空记录
                }
                if (_history.length > 0) {
                    for (var i = _history.length - 1; i >= 0; i--) {
                        if (_history[i] === url || _history[i] === window.location.pathname) {
                            _history.removeAt(i);
                            i--;
                        }
                    }
                    if (window.location.pathname === "/Client/ClientHome") url = "/Client/ClientHome";
                    _history.push(url);
                }
                else {
                    if (newUrl === "/Client/ClientHome/Index") {//清除首页的Index后缀
                        url = "/Client/ClientHome";
                    }
                    if (url === "/Client/MerchantLogin?clear=true") {//商家入口
                        url = window.location.pathname;//清除参数
                    }
                    _history.push(url);
                }
                resetLocalStorage(_history);
            } else {
                return alert("浏览器版本太老，请升级浏览器");
            }
        }

        /**
         * 设置物理返回键（针对安卓的返回键）的地址
         * @method setBackURL    
         * @author [杨瑜堃]
         * @version 1.0.1
         * @param {String} url
         */
        function setBackURL(url) {
            backUrl = url;
            isBackUrl = true;
            if (history && history.pushState) {
                window.history.pushState(null, null, null);
                window.onpopstate = function doState(e) {

                    //将制定路径之后的路径全部删除
                    removeSpecifyUrl(backUrl);

                    //跳转到指定路径
                    window.location.href = backUrl;
                };
            } else {
                return alert("浏览器版本太老，请升级浏览器");
            }
        }

        /**
         * 记录历史记录，并绑定PushState指定物理键的返回
         * @param {type} url
         */
        function pushHistoryAsPushState(url) {
            // 如果没有指定过后退的地址，就绑定pushstate
            if (isBackUrl === false) {
                pushHistory(url);
                if (history && history.pushState) {
                    firstUrlState();
                } else {
                    return alert("浏览器版本太老，请升级浏览器");
                }
            }
        }

        function firstUrlState() {
            window.history.pushState(null, null, null);
            window.onpopstate = function doState(e) {
                // 物理键返回上一页
                var backUrl = getPrev();
                if (backUrl !== null) {
                    back();
                } else {
                    wx.closeWindow();
                }
            };
        }

        return {
            back: back,
            pushHistory: pushHistory,
            setBackURL: setBackURL,
            getAllHistory: getLocalStorage,
            getPrev: getPrev,
            pushHistoryAsPushState: pushHistoryAsPushState
        };
    };

    //$(function onDocumentReady() {
    //    // 改为pushstate
    //    //window.common.history.pushHistory(window.location.pathname);
    //    window.common.history.pushHistoryAsPushState(window.location.pathname + window.location.search);
    //});

    //post内部使用的一些方法
    window.$_c4 = {
        /**
        * 
        * 对参数进行URL编码
        * 
        * @method encodeParam    
        * @author [杨瑜堃]
        * @version 1.0.1
        * @param {json} jsonObj 要编码的对象
        * @returns {string} 编码结果
        */
        encodeParam: function (jsonObj) {
            if (!jsonObj) return jsonObj;
            if (jsonObj instanceof String) {
                return encodeURIComponent(jsonObj);
            } else {
                return encodeURIComponent(JSON.stringify(jsonObj));
            }
        },

        /**
         * 
         * 显示遮罩层
         * 
         * @method show    
         * @author [杨瑜堃]
         * @version 1.0.1
         * @param [String] msg 可选：要显示的遮罩层文本
         */
        show: function (msg) {
            if (window.config.maskmsg === null) {
                var h = $(document).height();
                window.config.maskmsg = $('<div style="height:100%;width:100%;position:fixed;z-index:99999;background: rgba(255,255,255,0.8);left:0px;top:0px;">' +
                                '<div style="position:absolute;overflow:hidden;left:50%;top:50%;margin-left:-34px;margin-top:-34px;height:68px;width:68px;text-align:center;">' +
                                    '<span class="circles-loader"></span>' +
                                '</div>' +
                            '</div>');
                if (msg) {
                    window.config.maskmsg.find("div").text(msg);
                }
                window.config.maskmsg.appendTo("body");
                setTimeout(
            (function (maskmsg) {
                return function () {
                    maskmsg.remove();
                };
            })(window.config.maskmsg), window.config.postTimeOut);
            }
        },

        /**
         * 
         * 隐藏遮罩层
         * 
         * @method hide        
         * @author [杨瑜堃]
         * @version 1.0.1
         */
        hide: function () {
            if (window.config.maskmsg !== null) {
                window.config.maskmsg.remove();
                window.config.maskmsg = null;
            }
        }
    };

    /**
     * 序列化容器内的带name属性的输入框的值为json object
     * @method serializeObject
     * @for ObjectDomain
     * @author [王其]
     * @version 1.0.1
     * @returns {Object}  
     */
    $.fn.serializeObject = function () {
        var o = {
        };
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    /**
     * ajax post 
     * @param {String} url 提交的地址
     * @param {Json} data 提交的数据
     * @param {Function} onSuccess 成功时的回调函数，有个参数是回调的data
     * @param {Function} unSucess 失败时的回调函数，有个参数是回调的data
     * @param {Boolean} modal 是否启用遮罩层 默认开启
     * @param {Boolean} async 是否是异步
     * @param {Function} onError 错误时的回调函数
     * @param {Function} onComplete 完成时的回调函数
     * @param {String} dataType 数据类型
     */
    window.common.post = function post(url, data, onSuccess, unSucess, modal, async, onError, onComplete, dataType) {

        //判断是否提交期间开启遮罩层
        modal = (modal == false ? false : true);

        //如果要开启遮罩层就显示遮罩层（遮罩层的样式在项目根目录下的Conten/home.css中）
        if (modal) window.$_c4.show();

        //封装jsondata，对参数进行url编码
        var jsonData = window.$_c4.encodeParam({ data: data });

        var ajaxHandler = $.ajax.call(window.config.ajaxPostContext, {
            type: "post",
            url: url,
            cache: false,
            contentType: "application/x-www-form-urlencoded",
            dataType: (dataType ? dataType : "text"),
            data: jsonData,
            async: (async == false ? async : true),
            success: function (json) {
                try {
                    //调用成功，服务端返回结果

                    //如果显示了遮罩层就去掉
                    if (modal) {
                        window.$_c4.hide();
                    }

                    //由于返回的是text，这里解析成对象
                    var result = JSON.parse(json || null);

                    //将Data解析为对象
                    try {
                        result.Data = JSON.parse(result.Data || null);
                    } catch (e) {
                    }

                    //返回结果成功就调用成功的回调函数
                    if (result.Succeeded) {
                        onSuccess(result);
                    }

                    //返回失败且要自己处理错误就调用服务端通知失败的回调函数
                    if (!result.Succeeded && unSucess) {
                        unSucess(result);
                    }

                } catch (e) {
                    alert(e.message);
                }
            },
            error: onError ? onError : function () {
                ajaxHandler.abort();
                if (modal) {
                    window.$_c4.hide();
                }
                window.location.href = "/Status/ToServiceError";
            },
            //请求完成后最终执行参数
            complete: function (XMLHttpRequest, status) {
                if (status === 'timeout') {
                    //超时,status还有success,error等值的情况
                    alert("访问超时");
                    ajaxHandler.abort();
                    if (modal) {
                        window.$_c4.hide();
                    }
                }
            }
        });
    };

    /**
    * 分页相关
    */
    window.common.page = {
    };

    /*
    * 分页参数
    */
    window.common.page.pagerParam = new function () {
        //当前页码
        this.pageIndex = 1;
        //每页显示条数
        this.pageSize = 10;
        //共有多少页
        this.pageNumberCount = 1;
    };

    /**
     * 初始化分页控件
     * @param {Function】} callBack 回调函数
     * @param {String} noDataMsg 没有数据的提示信息
     */
    window.common.page.initDropLoad = function (callBack, noDataMsg, content) {
        var _content = content || 'body';
        $(_content).dropload({
            scrollArea: window,
            loadDownFn: function (me) {
                //这里是加载数据的地方
                window.common.page.paging(me, callBack, noDataMsg);
            }
        });
    };

    /**
     * 分页方法
     * @param {Object} dropMe dropme控件
     * @param {Function} callBack 回调函数
     * @param {String} noDataMsg 没有数据的提示信息
     */
    window.common.page.paging = function (dropMe, callBack, noDataMsg) {

        var me = window.common.page.pagerParam;
        setTimeout(function () {
            me.pageIndex++;
            if (me.pageIndex <= me.pageNumberCount) {
                if (callBack) {
                    callBack(me.pageIndex, me.pageSize);
                }
            }
            else {
                alert(noDataMsg || "没有数据啦！亲",null,500);
                me.pageIndex--;
            }
            dropMe.resetload();
        }, 200);
    };

    /**
     * 无数据显示的内容
     * @param {type} content 容器选择器
     */
    window.noData = function (content) {
        var _content = content || 'body';
        $(_content).html('<div class="w-notDATA" style="display:block" id="allOrder_havaNoData"><b></b><h4 class="w-font14 w-colorccc w-textalignC">暂时没有数据</h4></div>');
    };

    /**
     * 重新计算页码
     * @param {Number} maxCount 最大条数
     */
    window.common.page.calculationPage = function (maxCount) {
        if (parseInt(maxCount) % parseInt(window.common.page.pagerParam.pageSize) === 0) {
            window.common.page.pagerParam.pageNumberCount = parseInt(maxCount) / parseInt(window.common.page.pagerParam.pageSize);
        }
        else {
            window.common.page.pagerParam.pageNumberCount = parseInt(maxCount / window.common.page.pagerParam.pageSize) + 1;
        }
    };

    /**
     * 固定小数精度
     * @param {type} number 数字
     * @param {type} precision 精度
     * @param {type} isFourHomesFive 是否四舍五入
     * @returns {type} 
     */
    window.common.toFixed = function (number, precision, isFourHomesFive) {
        try {
            if (!precision) precision = 2;
            if (precision > 15 || precision < 0) {
                return number;
            }
            var numberStr = number.toString();

            if (isFourHomesFive) {
                if (numberStr.indexOf(".") > 0) {
                    var left = numberStr.substring(0, numberStr.indexOf("."));
                    var right = numberStr.substr(numberStr.indexOf(".") + 1, precision + 1);

                    if (right.length > precision) {
                        if (parseInt(right[precision]) >= 5) {
                            var temp = parseInt(right.substr(0, [precision])) + 1;
                            return parseFloat("{0}.{1}".format(left, temp));
                        } else {
                            return parseFloat("{0}.{1}".format(left, right.substring(0, precision)));
                        }
                    } else {
                        return number;
                    }
                } else {
                    return number;
                }
            } else {
                if (numberStr.indexOf(".") > 0) {
                    return parseFloat(numberStr.substring(0, numberStr.indexOf(".") + precision + 1));
                } else {
                    return number;
                }
            }
        } catch (e) {
            throw new Error("对数字进行固定精度操作出错，错误原因是：{0}".format(e.message));
        }
    };

    /**
     * 
     * 判断是否是手机电话
     * 
     * @method isMobilePhone
     * @for FuncDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {String} val 要判断的值
     * @returns {Boolean} 执行结果
     */
    window.common.isMobilePhone = function (val) {
        var patrn = /^((13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8})*$/;
        if (patrn.exec(val))
            return true;
        return false;
    };

    /**
     * 将变量放入localStorage
     * @param {String} key 键
     * @param {Object} value 值
     */
    window.common.setStorageItem = function (key, value) {
        window.localStorage.setItem(key, JSON.stringify(value));
    };

    /**
     * 从localStorage获取值
     * @param {String} key 键
     * @returns {Object} 获取的值，要是没有获取到则返回null 
     */
    window.common.getStorageItem = function (key) {
        var value = window.localStorage.getItem(key);
        if (value !== null && value !== undefined) {
            return JSON.parse(value);
        } else {
            return null;
        }
    };

    /**
     * 获取localStorage里面的值并不再保留
     * @param {String} key 
     * @returns {Object} 获取的值，要是没有获取到则返回null 
     */
    window.common.getStorageItemNoResident = function (key) {
        var value = window.localStorage.getItem(key);
        if (value !== null && value !== undefined) {
            window.localStorage.removeItem(key);
            return JSON.parse(value);
        } else {
            return null;
        }
    };

    /**
     * 从 localStorage 移除对象
     * @param {type} key 键
     */
    window.common.removeStorageItem = function (key) {
        if (window.localStorage.getItem(key)) {
            window.localStorage.removeItem(key);
        }
    };

    /**
     * 跳转至登录页面
     * @param {type} thisUrl登录返回的url
     */
    window.common.toLogin = function (thisUrl) {
        window.common.setStorageItem("_now_login_url", thisUrl);
    };

}());

