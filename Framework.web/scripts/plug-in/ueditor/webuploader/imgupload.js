/**
 * 上传图片插件
 * @param {type} target 插件的容器选择器，id选择器不用加#号
 */
function UploadImage(config) {
    this.config = config || {};
    this.config.isSingle = config.isSingle || false;
    this.target = config.target.constructor == String ? '#' + config.target : '#' + $(config.target).attr("id");
    this.$wrap/*插件容器对象*/ = config.target.constructor == String ? $('#' + config.target) : $(config.target);
    this.addPic = config.addPic || undefined;
    this.removePic = config.removePic || undefined;
    this.createExistsImageHandle = config.createExistsImageHandle || undefined;
    this.removeExistsImageHandle = config.removeExistsImageHandle || undefined;
    this.init();
}

/**
* 上传图片插件原型
*/
UploadImage.prototype = {
    removeFileCount: function () {
        this.fileCount--;
    },
    /**
     * 创建已有的图片框
     * @param {type} file
     */
    createExistsImage: function (file) {

        var $li/*显示图片的容器*/ = $('<li id="' + file.id + '">' +
                        '<p class="imgWrap"></p>' +
                    '</li>'),

        $btns/*删除按钮*/ = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span></div>').appendTo($li),

        $wrap/*图片容器*/ = $li.find('p.imgWrap');

        var img/*图片节点*/ = $('<img src="' + file.filePath + '">');

        $wrap.empty().append(img);

        //鼠标移过就显示删除按钮
        $li.on('mouseenter', function () {
            $btns.stop().animate({ height: 30 });
        });

        //鼠标离开就隐藏删除按钮
        $li.on('mouseleave', function () {
            $btns.stop().animate({ height: 0 });
        });

        //临时保存图片集合，用于btns在不同作用域引用
        var list = this.imageList;
        var _this = this;
        //绑定删除按钮事件
        $btns.on('click', 'span', function () {
            //移除li元素
            $li.remove();
            _this.removeFileCount();
            //从图片集合移除对应的图片信息
            var src = $(this).parents("li:first").find('.imgWrap img').attr("src");
            var obj = null;
            for (var i = 0; i < _this.imageList.length; i++) {
                if (src === _this.imageList[i].filePath.toString()) {
                    _this.imageList.splice(i, 1);
                    break;
                }
            }
            if (_this.removeExistsImageHandle) {
               // _this.removeExistsImageHandle(src);
            }
        });

        $li.appendTo(this.$queue);
    },    
    /**
     * 初始化对象
     */
    init: function () {

        this.imageList = this.config.imgLst || [];        
        // 添加的文件数量
        this.fileCount = this.config.fileCount || 0;

        //初始化已有图片队列容器
        this.initContainer();
        
        if (this.imageList.length > 0) {            
            var pix_id = 'WU_FILE_EX_';
            for (var i = 0; i < this.imageList.length; i++) {
                this.fileCount++;
                this.imageList[i].id = pix_id + i;
                this.createExistsImage(this.imageList[i]);
                if (this.createExistsImageHandle) {
                    this.createExistsImageHandle(this.imageList[i]);
                }
            }
        }

        this.initUploader();//初始化上传插件     
    },
    //addPic: this.addPic,
    //removePic:this.removePic,

    /**
     * 初始化已有图片队列容器
     */
    initContainer: function () {
        //在class为queueList的div容器下添加一个ul，设置class为filelist并用其来装填已上传图片
        this.$queue = $('<ul class="filelist"></ul>').insertBefore(this.$wrap.find('.placeholder'));
    },
    /* 初始化容器 */
    initUploader: function () {
        var _this = this,
            $ = jQuery, // 确认$表示的是jQuery插件库
            $wrap = _this.$wrap,//插件容器对象
            // 图片容器
            $queue = this.$queue,
            // 上传按钮
            //$upload = $wrap.find('.uploadBtn'),
            // 没选择文件之前的内容。
            $placeHolder = $wrap.find('.placeholder'),
            
            // 添加的文件总大小
            //fileSize = 0,
            //设置文件的总数量
            fileLimitCount = _this.config.maxFileCount || 10,
            // 优化retina, 在retina下这个值是2
            ratio = window.devicePixelRatio || 1,
            // 缩略图大小
            thumbnailWidth = 110 * ratio,
            thumbnailHeight = 110 * ratio,

            // 可能有pedding, ready, uploading, confirm, done.
            state = 'pedding',

            // 所有文件的进度信息，key为file id
            percentages = {},

            supportTransition = (function () {
                var s = document.createElement('p').style,
                    r = 'transition' in s ||
                          'WebkitTransition' in s ||
                          'MozTransition' in s ||
                          'msTransition' in s ||
                          'OTransition' in s;
                s = null;
                return r;
            })(),
        // WebUploader实例
        uploader;

        if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
            throw new Error('WebUploader does not support the browser you are using.');
        }
        
        // 实例化
        uploader = WebUploader.create({
            pick: {
                id:_this.target +' #filePicker',
                label: '点击选择图片'
            },
            //拖拽容器
            dnd: _this.target + ' .queueList',
            //粘贴容器
            paste: _this.target + ' .queueList',//document.body,            
            auto: true,//有文件自动上传
            duplicate: true,//不去重
            chunked: true,//[默认值：false] 是否要分片处理大文件上传
            chunkSize: 5242880,// {Boolean} [可选] [默认值：5242880] 如果要分片，分多大一片？ 默认大小为5M.
            chunkRetry: 4,//{Boolean} [可选] [默认值：2] 如果某个分片由于网络问题出错，允许自动重传多少次？
            threads: 10,// {Boolean} [可选] [默认值：3] 上传并发数。允许同时最大上传进程数
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',
                mimeTypes: 'image/gif,image/jpg,image/jpeg,image/bmp,image/png'
            },

            // swf文件路径   
            swf: '/third-party/webuploader/Uploader.swf',
            disableGlobalDnd: true,
            chunked: true,
            server: 'http://115.28.48.78:8092/api/upload/controller?action=custom&module=YKShareStore&field=YKShareStore&appKey=615MK4VF6PQE9T30L325W2UCA1SYIRXNJDBK',
            fileNumLimit: fileLimitCount,
            fileSizeLimit: 5 * 1024 * 1024,    // 200 M
            fileSingleSizeLimit: 1 * 1024 * 1024    // 50 M
        });

        this.uploader = uploader;

        this.uploader.refresh();


        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            var $li = $('<li id="' + file.id + '">' +
                    //'<p class="title">' + file.name + '</p>' +
                    '<p class="imgWrap"></p>' +
                    '<p class="progress"><span></span></p>' +
                    '</li>'),

                $btns /*图片的文件操作panel，包括3个按钮，删除，左旋转，右旋转*/
                = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +
                    '<span class="rotateRight">向右旋转</span>' +
                    '<span class="rotateLeft">向左旋转</span></div>').appendTo($li),
                $prgress/*上传百分比容器*/ = $li.find('p.progress span'),
                $wrap/*图片封装*/ = $li.find('p.imgWrap'),
                $error/*错误信息*/ = $('<p class="error"></p>').hide().appendTo($li),

                showError/*显示错误信息*/ = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;
                        case 'interrupt':
                            text = '上传暂停';
                            break;
                        case 'http':
                            text = "网络出错";
                            break;
                        case 'not_allow_type':
                            text = "文件类别不匹配";
                            break;
                        default:
                            text = '上传失败，请重试';
                            break;
                    }
                    $error.text(text).show();
                };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                // @todo lazyload
                $wrap.text('预览中');
                uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        $wrap.text('不能预览');
                        return;
                    }

                    var img = $('<img src="' + src + '">');
                    $wrap.empty().append(img);
                }, thumbnailWidth, thumbnailHeight);

                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    //隐藏旋转和删除                    
                    //$li.find(".file-panel").hide().find("*").hide();                    
                    $btns.hide();
                }
                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    //console.log(file.statusText);
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    $li.find(".file-panel").show().find(".cancel").show();
                    $btns.show();
                } else if (cur === 'cancelled') {
                    //if (_this.removePic) {
                    //    _this.removePic();
                    //}
                }
                $li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            $li.on('mouseenter', function () {
                $btns.stop().animate({ height: 30 });
            });

            $li.on('mouseleave', function () {
                $btns.stop().animate({ height: 0 });
            });

            $btns.on('click', 'span', function () {
                var index = $(this).index(),
                    deg;

                switch (index) {
                    case 0:
                        uploader.removeFile(file);
                        return;

                    case 1:
                        file.rotation += 90;
                        break;

                    case 2:
                        file.rotation -= 90;
                        break;
                }

                if (supportTransition) {
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                } else {
                    $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');
                }
            });

            $li.appendTo($queue);
        }

        // 负责view的销毁
        function removeFile(file) {
            var $li = $('#' + file.id);
            delete percentages[file.id];
            $li.off().find('.file-panel').off().end().remove();
        }

        /**
         * 设置上传状态
         * @param {type} val
         */
        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }

            state = val;

            switch (state) {
                case 'pedding':
                    $queue.hide();
                    uploader.refresh();
                    break;

                case 'ready':
                    $queue.show();
                    uploader.refresh();
                    break;

                case 'uploading':
                    break;

                case 'paused':
                    break;

                case 'confirm':
                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        $queue.show();
                        uploader.refresh();
                        $("div.message").html('上传成功');
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }
        }

        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');
            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
        };

        /**
         * 当文件添加进文件队列时
         * @param {type} file 文件对象
         */
        uploader.onFileQueued = function (file) {
            _this.fileCount++;

            addFile(file);
            setState('ready');
        };

        /**
         * 当文件从文件队列中移除时
         * @param {type} file 文件对象
         */
        uploader.onFileDequeued = function (file) {
            _this.fileCount--;

            if (!_this.fileCount) {
                setState('pedding');
            }

            removeFile(file);
        };

        /**
         * 上传成功时触发的事件
         */
        uploader.on('uploadSuccess', function (file, ret) {
            var $file = $('#' + file.id);
            try {
                var responseText = (ret._raw || ret),
                    json = eval('(' + responseText + ')');
                if (json.state == 'SUCCESS') {
                    
                    _this.imageList.push(json);
                    $file.append('<span class="success"></span>');
                } else {
                    $file.find(".error").text(json.state).show();
                }
            } catch (e) {
                $file.find(".error").text(json.state).show();
            }
        });

        uploader.on('uploadError', function (file, code) {
        });

        //图片上传数量判断
        uploader.on('beforeFileQueued', function (file) {
            
            if (_this.fileCount >= fileLimitCount) {
                alert("文件数量超过限制！");
                return false;
            }
        });

        uploader.on('fileDequeued', function (file) {
            if (_this.removePic) {
                _this.removePic(file);
            }
        });

        uploader.on('error', function (code, file) {
            if (code == 'Q_TYPE_DENIED' || code == 'F_EXCEED_SIZE') {
                if (code == 'F_EXCEED_SIZE')
                    alert("上传图片大小超过限制!");
                //addFile(file);
                //console.log('add error file');
            }

            if (code == "Q_EXCEED_NUM_LIMIT") {
                alert("只能上传" + fileLimitCount + "张图片");
            }

        });

        uploader.on('uploadComplete', function (file, ret) {
            
            if (_this.addPic) {
                _this.addPic(file);
            }
        });

        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });

        uploader.onError = function (code) {
            console.log('Eroor: ' + code);
        };
    },
    /**
     * 销毁插件对象容器
     */
    destroy: function () {
        this.$wrap.remove();
    }
};



