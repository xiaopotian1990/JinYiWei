$(function() {
    layui.use(["form", "layer"],
        function() {
            var layer = layui.layer;
            window.form = layui.form();
            /*医院*/
            form.on("select(hospital)",
                function(data) {
                    gethospitalByDept(null,data);
                    /*更新全部 参数值 可指定刷新form.render("select");select,checkbox,radio*/
                    form.render();
                    //清空选择权限
                    $("#showuserRole_div").empty();
                });
            form.on("select(rolefilter)", function (data) {
                selectUserRole(data);
            });
        });
    gethospitalByDept(0);
    getTableData(1);
    $(".Hospital").each(function (index, element) {
        $(element).attr("lay-filter", "hospital");
    });
    
    /*关闭按钮.调用方法closeEvent当前的btn按钮,把当前form表单中的元素重置*/
    $(".information_close").click(function () {
        layer.close($(this).parents(".layui-layer").data("index"));
        /*关闭弹窗按钮*/
        closeEvent($(this));
    });
    /*添加角色*/
    function selectUserRole(data) {
        
        var seltext = $(data.elem).next().find('.layui-this').text();
        var html = $("#showuserRole_div").html();
        if (html.indexOf(seltext) < 0) {
            var html = " <a value=" +
                data.value +
                " name='roleUserdel' class='layui-btn roleUserdel'>" +
                seltext +
                "<i class='layui-icon red'>&nbsp&#xe640;</i>" +
                "</a>";
            $("#showuserRole_div").append(html);
        }
        
    }

});
/*密码重置*/
$(".site-table").on("click",".passwordResetBtn",function() {
    var passwordRe = $(this).attr("passwordRe");
    var hospitaldataid = $(this).attr("hospitaldataid");
    layer.confirm("确认重置密码？", function () {

        var paraObj = {};
        paraObj.data = {
            UserID: passwordRe,
            HospitalID: hospitaldataid,
            CreateUserID: 1,
            UserHospitalID: 1
        };

        var url = "/User/UserPasswordReset";

        var data = ajaxProcess(url, paraObj);
        if (data) {
            //请求成功返回
            if (parseInt(data.ResultType) === 0) {
                //提示消息
                layer.msg(data.Message + "&nbsp&nbsp默认密码:123456", { icon: 6 });
                //刷新table页面数据
                getTableData(pageNum);
            } else {//请求成功返回,但是后台出现错误
                layer.msg(data.Message, { icon: 5 });
            }
        };
    }, function () {
        layer.msg("您已取消操作~", {icon:1});
    });
});
/*停用数据*/
$(".role-table").on("click",".editStopBut",function() {
        /*当前状态*/
        var infostatus = $(this).attr("status");
        /*当前ID*/
        var infostopId = $(this).attr("stopID");
        /*医院ID*/
        var userhospital = $(this).attr("hospitaldataId");

        if (infostatus === "0") {
            layer.confirm("您确定停用本条数据？",
                { btn: ["确定", "取消"] }, //按钮
                function() {
                    disableFun();
                },
                function() {
                    layer.msg("已经取消此操作", { icon: 6 });
                });
        } else {
            layer.confirm("您确定启用本条数据？",
                { btn: ["确定", "取消"] },
                function() {
                    disableFun();
                },
                function() {
                    layer.msg("已经取消此操作", { icon: 6 });

                });
        }
        /*停用数据方法*/
        var disableFun = function() {
            //定义数组
            var paraObj = {};
            //组合传值
            paraObj.data = {
                UserID: infostopId,
                Status: infostatus,
                HospitalID: userhospital,
                CreateUserID: 1,
                UserHospitalID: 1
            };
            /*接口*/
            var url = "/User/Disable";
            var data = ajaxProcess(url, paraObj);

            if (data) {
                //请求成功返回
                if (parseInt(data.ResultType) === 0) {

                    //提示消息
                    layer.msg(data.Message, { icon: 6 });
                    //刷新table页面数据
                    getTableData(pageNum);
                } else {
                    //请求成功返回,但是后台出现错误
                    layer.msg(data.Message, { icon: 5 });
                }
            }
            return false;
        };
    });
