$(function () {
    layui.use(["form", "layer", "laydate", "laypage", "element"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.element = layui.element();
        window.form = layui.form();
        getPageData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var emptyData = { ID: "", Name: "", Remark: "" };
        fillData(".customerGroup-form", ".customerGroup-form-tmp", emptyData);
        openPop("CustomerGroupAdd", ".customerGroup-pop", "添加客户组");
    });
    // 编辑按钮
    $(".customerGroup-table").on("click", ".btn-edit", function () {
        params.setDataParam("ID", $(this).data("id"));
        var dotEle = [{ container: ".customerGroup-form", tmp: ".customerGroup-form-tmp" }];
        ajaxObj.setUrl("/CustomerGroup/GetByID").setParaObj(params).setDotEle(dotEle).getData();
        openPop("CustomerGroupUpdate", ".customerGroup-pop", "修改客户组");
        $(".customerGroup-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
            closeLayer(this);
        });
    });
    // 编辑按钮
    $(".customerGroup-table").on("click", ".btn-editGroup", function () {
        var id = $(this).data("id"),
            name = $(this).data("name");
        //window.open("/CustomerGroup/CustomerGroupData?customerGroup=" + id);
        //return false;
        parent.layui.tab({
            elem: ".admin-nav-card"
        }).tabAdd(
        {
            title: "客户组-" + name,
            href: "/CustomerGroup/CustomerGroupData?customerGroup=" + id, //地址
            icon: "fa-user"
        });
    });
    $(".customerGroup.submit-btn").click(function () {
        params.setDataParams({
            ID: $("[name=ID]").val(),
            Name: $("[name=Name]").val(),
            Remark: $("[name=Remark]").val()
        });
        if(ajaxObj.setUrl("/CustomerGroup/"+$(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0){
            closeLayer(this);
        }
    });
    // 查询按钮
    $(".search-btn").click(function () {
        getcustomerGroupData();
    });

});
var verify = function () { return true; }
//获取用户通知列表数据
var getcustomerGroupData = function () {
    params.setDataParam("Name", $("[name=sName]").val());
    var dotEle = [{ container: ".customerGroup-table", tmp: ".customerGroup-tmp" }];
    ajaxObj.setUrl("/CustomerGroup/CustomerGroupGet").setParaObj(params).setDotEle(dotEle).getData();
}
var getPageData = getcustomerGroupData;
