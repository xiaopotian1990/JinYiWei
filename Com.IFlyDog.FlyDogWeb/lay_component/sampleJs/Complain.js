$(function () {
    layui.use(["form", "layer", "laypage", "laydate", "element"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.laydate = layui.laydate;
        window.form = layui.form();
        getPageData();
    });
    $(".complain-table").on("click", ".btn-edit", function () {
        var dotEle = [{ container: ".complain-form", tmp: ".complain-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        ajaxObj.setUrl("/Complain/ComplainGetByID").setDotEle(dotEle).setParaObj(params).getData();
        $("[name=StoreName]").val($(this).parents("tr").find(".store-a").text()).prop("disabled", true);
        openPop("ComplainEdit", ".complain-info-pop", "处理投诉");
    });
    $(".btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ID: $("[name=ID]").val(),
            Solution: $("[name=Solution]").val()
        });
        if (ajaxObj.setUrl("/Complain/ComplainEdit").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".search-btn").click(function () {
        getPageData();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getComplainData = function () {
    params.setDataParams({
        PageNum: pageNum,
        PageSize: pageSize,
        BeginTime: $("[name=sBeginTime]").val(),
        EndTime: $("[name=sEndTime]").val(),
        CustomerName: $("[name=sCustomerName]").val(),
        FinishUserName: $("[name=sFinishUserName]").val()
    });
    var dotEle = [{ container: ".complain-table", tmp: ".complain-tmp" }];
    ajaxObj.setUrl("/Complain/ComplainGet").usePage().setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (Common.StrUtils.isNullOrEmpty($("[name=Solution]").val())) {
        layer.msg("请输入处理内容！",{icon:2});
        return false;
    }
    return true;
}
getPageData = getComplainData;