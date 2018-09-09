(function (win, $) {
    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none");
        //提交
        pageContent.on("click", ".AddDeposit .add-submit", function () { 
            var amount = $(".fr input[name=Amount]").val();

            var customerId = $("input[name=custid]").val();
            var orderId = $("input[name=orderid]").val();
            var CardList = [];
            var _allAmount = 0;
            $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
                var obj = {};
                obj['CardCategoryID'] = $(e).find(".AddDepositCard").val();
                obj['Amount'] = parseInt($(e).find("input[name=tAddDepositAmount]").val());
                if (obj['Amount'] > 0) {
                    CardList.push(obj);
                    _allAmount += obj['Amount'];
                }
            });
            _allAmount += parseInt(pageContent.find("input[name=FinalPrice]").val());
            if (_allAmount != parseInt(pageContent.find("input[name=toAmount]").val())){
                layer.msg("金额不一致无法收银");
                return;
            }
            params.setDataParams({
                CustomerID: customerId,
                OrderID: orderId,//
                Cash: pageContent.find("input[name=FinalPrice]").val(),//现金
                CardList: CardList,//支付列表
                Remark: pageContent.find("[name=Remark]").val()
            });
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
                getNoPaidOrders(); 
                /*收取金额-推送信息*/
                customerConsume(customerId, amount);
            }
        });
        
        pageContent.on("change", ".order-pay-tbody > tr", function () {
            var FinalPrice = pageContent.find("input[name=toAmount]").val();
            var amount = 0;
            $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
                amount += parseInt(pageContent.find(e).find("input[name=tAddDepositAmount]").val());
            });
            pageContent.find("input[name=FinalPrice]").val(FinalPrice - amount); 
        });

        pageContent.on("click", ".add-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            var tdH = "<td class=\"fl\"><span class=\"layui-btn layui-btn-danger del-pay-btn fr \"><i class=\"layui-icon\">&#xe640;</i></span></td>"
            var tdHs = "<tr><td>" + _tr.children().eq(0).html()+ "</td><td>"+ _tr.children().eq(1).html()+ "</td>"+ tdH + "</tr>";
            var html = pageContent.find(".order-pay-tbody").html();
            pageContent.find(".order-pay-tbody").html(html + tdHs);
            form.render();
        });
        pageContent.on("click", ".del-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            _tr.remove();
        });

        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        //获取预收款
        params.setDataParams({
            orderId: $("input[name=orderid]").val()
        });
        var result = ajaxObj.setUrl("/Deposit/GetDepositDetail").setParaObj(params).getData();
        if (result != null && result.ResultType == 0) {
            pageContent.find("input[name=toAmount]").val(result.Data.Amount);
            pageContent.find("input[name=FinalPrice]").val(result.Data.Amount);

            pageContent.find("input[name=CustomerID]").val(result.Data.CustomerID);
            pageContent.find("input[name=CreateTime]").val(result.Data.CreateTime);
            pageContent.find("input[name=CreateUserName]").val(result.Data.CreateUserName);
            pageContent.find("input[name=Amount]").val(result.Data.Amount);
            pageContent.find("input[name=Remark]").val(result.Data.Remark); 
            
            fillData(pageContent.find(".DepositOrderDetial-tbody"), pageContent.find(".addDeposits-tmp"), result.Data.Details);
        }
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    //
    Model.init("AddDeposit", "/CashierDesk/AddDeposit", "预收款收银", "DepositOrderCashier", emptyFormData, successFunc, openFunc).setArea(["85%;", "80%;"]);
})(window, jQuery);