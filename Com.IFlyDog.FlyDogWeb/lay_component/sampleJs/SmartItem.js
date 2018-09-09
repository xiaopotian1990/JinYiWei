$(function () {
    layui.use(["form", "layer", "laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();     
    });
    $(".btn-add").click(function () {
        var data = { ID: "", Name: "", GroupID: "", SortNo: "", Remark: "", SymptomDetailInfoAdd: [], ChargeDetailInfoAdd: [] }
        fillData(".charge-table", ".charge-tmp", data);
        fillData(".symptom-table", ".symptom-tmp", data);
        fillData(".smartItem-form", ".smartItem-form-tmp", data);
        openPop("SmartItemAdd", ".smartItem-pop", "添加报表项目");
    });
    // 列表按钮
    $(".smartItem-table").on("click", ".btn-edit", function () {
        params.setDataParam("id", $(this).data("id"));
        $("[name=StoreUserID]").val(params.data.userID);
        var dotEle = [{ container: ".smartItem-form", tmp: ".smartItem-form-tmp" }, { container: ".charge-table", tmp: ".charge-tmp" }, { container: ".symptom-table", tmp: ".symptom-tmp" }];
        var result = ajaxObj.setUrl("/SmartItem/smartItemEditGet").setParaObj(params).setDotEle(dotEle).getData();
        $("#GroupID").find("[value=" + result.Data.GroupID + "]").prop("selected",true);
        openPop("SmartItemSubmit", ".smartItem-pop", "编辑报表项目");
    });
    // 添加弹窗提交按钮
    $(".smartItem.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        var symptomDetailInfoAdd = [];
        $(".symptom-table").find("tr").each(function (i, item) {
            symptomDetailInfoAdd.push({ ID: $(item).attr("symptomid") });
        });
        var chargeDetailInfoAdd = [];
        $(".charge-table").find("tr").each(function (i, item) {
            chargeDetailInfoAdd.push({ ID: $(item).attr("chargeid") });
        });
        params.setDataParams({
            ID: $("[name=ID]").val(),
            Name: $("[name=Name]").val(),
            GroupID: $("#GroupID").val(),
            SortNo: $("[name=SortNo]").val(),
            SymptomDetailInfoAdd: symptomDetailInfoAdd,
            ChargeDetailInfoAdd: chargeDetailInfoAdd,
            Remark: $("[name=Remark]").val()
        });
        if (ajaxObj.setUrl("/SmartItem/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 项目表格删除按钮
    $(".smartItem-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("您确定删除本条数据？", function () {
            params.setDataParams({
                id: id
            });
            if (ajaxObj.setUrl("/SmartItem/SmartItemDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    // 添加弹窗表格删除按钮
    $(".symptom-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    // 添加弹窗表格删除按钮
    $(".charge-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    // 打开选择
    $(".add-symptom").click(function () {
        getSymptomData();
        openPop("", ".symptom-pop", "选择渠道");
    })
    // 打开选择
    $(".add-charge").click(function () {
        getChargeData();
        openPop("", ".charge-pop", "选择项目");
    })
    // 选择弹窗提交按钮
    $(".symptom.btn-submit").click(function () {
        $(".symptom-pop-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".symptom-table").find("[channelid=" + item.val() + "]").length == 0) {
                fillData(".symptom-table", ".symptom-tmp", { SymptomDetailInfoAdd: [{ ID: item.val(), Name: item.data("name") }] }, true);
            }
        });
        closeLayer(this);
    });
    // 选择弹窗提交按钮
    $(".charge.btn-submit").click(function () {
        $(".charge-pop-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".charge-table").find("[channelid=" + item.val() + "]").length == 0) {
                fillData(".charge-table", ".charge-tmp", { ChargeDetailInfoAdd: [{ ID: item.val(), Name: item.data("name") }] }, true);
            }
        });
        closeLayer(this);
    });
    // 检测按钮
    $(".btn-check-symptom").click(function () {
        var dotEle = [{ container: ".symptom-check-table", tmp: ".symptom-check-tmp" }];
        ajaxObj.setUrl("/SmartItem/SmartChickSymptomPost").setDotEle(dotEle).getData();
        openPop("", ".symptom-check-pop", "报表症状检测");
    });
    // 检测按钮
    $(".btn-check-charge").click(function () {
        params.setDataParams({
            PageNum: 1,  //分页数据，目前先写死
            PageSize: 6
        });
        var dotEle = [{ container: ".charge-check-table", tmp: ".charge-check-tmp" }];
        ajaxObj.setUrl("/SmartItem/SmartChickChargePost").setDotEle(dotEle).setParaObj(params).usePage().getData();
        openPop("", ".charge-check-pop", "报表项目检测");
    });
    $(".charge.search-btn").click(function () {
        getChargeData();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getSmartItemData = function () {
    var dotEle = [{ container: ".smartItem-table", tmp: ".smartItem-tmp" }];
    ajaxObj.setUrl("/SmartItem/SmartItemPost").setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=Name]").val())) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=SortNo]").val())) {
        layer.msg("序号不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getSymptomData = function () {
    ajaxObj.setUrl("/SymptomSetting/SymptomGetByOk").setDataCallBack(function (data) {

        fillData(".symptom-pop-table", ".symptom-pop-tmp", []);
        $.each(data.Data, function (i, item) {
            $(".symptom-table").find("[symptomid=" + item.ID + "]").length == 0 && fillData(".symptom-pop-table", ".symptom-pop-tmp", [item], true);
        });
    }).getData();
}
var getChargeData = function () {
    var dotEle = [{ container: ".charge-check-table", tmp: ".charge-check-tmp" }];
    params.setDataParams({Type:1, PinYin: $("[name=smartProductPinYin]").val(), Name: $("[name=smartProductNmae]").val() });
    ajaxObj.setUrl("/Charge/ChargeGetData").setDotEle(dotEle).setParaObj(params).setDataCallBack(function (data) {
        var tmpHtml = $(doT.template($(".charge-pop-tmp").text())(data.Data));
        $(".charge-table").find("tr").each(function (i, item) {
            item = $(item);
            tmpHtml = tmpHtml.not("[chargeId= " + item.attr("chargeId") + "]");
        });
        $(".charge-pop-table").html(tmpHtml);
    }).getData();
}
getPageData = getSmartItemData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;


