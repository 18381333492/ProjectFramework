使用时间插件


头部需要引用两个样式

<!--基础样式-->
<link href="~/scripts/plug-in/date/laydate-master/need/laydate.css" rel="stylesheet" />

<!--主题样式-->
<link href="~/scripts/plug-in/date/laydate-master/skins/molv/laydate.css" rel="stylesheet" />

主题样式有6个，都放在~/scripts/plug-in/date/laydate-master/skins目录下，

切换主题：

第一，需要修改laydate.dev.js（这是开发代码，我们都用这个代码，因为配置要在这里改，麻烦的很）

中config的配置，config的配置如下

var config = {
    path: '', //laydate所在路径
    skin: 'molv', //初始化皮肤
    format: 'YYYY-MM-DD hh:mm:ss', //日期格式
    min: '1900-01-01 00:00:00', //最小日期
    max: '2099-12-31 23:59:59', //最大日期
    isv: false,
    init: true
};

第二，需要引入你要用的主题样式

头部需要引入的js

<script src="~/scripts/plug-in/date/laydate-master/laydate.dev.js"></script>

网页中需要添加的元素

<input onclick="laydate({ istime: true })" class="textbox" style="height:20px;">

其中，里面的参数可以配置显示的功能，详细参数如下

关于options的属性：

    options是一个对象，它包含了以下key: '默认值'
        elem: '#id', //需显示日期的元素选择器
        event: 'click', //触发事件
        format: 'YYYY-MM-DD hh:mm:ss', //日期格式
        istime: false, //是否开启时间选择
        isclear: true, //是否显示清空
        istoday: true, //是否显示今天
        issure: true, 是否显示确认
        festival: true //是否显示节日
        min: '1900-01-01 00:00:00', //最小日期
        max: '2099-12-31 23:59:59', //最大日期
        start: '2014-6-15 23:00:00',    //开始日期
        fixed: false, //是否固定在可视区域
        zIndex: 99999999, //css z-index
        choose: function(dates){ //选择好日期的回调
        }