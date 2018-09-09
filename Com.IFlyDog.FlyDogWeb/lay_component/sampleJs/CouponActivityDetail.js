$(function () {
    layui.use(["form", "layer", "laypage", "laydate"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.laydate = layui.laydate;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "导入代码";
        opt.url = "CouponActivityDetailAdd";
        opt.popEle = ".couponActivity-info-pop";
        opt.area = ["33%;max-width:800px", "65%;min-height:300px;max-height:300px"];
        openPopWithOpt(opt);
        form.render();
    });
    $(".couponActivity.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ActivityID:getQueryString("ActivityID"),
            CodeBegin: $("[name=CodeBegin]").val(),
            CodeEnd: $("[name=CodeEnd]").val(),
        });
        if (ajaxObj.setUrl("/CouponActivity/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".couponActivity-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("确定删除？", function () {
            params.setDataParams({
                ID: id
            });
            if (ajaxObj.setUrl("/CouponActivity/CouponActivityDetailDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    $(".btn-rmv-all").click(function () {
        layer.confirm("确定清空？", function () {
            params.setDataParams({
                ActivityID: getQueryString("ActivityID")
            });
            if (ajaxObj.setUrl("/CouponActivity/CouponActivityDetailDeleteAllDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
        
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getCouponActivityData = function () {
    params.setDataParams({
        PageNum: pageNum,
        PageSize: pageSize,
        CategoryID: getQueryString("ActivityID")
    });
    var dotEle = [{ container: ".couponActivity-table", tmp: ".couponActivity-tmp" }];
    ajaxObj.setUrl("/CouponActivity/CouponActivityDetailGet").usePage().setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=CodeBegin]").val())) {
        layer.msg("开始区间不能为空！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=CodeEnd]"))) {
        layer.msg("结束区间不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
getPageData = getCouponActivityData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;