var i = 1;
//显示
$("#selProducthtml")
    .ready(function () {
        var infoFunc = function () {
            var productPinYin = $("#smartProductPinYin").val();
            var productName = $("#smartProductName").val();
            var smartProductDetail = $("#smartProductDetaiName").val();

            var realData = {};
            realData.PinYin = productPinYin;
            realData.Name = productName;
            realData.CategoryId = smartProductDetail;

            var url = "/SmartProduct/SmartProductInfoGetAll";

            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj);


            var interText = doT.template($("#smartProduct_template").text());
            var tempHtml = $(interText(data.Data));
            if (parent.window.productId != undefined) {
                $.each(parent.window.productId, function (i, item) {
                    tempHtml.find("[deptinfoid=" + item + "]").remove();
                });
            }
            $(".layui-field-box").html(tempHtml);
        };
        infoFunc();
    });


function btnSerache() {
    var productPinYin = $("#smartProductPinYin").val();
    var productName = $("#smartProductName").val();
    var smartProductDetail = $("#smartProductDetaiName").val();

    var realData = {};
    realData.PinYin = productPinYin;
    realData.Name = productName;
    realData.CategoryId = smartProductDetail;

    var url = "/SmartProduct/SmartProductInfoGetAll";

    var paraObj = new Object();
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj);

    var interText = doT.template($("#smartProduct_template").text());
    var tempHtml = $(interText(data.Data));
    if (parent.window.productId != undefined) {
        $.each(parent.window.productId, function (i, item) {
            tempHtml.find("[deptinfoid=" + item + "]").remove();
        });
    }
    $(".layui-field-box").html(tempHtml);
}