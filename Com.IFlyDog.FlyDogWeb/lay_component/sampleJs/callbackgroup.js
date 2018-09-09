var i = 1;
//显示
$("#callbackGrouphtml")
    .ready(function () {
        var infoFunc = function () {

            var url = "/CallbackGroup/CallbackGroupGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#callbackGroup_template").text());
            $(".layui-field-box").html(interText(data.Data));
        };
        infoFunc();
    });
