$(function () {
    layui.use(["form", "layer", "laydate", "element"], function () {
        var layer = layui.layer;
        window.laydate = layui.laydate;
        window.element = layui.element();
        window.form = layui.form();
        //监听Tab切换，以改变地址hash值
        element.on('tab(MemberDest)', function () {
            if (this.getAttribute('lay-id') == "birthday") getBirthdayData();
        });
        getPageData();
    });
    $(".customer-table,.birthday-table").on("click", ".PointToCoupon", function () {
        $(".PointToCoupon-group").show();
        $(".DeductPoint-group").hide();
        getPointInfo(this, "PointToCoupon");
    });
    $(".customer-table,.birthday-table").on("click", ".DeductPoint", function () {
        $(".DeductPoint-group").show();
        $(".PointToCoupon-group").hide();
        getPointInfo(this, "DeductPoint");
    });
    $(".memberDesk.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            CustomerID: $(".CustomerID").text(),
            Point: $("[name=Point]").val(),
            PointAmount: $("[name=PointAmount]").val(),
            CouponCategory: $("[name=couponCategoryId]").val(),
            CouponAmount: $("[name=CouponAmount]").val(),
            Remark: $("[name=Remark]").val(),
        });
        if (ajaxObj.setUrl("/Point/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 回款搜索
    $(".memberDesk.search-btn").click(function () {
        getPageData();
    });
    // 选择劵类型input点击事件
    $(document).on("click", "[name=couponCategory]:not(:disabled)", function () {
        getCouponData();
        openPop("", ".coupon-pop", "选择劵类型");
    });
    // 选择劵类型弹窗复选框事件
    $(".coupon-table").on("click", "[type=checkbox]", function () {
        $(".coupon-table").find("[type=checkbox]").not($(this)).prop("checked", false);
    });
    // 项目弹窗提交
    $(".coupon.submit-btn").click(function () {
        $(".coupon-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("[name=couponCategoryId]").val(item.val());
            $("[name=couponCategory]").val(item.data("name"));
        });
        closeLayer(this);
    });
});
var getMemberDeskData = function () {
    params.setDataParams({
        CustomerID: $("[name=sCustomerID]").val(),
        CustomerName: $("[name=sCustomerName]").val(),
        Mobile: $("[name=sMobile]").val(),
        Gender: $("[name=sGender]").find(":selected").val(),
        MemberCategoryID: $("[name=sMemberCategoryID]").val(),
        ShareCategoryID: $("[name=sShareCategoryID]").val(),
        BirthdayStart: $("[name=sBirthdayStart]").val(),
        BirthdayEnd: $("[name=sBirthdayEnd]").val()
    });
    var dotEle = [{ container: ".customer-table", tmp: ".customer-tmp" }];
    ajaxObj.setUrl("/MemberDesk/Get").setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    return true;
}
var getBirthdayData = function () {
    var dotEle = [{ container: ".birthday-table", tmp: ".birthday-tmp" }];
    ajaxObj.setUrl("/MemberDesk/GetBirthday").setDotEle(dotEle).getData();
}
var getPointInfo = function (obj, url) {
    var dotEle = [{ container: ".memberDesk-info", tmp: ".memberDesk-info-tmp" }];
    params.setDataParam("customerID", $(obj).data("id"));
    ajaxObj.setUrl("/Point/GetPointInfo").setDotEle(dotEle).setParaObj(params).getData();
    openPop(url, ".memberDesk-pop", url == "DeductPoint" ? "手动增加扣减积分" : "积分兑换券");
}
var getCouponData = function () {
    var ajaxObj = {
        url: "/CouponCategory/CouponCategoryInfoGet",
        paraObj: {},
        dotEle: [
            {
                container: ".coupon-table",
                tmp: ".coupon-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
getPageData = getMemberDeskData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;