//公共函数库---基于属性驱动方式--需要页面加载完毕的方法

$(function(){
//		var webFont = (function(){
//	    //移动端判断设备显示字体
//		if(judge.platform() == "ios"){
//			var str = "<style> body{ font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif!important;}</style>";
//			$('head').append(str);
//		}
//		if(judge.platform()=="android"){
//			var str = "<style>body{ font-family: 'RobotoRegular', 'Droid Sans', sans-serif!important;}</style>";
//			$('head').append(str);
//		}
//	})()
		
	
	/*点击切换事件函数*/
	
	/*首页幻灯片简便切换，按钮组*/
	//以按钮组驱动指向banner列表，data-bannerBtn=".className  banner列表DIV"
	(function click_showOtherDiv(){
		var dom=arguments[0];
		var bannerList=$(arguments[0].attr("data-bannerBtn")).children();
		var num=bannerList.length;
        bannerList.css({"position":"absolute","left":"0px","top":"0px","z-index":"1","opacity":"0.6"});
        bannerList.eq(0).css({"z-index":"10","opacity":"1"})
		var ind=stop=0;
		dom.children().click(function(){
		   
		   $(this).addClass("action").siblings().removeClass("action");
			ind=$(this).index();
			var curr=bannerList.eq(ind);
			dom.stop(true);
			bannerList.css("z-index","1");
			curr.css("z-index","10");
			curr.siblings().animate({"opacity":"0.6"},1000);
			curr.animate({"opacity":"1"},1000);
			clearInterval(stop);
			stop=setInterval(function(){
		        ind=ind%num;
		        dom.children().eq(ind).click();
		        ind++;
		   
		    },4000);
			

		})

		stop=setInterval(function(){
		        ind=ind%num;
		        dom.children().eq(ind).click();
		        ind++;
		   
		    },4000);
		    
	}($("[data-bannerBtn]")));
	


	/******选项卡特效******/
	// 核心属性名 data-switchFor=".classname", .action
	(function items_swith(){
		
		if(arguments[0].length==0)
		{
			return false;
		}
		var o=[], itemClass=null;
		var btn_array=arguments[0];
		while(btn_array.length>0)
		{
		     itemClass=btn_array.eq(0).attr("data-switchFor");

		     o.push(itemClass);
		     btn_array=btn_array.not("[data-switchFor='"+itemClass+"']");
		}
		while(o.length>0)
		{
              (function(){
		              	var name=o.pop();
		                var node=$("[data-switchFor='"+name+"']");
					   	var array_list=node.attr("data-switchFor").split("||");
						var forClass=array_list[0];
						var once=array_list[1];
						var dom =(node.length==1)?node.children():node;
						dom.click(function(e) {
		
							if($(this).parents("[data-switchFor]").length>0)
							{
								$(this).addClass("action").siblings().removeClass("action");
							}

                             var ind=dom.index($(this));

		             		
							  if($(forClass).length==1)
							  {
						
							  	var innerList=$(forClass).children();
							  	while(innerList.length==1)
							  	{
							  		var innerList=innerList.children();
							  	}
							  
							  	$(forClass).show();
						  	    innerList.hide();
							  	innerList.eq(ind).show();
							  	if(innerList.find(".close_btn").length>0)
							  	{
							  		innerList.find(".close_btn").click(function(){
							  			$(this).parents(forClass).hide();
					
							  		})

							  	}
							  }
							  else if($(forClass).length==dom.length)
							  {
							  	
							  	 $(forClass).eq(ind).show().siblings().hide();
							  	
							  }	  
						})
				      if(once!="no")
				      {
				      	 dom.eq(0).click();	
				      }
              }())
		}

	})($("[data-switchFor]"));
	
	/*显示--隐藏关联dome操作*/
	// 核心属性名 data-clickshowFor=".classname"
	(function click_showOtherDiv(){
		var dom=arguments[0];
		dom.click(function(){
		

	
			var fordom=$(this).attr("data-clickshowFor");
		
			$(fordom).show().find(".close_btn").click(function(){
				$(this).parents(fordom).hide();
			})
	
		})	
	}($("[data-clickshowFor]")));

	/*表格全选框特效*/
	// 核心属性名 data-selectAll=".classname"
		(function click_selectAll(){
		var dom=arguments[0];
		dom.click(function(){
			var table=$(this).attr("data-selectAll");
		
			
			
			if($(this).prop("checked"))
			{
				$("[data-selectAll='"+table+"']").prop("checked",$(this).prop("checked")).prop("data-checked","true");
				$("table"+table).find("tr").each(function(){
			
					$(this).find("*:first input[type=checkbox]").prop("checked",true).prop("data-checked","true");
					
				})
				
			}
			else {
			   
				$("[data-selectAll='"+table+"']").prop("checked",$(this).prop("checked")).removeAttr("data-checked");
				$("table"+table).find("tr").each(function(){
					$(this).find("*:first input[type=checkbox]").prop("checked",false).removeAttr("data-checked");
					
				})

			}
		
			$(table).find("tr  input[type=checkbox]").click(function(){
		
			
				if(dom.prop("checked"))
				{
					
					dom.prop("checked",false).removeAttr("data-checked");
				}
				
				
				
			})
			
			

		})	
	}($("[data-selectAll]")));
	
	//全选效果 二
	 function checked_all(dom){

	 	dom.each(function(){
	 		var all_btn=$(this);
	 		var input_all=$(this).attr("data-selectAll2");
	 		
	 		if($(input_all)[0].nodeName=="input")
	 		{ 
	 			
	 			$(this).click(function(){	
		 			if(!$(this).prop("checked"))
		 			{
		 				$(input_all).prop("checked",false)	
		 		
		 			}
		 			else{
		 				$(input_all).prop("checked",true)
		 		
		 				
		 			}	
	 			
	 		    });
	 			$(input_all).click(function(){
	 			
		 			if(input_all.length==$(input_all+" input:checked").length)
		 			{
		 				all_btn.prop("checked",true)
		 				
		 			}
		 			else{
		 				
		 				all_btn.prop("checked",true)
		 			}
		 		})

	 		}
	 		else{
	 			$(this).click(function(){
	 				
	 			   console.log($(input_all).find("input").length);
		 			if(!$(this).prop("checked"))
		 			{
		 				$(input_all).find("input").prop("checked",false)	
		 			}
		 			else{
		 				$(input_all).find("input").prop("checked",true)
		 			}	
	 			
	 		   });
	 			$(input_all).find("input").click(function(){
	 		
	 				if($(input_all).length==$(input_all+" input:checked").length)
		 			{
		 				all_btn.prop("checked",true)
		 			}
		 			else{
		 				all_btn.prop("checked",false)
		 			}

	 				
	 			})
	
	 		}

	 		
	 		
	 	})

	 };
	
	 //可以用于ajax回调
//	checked_all($("[data-selectAll2]"));
	
	
/*倒计时封装*/
function downTime(time){
	//if(typeof end_time == "string")
	var end_time = new Date(time).getTime(),//月份是实际月份-1
	//current_time = new Date().getTime(),
	sys_second = (end_time-new Date().getTime())/1000;
		if (sys_second > 0) {
		
			sys_second -= 1;
			var day = Math.floor((sys_second / 3600) / 24);
			var hour = Math.floor((sys_second / 3600) % 24);
			var minute = Math.floor((sys_second / 60) % 60);
			var second = Math.floor(sys_second % 60);
			return {"date_day":day,"date_hours":(hour<10?"0"+hour:hour),"data_minute":(minute<10?"0"+minute:minute),"date_second":(second<10?"0"+second:second)};
		}
}

  //倒计时dom渲染,时间显示元素.days ,.hours,.minute,.second类
  (function show_downTime(dom){
  	dom.each(function(){
  
  		var time=$(this).find("input").val();
  		var sys_second;
  		var that=$(this);
  		var stop=setInterval(function(){
  	
  			if(sys_second<=0)
  			{clearInterval(stop)}
  			else{			
  			   var down_time=downTime(time);
			   that.find(".days").text(down_time["date_day"]+"天");
  			   that.find(".hours").text(down_time["date_hours"]);
  			   that.find(".minute").text(down_time["date_minute"]);
  			   that.find(".second").text(down_time["date_second"]);
	
  			}

  		},1000)
	
  	})	
  })($(".down_time"));
	
	
	

	/*购物框物品数量增减*/
	// 核心属性名 .setNum 类
		(function click_setNum(){
		var dom=arguments[0];
		dom.click(function(){
			var val=parseInt($(this).siblings("input").val());
			var ind=$(this).parent().find(dom).index($(this));
			if(ind==0)
			{
				$(this).siblings("input").val(val>0?--val:0);

			}
			else{
				var max=$(this).siblings("input").attr("max");
				
				if(max)
				{
					
					console.log(val);
					$(this).siblings("input").val(++val<max?val:max);
				}
				else{
					$(this).siblings("input").val(++val);
					
				}
				
				
			}

		})	
	}($(".setNum")));
	/*点击切换*/
	//核心属性 data-clickSwidth="className 切换当前活动类名"
	(function click_clickSwidth(){
		
       var dom=arguments[0];
  
		dom.click(function(e){
			
			
			var active_class=$(this).attr("data-clickSwidth");
			var node_tag=$(e.target);
			while(node_tag.siblings(node_tag[0].nodeName).length<1)
			{
				node_tag=node_tag.parent();
				
			}
		
			
		node_tag.has($(e.target)).addClass(active_class).siblings().removeClass(active_class);
		})	
	}($("[data-clickSwidth]")));
	/*图片列表横排左右按钮LOOP滚动*/
	//核心属性  左右按钮 data-RunShow=".className 图片列表外"

    /*input iE8不支持:checked 处理方案*/
   	(function set_checked(){
		
       var dom=arguments[0];
  
		dom.click(function(e){
		
			if($(this).find("input[type=checkbox]").prop("checked"))
			{
				$(this).find("input").attr("data-checked","true")
				
				
			}
			else{
				$(this).find("input").removeAttr("data-checked")
				
			}
			

		})	
	}($("[data-checked]")));
	

	
//验证邮箱
function checkEmail(str){
   return str.match(/[A-Za-z0-9_-]+[@](\\S*)(net|com|cn|org|cc|tv|[0-9]{1,3})(\\S*)/g);

}
//验证手机
function checkMobilePhone(str){

    return str.match(/^1[358][0-9]{9}$/);
}

	
	/*ul li结构左右按钮无缝滚动*/
	/*核心属性 data-RunNoStop=".className||num 左右按钮类名||一屏数量"*/
	(function run_noStop(){
       var dom=arguments[0];
		dom.each(function(){
			var num=$(this).attr("data-RunNoStop").split("||")[1];
			var li_num=$(this).find("ul li").length;
			if(li_num<=num)
			{
				return;	
			}
			var Ul=$(this).find("ul");
			var className=$(this).attr("data-RunNoStop").split("||")[0];
			var wid=Ul.find("li").outerWidth();
		    $(this).width(num*wid);
			var list=Ul.find("li").clone();
			Ul.append(list.clone());
			Ul.append(list.clone());
			
			Ul.width(li_num*3*wid);
			Ul.css("position","relative").css("left",-li_num*wid+"px");
			var that=$(this);
			var speed=1;
			$(this).siblings(className).click(function(){
				var ind=that.siblings(className).index($(this));
                speed=(ind==0?1:-1);	
              
			})
			
			var stop=setInterval(function(){
				
				var position=parseInt(Ul.css("left"));
				console.log(position);
			
				if(position>=0||position<=-li_num*2*wid)
				{
				
					Ul.css("left",-li_num*wid+"px");
					position=parseInt(Ul.css("left"));
				}
			
				Ul.css("left",(position+=speed)+"px");
				

			},30);
			Ul.mouseover(function(){
				clearInterval(stop);
			    }).mouseout(function(){
			    		stop=setInterval(function(){
						var position=parseInt(Ul.css("left"));
						console.log(position);
					
						if(position>=0||position<=-li_num*2*wid)
						{
							Ul.css("left",-li_num*wid+"px");
							position=parseInt(Ul.css("left"));
						}
						Ul.css("left",(position+=speed)+"px");
					},30);

			    })
		})
	}($("[data-RunNoStop]")));
	

		
		/*分页 特效*/
		/*分页器初始化*/
			var fenye_option={
				"total":155,
				"target":2,
				"callback":function(index){
					console.log(index);
				},
				"start":function(){
					showPageCommon(this);	
				}
			  };
	    fenye_option.start();
	    function showPageCommon(fenye_option) {
		   if(!fenye_option)return;
		  var  page=fenye_option.target;
		  var  total=fenye_option.total;
		  var  callback=fenye_option.callback;
		
		    var add_html=function(page,total){
		    	   var str = '<a class="color-red" id="js_nowpage">' + page + '</a>';
			        for (var i = 1; i <= 3; i++) {
			            if (page - i > 1) {
			                str = '<a >' + (page - i) + '</a> ' + str;
			            }
			            if (page + i < total) {
			                str = str + ' ' +'<a >' + (page + i)+ '</a> ';
			            }
			        }
			        if (page - 4 > 1) {
			            str = '... ' + str;
			        }
			
			        if (page > 1) {
			            str = '<span class="js_last">'+'上一页 '+'</span>' + '<a >'+1 + '</a> '+ ' ' + str;
			        }
			
			        if (page + 4 < total) {
			            str = str + ' ...';
			        }
			
			        if (page < total) {
			            str =str + ' ' + '<a >' + total+ '</a> ' + '<span class="js_next">'+'下一页 '+'</span>';
			        }
		    	
	           $(".fenye").html(str);
		    };
		    add_html(page,total);
			$('body').on('click','.fenye a',function(){
			    page= parseInt($(this).text());

			    $(this).parents('.fenye').empty(); 
			    add_html(page,total);
                callback&&callback(page);	
			   
			   
			});
			$('body').on('click','.js_last',function(){
			    var page = parseInt($('#js_nowpage').text());
			
			   $(this).parents(".fenye").empty();
			    add_html(page-1<1?1:page-1, total);
			    callback&&callback(page-1);
			});
			$('body').on('click','.js_next',function(){
			    var page = parseInt($('#js_nowpage').text());
			    $(this).parents(".fenye").empty();
			    add_html(page+1>total?total:page+1, total);
			    callback&&callback(page+1);
			});
			
			    
		};
		/*select 下拉样式美化*/
        (function(){
			$("select.beautify").each(function(){
			$(this).width($(this).width()+20);
	        var the_wid=$(this).outerWidth()-2;
	        var hgt=$(this).outerHeight()
	        var bg_color=$(this).css("background-color");
	    	$(this).wrap("<span style='width:"+the_wid+"px;height:"+hgt+"px;background-color:"+bg_color+";display:inline-block;position:relative'></span>");
	    	var border_color=$(this).css("border-left-color");
	    	$(this).css({"border":"0","outline":"none","opacity":"0","vertical-align":"top"}).css({"position":"absolute","left":"-1px","top":"1px","z-index":"2"});
	    	$(this).parents("span").css("border","1px solid "+border_color);
	    	$(this).parents("span").append("<em style='display: inline-block;width:100%;height: 100%;line-height: "+hgt+"px;left:0px;background-color: inherit;font-style:normal;padding-right:20px;box-sizing:border-box;overflow:hidden;padding-left:5px;word-break:break-all'>"+$(this).find("option:first").text()+"</em>")
	        $(this).parents("span").append("<i style='position: absolute;right:5px;top:50%; margin-top: -4px;border-top:8px solid red;border-color: inherit;border-left: 4px solid transparent;border-right: 4px solid transparent;display:inline-block;width:0;height:0'></i>")
	        $(this).change(function(){
	        	$(this).siblings("em").text($(this).find("option:selected").text())
	          })
			})
	    })();
	    
	    /*底部滑出层对齐,需要对齐的项目类名为‘center_align’*/
	   (function cneter_align(dom){
	   	  dom.each(function(){
	   	  	var max_wid=0;
	   	  	$(this).find(".center_align").each(function(){
	   	  		if($(this).width()>max_wid)max_wid=$(this).outerWidth();
	   	  	});
	   	  	$(this).find(".center_align").css("margin-left",-max_wid/2+"px")

	   	  })

	   })($(".float_div"));
	   
	   /*产品进度条显示*/
	  (function progress_bar(dom){
	  	 if(dom.length==0)return;
	  	 dom.each(function(){
	  	 	var wid=$(this).find("input").val()+"%";
	  	 	$(this).children().css("width",wid)
	  	 	
	  	 })
	  })($(".progress_bar .bar"));
	   
       /*上传图片功能，3张或者更多*/
       /*上传图片初始化，3张*/
        (function up_loadfile(){
             /*上传图片初始化，多张*/
        	if($(".up_img_more").length>1)
	        {
	         	 
	         	   new uploadPreview({ UpBtn:$(".up_img_more")});
	         	   $(".up_img_more").change(function(){
	         	   	    $(this).parents("label").addClass("imgfile_show");
	         	   	    $(this).siblings("label").click(function(){
         	   	    	$(this).parents("label").removeClass("imgfile_show").appendTo($(this).parents("label").parent());
         	   	    	$(this).siblings("img").prop("src","img/slice/add.png");
         	   	    	$("body").append("<form id='clear_erea'></form>");
         	   	    	$(this).siblings("input[type=flie]").appendTo($("#clear_erea"))
 						$("#clear_erea")[0].reset();
                        $("#clear_erea").find("input").appendTo($(this).parents());
                        $("#clear_erea").remove();	
	         	   	    })

	         	   })

	        }     
		     else if($(".up_img_more").length==1)
		     {
		     	 new uploadPreview({ UpBtn:$(".up_img_more") });
		     }
        	
        })()
      
 
		

})

