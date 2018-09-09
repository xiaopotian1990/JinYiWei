$(function () {
    layui.use(["form", "layer", "laydate", "laypage"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
        form.on("radio(CustomerType)", function (data) {
            $("[name=MemberCategoryID],[name=CustomerScope],[name=ShareCategoryID]").prop("disabled", true);
            $(data.elem).siblings("[name=MemberCategoryID],[name=CustomerScope],[name=ShareCategoryID]").prop("disabled", false);
            form.render();
        });
        form.on("checkbox(AssignUserInfo)", function (data) {
            var check = $(data.elem).siblings("[type=text]");
            check.prop("disabled", !data.elem.checked);
            form.render();
        });
        form.on("checkbox(AssignDeptInfo)", function (data) {
            var check = $(data.elem).siblings("[type=text]");
            check.prop("disabled", !data.elem.checked);
            form.render();
        });
        form.on("checkbox(checkAll)", function (data) {
            var _this = $(data.elem),
                checkboxs = _this.parent().siblings(".list").find("[type=checkbox]");
            checkboxs.prop("checked", data.elem.checked);
            form.render();
            event.stopPropagation();
            return false;
        });
        form.on("checkbox(singleCheck)", function (data) {
            var preList = $(data.elem).parents(".list");
            if (preList.find("[type=checkbox]").length == preList.find(":checked").length) {
                preList.siblings("p").find("[type=checkbox]").prop("checked", true);
            } else {
                preList.siblings("p").find("[type=checkbox]").prop("checked", false);
            }
            form.render();
            event.stopPropagation();
            return false;
        });
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var emptyData = { ID: "", Name: "", Type: "", CustomerType: 0, MemberCategoryID: "", CustomerGroupID: "", CustomerScope: "", ShareCategoryID: "", Info: "", ExploitUserStatus: "", ManagerUserStatus: "", AssignUserInfoAdd: [], AssignDeptInfoAdd: [], Remark: "" };
        fillData(".userTrigger-form", ".userTrigger-form-tmp", emptyData);
        emptyData.CustomerType != 1 && $("[name=MemberCategoryID]").prop("disabled", true);
        emptyData.CustomerType != 3 && $("[name=ShareCategoryID]").prop("disabled", true);
        getUser_DetpList(emptyData);
        openPop("UserTriggerAdd", ".userTrigger-pop", "添加用户通知方案");
    });
    // 编辑按钮
    $(".userTrigger-table").on("click", ".btn-del", function () {
        var id = $(this).data("id");
        layer.confirm("您确定删除本条数据？", function () {
            params.setDataParam("ID", id);
            if (ajaxObj.setUrl("/UserTrigger/UserTriggerDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消删除！", { icon: 1 });
        });
    });
    // 编辑按钮
    $(".userTrigger-table").on("click", ".btn-edit", function () {
        params.setDataParam("ID", $(this).data("id"));
        var dotEle = [{ container: ".userTrigger-form", tmp: ".userTrigger-form-tmp" }];
        var result = ajaxObj.setUrl("/UserTrigger/UserTriggerGet").setParaObj(params).setDotEle(dotEle).getData();
        $("[name=MemberCategoryID]").find("[value=" + result.Data.MemberCategoryID + "]").prop("selected", true);
        $("[name=ShareCategoryID]").find("[value=" + result.Data.ShareCategoryID + "]").prop("selected", true);
        result.Data.CustomerType != 1 && $("[name=MemberCategoryID]").prop("disabled", true);
        result.Data.CustomerType != 3 && $("[name=ShareCategoryID]").prop("disabled", true);
        getUser_DetpList(result.Data);
        openPop("UserTriggerSubmit", ".userTrigger-pop", "修改用户通知方案");
        $(".UserTrigger-pop").parents(".layui-layer").find(".layui-layer-close").click(function () {
            closeLayer(this);
        });
    });
    // 详细弹窗弹窗提交
    $(".userTrigger.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var users = [], depts = [];
        $(".AssignUserInfo .list").find(":checked").each(function (i,item) {
            item = $(item);
            users.push({ ID: item.val(), Name: item.attr("title") });
        });
        $(".AssignDeptInfo .list").find(":checked").each(function (i, item) {
            item = $(item);
            depts.push({ ID: item.val(), Name: item.attr("title") });
        });
        params.setDataParams({
            ID: $("[name=ID]").val(),
            Name: $("[name=Name]").val(),
            Type: $("[name=Type]").val(),
            CustomerType: $("[name=CustomerType]:checked").val(),
            CustomerGroupID: $("[name=CustomerGroupID]").val(),
            MemberCategoryID: $("[name=MemberCategoryID]").find(":selected").val(),
            ShareCategoryID: $("[name=ShareCategoryID]").find(":selected").val(),
            Info: $("[name=Info]").val(),
            AllUsers: $("[name=AllUsers]").is(":checked") ? 1 : 0 ,
            ExploitUserStatus: $("[name=ExploitUserStatus]").is(":checked") ? 1 : 0,
            ManagerUserStatus: $("[name=ManagerUserStatus]").is(":checked") ? 1 : 0,
            AssignUserInfoAdd: users,
            AssignDeptInfoAdd: depts,
            Remark: $("[name=Remark]").val()
        });
        if (ajaxObj.setUrl("/UserTrigger/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }

    });
    // 客户组文本框
    $(".userTrigger-form").on("click", "[name=CustomerScope]:not(disabled)", function () {
        var dotEle = [{ container: ".customerScope-table", tmp: ".customerScope-tmp" }];
        ajaxObj.setUrl("/CustomerGroup/CustomerGroupGet").setDotEle(dotEle).getData();
        openPop("", ".customerScope-pop","选择客户组");
    });
    // 客户组弹窗复选框点击
    $(".customerScope-table").on("click", "[type=checkbox]", function () {
        var _this = $(this);
        _this.parents("table").find(":checked").not(_this).prop("checked", false);
    });
    // 客户组弹窗确认按钮
    $(".customerScope.submit-btn").click(function () {
        var _this = $(this),
            checkEle = _this.parents(".customerScope-pop").find(":checked");
        $("[name=CustomerScope]").val(checkEle.attr("title"));
        $("[name=CustomerGroupID]").val(checkEle.val());
        closeLayer(this);
    });
    $(".userTrigger-form").on("click", "[name=AssignUserInfo]:not(disabled),[name=AssignDeptInfo]:not(disabled)", function () {
        $(this).siblings(".require").toggle();
    });
    $(document).on("click", ":not([type=checkbox])", function (event) {
        var _this = $(this);
        (_this.is("[name=AssignUserInfo]") || _this.is("[name=AssignDeptInfo]") || _this.add(_this.parents("div")).hasClass("AssignUserInfo") || _this.add(_this.parents("div")).hasClass("AssignDeptInfo")) || $(".require").hide();
        event.stopPropagation();
    });
    $(".userTrigger-form").on("click", ".user.btn-sub,.dept.btn-sub", function () {
        var preList = $(this).parent().prev(".list");
        var checkeds = preList.find(":checked");
        var title = checkeds.length > 1 ? checkeds.eq(0).attr("title") + "..." : checkeds.eq(0).attr("title");
        preList.parent().hide().prev().val(title);
    })

});
var verify = function () { return true;}
var getUser_DetpList = function (data) {
    var dotEle = [{ container: ".AssignUserInfo .list", tmp: ".userCheck-tmp" }];
    ajaxObj.setUrl("/SmartUser/SmartUserGet").setDotEle(dotEle).getData();
    var dotEle = [{ container: ".AssignDeptInfo .list", tmp: ".deptCheck-tmp" }];
    ajaxObj.setUrl("/Dept/DeptGet").setDotEle(dotEle).getData();
    $.each(data.AssignUserInfoAdd, function (i, item) {
        $(".AssignUserInfo .list").find("[value=" + item.ID + "]").prop("checked", true);
    });
    $.each(data.AssignDeptInfoAdd, function (i, item) {
        $(".AssignDeptInfo .list").find("[value=" + item.ID + "]").prop("checked", true);
    });
    var checks = $(".AssignUserInfo .list").find(":checked");
    $(".AssignUserInfo .list").parent().prev().val(checks.length > 1 ? checks.eq(0).attr("title") + "..." : checks.eq(0).attr("title"));
    checks = $(".AssignDeptInfo .list").find(":checked");
    $(".AssignDeptInfo .list").parent().prev().val(checks.length > 1 ? checks.eq(0).attr("title") + "..." : checks.eq(0).attr("title"));
    $(".AssignUserInfo .list").find("[type=checkbox]").length == $(".AssignUserInfo .list").find(":checked").length && $(".AssignUserInfo .list").prev().find("[type=checkbox]").prop("checked", true);
    $(".AssignDeptInfo .list").find("[type=checkbox]").length == $(".AssignDeptInfo .list").find(":checked").length && $(".AssignDeptInfo .list").prev().find("[type=checkbox]").prop("checked", true);
    form.render();
}
//获取用户通知列表数据
var getUserTriggerData = function () {
    var dotEle = [{ container: ".userTrigger-table", tmp: ".userTrigger-tmp" }];
    ajaxObj.setUrl("/UserTrigger/UserTriggerGetData").setDotEle(dotEle).getData();
}
var getPageData = getUserTriggerData;