/*查询按钮*/
$(".layui-form").on("click","#rolesearch",function() {
    getTableData(1);
});

var infoEditId, authority_url, _userId, currIndex;
/*添加按钮*/
$(".btn-user-add").click(function(data) {
    authority_url = "/User/UserAdd";
    infoEditId = null;
    /*打开弹窗口*/
    currIndex = layer.open({
        type: 1,
        id:"user-add-layer",
        title: "添加用户信息",
        area: ["40%", "75%"], //宽高
        shade: [0.8, "#B3B3B3", false],
        closeBtn: 1,
        shadeClose: false, //点击遮罩关闭
        content: $(".data-userAdd-pop"),
        end: function (i) {
            layui.layer.close(i);
        },
        success: function(layero, index) {
            layero.data("index", index);
            /*隐藏IDinput*/
            layero.find(".userInfo").hide();
            /*给医院下拉框子添加lay-filter用于监听*/
            layero.find("form")[0].reset();
            /*清空*/
            layero.find("tbody").empty();
            
            gethospitalByDept(1);
            layero.find("select[name=userAddRoleName]").attr("lay-filter", "rolefilter");
            layero.find("#user-add-layer .site-demo-button").empty();
            
            /*删除当前点击的显示按钮元素*/
            $(document).on("click", ".roleUserdel", function() { $(this).remove(); });
            
            $(".layui-layer-setwin .layui-layer-close1").click(function () {

                layero.find("form")[0].reset();
                /*清空*/
                layero.find("tbody").empty();
                gethospitalByDept(0);
            });
        }
    });
    
});
/*提交按钮*/
$("#submit_User").click(function () {
    var hospitalId = $("div[name=userHospital]").find(".layui-this").attr('lay-value');
    if(infoEditAndAddFun(authority_url, infoEditId, hospitalId))closeCurrentWindow(currIndex, 1000);
});

