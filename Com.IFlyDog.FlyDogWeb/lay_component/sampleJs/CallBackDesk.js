$(function () {
 
    /*设置选择用户input只为可读*/
    $("input[name='remindUserName']").attr("readonly", true);
    layui.use(["form", "layer", "laydate", "element","laypage"], function () {
            var laydate = layui.laydate,
                element = layui.element(),
            layer = layui.layer;
            window.laypage = layui.laypage;
            window.form = layui.form();
            $(".callback-tabtbody").on("click", ".addCustomerTab", function () { 
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
            /*是否开启增加下次回访*/
            form.on("switch(cabkswitch)", function (ele) {
                if (ele.elem.checked === false) {
                        $("[name='callbackcheIsNext']").val(0);
                        $(".addnextcallBack").css("display", "none");
                } else
                {
                        $("[name='callbackcheIsNext']").val(1);
                        $(".addnextcallBack").css("display", "block");
                 }
                    form.render();
            });

            var start = {
                min: "1900-01-01 00:00:00",
                max: "2099-06-16 23:59:59",
                istoday: false,
                choose: function(datas) {
                    end.min = datas; //开始日选好后，重置结束日的最小日期
                    end.start = datas; //将结束日的初始值设定为开始日
                }
            };
            var end = {
                min: "1900-01-01 00:00:00",
                max: "2099-06-16 23:59:59",
                istoday: false,
                choose: function(datas) {
                    start.max = datas; //结束日选好后，重置开始日的最大日期
                }
            };
            $("#beginDate").click(function() {
                start.elem = this;
                laydate(start);
            });
            $("#endDate").click(function() {
                end.elem = this;
                laydate(end);
            });
            getPageData();

    });
    /*主页-选择顾客input事件*/
    $(".layui-form-item").on("click","input[name='remindUserName']",function() {
            openPop("", ".data-remindUser-pop", "选择用户");
    });

    /*主页-选择回访人员-查询按钮*/
    $(".data-remindUser-pop").on("click", ".index-reUserSeaBtn", function ()
    {
            currentExploitUser();
    });

    /*主页-选择回访人员-选中用户*/
    $(".data-remindUser-pop").on("click",".present-user",function() {
            var userid = $(this).attr("value");
            var username = $(this).parent().parent().find("td")[1].innerHTML;
            $("input[name='remindUserName']").attr("value", userid);
            $("input[name='remindUserName']").val(username);
            if (userid !== "0" || username !== "") {
                layer.msg("提醒用户选择成功!", { icon: 6 });
                closeLayer(this);
            }
            form.render();
        });

    /*编辑信息按钮事件*/
    $(".layui-field-box").on("click",".callBackEditbtn",function() {
        openPop("", ".data-claaBackedit-pop", "修改回访信息");
        var callId = $(this).attr("edit-id");
        var cuId = $(this).attr("customerID");
        $("input[name='edithideTId']").attr("hidcustomerID", cuId);
        $("input[name='edithideTId']").attr("hidedit-id", callId);
    });
 
    /*修改回访-查询详细-按钮事件*/
    $(".layui-field-box").on("click", ".callBackEditbtn", function () {
        var callId = $(this).attr("edit-id");
        var url = "/CallbackDesk/GetDetailByCallbackId";
        var paraObj = { data: { callbackId: callId } };
        var result = ajaxProcess(url, paraObj).Data;
        console.log(result);
        $(".smartCallbackeditTool").find("[value=" + result.Tool + "]").attr("selected", true);
       
        $("input[name='editCategoryName']").val(result.CategoryName === null ? "无" : result.CategoryName).attr("disabled", true).css("border", "none");
        form.render();
    });
   
    /*修改回访-提交修改-按钮事件*/
    $(".data-claaBackedit-pop").on("click", ".editsub", function () {
      
        var realData = {};
        realData.ID = $("input[name='edithideTId']").attr("hidedit-id");
        realData.CustomerID = $("input[name='edithideTId']").attr("hidcustomerID");
        realData.Tool = $(".smartCallbackeditTool").val();
        realData.Content = $(".editcallbkContent").val();
        var paraObj = {};
        paraObj.data = realData;

        var url = "/CallbackDesk/UpdateCallback";
        var data = ajaxProcess(url, paraObj);
        if (data) {
            if (parseInt(data.ResultType) === 0) { //请求成功返回
                layer.msg("修改成功!", { icon: 1 });
                getPageData();
                closeLayer(this);
            } else {
                //请求成功返回,但是后台出现错误
                layer.msg(data.Message, { icon: 5 });
            }
        }
        return false;
    });

    /*添加回访按钮事件*/
    $(".layui-field-box").on("click", ".callBackbtn", function () {
        openPop("", ".data-callBack-pop", "回访信息");
        $("[name='callbackcheIsNext']").attr("value", 0);
        $(".layui-tab").css("overflow", "visible");
        $(".hfqkdiv table").css("width", "158%");
        $(".hfqkdiv").css("overflow", "auto");
        $(".xfqkdiv table").css("width", "200%");
        $(".xfqkdiv").css("overflow", "auto");
        $(".hkqkdiv table").css("width", "240%");
        $(".hkqkdiv").css("overflow", "auto");
        /*右上角关闭事件*/
        $(".layui-layer-ico.layui-layer-close.layui-layer-close1").click(function () {
            $(".tbody-zxqk-callB").empty();
            $(".tbody-hfqk-callB").empty();
            $(".tbody-xfqk-callB").empty();
            $(".tbody-hkqk-callB").empty();
        });
        /*当前回访ID*/
        var callbaId = $(this).attr("data-id");
 
        var url = "/CallbackDesk/GetDetailByCallbackId";
        var paraObj = { data: { callbackId: callbaId } };
        var result = ajaxProcess(url, paraObj).Data;
        $(".layui-tab-content input[name='callBaCustomerName']").val(result.CustomerName).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBaAge']").val(result.Age).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBaGender']").val(result.Gender).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBaMobile']").val(result.Mobile).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBaMobileBackup']").val(result.MobileBackup).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBChannelName']").val(result.ChannelName).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBaCategoryName']").val(result.CategoryName === null ? "无" : result.CategoryName).attr("disabled", true).css("border", "none");
        $(".layui-tab-content input[name='callBackID']").attr("callBackID", result.ID);
        $(".layui-tab-content input[name='callBackID']").attr("customerID", result.CustomerID);
  
    });

    /*添加回访按钮页面中-回访情况确认按钮*/
    $(".layui-tab-content").on("click", ".add-callbk-submit-btn", function () {
        if (!verify()) { return false; };
        var realData = {};
        realData.ID = $("input[name='callBackID']").attr("callBackID");
        realData.CustomerID = $(".layui-tab-content input[name='callBackID']").attr("customerID");
        realData.Tool = $(".smartCallbackTool").val();
        realData.Content = $(".callbremark").val();
        realData.IsNext = $("[name='callbackcheIsNext']").val();
        realData.NextUserID = $("input[name='callbaUserName']").attr("value");
        realData.NextCategoryID = $(".smartaddCallback").val();
        realData.NextTaskTime = $("[name='addCallbackNextTaskTime']").val();
        realData.NextName = $("[name='callbaNextName']").val();
        var paraObj = {};
        paraObj.data = realData;

        var url = "/CallbackDesk/CallbackAddByDesk";
        var data = ajaxProcess(url, paraObj);
        if (data) {
            if (parseInt(data.ResultType) === 0) { //请求成功返回
                layer.msg("添加成功!", { icon: 6 });
                            getPageData();
                            closeLayer(this);
            } else {
                //请求成功返回,但是后台出现错误
                layer.msg(data.Message, { icon: 5 });
            }
        }
        return false;
    });

    /*添加回访中-选择用户*/
    $(".layui-tab-content").on("click", ".callbaUserNamecl", function () {
        openPop("", ".data-callBackUser-pop", "选择用户");

    });

    /*添加回访中-选中回访人员*/
    $(".data-callBackUser-pop").on("click", "#addback-present-user", function () {
       
        var id = $(this).attr("value");
        var name = $(this).parent().parent().find("td")[1].innerHTML;
       
        $("input[name='callbaUserName']").attr("value", id);
        $("input[name='callbaUserName']").val(name);
        if (name !== "0" || name !== "") {
            layer.msg("回访人员选择成功!", { icon: 6 });
            closeLayer(this);
        }
        form.render();

    });

    /*添加回访中-查询按钮*/
    $(".data-callBackUser-pop").on("click", "#clBkUserSeaBtn", function () {
        addcallbacurrentExploitUser();
    });
   
    /*主页面搜索按钮*/
    $("#subtmValue").click(function() {
        getPageData();
    });

    /*咨询情况*/
    $(".data-callBack-pop").on("click", ".call-zxqk-cl", function () {
        getConsultInfo();
    });
    /*回访情况*/
    $(".data-callBack-pop").on("click", ".call-hfqk-cl", function () {
        getCallbackCustomerinfo();
    });
    /*未成交*/
    $(".data-callBack-pop").on("click", ".call-wcjqk-cl", function () {
        getFailtureCustomer();
    });
});

