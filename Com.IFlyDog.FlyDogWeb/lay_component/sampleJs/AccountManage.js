$(function() {
layui.use(["form", "element", "layer"], function () {
    var element = layui.element(), layer = layui.layer;window.form = layui.form();  
    form.render();
    form.on("checkbox(coupon)", function (data) {
        var _this = $(data.elem);
        _this.parents("table").find("[type=checkbox]").not(_this).prop("checked", false);
        form.render();
    });
    $(".tb-money").on("click",".commissionOut",function () {
        openPop("", ".commissionOut-pop", "佣金提现");
    });
    $(".commissionOut.submit-btn").click(function () {
        params.setDataParams({
            CustomerID: $(".tb-money").find("td").eq(4).attr("customerId"),
            Amount: $("[name=Amount]").val(),
            Remark: $("[name=Remark]").val()
        });
        if (ajaxObj.setUrl("/PromoterCommission/CommissionOut").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
}); 
//查询按钮
$(".search-btn").click(function() {
    getcustomerMoney();
    $(".tb-money tr td:eq(4)").attr("customerId", $("input[name=accCustId]").val());
});
var flag,conType;
//扣减积分
$(".tb-money").on("click", ".sub-point", function () {
    $(".DeductPoint-group").show();
    flag = 1;
   
    $(".PointToCoupon-group").hide();
    $(".AddPoint-group").hide();
    getPointInfo(this, "DeductPoint");
});
//增加积分
$(".tb-money").on("click", ".add-point-btn", function () {
    $(".AddPoint-group").show();
    flag = 2; 
    $(".PointToCoupon-group").hide();
    $(".DeductPoint-group").hide();
    getPointInfo(this, "AddPoint");
});
//积分兑换券
$(".tb-money").on("click", ".point-to-coupon", function () {
    $(".PointToCoupon-group").show();
    $(".DeductPoint-group").hide();
    $(".AddPoint-group").hide();
    getPointInfo(this, "PointToCoupon");
});
// 选择劵类型input点击事件
$(document).on("click", "[name=ToCouponCategoryName]", function () {
    conType = 1;
    getCouponData();
    openPop("", ".coupon-pop", "选择劵类型");  
});

//重置按钮
$(".reset-btn").click(function() {
    $("input[name=accCustId]").val(""); 
    $(".tb-money").html("");
    $(".acc-info tbody tr").html("");
});
//(积分-提交 
$(".btn-submit").on("click", function () {
    if (!verify()) {
        return false;
    }
    var Points, Remarks;
    if (flag === 1) {
        Points = $("input[name=DeductionCoupon]").val();
        Remarks = $("textarea[name=DeductionRemark]").val();
    }
    if (flag === 2) {
        Points = $("input[name=AddPoint]").val();
        Remarks = $("textarea[name=AddPointRemark]").val();
    }
    var paraObj = {};
    paraObj.data = {
        CustomerID: $(".CustomerID").val(),
        Point: Points === "" ? "" : Points,
        PointAmount: $("input[name=ToPoint]").val(),
        CouponCategory: $("input[name=TocouponCategoryId]").val(),
        CouponAmount: $("input[name=ToCoupon]").val(),
        Remark: Remarks === "" ? "" : Remarks
    };
    var url = "/Point/" + $(this).parents(".layui-layer").data("url");
    var result = ajaxProcess(url, paraObj);
    if (result) {
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1, time: 1000 });
            getcustomerMoney();
            closeLayer(this);

        } else {
            layer.msg(result.Message, { icon: 5 });
        }
    };
    return false; 
});
//增加券
$(".tb-money").on("click", ".add-coupon-btn", function () {
    openPop("", ".SendCoupon-group", "赠送券"); 
    $(".SendCoupCuID").val($("input[name=accCustId]").val()); 
});
// 选择劵类型input点击事件
$(document).on("click", "[name=CouponCategoryName]", function () {
    conType = 2;
    getCouponData();
    openPop("", ".coupon-pop", "选择劵类型"); 
});
//增加券*提交
$(".SendCoupon-group").on("click", ".SendCoupon-btn", function () {
    if (!verify()) { return false; }
    var paraObj = {};
    paraObj.data = {
        CustomerID:$("input[name=accCustId]").val(),
        CouponAmount: $("input[name='SendCoupon']").val(),
        CouponID: $("input[name=CouponCategoryId]").val(),
        Remark: $("[name='SendCouponRemark']").val()
    };
    var url = "/Coupon/SendCoupon" ;
    var result = ajaxProcess(url, paraObj);
    if (result) {
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1, time: 1000 });
            getcustomerMoney();
            closeLayer(this);

        } else {
            layer.msg(result.Message, { icon: 5 });
        }
    };
    return false; 
});
//扣减券
$(".tb-money").on("click", ".sub-coupon", function () {
    openPop("", ".DeductCoupon-group", "扣减券");
    $(".DeductCouponID").val($("input[name=accCustId]").val());
    getNoDoneCoupon($("input[name=accCustId]").val());
   
});
//扣减券-提交
$(".DeductCoupon-group").on("click", ".DeductCoupon-btn", function () {
    if (!deductverify()) { return false; }
    var paraObj = {};
    paraObj.data = {
        CustomerID: $("input[name=accCustId]").val(),
        CouponID: $("input[name=DeductCouponID]").val(),
        CouponAmount: $("input[name='DeductCoupon']").val(),
        Remark: $("[name='DeductCouponRemark']").val()
    };
    var url = "/Coupon/DeductCoupon";
    var result = ajaxProcess(url, paraObj);
    if (result) {
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1, time: 1000 });
            getcustomerMoney();
            closeLayer(this);

        } else {
            layer.msg(result.Message, { icon: 5 });
        }
    };
    return false;
}); 
//选择券类型-确认
$(".coupon.submit-btn").click(function () {
    if (conType === 1) {
        $(".coupon-Category-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("input[name=TocouponCategoryId]").val(item.val());
            $("input[name=ToCouponCategoryName]").val(item.data("name"));
        });
    }
    if (conType ===2) {
        $(".coupon-Category-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            $("input[name=CouponCategoryId]").val(item.val());
            $("input[name=CouponCategoryName]").val(item.data("name"));
        });
    }
    getcustomerMoney();
    closeLayer(this);
});
});

