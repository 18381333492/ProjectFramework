 
 qr二维码生成插件
 
 页面声明如下结构
 <div id="code" style="width: 100px; height: 100px"></div>

 js初始化 

    $("#code").qrcode({
        //render: "html",
        width: 100, //宽度 
        height: 100, //高度 
        text: 生成的二维码内容 //任意内容 
    });