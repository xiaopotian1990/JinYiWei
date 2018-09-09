$(function () {
    layui.use(["form", "element", "layer", "laydate"], function () {
        var element = layui.element(),
            laydate = layui.laydate(),
            layer = layui.layer;
        window.form = layui.form();//
        $(".tbody-cust-inshow,.tbody-jrsm-inshow,.tbody-jrfz-inshow,.tbody-jrhz-inshow,.tbody-today-inshow,.tbody-todayAppo-inshow,.tbody-jrsm-inshow").on("click", ".addCustomerTab", function () {
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
    /*项目*/
    $(".smartaddcustomerSymptom").attr("lay-filter", "Symptom");
    /*识别输入框*/
    $(".layui-tab-content input[name='cuident']").css("width", "45%");
    /*清空选择的项目*/
    $(".addSymptom-selected").on("click", "span", function () {
        $(this).remove();

    });
    /*顾客识别*/
    $(".layui-tab-content").on("click", ".online-cu-discbut", function () {
        var cuidentName = $("input[name='cuident']").val();
        if (cuidentName === "" || cuidentName === null) { layer.msg("请输入查询条件!", { icon: 5 }); }
        else {
            var url = "/Customer/CustomerIdentifyAsync";
            var paraObj = {};
            paraObj.data = { name: cuidentName };
            //返回数据
            var result = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($(".cust-inshow-temp").text());

            var html = "<blockquote class='layui-elem-quote'>" + "暂无当前顾客信息" +
                      "<span href='javascript:;' class='layui-btn  layui-btn-small add-customer-info'>" +
                      "<i class='layui-icon'>&#xe608;</i> " + "添加顾客信息" + "</span>" + "</blockquote>";
            //判断返回数据是否为空
            if (result.length > 0) {
                $(".tbody-cust-inshow").html(interText(result));
                $(".add-cu-hint").empty();
            }
            else {
                /*在doT模版输出数据*/
                $(".add-cu-hint").html(html);
                $(".tbody-cust-inshow").empty();

            }
        }
    });
    /*顾客分诊-按钮>弹出页面*/
    $(".layui-tab-content").on("click", ".online-cu-triage-but", function () {
        openPop("", ".data-triage-pop", "顾客分诊");
        var custid = $(this).attr("tr-customerID");
        var custName = $(this).parent().parent().find("td>a").text();
        $("[name='triagecuId'] span").text(custid).css("color", "#FF5722");
        $("[name='triagecuName'] span").text(custName).css("color", "#FF5722");
        var url = "/FrontDesk/GetCustomerInfoBefaultTriageAsync";
        var paraObj = { data: { customerId: custid } };
        var result = ajaxProcess(url, paraObj);
        $("input[name='fz-zxpro-inp']").val(result.Data.Symptom === null ? "暂无" : result.Data.Symptom + ",").attr("disabled", true).css("border", "none");;
        $("input[name='fz-gszxUser-inp']").val(result.Data.ManagerUserName === null ? "暂无" : result.Data.ManagerUserName).attr("disabled", true).css("border", "none");;
        $(".SelectFZUserByHospital").find("[value=" + result.Data.ManagerUserID + "]").attr("selected", "selected");
        form.render();
    });
    /*确认-分诊-添加信息*/
    $(".data-triage-pop").on("click", ".addfz-sub", function () {
        if ($(".SelectFZUserByHospital").val() === "-1") {
            layer.msg("请选择指派咨询师!", { icon: 5 });
            return false;
        }
        if ($(".fz-gszxUser-Remark").val().length > 50) {
            layer.msg("描述备注不可超过50字符!", { icon: 5 });
            return false;
        }
        var url = "/FrontDesk/AddTriageAsync";
        var paraObj = {
            data: {
                SelectID: $(".SelectFZUserByHospital").val(),
                Remark: $(".fz-gszxUser-Remark").val(),
                CustomerID: $("[name='triagecuId'] span").text(),
                Type: 0
            }
        };
        var result = ajaxProcess(url, paraObj);

        if (result.ResultType === 0) {           
            customerTriage($(".SelectFZUserByHospital").val(), '顾客');
            layer.msg(result.Message, { icon: 1 });        
            closeLayer(this);
            /*分诊提示*/
            
        } else {
            layer.msg(result.Message, { icon: 5 });
            closeLayer(this);
        }

    });
    /*手术治疗-确认*/
    $(".data-triage-pop").on("click", ".addfz-zlss-sub", function () {
        if ($(".SelectCategoryByDeptByFz").val() === "-1") {
            layer.msg("请选择部门!", { icon: 5 });
            return false;
        }
        var url = "/FrontDesk/AddTriageAsync";
        var paraObj = {
            data: {
                SelectID: $(".SelectCategoryByDeptByFz").val(),
                CustomerID: $("[name='triagecuId'] span").text(),
                Remark: "",
                Type: 1
            }
        };
        var result = ajaxProcess(url, paraObj);
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1 });
            closeLayer(this);
        } else {
            layer.msg(result.Message, { icon: 5 });
            closeLayer(this);
        }

    });
    /*顾客候诊-按钮>弹出页面*/
    $(".layui-tab-content").on("click", ".online-cu-Wartez-but", function () {
        openPop("", ".data-Wartez-pop", "顾客候诊");
        var custid = $(this).attr("tr-customerID");
        var custName = $(this).parent().parent().find("td")[1].innerHTML;
        $("[name='triagecuId'] span").text(custid).css("color", "#FF5722");
        $("[name='triagecuName'] span").text(custName).css("color", "#FF5722");
        form.render();
    });
    /*候诊提交*/
    $(".data-Wartez-pop").on("click", ".hz-sub", function () {
        if ($(".hzRemark").val().length > 30 || $(".hzRemark").val() === "") {
            layer.msg("候诊描述备注不可为空且不超过30字符!", { icon: 5 });
            return false;
        }
        var url = "/FrontDesk/AddWaitAsync";
        var paraObj = {
            data: {
                Remark: $(".hzRemark").val(),
                CustomerID: $("[name='triagecuId'] span").text()
            }
        };
        var result = ajaxProcess(url, paraObj);
        if (result.ResultType === 0) {
            //layer.close(this);
            layer.msg(result.Message, { icon: 1 });
            setTimeout(function () {
                    location.reload();
                }, 1500);
            return false;
        } else {
            layer.msg(result.Message, { icon: 5 });
            return false;
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
        $("input").val("");
        $("textarea").val("");
        $(".addSymptom-selected").empty();
    });
    /*查询信息后没有该顾客,进行添加按钮*/
    $(".layui-tab-content").on("click", ".add-customer-info", function () {

        openPop("", ".data-addcustomer-pop", "添加顾客信息");
        $("input[name='remindUserName']").attr("readonly", true);
        var cuident = $("input[name='cuident']").val();
        $("input[name='addcuMobile']").attr("value", cuident);
        var phone = $("input[name='addcuMobile']").val();
        getProvinceCityByMobile(phone);
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
            Gender: $("input[name='addcuGender']:checked").val(),
            Mobile: $("input[name='addcuMobile']").val(),
            ChannelID: $(".smartaddcustomerChannel").val(),
            CityID: $("select[name='smartAddCity']").val(),
            CurrentExploitUserID: "",
            Age: $("[name='addcuAge']").val(),
            Birthday: $("[name='addcuBirthday']").val(),
            Address: $("[name='addcuAddress']").val(),
            SymptomIDS: symptomCadata,
            ToolID: $(".smartaddcustomerChannel").val(),
            ConsultContent: $("textarea").val(),
            StoreID: ""
        };
        var url = "/Customer/AddAsyncByFore";
        var result = ajaxProcess(url, paraObj);

        if (result) {
            if (parseInt(result.ResultType === 0)) {
                layer.confirm(result.Message, { icon: 1 }, {
                    btn: ["继续添加", "取消关闭"] //按钮
                }, function () {
                    $(".data-addcustomer-pop").empty();
                    openPop("", ".data-addcustomer-pop", "添加顾客信息");
                }, function () {
                    closeLayer($(".layui-layer"));
                });

            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
    /*接诊人员列表*/
    $(".layui-field-box").on("click", ".jzry-cli", function () {
        getFzUser();
    });
    /*今日预约上门列表*/
    $(".layui-field-box").on("click", ".jryysm-cli", function () {
        getAppointmentComeToday();
    });
    /*今日上门列表*/
    $(".layui-field-box").on("click", ".jrsm-cli", function () {
        getVisitTodayAsync();
    });
    /*今日分诊列表*/
    $(".layui-field-box").on("click", ".jrfz-cli", function () {
        getTriageTodayAsync();
    });
    /*今日候诊列表*/
    $(".layui-field-box").on("click", ".jrhz-cli", function () {
        GetWaitTodayAsync();
    });
    /*今日新登记顾客*/
    $(".layui-field-box").on("click", ".today-cu-click", function () {
        /*共用方法示例
        var obj = {
            url: "/Customer/CustomerCreateTodayAsyncByForeGround",
            paraObj : {},
            isUpdate : false,
            dotEle : [{ container: ".tbody-today-inshow", tmp: ".fd-tbody-today-inshow-temp" }],
            isDataRoot : false,
            hasPage : false
        }
        dataFunc(obj);*/
        getCustomerByForeGround();
    });
    /*今日新增预约*/
    $(".layui-field-box").on("click", ".today-addAppo-click", function () {
        getAppointmentToday();
    });
});
//验证提交表单
var verify = function () {
    var cuname = $("[name='addcuName']").val();
    var addcuMobile = $("[name='addcuMobile']").val();
    var addcuBirthday = $("[name='addcuBirthday']").val();
    var addcuAddress = $("[name='addcuAddress']").val();
    var addcuProvince = $(".customerProvince").val();
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

    if (addcuProvince === "-1") {
        layer.msg("请选择省市！", { icon: 5 });
        return false;
    }
    if (addChannel === "-1") {
        layer.msg("请选择渠道！", { icon: 5 });
        return false;
    }
    if ($(".addSymptom-selected").find("span").length === 0) {
        layer.msg("请选择咨询症状！", { icon: 5 });
        return false;
    }
    return true;
}
/*查询市级*/
var getCity = function () {
    var data = $("#customerProvince").find(":selected");

    var url = "/Customer/GetCity";
    var paraObj = { data: { provinceID: data.val() } };
    var result = ajaxProcess(url, paraObj).Data;

    /*清空页面上部门下拉框的值*/
    $(".City-selected").empty().append("<option value>请选择</option>");
    $.each(result, function (i, item) {
        /*查询出来的赋值给页面上的下拉框*/
        $("select[name='smartAddCity']").append("<option value=" + item.ID + ">" + item.Name + "</option>");
    });
};
/*根据手机号查询省市*/
var getProvinceCityByMobile = function (phone) {

    var url = "/Customer/GetProvinceCityByPhone";
    var paraObj = { data: { phone: phone } };
    var result = ajaxProcess(url, paraObj).Data;
    // $(".customerProvince").attr("selected", "");

    $(".customerProvince").find("[value=" + result.ProvinceID + "]").attr("selected", "selected");
    getCity();
    $("select[name='smartAddCity']").find("[value=" + result.CityID + "]").attr("selected", "selected");
    form.render();
}
/*接诊人员列表*/
var getFzUser = function () {

    var url = "/User/GetFzUsers";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj);
    console.log(result);
    var interText = doT.template($(".fd-jzry-inshow-temp").text());
    var html = "<blockquote class='layui-elem-quote'>" + " 暂无接诊人员" + "</blockquote>";
    if (result.ResultType === 0) {
        //判断返回数据是否为空
        if (result.Data === null) {
            $(".jzry-info").html(html);
            $(".tbody-jzry-inshow").empty();
        } else {
            $(".tbody-jzry-inshow").html(interText(result.Data));
        }
    } else {
        layer.msg(result.Message);
    }
};
/*今日预约上门列表*/
var getAppointmentComeToday = function () {
    var url = "/Appointment/GetAppointmentComeToday";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".fd-jryysm-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + " 暂无预约顾客" + "</blockquote>";

    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".jryysm-info").html(html);
        $(".tbody-jryysm-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-jryysm-inshow").html(interText(result));
    }
};
/*今日上门记录*/
var getVisitTodayAsync = function () {
    var url = "/FrontDesk/GetVisitTodayAsync";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;

    var interText = doT.template($(".fd-jrsm-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + " 暂无上门顾客" + "</blockquote>";

    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".jrsm-info").html(html);
        $(".tbody-jrsm-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-jrsm-inshow").html(interText(result));
    }
};
/*今日分诊记录*/
var getTriageTodayAsync = function () {
    var url = "/FrontDesk/GetTriageTodayAsync";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;

    var interText = doT.template($(".fd-jrfz-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + " 暂无分诊顾客" + "</blockquote>";

    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".jrfz-info").html(html);
        $(".tbody-jrfz-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-jrfz-inshow").html(interText(result));
    }
};
/*今日候诊记录*/
var GetWaitTodayAsync = function () {
    var url = "/FrontDesk/GetWaitTodayAsync";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".fd-jrhz-inshow-temp").text());
    var html = "<blockquote class='layui-elem-quote'>" + " 暂无候诊顾客" + "</blockquote>";
    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".jrhz-info").html(html);
        $(".tbody-jrhz-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-jrhz-inshow").html(interText(result));
    }
};
/*今日新登记顾客*/
var getCustomerByForeGround = function () {
    var url = "/Customer/CustomerCreateTodayAsyncByForeGround";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".fd-tbody-today-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + " 今日暂无登记顾客" + "</blockquote>";

    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".notoday-info").html(html);
        $(".tbody-today-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-today-inshow").html(interText(result));
    }
};
/*今日新增预约*/
var getAppointmentToday = function () {
    var url = "/Appointment/GetAppointmentToday";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".fd-tbody-todayAppo-inshow-temp").text());

    var html = "<blockquote class='layui-elem-quote'>" + " 今日暂无新增预约顾客" + "</blockquote>";

    //判断返回数据是否为空
    if (result.length <= 0 || result === undefined) {
        $(".notodayAppo-info").html(html);
        $(".tbody-todayAppo-inshow").empty();
    }
    else {
        /*在doT模版输出数据*/
        $(".tbody-todayAppo-inshow").html(interText(result));
    }
};