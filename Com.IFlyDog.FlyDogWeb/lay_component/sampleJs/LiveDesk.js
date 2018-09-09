$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form(); 
    });
    getRecTodayAsync();
    //刷新
    $(".refresh-btn").click(function() {
        getRecTodayAsync();
    });
    //双击事件
    $(".tb-receptions").on("dblclick", "tr", function () {
        //$(this).children("td:last").slideToggle();
        //$(this).children("td:last").fadeToggle();
      
    });
});
//查询今天信息
var getRecTodayAsync = function () {
    var url = "/SmartLiveDesk/GetReceptionTodayAsync";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        var interText = doT.template($(".rec-TodayInfo-temp").text());
        $(".tb-receToday").html(interText(result.Data));
        $(".tb-receToday td:even").attr("class", "layui-bg-gray");

        if (result.Data.Receptions !== null) {
            var interText1 = doT.template($(".receptions-temp").text());
            $(".tb-receptions").html(interText1(result.Data.Receptions));
        }
    } else {
//        layer.msg(result.Message);
    }
};