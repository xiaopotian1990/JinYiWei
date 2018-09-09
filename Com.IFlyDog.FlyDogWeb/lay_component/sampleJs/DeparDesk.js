$(function () {
    layui.use(["form", "element", "layer", "laydate"], function () {
        var element = layui.element(), laydate = layui.laydate(), layer = layui.layer;
        window.form = layui.form();
        //列表-跳转客户页面
        $(".tb-HosVisToday,.tb-DeVisToday,.tb-OperToday,.tbody-cust-inshow").on("click", ".addCustomerTab", function () {

            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd({
                title: "客户档案:" + $(this).text() + "编号:" + id,
                href: "/Customer/CustomerProfile", //地址
                icon: "fa-user"
            });
        });
        //列表添加划扣
        $(".tb-HosVisToday,.tb-DeVisToday,.tb-OperToday,.tbody-cust-inshow").on("click", ".add-peration-btn", function () {
            var id = $(this).parent().parent().children("td:eq(0)").text();
            parent.layui.tab({
                elem: ".admin-nav-card"
            }).tabAdd({
                title: "添加划扣:" + "客户编号:" + id,
                href: "/DeparDesk/OperationAdd", //地址
                icon: "fa-user",
                id: "pay"
            });
        });
    });
    getHosDeToday("", "GetHospitalVisitTodayAsync");

    $(".layui-tab-content input[name='cuident']").css("width", "45%");
    //客户识别
    $(".cu-discbut").click(function () {
        var cuidentName = $("input[name='cuident']").val();
        if (cuidentName === "" || cuidentName === null) { layer.msg("请输入查询条件!", { icon: 5 }); }
        else {
            var url = "/Customer/CustomerIdentifyAsync";
            var paraObj = {};
            paraObj.data = { name: cuidentName };
            //返回数据
            var result = ajaxProcess(url, paraObj);
            var interText = doT.template($(".cust-inshow-temp").text());
            var html = "<blockquote class='layui-elem-quote'>暂无当前顾客信息</blockquote>";
            if (result.ResultType === 0) {
                //判断返回数据是否为空
                if (result.Data.length <= 0) {
                    $(".add-cu-hint").html(html);
                    $(".tbody-cust-inshow").empty();
                }
                else {
                    /*在doT模版输出数据*/
                    $(".tbody-cust-inshow").html(interText(result.Data));
                    $(".add-cu-hint").empty();
                }
            } else {
                layer.msg(result.Message);
            }
        }
    });
    //重置
    $(".cu-resetbut").click(function () {
        $(".layui-tab-content input[name='cuident']").val("");
        $(".tbody-cust-inshow").empty();
        $(".add-cu-hint").empty();
    });
    //今日医院上门
    $(".hosVisToday").click(function () {
        getHosDeToday("", "GetHospitalVisitTodayAsync");
    });
    //医院查询
    $(".hos-search-btn").click(function () {
        var cuName = $("input[name=cuHosName]").val();
        getHosDeToday(cuName, "GetHospitalVisitTodayAsync");
    });
    //今日科室顾客
    $(".deVisToday").click(function () {
        var cuName = $("input[name=cuDeName]").val();
        getHosDeToday(cuName, "GetDeptVisitTodayAsync");
    });
    //科室查询
    $(".de-search-btn").click(function () {
        var cuName = $("input[name=cuDeName]").val();
        getHosDeToday(cuName, "GetDeptVisitTodayAsync");
    });
    /*顾客分诊-按钮>弹出页面*/
    $(".tbody-cust-inshow").on("click", ".online-cu-triage-but", function () {
        openPop("", ".data-triage-pop", "顾客分诊");
        var custid = $(this).attr("tr-customerID");
        var custName = $(this).parent().parent().find("td>a").text();
        $("[name='triagecuId'] span").text(custid).css("color", "#FF5722");
        $("[name='triagecuName'] span").text(custName).css("color", "#FF5722");
        var url = "/FrontDesk/GetCustomerInfoBefaultTriageAsync";
        var paraObj = { data: { customerId: custid } };
        var result = ajaxProcess(url, paraObj);

        $("input[name='fz-zxpro-inp']").val(result.Data.Symptom === null ? "暂无" : result.Data.Symptom + ",").attr("disabled", true).css("border", "none");;
        $("input[name='fz-gszxUser-inp']").val(result.Data.ManagerUserName === null ? "暂无" : result.Data.ManagerUserName).attr("disabled", true).css("border", "none");;
        $(".SelectFZUserByHospital").find("[value=" + result.Data.ManagerUserID + "]").attr("selected", "selected");
        form.render();
    });
    $(".data-triage-pop").on("click", ".addfz-zlss-sub", function () {
        if ($(".SelectCategoryByDeptByFz").val() === "-1") {
            layer.msg("请选择部门!", { icon: 5 });
            return false;
        }
        var url = "/FrontDesk/AddTriageAsync";
        var paraObj = {
            data: {
                SelectID: $(".SelectCategoryByDeptByFz").val(),
                CustomerID: $("[name='triagecuId'] span").text(),
                Remark: "",
                Type: 1
            }
        };
        var result = ajaxProcess(url, paraObj);
        if (result.ResultType === 0) {
            layer.msg(result.Message, { icon: 1 });
            closeLayer(this);
        } else {
            layer.msg(result.Message, { icon: 5 });
            closeLayer(this);
        }

    });
    //今日执行记录
    $(".operToday").click(function () {
        getOperToday();
    });
    //今日执行记录-操作-删除
    $(".tb-OperToday").on("click", ".oper-del-btn", function () {
        var operId = $(this).parent().attr("operId");
        layer.confirm("确定删除本条数据吗？", { btn: ["确定", "取消"] },
         function () {
             var ajaxObj = {
                 url: "/DeparDesk/DeleteOperation",
                 paraObj: {
                     data: {
                         ID: operId
                     }
                 }
             };
             var resu = dataFunc(ajaxObj).ResultType;
             if (resu === 0) {
                 layer.msg("删除成功!", { icon: 1 });
                 getOperToday();
             } else {
                 layer.msg(resu.Message, { icon: 1 });
             }
         },
         function () {
             layer.msg("已取消!", { icon: 1 });
         });
    });
    $(".operDetail-form").on("click", ".add-default", function () {
        var _this = $(this),
            _tr = _this.parents("tr"),
            num = _tr.find("[name=num]").val(),
            WarehouseID = _tr.find("[name=WarehouseID]").val(),
            operId = _this.data("operid");

        if (WarehouseID == -1) {
            layer.msg("请选择仓库！", { icon: 2 });
            return false;
        }
        if (!(num >= _this.data("min") && num <= _this.data("max"))) {
            layer.msg("数量不能小于最小数量，并且不能大于最大数量！", { icon: 2 });
            return false;
        }
        params.setDataParams({
            "OperationID": operId,
            "ProductID": _this.data("id"),
            "WarehouseID": WarehouseID,
            "Num": num
        });
        ajaxObj.setUrl("/DeparDesk/AddProduct").setIsUpdateTrue().setParaObj(params).getData();
        params.setDataParam("operationId", operId);
        var dotEle = [{ container: ".operDetail-form", tmp: ".operDetail-form-tmp" }];
        ajaxObj.setUrl("/DeparDesk/GetDefaultChargeInfo").setParaObj(params).setDotEle(dotEle).getData();
    });
    $(".operDetail-form").on("click", ".btn-rmv", function () {
        var _this = $(this),
            operProId = _this.data("id"),
            operId = _this.data("operid");
        params.setDataParams({
            "ID": operProId,
            "CreateUserID": 0
        });
        ajaxObj.setUrl("/DeparDesk/DeleteProduct").setIsUpdateTrue().setParaObj(params).getData();
        params.setDataParam("operationId", operId);
        var dotEle = [{ container: ".operDetail-form", tmp: ".operDetail-form-tmp" }];
        ajaxObj.setUrl("/DeparDesk/GetDefaultChargeInfo").setParaObj(params).setDotEle(dotEle).getData();
    });
    //添加耗材
    $(".tb-OperToday").on("click", ".oper-pro-btn", function () {
        var operId = $(this).parent().attr("operId");
        params.setDataParam("operationId", operId);
        var dotEle = [{ container: ".operDetail-form", tmp: ".operDetail-form-tmp" }];
        ajaxObj.setUrl("/DeparDesk/GetDefaultChargeInfo").setParaObj(params).setDotEle(dotEle).getData();
        var opt = {};
        opt.title = "耗材操作";
        opt.url = "";
        opt.popEle = ".addOpeProduct";
        opt.func = function () { };
        opt.area = ["80%", "70%"];
        openPopWithOpt(opt);
    });
    //编辑
    $(".tb-OperToday").on("click", ".oper-edit-btn", function () {
        var operId = $(this).parent().attr("operId");
        openPop("", ".edit-depar-pop", "编辑划扣");
        $(".layui-fontColor-6").css("border", "none");

        var ajaxObj = {
            url: "/DeparDesk/GetOperationDetail",
            paraObj: {
                data: {
                    id: operId
                }
            }
        };
        var result = dataFunc(ajaxObj);
        if (result.ResultType === 0) {
            $("input[name=cuName]").val(result.Data.CustomerName);
            $("input[name=cuChargeName]").val(result.Data.ChargeName);
            $("input[name=cuId]").val(result.Data.CustomerID);
            $("input[name=cuOperId]").val(result.Data.OperationID);
            $("input[name=cuNum]").val(result.Data.Num);

            if (result.Data.OperatorList.length > 0) {
                var tdHs;
                $.each(result.Data.OperatorList, function (i, item) {
                    var tdH = "<td style=\"border: none\" ><span class=\"layui-btn layui-btn-danger fr deltr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
                    tdHs += "<tr>" + "<td><input type=\"hidden\" name=\"editdocId\" editdocId=\"" + item.UserID + "\" /><input class=\"layui-input pointer\" name=\"thseldoc\" value=\"" + item.UserName + "\" readonly=\"readonly\"/></td>" + "<td><select name=\"editPosition\" class=\"sel-position\">" + "<option value=\"" + item.PositionID + "\">" + item.Name + "</option></select></td>" + tdH + "</tr>";
                });
                $(".tb-editDoc").html(tdHs);
                getPosition();
                $.each(result.Data.OperatorList, function (i, item) {
                    $(".sel-position").eq(i).val(item.PositionID);
                });
                form.render();
            }
        }
    });
    //删除当前行
    $(".tb-editDoc").on("click", ".deltr", function () {
        _this = $(this);
        _tr = _this.parent().parent();
        _tr.remove();
    });
    //添加医生跟分工
    $(".add-pay-btn").click(function () {

        _this = $(this);
        _tr = _this.parent().parent();
        var tdH = "<td style=\"border: none\" ><span class=\"layui-btn layui-btn-danger fr deltr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
        var tdHs = "<tr>" + "<td><input type=\"hidden\" name=\"editdocId\" editdocId=\"\" /><input class=\"layui-input pointer\" name=\"thseldoc\" readonly=\"readonly\"/></td>" +
                        "<td><select name=\"editPosition\" class=\"sel-position\">" + "<option>请选择</option></select></td>" + tdH + "</tr>";
        $(".tb-editDoc").append(tdHs);
        getPosition();
        form.render();
    });
    //选择医生
    $(".tb-editDoc").on("click", "input[name=thseldoc]", function () {
        //获取行号
        var trindex = $(this).parent().parent()[0].rowIndex - 1;
        $("input[name=edittrIndex]").val(trindex);
        openPop("", ".doctor-pop", "选择医生");
        form.render();
    });
    //选择用户
    $(".user-table").on("click", ".present-user", function () {
        var userid = $(this).attr("value");
        var username = $(this).parent().parent().find("td")[1].innerHTML;
        var hidtrIndex = $("input[name=edittrIndex]").val();
        var tr = $(".tb-editDoc").children().eq(hidtrIndex);
        tr.find("input[name=editdocId]").attr("editdocid", userid);
        tr.find("input[name=thseldoc]").val(username);
        if (userid !== "0" || username !== "") {
            layer.msg("选择成功!", { icon: 6 });
            closeLayer(this);
        }
    });
    //查询
    $(".search-user").click(function () {
        currentExploitUser();
    });
    //编辑保存
    $(".edit-btn").click(function () {
        var operationerList = [];
        var doctr = $(".tb-editDoc").children();
        $.each(doctr, function (i, item) {
            item = $(item);
            var obj = {};
            obj["UserID"] = item.find($("input[name=editdocId]")).attr("editdocid");
            obj["PositionID"] = item.find($(".sel-position")).val();
            operationerList.push(obj);
        });

        var paraObj = {};
        paraObj.data = {
            ID: $("input[name=cuOperId]").val(),
            CustomerID: $("input[name=cuId]").val(),
            OperationerList: operationerList
        };
        var url = "/DeparDesk/UpdateOperation";
        var result = ajaxProcess(url, paraObj);
        if (result) {
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1, time: 1000 });
                closeLayer(this);
                getOperToday();
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
});
//今日医院-科室上门
var getHosDeToday = function (cuName, conurl) {
    var url = "/DeparDesk/" + conurl;
    var paraObj = { data: { Name: cuName } };
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        if (conurl === "GetHospitalVisitTodayAsync") {
            var interText = doT.template($(".HosVisToday-temp").text());
            $(".tb-HosVisToday").html(interText(result.Data));
        } else {
            var interText1 = doT.template($(".DeVisToday-temp").text());
            $(".tb-tb-DeVisToday").html(interText1(result.Data));
        }
    } else {
        layer.msg(result.Message);
    }
};
//查询执行记录
function getOperToday() {
    var result = ajaxObj.setUrl("/DeparDesk/GetOperationToday").setParaObj(params).getData();
    fillData($(".tb-OperToday"), $(".OperToday-temp"), result.Data);
}
//查询岗位 
var getPosition = function () {
    var url = "/Position/PositionGet";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj);

    var interText = doT.template($(".position-tmp").text());
    $(".sel-position").html(interText(result.Data));
};
//选择用户
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

