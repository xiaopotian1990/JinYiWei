$(function () {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.form = layui.form();
        getOptionData();
    });
    // 编辑按钮
    $(".option-table").on("click", ".btn-edit", function () {
        $(this).hide().siblings(".submit-btn").css("display", "inline-block").parents("tr").find("input,textarea").prop("disabled", false);
        form.render();
    });
    // 表格中保存按钮提交
    $(".option-table").on("click",".submit-btn",function () {
        var url = $(this).data("url");
        var data = {},
            fieldName = "Option" + url.replace("Fun", "");
        // 循环保存按钮所在行的所有表单元素
        $(this).parents("tr").find("input,textarea").each(function (i,item) {
            item = $(item);
            var name = item.attr("name");
            // 判断是否是单选框
            if(item.is("[type=radio]")){
                data[name + "Value"] = item.parents("td").find("[name=" + name + "]:checked").val();
                data["Option" + item.data("index")] = item.data("index");
            }
            // 判断是否是单选框以外的表单元素
            if(item.is(":not([type=radio])")){
                data[name + "Value"] = item.val();
                data["Option" + item.data("index")] = item.data("index");
            }
        });
        var ajaxObj = {
            url: "/Option/" + url,
            paraObj: {
                data: data
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            $(this).hide().siblings(".btn-edit").css("display", "inline-block").parents("tr").find("input,textarea").prop("disabled", true);
            form.render();
            closeLayer(this);
        }

    });
    $(".option-table").on("click", ".btn-edit.appointment", function () {
        MakeBeginTime = new TimePlugin($("[name=MakeBeginTimeCode]").val(), ".timePluginBegin").setSelectFunc(function (time,timer) {
            $("[name=MakeBeginTimeCode]").val(time);
        }).init();
        MakeEndTime = new TimePlugin($("[name=MakeEndTimeCode]").val(), ".timePluginEnd").setSelectFunc(function (time, timer) {
            $("[name=MakeEndTimeCode]").val(time);
        }).init();
    });
    // 项目弹窗提交
    $(".charge.submit-btn").click(function () {
        $(".charge-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("[name=RegistrationChargeNameCode]").val(item.attr("title"));
            $("[name=RegistrationChargeCode]").val(item.val());
        });
        closeLayer(this);
    });
    // 弹窗选择项目按钮
    $(".option-table").on("click","[name=RegistrationChargeNameCode]",function () {
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择收费项目");
        }
    });
    $(".charge-pop").on("change", "[type=checkbox]", function () {
        $(this).parents("table").find("[type=checkbox]").not(this).prop("checked", false);
    });
    $(".search-btn").click(function () {
        getChargeData();
    });
    

});
var MakeBeginTime = null, MakeEndTime = null;
var getOptionData = function () {
    var ajaxObj = {
        url: "/Option/OptionGet",
        paraObj: {}, 
        dotEle: [
            {
                container: ".option-table",
                tmp: ".option-tmp"
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
var getPageData = getOptionData;