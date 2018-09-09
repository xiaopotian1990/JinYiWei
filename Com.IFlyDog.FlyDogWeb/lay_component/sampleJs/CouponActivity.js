$(function () {
    layui.use(["form", "layer", "laypage","laydate"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.laydate = layui.laydate;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        var data = { ID: "", ActivityName: "", CategoryID: "", CouponCategoryName: "", Amount: "", Price: "", Channel: "", CreateDate: "", Expiration: "", IsRepetition: "", Prefix: "", Remark: "" };
        fillData(".couponActivity-form", ".couponActivity-form-tmp", data);
        form.render();
        openPop("CouponActivityAdd", ".couponActivity-info-pop", "添加劵活动");
    });
    $(".couponActivity-table").on("click", ".btn-edit", function () {
        var dotEle = [{ container: ".couponActivity-form", tmp: ".couponActivity-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        ajaxObj.setUrl("/CouponActivity/CouponActivityGetByID").setDotEle(dotEle).setParaObj(params).getData();
        form.render();
        openPop("CouponActivityEdit", ".couponActivity-info-pop", "编辑劵活动");
    });
    $(".couponActivity.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ID: $("[name=ID]").val(),
            ActivityName: $("[name=ActivityName]").val(),
            CategoryID: $("[name=CategoryID]").val(),
            CouponCategoryName: $("[name=CouponCategoryName]").val(),
            Amount: $("[name=Amount]").val(),
            Price: $("[name=Price]").val(),
            Channel: $("[name=Channel]").val(),
            CreateDate: $("[name=CreateDate]").val(),
            Expiration: $("[name=Expiration]").val(),
            IsRepetition: $("[name=IsRepetition]:checked").val(),
            Prefix: $("[name=Prefix]").val(),
            Remark: $("[name=Remark]").val()
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
            if (ajaxObj.setUrl("/CouponActivity/CouponActivityDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    $(".couponActivity.search-btn").click(function () {
        getPageData();
    });
    $(".couponActivity-form").on("click", "[name=CouponCategoryName]", function () {
        var dotEle = [{ container: ".category-table", tmp: ".category-tmp" }];
        ajaxObj.setUrl("/CouponCategory/CouponCategoryInfoGetByHospitalID").setParaObj().setDotEle(dotEle).getData();
        openPop("", ".category-pop", "选择劵类型");
    });
    $(".category-pop").on("click", "[type=checkbox]", function () {
        $(".category-pop").find("[type=checkbox]:checked").not(this).prop("checked", false);
    });
    $(".category.btn-submit").click(function () {
        $(".category-table").find(":checked").each(function (i, item) {
            item = $(item);
            $("[name=CategoryID]").val(item.val());
            $("[name=CouponCategoryName]").val(item.data("name"));
        });
        closeLayer(this);
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
        Name: $("[name=sName]").val(),
        BeginTime: $("[name=sBeginTime]").val(),
        EndTime: $("[name=sEndTime]").val()
    });
    var dotEle = [{ container: ".couponActivity-table", tmp: ".couponActivity-tmp" }];
    ajaxObj.setUrl("/CouponActivity/CouponActivityGet").usePage().setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=ActivityName]").val())) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    } else if ($("[name=ActivityName]").val().length > 20) {
        layer.msg("名称长度不能超过20！", { icon: 2 });
        return false;
    }
    return true;
}
getPageData = getCouponActivityData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;