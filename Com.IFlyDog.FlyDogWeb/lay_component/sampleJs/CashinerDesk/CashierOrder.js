(function (win, $) {

    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none"); 
        //提交
        //欠款-收银
        pageContent.on("click", ".cashier-order .add-debt-submit", function () {
            var _allAmount = joinParams(pageContent);
            params.setDataParams({
                IsDebt: "1"
            });
            console.log(params);
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this); 
                getNoPaidOrders();
            }
        });
        //收银
        var html = "";
        pageContent.on("click", ".cashier-order .add-submit", function () {
            var cuid = $(".fr input[name=CustomerID]").val();
            var amount = $(".fl input[name=toAmount]").val();
   
            var _allAmount = joinParams(pageContent);
            params.setDataParams({
                IsDebt: "0"
            });
            //console.log(params); 
            if (_allAmount == parseInt(pageContent.find("input[name=toAmount]").val())) {
                if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                    closeLayer(this);
                    getNoPaidOrders();
                    /*收取金额-推送信息*/
                    customerConsume(cuid, amount);
                }
            } else {
                layer.msg("收款金额不匹配");
            }

        });
        
        pageContent.on("change", "tbody > tr", function () {
            computeAmount(pageContent);
        }); 

        pageContent.on("change", "input[name=Commission]", function () {
            computeAmount(pageContent);
        }); 

        pageContent.on("click", ".add-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            var tdH ="<td class=\"fl\"><span class=\"layui-btn layui-btn-danger del-pay-btn fr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
            var tdHs = "<tr><td>" + _tr.children().eq(0).html()+ "</td><td>"+ _tr.children().eq(1).html()+ "</td>"+ tdH +"</tr>";
            var html = pageContent.find(".order-pay-tbody").html();
        
            pageContent.find(".order-pay-tbody").html(html + tdHs);
            form.render();
        });
        
        pageContent.on("click", ".del-pay-btn", function () {
            _this = $(this);
            _tr = _this.parent().parent();
            _tr.remove();
        });
         
    };

    function computeAmount(pageContent) {
        var FinalPrice = pageContent.find("input[name=toAmount]").val();
        var amount = 0;
        $.each(pageContent.find(".deposits-tbody > tr"), function (i, e) {
            amount += parseInt(pageContent.find(e).find("input[name=tUseDepositAmount]").val());
        });
        $.each(pageContent.find(".coupons-tbody > tr"), function (i, e) {
            amount += parseInt(pageContent.find(e).find("input[name=tUseCouponAmount]").val());
        });
        $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
            amount += parseInt(pageContent.find(e).find("input[name=tCasherAmount]").val());
        });
        amount += parseInt(pageContent.find("input[name=Commission]").val());
        pageContent.find("input[name=FinalPrice]").val(FinalPrice - amount); 
    }
    
    function joinParams(pageContent) {
        var customerId = $("input[name=custid]").val();
        var orderId = $("input[name=orderid]").val();
        var CardList = [], DepositUseList = [], CouponUseList = [];
        var _allAmount = 0;

        $.each(pageContent.find(".order-pay-tbody > tr"), function (i, e) {
            var obj = {};
            obj['CardCategoryID'] = $(e).find(".CardCategory").val();
            obj['Amount'] = parseInt($(e).find("input[name=tCasherAmount]").val());
            if (obj['Amount'] > 0) {
                CardList.push(obj);
                _allAmount += obj['Amount'];
            }
        });

        $.each(pageContent.find(".deposits-tbody > tr"), function (i, e) {
            var obj = {};
            obj['CardCategoryID'] = $(e).attr("data-depositid");
            obj['Amount'] = parseInt($(e).find("input[name=tUseDepositAmount]").val());
            if (obj['Amount'] > 0) {
                DepositUseList.push(obj);
                _allAmount += obj['Amount'];
            }
        });
        $.each(pageContent.find(".coupons-tbody > tr"), function (i, e) {
            var obj = {};
            obj['CardCategoryID'] = $(e).attr("data-couponid");
            obj['Amount'] = parseInt($(e).find("input[name=tUseCouponAmount]").val());
            if (obj['Amount'] > 0) {
                CouponUseList.push(obj);
                _allAmount += obj['Amount'];
            }
        });
        _allAmount += parseInt(pageContent.find("input[name=FinalPrice]").val());
        _allAmount += parseInt(pageContent.find("input[name=Commission]").val());

        params.setDataParams({
            CustomerID: customerId,
            OrderID: orderId,//
            Cash: pageContent.find("input[name=FinalPrice]").val(),//现金
            Commission: pageContent.find("input[name=Commission]").val(),//佣金
            CardList: CardList,//支付列表
            DepositUseList: DepositUseList,
            CouponUseList: CouponUseList,
            Remark: pageContent.find("[name=orderRemark]").val()
        });
        return _allAmount;
    }
    var openFunc = function (layero, pageContent, data) {
        pageContent.find("[name=toAmount]").val(0);
        pageContent.find("[name=toMoney]").val(0);
        pageContent.find("[name=toDiscount]").val(0);

        pageContent.find("input[name=CustomerID]").val("");
        pageContent.find("input[name=CreateTime]").val("");
        pageContent.find("input[name=CreateUserName]").val("");
        pageContent.find("input[name=FinalPrice]").val("");
        pageContent.find("input[name=Remark]").val("");

        //获取预收款及券
        params.setDataParams({
            customerId: $("input[name=custid]").val(),
            orderId: $("input[name=orderid]").val()
        });
        var result = ajaxObj.setUrl("/CashierDesk/GetCanCashier").setParaObj(params).getData();
        if (result != null && result.ResultType == 0) {
            pageContent.find("input[name=toMoney]").val(result.Data.Commission); 
            fillData(pageContent.find(".coupons-tbody"), pageContent.find(".coupons-tmp"), result.Data.Coupons);
            fillData(pageContent.find(".deposits-tbody"), pageContent.find(".deposits-tmp"), result.Data.Deposits);
        }
        //获取项目详情
        params.setDataParams({
            customerId: $("input[name=custid]").val(),
            orderId: $("input[name=orderid]").val()
        });
        var resultInfo = ajaxObj.setUrl("/CashierDesk/GetOrderDetail").setParaObj(params).getData();
        if (resultInfo != null && resultInfo.ResultType === 0) {
            pageContent.find("input[name=CustomerID]").val(resultInfo.Data.CustomerID);
            pageContent.find("input[name=CreateTime]").val(resultInfo.Data.CreateTime);
            pageContent.find("input[name=CreateUserName]").val(resultInfo.Data.CreateUserName);
            pageContent.find("input[name=FinalPrice]").val(resultInfo.Data.FinalPrice);
            pageContent.find("input[name=Remark]").val(resultInfo.Data.Remark);

            pageContent.find("input[name=toAmount]").val(resultInfo.Data.FinalPrice);
            pageContent.find("input[name=toDiscount]").val(resultInfo.Data.Discount); 
            pageContent.find("input[name=FinalPrice]").val(resultInfo.Data.FinalPrice);
            var html = '';
            var ChargeDetials = resultInfo.Data.ChargeDetials;
            if (ChargeDetials != null && ChargeDetials.length > 0){
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
                    if (CDetails != null && CDetails.length > 0){
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
    // js /html url /tital /controller method
    Model.init("CashierOrder", "/CashierDesk/CashierOrder", "收银", "CashierOrder", emptyFormData, successFunc, openFunc).setArea(["90%;", "80%;"]);
    form.render();
})(window, jQuery);