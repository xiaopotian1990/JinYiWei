(function (win, $) {
    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none");
        //提交欠款
        pageContent.on("click", ".RefundMoney .add-debt-submit", function () {
            joinParams(pageContent);
            params.setDataParams({
                IsDebt: 1
            });
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
                getNoPaidOrders();
            }
        });
        //
        pageContent.on("click", ".RefundMoney .add-submit", function () {
            var _allAmount = joinParams(pageContent);
            if (_allAmount != parseInt(pageContent.find("input[name=toAmount]").val())) {
                layer.msg("金额不一致无法收银");
                return;
            }
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
                getNoPaidOrders();
            }
        });


        
        pageContent.on("change", ".order-pay-tbody > tr", function () {
            var FinalPrice = pageContent.find("input[name=toAmount]").val();
            var amount = 0;
            $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
                amount += parseInt(pageContent.find(e).find("input[name=tRefundMoneyAmount]").val());
            });
            pageContent.find("input[name=FinalPrice]").val(FinalPrice - amount); 
        });

        pageContent.on("click", ".add-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            var tdH = "<td class=\"fl\"><span class=\"layui-btn layui-btn-danger del-pay-btn fr \"><i class=\"layui-icon\">&#xe640;</i></span></td>"
            var tdHs = "<tr><td>" + _tr.children().eq(0).html()+ "</td><td>"+ _tr.children().eq(1).html()+ "</td>"+ tdH + "</tr>";
            var html = pageContent.find(".order-pay-tbody").html();
            pageContent.find(".order-pay-tbody").append(tdHs);
            form.render();
        });
        pageContent.on("click", ".del-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            _tr.remove();
        });

        form.render();
    };

    function joinParams(pageContent) {
        var customerId = $("input[name=custid]").val();
        var orderId = $("input[name=orderid]").val();
        var CardList = [];
        var _allAmount = 0;
        $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
            var obj = {};
            obj['CardCategoryID'] = $(e).find(".RefundMoneyCard").val();
            obj['Amount'] = parseInt($(e).find("input[name=tRefundMoneyAmount]").val());
            if (obj['Amount'] > 0) {
                CardList.push(obj);
                _allAmount += obj['Amount'];
            }
        });
        _allAmount += parseInt(pageContent.find("input[name=FinalPrice]").val());
        
        params.setDataParams({
            CustomerID: customerId,
            OrderID: orderId,//
            Cash: pageContent.find("input[name=FinalPrice]").val(),//现金
            CardList: CardList,//支付列表
            Remark: pageContent.find("[name=Remark]").val()
        });
        return _allAmount;
    }

    var openFunc = function (layero, pageContent, data) {
        //获取
        pageContent.find("[name=toAmount]").val(0);
        pageContent.find("[name=FinalPrice]").val(0);

        pageContent.find("input[name=CustomerID]").val("");
        pageContent.find("input[name=CreateTime]").val("");
        pageContent.find("input[name=CreateUserName]").val("");
        pageContent.find("input[name=Amount]").val("");
        pageContent.find("input[name=DebtAmount]").val("");
        pageContent.find("input[name=Remark]").val("");
        
        //获取项目详情
        params.setDataParams({
            customerId: $("input[name=custid]").val(),
            orderId: $("input[name=orderid]").val()
        });
        var resultInfo = ajaxObj.setUrl("/CashierDesk/GetOrderDetail").setParaObj(params).getData();
        if (resultInfo != null && resultInfo.ResultType == 0) {
            pageContent.find("input[name=CustomerID]").val(resultInfo.Data.CustomerID);
            pageContent.find("input[name=CreateTime]").val(resultInfo.Data.CreateTime);
            pageContent.find("input[name=CreateUserName]").val(resultInfo.Data.CreateUserName);
            pageContent.find("input[name=Amount]").val(resultInfo.Data.FinalPrice); 
            pageContent.find("input[name=DebtAmount]").val(resultInfo.Data.DebtAmount);
            pageContent.find("input[name=Remark]").val(resultInfo.Data.Remark);

            pageContent.find("input[name=toAmount]").val(resultInfo.Data.DebtAmount);
            pageContent.find("input[name=FinalPrice]").val(resultInfo.Data.DebtAmount);
            var html = '';
            var ChargeDetials = resultInfo.Data.ChargeDetials;
            if (ChargeDetials != null && ChargeDetials.length > 0) {
                for (var item in ChargeDetials) {
                    var obj = ChargeDetials[item];
                    html += "<tr><td>" + obj["ChargeID"] + "</td>" +
                        "<td>" + obj["ChargeName"] + "</td><td>" + obj["Num"] + "</td>" +
                        "<td>" + obj["Price"] * obj["Num"] + "</td><td>" + obj["FinalPrice"] + "</td>" +
                        "<td>--</td></tr>";
                }
            }

            var SetDetials = resultInfo.Data.SetDetials;
            if (SetDetials != null && SetDetials.length > 0) {
                for (var item in SetDetials) {
                    var SetDetial = SetDetials[item];
                    var CDetails = SetDetial["ChargeDetails"];
                    if (CDetails != null && CDetails.length > 0) {
                        for (var i in CDetails) {
                            var ChargeDetial = CDetails[i];
                            html += "<tr><td>" + ChargeDetial["ChargeID"] + "</td>" +
                                "<td>" + ChargeDetial["ChargeName"] + "</td><td>" + ChargeDetial["Num"] + "</td>" +
                                "<td>" + ChargeDetial["Price"] * ChargeDetial["Num"] + "</td><td>" + ChargeDetial["FinalPrice"] + "</td>" +
                                "<td>" + SetDetial["SetName"] + "</td></tr>";
                        }
                    }
                }
            }

            pageContent.find(".order-tbody").html(html);
        }
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    //
    Model.init("RefundMoney", "/CashierDesk/RefundMoney", "收银", "DebtCashier", emptyFormData, successFunc, openFunc).setArea(["85%;", "80%;"]);
})(window, jQuery);