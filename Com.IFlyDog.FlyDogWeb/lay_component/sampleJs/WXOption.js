$(function () {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.form = layui.form();
        getOptionData();
        form.on("select(ChannelSpecial)", function (data) {
            if ($(data.elem).parent().siblings(".sub-list").find("[data-val=" + data.value + "]").length > 0) {
                return;
            }
            $(data.elem).parent().siblings(".sub-list").append('<div class="sub-row layui-btn layui-btn-small"><span class="sub-col" name="ID" data-val="' + data.value + '">' + $(data.elem).find(":selected").text() + '<i class="layui-icon">&#xe640;</i></span></div>');
        });
    });
    // 编辑按钮
    $(".option-table").on("click", ".btn-edit", function () {
        $(this).hide().siblings(".submit-btn").css("display", "inline-block").parents("tr").find("input,textarea,select").prop("disabled", false);
        form.render();
    });
    // 编辑按钮
    $(".option-table").on("click", ".edit-pro", function () {
        var _this = $(this);
        _this.parents("tr").find("table").find("[hidden]").prop("hidden", false);
        _this.parents("tr").find(".add-pro").toggleClass("hide");
        form.render();
    });
    // 表格中保存按钮提交
    $(".option-table").on("click", ".submit-btn", function () {
        var url = $(this).data("url");
        var data = {},
            fieldName = "Option" + url.replace("Fun", "");
        // 循环保存按钮所在行的所有表单元素
        $(this).parents("tr").find("input,textarea,select,.sub-list:not(.layui-unselect)").each(function (i, item) {
            item = $(item);
            var name = item.attr("name");
            // 判断是否是单选框
            if (item.is("[type=radio]")) {
                params.setDataParam(name + "Value", item.parents("td").find("[name=" + name + "]:checked").val()).setDataParam(name + "Code", item.data("index"));
            } else if (item.is(".sub-list")) {
                var subList = [];
                item.find(".sub-row").each(function (j, tr) {
                    tr = $(tr);
                    var objCol = {};
                    tr.find(".sub-col").each(function (k, subCol) {
                        subCol = $(subCol);
                        objCol[subCol.attr("name")] = subCol.data("val");
                    });
                    subList.push(objCol);
                });
                params.setDataParam(name, subList);
            } else if (item.is(":not([type=radio]):not(.sub-list)")) {// 判断是否是单选框以外的表单元素
                params.setDataParam(name + "Value", item.val()).setDataParam(name + "Code", item.data("index"));
            }
        });
        if (ajaxObj.setUrl("/WXOption/" + url).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
            $(this).hide().siblings(".btn-edit").css("display", "inline-block").parents("tr").find("input,textarea").prop("disabled", true);
            form.render();
            closeLayer(this);
        }

    });
    // 项目弹窗提交
    $(".category.submit-btn").click(function () {
        $(".category-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("[name=UserCouponCategoryName]").val(item.data("name"));
            $("[name=UserCouponCategory]").val(item.val());
        });
        closeLayer(this);
    });
    // 弹窗选择项目按钮
    $(".option-table").on("click", "[name=UserCouponCategoryName]", function () {
        if (getCategoryData()) {
            openPop("", ".category-pop", "选择分类");
        }
    });
    $(".category-table").on("click", "[type=checkbox]", function () {
        var _this = $(this);
        _this.parents("table").find(":checked").not(_this).prop("checked",false);
    });
    $(".option-table").on("click", ".add-pro", function () {
        var _this = $(this);
        var tableEle = _this.siblings("table");
        if (tableEle.find(".confirm-add").length > 0) return false;
        var trEle = $("<tr class='sub-row'><td class='sub-col' name='Level' contentEditable ></td><td class='sub-col' name='Rate' contentEditable ></td><td><span class='layui-btn layui-btn-mini confirm-add'>确认</span></td></tr>");
        tableEle.find(".sub-list").append(trEle);
        trEle.find("td").eq(0).focus();
    });
    $(".option-table").on("click", ".confirm-add", function () {
        var _this = $(this);
        _this.parents("tr").find("td").attr("contentEditable", false);
        _this.toggleClass("btn-rmv").text("删除").toggleClass("confirm-add");
    });
    $(".option-table").on("click", ".btn-rmv", function () {
        var _this = $(this);
        _this.parents("tr").eq(0).remove();
    });
    $(".option-table").on("click", "[name=SpecialChannelAddInfoList] .sub-row", function () {
        $(this).remove();
    });
    $(".option-table").on("keyup", "[name=PromoteLevelAddInfo] .sub-col", function () {
        var _this = $(this);
        _this.data("val",_this.text());
    });
});

var getOptionData = function () {
    var dotEle = [{ container: ".option-table", tmp: ".option-tmp" }];
    ajaxObj.setUrl("/WXOption/WXOptionGet").setDotEle(dotEle).getData();
}
var getCategoryData = function () {
    var dotEle = [{ container: ".category-table", tmp: ".category-tmp" }];
    ajaxObj.setUrl("/CouponCategory/CouponCategoryInfoGetByHospitalID").setParaObj().setDotEle(dotEle).getData();
    return true;
}
var getPageData = getOptionData;