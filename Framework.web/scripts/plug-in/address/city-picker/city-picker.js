/*!
 * CityPicker v1.0.2
 * https://github.com/tshi0912/citypicker
 *
 * Copyright (c) 2015-2016 Tao Shi
 * Released under the MIT license
 *
 *
 * Date: 2016-02-29T12:11:36.477Z
 *
 * modify by 杨瑜堃 2016-09-11 取消了严格模式（兼容考虑），地址数据采用载入对象而不再使用原作者的全局变量方式
 * 同时为了达到选择地址的栏位可以在网页最上方不放在其创建的元素中，将生成的规则进行了极大的变更，由以前随控件
 * 创建改为动态创建，基本等于将整个事件触发机制重制
 */

(function (factory) {
    factory(jQuery);
})(function ($) {
    var ChineseDistricts = modules.get(enums.Modules.CACHE).getCache(enums.VARIABLE.BOOTSTRAP_ADDRESS_DATA);
    var NAMESPACE = 'citypicker';
    var EVENT_CHANGE = 'change.' + NAMESPACE;
    var PROVINCE = 'province';
    var CITY = 'city';
    var DISTRICT = 'district';

    function CityPicker(element, options) {
        this.$element = $(element);
        this.$dropdown = null;
        this.options = $.extend({}, CityPicker.DEFAULTS, $.isPlainObject(options) && options);
        this.active = false;
        this.dems = [];
        this.init();
    }

    CityPicker.prototype = {
        constructor: CityPicker,
        init: function () {

            this.defineDems();

            this.render();

            this.bind();

            this.active = true;
        },
        render: function () {
            var p = this.getPosition(),
                placeholder = this.$element.attr('placeholder') || this.options.placeholder,
                textspan = '<span class="city-picker-span" style="' +
                    this.getWidthStyle(p.width) + 'height:' +
                    p.height + 'px;line-height:' + (p.height - 1) + 'px;">' +
                    (placeholder ? '<span class="placeholder">' + placeholder + '</span>' : '') +
                    '<span class="title"></span><div class="arrow"></div>' + '</span>';
            this.$element.addClass('city-picker-input');
            this.$textspan = $(textspan).insertAfter(this.$element);
        },
        rerender: function () {
            var $this = this;
            var p = this.getPosition(),
                dropdown = '<div class="city-picker-dropdown" style="z-index:9999;' +
                    this.getWidthStyle(p.width, true) + '">' +
                    '<div class="city-select-wrap">' +
                    '<div class="city-select-tab">' +
                    '<a class="active" data-count="province">省份</a>' +
                    (this.includeDem('city') ? '<a data-count="city">城市</a>' : '') +
                    (this.includeDem('district') ? '<a data-count="district">区县</a>' : '') + '</div>' +
                    '<div class="city-select-content">' +
                    '<div class="city-select province" data-count="province"></div>' +
                    (this.includeDem('city') ? '<div class="city-select city" data-count="city"></div>' : '') +
                    (this.includeDem('district') ? '<div class="city-select district" data-count="district"></div>' : '') +
                    '</div></div>';
            this.$dropdown = $(dropdown).appendTo("body");
            var $select = this.$dropdown.find('.city-select');

            $.each(this.dems, $.proxy(function (i, type) {
                this['$' + type] = $select.filter('.' + type);
            }, this));

            this.$dropdown.on('click', '.city-select a', this, function (e) {

                var $select = $(this).parents('.city-select');
                var $active = $select.find('a.active');
                var last = $select.next().length === 0;
                $active.removeClass('active');
                $(this).addClass('active');
                if ($active.data('code') !== $(this).data('code')) {
                    $select.data('item', {
                        address: $(this).attr('title'), code: $(this).data('code')
                    });
                    $(this).trigger(EVENT_CHANGE);
                    $this.feedText();
                    $this.feedVal();
                    if (last) {
                        $this.close(true);
                    }
                }
            }).on('click', '.city-select-tab a', function () {
                if (!$(this).hasClass('active')) {
                    var type = $(this).data('count');
                    $this.tab(type);
                }
            }).on("mouseleave", function () {
                $this.close(true);
            });

            if (this.$province) {
                this.$province.on(EVENT_CHANGE, (this._changeProvince = $.proxy(function () {
                    this.output(CITY);
                    this.output(DISTRICT);
                    this.tab(CITY);
                }, this)));
            }

            if (this.$city) {
                this.$city.on(EVENT_CHANGE, (this._changeCity = $.proxy(function () {
                    this.output(DISTRICT);
                    this.tab(DISTRICT);
                }, this)));
            }

            this.refresh();
        },
        refresh: function (force) {
            // clean the data-item for each $select            
            var $select = this.$dropdown.find('.city-select');
            $select.data('item', null);
            // parse value from value of the target $element
            var val = this.$element.val() || '';
            val = val.split('/');
            $.each(this.dems, $.proxy(function (i, type) {
                if (val[i] && i < val.length) {
                    this.options[type] = val[i];
                } else if (force) {
                    this.options[type] = '';
                }
                this.output(type);
            }, this));
            this.tab(PROVINCE);
            this.feedText();
            this.feedVal();
        },
        defineDems: function () {
            var stop = false;
            $.each([PROVINCE, CITY, DISTRICT], $.proxy(function (i, type) {
                if (!stop) {
                    this.dems.push(type);
                }
                if (type === this.options.level) {
                    stop = true;
                }
            }, this));
        },
        includeDem: function (type) {
            return $.inArray(type, this.dems) !== -1;
        },
        getPosition: function () {
            var p, h, w, s, pw;
            p = this.$element.position();
            s = this.getSize(this.$element);
            h = s.height;
            w = s.width;
            if (this.options.responsive) {
                pw = this.$element.offsetParent().width();
                if (pw) {
                    w = w / pw;
                    if (w > 0.99) {
                        w = 1;
                    }
                    w = w * 100 + '%';
                }
            }

            return {
                top: p.top || 0,
                left: p.left || 0,
                height: h,
                width: w
            };
        },
        getSize: function ($dom) {
            var $wrap, $clone, sizes;
            if (!$dom.is(':visible')) {
                $wrap = $("<div />").appendTo($("body"));
                $wrap.css({
                    "position": "absolute !important",
                    "visibility": "hidden !important",
                    "display": "block !important"
                });

                $clone = $dom.clone().appendTo($wrap);

                sizes = {
                    width: $clone.outerWidth(),
                    height: $clone.outerHeight()
                };

                $wrap.remove();
            } else {
                sizes = {
                    width: $dom.outerWidth(),
                    height: $dom.outerHeight()
                };
            }

            return sizes;
        },
        getWidthStyle: function (w, dropdown) {
            if (this.options.responsive && !$.isNumeric(w)) {
                return 'width:' + w + ';';
            } else {
                return 'width:' + (dropdown ? Math.max(320, w) : w) + 'px;';
            }
        },
        bind: function () {
            var $this = this;
            this.$textspan.on('click', function (e) {
                if (!$this.$dropdown) {
                    $this.rerender();
                }
                //这里是我改的，由于这里吧选择的dom放到了body下，所以这里在重新算边距                
                $this.$dropdown.css("left", $this.$textspan.offset().left + "px").css("top", $this.$textspan.offset().top + 3 + "px");

                var $target = $(e.target), type;

                if ($target.is('.select-item')) {
                    type = $target.data('count');
                    $this.open(type);
                } else {
                    if ($this.$dropdown.is(':visible')) {
                        $this.close(true);
                    } else {
                        $this.open();
                    }
                }
            });
        },
        open: function (type) {
            type = type || PROVINCE;
            $(".city-select.city").height(this.$dropdown.height());
            $(".city-select.district").height(this.$dropdown.height());
            this.$dropdown.show();
            this.$textspan.addClass('open').addClass('focus');
            this.tab(type);
        },
        close: function (blur) {
            if (this.$dropdown) {
                this.unbindDropDown();
                this.$dropdown.remove();
                this.$dropdown = null;
            }
            this.$textspan.removeClass('open');
            if (blur) {
                this.$textspan.removeClass('focus');
            }
        },
        //老子扩展的卸载事件
        unbindDropDown: function () {
            this.$dropdown.off('click');
            this.$dropdown.off('mousedown');
            if (this.$province) {
                this.$province.off(EVENT_CHANGE, this._changeProvince);
                delete this.$province;
            }
            if (this.$city) {
                this.$city.off(EVENT_CHANGE, this._changeCity);
                delete this.$city;
            }
        },
        getText: function () {
            var text = '';
            this.$dropdown.find('.city-select')
                .each(function () {
                    var item = $(this).data('item'),
                        type = $(this).data('count');
                    if (item) {
                        text += ($(this).hasClass('province') ? '' : '/') + '<span class="select-item" data-count="' +
                            type + '" data-code="' + item.code + '">' + item.address + '</span>';
                    }
                });
            return text;
        },
        getPlaceHolder: function () {
            return this.$element.attr('placeholder') || this.options.placeholder;
        },
        feedText: function () {
            var text = this.getText();
            if (text) {
                this.$textspan.find('>.placeholder').hide();
                this.$textspan.find('>.title').html(this.getText()).show();
            } else {
                this.$textspan.find('>.placeholder').text(this.getPlaceHolder()).show();
                this.$textspan.find('>.title').html('').hide();
            }
        },
        getVal: function () {
            var text = '';
            this.$dropdown.find('.city-select')
                .each(function () {
                    var item = $(this).data('item');
                    if (item) {
                        text += ($(this).hasClass('province') ? '' : '/') + item.address;
                    }
                });
            return text;
        },
        feedVal: function () {
            this.$element.val(this.getVal());
        },
        output: function (type) {
            var options = this.options;
            var placeholders = this.placeholders;
            var $select = this['$' + type];
            var data = type === PROVINCE ? {} : [];
            var item;
            var districts;
            var code;
            var matched = null;
            var value;
            if (!$select || !$select.length) {
                return;
            }
            item = $select.data('item');
            value = (item ? item.address : null) || options[type];
            code = (
                type === PROVINCE ? 86 :
                    type === CITY ? this.$province && this.$province.find('.active').data('code') :
                        type === DISTRICT ? this.$city && this.$city.find('.active').data('code') : code
            );
            districts = $.isNumeric(code) ? ChineseDistricts[code] : null;
            if ($.isPlainObject(districts)) {
                $.each(districts, function (code, address) {
                    var provs;
                    if (type === PROVINCE) {
                        provs = [];
                        for (var i = 0; i < address.length; i++) {
                            if (address[i].address === value) {
                                matched = {
                                    code: address[i].code,
                                    address: address[i].address
                                };
                            }
                            provs.push({
                                code: address[i].code,
                                address: address[i].address,
                                selected: address[i].address === value
                            });
                        }
                        data[code] = provs;
                    } else {
                        if (address === value) {
                            matched = {
                                code: code,
                                address: address
                            };
                        }
                        data.push({
                            code: code,
                            address: address,
                            selected: address === value
                        });
                    }
                });
            }
            $select.html(type === PROVINCE ? this.getProvinceList(data) :
                this.getList(data, type));
            $select.data('item', matched);
        },
        getProvinceList: function (data) {
            var list = [],
                $this = this;

            $.each(data, function (i, n) {
                list.push('<dl class="clearfix">');
                list.push('<dt>' + i + '</dt><dd>');
                $.each(n, function (j, m) {
                    list.push(
                        '<a' +
                        ' title="' + (m.address || '') + '"' +
                        ' data-code="' + (m.code || '') + '"' +
                        ' class="' +
                        (m.selected ? ' active' : '') +
                        '">' +
                        m.address +
                        '</a>');
                });
                list.push('</dd></dl>');
            });

            return list.join('');
        },
        getList: function (data, type) {
            var list = [],
                $this = this;
            list.push('<dl class="clearfix"><dd>');

            $.each(data, function (i, n) {
                list.push(
                    '<a' +
                    ' title="' + (n.address || '') + '"' +
                    ' data-code="' + (n.code || '') + '"' +
                    ' class="' +
                    (n.selected ? ' active' : '') +
                    '">' +
                    n.address +
                    '</a>');
            });
            list.push('</dd></dl>');

            return list.join('');
        },
        tab: function (type) {
            var $selects = this.$dropdown.find('.city-select');
            var $tabs = this.$dropdown.find('.city-select-tab > a');
            var $select = this['$' + type];
            var $tab = this.$dropdown.find('.city-select-tab > a[data-count="' + type + '"]');
            if ($select) {
                $selects.hide();
                $select.show();
                $tabs.removeClass('active');
                $tab.addClass('active');
            }
        }
    };

    CityPicker.DEFAULTS = {
        responsive: false,        
        level: 'district',
        province: '',
        city: '',
        district: ''
    };

    $.fn.citypicker = function (option) {
        var args = [].slice.call(arguments, 1);

        return this.each(function () {
            var $this = $(this);
            var data = $this.data(NAMESPACE);
            var options;
            var fn;

            if (!data) {

                options = $.extend({}, $this.data(), $.isPlainObject(option) && option);
                $this.data(NAMESPACE, (data = new CityPicker(this, options)));
            }

            if (typeof option === 'string' && $.isFunction(fn = data[option])) {
                fn.apply(data, args);
            }
        });
    };

    $.fn.citypicker.Constructor = CityPicker;

    $(function () {
        $('[data-toggle="city-picker"]').citypicker();
    });
});