//查询剩余券
var getNoDoneCoupon = function (custid) {
    var url = "/Coupon/GetNoDoneCoupon";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj).Data;
    //顾客信息-输出
    var interText = doT.template($(".NoDoneCoupon-tmp").text());
    $(".tb-NoDoneCoupon").html(interText(result));
}
var verify = function () { 
    if ($("input[name=accCustId]").val() ==="") {
        layer.msg("请输入顾客编号,进行查询！", { icon: 2 });
        return false;
    }
    return true;
}
var deductverify = function() {
    if ($("input[name=DeductCouponID]").val() === "") {
        layer.msg("请输入券编号！上方可复制！", { icon: 2 });
        return false;
    }
    if ($("input[name=DeductCoupon]").val() === "") {
        layer.msg("请输入券数量！", { icon: 2 });
        return false;
    }
    if ($("[name='DeductCouponRemark']").val() === "") {
        layer.msg("请输入描述！", { icon: 2 });
        return false;
    }
    return true;
};
//查询账户
var getcustomerMoney = function () {
    if (!verify()) { return false; }
    var url = "/CustomerProfile/GetMoney";
    var paraObj = { data: { customerId: $("input[name=accCustId]").val() } };
    var result = ajaxProcess(url, paraObj); 
    if (result.ResultType === 0) {
        if (result.Data === null) {
            layer.msg("暂无顾客信息");
            return false;
        } else { 
            if (result.Data !== null) {
                var interText = doT.template($(".money-temp").text());
                $(".tb-money").html(interText(result.Data)); 
            }
            if (result.Data.Deposits !== null) {
                var interText1 = doT.template($(".money-deposit-temp").text());
                $(".tb-money-deposit").html(interText1(result.Data.Deposits));
            }
            if (result.Data.Coupons !== null) {
                var interText2 = doT.template($(".money-coupon-temp").text());
                $(".tb-money-coupon").html(interText2(result.Data.Coupons));
            }
            if (result.Data.OverDueCoupons !== null) {
                var interText3 = doT.template($(".money-overcoupon-temp").text());
                $(".tb-money-overcoupon").html(interText3(result.Data.OverDueCoupons));
            }
            if (result.Data.CouponChanges !== null) {
                var interText4 = doT.template($(".money-coupchan-temp").text());
                $(".tb-money-coupchan").html(interText4(result.Data.CouponChanges));
            }
            if (result.Data.PointChanges !== null) {
                var interText5 = doT.template($(".money-poinchan-temp").text());
                $(".tb-money-poichan").html(interText5(result.Data.PointChanges));
            }
            if (result.Data.DepositChanges !== null) {
                var interText6 = doT.template($(".money-depochan-temp").text());
                $(".tb-money-depochan").html(interText6(result.Data.DepositChanges));
            }
            if (result.Data.CommissionChanges !== null) {
            var interText7 = doT.template($(".money-commichan-temp").text());
            $(".tb-money-commichange").html(interText7(result.Data.CommissionChanges));
        }
        }
    } else {
        layer.msg(result.Message);
        $("input[name=accCustId]").val("");
        $(".tb-money").html("");
        $(".acc-info tbody tr").html("");
    }
};
//积分查询
var getPointInfo = function (obj, url) {
    var dotEle = [{ container: ".memberDesk-info", tmp: ".memberDesk-info-tmp" }];
    params.setDataParam("customerID", $("input[name=accCustId]").val());
    ajaxObj.setUrl("/Point/GetPointInfo").setDotEle(dotEle).setParaObj(params).getData();
    openPop(url, ".memberDesk-pop", url === "DeductPoint" ? "手动增减积分" : "积分兑换券");
} 
//查询券类型
var getCouponData = function () {
    var ajaxObj = {
        url: "/CouponCategory/CouponCategoryInfoGet",
        paraObj: {},
        dotEle: [
            {
                container: ".coupon-Category-table",
                tmp: ".coupon-Category-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
