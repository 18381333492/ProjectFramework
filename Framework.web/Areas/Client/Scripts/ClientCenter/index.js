/// <reference path="E:\源码\成都亿合科技\源码\友客分享商城\ProjectFramework\Framework.web\scripts/lib/client.common.js" />

/*
*个人中心相关
*/

$(function () {

    //加载用户相关信息
    scope.GetNoReadMessageCount();
    scope.GetNoReadOrderCount();
});


var scope = (function (obj) { return obj; })(new function () {
    
    /*!
    * 获取会员站内信未读条数
    */
    function GetNoReadMessageCount() {
        common.post("/Client/ClientCenter/GetNoReadMessageCount", null, function (r) {

        });
    }

    /*!
    * 获取待付款和带使用订单未读数目
    */
    function GetNoReadOrderCount() {
        common.post("/Client/ClientCenter/GetNoReadMessageCount", null, function (r) {

        });
    }


    return {
        GetNoReadMessageCount: GetNoReadMessageCount,
        GetNoReadOrderCount, GetNoReadOrderCount
    }

});