/*不需啊哟页面加载的自定义函数*/
/*alert ,confirm 封装*/

   var go_alert = function (msg,url) {
        $("body").append("<div class='alert_box' style='position:fixed;z-index:1500;width:100%;opacity:1;height:100%;left:0;top:0px;background-color:rgba(68,68,68,0.4);'><div class='content' style='width:260px;text-align:center;position:absolute;font-size:14px;left:50%;top:50%; transform:translateY(-50%) translateX(-50%);-webkit-transform:translateY(-50%) translateX(-50%);background-color:#fff;border-radius:10px'><P style='padding:20px 25px; line-height:25px; border-bottom:1px solid #ccc;color:#333;font-size:14px'>" + msg + "<P><button style='margin:10px 0px; background-color:#fff;border:0;display:inline_block;width:100%;;color:#df3033;font-size:14px;outline:none'>确定</button><div><div>");
        $(".alert_box .content button").click(function () {
            var that = $(this);
            setTimeout(function () {
                that.parents(".alert_box").css({"opacity":"0","width":"0%","height":"0"})
                setTimeout(function(){that.parents(".alert_box").remove()},300);
                if(url!=undefined&&url!=" ")window.location.href=url;
            }, 500)

        })
    };
   var go_alert2 = function (msg,url) {
        $("body").append("<div class='alert_box' style='position:fixed;z-index:1500;width:100%;opacity:1;height:100%;left:0;top:0px;background-color:rgba(68,68,68,0.4);'><div class='content' style='width:260px;text-align:center;position:absolute;font-size:14px;left:50%;top:50%; transform:translateY(-50%) translateX(-50%);-webkit-transform:translateY(-50%) translateX(-50%);background-color:#fff;border-radius:10px'><P style='padding:20px 25px; line-height:25px; border-bottom:1px solid #ccc;color:#333;font-size:14px'>" + msg + "<P><div><div>");
        setTimeout(function () {
                $(".alert_box").css({"opacity":"0","width":"0%","height":"0"})
                setTimeout(function(){$(".alert_box").remove()},300);
                if(url!=undefined&&url!=" ")window.location.href=url;
            }, 2000)

    };
   var go_confirm=function(msg,func){
        $("body").append("<div class='confirm_box' style='position:fixed;z-index:1500;width:100%;opacity:1;height:100%;left:0;top:0px;background-color:rgba(68,68,68,0.4);'><div class='content' style='width:260px;text-align:center;position:absolute;font-size:14px;left:50%;top:50%; transform:translateY(-50%) translateX(-50%);-webkit-transform:translateY(-50%) translateX(-50%);background-color:#fff;border-radius:10px'><P style='padding:20px 35px; line-height:25px; border-bottom:1px solid #ccc;color:#999;font-size:16px'>" + msg + "<P><button style='margin:10px 0px; background-color:#fff;border:0;display:inline_block;width:48%;;color:#999;font-size:16px;outline:none'>确定</button><button style='margin:10px 0px; background-color:#fff;border:0;display:inline_block;width:48%;;color:#999;font-size:16px;outline:none;border-left:1px solid #ccc'>取消</button><div><div>");
        $(".confirm_box .content button").click(function () {

            var that = $(this);
            setTimeout(function () {
                that.parents(".confirm_box").css({"opacity":"0","width":"0%","height":"0"});
                setTimeout(function(){that.parents(".confirm_box").remove()},300);
            }, 500)
            if($(this).text()=="确定")
            {
            
            	func&&func();
            }
        })
   	
   	
   	
   }

   
