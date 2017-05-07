/**
 * Created by Administrator on 2016/10/27.
 */
$(function(){
    /*优惠券切换*/
    $("#coupons .title a").click(function(){
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
    });
    $("#coupons .title a:nth-of-type(1)").click(function(){
        $(".available").css("display","block");
        $(".expired").css("display","none");
    });
    $("#coupons .title a:nth-of-type(2)").click(function(){
        $(".available").css("display","none");
        $(".expired").css("display","block");
    });
    /*标题切换*/
    $("#myvip .title a").click(function(){
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
    });
    $("#share-store .head .title a").click(function(){
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
    });
    $("#order-list .title a").click(function(){
        $(this).addClass("active");
        $(this).siblings().removeClass("active");
    });
    /*input的value值改变*/
    var a
    $(".ipt").focus(function(){
        a= $(".ipt").val()
        if(a=="请输入标题，最多20字。"){
            $(this).val("")
        }else{
            $(this).val(a)
        }

    });
    $(".ipt").blur(function(){
        a= $(".ipt").val()
        if(a==0){
            $(this).val("请输入标题，最多20字。")
        }else{
            $(this).val(a)
        }
    });
    /*弹窗*/
    $(".mask .title button").click(function(){
        $(".mask").hide()
    })
    $(".w-youhuiBox .w-youhui").click(function(){
        $(".mask").show()
    })
});