
基于bootstrap的时间选择控件

使用时间插件


头部需要引用样式

<!--请注意使用该样式需要在最外面引用bootstrap的样式，我在main里面引用了，因此后台你们要用这个控件就不需要引用了-->
<link href="~/scripts/plug-in/date/date-picker/daterangepicker-bs3.css" rel="stylesheet" />

头部需要引用的js

<script src="~/scripts/plug-in/date/date-picker/moment.js"></script>
<script src="~/scripts/plug-in/date/date-picker/daterangepicker.js"></script>

页面的元素代码为
<input type="text" style="width: 300px;height:22px;" name="reservation" id="reservationtime" class="textbox" />

js的代码
$('#reservationtime').daterangepicker({
                    timePicker: true,
                    timePickerIncrement: 10,//表示选择时间的分钟间隔
                    format: 'YYYY年MM月DD日 h时mm分'
                });