$(function () {
    layui.use(["form", "layer", "laydate", "laypage"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
            window.laypage = layui.laypage;
            window.form = layui.form();
            getChargeDiscountData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "项目折扣";
        opt.url = "ChargeDiscountAdd";
        opt.popEle = ".chargeDiscount-pop";
        opt.func = function () {
            $(".chargeDiscount-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
                closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
            });
        };
        opt.area = ["35%;min-width:680px;max-width:800px", "85%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 编辑按钮
    $(".chargeDiscount-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/ChargeDiscount/ChargeDiscountGetByID",
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=chargeDiscountId]").val(data.ID);
                $("[name=useScope][data-id=" + data.ScopeLimit + "]").prop("checked", true);
                $("[name=discount]").val(data.Discount);
                if (data.ScopeLimit == 1) {
                    $(".chargeCategory-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeCategoryId=" + data.ChargeCategoryID + ">" + data.ProjectName + "<i class='layui-icon'>&#xe640;</i></span>");
                }
                if (data.ScopeLimit == 2) {
                    $(".charge-selected").append("<span class='layui-btn-small layui-btn m-bt-10' chargeId=" + data.ChargeID + ">" + data.ProjectName + "<i class='layui-icon'>&#xe640;</i></span>");
                }
                $("[name=start]").val(data.StartTime);
                $("[name=end]").val(data.EndTime);
                $("[name=status]").find("[value=" + data.Status + "]").prop("selected", true);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title =  "项目折扣";
        opt.url = "ChargeDiscountEdit";
        opt.popEle =  ".chargeDiscount-pop";
        opt.func = function () {
            $(".chargeDiscount-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
                closeLayer(this, function () { $(".Hospital-selected,.chargeCategory-selected,.charge-selected").empty() });
            });
        };
        opt.area = ["35%;min-width:680px;max-width:800px", "85%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 详细弹窗弹窗提交
    $(".chargeDiscount.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        // 医院ID
        var hospitalIds = [];
        // 使用范围限制
        var ScopeLimit = $("[name=useScope]:checked").data("id");
        var ajaxObj = {
            url: "/ChargeDiscount/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=chargeDiscountId]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    ScopeLimit: ScopeLimit,
                    ChargeCategoryID : Common.StrUtils.isFalseSetEmpty($(".chargeCategory-selected").find("span").attr("chargecategoryid")),
                    ChargeID : Common.StrUtils.isFalseSetEmpty($(".charge-selected").find("span").attr("chargeid")),
                    Discount: Common.StrUtils.isFalseSetEmpty($("[name=discount]").val()),
                    StartTime: Common.StrUtils.isFalseSetEmpty($("[name=start]").val()),
                    EndTime: Common.StrUtils.isFalseSetEmpty($("[name=end]").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this, function () { $(".chargeCategory-selected,.charge-selected").empty() });
        }

    });
    // 项目分类弹窗提交
    $(".chargeCategory.submit-btn").click(function () {
        $(".chargeCategory-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $(".chargeCategory-selected").html("<span class='layui-btn-small layui-btn m-bt-10' chargeCategoryId=" + item.val() + ">" + item.attr("title") + "<i class='layui-icon'>&#xe640;</i></span>");
            
        });
        closeLayer(this);
    });
    // 项目弹窗提交
    $(".charge.submit-btn").click(function () {
        $(".charge-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $(".charge-selected").html("<span class='layui-btn-small layui-btn m-bt-10' chargeId=" + item.val() + ">" + item.attr("title") + "<i class='layui-icon'>&#xe640;</i></span>");
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
    $(".chargeCategory-selected,.charge-selected").on("click", "span", function () {
        $(this).remove();
    });
    $(".chargeCategory-pop,.charge-pop").on("change", "[type=checkbox]", function () {
        $(this).parents("table").find("[type=checkbox]").not(this).prop("checked", false);
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var verify = function () {
    if (Common.StrUtils.isNullOrEmpty($("[name=discount]").val())) {
        layer.msg("折扣不能为空！", { icon: 2 });
        return false;
    } else if (isNaN($("[name=discount]").val())) {
        layer.msg("折扣只能为数字！", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == 1 && $(".chargeCategory-selected").find("span").length == 0) {
        layer.msg("请选择收费项目类型！", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == 2 && $(".charge-selected").find("span").length == 0) {
        layer.msg("请选择收费收费项目！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=start]").val())) {
        layer.msg("开始日期不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=end]").val())) {
        layer.msg("结束日期不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getChargeDiscountData = function () {
    var ajaxObj = {
        url: "/ChargeDiscount/ChargeDiscountGet",
        paraObj: {
            data:{
                PageNum: pageNum,
                PageSize: pageSize
            }
        },
        hasPage: true,
        dotEle: [
            {
                container: ".chargeDiscount-table",
                tmp: ".chargeDiscount-tmp"
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
var getPageData = getChargeDiscountData;


//搜索项目
function onclickChargeBtn() {
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