$(function(){
	
/*弹出框按钮绑定*/
   (function(btn){
		btn.each(function(){
				var msg=$(this).attr("go_alert");

				$(this).click(function(){
				       go_alert(msg);
				})
		})

     })($("[go_alert]"));
    
    (function(btn){
		btn.each(function(){
				var msg=$(this).attr("go_confirm");
				$(this).click(function(){
				       go_confirm(msg);
				})
		})

     })($("[go_confirm]"));
     
     

})
   
   
   
     /*发送验证码，60秒等待效果*/
   //核心属性  .identify_btn  针对input[type=button],button; func发送验证码回调函数,val值为电话号码
   	function click_identify(dom,func,val){
	   var downTime=60;
       dom.click(function(){
       	  $(this).attr("disabled",true);
       	    func&&func(val);
       	     
			if($(this)[0].nodeName=="INPUT")
			{
			   
				var stop=setInterval(function(){
					
					if(downTime>0)
					{
						$(this).attr("disabled",true);
						$(this).val((--downTime)+"秒后获取");
					}
					else{
						clearInterval(stop);
						$(this).attr("disabled",false);
						that.text("发送验证码");
						downTime=60;
					}
	
				},1000)

			}
			else {
			 
				$(this).text((--downTime)+"秒后获取");
				var that=$(this);
				var stop=setInterval(function(){

					if(downTime>0)
					{
						that.attr("disabled",true);
						that.text((--downTime)+"秒后获取");	
					}
					else{
						clearInterval(stop);
						that.attr("disabled",false)
						that.text("发送验证码");
						downTime=60;
					}
				},1000)
			}
		})

	};

