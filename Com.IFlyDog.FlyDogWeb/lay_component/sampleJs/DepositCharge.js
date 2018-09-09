$(function () {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.form = layui.form();
        // 是否是用时间限制
        form.on("checkbox(useHasCoupon)", function (ele) {
            if (ele.elem.checked) {
                $("[name=couponCategory],[name=couponPrice]").prop("disabled", false);
            } else {
                $("[name=couponCategory],[name=couponPrice]").prop("disabled", true);
            }
            form.render();
        });
        // 医院下拉框
        form.on("select(Hospital)", function (ele) {
            if ($(".Hospital-selected").find("[hospitalId=" + ele.value + "]").length == 0) {
                $(".Hospital-selected").append("<span class='layui-btn-small layui-btn m-bt-10' hospitalId=" + ele.value + ">" + $(ele.elem).find(":selected").text() + " <i class='layui-icon'>&#xe640;</i></span>");
            }
        });
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
    // 添加医院下拉框的事件，用作layui监控
    $(".Hospital").attr("lay-filter", "Hospital");
    // 获取数据
    getDepositChargeData();
    // 添加按钮
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "预收款项目";
        opt.url = "DepositChargeAdd";
        opt.popEle = ".depositCharge-pop";
        opt.func = function (layero) {
            if (layero.find(".Hospital-selected,.chargeCategory-selected,.charge-selected").length != 0) {
                layero.find(".dept_close,.layui-layer-close").bind("click", function () {
                    closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty(); $(this).parents(".layui-layer").find(".dept_close,.layui-layer-close").unbind("click") });
                });
            }
        }
        opt.area = ["50%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 编辑按钮
    $(".depositCharge-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/DepositCharge/DepositChargeGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=depositChargeId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=price]").val(data.Price);
                $.each(data.SmartDepositChargeHospitalAdd, function (i, item) {
                    $(".Hospital-selected").append("<span class='layui-btn-small layui-btn m-bt-10' hospitalId=" + item.HospitalID + ">" + item.HospitalName + " <i class='layui-icon'>&#xe640;</i></span>");
                });
                $("[name=useScope][data-id=" + data.ScopeLimit + "]").prop("checked", true);
                if (data.ScopeLimit == 2) {
                    $.each(data.SmartDepositChargeChargeCategoryAdd, function (i, item) {
                        $(".chargeCategory-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeCategoryId=" + item.ChargeCategoryID + ">" + item.ChargeCategoryName + "<i class='layui-icon'>&#xe640;</i></span>");
                    });
                }
                if (data.ScopeLimit == 3) {
                    $.each(data.SmartDepositChargeChargeAdd, function (i, item) {
                        $(".charge-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeId=" + item.ChargeID + ">" + item.ChargeName + "<i class='layui-icon'>&#xe640;</i></span>");
                    });
                }
                if (data.HasCoupon == 1) {
                    //
                    $("[name=useHasCoupon]").prop("checked", true)
                    $("[name=couponCategoryId]").val(data.CouponCategoryID);
                    $("[name=couponCategory]").val(data.CouponCategoryName);
                    $("[name=couponPrice]").val(data.CouponAmount);
                    $("[name=couponCategory]").prop("disabled",false);
                    $("[name=couponPrice]").prop("disabled", false);
                }
                $("[name=Status]").find("[value=" + data.Status + "]").prop("selected", true);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title =  "预收款项目";
        opt.url = "DepositChargeEdit";
        opt.popEle = ".depositCharge-pop";
        opt.func = function (layero) {
            if (layero.find(".Hospital-selected,.chargeCategory-selected,.charge-selected").length != 0) {
                layero.find(".dept_close,.layui-layer-close").bind("click", function () {
                    closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty(); $(this).parents(".layui-layer").find(".dept_close,.layui-layer-close").unbind("click") });
                });
            }
        };
        opt.area = ["50%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
        $(".coupon-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
            closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
        });
    });
    // 取消关闭按钮
    $(".close-layer").click(function () {
        closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
    });
    // 详细弹窗弹窗提交
    $(".depositCharge.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        // 医院ID
        var hospitalIds = [];
        // 使用范围限制
        var ScopeLimit = $("[name=useScope]:checked").data("id"),
            chargeCategory = [],
            charge = [];
        $(".chargeCategory-selected").find("span").each(function (i, item) {
            item = $(item);
            if (Common.StrUtils.isNullOrEmpty(chargeCategory)) {
                chargeCategory = [];
            }
            chargeCategory.push({ ChargeCategoryID: item.attr("chargecategoryid") });
        });
        $(".charge-selected").find("span").each(function (i, item) {
            item = $(item);
            if (Common.StrUtils.isNullOrEmpty(charge)) {
                charge = [];
            }
            charge.push({ ChargeID: item.attr("chargeid") });
        });
        $(".Hospital-selected").find("span").each(function (i, item) {
            item = $(item);
            hospitalIds.push({ HospitalID: item.attr("hospitalId") });
        });
        // 使用时间限制
        var HasCoupon = $("[name=useHasCoupon]:checked").length == 0 ? 0 : 1,
            couponCategoryId = "",
            couponPrice = "";
        if (HasCoupon == 1) {
            couponCategoryId = $("[name=couponCategoryId]").val();
            couponPrice = $("[name=couponPrice]").val();
        }
        var ajaxObj = {
            url: "/DepositCharge/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=depositChargeId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    Price: $("[name=price]").val(),
                    SmartDepositChargeHospitalAdd: hospitalIds,
                    ScopeLimit: ScopeLimit,
                    SmartDepositChargeChargeCategoryAdd: chargeCategory,
                    SmartDepositChargeChargeAdd: charge,
                    HasCoupon: HasCoupon,
                    CouponCategoryID: couponCategoryId,
                    CouponAmount: couponPrice,
                    Remark: Common.StrUtils.isFalseSetEmpty($("[name=remark]").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
        }

    });
    // 项目分类弹窗提交
    $(".chargeCategory.submit-btn").click(function () {
        $(".chargeCategory-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".chargeCategory-selected").find("[chargeCategoryId=" + item.val() + "]").length == 0) {
                $(".chargeCategory-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeCategoryId=" + item.val() + ">" + item.attr("title") + "<i class='layui-icon'>&#xe640;</i></span>");
            }
        });
        closeLayer(this);
    });
    // 项目弹窗提交
    $(".charge.submit-btn").click(function () {
        $(".charge-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".charge-selected").find("[chargeId=" + item.val() + "]").length == 0) {
                $(".charge-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeId=" + item.val() + ">" + item.attr("title") + "<i class='layui-icon'>&#xe640;</i></span>");
            }
        });
        closeLayer(this);
    });
    // 项目弹窗提交
    $(".coupon.submit-btn").click(function () {
        $(".coupon-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("[name=couponCategoryId]").val(item.val());
            $("[name=couponCategory]").val(item.attr("title"));
        });
        closeLayer(this);
    });
    // 弹窗选择项目分类按钮
    $(".ChargeCategory-btn").click(function () {
        if (getChargeCategoryData()) {
            openPop("", ".chargeCategory-pop", "选择收费项目类型");
        }
    });
    // 弹窗选择项目按钮
    $(".Charge-btn").click(function () {
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择收费项目");
        }
    });
    // 指定项目分类和指定项目里面条件按钮
    $(".Hospital-selected,.chargeCategory-selected,.charge-selected").on("click", "span", function () {
        $(this).remove();
    });
    // 查询按钮
    $(".search-btn").click(function () {
        getChargeData();
    });
});
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=price]").val())) {
        layer.msg("价格不能为空！", { icon: 2 });
        return false;
    } else {
        if (isNaN($("[name=price]").val())) {
            layer.msg("价格只能是数字！", { icon: 2 });
            return false;
        }
    }
    if ($(".Hospital-selected").find("span").length == 0) {
        layer.msg("请选择医院！", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == 2 && $(".chargeCategory-selected").find("span").length == 0) {
        layer.msg("请选择收费项目类型！", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == 3 && $(".charge-selected").find("span").length == 0) {
        layer.msg("请选择收费项目！", { icon: 2 });
        return false;
    }
    if ($("[name=useHasCoupon]:checked").length != 0) {
        if (Common.StrUtils.isNullOrEmpty($("[name=couponCategory]").val()) || Common.StrUtils.isNullOrEmpty($("[name=couponCategoryId]").val())) {
            layer.msg("请添加劵类型！", { icon: 2 });
            return false;
        }
        if (Common.StrUtils.isNullOrEmpty($("[name=couponPrice]").val()) || isNaN($("[name=couponPrice]").val())) {
            layer.msg("劵金额格式不正确！", { icon: 2 });
            return false;
        }
    }
    return true;
}
var getDepositChargeData = function () {
    var ajaxObj = {
        url: "/DepositCharge/DepositChargeInfoGet",
        paraObj: {},
        dotEle: [
            {
                container: ".depositCharge-table",
                tmp: ".depositCharge-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
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
var getChargeData = function () {
    if ($("[name=useScope]").filter("[value=charge]:checked").length == 0) {
        return false;
    }
    var ajaxObj = {
        url: "/Charge/ChargeGetData",
        paraObj: {
            data: {
                PinYin: Common.StrUtils.isFalseSetEmpty($("[name=smartProductPinYin]").val()),
                Name: Common.StrUtils.isFalseSetEmpty($("[name=smartProductNmae]").val())
            }
        },
        dataCallBack: function (data) {
            var tmpHtml = $(doT.template($(".charge-tmp").text())(data.Data));
            $(".charge-selected").find("span").each(function (i, item) {
                item = $(item);
                tmpHtml = tmpHtml.not("[chargeId= " + item.attr("chargeId") + "]");
            });
            $(".charge-table").html(tmpHtml);
        }
    };
    dataFunc(ajaxObj);
    return true;
}
var getChargeCategoryData = function () {
    if ($("[name=useScope]").filter("[value=chargeCategory]:checked").length == 0) {
        return false;
    }
    var ajaxObj = {
        url: "/ChargeCategory/ChargeCategoryGet",
        paraObj: {},
        dataCallBack: function (data) {
            var tmpHtml = $(doT.template($(".chargeCategory-tmp").text())(data.Data));
            $(".chargeCategory-selected").find("span").each(function (i, item) {
                item = $(item);
                tmpHtml = tmpHtml.not("[chargeCategoryId= " + item.attr("chargeCategoryId") + "]");
            });
            $(".chargeCategory-table").html(tmpHtml);
        }
    };
    dataFunc(ajaxObj);
    return true;
}
var getPageData = getDepositChargeData;