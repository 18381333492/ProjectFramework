$(function () {
    var f = modules.get(enums.Modules.FUNC);
    var eui = modules.get(enums.Modules.JQUERY_EASYUI);

    $("#menu_left").mCustomScrollbar({
        autoHideScrollbar: false
        //,
        //theme: "rounded"
    });

    f.post("/Admin/Main/GetServerUrl", null, function (r) {
        modules.get(enums.Modules.CACHE).setCache(enums.VARIABLE.UEDITOR_URL, r.Data);
    }, function (r) {
        eui.alertErr(r.Msg);
    }, false, true, null, null, "text", "body");
});