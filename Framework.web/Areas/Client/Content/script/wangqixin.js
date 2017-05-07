$(function(){
    /*60s计时器*/
   
    $('.w-si').click(function(){
        $(this).css('color','#fe8285').children('b').addClass('siB-active');
        $(this).siblings().css('color','#333').children('b').removeClass('siB-active');
    });
    /*收藏星星点击换图*/
    $('.w-MyStar').click(function(){
        if($(this).hasClass('w-MyStar-img')){
            $(this).removeClass('w-MyStar-img');
        }else{
            $(this).addClass('w-MyStar-img');
        };
    });
    /*点击优惠券弹出层*/
    $('.w-youhui').click(function(){
        $('.w-markBox').show();
    });

    $('.w-markBox').click(function(){
        $('.w-markBox').hide();
    });
    $('.w-mark').click(function(){
        return false;
    })
    $('.w-mark input').click(function(){
        $('.w-markBox').hide();
    });
});
