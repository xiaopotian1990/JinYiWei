var i = 1;
var wareHoseId = "";
//显示
$("#selProducthtml")
    .ready(function () {
        var infoFunc = function () {

            var realData = {};
            realData.PinYin = $("#smartProductPinYin").val();
            realData.WPName = $("#smartProductName").val();
            realData.CategoryId = $("#smartProductDetaiName").val();
            realData.WarehouseID = parent.$('#smartAddDCWarehouse').val();//父页面选择的仓库id
            realData.Type = "1"; //进货入库类型
            wareHoseId = parent.$('#smartAddWarehouse').val();
            var url = "/SmartStock/SmartStockConditionGet";

            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#smartProduct_template").text());
            var tempHtml = $(interText(data.Data));
            if (parent.window.productStockId != undefined) {
                $.each(parent.window.productStockId, function (i, item) {
                    tempHtml.find("[deptinfoid=" + item + "]").remove();
                });
            }
            
            $(".layui-field-box").html(tempHtml);
        };
        infoFunc();
    });


function btnSerache() {
    var realData = {};
    realData.PinYin = $("#smartProductPinYin").val();
    realData.Name = $("#smartProductName").val();
    realData.CategoryId = $("#smartProductDetaiName").val();
    realData.WarehouseID = parent.$('#smartAddDCWarehouse').val();//父页面选择的仓库id
    //alert(parent.$('#smartAddWarehouse').val());
    //alert(wareHoseId);
    realData.Type = "1"; //进货入库类型

    var url = "/SmartStock/SmartStockConditionGet";

    var paraObj = new Object();
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj);

    var interText = doT.template($("#smartProduct_template").text());
    $(".layui-field-box").html(interText(data.Data));
}