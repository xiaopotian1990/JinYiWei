(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".CallBackPlan.add-submit", function () {
            if (!verify()) {
                return false;
            };

            params.setDataParams({
                CustomerID: custid,
                SetID: pageContent.find("[name=SetID]").val(),
                TaskTime: pageContent.find("[name=TaskTime]").val(),
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
        pageContent.on("click", ".add-plan", function () {
            var dotEle = [{ container: ".callBackPlan-table", tmp: ".callBackPlan-tmp" }];
            ajaxObj.setUrl("/CustomerProfile/GetCallbackSet").setDotEle(dotEle).getData();
            var callBackPlansEle = pageContent.find(".callBackPlans-pop");
            if ($("body").find(".callBackPlansEle").length > 0) $("body").find(".callBackPlansEle"), remove();
            $("body").append(callBackPlansEle);
            openPop("", ".callBackPlans-pop", "选择回访计划");
        });
        pageContent.find(".callBackPlan-table").on("click", "tr", function () {
            getPlanDetail($(this).data("id"));
        });
        pageContent.find(".callBackPlan-table").on("click", ".btn-select", function () {
            getPlanDetail($(this).parents("tr").data("id"));
            closeLayer(this);
        });
        window.CallBackPlan.getDetail = function (id) {
            params.setDataParams({
                ID: id,
                customerID: custid
            });
            var result = ajaxObj.setUrl("/CustomerProfile/GetCallPlanDetail").setParaObj(params).getData();
            this.setEntry(result.Data);
            return this;
        }
        var verify = function () {
            if (Common.StrUtils.isNullOrEmpty(pageContent.find("[name=TaskTime]").val())) {
                layer.msg("日期不能为空！", {icon:2});
                return false;
            }
            return true;
        };
        form.render();
        var getPlanDetail = function (setID) {
            params.setDataParam("setID", setID);
            $("[name=SetID]").val(setID);
            var dotEle = [{ container: ".callBackPlan-detail-table", tmp: ".callBackPlan-detail-tmp" }];
            ajaxObj.setUrl("/CustomerProfile/GetCallbackSetDetail").setParaObj(params).setDotEle(dotEle).getData();
        }
    };
    var openFunc = function (layero, pageContent, data) {
        // 填充页面模版到页面容器中
        fillData(pageContent.find(".callBackPlan-detail-table"), pageContent.find(".callBackPlan-detail-tmp"), data);
        form.render();
    }
    getPageData = function () { };
    var MakeBeginTime = null, MakeEndTime = null,
    // 填充页面模版到页面容器中
    emptyFormData = { ID: "", CategoryID: "", TaskTime: "", Name: "", UserID: "", UserName: "" };
    Model.init("CallBackPlan", "/Customer/CallBackPlan", "添加回访计划", "CallbackPlanAdd", emptyFormData, successFunc, openFunc, ".callBackPlan");
})(window, jQuery);