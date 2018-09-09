(function (win, $) {
    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none");
        //提交
        pageContent.on("click", ".BackOrder .add-submit", function () {
            var customerId = $("input[name=custid]").val();
            var orderId = $("input[name=orderid]").val();
            var _allAmount = parseInt(pageContent.find("input[name=tBackOrderAmount]").val());
            _allAmount += parseInt(pageContent.find("input[name=tBackOrderCAmount]").val());
            _allAmount += parseInt(pageContent.find("input[name=tBackOrderDAmount]").val());
            _allAmount += parseInt(pageContent.find("input[name=FinalPrice]").val());

            if (_allAmount != parseInt(pageContent.find("input[name=toAmount]").val())) {
                layer.msg("金额不一致无法收银");
                return;
            }

            params.setDataParams({
                CustomerID: customerId,
                OrderID: orderId,
                Cash: pageContent.find("[name=FinalPrice]").val(),
                CardCategoryID: pageContent.find(".BackOrderCard").val(),
                Card: pageContent.find("input[name=tBackOrderAmount]").val(),
                DepositChargeID: pageContent.find(".BackOrderDeposit").val(),
                Deposit: pageContent.find("input[name=tBackOrderDAmount]").val(),
                CouponCategoryID: pageContent.find(".BackOrderCoupon").val(),
                Coupon: pageContent.find("input[name=tBackOrderCAmount]").val(),
                Remark: pageContent.find("[name=Remark]").val()
            });
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
                getNoPaidOrders();
            }
        });
        
        pageContent.on("change", "tbody > tr", function () {
            
        });
        
        pageContent.on("change", "input[name=tBackOrderAmount]", function () {
            computerAll();
        });
        pageContent.on("change", "input[name=tBackOrderCAmount]", function () {
            computerAll();
        });
        pageContent.on("change", "input[name=tBackOrderDAmount]", function () {
            computerAll();
        });
        function computerAll() {
            var FinalPrice = pageContent.find("input[name=toAmount]").val();
            var amount = parseInt(pageContent.find("input[name=tBackOrderAmount]").val());
            amount += parseInt(pageContent.find("input[name=tBackOrderCAmount]").val());
            amount += parseInt(pageContent.find("input[name=tBackOrderDAmount]").val());
            pageContent.find("input[name=FinalPrice]").val(FinalPrice - amount);
            return amount;
        }


        form.render();
    };
    var openFunc = function (layero, pageContent, data) {

        params.setDataParams({
            customerId: $("input[name=custid]").val(),
            orderId: $("input[name=orderid]").val()
        });
        var result = ajaxObj.setUrl("/CashierDesk/GetBackOrderDetail").setParaObj(params).getData();
        if (result != null && result.ResultType == 0) {
            pageContent.find("input[name=toAmount]").val(result.Data.Amount);
            pageContent.find("input[name=FinalPrice]").val(result.Data.Amount); 
            pageContent.find("input[name=CustomerID]").val($("input[name=custid]").val());
            pageContent.find("input[name=CreateTime]").val(result.Data.CreateTime);
            pageContent.find("input[name=CreateUserName]").val(result.Data.CreateUserName);
            pageContent.find("input[name=Amount]").val(result.Data.Amount);
            pageContent.find("input[name=Remark]").val(result.Data.Remark);

            fillData(pageContent.find(".BackOrder-tbody"), pageContent.find(".BackOrder-tmp"), result.Data.Details);
        }
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    //
    Model.init("BackOrder", "/CashierDesk/BackOrder", "收银", "BackOrderCashier", emptyFormData, successFunc, openFunc).setArea(["85%;", "80%;"]);

})(window, jQuery);