/*添加编辑公用方法*/
var infoEditAndAddFun = function (url, ID, hospitalId) {
    /*获取选中的角色*/
    var datalist = [];
    for (var i = 0; i < $("#showuserRole_div a").length; i++) {
        datalist.push($("#showuserRole_div a:eq(" + i + ")").attr("value"));
    }
    /*赋值*/

    
    if ($("input[name='userInfoAccount']").val() === "" ||
        $("input[name='userInfoAccount']").val().length > 15) {
        layer.msg("帐号不可为空且不可超过15字符!", { icon: 5 });
        return false
    };
    if ($("input[name='userInfoName']").val() === "" || $("input[name='userInfoName']").val().length > 10) {
        layer.msg("姓名不可为空且不可超过10字符!", { icon: 5 });
        return false
    };
    if ($("input[name='userInfoPhone']")
        .val() ===
        "" ||
        $("input[name='userInfoPhone']").val().length > 11) {
        layer.msg("请输入正确手机号码!", { icon: 5 });
        return false
    };
    if ($("input[name='userInfoMobile']").val() === "" ||
        $("input[name='userInfoMobile']").val().length > 11) {
        layer.msg("请输入正确电话号码!", { icon: 5 });
        return false
    };
    if ($(".data-userAdd-pop .Hospital").next().find(".layui-this").attr("lay-value") === "") {
        layer.msg("请选择医院", { icon: 5 });
        return false
    };
    if ($(".data-userAdd-pop select[name='smartAddDeptName']").next().find(".layui-this").attr("lay-value") === "") {
        layer.msg("请选择部门!", { icon: 5 });
        return false
    };
    if (datalist === "") {
        layer.msg("请选择角色!", { icon: 5 });
        return false
    };
    var paraObj = {};
    paraObj.data = {
        Account: $("input[name='userInfoAccount']").val(),
        Name: $("input[name='userInfoName']").val(),
        Gender: $('div[name="userInfosex"]').find('.layui-form-radioed').prev().val(),
        DeptID: $(".data-userAdd-pop select[name='smartAddDeptName']").next().find(".layui-this").attr("lay-value"),
        Remark: $("#userInfoRemark").val(),
        Phone: $("input[name='userInfoPhone']").val(),
        Mobile: $("input[name='userInfoMobile']").val(),
        UserHospitalID: hospitalId,
        Roles: datalist
    };
    if (ID != null){
        paraObj.data.ID = ID;
    }

    var result = ajaxProcess(url, paraObj);
    if (result) {
        if (parseInt(result.ResultType) === 0) { //请求成功返回
            $("#showchannelAddInfo_div").html("");
            //提示消息
            layer.msg(result.Message, { icon: 6, time: 1000 });
            //刷新主页面数据.
            getTableData(pageNum);
            closeCurrentWindow(currIndex, 1000);
        } else {
            layer.msg(result.Message, { icon: 5 });
            return false;
        } //请求成功返回,但是后台出现错误
    }
    return true;
};
/*编辑按钮*/
$(".site-table").on("click",".editInfoBut",function(data) {
        /*获取当前ID*/
        infoEditId = $(this).attr("editThisId");
        authority_url = "/User/UserUpdate";
        currIndex = layer.open({
            type: 1,
            id:'user-edit-layer',
            title: "编辑用户信息",
            area: ["40%"], //宽高
            shade: [0.8, "#B3B3B3", false],
            closeBtn: 1,
            shadeClose: false, //点击遮罩关闭
            content: $(".data-userAdd-pop"),
            success: function (layero1, index1) {
                layero1.data("index", index1);
                layero1.find("form")[0].reset();
                form.render();
                $("#showuserRole_div").empty();
                layero1.find(".userInfo").prop("disabled", true);
                layero1.find(".smartHospital").attr("lay-filter", "hospital");
                layero1.find("select[name=userAddRoleName]").attr("lay-filter", "rolefilter");
                /*更新全部 参数值 可指定刷新form.render("select");select,checkbox,radio*/
                var url = "/User/GetDetail";

                var paraObj = { data: { hospitalId: infoEditId } };

                var result = ajaxProcess(url, paraObj).Data;

                layero1.find("input[name=userInfoID]").val(result.ID);
                layero1.find("input[name=userInfoAccount]").val(result.Account);
                layero1.find("input[name=userInfoName]").val(result.Name);
                if (result.Gender === "1") {
                    $(layero1.find("input[name=userInfosex]")[1]).removeAttr("checked");
                    $(layero1.find("input[name=userInfosex]")[0]).attr("checked", "checked");
                } else {
                    $(layero1.find("input[name=userInfosex]")[0]).removeAttr("checked");
                    $(layero1.find("input[name=userInfosex]")[1]).attr("checked", "checked");
                }
                //layero1.find("input[name=userInfosex]").val(result.Gender === "1" ? "checked='checked'" : "");
                layero1.find("input[name=userInfoPhone]").val(result.Phone);
                layero1.find("input[name=userInfoMobile]").val(result.Mobile);

                
                $(document).on("click", ".roleUserdel", function() { $(this).remove(); });
                /*查询出来的赋值给页面上的下拉框*/

                $(".smartUserAddDeptName select").empty()
                    .append("<option value=" + result.DeptID + ">" + result.DeptName + "</option>");
                form.render();

                $.each(result.Roles,function(i, item) {
                    var html = " <a value=" + item.RoleID + " name='roleUserdel' class='layui-btn roleUserdel'>" + item.RoleName +"<i class='layui-icon red'>&nbsp&#xe640;</i>" +"</a>";
                    $("#showuserRole_div").append(html);
                });
            }
        });
});

