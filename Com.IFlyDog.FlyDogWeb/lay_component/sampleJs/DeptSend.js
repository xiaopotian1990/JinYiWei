$(function () {
    layui.use(["form", "element", "layer"], function () {
        var layer = layui.layer,
            element = layui.element();
            window.form = layui.form();
            $(".tb-DeptSend,.tb-DeptSendToday").on("click", ".addCustomerTab", function () {
                var id = $(this).parent().prev().text();
                parent.layui.tab({
                    elem: ".admin-nav-card"
                }).tabAdd(
                {
                    title: "客户档案:" + $(this).text() + "编号:" + id,
                    href: "/Customer/CustomerProfile", //地址
                    icon: "fa-user"
                });
            });

    });
    getGetDeptSend();
    /*发货*/ 
    $(".tb-DeptSend").on("click", ".sen-btn", function () { 
        var sendId = $(this).parent().parent().attr("send-Id");
        console.log(sendId);
        layer.confirm("确认要发货吗",
            { btn: ["确定", "取消"] },
            function () {
                var paraObj = { data: { ID: sendId } };
                var url = "/DeptSend/Send";
                var result = ajaxProcess(url, paraObj);
                if (result) {
                    if (result.ResultType === 0) {
                        layer.msg("操作成功！", { icon: 1, time: 2000 });
                        getGetDeptSend();
                    } else {
                        layer.msg(result.Message, { icon: 5 });
                    }
                };
                return false;
            },
            function () {
                
            });
    });
  
    $(".DeptSend").click(function () { getGetDeptSend(); });
    $(".DeptSendToday").click(function () { getGetDeptSendToday(); });
}); 
/*待发货*/
var getGetDeptSend = function () {
    var result = ajaxObj.setUrl("/DeptSend/GetDeptSend").setParaObj(params).getData();
    fillData($(".tb-DeptSend"), $(".DeptSend-temp"), result.Data); 
};
/*今日发货记录*/
var getGetDeptSendToday = function () {
    var result = ajaxObj.setUrl("/DeptSend/GetDeptSendToday").setParaObj(params).getData();
    fillData($(".tb-DeptSendToday"), $(".DeptSendToday-temp"), result.Data);
};
