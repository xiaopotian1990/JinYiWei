$(function () {
    layui.use(["form", "layer", "laypage", "laydate"], function () {
        var layer = layui.layer,
        laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
        getOrderDetailData();
    });
    $(".auditOperation-table").on("click", ".audit-btn", function () {
        var _this = $(this);
        params.setDataParams({
            UserLevel: _this.data("level"),
            MaxLevel: maxLevel,
            AuditDataID: auditDataID,
            AuditUserID: userId,
            Status: _this.data("status"),
            Content: _this.parent().prev("div").find("textarea").val(),
            AutitType: type,
            CustomerID: getQueryString("customerId"),
            OldID: $("[name=OldID]").val(),
            NewID: $("[name=NewID]").val()
        });
        ajaxObj.setUrl("/AuditWorkbench/AuditOperationAdd").setIsUpdateTrue().setParaObj(params).getData();
    });
    $(".search-btn").click(function () {
        getPageData();
    });
    $(".auditOperation-table,.auditOperation-history-table").on("click", ".addCustomerTab", function () {
        var _this = $(this),
            id = _this.data(id),
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
    auditStatus = ["待审核", "无需审核", "审核不通过", "审核通过", "未提交"],
    type = getQueryString("type"),
    maxLevel = 0,
    userId = 0,
    auditDataID = getQueryString("auditDataID");
var getDetailData = function () {
    params.setDataParams({ Type: type, OrderID: auditDataID });
    var dotEle = [{ container: ".auditOperation-table", tmp: ".auditOperation-tmp" }];
    var result = ajaxObj.setUrl("/AuditWorkbench/ByTypeGet").setParaObj(params).setDotEle(dotEle).getData().Data;
    maxLevel = result.SumLevel;
    userId = result.UserID;
}
var getOrderDetailData = function () {
    var urls = ["/Order/GetOrderDetail", "/AuditWorkbench/GetBackOrderDetail", "/AuditWorkbench/GetDepositRebateOrderDetail", "/AuditWorkbench/GetAuditOrderInfo"],
        urlIndex = 0;
    console.log(urlIndex);
    if (type < 4) {
        params.setDataParams({
            customerId: getQueryString("customerId"),
            orderId: auditDataID
        });
        urlIndex = type - 1;
    } else {
        params.setDataParams({
            type: type,
            id: auditDataID
        });
        urlIndex = 3;
    }
    var dotEle = [{ container: ".info-form", tmp: ".info-tmp" }];
    ajaxObj.setUrl(urls[urlIndex]).setParaObj(params).setDotEle(dotEle).getData();
}
getPageData = getDetailData;
