$(function () {
    void function () {
        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var global = modules.get(enums.Modules.CACHE);
        var mainTab = $("#tabs");
        
        f.post(/*载入菜单数据*/"/Admin/Menu/LoadMenu", null, function (r) {

            mainTab.tabs({
                onClose: function (title, index) {
                    /*获取菜单操作对象*/
                    var menu = global.getMenuDomain(title);

                    /*进行资源释放的操作*/
                    if (menu !== undefined &&
                        menu != null &&
                        menu.destroy !== undefined &&
                        menu.destroy !== null
                        ) {
                        menu.destroy();
                    }

                    /*清除菜单操作对象*/
                    global.cleanMenuDomain(title);
                }, onBeforeClose: function () {

                    /* 如果套用了iframe就回收内存 */                    
                    var frame = $('iframe', this);
                    if (frame.length > 0) {
                        frame[0].contentWindow.document.write('');
                        frame[0].contentWindow.close();
                        frame.remove();                        
                    }
                }
            });

            /*初始化菜单*/
            $('#tree').treeview({
                data/*菜单数据*/: r.Data,
                showTags/*是否显示标签*/: true,
                collapseIcon/*折叠后的iocn*/: "glyphicon glyphicon-folder-open",
                expandIcon/*展开后的icon*/: "glyphicon glyphicon-folder-close",
                emptyIcon/*空的icon*/: "glyphicon glyphicon-tasks",
                selectedBackColor/*设置背景色*/: "#000000",
                onhoverColor/*设备鼠标滑过菜单项的颜色*/: "#ff9966",
                color/*菜单颜色*/: "#000000",
                onNodeSelected/*选中节点时触发*/: function (event, data) {
                    if (data.url !== "root"/*不是根节点就执行普通菜单操作*/) {
                        chooseTab(data);
                    } else {
                        //是根节点的话，没展开就展开，展开了就关闭
                        if (!data.state.expanded) {
                            $('#tree').treeview('expandNode', [data.nodeId, { levels: 10, silent: true, ignoreChildren: true }]);
                        } else {
                            $("#tree").treeview("collapseNode", [data.nodeId, { silent: true, ignoreChildren: true }]);
                        }
                    }
                },
                onNodeUnselected/*选中节点时触发*/: function (event, data) {
                    setTimeout(function () {
                        var selectedNode = $('#tree').treeview("getSelected")[0];/*当前选中的节点*/
                        if (selectedNode) {
                            if (data.url === "root" && data.state.expanded && selectedNode.nodeId === data.nodeId/*选中的是自己就折叠*/) {
                                $("#tree").treeview("collapseNode", [data.nodeId, { silent: true, ignoreChildren: true }]);
                            }
                        } else {
                            if (data.url === "root" && data.state.expanded /*取消选中的是展开的就折叠*/) {
                                $("#tree").treeview("collapseNode", [data.nodeId, { silent: true, ignoreChildren: true }]);
                            } else if (data.url !== "root" /*如果是普通操作菜单，直接模拟选中事件*/) {
                                chooseTab(data);
                            } else {
                                $('#tree').treeview('expandNode', [data.nodeId, { levels: 10, silent: true, ignoreChildren: true }]);
                            }
                        }
                    }, 0);
                }
            });

        }, function (r) {
            eui.alertErr("抱歉，载入菜单失败，请联系管理员，系统将在3秒后安全退出");
            setTimeout(function () {
                window.location.href = "/Admin/Login";
            }, 3000);
        }, true, true, null, null, "text", "body");

        /**
         * 选择菜单
         * @param {type} data
         */
        function chooseTab(data) {

            var extab/*获取指定面板*/ = mainTab.tabs("getTab", data.text);

            if (!f.definededAndNotNull(extab)) {

                var count/*已打开的面板数量*/ = mainTab.tabs("tabs").length;

                if (count >= 8) {

                    //只开8个，超出就关闭第一个
                    mainTab.tabs("close", 0);
                }
                mainTab.tabs("add", {
                    title: data.text,
                    cache: true,
                    closable: true,
                    href: data.url,
                    justified: true,                    
                    style: { padding: 1 }
                });
            } else {
                mainTab.tabs("select",
                    mainTab.tabs("getTabIndex", extab));
            }
        }
    }();
});