var hospitalOpenIndex, deptOpenIndex, _userIndex ;
/*权限-增加医院*/
$(".addAccesshospitalsdetail").on("click", function () {
    hospitalOpenIndex = layer.open({
        type: 1,
        title: "选择医院权限",
        area: ["40%", "50%"], //宽高
        shade: [0.8, "#B3B3B3", false],
        closeBtn: 1,
        
        content: $(".access-authority-detail"),
        success: function (hospitalslayero, indexhospitals) {
            /*siblings() 查找同辈元素*/
            $(".accessHospitals_div").show().siblings().hide();
            var url = "/Hospital/HospitalGet";
            var paraObj = {};
            var result = ajaxProcess(url, paraObj).Data;
            $(".tbody-access-hospitals").empty();
            $.each(result, function (i, item) {
                var html = "<tr><td><input name='hospitalsck' value=" + item.ID + " type='checkbox'></td><td>" + item.Name + "</td></tr>";
                $(".tbody-access-hospitals").append(html);
                form.render();
            });

        }
    });
    $(".layui-layer-ico.layui-layer-close.layui-layer-close1").click(function () {

        $(".tbody-access-hospitals").empty();

    });
    
});
/*确认提交*/
$(".accessHospitals_div").on("click", ".commit_Check", function () {

    var html = "";
    $(".accessHospitals_div tr [name=hospitalsck]:checked").each(function () {
        var ckval = $(this).val();
        var cktext = $(this).parent().next().text();
        var hoshtml = $(".addAccesshospitals_div").html();
        if (hoshtml.indexOf(ckval) < 0) {
            var html = " <a value=" + ckval + " class='layui-btn accHpitlsdel'>" + cktext + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccesshospitals_div").append(html);
        }

    });
    layer.msg("医院信息添加成功!", { icon: 6, time: 1000 });
    closeCurrentWindow(hospitalOpenIndex, 1000);

});


/*删除当前点击的显示按钮元素*/
$(document).on("click", ".accHpitlsdel", function () { $(this).remove(); });
/*权限-增加部门*/
$(".addAccessDeptdetail").on("click", function () {
    deptOpenIndex = layer.open({
        type: 1,
        title: "选择部门权限",
        area: ["40%", "50%"], //宽高
        shade: [0.8, "#B3B3B3", false],
        closeBtn: 1,
        
        content: $(".access-authority-detail"),
        success: function (deptlayero, indexdept) {
            /*siblings() 查找同辈元素*/
            $(".accessdept_div").show().siblings().hide();
            var url = "/Dept/DeptGet";
            var paraObj = { data: {} };
            var result = ajaxProcess(url, paraObj).Data;
            $(".tbody-access-dept").empty();
            $.each(result, function (i, item) {
                var html = "<tr><td><input name='deptsck' value=" + item.ID + " type='checkbox' ></td><td>" + item.Name + "</td></tr>";
                $(".tbody-access-dept").append(html);
                form.render();
            });

        }
    });
    $(".layui-layer-ico.layui-layer-close.layui-layer-close1").click(function () {
        $(".tbody-access-dept").empty();
    });
    
});
/*确认提交*/
$(".accessdept_div").on("click", ".commit_Check", function () {

    var html = "";
    $(".accessdept_div tr [name=deptsck]:checked").each(function () {

        var ckval = $(this).val();
        var cktext = $(this).parent().next().text();
        var depthtml = $(".addAccessDept_div").html();
        if (depthtml.indexOf(ckval) < 0) {
            $(this).prop("disabled", true);
            var html = " <a value=" + ckval + " class='layui-btn accessdeptdel'>" + cktext + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessDept_div").append(html);
        }
        layer.msg("部门信息添加成功!", { icon: 6, time: 1000 });
        closeCurrentWindow(deptOpenIndex, 1000);

    });
});

