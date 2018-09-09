var i = 1;
var wareHoseId = "";
//显示
$("#selPurchasehtml")
    .ready(function () {
        var infoFunc = function () {

            var realData = {};
            realData.BeginTime = $("#beginDate").val();
            realData.EndTime = $("#endDate").val();
            realData.WarehouseID = $("#smartDRWarehouse").val();
            var url = "/SmartPurchase/SmartPurchaseByHospitalIDGet";

            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#smartPurchase_template").text());
            $(".layui-field-box").html(interText(data.Data));
            form.render();
        };
        infoFunc();
    });


function btnSerache() {
    var realData = {};
    realData.BeginTime = $("#beginDate").val();
    realData.EndTime = $("#endDate").val();
    realData.WarehouseID = $("#smartDRWarehouse").val();
    var url = "/SmartPurchase/SmartPurchaseByHospitalIDGet";

    var paraObj = new Object();
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj);

    var interText = doT.template($("#smartPurchase_template").text());
    $(".layui-field-box").html(interText(data.Data));
    layform.render();
}