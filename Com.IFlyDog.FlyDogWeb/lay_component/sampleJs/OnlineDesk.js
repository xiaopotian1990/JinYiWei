$(function() {
layui.use(["form", "element", "layer", "laydate"], function () {
        var element = layui.element(),
            laydate=layui.laydate(),
            layer = layui.layer;
             window.form = layui.form();
             $(".tbody-cust-inshow,.tbody-today-inshow,.tbody-todayAppo-inshow").on("click", ".addCustomerTab", function () {
                 var id = $(this).parent().prev().text();
                 parent.layui.tab({
                     elem: ".admin-nav-card"
                 }).tabAdd(
                 {
                     title: "客户档案:" + $(this).text() + "编号:" + id,
                     href: "/Customer/CustomerProfile", //地址
                     icon: "fa-user"
                 });
             });

            form.on("select(Province)", function () {
                    getCity();
                    /*更新全部 参数值 可指定刷新form.render("select");select,checkbox,radio*/
                    form.render();
            });
      
            form.on("select(Symptom)", function (ele) {
                if ($(".addSymptom-selected").find("[symptomId=" + ele.value + "]").length === 0) {
                    $(".addSymptom-selected").append("<span class='layui-btn-small layui-btn m-bt-10' symptomId=" + ele.value + ">" + $(ele.elem).find(":selected").text() + " <i class='layui-icon'>&#xe640;</i></span>");
                }
            });
    });
    /*省级*/
    $(".customerProvince").attr("lay-filter", "Province");
    /*市级*/
    $("#smartAddCity").attr("lay-filter", "City");
    /*项目*/
    $(".smartaddcustomerSymptom").attr("lay-filter", "Symptom");

    $(".khsbCli").click();
    $(".layui-tab-content input[name='cuident']").css("width", "45%");
/*顾客识别*/
$(".layui-tab-content").on("click", ".online-cu-discbut", function () {
    var cuidentName = $("input[name='cuident']").val();
    if (cuidentName === "" || cuidentName === null) {
        layer.msg("请输入查询条件!", { icon: 5 });
    } else {
    
    var url = "/Customer/CustomerIdentifyAsync";

    var paraObj = {};
    paraObj.data= {
        name: cuidentName
    }
    //返回数据
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".cust-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + "暂无当前顾客信息" +
              "<span href='javascript:;' class='layui-btn  layui-btn-small add-customer-info'>" +
              "<i class='layui-icon'>&#xe608;</i> " + "添加顾客信息" + "</span>" + "</blockquote>";
    //判断返回数据是否为空
    if (result.length <= 0) {
        $(".add-cu-hint").html(html);
        $(".tbody-cust-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
     
        $(".tbody-cust-inshow").html(interText(result));
        $(".add-cu-hint").empty();
    }
    }
});
/*顾客识别重置值按钮*/
$(".layui-tab-content").on("click", ".online-cu-resetbut", function () {
    $(".layui-tab-content input[name='cuident']").val("");
    $(".tbody-cust-inshow").empty();
    $(".add-cu-hint").empty();
});
/*添加新顾客重置值按钮*/
$(".data-addcustomer-pop").on("click", ".add-cu-reset", function () {
    $(".data-addcustomer-pop form")[0].reset();
    $(".addSymptom-selected").empty();
    $("input[name='remindUserName']").val('');
});

/*查询信息后没有该顾客,进行添加按钮*/
$(".layui-tab-content").on("click", ".add-customer-info", function () {
    
    openPop("", ".data-addcustomer-pop", "添加顾客信息");
    $("input[name='remindUserName']").attr("readonly", true);
    var cuident = $("input[name='cuident']").val();
    $("input[name='addcuMobile']").val(cuident);
    $("input[name='remindUserName']").val('');
    $(".addSymptom-selected").empty();
    var phone = $("input[name='addcuMobile']").val();
    getProvinceCityByMobile(phone);
});

/*清空选择的项目*/
$(".addSymptom-selected").on("click", "span", function () {
    $(this).remove();
  
});

/*添加顾客弹窗中-选择开发人员按钮*/
$(".data-addcustomer-pop").on("click", "input[name='remindUserName']", function () {
    openPop("", ".data-addcustomer-remindUser-pop", "选择开发人员");
    form.render();
});
/*添加顾客弹窗中-选择开发人员页面中搜索*/
$(".layui-field-box").on("click", ".search-user", function () {
    currentExploitUser();

});
/*添加顾客弹窗中-选择开发人员页面中-选择该用户*/
$(".layui-field-box").on("click", ".present-user", function () {
    var userid = $(this).attr("value");
    var username = $(this).parent().parent().find("td")[1].innerHTML;
    $(".remindUsercl").attr("value", userid);
    $(".remindUsercl").val(username);
    if (userid !== "0" || username !== "")
    {
        layer.msg("开发人员选择成功!", { icon: 6 });
        closeLayer(this);
    }
});
/*添加顾客-确认提交*/
$(".data-addcustomer-pop").on("click", ".add-cu-submit", function () {

    if (!verify()) { return false; };
    var symptomCadata = [];
    $(".addSymptom-selected").find("span").each(function (i, item) {
        item = $(item);
        
        symptomCadata.push(item.attr("symptomId"));
    });
    var paraObj = {};
    paraObj.data = {
            Name: Common.StrUtils.isFalseSetEmpty($("[name='addcuName']").val()),
            Gender: $("[name='addcuGender']:checked").val(),
            Mobile: $("[name='addcuMobile']").val(),
            ChannelID: $(".smartaddcustomerChannel").val(),
            CityID: $("select[name='smartAddCity']").val(),
            CurrentExploitUserID: $("input[name='remindUserName']").attr("value"),
            Age: $("[name='addcuAge']").val(),
            Birthday: $("[name='addcuBirthday']").val(),
            Address: $("[name='addcuAddress']").val(),
            SymptomIDS: symptomCadata,
            ToolID: $(".smartaddcustomerChannel").val(),
            ConsultContent: $("textarea").val(),
            StoreID: ""
    };
    var url = "/Customer/AddAsync";
    var result = ajaxProcess(url, paraObj);

    if (result) {
        if (result.ResultType == 0) {
            layer.msg(result.Message, { icon: 1 });
            $("input[name='remindUserName']").val('');
            $(".addSymptom-selected").empty();
            $(".data-addcustomer-pop form")[0].reset();
        } else {
            layer.msg(result.Message, { icon: 5 });
        }

    };
    return false;
});
 /*今日新登记顾客*/
$(".layui-field-box").on("click",".today-cu-click",function() {
    gettodayCustomerInfo();
});
/*今日新增预约*/
$(".layui-field-box").on("click", ".today-addAppo-click", function() {
    getAppointmentTodayInfo();
});
});

//验证提交表单
var verify = function () {
    var cuname = $("[name='addcuName']").val();
    var addcuMobile = $("[name='addcuMobile']").val();
    
    var addcuBirthday = $("[name='addcuBirthday']").val();
    var addcuAddress = $("[name='addcuAddress']").val();
    var addcuProvince = $(".customerProvince").val();
    var addremindUserName = $("[name='remindUserName']").val();
    var addChannel = $(".smartaddcustomerChannel").val();
    if (Common.StrUtils.isNullOrEmpty(cuname)) {
        layer.msg("顾客名称不可为空！", { icon: 5 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(addcuMobile)) {
        layer.msg("顾客联系方式不可为空！", { icon: 5 });
        return false;
    }
  
    if (Common.StrUtils.isNullOrEmpty(addcuBirthday)) {
        layer.msg("顾客生日不可为空！", { icon: 5 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(addcuAddress)) {
        layer.msg("顾客地址不可为空！", { icon: 5 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(addremindUserName)) {
        layer.msg("请选择开发人员！", { icon: 5 });
        return false;
    }
    if (addcuProvince === "-1" ) {
        layer.msg("请选择省市！", { icon: 5 });
        return false;
    }
    if (addChannel === "-1") {
        layer.msg("请选择渠道！", { icon: 5 });
        return false;
    }
    if ($(".addSymptom-selected").find("span").length === 0) {
        layer.msg("请选择咨询项目！", { icon: 5 });
        return false;
    }
    return true;
}

/*查询市级*/
var getCity = function() {
    var data = $("#customerProvince").find(":selected");

    var url = "/Customer/GetCity";
    var paraObj = { data: { provinceID: data.val() } };
    var result = ajaxProcess(url, paraObj).Data;

    /*清空页面上部门下拉框的值*/
    $(".City-selected").empty().append("<option value>请选择</option>");
    $.each(result,function (i, item) {
            /*查询出来的赋值给页面上的下拉框*/
            $("select[name='smartAddCity']").append("<option value=" + item.ID + ">" + item.Name + "</option>");
        }); 
};

/*根据手机号查询省市*/
var getProvinceCityByMobile = function (phone) {

    var url = "/Customer/GetProvinceCityByPhone";
    var paraObj = { data: { phone: phone } };
    var result = ajaxProcess(url, paraObj).Data;
   
    $(".customerProvince").find("[value=" + result.ProvinceID + "]").attr("selected", "selected");
    getCity();
    $("select[name='smartAddCity']").find("[value=" + result.CityID + "]").attr("selected", "selected");
    form.render();
}
 
/*今日新登记顾客*/
var gettodayCustomerInfo = function () {
    var url = "/Customer/CustomerCreateTodayAsync";
    var paraObj = {  };
    var result = ajaxProcess(url, paraObj).Data;
    
    var interText = doT.template($(".today-incushow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>"+"今日暂无登记顾客"+"</blockquote>";
    //判断返回数据是否为空
    if (result.length ==="") {
        $(".notoday-info").html(html);
        $(".tbody-cust-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-today-inshow").html(interText(result));
    }
}
/*今日新增预约*/
var getAppointmentTodayInfo= function() {
    var url = "/Appointment/GetAppointmentToday";
    var paraObj = { };
    var result = ajaxProcess(url, paraObj).Data;

    var interText = doT.template($(".today-Appointment-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + "今日暂无预约顾客" + "</blockquote>";
    //判断返回数据是否为空
    if (result.length <= 0) {
        $(".notodayAppo-info").html(html);
        $(".tbody-todayAppo-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-todayAppo-inshow").html(interText(result));
    }
}
/*选择开发人员*/
var currentExploitUser = function () {
    var url = "/SmartUser/SmartUserGet";
    var paraObj = {
        data: {
            Name: $("input[name='addcuuserNmae']").val(),
            DeptId: $(".smartaddcuDept").val(),
            PageSize:999,
            PageNum :1
        }  
    };
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".user-tmp").text());
    var html = "<blockquote class='layui-elem-quote'>" + "没有该用户信息!请核实!" + "</blockquote>";
    if (result.length <= 0) {
        $(".user-info").html(html);
        $(".user-table").empty();
    } else {
        $(".user-table").html(interText(result));
        $(".user-info").html("");
    }
 
}