/*验证回调发送验证码  2种验证类型tel,id_number;func 发送验证码回调函数*/
   var identify_form=function(dom,func){
   	
   	   $(".identify_btn").click(function(){
   	   	
   	   	  go_alert("请输入手机号")

   	   })
   	   dom.change(function(){
   	   		
   	   	  switch($(this).attr("data-identify"))
   	   	  {
   	   	  	case "tel":
   	   	  	var tel_reg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/; 
   	   	  	if($(this).val()!=" "&&tel_reg.test($(this).val()))
   	   	  	{
   	   	  		click_identify($(".identify_btn"),func,$(this).val())
   	   	  	}
   	   	  	else{
   	   	  		go_alert("请输入正确的手机号");
   	   	  		$(".identify_btn").unbind("click");
   	   	  		
   	   	  		
   	   	  	}

   	   	  	break;

   	   	  }
	
   	   })
   	    dom.blur(function(){
   	    	$(".identify_btn").unbind("click");
   	    	dom.trigger("change");
   	    	
   	    })
   	   

   };


/*评星效果*/
	var set_stars=function(stars_box,img_url,stars_num,img_wid,style){   
		var img_json=arguments[1];
		var img_wid=(img_wid==undefined)?"":img_wid;
		stars_box.each(function(){
			
			 
		   var stars_number=stars_num;
		   var num = $(this).find("input[type=hidden]").val();

		    //如果之前有img,则将img清空
		   if ($(this).find("img").length > 0) $($(this).find("img")).remove();

		  $(this).append("<img class='star' src='"+img_json.filled+"' width='"+img_wid+"' style='display:inline-block;margin-right:5px'/>")
		   var wid=$(this).find("img:first").width();
		   var hgt=$(this).find("img:first").height();
		   while(--stars_number)
		   {
		   	  $(this).append($(this).find("img:last").clone());
		   };
		   if($(this).find("input[type=hidden]").val()>0)
		   {
		 
		   	 $(this).find("img.star:gt("+($(this).find("input[type=hidden]").val()-1)+")").attr("src",img_json.empty);
		   }
		   else{
		   	 $(this).find("img.star").attr("src",img_json.empty); 	
		   }
		  
		   if(style=="onlyread")return;
		   $(this).find("img.star").click(function(){
		
		   	var value=$(this).siblings("input[type=hidden]").val();
	
		   	var ind=$(this).parent().find("img.star").index($(this));
		   
		
		   	  if($(this).attr("src")==img_json.filled)
		   	  {
		   	     
		   	    if($(this).next("[src='"+img_json.filled+"']").length)
		   	    {
		   	        $(this).parent().find("img.star").attr("src",img_json.empty)
		   	    	$(this).parent().find("img.star:lt("+(ind+1)+")").attr("src",img_json.filled);
		   	    	
		   	    }else{
		   	    	$(this).parent().find("img.star").attr("src",img_json.empty)
		   	    	$(this).parent().find("img.star:lt("+ind+")").attr("src",img_json.filled);
		   	    	
		   	    }
		   	    
		   	  }else{
		   	  	 $(this).parent().find("img.star").attr("src",img_json.filled)
		   	     $(this).parent().find("img.star:gt("+ind+")").attr("src",img_json.empty);
		   	  
		   	  }
		   	  var val=$(this).parent().find("img.star[src='"+img_json.filled+"']").length;
		   	 $(this).parent().find("input[type=hidden]").val(val);
		   })
			
			
		});
			
	};