/*分页变量*/
var pageNum = 1, pageSize = 15, pageTotals;
/*顾客ID*/

/*回访-查询-*/
var getPageData = function() {
    //请求地址
    var url = "/CallbackDesk/GetCallbackDeskPages/";
    var paraObj = {};
    var thisdate = new Date();
    paraObj.data = {
        StartTime: $("#beginDate").val() === "" ? thisdate.toLocaleDateString() : $("#beginDate").val(),
        EndTime: $("#endDate").val() === "" ? thisdate.toLocaleDateString() : $("#endDate").val(),
        CategoryID: $(".smartCallback").val() === "" ? "-1" : $(".smartCallback").val(),
        CustomerID: $("input[name='cmId']").val() === "" ? 0 : $("input[name='cmId']").val(),
        Name: $("input[name='cmplan']").val() === "" ? "" : $("input[name='cmplan']").val(),
        Status: $("select[name='status']").val(),
        UserID: $("input[name='remindUserName']").attr("value"),
        PageNum: pageNum,
        PageSize: pageSize
    };
    //返回数据
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".callback-temp").text());
    $("#beginDate").attr("value", thisdate.toLocaleDateString());
    $("#endDate").attr("value", thisdate.toLocaleDateString()); 
    $(".callback-tabtbody").html(interText(result.PageDatas));
   
    /*分页总数量*/
    pageTotals = result.PageTotals;
    /*调用分页*/
    pageFun();
};
/*layui分页方法*/
var pageFun = function() {
    layui.use("laypage",
        function() {
            var laypage = layui.laypage;
            var pageCount = Math.ceil(pageTotals / pageSize);
            laypage({
                cont: $("#pageDiv"),
                pages: pageCount, //总页数
                curr: pageNum || 1,
                jump: function(obj, first) {
                    if (!first) {
                        pageNum = obj.curr;
                        getPageData();
                        //$(".role-table").find("td:first").empty().text(i++);
                    }
                }
            });
        });
};
/*选择用户-*/
var currentExploitUser = function() {
    var url = "/SmartUser/SmartUserGet";
    var paraObj = {
        data: {
            Name: $("#userNmae").val(),
            DeptId: $("#smartDept").val(),
            PageSize: 999,
            PageNum: 1
        }
    };
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".user-tmp").text());
    var html = "<blockquote class='layui-elem-quote'>" + "没有该用户信息!请核实!" + "</blockquote>";
    if (result.length <= 0) {
        $(".callba-index-user-info").html(html);
        $(".callba-index-user-table").empty();
    } else {
        $(".callba-index-user-table").html(interText(result));
        $(".callba-index-user-info").html("");
    }

};
/*添加回访中-选择用户-*/
var addcallbacurrentExploitUser = function () {
    var url = "/SmartUser/SmartUserGet";
    var paraObj = {
        data: {
            Name: $(".data-callBackUser-pop input[name='clBkUserNmae']").val(),
            DeptId: $(".data-callBackUser-pop select").val(),
            PageSize: 999,
            PageNum: 1
        }
    };
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".user-tmp").text());
    var html = "<blockquote class='layui-elem-quote'>" + "没有该用户信息!请核实!" + "</blockquote>";
    if (result.length <= 0) {
        $(".addcallba-index-user-info").html(html);
        $(".addcallba-index-user-table").empty();
    } else {
        $(".addcallba-index-user-table").html(interText(result));
        $(".addcallba-index-user-info").html("");
    }
    form.render();

};
//验证提交表单
var verify = function() {
    var cunameCallbackTool = $(".smartCallbackTool").val();
    var contentremark = $(".callbremark").val();
    if (Common.StrUtils.isNullOrEmpty(contentremark) || contentremark.length > 50) {
        layer.msg("回访描述不可为空且不可超过50字符！", { icon: 5 });
        return false;
    }
    if (cunameCallbackTool === "-1") {
        layer.msg("请选择回访方式！", { icon: 5 });
        return false;
    }
    return true;
};
/*咨询情况*/
var getConsultInfo = function () {
    var custmId = $("input[name='callBackID']").attr("customerID");
    var url = "/Consult/GetConsult";
    var paraObj = { data: { customerId: custmId } };
    var result = ajaxProcess(url, paraObj).Data;

    if (result.length > 0) {
        var interText = doT.template($(".callbk-zxqk-temp").text());
        $(".tbody-zxqk-callB").html(interText(result));
    } else {
        $(".zxqk-hint-info").html("<blockquote class='layui-elem-quote'>暂无数据</blockquote>");
    }
};
/*回访情况*/
var getCallbackCustomerinfo = function () {
    var custmId = $("input[name='callBackID']").attr("customerID");
    var url = "/CallbackDesk/GetCallbackByCustomerId";
    var paraObj = { data: { customerId: custmId } };
    var result = ajaxProcess(url, paraObj).Data;
   
    if (result.length > 0) {
            var interText = doT.template($(".callbk-hfqk-temp").text());
        $(".tbody-hfqk-callB").html(interText(result));
    } else {
       $(".hfqk-hint-info").html("<blockquote class='layui-elem-quote'>暂无数据</blockquote>");
    }
};
/*未成交*/
var getFailtureCustomer = function() {
    var custmId = $("input[name='callBackID']").attr("customerID");

    var url = "/Failture/GetFailtureByCustomerId";
    var paraObj = { data: { customerId: custmId } };
    var result = ajaxProcess(url, paraObj).Data;
  
    if (result.length > 0) {
            var interText = doT.template($(".callbk-wcjqk-temp").text());
            $(".tbody-wcjqk-callB").html(interText(result));
    } else {
            $(".wcjqk-hint-info").html("<blockquote class='layui-elem-quote'>暂无数据</blockquote>");
    }
};

 
