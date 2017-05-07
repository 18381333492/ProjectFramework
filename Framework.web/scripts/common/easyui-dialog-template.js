
/*=====================================================================
*   下面是创建dialog的模板代码，可以直接复制去修改一下
*======================================================================*/
var div = $("<div/>");
div.dialog({
    title: "标题",
    width: 500,
    height: 400,
    cache: false,
    href: '请求地址',
    modal: true,
    collapsible: false,
    minimizable: false,
    maximizable: false,
    resizable: false,
    buttons: [{
        text: '保存',
        iconCls: 'icon-save',
        handler: function () {
            if ($("#form的id，请自行替换").form("validate")) {

                //序列化提交数据
                var json = $("#form的id，请自行替换").serializeObject();
                
                f.post("提交地址", json,
                    function (ret) {
                        eui.alertInfo("成功的提示");
                        $(div).dialog("close");
                        eui.search(grid, false);
                    }, function (ret) {
                        eui.alertErr(ret.Msg);
                    });
            }
        }
    }, {
        text: '关闭',
        iconCls: 'icon-cancel',
        handler: function () {
            $(div).dialog("close");
        }
    }],
    onClose: function () {
        /**
        * 如果dialog里有对全局变量的操作并有驻留，这里记得要清除
        */
        $(div).dialog("destroy"); div = null;
    },
    onLoad: function () {
    }
});