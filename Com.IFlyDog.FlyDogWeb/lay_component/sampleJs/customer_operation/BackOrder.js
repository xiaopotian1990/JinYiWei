(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".backorder .add-submit", function () {
            var details = [];
            $tr = pageContent.find("tbody > tr");
            $.each($tr, function (i, e) {
                var obj = {};
                obj['ChargeID'] = $(e).attr("data-id");
                obj['DetailID'] = $(e).attr("data-detailid");
                obj['Num'] = $(e).find("[name=tNum]").val();
                obj['Amount'] = $(e).find("[name=tAmount]").val();
                details.push(obj);
            });
            params.setDataParams({
                CustomerID: custid,
                Point: pageContent.find("[name=Point]").val(),
                Remark: pageContent.find("[name=Remark]").val(),
                Details: details
            });
            if (ajaxObj.setUrl("/CustomerProfile/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        
        pageContent.on("change", "tbody > tr", function () {
            var allNum = 0, allAmount = 0;
            $tr = pageContent.find("tbody > tr");
            $.each($tr, function (i, e) {
                allNum += parseInt($(e).find("[name=tNum]").val());
                allAmount += parseInt($(e).find("[name=tAmount]").val());
            });
            pageContent.find("[name=allNum]").html(allNum);
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
        var result = ajaxObj.setUrl("/CustomerProfile/GetNoDoneOrders").setParaObj(params).getData();
        fillData(pageContent.find(".backorder-table"), pageContent.find(".backorder-tmp"), result.Data);
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {ID:"", CategoryID: "", Tool: "", Content: "" };
    Model.init("BackOrder", "/Customer/AddBackOrder", "退项目", "AddBackOrder", emptyFormData, successFunc, openFunc).setArea(["65%;", "60%;"]);
})(window, jQuery);