/*!
 * Yangyukun Script Library
 * version: 1.0.1
 * build: Sun Aug 14 2016 15:04:36 GMT+0800 (中国标准时间)
 * Released under MIT license
 * 
 * Include [func.module.js,tool.module.js] 
 */

modules.define("eui", ["func", "tool"], function (func, tool) {

    $($.extend($.fn.validatebox.defaults.rules, {
        //最小数字
        minNumber: {
            validator: function (value, param) {
                return parseFloat(value) > parseFloat(param[0]);
            },
            message: '该字段不得小于等于{0}'
        },
        //验证是否是数字
        isNumber: {
            validator: function (value, param) {
                return !isNaN(value);
            },
            message: '请输入正确数字'
        },
        //验证整数是否小于设置的值
        maxInt: {
            validator: function (value, param) {
                return parseInt(value) <= parseInt(param[0]);
            },
            message: '该字段值不得大于{0}'
        },
        //验证整数是否大于设置的值
        minInt: {
            validator: function (value, param) {
                return parseInt(value) >= parseInt(param[0]);
            },
            message: '该字段值不得小于{0}'
        },
        //验证Float是否小于设置的值
        maxFloat: {
            validator: function (value, param) {
                return parseFloat(value) <= parseFloat(param[0]);
            },
            message: '该字段值不得大于{0}'
        },
        //验证最小字符
        minLength: {
            validator: function (value, param) {
                return $.trim(value).length >= param[0];
            },
            message: '请至少输入{0}个字符'
        },
        //验证最大字符
        maxLength: {
            validator: function (value, param) {
                return $.trim(value).length < param[0];
            },
            message: '该字段只能输入{0}个字符'
        },
        //验证字符只能在某个范围内，param[0]是最小值，param[1]是最大值
        chartLengthBetween: {
            validator: function (value, param) {
                return $.trim(value).length <= param[1] && $.trim(value).length >= param[0];
            },
            //message: "{2}只能是{0}-{1}位的字符"
            message: "只能是{0}-{1}位的字符"
        },
        //是否是电话号码
        isPhoneNum: {
            validator: function (value, param) {
                var patrn = /^((13[0-9]|14[0-9]|15[0-9]|17[0-9]|18[0-9])\d{8})*$/;
                if (patrn.exec(value))
                    return true;
                var patrn = /^(\d{3}-\d{8}|\d{4}-\d{7})*$/;
                if (patrn.exec(value))
                    return true;
                return false;
            },
            message: "该字段只能是11位手机号"
        },
        //密码是否相同
        equals: {
            validator: function (value, param) {
                var flag = value == $(param[0]).val();
                return flag;
            },
            message: '两次输入不一致'
        },
        newPsdequals:{
            validator: function (value, param) {
                
                var flag = value == $(param[0]).val();                
                return flag;
            },
            message: '请确认要重置的新密码输入一致'
        },
        //是否是html
        isHtmlValidate: {
            validator: function (value, param) {
                //if (!/<[^>]+>/g.test(value)) {
                //    if (value.length != 0 && value.replace(/(^\s*)|(\s*$)/g, "").length == 0) {
                //        return false;
                //    }
                //    return true;
                //}
                //var re = new RegExp("<[^>]+>", "ig");
                //while (r = re.exec(value)) {
                //    var str = r[0].replace(/\<|\>|\"|\'|\&/g, "");
                //    if (/^[A-Za-z/]+$/g.test(str)) {
                //        return false;
                //    }
                //}
                //var re= new RegExp('^<([^>]+)[^>]*>(.*?<\/\\1>)?$');
                return $("<span/>").html(value).text() == value;
                //return /<[^>]+>/g.test(value);
            },
            message: "禁止输入非法的html字符"
        },
        //是否是正确货号
        isGoodsNo: {
            validator: function (value, param) {
                if (!/^[0-9]*$/.test(value))
                    return false;
                else
                    return true;
            },
            message: "请输入正确的货号"
        },
        //验证字符是否是指定长度
        chartLengthEquire: {
            validator: function (value, param) {
                return value.length === parseInt(param[0]);
            },
            message: '请输入长度为{0}的{1}'
        },
        //验证字符是否等于指定字符
        notEquireCharter: {
            validator: function (value, param) {
                return $.trim(value) !== param[0];
            },
            message: '{0}'
        }
    }));

    /**
     * 查询datagrid数据
     * @param {type} grid grid对象
     * @param {type} isSelected 是否选中grid行
     */
    function search(grid, isSelected) {
        var getPager;
        try {
            getPager = grid.datagrid('getPager').pagination("select");
            if (!isSelected) {
                grid.datagrid("unselectAll");
            }
        } catch (e) {
            throw e;
        } finally {
            getPager = null;
        }
    }

    /**
     * 弹出消息
     * @param {String} msg 消息
     */
    function alertMsg(msg) {
        $.messager.alert("提示", msg, "warning");
    }

    /**
     * 弹出错误
     * @param {type} err 错误
     */
    function alertErr(err) {
        $.messager.alert("错误", err, "error");
    }

    /**
     * 弹出信息
     * @param {type} info 信息
     */
    function alertInfo(info) {
        $.messager.alert("信息", info, "info");
    }

    /**
     * 绑定datagrid分页事件
     * @param {objec} datagrid 数据表格
     * @param {object} options 表格配置
     * @param {function} callback 翻页回调事件
     * @returns {pager} pager对象
     */
    function bindPaginationEvent(datagrid, options, callback) {
       
        try {
            if (func.definededAndNotNull(datagrid) && options && callback) {
                return datagrid.datagrid(options).datagrid('getPager').pagination({
                    pageList: [10, 20, 30, 40, 50, 100],
                    pageNumber: 1,
                    pageSize: 10,
                    beforePageText: '第',//页数文本框前显示的汉字 
                    afterPageText: '页    共 {pages} 页',
                    displayMsg: '当前显示 {from} - {to} 条记录  共{total}条记录',
                    onSelectPage: callback
                });
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 校验数据表格是否选中行
     * @param {type} datagrid 数据表格
     * @param {type} callBackFuc 回调事件
     * @param {type} unSelectRowMessage 未选中的消息
     */
    function checkSelectedRow(datagrid, callBackFuc, unSelectRowMessage) {
        try {
            selectedRow = datagrid.datagrid("getSelected");
            if (selectedRow !== null) {
                callBackFuc(selectedRow);
            } else {
                alertMsg(unSelectRowMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 开启一个确认框
     * @param {Datagrid} datagrid datagrid对象
     * @param {Function} callBackFuc 点了确定后回调的函数
     * @param {String} confirmMessage 要提示的信息
     * @param {String} unSelectRowMessage 未选中行提示的信息    
     * @param {Function} formatConfirmMSGCallBack 表示格式化提示信息的函数（需要返回格式化好的字符串）
     */
    function confirmDomain(datagrid, callBackFuc, confirmMessage, unSelectRowMessage, formatConfirmMSGCallBack) {
        try {
            var selectedRow = null;
            selectedRow = datagrid.datagrid("getSelected");
            if (selectedRow !== null) {
                if (formatConfirmMSGCallBack) {
                    confirmMessage = formatConfirmMSGCallBack(selectedRow);
                }
                $.messager.confirm('确认', confirmMessage, function (result) {
                    if (result) {
                        callBackFuc(selectedRow);
                    }
                });
            } else {
                alertInfo(unSelectRowMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 开启一个普通确认框
     * @param {Function} callBackFuc 点了确定后回调的函数 
     * @param {String} confirmMessage 要提示的信息 默认值是“您是否确认执行该操作”
     */
    function confirm(callBackFuc, confirmMessage) {
        try {
            confirmMessage = confirmMessage ? confirmMessage : "您是否确认执行该操作？";
            $.messager.confirm('确认', confirmMessage, function (result) {
                if (result) {
                    callBackFuc();
                }
            });
        } catch (e) {
            throw e;
        }
    }

    /**
     * 带多行验证的确认提示框
     * @param {Datagrid} datagrid datagrid对象
     * @param {Function} callBackFuc 点了确定后回调的函数
     * @param {String} confirmMessage 要提示的信息
     * @param {String} unSelectRowMessage 未选中行提示的信息    
     * @param {Function} formatConfirmMSGCallBack 表示格式化提示信息的函数（需要返回格式化好的字符串）
     */
    function confirmDomainByMultiRows(datagrid, callBackFuc, confirmMessage, unSelectRowMessage, formatConfirmMSGCallBack) {
        try {
            var selectedRow = datagrid.datagrid("getSelections");
            if (selectedRow.length > 0) {
                if (formatConfirmMSGCallBack) {
                    confirmMessage = formatConfirmMSGCallBack(selectedRow);
                }
                $.messager.confirm('确认', confirmMessage, function (result) {
                    if (result) {
                        callBackFuc(selectedRow);
                    }
                });
            } else {
                alertInfo(unSelectRowMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 校验是否选中树节点
     * @param {tree} tree 树对象
     * @param {function} callBackFuc 回调函数
     * @param {string} unSelectNodeMessage 未选中的消息
     */
    function checkTreeSelected(tree, callBackFuc, unSelectNodeMessage) {
        try {
            selectedNode = tree.tree("getSelected");
            if (selectedNode !== null) {
                callBackFuc(selectedNode);
            } else {
                alertMsg(unSelectNodeMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 带确认框的树点击事件
     * @param {tree} tree 树
     * @param {function} callBackFuc 回调函数
     * @param {string} confirmMessage 确认消息
     * @param {string} unSelectRowMessage 未选中的消息
     * @param {function} formatConfirmMSGCallBack 需要格式化选中节点的回调函数
     */
    function confirmTreeNode(tree, callBackFuc, confirmMessage, unSelectRowMessage, formatConfirmMSGCallBack) {
        try {
            var selectedNode = null;
            selectedNode = tree.tree("getSelected");
            if (selectedNode !== null) {
                if (formatConfirmMSGCallBack) {
                    confirmMessage = formatConfirmMSGCallBack(selectedNode);
                }
                $.messager.confirm('操作确认', confirmMessage, function (result) {
                    if (result) {
                        callBackFuc(selectedNode);
                    }
                });
            } else {
                alertInfo(unSelectRowMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    /**
     * 带确认框的选中操作确认树点击事件
     * @param {type} tree 树
     * @param {type} callBackFuc 回调函数
     * @param {type} judgeCallBack 确认操作回调函数
     * @param {type} confirmMessage 确认消息
     * @param {type} unSelectRowMessage 未选中的消息
     * @param {type} formatConfirmMSGCallBack 需要格式化选中节点的回调函数
     */
    function confirmTreeNodeJudge(tree, callBackFuc, judgeCallBack, confirmMessage, unSelectRowMessage, formatConfirmMSGCallBack) {
        try {
            var selectedNode = null;
            selectedNode = tree.tree("getSelected");
            if (selectedNode !== null) {
                var judeResult = true;
                if (judgeCallBack) judeResult = judgeCallBack(selectedNode);
                if (!judeResult) return;
                if (formatConfirmMSGCallBack) {
                    confirmMessage = formatConfirmMSGCallBack(selectedNode);
                }
                $.messager.confirm('操作确认', confirmMessage, function (result) {
                    if (result) {
                        callBackFuc(selectedNode);
                    }
                });
            } else {
                alertInfo(unSelectRowMessage);
            }
        } catch (e) {
            throw e;
        }
    }

    return {
        alertMsg: alertMsg,
        alertErr: alertErr,
        alertInfo: alertInfo,
        bindPaginationEvent: bindPaginationEvent,
        checkSelectedRow: checkSelectedRow,
        confirmDomain: confirmDomain,
        confirmDomainByMultiRows: confirmDomainByMultiRows,
        checkTreeSelected: checkTreeSelected,
        confirmTreeNode: confirmTreeNode,
        confirmTreeNodeJudge: confirmTreeNodeJudge,
        search: search,
        confirm: confirm
    };
});