$(function(){
	/*footer的切换*/
	$(".footerClick").children("p").click(function(){
    			$(this).addClass('active').parent().siblings().find("p").removeClass('active');
    		});
	/*五个图标下面蓝条来回切换效果*/
	$(".w-flex5").click(function(){
		$(this).children("b").addClass("w-flex5Bactive");
		$(this).siblings().find("b").removeClass('w-flex5Bactive');
	});
	/*mask提示框点击关闭*/
	/*button点击后出现提示框后body禁止滚动*/
	$(".w-maskClickclose").click(function(){
		$(".w-maskbg").hide();
		$('body').css('overflow','auto');
	});
	$(".w-btnprompt").click(function(){
		$(".w-maskbg").show();
		$('body').css('overflow','hidden');
	});
	
	/*header栏右面的小标签的显示与隐藏*/
	$(".w-righticon").click(function(){
          $(this).children("ul").toggle();
	});
	/*header二级菜单*/
	$(".w-allclass").click(function(){
		$(".w-hidenwarp").toggle();
	});
	$(".w-hidebox").click(function(){
		$(this).children(".w-hidebox2").show();
		$(this).css("background-color","#ccc");
		$(this).siblings(".w-hidebox").css("background-color","#fff");
		$(this).siblings(".w-hidebox").children(".w-hidebox2").hide();
	});
	/*底部栏占位高度*/
	
	$(".footer_erea .place_holder").each(function(){
		$(this).parents(".footer_erea").height($(this).height());
	})
	
	/**/
	
	
    /*回到顶部*/
   $(".back_top").click(function(){
   	   var curr=$(window).scrollTop();
   	   var span=Math.ceil(curr/20);
   	   var stop=setInterval(function(){
   	   	   curr-=span;
   	   	   if(curr<=0)
   	   	   {$(window).scrollTop(0);
   	   	   	clearInterval(stop)}
   	   	   else{
   	   	   	$(window).scrollTop(curr);
   	   	   }
   	   	
   	   },10);
   	  $(window).scrollTop(0);
   });
   $(window).scroll(function(){
   	  var hgt=$(this).height();
   	  if($(this).scrollTop()>=hgt)
   	  {
   	  	$(".back_top").css("opacity","1")
   	  	
   	  }else{
   	  	$(".back_top").css("opacity","0")
   	  }

   })
   
		/*swiper模式点击图片缩放浏览*/
		/*二种模式：（一），列表模式*/

//		if ($(".swiper_clickLook").length || $(".edit_area").length)
//		{
//        return false;
//        $.getScript("../lib/pinchzoom.min.js",function(){
//			var img_clickShow1 = function () {
//				setTimeout(function () {
//					if ($(".swiper_clickLook").length) {
//
//
//						$('body').append('<div class="detail_look" swiper_clickLook><em><b></b>/<i></i></em><div class="swiper-container"><div class="swiper-wrapper"></div></div><span class="close_btn" style=""><button href="javascript:" class="fa fa-times-circle-o color-fff font30 back-999" ></button></span></div>');
//						$(".swiper_clickLook").each(function () {
//							var dom = $(this).children();
//							while (dom.length == 1) {
//								dom = dom.children()
//							}
//							dom.click(function () {
//				
//
//								$(".swiper-container .swiper-wrapper").empty();
//								var ind = $(this).index();
//								var len = $(this).parent().children().length;
//								dom.each(function () {
//									var src = $(this).find("img").prop("src");
//									$(".swiper-container .swiper-wrapper").append('<div class="swiper-slide"><div class="look_box"><img src="' + src + '"/></div></div>')
//								});
//								$(".detail_look[swiper_clickLook] >em i").text(len);
//								$(".detail_look[swiper_clickLook]").show();
//								var mySwiper = new Swiper('.swiper-container', {
//									initialSlide: ind,
//									loop: true,
//									onSlideChangeEnd: function (mySwiper) {
//										$(".detail_look[swiper_clickLook] >em b").text(mySwiper.activeIndex == "0" ? len : (mySwiper.activeIndex == len + 1 ? 1 : mySwiper.activeIndex));
//									}
//
//
//
//								})
//								$('.look_box').each(function () {
//									new RTP.PinchZoom($(this), {});
//								});
//
//							});
//							$(".detail_look[swiper_clickLook] .close_btn button").click(function () {
//								$(".detail_look[swiper_clickLook]").hide();
//
//							})
//
//						})
//
//					}
//
//
//
//				}, 1000)
//
//
//
//			};
// 			img_clickShow1();
//			/*二种模式：（二），富文本编辑区模式*/
//			var img_clickShow2 = function () {
//				$('body').append('<div class="detail_look" edit_area><em><b>2</b>/<i>4</i></em><div class="swiper-container2"><div class="swiper-wrapper"></div></div><span class="close_btn" style=""><button href="javascript:" class="fa fa-times-circle-o color-fff font30 back-999" ></button></span></div>');
//				setTimeout(function () {
//					var dom = $(".edit_area").find("img");
//					var offsetTop;
//					dom.click(function () {
//						offsetTop = $(window).scrollTop();
//						$("body").css("position", "fixed").css("top", -offsetTop + "px");
//						$(".swiper-container2 .swiper-wrapper").empty();
//						var ind = dom.index($(this));
//						var len = dom.length;
//						$(".detail_look[edit_area] >em i").text(len);
//						dom.each(function () {
//							var src = $(this).prop("src");
//							$(".swiper-container2 .swiper-wrapper").append('<div class="swiper-slide"><div class="look_box"><img src="' + src + '"/></div></div>')
//						});
//
//						$(".detail_look[edit_area]").show();
//						var mySwiper = new Swiper('.swiper-container2', {
//							initialSlide: ind,
//							loop: true,
//							onSlideChangeEnd: function (mySwiper) {
//								$(".detail_look[edit_area] >em b").text(mySwiper.activeIndex == "0" ? len : (mySwiper.activeIndex == len + 1 ? 1 : mySwiper.activeIndex));
//							},
//
//						})
//						$('.look_box').each(function () {
//							new RTP.PinchZoom($(this), {});
//						});
//
//					});
//					$(".detail_look[edit_area] .close_btn button").click(function () {
//						$("body").css("position", "initial").scrollTop(offsetTop);
//						$(".detail_look[edit_area]").hide();
//
//					})
//
//				}, 1000)
//
//			};
//
//			img_clickShow2();
//       })
//    }
		
})
/*商品购物车全选*/
//var checkedonload=function(){
//	var labelQuan=$(".w-label15");
//	var labelQuaninput=$(".w-label15 input");
//	var labelFen=$(".w-chenkboxfen");
//	var labelFeninput=$(".w-chenkboxfen input");
//	var close=$(".w-close");
//	labelFeninput.bind('click',function(){
//		if(hasNochecked()){
//			labelQuaninput.prop('checked',false);
//		}else{
//			labelQuaninput.prop('checked',true);
//		}
//		if($(".w-chenkboxfen input[type=checkbox]:checked").length>0){
//			close.css('color','#f00');
//			close.css('background-image','url("img/slice/closeon.png")');
//		}else{
//			close.css('color','#868686');
//			close.css('background-image','url("img/slice/dustbin.png")');
//		};
//	});
//	labelQuaninput.bind('click',function(){
//		if (!$(this).prop('checked')){
//			labelFeninput.prop('checked',false);
//			close.css('color','#868686');
//			close.css('background-image','url("img/slice/dustbin.png")');
//		}else{
//			labelFeninput.prop('checked',true);
//			close.css('color','#f00');
//			close.css('background-image','url("img/slice/closeon.png")');
//		}
//	});
//	function checkedNum(){
//		return $(".w-chenkboxfen").length - $(".w-chenkboxfen input[type=checkbox]:checked").length;
//	};
//	function hasNochecked(){
//		return checkedNum() > 0 ? true : false;
//	};
//};
//checkedonload ();
