/*删除当前点击的显示按钮元素*/
$(document).on("click", ".accessdeptdel", function () { $(this).remove(); });
/*权限-增加用户*/
$(".addAccessusersdetail").on("click", function () {
    _userIndex = layer.open({
        type: 1,
        title: "选择用户权限",
        area: ["40%", "50%"], //宽高
        shade: [0.8, "#B3B3B3", false],
        closeBtn: 1,
        
        content: $(".access-authority-detail"),
        success: function (userlayero, indexuser) {
            /*siblings() 查找同辈元素*/
            $(".accessUser_div").show().siblings().hide();
            $(".tbody-access-user").empty();
            /*var url = "/Dept/DeptGet";
            var paraObj = { data: {} };
            var result = ajaxProcess(url, paraObj).Data;

            $.each(result.Users, function (i, item) {
                var html = "<tr><td><input name='userck' value=" + item.UserID + " type='checkbox'></td><td>" + item.UserName + "</td><td>当前医院</td></tr>";
                $(".tbody-access-user").append(html);
                form.render();
            });*/
        }
    });
    $(".layui-layer-ico.layui-layer-close.layui-layer-close1").click(function () {

        $(".tbody-access-user").empty();

    });
});

/*搜索*/
$("#accUsersrbut").click(function () {
    var seltext = $(".accessUser_div #Hospital").find("option:selected").text();
    $(".tbody-access-user").empty();
    var url = "/User/UserGet",

        paraObj = {};
    paraObj.data = {
        HospitalID: $(".accessUser_div #Hospital").val(),
        Name: $(".accessUser_div input[name='searchUserName']").val(),
        Status: 999,
        PageSize: pageSize,
        PageNum: pageNum
    };
    /*返回数据*ajaxProcess*/
    var result = ajaxProcess(url, paraObj).Data;
    console.log(result);
    //, interText = doT.template($("#roleAddtemplate").text());
    $.each(result, function (i, item) {
        var html = "<tr><td><input name='userck' value=" + item.ID + " type='checkbox'></td><td>" + item.Name + "</td><td>" + item.HospitalName + "</td></tr>";
        $(".tbody-access-user").append(html);
        form.render();
    });
    /*在doT模版输出数据*/
    //                    $(".tbody-access-user").append(result);
    // $(".Hospital").find("option[value=" + result.Data.HospitalID + "]").attr("selected", true);
    /*分页总数量*/
    pageTotals = result.PageTotals;
    /*调用分页*/
    pageFun();
});
/*确认提交*/
$(".accessUser_div").on("click", ".commit_Check", function () {

    var html = "";
    $(".accessUser_div tr [name=userck]:checked").each(function () {
        var ckval = $(this).val();
        var cktext = $(this).parent().next().text();
        var userhtml = $(".addAccessusers_div").html();
        if (userhtml.indexOf(ckval) < 0) {
            var html = " <a value=" + ckval + " class='layui-btn accuserdel'>" + cktext + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessusers_div").append(html);
        }
    });
    layer.msg("用户信息添加成功!", { icon: 6, time: 1000 });
    closeCurrentWindow(_userIndex, 1000);
});
/*删除当前点击的显示按钮元素*/
$(document).on("click", ".accuserdel", function () { $(this).remove(); });

