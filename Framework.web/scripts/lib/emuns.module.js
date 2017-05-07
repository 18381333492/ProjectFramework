modules.define("enum", [], function Enums() {

    var Enums = {
        //模块名枚举
        Modules: {
            //公用发放模块
            FUNC: "func",
            //缓存模块
            CACHE: "cache",
            //格式化
            DATE_HELPER: "dateHelper",
            //集合操作模块
            ARRAY: "array",
            //百度editor模块
            BAIDU_EDITOR: "ue",
            //jquery easy ui 模块
            JQUERY_EASYUI: "eui",
            //MD5加密模块
            MD5_SECURITY: "md5",
            //base64转码模块
            BASE_64: "base64"
        },
        //变量名枚举
        VARIABLE: {
            //百度地图+地址选择的插件选择结果
            BAIDU_MAP_ADDRESS_CHOOSER: "_bcRet",
            //百度editor的上传URL
            UEDITOR_URL: "_ueditorUrl",
            //bootstrap地址选择的数据
            BOOTSTRAP_ADDRESS_DATA: "_bootstrap_address_data",
            //游记管理上传游记图片key
            UPLOAD_TRAVEL_IMAGE: "oifbpkhj_epcd_4cpg_iegf_adkemmoecapn",
            //游记管理上传作者头像key
            UPLOAD_HEAD_IMAGE: "pgmlmkel_iadi_4jee_inbo_ghmphhhnokgj"
        }
    };

    window.enums = Enums;
});