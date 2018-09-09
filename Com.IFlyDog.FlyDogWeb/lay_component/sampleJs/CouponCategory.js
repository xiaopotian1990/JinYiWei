$(function () {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.form = layui.form();
        // 是否是用时间限制
        form.on("radio(useDate)", function (ele) {
            if (ele.value == "no") {
                $("[type=text][name=day-after],[type=text][name=date]").prop("disabled", true);
            }
            if (ele.value == "date") {
                $("[type=text][name=date]").prop("disabled", false);
                $("[type=text][name=day-after]").prop("disabled", true);
            }
            if (ele.value == "day-after") {
                $("[type=text][name=day-after]").prop("disabled", false);
                $("[type=text][name=date]").prop("disabled", true);
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
    $(".Hospital").attr("lay-filter", "Hospital");
    getCouponData();
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("SmartSupplierAdd", ".coupon-pop", "收费项目");
    });
    // 编辑按钮
    $(".coupon-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/CouponCategory/CouponCategoryGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=couponId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $.each(data.SmartCouponCategoryHospitalAdd, function (i, item) {
                    $(".Hospital-selected").append("<span class='layui-btn-small layui-btn m-bt-10' hospitalId=" + item.HospitalID + ">" + item.HospitalName + " <i class='layui-icon'>&#xe640;</i></span>");
                });
                $("[name=useScope][data-id=" + data.ScopeLimit + "]").prop("checked", true);
                if (data.ScopeLimit == 2) {
                    $.each(data.SmartCouponCategoryChargeCategoryAdd, function (i, item) {
                        $(".chargeCategory-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeCategoryId=" + item.ChargeCategoryID + ">" + item.ChargeCategoryName + "<i class='layui-icon'>&#xe640;</i></span>");
                    });
                }
                if (data.ScopeLimit == 3) {
                    $.each(data.SmartCouponCategoryChargeAdd, function (i, item) {
                        $(".charge-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeId=" + item.ChargeID + ">" + item.ChargeName + "<i class='layui-icon'>&#xe640;</i></span>");
                    });
                }
                $("[name=useDate][data-id=" + data.TimeLimit + "]").prop("checked",true);
                if (data.TimeLimit == 2) {
                    $("[name=date]").prop("disabled",false).val(data.EndDate);
                }
                if (data.TimeLimit == 3) {
                    $("[name=day-after]").prop("disabled", false).val(data.Days);
                }
                $("[name=status]").find("[value=" + data.Status + "]").prop("selected",true);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPop("SmartSupplierEdit", ".coupon-pop", "编辑劵类型");
        $(".coupon-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
            closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
        });
    });
    // 详细弹窗弹窗提交
    $(".coupon.submit-btn").click(function () {
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
        var TimeLimit = $("[name=useDate]:checked").data("id"),
            date = "",
            days = "";
        if (TimeLimit == 2) {
            date = $("[name=date]").val();
        }
        if (TimeLimit == 3) {
            days = $("[name=day-after]").val();
        }
        var ajaxObj = {
            url: "/CouponCategory/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=couponId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    SmartCouponCategoryHospitalAdd: hospitalIds,
                    ScopeLimit: ScopeLimit,
                    SmartCouponCategoryChargeCategoryAdd: chargeCategory,
                    SmartCouponCategoryChargeAdd: charge,
                    TimeLimit: TimeLimit,
                    EndDate: date,
                    Days: days,
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
    // 取消关闭按钮
    $(".close-layer").click(function () {
        closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
    });
});
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
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
        layer.msg("收费项目！", { icon: 2 });
        return false;
    }
    if ($("[name=useDate]:checked").val() == 2 && Common.StrUtils.isNullOrEmpty($("[name=date]").val())) {
        layer.msg("请填写日期！", { icon: 2 });
        return false;
    }
    if ($("[name=useDate]:checked").val() == 3 && Common.StrUtils.isNullOrEmpty($("[name=day-after]").val())) {
        layer.msg("请填写天数！", { icon: 2 });
        if (isNaN($("[name=useDate]:checked").val())) {
            layer.msg("天数只能为数字！", { icon: 2 });
        }
        return false;
    }
    return true;
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
var getPageData = getCouponData;