$(function () {
    layui.use(["form", "layer", "laypage","laydate","element"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.laydate = layui.laydate;
        window.element = layui.element();
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        var data = {ID:"", StoreID: "", StoreName: "", CreateDate: "", Amount: "", Remark :""}
        fillData(".sale-form", ".sale-form-tmp", data);
        openPop("SaleBackAdd", ".sale-info-pop", "添加回款");
    });
    $(".sale-table").on("click", ".btn-edit", function () {
        var dotEle = [{ container: ".sale-form", tmp: ".sale-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        ajaxObj.setUrl("/SaleBack/SaleBackGetByID").setDotEle(dotEle).setParaObj(params).getData();
        $("[name=StoreName]").val($(this).parents("tr").find(".store-a").text()).prop("disabled",true);
        openPop("SaleBackEdit", ".sale-info-pop", "编辑回款");
    });
    $(".sale.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ID: $("[name=ID]").val(),
            StoreID: $("[name=StoreID]").val(),
            CreateDate: $("[name=CreateDate]").val(),
            Amount: $("[name=Amount]").val(),
            Remark: $("[name=Remark]").val(),
        });
        if (ajaxObj.setUrl("/SaleBack/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 
    $(".sale-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        var storeid = $(this).data("storeid");
        layer.confirm("您确定删除本条数据？", function () {
            params.setDataParams({
                ID: id,
                StoreID:storeid
            });
            if (ajaxObj.setUrl("/SaleBack/SaleBackDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    // 店铺文本框
    $(".sale-info-pop").on("click", "[name=StoreName]:not(:disabled)", function () {
        getStoreData();
        openPop("", ".store-pop", "选择店铺");
    });
    $(".store-pop-table").on("click", "[type=checkbox]", function () {
        $(this).parents(".store-pop-table").find("[type=checkbox]").not(this).prop("checked", false);
        form.render();
    });
    // 选择商铺弹窗搜索
    $(".store.btn-search").click(function () {
        getStoreData();
    });
    // 回款搜索
    $(".saleBack.search-btn").click(function () {
        getSaleData();
    });
    // 选择商铺弹窗提交按钮
    $(".store.btn-submit").click(function () {
        var stores = [];
        $(".store-pop-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("[name=StoreID]").val(item.data("id"));
            $("[name=StoreName]").val(item.data("name"));
        });
        closeLayer(this);
    });
    $(".sale-table").on("click", ".store-a", function () {
        var _this = $(this),
            name = _this.data("name"),
            url = _this.data("url");
        parent.layui.tab({
            elem: '.admin-nav-card'
        }).tabAdd({
            title: name + '档案',
            href: url,
            icon: "fa-user"
        });
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getSaleData = function () {
    params.setDataParams({
        PageNum: pageNum,
        PageSize: pageSize,
        BeginTime: $("[name=sBeginTime]").val(),
        EndTime: $("[name=sEndTime]").val(),
        StoreName: $("[name=sStoreName]").val()
    });
    var dotEle = [{ container: ".sale-table", tmp: ".sale-tmp" }];
    ajaxObj.setUrl("/SaleBack/SaleBackCGet").usePage().setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=StoreID]").val())) {
        layer.msg("请选择商铺！", { icon: 2 });
        return false;
    }if (SUtils.isNullOrEmpty($("[name=CreateDate]").val())) {
        layer.msg("请选择回款日期！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=Amount]").val())) {
        layer.msg("回款金额！", { icon: 2 });
        return false;
    } else if (isNaN($("[name=Amount]").val())) {
        layer.msg("回款金额只能为数值！", { icon: 2 });
        return false;
    }
    return true;
}
var getStoreData = function () {
    params.setDataParams({Type:1,"Name": $("[name=sName]").val()});
    ajaxObj.setUrl("/Store/StoreGetNoPage").setParaObj(params).setDataCallBack(function (data) {
        $.each(data.Data, function (i, item) {
            $(".store-table").find("[storeid=" + item.ID + "]").length == 0 || (data.Data = Common.Utils.Array.remove(data.Data, item));
        });
        fillData(".store-pop-table", ".store-pop-tmp", data.Data);
    }).getData();
}
getPageData = getSaleData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;