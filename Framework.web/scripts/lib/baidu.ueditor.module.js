/*!
 * Yangyukun Script Library
 * version: 1.0.1
 * build: Sun Aug 07 2016 21:44:15 GMT+0800 (中国标准时间)
 * Released under MIT license
 * 
 */

modules.define("ue", ["cache"], function UEDomain(cache) {

    var ue = null;

    //编辑器初始化模块配置
    var uemodulecfg = {
        //锚点
        "anchor": false,
        //撤销
        "undo": false,
        //重做
        "redo": false,
        //加粗
        "bold": false,
        //首行缩进
        "indent": false,
        //截图
        "snapscreen": false,
        //斜体
        "italic": false,
        //下划线
        "underline": false,
        //删除线
        "strikethrough": false,
        //下标
        "subscript": false,
        //字符边框
        "fontborder": false,
        //上标
        "superscript": false,
        //格式刷
        "formatmatch": false,
        //源代码
        "source": false,
        //引用
        "blockquote": false,
        //纯文本粘贴模式
        "pasteplain": false,
        //全选
        "selectall": false,
        //打印
        "print": false,
        //预览
        "preview": false,
        //分隔线
        "horizontal": false,
        //清除格式
        "removeformat": false,
        //时间
        "time": false,
        //日期
        "date": false,
        //取消链接
        "unlink": false,
        //前插入行
        "insertrow": false,
        //前插入列
        "insertcol": false,
        //右合并单元格
        "mergeright": false,
        //下合并单元格
        "mergedown": false,
        //删除行
        "deleterow": false,
        //删除列
        "deletecol": false,
        //拆分成行
        "splittorows": false,
        //拆分成列
        "splittocols": false,
        //完全拆分单元格
        "splittocells": false,
        //删除表格标题
        "deletecaption": false,
        //插入标题
        "inserttitle": false,
        //合并多个单元格
        "mergecells": false,
        //删除表格
        "deletetable": false,
        //清空文档
        "cleardoc": false,
        //"表格前插入行"
        "insertparagraphbeforetable": false,
        //代码语言
        "insertcode": false,
        //字体
        "fontfamily": false,
        //字号
        "fontsize": false,
        //段落格式
        "paragraph": false,
        //单图上传
        "simpleupload": false,
        //多图上传
        "insertimage": false,
        //表格属性
        "edittable": false,
        //单元格属性
        "edittd": false,
        //超链接
        "link": false,
        //表情
        "emotion": false,
        //特殊字符
        "spechars": false,
        //查询替换
        "searchreplace": false,
        //Baidu地图
        "map": false,
        //Google地图
        "gmap": false,
        //视频
        "insertvideo": false,
        //帮助
        "help": false,
        //居左对齐
        "justifyleft": false,
        //居右对齐
        "justifyright": false,
        //居中对齐
        "justifycenter": false,
        //两端对齐
        "justifyjustify": false,
        //字体颜色
        "forecolor": false,
        //背景色
        "backcolor": false,
        //有序列表
        "insertorderedlist": false,
        //无序列表
        "insertunorderedlist": false,
        //全屏
        "fullscreen": false,
        //从左向右输入
        "directionalityltr": false,
        //从右向左输入
        "directionalityrtl": false,
        //段前距
        "rowspacingtop": false,
        //段后距
        "rowspacingbottom": false,
        //分页
        "pagebreak": false,
        //插入Iframe
        "insertframe": false,
        //默认
        "imagenone": false,
        //左浮动
        "imageleft": false,
        //右浮动
        "imageright": false,
        //附件
        "attachment": false,
        //居中
        "imagecenter": false,
        //图片转存
        "wordimage": false,
        //行间距
        "lineheight": false,
        //编辑提示
        "edittip ": false,
        //自定义标题
        "customstyle": false,
        //自动排版
        "autotypeset": false,
        //百度应用
        "webapp": false,
        //字母大写
        "touppercase": false,
        //字母小写
        "tolowercase": false,
        //背景
        "background": false,
        //模板
        "template": false,
        //涂鸦
        "scrawl": false,
        //音乐
        "music": false,
        //插入表格
        "inserttable": false,
        // 从草稿箱加载
        "drafts": false,
        // 图表
        "charts": false
    };

    /**
     * 
     * 初始化编辑器,请注意，调用该方法请在dom加载完后再调用
     * 
     * @method initUE
     * @for UEDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {String} content 富文本内容     
     */
    function initUE(name, defaultContent) {
        UE.delEditor(name);
        ue = UE.getEditor(name, {
            serverUrl: cache.getCache(enums.VARIABLE.UEDITOR_URL),
            //toolbars: [
            //    uemodulecfg
            //]
        });
        ue.ready(function () {
            if (defaultContent) {
                ue.setContent(defaultContent);
            }
        });
        return ue;
    }
    /**
   * 
   * 初始化编辑器,请注意，调用该方法请在dom加载完后再调用
   * 
   * @method initUE
   * @for UEDomain
   * @author [杨瑜堃]
   * @version 1.0.1
   * @param {String} content 富文本内容     
   */
    function setContent(ue, defaultContent) {
        ue.setContent(defaultContent);
    }



    /**
     * 
     * 获取编辑器的配置（私有方法）
     * 
     * @method getCfg
     * @for UEDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @param {string[]} 初始化配置    
     */
    function getCfg() {
        var cfg = [];
        for (var k in uemodulecfg) {
            if (uemodulecfg[k] === true) {
                cfg.push(k);
            }
        }
        return cfg;
    }

    /**
     * 
     * 获取html内容
     * 
     * @method getHtmlContent
     * @for UEDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @returns {String} html内容
     */
    function getHtmlContent(ue) {
        return ue.getContent();
    }

    /**
     * 
     * 获取文本内容
     * 
     * @method getTextContent
     * @for UEDomain
     * @author [杨瑜堃]
     * @version 1.0.1
     * @returns {String}  文本内容
     */
    function getTextContent(ue) {
        return ue.getContentTxt();
    }

    /**
   * 
   * 初始化编辑器,请注意，调用该方法请在dom加载完后再调用
   * 
   * @method initUE
   * @for UEDomain
   * @author [汤台]
   * @version 1.0.1
   * @param {String} name      
   */
    function UEeditor(name) {
        var _editor;
        if (name === 1) {  
            _editor = UE.getEditor('UE_container', { serverUrl: window.global.ServerUrl });
        }
        _editor = UE.getEditor('container_UE', { serverUrl: window.global.ServerUrl });
        var _callbackImg,
            _callbackFile; //_callbackImg 上传选择图片后回调；_callbackFile 上传选择附件后回调
        _editor.ready(function () {
            _editor.hide();  //隐藏编辑
            _editor.addListener('beforeInsertImage', function (t, arg) { 
                if (_callbackImg) {
                    _callbackImg(arg);
                    _callbackImg = null;
                } 
            });//侦听图片上传
        });
        _editor.SingleUpLoadImage = function (callback) {
            _editor.getDialog("insertimage").open();
            _callbackImg = callback;
        }//弹出图片上传的对话框 
        return _editor;
    }

     /**
     * 使用前参数初始化  @Html.Action("FilesInit", "Uediter", new { area = "", module = "goods", field = "goods", wh = "500,500|100,100" });
     *    引入插件地址  <script src="/Scripts/ueditor/single_upload/uediter-single-upload.js"></script>
     * 
     * 图片上传初始化
     * var selectImages = UE.ehecd_upload_image_init(id,images,maxNum);
     * @param  string[字符串]  id          附加元素id[不带#] 
     * @param  array [数组]    images      已有图片
     * @param  int             maxNum      最大上传张数
     *     demo   UE.ehecd_upload_image_init('images-select-id',[],maxNum);
     *            UE.ehecd_upload_image_init('images-select-id',['http://dddddd','http://dddddd'],55);
     * //获取选择上传图片
     * var sselectImagesData = selectImages.get_imgs();
     * 
     *  
     * 附件上传初始化
     * var selectfiles = UE.ehecd_upload_file_init(id,files);
     * @param  string[字符串]  id          附加元素id[不带#] 
     * @param  array [数组]    files       已有文件
     *     demo   UE.ehecd_upload_file_init('files-select-id',[]);
     *            UE.ehecd_upload_file_init('files-select-id',[{title:'文件名',url:'文件路径'},{title:'文件名',url:'文件路径'}]);
     * //获取选择上传图片
     * var selectfilesData = selectfiles.get_files();
     * 
     */
    function InitPicture(_editor, id, selected, maxNum, inputId) {
        maxNum = maxNum || 5; //最大上传张数默认5张图片
        var _html = '<ul id="c-' + id + '" class="ue-select-image-list"><ul>';
        $('#' + id).html(_html);
        var cSelectImage = $('#c-' + id);

        cSelectImage.html('<li><img id="btn-' + id + '" src="" class="btn-ue-upimage-toserver"></li>');
        function randerSelectImages(src) {
            return '<li><div class="redm"><img src="' + src + '" /><span><i class="icon-cancel"></i></span></div></li>';
        }
        
        var _num = 0;//当前上传张数
        if (selected && selected.length > 0)
        {//初始值
            _html = '';
            $(selected).each(function (i) {
                _html += randerSelectImages(selected[i]);
            });
            cSelectImage.append(_html);
            bindEvent();
            _num += selected.length;
        }
        var selectImg;
        var btnSelect = $('#btn-' + id)
        btnSelect.click(function () {
            _editor.SingleUpLoadImage(function (selectImages) {
                _html = '';
                if (selectImages && selectImages.length) {
                    for (var i = 0; i < selectImages.length; i++) {
                        if (_num >= maxNum) { alert("上传张数不能超过" + maxNum + "张"); break; }//当前上传张数不能超过最大上传张数
                        selectImg = Urifrmate(selectImages[i].src);
                        selected.push(selectImg);
                        _html += randerSelectImages(selectImg);
                        ++_num;
                    }
                    cSelectImage.append(_html);
                    bindEvent();
                    $('#' + inputId).val(selected.join());
                }

            });
        });
        function Urifrmate(uri) { return (uri.indexOf("-") > 0 ? uri.substring(0, uri.indexOf("-")) + uri.substring(uri.lastIndexOf("."), uri.length) : uri); }
        //绑定遮掩层效果时间
        function bindEvent() {
            cSelectImage.find('li').mouseenter(function () { $(this).find('span').show(); })
                .mouseleave(function () { $(this).find('span').hide(); });
            cSelectImage.find('.icon-cancel').unbind('click').bind('click',
                function () {
                var li = $(this).parents('li');
                selected.
                    splice(jQuery.inArray(li.find('img').attr('src'), selected), 1);
                li.remove();
                --_num;
                
                $('#' + inputId).val(selected.join());
            });
        }
        return { get_imgs: function () { return selected; } }
    };


    return {
        ueModuleCfg: uemodulecfg,
        initUE: initUE,
        setContent:setContent,
        UEeditor: UEeditor,
        InitPicture:InitPicture,
        getHtmlContent: getHtmlContent,
        getTextContent: getTextContent,
    };
});