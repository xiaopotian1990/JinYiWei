$(function () {
    layui.use(["form", "layer", "laydate", "laypage"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.form = layui.form();
        // 是否是用时间限制
        form.on("checkbox(TimeLimit)", function (ele) {
            if (ele.elem.checked) {
                $("[name=startDate],[name=day]").prop("disabled", false);
            } else {
                $("[name=startDate],[name=day]").prop("disabled", true);
            }
            form.render();
        });
        getChargeSetData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "套餐管理";
        opt.url = "ChargeSetAdd";
        opt.popEle = ".chargeSet-pop";
        opt.func = function () {
            $(".chargeSet-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
                closeLayer(this, function () { $(".chargeSet-product-table").empty() });
            });
        };
        opt.area = ["35%;min-width:680px;max-width:800px", "85%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 编辑按钮
    $(".chargeSet-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/ChargeSet/ChargeSetGetByID",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=chargeSetId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=pinYin]").val(data.PinYin);
                if (data.TimeLimit == 1) {
                    $("[name=useDate]").prop("checked", true);
                    $("[name=day]").val(data.Days);
                    $("[name=startDate]").prop("disabled",false).filter("[value=" + data.TimeStart + "]").prop("checked", true);
                }
                $("[name=Status]").find("[value=" + data.Status + "]").prop("selected", true);
                $("[name=remark]").val(data.Remark);
                $(".chargeSet-product-table").html(doT.template($(".chargeSet-product-tmp").text())(data.SmartChargeSetDetailAdd));
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title =  "套餐管理";
        opt.url = "ChargeSetEdit";
        opt.popEle = ".chargeSet-pop";
        opt.func = function () {
            $(".chargeSet-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
                closeLayer(this, function () { $(".chargeSet-product-table").empty() });
            });
        };
        opt.area = ["35%;min-width:680px;max-width:800px", "85%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
        
    });
    // 详细弹窗弹窗提交
    $(".chargeSet.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        // 使用范围限制
        var charge = [];
        $(".chargeSet-product-table").find("tr").each(function (i, item) {
            item = $(item);
            if (Common.StrUtils.isNullOrEmpty(charge)) {
                charge = [];
            }
            charge.push({
                ChargeID: item.attr("chargeid"),
                Num:item.find("[name=num]").val(),
                Amount: item.find("[name=amount]").val()
            });
        });
        // 使用时间限制
        var TimeLimit = $("[name=useDate]:checked").length,
            timeStart = $("[name=startDate]:checked").val(),
            days = $("[name=day]").val();
        var ajaxObj = {
            url: "/ChargeSet/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=chargeSetId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    PinYin: Common.StrUtils.isFalseSetEmpty($("[name=pinYin]").val()),
                    SmartChargeSetDetailAdd: charge,
                    TimeLimit: TimeLimit,
                    TimeStart: timeStart,
                    Days: days,
                    Remark: Common.StrUtils.isFalseSetEmpty($("[name=remark]").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this, function () { $(".chargeSet-product-table").empty() });
        }

    });
    // 项目弹窗提交
    $(".charge.submit-btn").click(function () {
        var charges = [];
        $(".charge-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".charge-selected").find("[chargeId=" + item.val() + "]").length == 0) {
                charges.push({
                    ChargeID: item.val(),
                    ChargeName: item.attr("title"),
                    Num: 0,
                    Amount: 0
                });
            }
        });
        $(".chargeSet-product-table").append(doT.template($(".chargeSet-product-tmp").text())(charges));
        closeLayer(this);
    });
    // 弹窗选择项目按钮
    $(".add-charge").click(function () {
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择项目");
        }
    });
    // 查询按钮
    $(".chargeSet.search-btn").click(function () {
        getPageData();
    });
    // 查询按钮
    $(".charge.search-btn").click(function () {
        getChargeData();
    });
    // 删除按钮
    $(".chargeSet-product-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });

});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=pinYin]").val())) {
        layer.msg("拼音码不能为空！", { icon: 2 });
        return false;
    }
    if ($("[name=useDate]:checked").length == 1 && Common.StrUtils.isNullOrEmpty($("[name=day]").val())) {
        layer.msg("请填写天数！", { icon: 2 });
        if (isNaN($("[name=startDate]:checked").val())) {
            layer.msg("天数只能为数字！", { icon: 2 });
        }
        return false;
    }
    return true;
}
var getChargeSetData = function () {
    var ajaxObj = {
        url: "/ChargeSet/ChargeSetGet",
        paraObj: {
            data: {
                Name: Common.StrUtils.isFalseSetEmpty($("[name=sNmae]").val()),
                PinYin:Common.StrUtils.isFalseSetEmpty($("[name=sPinYin]").val()),
                Status :Common.StrUtils.isFalseSetEmpty($("[name=sStatus]").val()),
                PageNum :pageNum,
                PageSize: pageSize
            }
        },
        hasPage:true,
        dotEle: [
            {
                container: ".chargeSet-table",
                tmp: ".chargeSet-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getChargeData = function () {
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
            $(".chargeSet-product-table").find("tr").each(function (i, item) {
                item = $(item);
                tmpHtml = tmpHtml.not("[chargeId= " + item.attr("chargeId") + "]");
            });
            $(".charge-table").html(tmpHtml);
        }
    };
    dataFunc(ajaxObj);
    return true;
}
var getPageData = getChargeSetData;