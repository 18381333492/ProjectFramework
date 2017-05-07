/*!
 * Yangyukun Script Library
 * version: 1.0.1
 * build: Sun Aug 07 2016 09:00:36 GMT+0800 (中国标准时间)
 * Released under MIT license
 * 
 * Include [baidu ueditor] (http://ueditor.baidu.com/website/)
 */


modules.define("tool", [], function ToolDomain() {

    /**
     * 
     * 对Date的扩展，将 Date 转化为指定格式的String
     * 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符， 
     * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字) 
     * 
     * @method Format
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

    /**
     * 
     * 格式化字符串
     * 
     * @method format
     * @for String
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
     * 将str中的html符号转义,将转义“'，&，<，"，>”五个字符
     * 
     * @method unhtml
     * @for String
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { String } reg 需要转义的正则表达式（可选）
     * @return { String } 转义后的字符串
     * 
     * @example
     * 
     * var html = '<body>&</body>';
     * 
     * console.log( html.unhtml());//output: &lt;body&gt;&amp;&lt;/body&gt;
     */
    String.prototype.unhtml = function (reg) {
        return this ? this.replace(reg || /[&<">'](?:(amp|lt|quot|gt|#39|nbsp|#\d+);)?/g, function (a, b) {
            if (b) {
                return a;
            } else {
                return {
                    '<': '&lt;',
                    '&': '&amp;',
                    '"': '&quot;',
                    '>': '&gt;',
                    "'": '&#39;'
                }[a];
            }
        }) : '';
    }

    /**
     * 
     * 将str中的转义字符还原成html字符  
     *    
     * @method html
     * @for String
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { String } str 需要逆转义的字符串
     * @return { String } 逆转义后的字符串
     * 
     * @example
     * 
     * var str = '&lt;body&gt;&amp;&lt;/body&gt;';
     * console.log( str.html( str ) );//output: <body>&</body>
     */
    String.prototype.html = function () {
        return this ? this.replace(/&((g|l|quo)t|amp|#39|nbsp);/g, function (m) {
            return {
                '&lt;': '<',
                '&amp;': '&',
                '&quot;': '"',
                '&gt;': '>',
                '&#39;': "'",
                '&nbsp;': ' '
            }[m];
        }) : '';
    }

    /**
     * 
     * 判断传入的是否是文件
     * 
     * @method isFile
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} obj 要判断的文件
     * @returns {Boolean} 结果 
     */
    function isFile(obj) {
        return toString.call(obj) === '[object File]';
    }

    /**
     * 
     * 判断对象是否是数字
     * 
     * @method isNumber
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} value 要判断的值
     * @returns {Boolean} 判断结果
     */
    function isNumber(value) {
        return typeof value === 'number';
    }

    /**
     * 
     * 判断对象是否是日期对象
     * 
     * @method isDate
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} value 要判断的对象
     * @returns {Boolean}  判断结果
     */
    function isDate(value) {
        return toString.call(value) === '[object Date]';
    }

    /**
     * 
     * 判断对象是否是数组
     * 
     * @method isArray
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} value 要判断的对象
     * @returns {Boolean}  判断结果
     */
    function isArray(value) {
        return Array.isArray(value);
    }

    /**
     * 
     * 判断一个数是否是小数(字符)
     * 
     * @method isDecimal
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {number} num 要判断的内容
     * @returns {Boolean} 结果
     */
    function isDecimal(num) {
        try {
            if (typeof num === 'number') {
                var numString = num.toString();
                if (numString.indexOf('.') > 0) {
                    return parseInt(numString.substring(numString.indexOf(".") + 1)) > 0;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        } catch (e) {
            return false;
        }
    }

    /**
     * 
     * 判断对象是否是字符串
     * 
     * @method isString
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} val 要判断的对象
     * @returns {Boolean}  判断结果
     */
    function isString(val) {
        return typeof val === 'string';
    }

    /**
     * 
     * 判断对象是否是函数
     * 
     * @method isFunction
     * @for ToolDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {Object} val 要判断的对象
     * @returns {Boolean} 判断结果
     */
    function isFunction(val) {
        return typeof val === 'function';
    }

    return {
        isFile: isFile,
        isNumber: isNumber,
        isDate: isDate,
        isArray: isArray,
        isDecimal: isDecimal,
        isString: isString,
        isFunction: isFunction
    };
});