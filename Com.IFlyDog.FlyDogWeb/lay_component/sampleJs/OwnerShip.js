$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getOwnerShip();
    });
    // 批量设置按钮
    $(".btn-add").click(function () {
        var _this = $(this),
        actionName = _this.data("text");
        openPop("Batch" + _this.data("action") + "UserAdd", ".ownerShip-pop", "批量设置" + actionName);
        $(".actionName").text(actionName);
    });
    // 批量设置弹窗提交
    $(".ownerShip.submit-btn").click(function () {
        var batchCustormAdd = [];
        $.each($("[name=BatchCustormAdd]").val().split("\n"), function (i, item) {
            item && batchCustormAdd.push({ CustormID: item });
        });
        params.setDataParams({
            UserID: $("[name=UserID]").val(),
            BatchCustormAdd: batchCustormAdd
        });
        if (ajaxObj.setUrl("/OwnerShip/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 编辑按钮
    $(".ownerShip-table").on("click",".btn-edit",function () {
        var _this = $(this);
        var data = {
            UserID: _this.data("id"),
            UserName: _this.data("name"),
            OwnerShipType: _this.data("text")
        }
        fillData(".ownerShip-form", ".ownerShip-form-tmp", data);
        openPop("Single" + _this.data("action") + "UserUpdateAdd", ".single-ownerShip-pop", "归属权调拨");
    });
    // 批量设置弹窗提交
    $(".singleOwnerShip.submit-btn").click(function () {
        params.setDataParams({
            OldUserID: $("[name=OldUserID]").val(),
            NewUserID: $("[name=NewUserID]").val()
        });
        if (ajaxObj.setUrl("/OwnerShip/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 用户弹窗点击按钮
    $("[name=UserName]").click(function () {
        UserInfo.setConfimFunc(function (userInfo) {
            $("[name=UserID]").val(userInfo.id);
            $("[name=UserName]").val(userInfo.name);
        }).openPop();
    });
    // 用户弹窗点击按钮
    $(".single-ownerShip-pop").on("click","[name=NewUserName]",function () {
        UserInfo.setConfimFunc(function (userInfo) {
            $("[name=NewUserID]").val(userInfo.id);
            $("[name=NewUserName]").val(userInfo.name);
        }).openPop();
    });
});
var getOwnerShip = function () {
    var dotEle = [{ container: ".ownerShip-table", tmp: ".ownerShip-tmp" }]
    ajaxObj.setUrl("/OwnerShip/OwnerShipGet").setDotEle(dotEle).getData();
}
getPageData = getOwnerShip;