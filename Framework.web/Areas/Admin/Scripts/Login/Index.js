$(function () {

    var f/*方法模块*/ = modules.get(enums.Modules.FUNC);
    var eui/*easyui模块*/ = modules.get(enums.Modules.JQUERY_EASYUI);
    var btn_reset/*重置按钮*/ = $("#btn_reset");
    var btn_lgon/*登录按钮*/ = $("#btn_lgon");
    var code/*验证码框*/ = $("#vcodepic");
    var loginPanel/*登录窗口*/ = $("#lg_panel");
    var a = "9527";
    var b = "9528";

    /**
     * 初始化登录框位置
     */
    function initLoginPanel() {
        try {
            var windowHeight/*窗体高度*/ = window.innerHeight;
            var windowWidth/*窗体宽度*/ = window.innerWidth;
            var panelWidth/*登录窗体宽度*/ = loginPanel.panel("options").width;
            var panelHeight/*登录窗体高度*/ = loginPanel.panel("options").height;
            var left/*左边距*/ = (windowWidth - panelWidth) > 0 ? (windowWidth - panelWidth) / 2 : 0;
            var top/*右边距*/ = (windowHeight - panelHeight) > 0 ? (windowHeight - panelHeight) / 2 : 0;

            //设置登录窗口位置
            loginPanel.panel("move", { left: left, top: top });
        } catch (e) {
            eui.alertErr(e.message);
        }
    }

    /**
     * 登录系统
     */
    function loginSystem() {

        var ln/*登录名*/ = $("#uname").textbox("getText");

        if (ln.indexOf("请输入登录名") > -1) {
            $("#uname").textbox("clear");
            $("#lg_form").form("validate");
            return;
        }

        if ($("#lg_form").form("validate")) {

            var lp/*密码*/ = $("#upwd").textbox("getText");
            var vc/*验证码*/ = $("#vcode").textbox("getText");

            f.post("/Admin/Login/Login",
                {
                    sLoginName: ln, sPassWord: lp, sUserName: vc
                },
                function (r) {
                    rememberlogin();
                    window.location = r.Data;
                }, function (r) {
                    eui.alertMsg(r.Msg);
                });
        }
    }

    function rememberlogin() {
        var checked = $("#remember_ac_info").prop("checked");
        if (!checked) {
            $.removeCookie(a, { path: '/' });
            $.removeCookie(b, { path: '/' });
        } else {
            var ua = $("#uname").textbox("getText");
            var up = $("#upwd").textbox("getText");

            $.cookie(a, ua, { path: '/', expires: 10 });
            $.cookie(b, up, { path: '/', expires: 10 });
        }
    }

    /**
     * 初始化验证码
     */
    function initVCode() {
        f.post("/ValidateCode/VCode?" + new Date(),
            null,
            function (result) {
                code.attr("src", result.Data);
            }, function (result) {
                eui.alertErr(result.Msg);
            }, false);
    }

    /**
     * 绑定事件
     */
    function bindElementEvent() {

        //window窗体改变大小时重新定位登录窗口位置
        $(window).on("resize", initLoginPanel);

        //点击验证码获取新的验证码
        code.on("click", initVCode);

        //登录名输入框获取焦点去掉默认的值
        $("#uname").textbox("textbox").on("focus", function () {
            if ($("#uname").textbox("getText").indexOf("请输入登录名") >= 0)
                $("#uname").textbox("clear");
        });

        //登录名输入框失去焦点如果没有输入则设置默认值
        $("#uname").textbox("textbox").on("blur", function () {
            if ($("#uname").textbox("getText").length <= 0)
                $("#uname").textbox("setText", "请输入登录名");
        });

        //密码输入框获取焦点去掉默认的值
        $("#upwd").textbox("textbox").on("focus", function () {
            if ($("#upwd").textbox("getText").length > 0)
                $("#upwd").textbox("clear");
        });

        //验证码输入框获取焦点去掉默认的值
        $("#vcode").textbox("textbox").on("focus", function () {
            if ($("#vcode").textbox("getText").indexOf("请输入验证码") >= 0)
                $("#vcode").textbox("clear");
        });

        //验证码输入框失去焦点如果没有输入则设置默认值
        $("#vcode").textbox("textbox").on("blur", function () {
            if ($("#vcode").textbox("getText").length <= 0)
                $("#vcode").textbox("setText", "请输入验证码");
        });

        //重置按钮
        btn_reset.on("click", function () {
            $("#uname").textbox("reset");
            $("#upwd").textbox("reset");
            $("#vcode").textbox("reset");
        });

        //忘记密码
        $("#forget_psd").on("click", function () {
            var div = $("<div/>");
            div.dialog({
                title: "找回密码",
                width: 450,
                height: 300,
                cache: false,
                href: '/Admin/Login/ToForgetPWD',
                modal: true,
                collapsible: false,
                minimizable: false,
                maximizable: false,
                resizable: false,
                buttons: [{
                    text: '确定',
                    iconCls: 'icon-save',
                    handler: function () {
                        var sCode = modules.get(enums.Modules.CACHE).getCache("_getmcode");
                        if (sCode === true) {
                            if ($("#forget_pwd_form").form("validate")) {
                                //序列化提交数据
                                var json = $("#forget_pwd_form").serializeObject();
                                json.sLoginName = $("#sLoginName").textbox("getText");
                                f.post("/Admin/Login/ChangePWD", json,
                                    function (ret) {
                                        modules.get(enums.Modules.CACHE).cleanCache("_getmcode");
                                        eui.alertInfo("已重置密码");
                                        $(div).dialog("close");
                                    }, function (ret) {
                                        modules.get(enums.Modules.CACHE).cleanCache("_getmcode");
                                        eui.alertErr(ret.Msg);
                                    });
                            }
                        } else {
                            if (sCode === null) {
                                eui.alertErr("请获取短信验证码");
                            } else {
                                eui.alertErr("短信验证码已过期，请重新获取");
                            }
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
                    $(div).dialog("destroy"); div = null;
                },
                onLoad: function () {
                }
            });
        });

        //登录
        btn_lgon.on("click", loginSystem);

        ////记录登录信息
        //$("#remember_ac_info").on("change", function () {
        //    var checked = $("#remember_ac_info").prop("checked");
        //    if (!checked) {
        //        $.removeCookie(a, { path: '/' });
        //        $.removeCookie(b, { path: '/' });
        //    } else {
        //        var ua = $("#uname").textbox("getText");
        //        var up = $("#upwd").textbox("getText");

        //        //if (ua === "" || ua==="请输入登录名"|| up === "") {
        //        //    eui.alertInfo("请输入您要保存的账号密码");
        //        //    $("#remember_ac_info").prop("checked",false);
        //        //    return;
        //        //}

        //        $.cookie(a, ua, { path: '/', expires: 10 });
        //        $.cookie(b, up, { path: '/', expires: 10 });
        //    }
        //});
    }

    //初始化
    try {
        initLoginPanel/*初始化登录框位置*/();
        initVCode/*初始化验证码*/();
        
        //验证是否记录登录信息
        if ($.cookie(a) !== undefined && $.cookie(b) != undefined) {
            $("#uname").textbox("setText", $.cookie(a));
            $("#upwd").textbox("setText", $.cookie(b));
            $("#remember_ac_info").prop("checked", true);
        } else {
            $("#remember_ac_info").prop("checked", false);
        }

        bindElementEvent/*绑定事件*/();
    } catch (e) {
        eui.alertErr(e.message);
    }
});