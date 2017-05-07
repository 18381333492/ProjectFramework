$(function () {
    modules.get(enums.Modules.CACHE).setMenuDomain("分享海报管理", new function () {

        var eui = modules.get(enums.Modules.JQUERY_EASYUI);
        var f = modules.get(enums.Modules.FUNC);
        var ue = modules.get(enums.Modules.BAIDU_EDITOR);

        var shareObj = {};

        var tempImgSize = {};

        function init() {
            
            f.post("/Admin/SharePoster/GetImage", null, function (res) {
                
                var imagelist = res.Data.sImagePath;
                    shareObj.UploadIDCardImage = new UploadImage({
                        target: "share_poster_Imges_upload",
                        maxFileCount: 1,
                        imgLst: [
                                {
                                    filePath: imagelist
                                }
                        ],
                        addPic: function (file) {
                            debugger
                            tempImgSize.size = file.size;
                            tempImgSize.height = file._info.height;
                            tempImgSize.width = file._info.width;
                        }
                    });
                
            }, function (res) {
                
                shareObj.UploadIDCardImage = new UploadImage({
                    target: "share_poster_Imges_upload",
                    maxFileCount: 1,
                    addPic: function (file) {
                        debugger
                        tempImgSize.size = file.size;
                        tempImgSize.height = file._info.height;
                        tempImgSize.width = file._info.width;
                    }
                });
            })
            
            $("#sava_share_poster").bind("click", function () {
                
                if (shareObj.UploadIDCardImage.imageList.length <= 0) {
                    return eui.alertInfo("请上传图片");
                }
                else {
                    debugger
                    if (tempImgSize.size > 0) {
                        if (tempImgSize.size > 1 * 1024 * 1024) {
                          return  eui.alertInfo("图片大小不能超过1M");
                        }
                        else if (tempImgSize.height < 918 || tempImgSize.width < 530) {
                            return eui.alertInfo("图片尺寸不能小于530*918");
                        }
                        $("#share_poster_idCardImges").val(shareObj.UploadIDCardImage.imageList[0].filePath);
                       f.post("/Admin/SharePoster/UploadImage", $("#share_poster_form").serializeObject(), function (res) {
                            eui.alertInfo("上传分享海报成功");
                        }, function (res) {
                            eui.alertInfo("上传分享海报失败");
                        })

                    }
                }
            })
        }
        
        try {
            init();
        }
        catch (e) {

        }
    });
});