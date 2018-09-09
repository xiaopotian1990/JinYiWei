$(function() {
    layui.use(["form", "element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(), laydate = layui.laydate(), layer = layui.layer, laypage = layui.laypage, form = layui.form();
        form.on("select(Hospital)", function (elem) {
            treatGet(elem.value);
            form.render();
        });
        form.render();
    });
    treatGet();
    $(".TreatSchedulerHospital").attr("lay-filter", "Hospital");
});
var thdate = new Date();

var treatGet = function () {
    var url = "/TreatScheduler/TreatGet";
    var paraObj = {
        data: {
            hospitalId: $(".TreatSchedulerHospital").val(),
            startTime: $("input[name='selTreatTime']").val() === "" ? thdate.toLocaleDateString() : $("input[name='selTreatTime']").val(),
            endTime: $("input[name='selTreatTime']").val() === "" ? thdate.toLocaleDateString() : $("input[name='selTreatTime']").val()
        }
    };
    var result = ajaxProcess(url, paraObj); 
    //判断返回数据是否成功
    if (result.ResultType === 0) {
        if (result.Data.length > 0) {
            /*在doT模版输出数据*/
            var interText = doT.template($(".treat-temp").text());
            $(".tb-treat-info").html(interText(result.Data));
            $(".treat-info-null").html("");
        } else {
            var html = "<blockquote class='layui-elem-quote'>" + "暂无数据" + "</blockquote>";
            $(".tb-treat-info").html("");
            $(".treat-info-null").html(html);

        }
    }
};