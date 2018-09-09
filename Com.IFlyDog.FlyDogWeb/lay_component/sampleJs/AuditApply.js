$(function () {
    layui.use(["form", "layer", "laypage", "laydate"], function () {
        var layer = layui.layer,
        laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
    });
    $(".auditApply-table").on("click", ".btn-look", function () {
        var _this = $(this);
        params.setDataParams({
            orderID: _this.data("id"),
            type: _this.data("type")
        });
        var dotEle = [{ container: ".auditApply-detail-table", tmp: ".auditApply-detail-tmp" }];
        var result = ajaxObj.setUrl("/AuditApply/GetAuditDetail").setParaObj(params).setDotEle(dotEle).getData();
        $(".auditStatus").text(auditStatus[result.Data.AuditStatus - 1]);
        var opt = {};
        opt.title = "申请审核信息";
        opt.url = "";
        opt.popEle = ".auditApply-pop";
        opt.area = ["65%", "65%"];
        openPopWithOpt(opt);
    });
    $(".auditApply-table").on("click", ".btn-cancel", function () {
        var _this = $(this);
        layer.confirm("确定取消？", function () {
            params.setDataParams({
                OrderID: _this.data("id"),
                OrderType: parseInt(_this.data("type")) + 3
            });
            ajaxObj.setUrl("/AuditApply/AuditApplyDelete").setIsUpdateTrue().setParaObj(params).getData()
        }, function () {
            layer.msg("您已取消操作~", {icon:1});
        });
    });
    $(".search-btn").click(function () {
        getPageData();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals,
    auditStatus = ["待审核", "无需审核", "审核不通过", "审核通过", "未提交"];;
var getAuditApplyData = function () {
    params.setDataParams({
        AuditType: $("[name=sAuditType]").val(),
        BeginTime: $("[name=sBeginTime]").val(),
        EndTime: $("[name=sEndTime]").val(),
        PageNum: pageNum,
        PageSize: pageSize
    });
    var dotEle = [{ container: ".auditApply-table", tmp: ".auditApply-tmp" }];
    ajaxObj.setUrl("/AuditApply/AuditApplyGet").usePage().setParaObj(params).setDotEle(dotEle).getData();
}
getPageData = getAuditApplyData;
