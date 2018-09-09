$(function () {
    layui.use('form', function () {
        window.form = layui.form();
    });
});

var i = 1;

//开始展示单位信息数据
$("#smartStockHtml").ready(
    function () {
        var getSmartStocklierFunc = function () {

            var ckId = $("#smartStockDetaiName").val(); //仓库id
            var py = $("#pinYinValue").val();//拼音码
            var gyName = $("#nameValue").val();//名称
            var smartCategoryName = $("#smartCategoryName").val();//名称
            var url = "/SmartStock/SmartStockInfoGet";//查询
            var realData = {};
            realData.WarehouseID = ckId;
            realData.PinYin = py;
            realData.WPName = gyName;
            realData.CategoryID = smartCategoryName;
            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($("#smartStock_template").text());
            $(".layui-field-box").html(interText(data));

        };
        getSmartStocklierFunc();

    });

function aa() {
    var ckId = $("#smartStockDetaiName").val(); //仓库id
    var py = $("#pinYinValue").val();//拼音码
    var gyName = $("#nameValue").val();//名称
    var smartCategoryName = $("#smartCategoryName").val();//名称
    var url = "/SmartStock/SmartStockInfoGet";//查询
    var realData = {};
    realData.WarehouseID = ckId;
    realData.PinYin = py;
    realData.WPName = gyName;
    realData.CategoryID = smartCategoryName;
    var paraObj = {};
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($("#smartStock_template").text());
    if (data == null) {
        $(".layui-field-box").html(interText(""));
    } else {
        $(".layui-field-box").html(interText(data));
    }
    form.render();

}