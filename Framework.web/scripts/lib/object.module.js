/*!
 * Yangyukun Script Library
 * version: 1.0.1
 * build: Sun Aug 07 2016 09:00:36 GMT+0800 (中国标准时间)
 * Released under MIT license
 * 
 * Include [baidu ueditor] (http://ueditor.baidu.com/website/)
 */

//定义object模块
modules.define("object", [], function ObjectDomain() {

    /**
     * 序列化容器内的带name属性的输入框的值为json object
     * @method serializeObject
     * @for ObjectDomain
     * @author [王其]
     * @version 1.0.1
     * @returns {Object}  
     */
    $.fn.serializeObject = function () {
        var o = {};
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
     * 
     * 以给定对象作为原型创建一个新对象
     * 
     * @method makeInstance
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Object } protoObject 该对象将作为新创建对象的原型
     * @returns { Object } 新的对象， 该对象的原型是给定的protoObject对象
     */
    function makeInstance(obj) {
        var noopObj = new Function();
        noopObj.prototype = obj;
        obj = new noopObj;
        noopObj.prototype = null;
        return obj;
    }

    /**
     * 
     * 模拟继承机制， 使得subClass继承自superClass。该方法只能让subClass继承超类的原型， subClass对象自身的属性和方法不会被继承
     * 
     * @method inherits
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Object } subClass 子类对象
     * @param { Object } superClass 父类对象
     * @returns { Object } 继承superClass后的子类对象
     */
    function inherits(subClass, superClass) {
        var oldP = subClass.prototype,
            newP = makeInstance(superClass.prototype);
        extend(newP, oldP, true);
        subClass.prototype = newP;
        return (newP.constructor = subClass);
    }

    /**
     * 
     * 将source对象中的属性扩展到target对象上， 根据指定的isKeepTarget值决定是否保留目标对象中与源对象属性名相同的属性值。
     * 
     * @method extend
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Object } target 目标对象， 新的属性将附加到该对象上
     * @param { Object } source 源对象， 该对象的属性会被附加到target对象上
     * @param { Boolean } isKeepTarget 是否保留目标对象中与源对象中属性名相同的属性
     * @returns { Object } 返回target对象     
     */
    function extend(t, s, b) {
        if (s) {
            for (var k in s) {
                if (!b || !t.hasOwnProperty(k)) {
                    t[k] = s[k];
                }
            }
        }
        return t;
    }

    /**
     * 
     * 将给定的多个对象的属性复制到目标对象target上，
     * 该方法将强制把源对象上的属性复制到target对象上，
     * 该方法支持两个及以上的参数， 从第二个参数开始， 
     * 其属性都会被复制到第一个参数上。 如果遇到同名的属性，
     * 将会覆盖掉之前的值。
     * 
     * @method extend2
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Object } target 目标对象， 新的属性将附加到该对象上
     * @param { Object... } source 源对象， 支持多个对象， 该对象的属性会被附加到target对象上
     * @returns { Object } 返回target对象
     * 
     * @example
     *
     * var target = {},
     *     source1 = { name: 'source', age: 17 },
     *     source2 = { title: 'dev' };
     *
     * extend2( target, source1, source2 );
     *
     * //output: { name: 'source', age: 17, title: 'dev' }
     * console.log( target );
     *
     */
    function extend2(t) {
        var a = arguments;
        for (var i = 1; i < a.length; i++) {
            var x = a[i];
            for (var k in x) {
                if (!t.hasOwnProperty(k)) {
                    t[k] = x[k];
                }
            }
        }
        return t;
    }

    /**
     * 
     * 用指定的context对象作为函数fn的上下文
     * 
     * @method bind
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Function } fn 需要绑定上下文的函数对象
     * @param { Object } content 函数fn新的上下文对象
     * @return { Function } 一个新的函数， 该函数作为原始函数fn的代理， 将完成fn的上下文调换工作。
     * 
     * @example
     *
     * var name = 'window',
     *     newTest = null;
     *
     * function test () {
     *     console.log( this.name );
     * }
     *
     * newTest = UE.utils.bind( test, { name: 'object' } );
     *
     * //output: object
     * newTest();
     *
     * //output: window
     * test();
     */
    function bind(fn, context) {
        return function () {
            return fn.apply(context, arguments);
        };
    }

    /**
     * 
     * 深度克隆对象，将source的属性克隆到target对象， 会覆盖target重名的属性。
     * 
     * @method clone
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param { Object } source 源对象
     * @param { Object } target 目标对象
     * @return { Object } 附加了source对象所有属性的target对象
     */
    function clone(source, target) {
        var tmp;
        target = target || {};
        for (var i in source) {
            if (source.hasOwnProperty(i)) {
                tmp = source[i];
                if (typeof tmp == 'object') {
                    target[i] = utils.isArray(tmp) ? [] : {};
                    utils.clone(source[i], target[i])
                } else {
                    target[i] = tmp;
                }
            }
        }
        return target;
    }

    /**
     * 
     * 删除对象中空的属性
     * 
     * @method clearEmptyAttrs
     * @for ObjectDomain
     * @author [baidu ueditor]
     * @version 1.4.3.3
     * @param {Object} obj 要删除空属性的对象。
     * @returns {Object} 已删除空属性的对象
     */
    function clearEmptyAttrs(obj) {
        for (var p in obj) {
            if (obj[p] === '') {
                delete obj[p]
            }
        }
        return obj;
    }

    return {
        extend: extend,
        makeInstance: makeInstance,
        extend2: extend2,
        inherits: inherits,
        bind: bind,
        clone: clone,
        clearEmptyAttrs: clearEmptyAttrs
    };

});