/*客户权限*/
$(".site-table").on("click", ".limitsBut", function () {
    authority_url = '/User/SetCustomerPermissionDetail';
    //当前用户ID
    form.render();
    $(".addAccesshospitals_div").empty();
    $(".addAccessDept_div").empty();
    $(".addAccessusers_div").empty();
    _userId = $(this).attr("getlimitsPermissionDetail");
    var url = "/User/GetCustomerPermissionDetail";
    var paraObj = { data: { userId: _userId } };
    var result = ajaxProcess(url, paraObj).Data;
    
    if (result.Hospitals.length > 0) {
        $.each(result.Hospitals, function (i,item) {
            var html = " <a value=" + item.HospitalID + " class='layui-btn accHpitlsdel'>" + item.HospitalName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccesshospitals_div").append(html);
        });
    }
    if (result.Depts.length > 0) {
        $.each(result.Depts, function (i, item) {
            var html = " <a value=" + item.DeptID + " class='layui-btn accessdeptdel'>" + item.DeptName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessDept_div").append(html);
        });
    }
    if (result.Users.length > 0) {
        $.each(result.Users, function (i, item) {
            var html = " <a value=" + item.UserID + " class='layui-btn accuserdel'>" + item.UserName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessusers_div").append(html);
        });
    }
    
        currIndex = layer.open({
            type: 1,
            id: 'limits',
            title: "选择操作权限",
            area: ["60%", "60%"], //宽高
            shade: [0.8, "#B3B3B3", false],
            closeBtn: 1,
            
            content: $(".data-access-authority"),
            success: function (layerolimits, indexlimits) {
                layerolimits.data("index", indexlimits);
            }
        });
});
$('.data-access-authority #submit_inforMation').click(function () {
    
    var HospitalsArr = [], DeptsArr = [], UsersArr = [];
    $.each($(".addAccesshospitals_div a"), function (i, item) {
        HospitalsArr.push($(item).attr('value'));
    });
    $.each($(".addAccessDept_div a"), function (i, item) {
        DeptsArr.push($(item).attr('value'));
    });
    $.each($(".addAccessusers_div a"), function (i, item) {
        UsersArr.push($(item).attr('value'));
    });
    var obj = {
        "UserID": _userId,
        "Hospitals": HospitalsArr,
        "Depts": DeptsArr,
        "Users": UsersArr
    };
    if (HospitalsArr.length > 0 && DeptsArr.length > 0 && UsersArr.length > 0) {
        var paraObj = { data: obj };
        var result = ajaxProcess(authority_url, paraObj);
        if (result.ResultType == 0) {
            layer.msg(result.Message, { icon: 1, time: 1000 });
            closeCurrentWindow(currIndex, 1000);
        } else {
            layer.msg(result.Message, { icon: 5, time: 1000 });
        }
    } else {
        layer.msg("请选择权限后提交！", { icon: 5, time: 1000 });
    }
    
});

/*回访权限*/
$(".site-table").on("click", ".visitBut", function () {
    authority_url = '/User/SetCustomerCallBackPermission';
    //当前用户ID
    form.render();
    $(".addAccesshospitals_div").empty();
    $(".addAccessDept_div").empty();
    $(".addAccessusers_div").empty();
    _userId = $(this).attr("visitItemId");
    var url = "/User/GetCallBackPermissionDetail";
    var paraObj = { data: { userId: _userId } };
    var result = ajaxProcess(url, paraObj).Data;
    
    if (result.Hospitals.length > 0) {
        $.each(result.Hospitals, function (i, item) {
            var html = " <a value=" + item.HospitalID + " class='layui-btn accHpitlsdel'>" + item.HospitalName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccesshospitals_div").append(html);
        });
    }
    if (result.Depts.length > 0) {
        $.each(result.Depts, function (i, item) {
            var html = " <a value=" + item.DeptID + " class='layui-btn accessdeptdel'>" + item.DeptName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessDept_div").append(html);
        });
    }
    if (result.Users.length > 0) {
        $.each(result.Users, function (i, item) {
            var html = " <a value=" + item.UserID + " class='layui-btn accuserdel'>" + item.UserName + "<i class='layui-icon red'>&nbsp&#x1006;</i>" + "</a>";
            $(".addAccessusers_div").append(html);
        });
    }

    currIndex = layer.open({
        type: 1,
        id:'visit',
        title: "选择操作权限",
        area: ["60%", "60%"], //宽高
        shade: [0.8, "#B3B3B3", false],
        closeBtn: 1,
        
        content: $(".data-access-authority"),
        success: function (layerolimits, indexlimits) {
            layerolimits.data("index", indexlimits);
        }
    });
});

