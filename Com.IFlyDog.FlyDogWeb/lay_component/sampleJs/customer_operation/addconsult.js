$(function () {
    //@ sourceURL=addconsult.js
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        //选择咨询项目
        form.on("select(AddcustomerSymptom)", function (ele) {
            if ($(".add-selected").find("[symptomId=" + ele.value + "]").length === 0) {
                if (ele.value === "-1") {
                    layer.msg("请正确选择", { icon: 5 }); return false;
                } else {
                    $(".add-selected").append("<span class='layui-btn-small layui-btn m-bt-10' symptomId=" + ele.value + ">" +
                        $(ele.elem).find(":selected").text() + " <i class='layui-icon'>&#xe640;</i></span>");
                }
            }
        });
    });
     var custid =$("input[name='hicutomerId']").attr("hicutomerId");
    $(".AddcustomerSymptom").attr("lay-filter", "AddcustomerSymptom");
    /*删除选择的项目*/
    $(".add-selected").on("click", "span", function () { $(this).remove(); });

    //咨询提交
    $(".add-sub").on("click", function () { 
        if (!verify()) { return false; }
        var symptomCadata = [];
        $(".add-selected").find("span").each(function (i, item) {
            item = $(item);
            symptomCadata.push(item.attr("symptomId"));
        });
        var paraObj = {};
        paraObj.data = {
            CustomerID: custid,
            ID: $("input[name=consultId]").attr("consultid") === "" ? "" : $("input[name=consultId]").attr("consultid"),
            ToolID: $(".AddcustomerTool").val(),
            SymptomIDS: symptomCadata,
            Content: $("textarea[name='AddRemark']").val()
        };

        var url = "/Consult/" + $(this).parents(".layui-layer").data("url");
        var result = ajaxProcess(url, paraObj);
        if (result) {
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1, time: 1000 });
                closeLayer(this);
                getConsultInfo();

            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
});
var verify = function () {
    return true;
};
