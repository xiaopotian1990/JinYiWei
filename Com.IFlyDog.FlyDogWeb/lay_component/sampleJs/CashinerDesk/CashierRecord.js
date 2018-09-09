$(function () {
    layui.use(["form", "layer", "laypage"], function () {
        var layer = layui.layer;
            window.laypage = layui.laypage;
            window.form = layui.form();
            form.render();
            getPageData();
    });
  
    //查询按钮
    $(".cashi-search-btn").click(function () { 
        getPageData();
    }); 
});
var pageIndex = 1, pageSize = 15, pageTotals;;
var getcashier = function () {
    var ajaxObj = {
        url: "/CashierDesk/GetCashier",
        paraObj: {
            data: {
                CustomerID: $("input[name='caCuID']").val(),
                No: $("input[name='caNo']").val(),
                StartTime: $("input[name='castartTime']").val(),
                EndTime: $("input[name='caendTime']").val(),
                PageNum: pageIndex,
                PageSize: pageSize
            }
        },
        hasPage: true,
        dotEle: [{ container: ".tb-cashier", tmp: ".cashier-temp" }]
    };
    dataFunc(ajaxObj);
};
getPageData = getcashier;