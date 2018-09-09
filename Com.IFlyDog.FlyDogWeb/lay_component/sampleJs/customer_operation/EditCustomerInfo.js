$(function() {
    //@ sourceURL=EditCustomerInfo.js
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate=layui.laydate();
            window.form = layui.form();
            form.on("select(Province)", function () {
                getCity();
                form.render();
            });
    });
    
    var cuId = $("input[name='hicutomerId']").attr("hicutomerid");
    /*省级*/
    $(".editCustomerProvince").attr("lay-filter", "Province");
    //修改提交信息
    $(".edit-cutomer-btn").click(function () {
        if (!verify()) {
            return false;
        }
       
        var ajaxObj = {
            url: "/CustomerProfile/CustomerInfoUpdate",
            paraObj: {
                data: {
                    CustomerID:cuId,
                    CustomerName: $("[name='editCustomerName']").val(),
                    Gender: $("select[name='editCustomercGender']").val(),
                    Address: $("[name='editCustomerAddress']").val(),
                    Remark:$("textarea").val(),
                    Birthday: $("[name='editCustomerBirthday']").val().substring(0, 10),
                    CityID: $("select[name='editCustomerCity']").val(),
                    WeChat: $("input[name='editCustomerWeChat']").val(),
                    QQ: $("input[name='editCustomerQQ']").val(),
                    Custom1: $("input[name='editCustomerCustom1']").val(),
                    Custom2: $("input[name='editCustomerCustom2']").val(),
                    Custom3: $("input[name='editCustomerCustom3']").val(),
                    Custom4: $("input[name='editCustomerCustom4']").val(),
                    Custom5: $("input[name='editCustomerCustom5']").val(),
                    Custom6: $("input[name='editCustomerCustom6']").val(),
                    Custom7: $("input[name='editCustomerCustom7']").val(),
                    Custom8: $("input[name='editCustomerCustom8']").val(),
                    Custom9: $("input[name='editCustomerCustom9']").val(),
                    Custom10: $("input[name='editCustomerCustom10']").val()    
                }
            }
        };
        var result = dataFunc(ajaxObj);
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1 });
            closeLayer(".site-text");
            getCustomerDetail();
        } else {
            layer.msg(result.Message, { icon: 5 });
        }

    });
  
});
//验证提交表单
var verify = function () {
    var cuname = $("[name='editCustomerName']").val();
    var editGender = $("select[name='editCustomercGender']").val();
    var editcuBirthday = $("[name='editCustomerBirthday']").val();
    var editcuAddress = $("[name='editCustomerAddress']").val();
  
    var editcuCitye = $(".editCustomerCity").val();
    if (Common.StrUtils.isNullOrEmpty(cuname)) {
        layer.msg("顾客名称不可为空！", { icon: 5 });
        return false;
    }
    if (editGender === "-1") {
        layer.msg("请选择顾客性别！", { icon: 5 });
        return false;
    }
  
    if (editcuCitye === "-1") {
        layer.msg("请选择市！", { icon: 5 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(editcuAddress)) {
        layer.msg("顾客地址不可为空！", { icon: 5 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(editcuBirthday)) {
        layer.msg("顾客生日不可为空！", { icon: 5 });
        return false;
    }
    
 
    return true;
}
/*查询市级*/
var getCity = function () {
    var data = $(".editCustomerProvince").find(":selected");

    var url = "/Customer/GetCity";
    var paraObj = { data: { provinceID: data .val()} };
    var result = ajaxProcess(url, paraObj).Data;

    /*清空页面上部门下拉框的值*/ 
    $(".editCustomerCity").empty().append("<option>请选择</option>");
    $.each(result, function (i, item) {
        /*查询出来的赋值给页面上的下拉框*/
        $(".editCustomerCity").append("<option value=" + item.ID + ">" + item.Name + "</option>");
    });
};
