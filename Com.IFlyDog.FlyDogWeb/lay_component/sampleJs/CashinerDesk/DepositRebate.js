(function (win, $) {
    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none");
        //提交
        pageContent.on("click", ".DepositRebate .add-submit", function () {
            var customerId = $("input[name=custid]").val();
            var orderId = $("input[name=orderid]").val();
            var CardList = [];
            var _allAmount = 0;
            _allAmount += parseInt(pageContent.find("input[name=tDepositRebateAmount]").val());
            _allAmount += parseInt(pageContent.find("input[name=FinalPrice]").val());

            if (_allAmount != parseInt(pageContent.find("input[name=toAmount]").val())) {
                layer.msg("金额不一致无法收银");
                return;
            }

            params.setDataParams({
                CustomerID: customerId,
                OrderID: orderId,
                Cash: pageContent.find("[name=FinalPrice]").val(),
                CardCategoryID: pageContent.find(".DepositRebateCard").val(),
                Card: pageContent.find("input[name=tDepositRebateAmount]").val(),
                Remark: pageContent.find("[name=Remark]").val()
            });
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        
        pageContent.on("change", "input[name=tDepositRebateAmount]", function () {
            var FinalPrice = pageContent.find("input[name=toAmount]").val();
            var amount = parseInt(pageContent.find("input[name=tDepositRebateAmount]").val());
            pageContent.find("input[name=FinalPrice]").val(FinalPrice - amount);
        });
        
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        //获取预收款
        params.setDataParams({
            customerId: $("input[name=custid]").val(),
            orderId: $("input[name=orderid]").val()
        });
        var result = ajaxObj.setUrl("/CashierDesk/GetDepositRebateDetail").setParaObj(params).getData();
        if (result != null && result.ResultType == 0) {
            pageContent.find("input[name=toAmount]").val(result.Data.Amount);
            pageContent.find("input[name=FinalPrice]").val(result.Data.Amount);

            pageContent.find("input[name=CustomerID]").val($("input[name=custid]").val());
            pageContent.find("input[name=CreateTime]").val(result.Data.CreateTime);
            pageContent.find("input[name=CreateUserName]").val(result.Data.CreateUserName);
            pageContent.find("input[name=Amount]").val(result.Data.Amount);
            pageContent.find("input[name=Remark]").val(result.Data.Remark);

            fillData(pageContent.find(".DepositRebate-tbody"), pageContent.find(".DepositRebate-tmp"), result.Data.Details);
            fillData(pageContent.find(".DepositRebateC-tbody"), pageContent.find(".DepositRebateC-tmp"), result.Data.CouponDetails);
        }
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    //
    Model.init("DepositRebate", "/CashierDesk/DepositRebate", "退预收款收银", "DepositRebateOrderCashier", emptyFormData, successFunc, openFunc).setArea(["85%;", "80%;"]);
})(window, jQuery);