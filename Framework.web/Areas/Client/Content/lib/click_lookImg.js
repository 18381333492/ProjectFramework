/*swiper模式点击图片缩放浏览*/
/*二种模式：（一），列表模式*/
$(function () {
 
    if ($(".swiper_clickLook").length || $(".edit_area").length) {
        var img_clickShow1 = function () {
            if ($(".swiper_clickLook").length) {
                $('body').append('<div class="detail_look" swiper_clickLook><em><b></b>/<i></i></em><div class="swiper-container"><div class="swiper-wrapper"></div></div><span class="close_btn" style=""><button href="javascript:" class="fa fa-times-circle-o color-fff font30" ></button></span></div>');
                $(".swiper_clickLook").each(function () {
                    var dom_box = $(this);
                    dom_box.click(function (e) {
                        if ($(this).find("img").length == 0) return;
                        var dom = $(this).children();
                        while (dom.length != $(this).find("img").length) {
                            dom = dom.children()
                        }

                        $(".swiper-container .swiper-wrapper").empty();
                        if (dom.has($(e.target)).length) {
                            var ind = dom.has($(e.target)).index();

                        } else {
                            var ind = $(e.target).index();

                        }

                        var len = dom.length;
                        dom.each(function () {
                            if ($(this).find("img").length) {
                                var src = $(this).find("img").prop("src");
                            } else {
                                var src = $(this).prop("src");
                            }
                            $(".swiper-container .swiper-wrapper").append('<div class="swiper-slide"><div class="look_box"><img src="' + src + '"/></div></div>')
                        });

                        $(".detail_look[swiper_clickLook] >em i").text(len);
                        $(".detail_look[swiper_clickLook]").show();
                        var mySwiper = new Swiper('.swiper-container', {
                            initialSlide: ind,
                            loop: true,
                            onInit: function () {
                                $(".detail_look[swiper_clickLook] >em b").text(ind + 1);

                            },
                            onSlideChangeEnd: function (mySwiper) {
                                $(".detail_look[swiper_clickLook] >em b").text(mySwiper.activeIndex == "0" ? len : (mySwiper.activeIndex == len + 1 ? 1 : mySwiper.activeIndex));
                            }



                        })

                        $('.look_box').each(function () {
                            new RTP.PinchZoom($(this), {});
                        });

                    });
                    $(".detail_look[swiper_clickLook] .close_btn button").click(function () {
                        $(".detail_look[swiper_clickLook]").hide();

                    })

                })

            }

        };

        /*二种模式：（二），富文本编辑区模式*/
        var img_clickShow2 = function () {
            $('body').append('<div class="detail_look" edit_area><em><b>2</b>/<i>4</i></em><div class="swiper-container2"><div class="swiper-wrapper"></div></div><span class="close_btn" style=""><button href="javascript:" class="fa fa-times-circle-o color-fff font30 back-999" ></button></span></div>');
            setTimeout(function () {
                var dom = $(".edit_area").find("img");
                var offsetTop;
                dom.click(function () {
                    offsetTop = $(window).scrollTop();
                    $("body").css("position", "fixed").css("top", -offsetTop + "px");
                    $(".swiper-container2 .swiper-wrapper").empty();
                    var ind = dom.index($(this));
                    var len = dom.length;
                    $(".detail_look[edit_area] >em i").text(len);
                    dom.each(function () {
                        var src = $(this).prop("src");
                        $(".swiper-container2 .swiper-wrapper").append('<div class="swiper-slide"><div class="look_box"><img src="' + src + '"/></div></div>')
                    });

                    $(".detail_look[edit_area]").show();
                    var mySwiper = new Swiper('.swiper-container2', {
                        initialSlide: ind,
                        loop: true,
                        onSlideChangeEnd: function (mySwiper) {
                            $(".detail_look[edit_area] >em b").text(mySwiper.activeIndex == "0" ? len : (mySwiper.activeIndex == len + 1 ? 1 : mySwiper.activeIndex));
                        },

                    })
                    $('.look_box').each(function () {
                        new RTP.PinchZoom($(this), {});
                    });

                });
                $(".detail_look[edit_area] .close_btn button").click(function () {
                    $("body").css("position", "initial").scrollTop(offsetTop);
                    $(".detail_look[edit_area]").hide();

                })

            }, 1000)

        };

        img_clickShow1();
        img_clickShow2();
    }
    //debugger
})