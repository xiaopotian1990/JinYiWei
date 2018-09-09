(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.find(".OwnerShipOrder.add-submit").click(function () {
            if (!verify()) {
                return false;
            };
            var oldUserID = pageContent.find("[name=OldUserID]").val(),
                newUserID = pageContent.find("[name=NewUserID]").val();
            oldUserID = oldUserID?oldUserID:newUserID;
            params.setDataParams({
                ID: pageContent.find("[name=ID]").val(),
                CustomerID: custid,
                OldUserID: oldUserID,
                NewUserID: newUserID,
                Content: pageContent.find("[name=Content]").val()
            });
            getPageData = getOwinerShip;
            if (ajaxObj.setUrl("/OwnerShipOrder/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        pageContent.find("[name=NewUserName]").click(function () {
            UserInfo.setConfimFunc(function (userInfo) {
                $("[name=NewUserID]").val(userInfo.id);
                $("[name=NewUserName]").val(userInfo.name);
            }).openPop();
        });
        var verify = function () {
            return true;
        };
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        var action = window.OwnerShipOrder.getSubmitUrl(),
            flag = action == "CustomerConsultanAdd",
            type = flag ? 2 : 1,
            ownerType = flag ? "咨询人员" : "开发人员";
        $(".ownerType").text(ownerType);
        if (flag) {
            UserInfo.notUseHospital();
        } else {
            UserInfo.useHospital();
        }
        params.setDataParams({ Type: type, CustomerID :custid});
        var result = ajaxObj.setUrl("/OwnerShipOrder/GetCustomerUserInfoGet").setParaObj(params).getData();
        result = result.Data;
        if (result) {
            result.OldUserInfo = "【" + result.DeptName + "】【" + result.UserName + "】";
            result.OldUserID = result.UserId;
        } else {
            result = {};
            result.OldUserInfo = "无";
            result.OldUserID = "";
            result.ID = "";
        }
        pageContent.find(".OldUserInfo").text(result.OldUserInfo);
        pageContent.find("[name=OldUserID]").val(result.OldUserID);
        pageContent.find("[name=ID]").val(result.ID);
    }
    getPageData = function () { };
    // 填充页面模版到页面容器中
    emptyFormData = {ID:"", CategoryID: "", Tool: "", Content: "" };
    Model.init("OwnerShipOrder", "/OwnerShipOrder/OwnerShipOrderInfo", "添加变更人员信息", "", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });
})(window, jQuery);