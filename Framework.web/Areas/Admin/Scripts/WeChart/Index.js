$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("微信绑定设置", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);

        function bindEvent()
        {
            $("#wechar_set_save_button").bind('click', function ()
            {
                if (!$("#wechart_set_index_form").form("validate"))
                {
                    return false;
                }
                
               
                //判断是否是网址
                var strRegex = /(http(s)?:\/\/|^$)([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?/;
                var wangzhi = new RegExp(strRegex);
                if (!wangzhi.exec($("#wechart_url").val()))
                { return eui.alertInfo("请输入正确的url"); }
              
                //保存设置
                f.post("/Admin/WeChartSet/WeChartSet", $("#wechart_set_index_form").serializeObject(), function () {
                    eui.alertInfo("保存成功");
                }, function () {
                    eui.alertInfo("保存失败");
                })
            })
        }

        try {
            bindEvent();
        }
        catch (e) { }
    });
});