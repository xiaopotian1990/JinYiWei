$(function () {
    layui.use(["form", "element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(),
            laydate = layui.laydate(),
            layer = layui.layer;
            window.laypage = layui.laypage;
            window.form = layui.form();
            form.render();
            getPageData();
    });
    //查询按钮
    $(".Settl-search-btn").click(function () {
        getPageData();
    });
});
var pageIndex = 1, pageSize = 15, pageTotals;;
var getSettlement = function () {
    var ajaxObj = {
        url: "/CashierDesk/SettlementGet",
        paraObj: {
            data: {
                Name: $("input[name='settCuName']").val(),
                HospitalID: $(".SettHospital").val(),
                StartTime: $("input[name='settstartTime']").val(),
                EndTime: $("input[name='settendTime']").val(),
                PageNum: pageIndex,
                PageSize: pageSize
            }
        },
        hasPage: true,
        dotEle: [{ container: ".tb-settlement", tmp: ".settlement-temp" }]
    };
    dataFunc(ajaxObj);
};
getPageData = getSettlement;
