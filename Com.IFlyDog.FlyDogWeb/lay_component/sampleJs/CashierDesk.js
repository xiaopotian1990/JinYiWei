$(function () {
    layui.use(["form", "element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(),
            laydate = layui.laydate,
            layer = layui.layer;

        window.laypage = layui.laypage;
        window.form = layui.form();
        form.render();
        //结算记录
        $(".layui-elem-quote span:eq(0)").click(function () {
            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd(
            {
                title: "结算记录",
                href: "/CashierDesk/SettlementRecord", //地址
                icon: "&#xe60a"
            });
        });
        //收银记录
        $(".layui-elem-quote span:eq(1)").click(function () {
            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd(
            {
                title: "收银记录",
                href: "/CashierDesk/CashierRecord", //地址
                icon: "&#xe60a"
            });
        });
        //还欠款
        $(".layui-elem-quote span:eq(2)").click(function () {
            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd(
            {
                title: "还欠款",
                href: "/CashierDesk/RepayDebt", //地址
                icon: "&#xe60a"
            });
        });
        //结算
        $(".layui-elem-quote span:eq(3)").click(function () {
            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd(
            {
                title: "结算",
                href: "/CashierDesk/SettleAccounts", //地址
                icon: "&#xe60a"
            });
        });
        //顾客识别后
        $(".tb-cust-inshow").on("click", ".addCustomerTab", function () {
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
        //顾客识别后-点击还欠款
        $(".tb-cust-inshow").on("click", ".repayMoney-btn", function () {
            var id = $(this).parent().parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd(
            {
                title: "还欠款",
                href: "/CashierDesk/RepayDebt", //地址
                icon: "&#xe60a"
            });
        });
        // 添加按钮
        $("div[name=WaitingCharge]").on("click", '.model-btn', function () {
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
        $(".cashier-desk-ul>li:eq(0)").click();

    });

    getNoPaidOrders();
    $(".layui-tab-content input[name='cuident']").css("width", "45%");
    //结算记录-按钮
    $(".layui-elem-quote span:eq(0)").click(function () {

    });
    //待收费
    $(".cashier-desk-ul>li:eq(0)").click(function () {
        getNoPaidOrders();
    });
    //今日收银列表
    $(".cashier-desk-ul>li:eq(1)").click(function () {
        getCashiToday();
    });
    //今日发票记录-事件
    $(".cashier-desk-ul>li:eq(2)").click(function () {
        getbillToday();
    });
    //顾客识别按钮
    $(".discbut").click(function () {
        var cuidentName = $("input[name='cuident']").val();
        if (cuidentName === "" || cuidentName === null) {
            layer.msg("请输入查询条件!", { icon: 5 });
        } else {

            var url = "/Customer/CustomerIdentifyAsync";
            var paraObj = {};
            paraObj.data = {
                name: cuidentName
            }
            //返回数据
            var result = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($(".cust-inshow-temp").text());
            var html = "<blockquote class='layui-elem-quote'>暂无当前顾客信息</blockquote>";
            //判断返回数据是否为空
            if (result.length <= 0) {
                $(".custo-null").html(html);
                $(".tb-cust-inshow").empty();
            }
            else {
                /*在doT模版输出数据*/
                $(".tb-cust-inshow").html(interText(result));
                $(".custo-null").empty();
            }
        }
    });
    //顾客重置按钮
    $(".resetbut").click(function () {
        $(".layui-tab-content input[name='cuident']").val("");
        $(".tb-cust-inshow").empty();
        $(".custo-null").empty();
    });
    //顾客识别出信息后-开发票按钮
    $(".tb-cust-inshow,.tb-cashierToday").on("click", ".invoice-btn", function () {
        var _this = $(this), model = _this.data("model"),
            action = _this.data("action");
        $("input[name=custid]").val(_this.parent().parent().attr("data-customerid"));
        if (!model) return false;
        if (!window[model]) {
            loadScript("/lay_component/sampleJs/CashinerDesk/" + model + ".js",
                function () {
                    action ? window[model] && window[model].setSubmitUrl(action).useEmptyEntry().openPop() : window[model] && window[model].useEmptyEntry().openPop();
                });
        } else {
            action ? window[model].useEmptyEntry().setSubmitUrl(action).openPop() : window[model].useEmptyEntry().openPop();
        }
        form.render();
    });
    //顾客识别出信息后-还欠款按钮
    $(".tb-cust-inshow").on("click", ".repayMoney-btn", function () {
        $("#CashierComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#CashierComm").load("RefundMoney", "", function () {
            openDynamicParam("还款", "", "", "CashierComm", "70%", "60%");
            $("input[name=ToMoney]").css("border", "none");
        });
        form.render();
    });
    //修改收银
    $(".tb-cashierToday").on("click", ".caToday-deit", function () {
        params.setDataParam("cashierID", $(this).data("id"));
        var dotEle = [{ container: ".Cashier-form", tmp: ".Cashier-form-tmp" }];
        ajaxObj.setUrl("/CashierDesk/GetCashierUpdateInfo").setParaObj(params).setDotEle(dotEle).getData();
        openPopWithOpt({
            url: "",
            popEle: ".editCashier",
            title: "修改收银",
            func: function () { form.render(); },
            area: ["50%", "60%"]
        });
    });
    //删除发票
    $(".tb-billToday").on("click", ".bill-del-btn", function () {
        var billId = $(this).parent().attr("bill-id");
        layer.confirm("确定删除本条数据吗？", { btn: ["确定", "取消"] },
       function () {
           var ajaxObj = {
               url: "/CashierDesk/DeleteBill",
               paraObj: {
                   data: {
                       ID: billId
                   }
               }
           };
           var resu = dataFunc(ajaxObj).ResultType;
           if (resu === 0) {
               layer.msg("删除成功!", { icon: 1 });
               getbillToday();
           } else {
               layer.msg(resu.Message, { icon: 1 });
           }
       }, function () {
           layer.msg("已取消!", { icon: 1 });
       });
    });
    //待收费取消
    $(".tb-noPaidOrder").on("click", ".cas-cancel-btn", function () {
        var orderType = $(this).attr("order-Type");
        var orderId = $(this).parent().parent().attr("data-orderid");
        var customerid = $(this).parent().parent().attr("data-customerid");
        layer.confirm("您确定要删除当前订单吗？", {
            btn: ["确定", "取消"] //按钮
        }, function () {
            if (orderType === "1") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteOrder").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getNoPaidOrders();
                }
            }
            if (orderType === "3") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteDeposit").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getNoPaidOrders();
                }
            }
            if (orderType === "4") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteBackOrder").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getNoPaidOrders();
                }
            }
            if (orderType === "5") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteDepositRebateOrderr").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getNoPaidOrders();
                }
            }
            return false;
        }, function () {
        });
    });


    $(".editCashier").on("click", ".submit-btn", function () {
        var _this = $(this),
            cardList = [];
        $(".pay-method-list").find(".list-row").each(function (i, item) {
            item = $(item);
            cardList.push({
                CardCategoryID: item.find("[name=CardCategoryID]").val(),
                Amount: item.find("[name=Amount]").val()
            });
        });
        params.setDataParams({
            "ID": $("[name=ID]").val(),
            "Cash": $("[name=Cash]").val(),
            "OrderType": $("[name=OrderType]").val(),
            "CardList": cardList
        });
        if (ajaxObj.setUrl("/CashierDesk/CashierUpdate").setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    $(".Cashier-form").on("click", ".add-pay-btn", function () {
        fillData(".pay-method-list", ".card-tmp", [emptyCard], true);
        form.render();
    });
    $(".Cashier-form").on("click", ".rmv-pay-btn", function () {
        $(this).parents(".list-row").remove();
    });
});
var nuhtml = "<blockquote class=\"layui-elem-quote\">暂无数据</blockquote>",
    emptyCard = { CardCategoryID: -1, Amount: 0 };
var cutomerId = parent.layui.tab({ elem: ".admin-nav-card" }).title().split(":")[2];
//今日收银
var getCashiToday = function () {
    var result = ajaxObj.setUrl("/CashierDesk/GetCashierToday").getData();
    if (result.ResultType === 0) {
        if (result.Data.length <= 0) {
            $(".cashierToday-null").html(nuhtml);
            $(".tb-cashierToday").empty();
        } else {
            fillData(".tb-cashierToday", ".cashierToday-temp", result.Data);
            $(".cashierToday-null").html("");
        }
    } else {
        layer.msg(result.Message);
    }
};
//今日发票
var getbillToday = function () {

    var result = ajaxObj.setUrl("/CashierDesk/GetBillToday").getData();
    if (result.ResultType === 0) {

        if (result.Data.length <= 0) {
            $(".billToday-null").html(nuhtml);
            $(".tb-billToday").empty();
        } else {
            fillData(".tb-billToday", ".billToday-temp", result.Data);
        }
    } else {
        layer.msg(result.Message);
    }
};
//待收费
window.setInterval(getNoPaidOrders, 10000);
function getNoPaidOrders() {
    var result = ajaxObj.setUrl("/CashierDesk/GetNoPaidOrders").getData();
    if (result.ResultType === 0) {
        fillData(".tb-noPaidOrder", ".noPaidOrder-temp", result.Data);
    }
};

