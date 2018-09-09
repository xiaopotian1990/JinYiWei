$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        var data = { ID: "", Name: "", SortNo: "", Remark: "" };
        fillData(".itemGroup-form", ".itemGroup-form-tmp", data);
        openPop("ItemGroupAdd", ".itemGroup-info-pop", "添加项目组");
    });
    $(".itemGroup-table").on("click", ".btn-edit", function () {
        var dotEle = [{ container: ".itemGroup-form", tmp: ".itemGroup-form-tmp" }];
        params.setDataParam("id", $(this).data("id"));
        ajaxObj.setUrl("/ItemGroup/ItemGroupEditGet").setDotEle(dotEle).setParaObj(params).getData();
        openPop("ItemGroupSubmit", ".itemGroup-info-pop", "编辑项目组");
    });
    $(".itemGroup.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        params.setDataParams({
            ID:$("[name=ID]").val(),
            Name:$("[name=Name]").val(),
            SortNo:$("[name=SortNo]").val(),
            Remark:$("[name=Remark]").val()
        });
        if (ajaxObj.setUrl("/ItemGroup/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".itemGroup-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("您确定删除本条数据？", function () {
            params.setDataParams({
                ID: id
            });
            if (ajaxObj.setUrl("/ItemGroup/ItemGroupDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！", { icon: 1 });
        });
    });
    $(".itemGroup.search-btn").click(function () {
        getPageData();
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getItemGroupData = function () {
    var dotEle = [{ container: ".itemGroup-table", tmp: ".itemGroup-tmp" }];
    ajaxObj.setUrl("/ItemGroup/ItemGroupGet").setDotEle(dotEle).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=Name]").val())) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    } else if ($("[name=Name]").val().length > 20) {
        layer.msg("名称长度不能超过20！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=SortNo]").val())) {
        layer.msg("排序号不能为空！", { icon: 2 });
        return false;
    }else if (isNaN($("[name=SortNo]").val())) {
        layer.msg("排序号只能是数字！", { icon: 2 });
        return false;
    }
    return true;
}
getPageData = getItemGroupData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;