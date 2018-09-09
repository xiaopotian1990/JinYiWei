(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".CallBackRemind.add-submit", function () {
            if (!verify()) {
                return false;
            };
            params.setDataParams({
                ID: pageContent.find("[name=ID]").val(),
                CustomerID: custid,
                CategoryID: pageContent.find("[name=CategoryID]").val(),
                TaskTime: pageContent.find("[name=TaskTime]").val(),
                Name: pageContent.find("[name=Name]").val(),
                UserID: pageContent.find("[name=UserID]").val()
            });
            getPageData = getCallBack;
            if (ajaxObj.setUrl("/CustomerProfile/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        pageContent.on("click", "[name=UserName]", function () {
            UserInfo.setConfimFunc(function (userInfo) {
                pageContent.find("[name=UserID]").val(userInfo.id);
                pageContent.find("[name=UserName]").val(userInfo.name);
            }).openPop();
        });
        window.CallBackRemind.getDetail = function (id) {
            params.setDataParams({
                ID: id,
                customerID: custid
            });
            var result = ajaxObj.setUrl("/CustomerProfile/GetCallRemindDetail").setParaObj(params).getData();
            this.setEntry(result.Data);
            return this;
        }
        var verify = function () {
            return true;
        };
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        // 填充页面模版到页面容器中
        fillData(pageContent.find(".callBackRemind-form"), pageContent.find(".callBackRemind-tmp"), data);

        pageContent.find("[name=CategoryID]").find("[value=" + data.CategoryID + "]").prop("selected", true)
        form.render();
    }
    getPageData = function () { };
    var MakeBeginTime = null, MakeEndTime = null,
    // 填充页面模版到页面容器中
    emptyFormData = { ID: "", CategoryID: "",TaskTime:"", Name: "", UserID: "", UserName: "" };
    Model.init("CallBackRemind", "/Customer/CallBackRemind", "添加回访提醒", "CallbackRemindAdd", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });
})(window, jQuery);