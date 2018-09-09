$(function () {
    layui.use(["form", "layer", "laypage", "laydate"], function () {
        var layer = layui.layer,
        laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-history").click(function () {
        getAuditWorkHistoryData();
        var opt = {};
        opt.title = "审核记录";
        opt.url = "";
        opt.popEle = ".auditWork-pop";
        opt.func = function () { };
        opt.area = ["85%", "85%"];
        openPopWithOpt(opt);
    });
    $(".search-btn").click(function () {
        getAuditWorkHistoryData();
    });
    $(".auditWork-table,.auditWork-history-table").on("click", ".addCustomerTab", function () {
        var _this = $(this),
            id = _this.data("id"),
            name = _this.text();
        parent.layui.tab({
            elem: ".admin-nav-card"
        }).tabAdd(
        {
            title: "客户档案:" + name + "编号:" + id,
            href: "/Customer/CustomerProfile", //地址
            icon: "fa-user"
        });
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals,
    auditStatus = ["待审核", "无需审核", "审核不通过", "审核通过", "未提交"];
var getAuditWorkData = function () {
    params.setDataParams({
        PageNum: pageNum,
        PageSize: pageSize
    });
    var dotEle = [{ container: ".auditWork-table", tmp: ".auditWork-tmp" }];
    ajaxObj.setUrl("/AuditWorkbench/GetAllAuditGet").usePage().setParaObj(params).setDotEle(dotEle).getData();
}
var getAuditWorkHistoryData = function () {
    params.setDataParams({
        AuditType: $("[name=sAuditType]").val(),
        AuditBeginTime: $("[name=sAuditBeginTime]").val(),
        AuditEndTime: $("[name=sAuditEndTime]").val(),
        CustormNo: $("[name=sCustormNo]").val(),
        PageNum: pageNum,
        PageSize: pageSize
    });
    var dotEle = [{ container: ".auditWork-history-table", tmp: ".auditWork-history-tmp" }];
    ajaxObj.setUrl("/AuditWorkbench/AuditRecordGet").usePage("popPageDiv").setParaObj(params).setDotEle(dotEle).getData();
}
getPageData = getAuditWorkData;