/*分页变量*/
var pageNum = 1, pageSize = 15, pageTotals;
/*表格数据显示方法-复用*/
var getTableData = function (curr_pagenum) {
    $(".role-table").empty();
    getHospitalId();
    $(".Hospital").each(function (index, element) {
        $(element).attr("lay-filter", "hospital");
    });

    /*请求地址*/
    var url = "/User/UserGetPages",
        paraObj = {};
    /*提交参数-组装数据*/
    paraObj.data = {
        HospitalID: $("#Hospital").val(),
        DeptID: $("#smartAddDeptName").val(),
        RoleID: $("#userAddRoleName").val(),
        Name: $("input[name='searchUserName']").val(),
        Status: 999,
        PageSize: pageSize,
        PageNum: curr_pagenum
    };
    /*返回数据*ajaxProcess*/
    var result = ajaxProcess(url, paraObj).Data, interText = doT.template($("#roleAddtemplate").text());
    
    $(".Hospital").find("option[value=" + result.PageDatas.HospitalID + "]").attr("selected", true);
    /*分页总数量*/
    pageTotals = result.PageTotals;
    /*调用分页*/
    pageFun();
    /*在doT模版输出数据*/
    $(".role-table").html(interText(result.PageDatas));
};
/*医院查询*/
var getHospitalId = function() {
    var id = $("#Hospital").val();
};
/*layui分页方法*/
var pageFun = function() {
    layui.use("laypage",
        function() {
            var laypage = layui.laypage;
            var pageCount = Math.ceil(pageTotals / pageSize);
            laypage({
                cont: "pageDiv",
                pages: pageCount, //总页数
                curr: pageNum || 1,
                jump: function(obj, first) {
                    if (!first) {
                        pageNum = obj.curr;
                        getTableData(pageNum);
                    } else {
                        pageNum = 1;
                    }
                }
            });
        });
};
/*关闭弹窗按钮方法*/
var closeEvent = function(btn) {
    /*元素重置*/
    btn.parents(".layui-form").find("form")[0].reset();
    /*清空*/
    btn.parents(".layui-form").find("tbody").empty();
    gethospitalByDept(0);
};
/*医院查询部门*/
var gethospitalByDept = function (_index, data) {
    if (typeof (data) == "undefined") {
        data = $(".Hospital")[_index];
    }
    var url = "/Dept/DeptGetByHospitalId";
    var paraObj = { data: { hospitalId: data.value } };
    var result = ajaxProcess(url, paraObj).Data;

    /*清空页面上部门下拉框的值*/
    $(".smartUserAddDeptName").each(function (index, element) {
        $(element).empty().append("<select name='smartAddDeptName'id='smartAddDeptName'><option value=\"0\">请选择</option></select>");
        
    });

    $.each(result,function (i, item) {
        /*查询出来的赋值给页面上的下拉框*/

        $("select[name='smartAddDeptName']").each(function (index, element) {
            $(element).append("<option value=" + item.ID + ">" + item.Name + "</option>");
        });
    });
    gethospitalByRole(_index, data);
};
/*医院查询角色*/
var gethospitalByRole = function(_index, data) {
    if (typeof (data) == "undefined") {
        data = $(".Hospital")[_index];
    }
    var roleurl = "/Role/GetAllRole";
    var roleparaObj = { data: { hospitalId: data.value } };
    var roleresultdata = ajaxProcess(roleurl, roleparaObj).Data;
    /*清空页面上的角色信息*/

    $(".smartRoleAddName").empty()
        .append("<select name='userAddRoleName'id='userAddRoleName' lay-filter='rolefilter'><option value></option></select>");
    $.each(roleresultdata,
        function(i, item) {
            $("select[name='userAddRoleName']").append("<option value=" + item.ID + ">" + item.Name + "</option>");
        });

};
/*权限操作*/