$(function () {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.form = layui.form();
        getShiftData();
    });
    // 表格点击事件
    $(".shift-table").on("click", "[data-date]", function () {
        $(".info-submit").data({ "id": $(this).parent().data("id"), "date": $(this).data("date") });
        $(".info-remove").data({ "id": $(this).data("id")});
        $(".info-user").text($(this).siblings(".t-user").text());
        $(".info-date").text($(this).data("date"));
        var url = "ShiftAdd";
        if (!Common.StrUtils.isNullOrEmpty($(this).data("categoryid"))) {
            url = "ShiftSubmit";
            $(".info-remove").removeClass("hide");
            $("#infoShiftCategory").find("[value=" + $(this).data("categoryid") + "]").prop("selected", true);
        }
        form.render();
        openPop(url, ".shiftInfo-pop", "排班信息");
    });
    // 提交按钮
    $(".info-submit").click(function () {

        var shiftCategoryId = $("#infoShiftCategory").val();
        var url = $(this).parents(".layui-layer").data("url");
        var data = {};
        if (url == "ShiftAdd") {
            var UserInfoList = [],
                ShiftDateList = [];
            ShiftDateList.push({
                UserId: $(this).data("id"),
                ShiftDataTime: $(this).data("date"),
                ShiftCategoryID: shiftCategoryId
            });
            UserInfoList.push({ UserId: $(this).data("id") });
            data.UserInfoList= UserInfoList;
            data.ShiftDateList= ShiftDateList;
        } else {
            data.UserId= $(this).data("id");
            data.DataTimeShif = $(this).data("date");
            data.ShiftCategoryID= shiftCategoryId;
            $(".info-remove").removeClass("hide");
        }
        shiftUpdate(url, data,this, function () { $(".info-remove").addClass("hide"); });
    });
    $(".info-remove").click(function () {
        var data = { ID:$(this).data("id")};
        shiftUpdate("ShiftDelete", data, this, function () { $(".info-remove").addClass("hide"); });
    });

    $(".self").click(function () {
        incr = 0;
        getShiftData();
    });
    $(".last").click(function () {
        incr--;
        getShiftData();
    });
    $(".next").click(function () {
        incr++;
        getShiftData();
    });
    $(".btn-add").click(function () {
        openPop("", ".shift-pop","批量添加");
    });
    $(".reset-shift").click(function () {
        $(this).parent().find(".sshift-table").empty();
    });
    $(".add-user").click(function () {
        UserInfo.setUrl("/SmartUser/SmartCYPBUserGet").useHospital().setConfimFunc(function (userInfo) {
            if ($(".suser-table").find("[data-id=" + userInfo.id + "]").length == 0) {
                $(".suser-table").append("<tr data-id='" + userInfo.id + "' data-name='" + userInfo.name + "' data-account='" + userInfo.account + "'><td>" + userInfo.account + "</td><td>" + userInfo.name + "</td><td><span class='layui-btn layui-btn-mini btn-remove'>删除</span></td></tr>");
            }
        }).openPop();
    });
    $(".add-shift").click(function () {
        if ($("#ShiftCategory").val() == -1) {
            layer.msg("请选择分类", { icon: 2 });
            return false;
        }
        var startTime = getDate($("[name=start]").val()),
            endTime = getDate($("[name=end]").val()),
            shifts = [],
            categoryId = $("#ShiftCategory").val(),
            categoryName = $("#ShiftCategory").find(":selected").text();
        while ((endTime.getTime() - startTime.getTime()) >= 0) {
            var year = startTime.getFullYear();
            var month = startTime.getMonth().toString().length == 1 ? "0" + startTime.getMonth().toString() : startTime.getMonth();
            var day = startTime.getDate().toString().length == 1 ? "0" + startTime.getDate() : startTime.getDate();
            var date = year + "-" + month + "-" + day;
            if ($(".sshift-table").find("[data-shiftDate=" + date + "][data-shiftcategoryid=" + categoryId + "]").length == 0) {
                $(".sshift-table").append("<tr data-shiftdate='" + date + "' data-shiftcategoryid='" + categoryId + "'><td>" + date + "</td><td>" + categoryName + "</td><td><span class='layui-btn layui-btn-mini btn-remove'>删除</span></td></tr>");
            }
            startTime.setDate(startTime.getDate() + 1);
        }
    });
    $(".submit-btn").click(function () {
        if ($(".suser-table").find("tr").length == 0) {
            layer.msg("请添加用户！", { icon: 2 });
            return false;
        }
        if ($(".sshift-table").find("tr").length == 0) {
            layer.msg("请添加日期！", { icon: 2 });
            return false;
        }
        var data = {},
            UserInfoList = [],
            ShiftDateList = [];
        $(".sshift-table").find("tr").each(function (i, item) {
            item = $(item);
            ShiftDateList.push({
                UserId: "",
                ShiftDataTime: item.data("shiftdate"),
                ShiftCategoryID: item.data("shiftcategoryid")
            });
        });
        $(".suser-table").find("tr").each(function (i, item) {
            item = $(item);
            UserInfoList.push({ UserId: item.data("id") });
        });
        data.UserInfoList = UserInfoList;
        data.ShiftDateList = ShiftDateList;
        shiftUpdate("ShiftAdd", data, this, function () { $(".info-remove").addClass("hide"); $(".sshift-table,.suser-table").empty() });
    });
});

function getDate(datestr) {
    var temp = datestr.split("-");
    var date = new Date(temp[0], temp[1], temp[2]);
    return date;
}
var incr = 0;
var shiftUpdate = function (url, data, obj, closeFunc) {
    if (ajaxObj.setUrl("/Shift/" + url).setParaObj({ data: data }).setIsUpdateTrue().getData().ResultType == 0) {
        closeLayer(obj, closeFunc);
    }
}
// 获取排班数据
var getShiftData = function () {
    ajaxObj.setUrl("/Shift/ShiftGet").setParaObj({ data: { number: incr } }).setDotEle([{ container: ".shift-table", tmp: ".shift-tmp" }]).getData();
}
var getPageData = getShiftData;