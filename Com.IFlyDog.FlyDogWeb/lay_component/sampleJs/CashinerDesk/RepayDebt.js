$(function () {
    layui.use(["form","element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(),
             laydate = layui.laydate(),
             layer = layui.layer,
             laypage = layui.laypage;
             window.form = layui.form(); 
        // 添加按钮
        $(".tb-debt").on("click", ".model-btn", function () {
            var _this = $(this), model = _this.data("model"),
                action = _this.data("action");
            $("input[name=custid]").val(_this.parent().parent().attr("data-customerid"));
            $("input[name=orderid]").val(_this.parent().parent().attr("data-orderid"));
            if (!model) return false;
            if (!window[model]) {
                loadScript("/lay_component/sampleJs/CashinerDesk/" + model + ".js",
                    function () {
                        action ? window[model] && window[model].setSubmitUrl(action).useEmptyEntry().openPop() : window[model] && window[model].useEmptyEntry().openPop();
                    });
            } else {
                action ? window[model].useEmptyEntry().setSubmitUrl(action).openPop() : window[model].useEmptyEntry().openPop();
            }
        });
    }); 
    getDebtCashi();
    //查询
    $(".debt-search-btn").click(function() {
        getDebtCashi();
    });

});
//查询欠款
var getDebtCashi = function() {
    var url = "/CashierDesk/GetDebtOrdes";
    var paraObj = {
        data: { 
            StartTime: $("#beginTime").val(),
            EndTime: $("#endTime").val(),
            CustomerID: $("#cuID").val(),
            HospitalID: $("#debtHospital").val()
           }
    };
    var result = ajaxProcess(url, paraObj); 
    if (result.ResultType === 0) {
        var interText = doT.template($(".debt-tmp").text());
        $(".tb-debt").html(interText(result.Data));
    } 

};