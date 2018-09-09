(function (win, $) {
    var ids = [];
    var successFunc = function (pageContent) {
        $(".ToMoney").css("border", "none");
        //提交
        pageContent.on("click", ".AddInvoice .add-submit", function () {
            var customerId = $("input[name=custid]").val();
            var Detail = [];
            $.each($(".invoice-detail-tbody").children(), function (i, e) {
                var id = $(e).attr("data-id");
                Detail.push(id);
            });
            params.setDataParams({
                CustomerID: customerId,
                Code: pageContent.find("input[name=Code]").val(),//
                Name: pageContent.find("input[name=Name]").val(),//
                Amount: parseInt(pageContent.find("input[name=Amount]").val()),//
                CreateDate:pageContent.find("input[name=BillTime]").val(),
                OrderDetailID: Detail,//支付列表
                Remark: pageContent.find("[name=Remark]").val()
            });
            if (ajaxObj.setUrl("/CashierDesk/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType === 0) {
                $("input").val("");
                closeLayer(this);
            }
        });

        pageContent.on("click", ".add-detail", function () {
    
            openPop("", ".bill-detail", "发票详情");
            params.setDataParams({
                customerId: $("input[name=custid]").val()
            });
            var result = ajaxObj.setUrl("/CashierDesk/GetCanBillCharges").setParaObj(params).getData();
            if (result != null && result.ResultType == 0) {
                fillData($(".bill-detail-tbody"), $(".bill-detail-tmp"), result.Data);
            }

        });
        
        pageContent.on("click", ".del-invoice-detail", function () {
            $tr = $(this).parent().parent();
            ids.splice($.inArray($tr.attr('data-id'), ids), 1);
//            ids.splice($tr.attr("data-id"));
            $tr.remove();
        });

        $(".bill-detail-tbody").on("click", ".select-detail", function () {
            $tr = $(this).parent().parent();
            var id = $tr.attr("data-id");
            if ($.inArray(id, ids) < 0) {
                ids.push(id);
                $tds = $tr.children();
                var html = '<tr data-id="' + id + '">'
                    + '<td>' + $tds[0].innerHTML + '</td>'
                    + '<td>' + $tds[1].innerHTML + '</td>'
                    + '<td>' + $tds[2].innerHTML + '</td>'
                    + '<td>' + $tds[3].innerHTML + '</td>'
                    + '<td>' + $tds[4].innerHTML + '</td>'
                    + '<td>' + $tds[5].innerHTML + '</td>'
                    + '<td>' + $tds[6].innerHTML + '</td>'
                    + '<td>' + $tds[7].innerHTML + '</td>'
                    + '<td><span class="layui-btn layui-btn-mini layui-btn-danger del-invoice-detail">删除</span></td></tr>';
                pageContent.find('.invoice-detail-tbody').append(html);
            }
            
        });
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        pageContent.find('.invoice-detail-tbody').html('');
        ids.length = 0;
        form.render();
    }
    getPageData = function () { };//getCallBack;
    // 填充页面模版到页面容器中
    emptyFormData = {};
    //
    Model.init("AddInvoice", "/CashierDesk/AddInvoice", "发票", "AddBill", emptyFormData, successFunc, openFunc).setArea(["85%;", "80%;"]);
})(window, jQuery);