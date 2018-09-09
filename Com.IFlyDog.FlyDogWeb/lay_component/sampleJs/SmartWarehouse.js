$(function () {
    layui.use("form", function () {
        window.form = layui.form();
    });
});
var i = 1;
//显示
$("#smartWarehousehtml")
    .ready(function () {
        var infoFunc = function () {

            var url = "/SmartWarehouse/SmartWarehouseGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#smartWarehouse_template").text());
            $(".layui-field-box").html(interText(data.Data));
        };
        infoFunc();
    });
