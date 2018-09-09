$(function () {
    layui.use(["form", "layer", "element"], function () {
        var layer = layui.layer;
        window.element = layui.element;
        window.form = layui.form();
        getPageData();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getStoreInfoData = function () {
    var data = params.setDataParams({ id: getQueryString("storeID") });
    // 基础信息
    var dotEle = [{ container: ".basic", tmp: ".basic-tmp" }];
    ajaxObj.setUrl("/Store/GetByIDStoreBasicData").setDotEle(dotEle).setParaObj(data).getData();
    // 客户信息
    var dotEle = [{ container: ".manager-table", tmp: ".manager-tmp" }];
    var manager = ajaxObj.setUrl("/Store/GetByIDStoreManagerData").setDotEle(dotEle).setParaObj(data).getData();
    $(".manager-count").html(manager.length);
    // 佣金信息
    var dotEle = [{ container: ".commission-table", tmp: ".commission-tmp" }];
    var manager = ajaxObj.setUrl("/Store/GetByIDStoreCommissionData").setDotEle(dotEle).setParaObj(data).getData();
    // 回款信息
    var dotEle = [{ container: ".saleback-table", tmp: ".saleback-tmp" }];
    var manager = ajaxObj.setUrl("/Store/GetByIDStoreSaleBackData").setDotEle(dotEle).setParaObj(data).getData();
}
getPageData = getStoreInfoData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;