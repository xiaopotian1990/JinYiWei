$(function () {
    //@ sourceURL=AddDeposit.js
    var orderOprationType = "";
    layui.use(["form", "layer", "laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        //获取预收款项目
        getDepositCharges();
        //判断是否加载订单信息
        var dtype = $("#opration").attr("data-type");
        if (dtype != null) {
            if (dtype.trim() != "") {
                orderOprationType = dtype;
                var orderId = $("#opration").attr("data-orderId");
                var hicutomerid = $("input[name=hicutomerId]").attr("hicutomerid");
                showOrderInfos(dtype, orderId, hicutomerid);
            }
        } else {
            orderOprationType = $("input[name=opration]").attr("data-type");
            var orderId = $("input[name=opration]").attr("data-orderId");
            if (orderId != null && orderId.trim() != "") {
                var hicutomerid = $("input[name=hicutomerId]").attr("hicutomerid");
                showOrderInfos('modify', orderId, hicutomerid);
            }

        }
    });
    //加载信息
    function showOrderInfos(dtype, orderId, hicutomerid) {

        var ajaxObj = {
            url: "/Deposit/GetDepositDetail",
            paraObj: {
                data: {
                    customerId: hicutomerid,
                    orderId: orderId
                }
            },
            dataCallBack: dtype == 'modify' ? modifyOrder : seeOrder
        };
        dataFunc(ajaxObj);
    }
    function seeOrder(result) {
        if (result.ResultType === 0 && result.Data != null) {
            $("#Remark").val(result.Data.Remark);
            $("#priceInfo").find("span[name=amount]").html(result.Data.TotalPrice);
            $("#priceInfo").find("span[name=finalPrice]").html(result.Data.FinalPrice);
            $("#priceInfo").find("span[name=discount]").html(result.Data.Discount);
            //构造订单包含项目列表
            fillData(".order-deposit-charge-table", ".see-Deposit-temp", result.Data.Details); 
            //保存按钮置灰
            $("#saveOrder").addClass("layui-btn-disabled");
            $("#saveOrder").off("click");
        }
    }

    //获取预收款项目
    function getDepositCharges() {
        var ajaxObj = {
            url: "/Deposit/GetAllDeposit",
            paraObj: {
                data: {}
            },
            dotEle: [
                {
                    container: ".deposit-charge-table",
                    tmp: ".deposit-charge-tmp"
                }
            ]
        };
        dataFunc(ajaxObj);
    }
    var depositChargeIDArrs = [], depositChargePriceObj = {};
    //添加预收款项目
    $("#depositChargesDiv").on("click", "span[name=add-deposit-charge]", function () {
        $span = $(this);
        $td = $span.parent();
        $tr = $td.parent();
        $tds = $tr.children();//
        var id = $tr.attr("data-id");
        if ($.inArray(id, depositChargeIDArrs) < 0){
            depositChargeIDArrs.push(id);
            var obj = {};
            obj['Price'] = parseFloat($tds[1].innerHTML);
            obj['Num'] = 1;
            obj['FinalPrice'] = parseFloat($tds[1].innerHTML);
            obj['CouponAmount'] = parseFloat($($tds[3]).attr("couponamount"));
            depositChargePriceObj[id] = obj;
            
            var html = "<tr data-id=\"" + id + "\"><td>" + $tds[0].innerHTML + "</td>"
                + "<td>" + $tds[1].innerHTML + "</td>"
                + "<td><input name=\"Num\" type=\"number\" class=\"layui-input\" min=\"1\" value=\"1\" style=\"width: 47px;\"/></td>"
                + "<td>" + $tds[1].innerHTML + "</td>"
                + "<td><span class=\"layui-btn layui-btn-mini  layui-btn-danger\" name=\"delDepositCharge\">删除</span></td>";

            $(".order-deposit-charge-table").append(html);
            makeTotalPriceInfo();
        }
    });

    //从订单中删除已添加项目
    $("#orderDepositChargeDiv").on("click", "span[name=delDepositCharge]", function () {
        var $span = $(this);
        var $tr = $span.parent().parent();
        var id = $tr.attr("data-id");
        var index = $.inArray(id, depositChargeIDArrs);
        depositChargeIDArrs.splice(index, 1);
        delete depositChargePriceObj[id];
        $tr.remove();
        makeTotalPriceInfo();
    });

    //订单修改项目数量
    $("#orderDepositChargeDiv").on("keyup", "input[name=Num]", function () {
        $input = $(this);
        if ($input.val() <= 0) {
            layer.msg("数量不能小于1");
            $input.val(1);
        }
        var num = $input.val();
        var $td = $input.parent();
        var $tr = $td.parent();
        var id = $tr.attr("data-id");
        var obj = depositChargePriceObj[id];
        var finalPrice = parseInt(num) * obj["Price"];
        obj["Num"] = parseInt(num);
        obj["FinalPrice"] = finalPrice;
        //修改金额
        $next = $td.next();
        $next.html(finalPrice);
        makeTotalPriceInfo();
    });

    $("#saveDepositOrder").click(function () {
        var hicutomerid = $("input[name=hicutomerId]").attr("hicutomerid");
        var obj = {};
        obj['CustomerID'] = hicutomerid;
        obj['Remark'] = $("#Remark").val();
        var details = [];
        $.each(depositChargePriceObj, function (key, value) {
            var charge = {};
            charge['ChargeID'] = key;
            charge['Num'] = value['Num'];
            details.push(charge);
        });
        obj['Details'] = details;
        params.setDataParams(obj);
        var result = ajaxObj.setParaObj(params).setUrl("/Deposit/DepositAdd").getData();
        if (result) {
            if (result.ResultType == 0) {
                layer.msg(result.Message, { icon: 6 });
                closeLayer(this);
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        }
    });

    function makeTotalPriceInfo() {
        var couponAmount = 0, finalPrice = 0
        $.each(depositChargePriceObj, function (key, value) {
            couponAmount += value['CouponAmount'] * value['Num'];
            finalPrice += value['FinalPrice'];
        });
        $("#fAmount").html(finalPrice);
        $("#couponAmount").html(couponAmount);
    }
});