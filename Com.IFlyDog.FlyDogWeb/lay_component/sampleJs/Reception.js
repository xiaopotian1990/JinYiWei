$(function() {
    getReceptionTodayAsync();
});
/*今日上门记录*/
var getReceptionTodayAsync = function () {
    var url = "/Reception/GetReceptionTodayAsync";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
 
    var interText = doT.template($(".fd-reception-inshow-temp").text());
    var html = "<blockquote class='layui-elem-quote'>" + " 暂无接诊顾客" + "</blockquote>";
    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".reception-info").html(html);
        $(".tbody-Reception-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-Reception-inshow").html(interText(result));
    }
};