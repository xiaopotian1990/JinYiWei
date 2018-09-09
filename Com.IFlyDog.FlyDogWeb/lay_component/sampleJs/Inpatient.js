$(function () {
    layui.use(["form", "layer", "element"], function () {
        var layer = layui.layer,
            element = layui.element();
        window.form = layui.form();
        getPageData();
        element.on("tab(inpatient)", function (elem) {
            $("[name=name]").val("");
            $(".layui-field-box.hide").hide();
            var action = $(this).data("action");
            action && eval(action+"();");
        });
    });

    // 住院列表tab页
    $(".inpatient-table").on("click", ".btn-out", function () {
        var _this = $(this),
            id = _this.data("id"),
            bedID = _this.data("bedid"),
            confirmEle = "<div>确认出院？</div><div>客户名称：" + _this.data("name") + "</div>";
        layer.confirm(confirmEle, function () {
            params.setDataParams({ ID: id, BedID: bedID });
            ajaxObj.setUrl("/Inpatient/Out").setParaObj(params).setIsUpdateTrue().getData();
        }, function () {
            layer.msg("已取消出院~", { icon: 1 });
        });
    });
    // 点击客户姓名
    $(".inpatient-table").on("click", ".customer-a", function () {
        var id = $(this).data();
        parent.layui.tab({
            elem: ".admin-nav-card"
        }).tabAdd(
        {
            title: "客户档案:" + $(this).text() + "编号:" + id,
            href: "/Customer/CustomerProfile", //地址
            icon: "fa-user"
        });
    });

    // 办理住院列表tab页
    $(".search-btn").click(function () {
        if (getInpatientInData().ResultType == 0) {
            $(this).parent().siblings(".layui-field-box").show();
        }
    });
    // 办理住院按钮
    $(".inpatient-live-table").on("click", ".btn-in", function () {
        var _this = $(this),
            cusName = _this.data("name"),
            cusId = _this.data("id");
        $("customer").text(cusName + " - " + cusId);
        var dotEle = [{ container: ".bed-select", tmp: ".bed-select-tmp" }];
        ajaxObj.setUrl("/Bed/GetSelect").setDotEle(dotEle).getData();
        openPop("", ".inpatient-live-pop", "办理住院");
    });
    // 办理住院提交
    $(".inpatient-in.btn-submit").click(function () {
        params.setDataParams({
            CustomerID: $("customer").text().split(" - ")[1],
            BedID: $("[name=BedID]").val(),
            Remark: $("[name=Remark]").val()
        });
        if (ajaxObj.setUrl("/Inpatient/In").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            $(".layui-field-box.hide").hide();
            closeLayer(this);
        }
    });
    //住院单维护
    $(".inpatient-table").on("click", ".btn-order", function () {
        $span = $(this);
        var InpatientID = $span.attr("data-id");
        var CustomerID = $span.attr("data-customerid");
        var OrderID = $span.attr("data-orderid");
        $("input[name=hicutomerId]").attr("hicutomerId", CustomerID);
        $("input[name=opration]").attr("data-type", "Inpatient");
        $("input[name=opration]").attr("data-orderId", OrderID);
        $("input[name=opration]").attr("data-InpatientID", InpatientID);

        $("#inPerationComm").html("");
        $("#inPerationComm").load("/Customer/AddOrder", "", function () {
            openDynamicParam("住院单维护", "/Customer/AddOrder", "", "inPerationComm", "90%", "90%");
            form.render();
        });
    });

});
var getInpatientData = function () {
    var dotEle = [{ container: ".inpatient-table", tmp: ".inpatient-tmp" }];
    ajaxObj.setUrl("/Inpatient/GetIn").setDotEle(dotEle).getData();
}
var getBedData = function () {
    var dotEle = [{ container: ".inpatient-bed-table", tmp: ".inpatient-bed-tmp" }];
    ajaxObj.setUrl("/Bed/Get").setDotEle(dotEle).getData();
}
var getInpatientInData = function () {
    params.setDataParam("name", $("[name=name]").val());
    var dotEle = [{ container: ".inpatient-live-table", tmp: ".inpatient-live-tmp" }];
    return ajaxObj.setUrl("/Customer/CustomerIdentifyAsync").setParaObj(params).setDotEle(dotEle).getData();
}
getPageData = getInpatientData;