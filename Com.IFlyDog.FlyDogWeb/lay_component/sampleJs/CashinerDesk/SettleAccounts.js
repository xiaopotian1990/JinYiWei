$(function () {
    layui.use(["form","element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(),
             laydate = layui.laydate(),
             layer = layui.layer,
             laypage = layui.laypage;
             window.form = layui.form();  
    }); 
    getCashier(); 
    //保存收银
    $(".settle-btn").click(function() {
       layer.confirm("是否保存收银？", { btn: ["是", "否"] },
       function () {
           var ajaxObj = {
               url: "/CashierDesk/AddSettlement",
               paraObj: {}
           };
           var resu = dataFunc(ajaxObj).ResultType;
           if (resu === 0) {
               layer.msg("保存成功!", { icon: 1 });
               getCashier(); 
           } else {
               layer.msg(resu.Message, { icon: 1 });
           }
       },
       function () {
           layer.msg("已取消!", { icon: 1 });
       });
    });
});
//查询结算
var getCashier = function () {
    var url = "/CashierDesk/SettlementGetCashier";
    var paraObj = { };
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        var interText = doT.template($(".casUserInfo-temp").text());
        $(".casUserInfo").html(interText(result.Data));

        var interText1 = doT.template($(".cashierOfUser-temp").text());
        $(".tb-cashierOfUser").html(interText1(result.Data));
    } else {
        layer.msg(result.Message);
    }

};