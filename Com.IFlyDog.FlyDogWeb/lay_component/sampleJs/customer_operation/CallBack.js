(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".CallBack.add-submit", function () {
            if (!verify()) {
                return false;
            };
            params.setDataParams({
                ID: pageContent.find("[name=ID]").val(),
                CustomerID: custid,
                CategoryID: pageContent.find("[name=CategoryID]").val(),
                Tool: pageContent.find("[name=Tool]").val(),
                Content: pageContent.find("[name=Content]").val()
            });
            getPageData = getCallBack;
            if (ajaxObj.setUrl("/CustomerProfile/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        window.CallBack.getDetail = function (id) {
            params.setDataParams({
                ID: id,
                customerID:custid
            });
            var result = ajaxObj.setUrl("/CustomerProfile/GetCallbackDetail").setParaObj(params).getData();
            this.setEntry(result.Data);
            getPageData = getCallBack;
            return this;
        }
        var verify = function () {
            if (pageContent.find("[name=CategoryID]").val() == -1) {
                layer.msg("请选择回访类型！", {icon:2});
                return false;
            }
            if (pageContent.find("[name=Tool]").val() == -1) {
                layer.msg("请选择回访方式！", { icon: 2 });
                return false;
            }
            return true;
        };
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        // 填充页面模版到页面容器中
        fillData(pageContent.find(".callBack-form"), pageContent.find(".callBack-form-tmp"), data);
        pageContent.find("[name=Tool]").find("[value=" + data.Tool + "]").prop("selected", true)
        form.render();
    }
    getPageData = function () { };//getCallBack;
    var MakeBeginTime = null, MakeEndTime = null,
    // 填充页面模版到页面容器中
    emptyFormData = {ID:"", CategoryID: "", Tool: "", Content: "" };
    Model.init("CallBack", "/Customer/CallBack", "添加回访", "CallbackAdd", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });
})(window, jQuery);