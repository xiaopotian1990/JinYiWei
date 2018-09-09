$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getPageData();
        form.on("select(level)", function (data) {
            $(".level").hide();
            for (var i = 1; i <= data.value; i++) {
                $("[data-level=" + i + "]").show();
            }
        });
    });
    $(".btn-add").hover(function () {
        $(this).siblings(".types").show();
    }, function () {
        $(this).siblings(".types").hide();
    });
    $(".types").hover(function () {
        $(this).show();
    }, function () {
        $(this).hide();
    });
    $(".types").on("click", "li", function () {
        var data = {Type:$(this).data("type"), ID: "", Discount: "", Amount: "", Level: "", Remark: "" };
        fillData(".auditRule-form", ".auditRule-form-tmp", data);
        openPop("AuditRuleAdd", ".auditRule-info-pop", "添加审核规则");
    })
    $(".auditRule-table").on("click", ".btn-edit", function () {
        var dotEle = [{ container: ".auditRule-form", tmp: ".auditRule-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        var result = ajaxObj.setUrl("/AuditRule/AuditRuleEditGet").setDotEle(dotEle).setParaObj(params).getData();
        $.each(result.Data.AuditUserInfoAdd, function (i, item) {
            var container = $(".level[data-level=" + item.Level + "]").show().find(".user-table");
            fillData(container, ".user-tmp", [item], true);
        });
        $(".level").each(function (i,item) {
            item = $(item);
            item.find(".user-table").find("tr").length > 0 && item.show();
        });
        $("[name=Level]").find("[value=" + result.Data.Level + "]").prop("selected", true);
        openPop("AuditRuleSubmit", ".auditRule-info-pop", "编辑审核规则");
    });
    $(".auditRule.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        var auditRuleDetailAdd = [],
            url = $(this).parents(".layui-layer").data("url");
        $(".level:visible").find("[data-userid]").each(function (i, item) {
            item = $(item);
             auditRuleDetailAdd.push({ UserID: item.data("userid"), Level: item.parents(".level").data("level") });
        });
        params.setDataParams({
            HospitalID:getQueryString("hospitalID"),
            ID: $("[name=ID]").val(),
            Type: $("[name=Type]").val(),
            Amount: $("[name=Amount]").val(),
            Discount: $("[name=Discount]").val(),
            Level: $("[name=Level]").val(),
            Remark: $("[name=Remark]").val(),
            Status: url == "AuditRuleSubmit" ? "" : 1,
            AuditRuleDetailAdd: auditRuleDetailAdd
        });
        if (ajaxObj.setUrl("/AuditRule/" + url).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this, function () { $(".user-table").empty(); $(".level:not([data-level=1])").hide(); });
        }
    });
    $(".auditRule-table").on("click", ".btn-remove", function () {
        params.setDataParams({
            ID: $(this).data("id")
        });
        if (ajaxObj.setUrl("/AuditRule/auditRuleDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".auditRule.search-btn").click(function () {
        getPageData();
    });
    $(".level-tables").on("click", ".add-user", function () {
        var level = $(this).parent(".level").data("level");
        UserInfo.setConfimFunc(function (userInfo) {
            userInfo = {Account:userInfo.account,Name:userInfo.name,ID:userInfo.id};
            fillData("[data-level=" + level + "] .user-table", ".user-tmp",[userInfo],true);
        }).openPop();
    });
    $(".level-tables").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    $(".auditRule-table").on("click", ".btn-status", function () {
        var _this = $(this),
            id = _this.data("id"),
            status = _this.data("status") == 1 ? 0 : 1;
        layer.confirm("是否" + (status == 0 ? "使用" : "停用") + "?", function () {
            params.setDataParams({ "ID": id, Status: status });
            ajaxObj.setUrl("/AuditRule/AuditRuleStopOrUse").setParaObj(params).setIsUpdateTrue().setDataCallBack(function (data) {
                if (data.ResultType == 0) {
                    _this.data("status", status).text(status == 0 ? "使用" : "停用");
                }
            }).getData();
        }, function () {
            layer.msg("您已取消操作！");
        });
    });
    $(document).on("click", ".layui-layer-close", function () { $(".user-table").empty(); $(".level:not([data-level=1])").hide(); });
    $(".close-layer").click(function () {
        $(".user-table").empty(); $(".level:not([data-level=1])").hide();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getAuditRuleData = function () {
    params.setDataParam("hospitalID", getQueryString("hospitalID"));
    fillData(".auditRule-table", ".auditRule-tmp", []);
    ajaxObj.setUrl("/AuditRule/AuditRuleGet").setParaObj(params).setDataCallBack(function (data) {
        $.each(data.Data,function (i, item) {
            fillData($(".auditRule-table").filter("[type=" + item.Type + "]"),".auditRule-tmp",[item],true);
        });
    }).getData();
}
var verify = function () {
    return true;
}
getPageData = getAuditRuleData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;