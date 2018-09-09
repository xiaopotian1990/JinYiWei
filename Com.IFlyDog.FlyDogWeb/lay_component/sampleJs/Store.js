$(function () {
    layui.use(["form", "layer","laypage","element"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.element = layui.element();
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        fillData(".store-form", ".store-form-tmp", "");
        openPop("StoreAdd", ".store-info-pop", "添加店家");
    });
    $(".store-table").on("click",".btn-edit",function () {
        var dotEle = [{ container: ".store-form", tmp: ".store-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        ajaxObj.setUrl("/Store/StoreGetByID").setDotEle(dotEle).setParaObj(params).getData();
        openPop("StoreEdit", ".store-info-pop", "编辑店家");
    });
    $(".store.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ID: SUtils.setEmpty($("[name=ID]").val()),
            Name: SUtils.setEmpty($("[name=Name]").val()),
            Linkman: SUtils.setEmpty($("[name=Linkman]").val()),
            Mobile: SUtils.setEmpty($("[name=Mobile]").val()),
            Address: SUtils.setEmpty($("[name=Address]").val()),
            OwnerName: SUtils.setEmpty($("[name=OwnerName]").val()),
            Bank: SUtils.setEmpty($("[name=Bank]").val()),
            CardNo: SUtils.setEmpty($("[name=CardNo]").val()),
            Remark: SUtils.setEmpty($("[name=Remark]").val())
        });
        if (ajaxObj.setUrl("/Store/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".store-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("您确认删除本条数据？", function () {
            params.setDataParams({
                ID: id
            });
            if (ajaxObj.setUrl("/Store/StoreDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    $(".store.search-btn").click(function () {
        getPageData();
    });
    $(".store-table").on("click", ".store-a", function () {
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
var getStoreData = function () {
    params.setDataParams({
        PageNum: pageNum,
        PageSize: pageSize,
        Name: SUtils.setEmpty($("[name=sName]").val()),
        Linkman: SUtils.setEmpty($("[name=sLinkman]").val()),
        Mobile: SUtils.setEmpty($("[name=sMobile]").val()),
        OwnerName: SUtils.setEmpty($("[name=sOwnerName]").val())
    });
    var dotEle = [{ container: ".store-table", tmp: ".store-tmp" }];
    ajaxObj.setUrl("/Store/StoreGet").usePage().setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=Name]").val())) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    } else if ($("[name=Name]").val().length > 20) {
        layer.msg("名称长度不能超过20！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=Linkman]").val())) {
        layer.msg("联系人不能为空！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=Mobile]").val())) {
        layer.msg("手机号不能为空！",{icon:2});
        return false;
    }
    return true;
}
getPageData = getStoreData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;