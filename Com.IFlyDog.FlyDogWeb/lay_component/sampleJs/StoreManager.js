$(function () {
    layui.use(["form", "layer","laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        fillData(".store-table", ".store-tmp", "");
        openPop("StoreManagerAdd", ".storeManager-pop", "添加负责人");
    });
    // 添加弹窗提交按钮
    $(".storeManager.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        var stores = [];
        $(".store-table").find("tr").each(function (i, item) {
            stores.push({ StoreID: $(item).attr("storeid") });
        });
        params.setDataParams({
            UserID: SUtils.setEmpty($("[name=UserID]").val()),
            StoreManagerInfoData: stores
        });
        if (ajaxObj.setUrl("/StoreManager/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 负责人表格删除按钮
    $(".storeManager-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("您确认删除本条数据？", function () {
            params.setDataParams({
                UserID: id
            });
            if (ajaxObj.setUrl("/StoreManager/StoreManagerUserDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    // 添加弹窗表格删除按钮
    $(".store-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    // 选择用户
    $(".storeManager-pop").on("click", "[name=UserName]", function () {
        UserInfo.setConfimFunc(function (userInfo) {
            $("[name=UserID]").val(userInfo.id);
            $("[name=UserName]").val(userInfo.name);
        }).openPop();
    });
    // 打开选择商铺弹窗
    $(".add-stroe").click(function () {
        getStoreData();
        openPop("", ".store-pop", "选择店家");
    })
    // 选择商铺弹窗提交按钮
    $(".store.btn-submit").click(function () {
        var stores = [];
        $(".store-pop-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".store-table").find("[storeid=" + item.data("id") + "]").length == 0) {
                fillData(".store-table", ".store-tmp", [{ ID: item.data("id"), Name: item.data("name") }], true);
            }
        });
        closeLayer(this);
    });
    // 选择商铺弹窗搜索
    $(".store.btn-search").click(function () {
        getStoreData();
    });
    // 店家列表按钮
    $(".storeManager-table").on("click", ".btn-list", function () {
        params.setDataParam("userID", $(this).data("id"));
        $("[name=StoreUserID]").val(params.data.userID);
        var dotEle = [{ container: ".user-store-table", tmp: ".user-store-tmp" }];
        ajaxObj.setUrl("/StoreManager/GetUserIDData").setParaObj(params).setDotEle(dotEle).getData();
        openPop("", ".store-info-pop", "店家列表");
    });
    // 列表删除
    $(".user-store-table").on("click", ".userStore.btn-remove", function () {
        var _this = $(this),
            userId =  $("[name=StoreUserID]").val(),
            storeId = $(this).data("id");
        layer.confirm("您确认删除本条数据？", function () {
            params.setDataParams({ UserID: userId, StoreID: storeId });
            if (ajaxObj.setUrl("/StoreManager/StoreManagerDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                _this.parents("tr").remove();
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    })
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getStoreManagerData = function () {
    var dotEle = [{ container: ".storeManager-table", tmp: ".storeManager-tmp" }];
    ajaxObj.setUrl("/StoreManager/GetByHospitalIDData").setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=UserID]").val())) {
        layer.msg("请选择用户！", { icon: 2 });
        return false;
    } else if ($(".store-table").find("tr").length == 0) {
        layer.msg("请添加商铺！", { icon: 2 });
        return false;
    }
    return true;
}
var getStoreData = function () {
    params.setDataParam("Name", $("[name=sName]").val());
    ajaxObj.setUrl("/Store/StoreGetNoPage").setParaObj(params).setDataCallBack(function (data) {
        fillData(".store-pop-table", ".store-pop-tmp", []);
        $.each(data.Data, function (i, item) {
            $(".store-table").find("[storeid=" + item.ID + "]").length == 0 && fillData(".store-pop-table", ".store-pop-tmp", [item], true);
            
        });
    }).getData();
}
getPageData = getStoreManagerData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;