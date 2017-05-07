/*!
 * Yangyukun Script Library
 * version: 1.0.1
 * build: Sun Aug 07 2016 09:00:36 GMT+0800 (中国标准时间)
 * Released under MIT license
 * 
 * Include [baidu ueditor] (http://ueditor.baidu.com/website/)
 */

modules.define("array", [], function ArrayDomain() {
    /**
     * 
     * 用给定的迭代器遍历数组或类数组对象
     * 
     * @method each
     * @for ArrayDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Array } array 需要遍历的数组或者类数组
     * @param { Function } iterator 迭代器， 该方法接受两个参数， 第一个参数是当前所处理的value， 第二个参数是当前遍历对象的key
     * @param [ Object ] context 上下文对象
     * 
     * @example
     * 
     * var divs = document.getElmentByTagNames( "div" );
     * //output: 0: DIV, 1: DIV ...
     * each( divs, funciton ( value, key ) {
     *
     *     console.log( key + ":" + value.tagName );
     *
     * } );
     * 
     */
    function each(obj, iterator, context) {
        if (obj == null) return;
        if (obj.length === +obj.length) {
            for (var i = 0, l = obj.length; i < l; i++) {
                if (iterator.call(context, obj[i], i, obj) === false)
                    return false;
            }
        } else {
            for (var key in obj) {
                if (obj.hasOwnProperty(key)) {
                    if (iterator.call(context, obj[key], key, obj) === false)
                        return false;
                }
            }
        }
    }

    /**
     * 
     * 获取元素item数组array中首次出现的位置, 如果未找到item， 则返回-1。
     * 通过start的值可以指定搜索的起始位置。该方法的匹配过程使用的是全等“===”
     * 
     * @method indexOf
     * @for ArrayDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Array } array 需要查找的数组对象
     * @param { * } item 需要在目标数组中查找的值
     * @param { int } start 搜索的起始位置
     * @return { int } 返回item在目标数组array中的start位置之后首次出现的位置， 如果在数组中未找到item， 则返回-1
     * 
     * @example
     * 
     * var item = 1,
     *     arr = [ 3, 4, 6, 8, 1, 2, 8, 3, 2, 1, 1, 4 ];
     *
     * //output: 9
     * console.log( indexOf( arr, item, 5 ) );
     */
    function indexOf(array, item, start) {
        var index = -1;
        start = this.isNumber(start) ? start : 0;
        this.each(array, function (v, i) {
            if (i >= start && v === item) {
                index = i;
                return false;
            }
        });
        return index;
    }

    /**
     * 
     * 移除数组array中的元素item，该方法的匹配过程使用的是恒等“===”
     * 
     * @method removeItem
     * @for ArrayDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Array } array 要移除元素的目标数组
     * @param { * } item 将要被移除的元素
     * 
     * @example
     * 
     * var arr = [ 4, 5, 7, 1, 3, 4, 6 ];
     *
     * removeItem( arr, 4 );
     * //output: [ 5, 7, 1, 3, 6 ]
     * console.log( arr );
     *
     */
    function removeItem(array, item) {
        for (var i = 0, l = array.length; i < l; i++) {
            if (array[i] === item) {
                array.splice(i, 1);
                i--;
            }
        }
    }

    /**
     * 
     * 对集合元素进行排序
     * 
     * @method sort
     * @for ArrayDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param {Array} array 要排序的集合
     * @param {Function} compareFn 排序的规则
     * @returns {Array} 排完序的集合
     * 
     * @example
     * 
     * var arr = [4, 5, 7, 1, 3, 4, 6];
     *   arr  = sort(arr, function (v1, v2) {
     *       return v1 > v2;
     *   });
     */
    function sort(array, compareFn) {
        compareFn = compareFn || function (item1, item2) { return item1.localeCompare(item2); };
        for (var i = 0, len = array.length; i < len; i++) {
            for (var j = i, length = array.length; j < length; j++) {
                if (compareFn(array[i], array[j]) > 0) {
                    var t = array[i];
                    array[i] = array[j];
                    array[j] = t;
                }
            }
        }
        return array;
    }

    return {
        each: each,
        indexOf: indexOf,
        removeItem: removeItem,
        sort: sort
    };
});