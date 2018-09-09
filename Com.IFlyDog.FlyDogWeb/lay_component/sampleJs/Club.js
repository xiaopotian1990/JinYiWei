$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getPageData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("ClubAdd", ".club-pop", "单项目");
    });
    // 弹窗提交
    $(".club.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var chargeCategoryID = "",
            chargeID = "";
        if ($("[name=useScope]:checked").data("id") == 1) {
            chargeCategoryID = $("[name=chargeCategoryId]").val();
        } else {
            chargeID = $("[name=chargeId]").val();
        }
        var ajaxObj = {
            url: "/Club/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    ScopeLimit: $("[name=useScope]:checked").data("id"),
                    ChargeCategoryID: chargeCategoryID,
                    ChargeID: chargeID,
                    Icon: Common.StrUtils.isFalseSetEmpty($("[name=icon]").data("url")),
                    Remark: Common.StrUtils.isFalseSetEmpty($("[name=remark]").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
            $("[name=icon]").src("").data("url", "");
        }

    });
    // 删除按钮
    $(".club-table").on("click", ".btn-remove", function () {
        var smartProductCategoryDelId = Common.StrUtils.isFalseSetEmpty($(this).data("id"));
        layer.confirm('您确定删除本条数据？',{ btn: ['确定', '取消'] },function () {
            var ajaxObj = {
                url: "/Club/ClubDelete",
                paraObj: {
                    data: {
                        ID: smartProductCategoryDelId
                    }
                },
                isUpdate: true
            };
            dataFunc(ajaxObj);
        },function () {
            layer.msg('已经取消此操作',{icon: 6});
        });

    });
    // 弹窗选择项目分类按钮
    $("[name=chargeCategoryName]").click(function () {
        if (getChargeCategoryData()) {
            openPop("", ".chargeCategory-pop", "选择收费项目");
        }
    });
    $("[name=chargeName]").click(function () {
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择收费项目");
        }
    });
    // 项目弹窗提交
    $(".charge.submit-btn").click(function () {
        var chargeItem = $(".charge-table").find("[type=checkbox]:checked");
        $("[name=chargeId]").val(chargeItem.val())
        $("[name=chargeName]").val(chargeItem.attr("title"));
        closeLayer(this);
    });
    // 项目分类和项目弹窗弹窗复选框选中
    $(".chargeCategory-table,.charge-table").on("change", "[type=checkbox]", function () {
        $(this).parents("table").find("[type=checkbox]").not(this).prop("checked", false);
    });
    // 项目弹窗提交
    $(".chargeCategory.submit-btn").click(function () {
        var chargeCategoryItem = $(".chargeCategory-table").find("[type=checkbox]:checked");
        $("[name=chargeCategoryId]").val(chargeCategoryItem.val())
        $("[name=chargeCategoryName]").val(chargeCategoryItem.attr("title"));
        closeLayer(this);
    });
    // 上传图片按钮
    $(".upload-btn").click(function () {
        $("#hideFile").click();
    });
    $("#hideFile").change(function (data) {
        FileUpload("share", function (data) {
            $("[name=icon]").data("url", data.Data).attr("src", data.Data);
        }, "hideFile");
    });
    $(".search-btn").click(function () {
        getChargeData();
    });
    $(".close-layer").click(function () {
        $("[name=icon]").src("").data("url", "");
    });
});
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == "chargeCategory" && Common.StrUtils.isNullOrEmpty($("[name=chargeCategoryId]").val())) {
        layer.msg("请选择收费项目分类", { icon: 2 });
        return false;
    }
    if ($("[name=useScope]:checked").val() == "charge" && Common.StrUtils.isNullOrEmpty($("[name=chargeId]").val())) {
        layer.msg("请选择收费项目", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=icon]").data("url"))) {
        layer.msg("请选择图标！", { icon: 2 });
        return false;
    }
    return true;
}
var getClubData = function () {
    var ajaxObj = {
        url: "/Club/ClubtGet",
        paraObj: {},
        dotEle: [
            {
                container: ".club-table",
                tmp: ".club-tmp"
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
var getPageData = getClubData;