//调用方法
//	var img_url={filled:"img/pingjia1.png",empty:"img/pingjia2.png"};
//	set_stars($(".set_stars"),img_url,5,25,"onlyread")

/*多行文本超出隐藏,并显示省略号*/
	function more_txtHide(dom){
	 if(!dom.length)return;
	   dom.each(function(){
		     var txt=$(this).text();
		     var len=txt.length;
		     var hgt=$(this).height();
		     $(this).css("max-height","none");
		     if($(this).height()>hgt)
		     {
		     	while($(this).height()>hgt)
		     	{
		     		$(this).empty().text(txt.slice(0,--len));
		     		$(this).append($("<i>...</i>"));
		     	}
 	
		     }

	   })
	};
	/*此处可用于ajax 回调*/
	more_txtHide($(".txt_ellipsis"));

/*对付ajax 异步生成dom 的轮询处理方案,dom节点检测，回调func,ary参数数组*/
function do_ajax(dom,func,ary){
		var num=0;
		var time=10;
		var span=10;
		var no_add=20;
		var stop=setInterval(function(){
			if(dom.children()>num)
			{
				num=dom.children().length;
				span=time;
		
				
			}else if(no_add&&dom.children().length==num){
				
				no_add--;
				
			}else if(dom.children().length&&no_add==0){
				func(ary.join(","))
			}
			time+=10;
	
		},span)
	
	
	
}

/*实际调用的*/

do_ajax($($("[data-selectAll2]").attr("data-selectAll2")))







function choiseAll() {
    //            debugger;
    var arr = $(".choiseChangeBox");
    for (var i = 0; i < arr.length; i++) {
        console.info(arr.eq(i).hasClass("checkedBox"))
        if (arr.eq(i).hasClass("checkedBox") && i > 0) {
            console.info(i)
            break;
        }
    }
    console.info(i);
    if (i != arr.length) {
        arr.eq(0).addClass("checkedBox")
    } else {
        //                alert(1)
        arr.eq(0).removeClass("checkedBox")
    }
}




















