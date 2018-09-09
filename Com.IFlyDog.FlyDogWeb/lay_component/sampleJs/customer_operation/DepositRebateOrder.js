(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".depositRebateOrder .add-submit", function () {
            var details = [], couponDetails = [];

            $Couponstr = pageContent.find("#Coupons").find("tbody > tr");
            $.each($Couponstr, function (i, e) {
                var obj = {};
                obj["CouponID"] = $(e).attr("data-id");
                obj["Amount"] = $(e).find("input").val();
                if (obj["Amount"] > 0){
                    couponDetails.push(obj);
                }
            });

            $Depositstr = pageContent.find("#Deposits").find("tbody > tr");
            $.each($Depositstr, function (i, e) {
                var obj = {};
                obj["DepositID"] = $(e).attr("data-id");
                obj["Amount"] = $(e).find("input").val();
                if (obj["Amount"] > 0) {
                    details.push(obj);
                }
            });

            params.setDataParams({
                CustomerID: custid,
                Point: pageContent.find("[name=Point]").val(),
                Amount: pageContent.find("[name=Amount]").val(),
                Remark: pageContent.find("[name=Remark]").val(),
                Details: details,
                CouponDetails: couponDetails
            });
            //console.log(params);
            if (ajaxObj.setUrl("/CustomerProfile/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        //改券
        pageContent.on("change", "input[name=couponsNum]", function () {
            var allNum = 0;
            $tr = pageContent.find("#Coupons").find("tbody > tr");
            $.each($tr, function (i, e) {
                allNum += parseInt($(e).find("[name=couponsNum]").val());
            });
            pageContent.find("[name=allNum]").html(allNum);
        });
        //改预收款
        pageContent.on("change", "input[name=depositsNum]", function () {
            var allAmount = 0;
            $tr = pageContent.find("#Deposits").find("tbody > tr");
            $.each($tr, function (i, e) {
                allAmount += parseInt($(e).find("[name=depositsNum]").val());
            });
            pageContent.find("[name=allAmount]").html(allAmount);
        });
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        pageContent.find("[name=allNum]").html(0);
        pageContent.find("[name=allAmount]").html(0);
        params.setDataParams({
            customerId: custid
        });
        var result = ajaxObj.setUrl("/CustomerProfile/GetCanRebate").setParaObj(params).getData();
        //console.log(result);
        if (result.ResultType == 0){
            fillData(pageContent.find("#Deposits"), pageContent.find(".deposits-tmp"), result.Data.Deposits);
            fillData(pageContent.find("#Coupons"), pageContent.find(".coupons-tmp"), result.Data.Coupons);
        }
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    Model.init("DepositRebateOrder", "/Customer/AddRebateDeposit", "退预收款", "AddDepositRebate", emptyFormData, successFunc, openFunc).setArea(["90%;", "90%;"]);
})(window, jQuery);