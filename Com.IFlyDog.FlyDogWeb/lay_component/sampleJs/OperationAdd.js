$(function () {
    layui.use(["form", "layer","element"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        window.element = layui.element();
            form.render();
          
    });
    getCanOperation(); 
    //输入划扣数量
    $("input[name=operaNum]").keyup(function () {
        var opnum = $(this);
        if (!opnum.val().replace(/[^1-9.]/g, "") || opnum.val() === "") {
            layer.msg("只可输入整数!最小值为1!");
            return false;
        }
        if (opnum.val() > $(this).parent().prev().text()) {
            layer.msg("划扣数量不可大于剩余数量!");
            opnum.val($(this).parent().prev().text()); 
            return false;
        }  
    });  
    //医生选择框
    $(".tb-doctor").on("click", "input[name=seldoctorName]", function () {
       //获取行号
       var  trindex= $(this).parent().parent().parent()[0].rowIndex;
       $("input[name=trIndex]").val(trindex);
        openPop("", ".doctor-pop", "选择医生"); 
        form.render();
    });
    /*选择该用户*/
    $(".user-table").on("click", ".present-user", function () {
        var userid = $(this).attr("value");
        var username = $(this).parent().parent().find("td")[1].innerHTML;
        var hidtrIndex = $("input[name=trIndex]").val();
        var tr = $(".tb-doctor").children().eq(hidtrIndex);

        tr.find("input[name=doctorId]").attr("doctorId", userid);
        tr.find("input[name=seldoctorName]").val(username);
        if (userid !== "0" || username !== "") {
            layer.msg("选择成功!", { icon: 6 });
            closeLayer(this);
        }
    });
    //查询
    $(".search-user").click(function() {
        currentExploitUser();
    });
    //添加医生跟分工
    $(".add-pay-btn").click(function() {
        _this = $(this);
        _tr = _this.parent().parent();
        var tdH = "<td class=\"fl\"><span class=\"layui-btn layui-btn-danger del-pay-btn fr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
        var tdHs = "<tr><td>" + _tr.children().eq(0).html() + "</td><td>" + _tr.children().eq(1).html() + "</td>" + tdH + "</tr>";
        var html = $(".tb-doctor").html(); 
        $(".tb-doctor").append(tdHs);
        form.render();
    });
    //删除
    $(".tb-doctor").on("click", ".del-pay-btn", function() {
        _this = $(this);
        _tr = _this.parent().parent();
        _tr.remove();
    });
    //保存提交按钮
    $(".oper-btn").click(function () {
        if (!verify()) { return false; }
        addOperation();
    }); 
}); 
var cutomerId = parent.layui.tab({ elem: ".admin-nav-card" }).title().split(":")[2];

/*选择人员*/
var currentExploitUser = function () {
    var url = "/SmartUser/SmartUserGet";
    var paraObj = {
        data: {
            Name: $("input[name='addcuuserNmae']").val(),
            DeptId: $(".smartaddcuDept").val(),
            PageSize: 999,
            PageNum: 1
        }
    };
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".user-tmp").text());
    var html = "<blockquote class='layui-elem-quote'>没有该用户信息!请核实!</blockquote>";
    if (result.length <= 0) {
        $(".user-info").html(html);
        $(".user-table").empty();
    } else {
        $(".user-table").html(interText(result));
        $(".user-info").html("");
    }

} 
//添加划扣
var addOperation = function () {
    var chargesList = [], operationerList = [];
    var tr = $(".tb-canOper tr");
    for (var i = 0; i < tr.length; i++) {
        var obj = { ChargeID: "", DetailID: "", Num: "" };
        obj.DetailID = $(tr.eq(i).find("td").eq(0)).text();
        obj.ChargeID = $(tr.eq(i).find("td").eq(1)).text();
        obj.Num = $(tr.eq(i).find("td").eq(10).children("input")).val();
        chargesList.push(obj);
    };
 
    var doctr = $(".tb-doctor").children();
    $.each(doctr, function (i, item) {
        item = $(item);
        var obj = {};
        obj["UserID"] = item.find($("input[name=doctorId]")).attr("doctorId");
        obj["PositionID"] = item.find($(".OperPosition")).val();
        operationerList.push(obj);
    });

    var paraObj = {};
    paraObj.data = {
        CustomerID: cutomerId,
        ChargesList: chargesList,
        Remark: $(".layui-textarea.operRemark").val(),
        OperationerList: operationerList
    };
    var url = "/DeparDesk/AddOperation";
    var result = ajaxProcess(url, paraObj);
    if (result) {
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1, time: 1000 });
            getCanOperation();
            $(".pay-way input").val("");
            $(".pay-way").next().empty("");
            parent.layui.tab({ elem: ".admin-nav-card" }).tabDelete({ id: "pay" });
        } else {
            layer.msg(result.Message, { icon: 5 });
        }
    };
    return false;
};
//查询可划扣项目
var getCanOperation = function () {
    var url = "/DeparDesk/GetCanOperation";
    var paraObj = { data: { "customerId": cutomerId } };
    var result = ajaxProcess(url, paraObj);

    if (result.ResultType === 0) {
        var interText = doT.template($(".canOper-temp").text());
        $(".tb-canOper").html(interText(result.Data));
    } else {
        layer.msg(result.Message);
    }
};
//判断
var verify = function () {
    var operPos = $(".OperPosition").val();
    var docName = $(".tb-doctor tr").children("td").find("input[name=seldoctorName]").val();
    if (docName === "") {
        layer.msg("请选择医生！", { icon: 2 });
        return false;
    }
    if (operPos === "-1") {
        layer.msg("请选择分工！", { icon: 2 });
        return false;
    }
    
    return true;
}

