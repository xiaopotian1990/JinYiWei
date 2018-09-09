$(function () {
    layui.use(["form", "layer","laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
    getChargeCategoryData();
    getCargeData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "收费项目";
        opt.url = "ChargeAdd";
        opt.popEle = ".charge-pop";
        opt.area = ["35%;min-width:680px;max-width:800px", "75%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 编辑按钮
    $(".charge-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/Charge/ChargeGetByID",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=chargeId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=productPinYin]").val(data.PinYin);
                $("[name=ChargeCategory]").find("[value=" + data.CategoryID + "]").prop("selected", true);
                $("[name=status]").find("[value=" + data.Status + "]").prop("selected", true);
                $("[name=smartUnitKCDetaiName]").find("[value=" + data.UnitID + "]").prop("selected", true);
                $("[name=size]").val(data.Size);
                $("[name=price]").val(data.Price);
                $("[name=remark]").val(data.Remark);
                //$("[name=productAdd]").prop(":checked", data.ProductAdd == 0 ? true : false);
                $("[name='productAdd']").attr("checked",  data.ProductAdd == 0 ? true : false);//是否选     
                $(".charge-detail-table").html(doT.template($(".charge-detail-tmp").text())(data.SmartChargeProductDetailAdd));
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title = "收费项目";
        opt.url = "ChargeEdit";
        opt.popEle = ".charge-pop";
        opt.area = ["35%;min-width:680px;max-width:800px", "75%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 弹窗提交
    $(".submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var productDetails = [];
        $(".charge-detail-table").find("tr").each(function(i,item){
            item = $(item);
            productDetails.push({
                ProductID :item.attr("ProductID"),
                MinNum :item.find("[name=MinNum]").val(),
                MaxNum :item.find("[name=MaxNum]").val()
            });
        });
        var ajaxObj = {
            url: "/Charge/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=chargeId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    CategoryID: Common.StrUtils.isNullOrEmpty($("[name=ChargeCategory]").val()) ? 0 : $("[name=ChargeCategory]").val(),
                    PinYin: Common.StrUtils.isFalseSetEmpty($("[name=productPinYin]").val()),
                    Price: Common.StrUtils.isFalseSetEmpty($("[name=price]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    UnitID: Common.StrUtils.isFalseSetEmpty($("#smartUnitKCDetaiName").val()),
                    Size: Common.StrUtils.isFalseSetEmpty($("[name=size]").val()),
                    ProductAdd: $("input[name='productAdd']").is(":checked") ? 0 : 1,
                    Remark: Common.StrUtils.isFalseSetEmpty($(".remark").val()),
                    SmartChargeProductDetailAdd: productDetails
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
        }

    });
    // 查询按钮
    $(".search-btn").click(function () {
        getCargeData();
    });
    // 弹窗增加按钮
    $(".add-detail-btn").click(function () {
        var ajaxObj = {
            url: "/SmartProduct/SmartProductInfoGetAll",
            paraObj: {
                data: {
                    PinYin: Common.StrUtils.isFalseSetEmpty($("#smartProductPinYin").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("#smartProductName").val()),
                    CategoryId: Common.StrUtils.isFalseSetEmpty($("#smartProductDetaiName").val())
                }
            },
            dataCallBack: function (data) {
                data = setObjFieldEmpty(data.Data);
                var inner = $(doT.template($(".smartProduct-tmp").text())(data));
                $(".charge-detail-table").find("tr").each(function (i,item) {
                    item = $(item);
                    inner = inner.not("[productId=" + item.attr("ProductID") + "]");
                });
                $(".purchase-table").html(inner);
            }
        };
        dataFunc(ajaxObj);
        openPop("", ".purchase-pop","选择药物品");
    });

    // 药物品弹窗确认提交按钮
    $(".dept_commit").click(function () {
        var datas = [];
        $(".purchase-table").find("[type=checkbox]:checked").each(function (i,item) {
            item = $(item);
            if ($(".charge-detail-table").find("[productId=" + item.val() + "]").length == 0) {
                datas.push({
                    ProductID:Common.StrUtils.isFalseSetEmpty(item.val()),
                    ProductName: Common.StrUtils.isFalseSetEmpty(item.attr("productNameVal")),
                    Size: Common.StrUtils.isFalseSetEmpty(item.attr("productSizeVal")),
                    UnitName: Common.StrUtils.isFalseSetEmpty(item.attr("productKcName")),
                    MinNum: 1,
                    MaxNum: 1
                });
            }
        });
        $(".charge-detail-table").append(doT.template($(".charge-detail-tmp").text())(datas));
        closeLayer(this);
    });
    // 收费项目详情弹窗删除按钮
    $(".charge-detail-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    $(".close-layer,.submit-btn").click(function () {
        closeLayer(this, function () { $(".charge-detail-table").empty() });
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
    if (Common.StrUtils.isNullOrEmpty($("[name=productPinYin]").val())) {
        layer.msg("拼音码不能为空！", { icon: 2 });
        return false;
    }
    if ($("[name=ChargeCategory]").val() == 0) {
        layer.msg("请选择收费项目类型！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=smartUnitKCDetaiName]").val())) {
        layer.msg("请选择单位！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=size]").val())) {
        layer.msg("规格不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=price]").val())) {
        layer.msg("价格不能为空！", { icon: 2 });
        return false;
    }
    if ($(".charge-detail-table").find("tr").length == 0) {
        layer.msg("请添加药物品！", { icon: 2 });
        return false;
    }
    if (isNaN($("[name=MinNum]").val())) {
        layer.msg("最小值只能是数字！", { icon: 2 });
        return false;
    }
    if (isNaN($("[name=MaxNum]").val())) {
        layer.msg("最大值只能是数字！", { icon: 2 });
        return false;
    }
    return true;
}
var getCargeData = function () {
    var ajaxObj = {
        url: "/Charge/ChargeGet",
        paraObj: {
            data: {
                PinYin: Common.StrUtils.isFalseSetEmpty($("[name=smartProductPinYin]").val()),
                Name: Common.StrUtils.isFalseSetEmpty($("[name=smartProductNmae]").val()),
                CategoryID: Common.StrUtils.isNullOrEmpty($("[name=smartChargeCategory]").val()) || $("[name=smartChargeCategory]").val() == 0 ? -1 : $("[name=smartChargeCategory]").val(),
                Status: Common.StrUtils.isFalseSetEmpty($("[name=smartStatus]").val()),
                PageNum: pageNum,
                PageSize: pageSize
            }
        },
        hasPage: true,
        dotEle: [
            {
                container: ".charge-table",
                tmp: ".charge-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getChargeCategoryData = function () {
    var ajaxObj = {
        url: "/ChargeCategory/ChargeCategoryGet",
        paraObj: {},
        dotEle: [
            {
                container: ".smartChargeCategory,.ChargeCategory",
                tmp: ".chargeCategory-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
getPageData = getCargeData;