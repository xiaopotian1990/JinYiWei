$(function () {
    //@ sourceURL=AddOrder.js
    var chargeIDsArrs = [], chargeSetIDsArrs = [];
    var chargeIDPriceArrs = {},//项目ID：{"Price":1,"Num":1,"FinalPrice":1} 单价 数量
        chargeSetIDPriceObjs = {};//套餐ID：{"Price":1,"Num":1,"FinalPrice":1} 单价 数量
    var orderOprationType = "";

    var getChargeBySelf = function () {
        var ajaxObj = {
            url: "/Charge/ChargeGetData",
            paraObj: {
                data: {
                    PinYin: $("[name=smartProductPinYin]").val(),
                    Name: $("[name=smartProductNmae]").val(),
                    CategoryID: $(".smartChargeCategory").val() === "" ? "-1" : $(".smartChargeCategory").val()
                }
            },
            dotEle: [
                {
                    container: ".addorder-charge-table",
                    tmp: ".addorder-charge-tmp"
                }
            ]
        };
        dataFunc(ajaxObj);
    }
    layui.use(["form", "layer", "laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        //加载项目
        getChargeBySelf();
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
    //加载订单信息
    function showOrderInfos(dtype, orderId, hicutomerid) {
        
        var ajaxObj = {
            url: "/Order/GetOrderDetail",
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
        if (result.ResultType == 0 && result.Data != null) {
            $("#Remark").val(result.Data.Remark);
            $("#priceInfo").find("span[name=amount]").html(result.Data.TotalPrice);
            $("#priceInfo").find("span[name=finalPrice]").html(result.Data.FinalPrice);
            $("#priceInfo").find("span[name=discount]").html(result.Data.Discount);
            //构造订单包含项目列表
            fillData(".tb-Charge", ".seeorder-charge-tmp", result.Data.ChargeDetials);
            //构造订单包含套餐列表
            fillData(".tb-chargeSet", ".seeorder-chargeSet-tmp", result.Data.SetDetials);
            //保存按钮置灰
            $("#saveOrder").addClass("layui-btn-disabled");
            $("#saveOrder").off("click");
        }
    }
    function modifyOrder(result) {
        if (result.ResultType == 0 && result.Data != null) {
            $("#OrderID").val(result.Data.OrderID);
            $("#Remark").val(result.Data.Remark);
            $("#priceInfo").find("span[name=amount]").html(result.Data.TotalPrice);
            $("#priceInfo").find("span[name=finalPrice]").html(result.Data.FinalPrice);
            $("#priceInfo").find("span[name=discount]").html(result.Data.Discount);
            //构造订单包含项目列表
            var ChargeDetials = result.Data.ChargeDetials;
            $.each(ChargeDetials, function (index, item) {
                chargeIDsArrs.push(item.ChargeID);
                var obj = {};
                obj["Price"] = parseFloat(item.Price);
                obj["Num"] = parseInt(item.Num);
                obj["FinalPrice"] = parseFloat(item.FinalPrice);
                chargeIDPriceArrs[item.ChargeID] = obj;
            });
            fillData(".tb-Charge", ".modifyorder-charge-tmp", ChargeDetials);

            //构造订单包含套餐列表
            var SetDetials = result.Data.SetDetials;
            $.each(SetDetials, function (index, item) {
                chargeSetIDsArrs.push(item.SetID);
                var obj = {};
                obj["Price"] = parseFloat(item.Price);
                obj["Num"] = parseInt(item.SetNum);
                obj["FinalPrice"] = parseFloat(item.FinalPrice);
                chargeSetIDPriceObjs[item.SetID] = obj;
            });
            fillData(".tb-chargeSet", ".modifyorder-chargeSet-tmp", result.Data.SetDetials);
        }
    }


    $(".tb-Charge tr>td:eq(2)").addClass("pointer");
    $(".tb-Charge tr>td:eq(3)").addClass("pointer");
    //项目-查询按钮
    $(".search-btn").click(function () {
        getChargeBySelf();
    });
    //套餐数量
    $(".tc-search-btn").click(function () {
        getChargeBySelfetData();
    });
    
    //添加常规项目
    $("div[name=add-charge-div]").on("click", "span[name=add-charge]", function () {
        var $span = $(this);
        var $tr = $span.parent().parent();
        var $tds = $tr.children();
        var chargeId = $tr.attr("data-id");
        if ($.inArray(chargeId, chargeIDsArrs) < 0) {// + 
            chargeIDsArrs.push(chargeId);
            var obj = {};
            obj["Price"] = parseFloat($tds[3].innerHTML);
            obj["Num"] = 1;
            obj["FinalPrice"] = parseFloat($tds[3].innerHTML);
            chargeIDPriceArrs[chargeId] = obj;
            var html = "<tr data-id=\"" + chargeId + "\"><td>"
                + $tds[0].innerHTML + "</td><td>"
                + $tds[3].innerHTML + "</td><td>"
                + "<input name=\"Num\" type=\"number\" class=\"layui-input\" min=\"1\" value=\"1\" style=\"width: 47px;\"/></td><td>"
                + "<input name=\"FinalPrice\" type=\"number\" class=\"layui-input\" min=\"0\" value=\"" + $tds[3].innerHTML + "\" style=\"width: 75px;\"/></td><td>"
                + "100</td><td>"
                + "<span class=\"layui-btn layui-btn-mini layui-btn-danger\" name=\"delCharge\">删除</span></td>";
            
            $("div[name=chargeTable] .tb-Charge").append(html);
            makeTotalPriceInfo();
        }
    });
    //从订单中删除已添加项目
    $("div[name=chargeTable]").on("click", "span[name=delCharge]", function () {
        var $span = $(this);
        var $tr = $span.parent().parent();
        var chargeId = $tr.attr("data-id");
        var index = $.inArray(chargeId, chargeIDsArrs);
        chargeIDsArrs.splice(index, 1);
        
        delete chargeIDPriceArrs[chargeId];
        $tr.remove();
        makeTotalPriceInfo();
    });
    //订单修改项目数量
    $("div[name=chargeTable]").on("change", "input[name=Num]", function () {
        $input = $(this);
        if ($input.val() <= 0) {
            layer.msg("数量不能小于1");
            $input.val(1);
        }
        var num = $input.val();
        var $td = $input.parent();
        var $tr = $td.parent();
        var chargeId = $tr.attr("data-id");
        var obj = chargeIDPriceArrs[chargeId];
        var finalPrice = parseInt(num) * obj["Price"];
        obj["Num"] = parseInt(num);
        obj["FinalPrice"] = finalPrice;
        //修改金额
        $pre = $td.prev();
        $pre.html(finalPrice);
        $next = $td.next();
        $($next.children()[0]).val(finalPrice);
        makeTotalPriceInfo();
    });

    //订单修改项目成交金额
    $("div[name=chargeTable]").on("change", "input[name=FinalPrice]", function () {
        $input = $(this);
        if ($input.val() < 0) {
            layer.msg("金额不能小于0");
            $input.val(0);
        }
        var finalPrice = $input.val();
        var $td = $input.parent();
        var $tr = $td.parent();
        var chargeId = $tr.attr("data-id");
        var obj = chargeIDPriceArrs[chargeId];
        obj["FinalPrice"] = parseFloat(finalPrice);
        //计算折扣
        var fp = finalPrice * 100;
        var totalP = obj['Price'] * obj['Num'];
        var discount = parseInt(fp.toFixed(2) / totalP.toFixed(2));
        $td.next().html(discount);
        makeTotalPriceInfo();
    });

    //添加套餐
    $("div[name=add-chargeSet-div]").on("click", "span[name=add-chargeSet]", function () {
        var $span = $(this);
        var $td = $span.parent();
        var rows = $td.attr("rowspan");//行合并数量
        var $tr = $td.parent();
        var $tds = $tr.children();//
        var rowNum = $tr.index();//tr索引
        var chargeSetId = $tr.attr("data-id");
        if ($.inArray(chargeSetId, chargeSetIDsArrs) < 0) {
            chargeSetIDsArrs.push(chargeSetId);

            var obj = {};
            obj["Price"] = parseFloat($tds[1].innerHTML);
            obj["Num"] = 1;
            obj["FinalPrice"] = parseFloat($tds[1].innerHTML);
            chargeSetIDPriceObjs[chargeSetId] = obj;
            var html = "<tr data-id=\"" + chargeSetId + "\"><td rowspan=\"" + rows + "\">" + $tds[0].innerHTML + "</td>"
                + "<td rowspan=\"" + rows + "\">" + $tds[1].innerHTML + "</td>"
                + "<td rowspan=\"" + rows + "\"><input name=\"Num\" type=\"number\" class=\"layui-input\" min=\"1\" value=\"1\" style=\"width: 49px;\"/></td>"
                + "<td chargeid=" + $($tds[2]).attr("chargeid") + ">" + $tds[2].innerHTML + "</td>"
                + "<td>" + $tds[3].innerHTML + "</td>"
                + "<td>" + $tds[4].innerHTML + "</td>"
                + "<td rowspan=\"" + rows + "\"><span class=\"layui-btn layui-btn-mini layui-btn-danger\" name=\"delChargeSet\">删除</span></td></tr >";
            if (rows > 1){
                for (var i = 1; i < rows; i++) {
                    var $chargeTds = $($tr.parent().children()[rowNum + i]).children();
                    html += "<tr><td chargeid=\"" + $($chargeTds[0]).attr("chargeid") + "\">" + $chargeTds[0].innerHTML + "</td>"
                        + "<td>" + $chargeTds[1].innerHTML + "</td>"
                        + "<td>" + $chargeTds[2].innerHTML + "</td></tr>";
                }
            }
            $("div[name=chargeSetTable] .tb-chargeSet").append(html);
            makeTotalPriceInfo();
        }
    });
    //从订单中删除套餐
    $("div[name=chargeSetTable]").on("click", "span[name=delChargeSet]", function () {
        var $span = $(this);
        var $td = $span.parent();
        var rows = $td.attr("rowspan");//行合并数量
        var $tr = $td.parent();
        var rowNum = $tr.index();//tr索引
        var chargeSetId = $tr.attr("data-id");
        var index = $.inArray(chargeSetId, chargeSetIDsArrs);
        chargeSetIDsArrs.splice(index, 1);
        delete chargeSetIDPriceObjs[chargeSetId];
        if (rows > 1){
            for (var i = 1; i < rows; i++) {
                $($tr.parent().children()[rowNum + i]).remove();
            }
        }
        $tr.remove();
        makeTotalPriceInfo();
    });
    //订单修改套餐数量
    $("div[name=chargeSetTable]").on("change", "input[name=Num]", function () {
        $input = $(this);
        if ($input.val() <= 0){
            layer.msg("数量不能小于1");
            $input.val(1);
        }
        var num = $input.val();
        var $td = $input.parent();
        var $tr = $td.parent();
        var SetID = $tr.attr("data-id");
        var obj = chargeSetIDPriceObjs[SetID];
        var finalPrice = parseInt(num) * obj["Price"];
        obj["Num"] = parseInt(num);
        obj["FinalPrice"] = finalPrice;
        //修改金额
        $pre = $td.prev();
        $pre.html(finalPrice);
        makeTotalPriceInfo();
    });

    $("#saveOrder").click(function () {
        var hicutomerid = $("input[name=hicutomerId]").attr("hicutomerid");
        var obj = {}, url = "/Order/AddOrder";
        obj['InpatientID'] = 0;//新添加订单为0，否则为住院单ID
        if (orderOprationType == "modify"){
            url = "/Order/UpdateOrder";
            obj['OrderID'] = $("#OrderID").val();
        }
        if (orderOprationType == "Inpatient") {
            var oid = $("#OrderID").val();
            if (oid != null && oid.trim() != ''){
                url = "/Order/UpdateOrder";
            }
            obj['OrderID'] = $("#OrderID").val();
            obj['InpatientID'] = $("input[name=opration]").attr("data-InpatientID");
        }
        obj['CustomerID'] = hicutomerid;
        obj['Remark'] = $("#Remark").val();
        var details = [];
        $.each(chargeIDPriceArrs, function (key, value) {
            var charge = {};
            charge['ChargeID'] = key;
            charge['Num'] = value['Num'];
            charge['FinalPrice'] = value['FinalPrice'];
            details.push(charge);
        });
        $.each(chargeSetIDPriceObjs, function (key, value) {
            var charge = {};
            charge['SetID'] = key;
            charge['Num'] = value['Num'];
            charge['FinalPrice'] = value['FinalPrice'];
            details.push(charge);
        });
        obj['Details'] = details;
        params.setDataParams(obj);
        var result = ajaxObj.setParaObj(params).setUrl(url).getData();
        if (result) {
            if (result.ResultType == 0) {
                layer.msg(result.Message, { icon: 6 });
                closeLayer(this);
                if (orderOprationType == "Inpatient"){
                    getInpatientData();
                }
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        }
    });
    //计算总体价格信息
    function makeTotalPriceInfo() {
        var amount = 0, finalPrice = 0, discount = 0;
        $.each(chargeIDPriceArrs, function (key, value) {
            amount += value['Price'] * value['Num'];
            finalPrice += value['FinalPrice'];
        });
        $.each(chargeSetIDPriceObjs, function (key, value) {
            amount += value['Price'] * value['Num'];
            finalPrice += value['FinalPrice'];
        });
        $("#priceInfo").find("span[name=amount]").html(amount);
        $("#priceInfo").find("span[name=finalPrice]").html(finalPrice);
        $("#priceInfo").find("span[name=discount]").html(parseInt(((finalPrice * 100) / amount).toFixed(2)));
    }

    // 分页相关变量 

    var $doc = $("iframe").contents();
    //查询套餐
    var getChargeBySelfetData = function () {
        var ajaxObj = {
            url: "/Order/ChargeSetGet",
            paraObj: {
                data: {
                    Name: $("[name=addTcProductNmae]").val(),
                    PinYin: $("[name=addTcPinYinMa]").val()
                }
            },
            dotEle: [
                {
                    container: ".addorder-chargeSet",
                    tmp: ".addorder-chargeSet-tmp"
                }
            ]
        };
        dataFunc(ajaxObj);
    }
    var getPageData = getChargeBySelfetData;
    
});