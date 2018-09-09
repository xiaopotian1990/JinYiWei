$(function () {
    layui.use(["form", "element", "layer", "laydate", "laypage"], function () {
        var element = layui.element(), laydate = layui.laydate(), layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getCustomerManager();
        //打开Tab页
        $(".td-customer-info").on("click", ".addCustomerTab", function () {
            var id = $(this).parent().prev().text();
            parent.layui.tab({
                elem: ".admin-nav-card"  
            }).tabAdd(
            {
                title: "客户档案:" + $(this).text() +"编号:"+ id,
                href: "/Customer/CustomerProfile", //地址
                icon: "fa-user"
            });
        });
    });
    
    $(".layui-disabled").attr("readonly", "readonly");
    /*重置按钮*/
    $(".customer-reset-btn").click( function () {
        $("input").val(""); 
        $("input[name='hideManagerUserID']").attr("manageruserid", "");
        $("input[name='hideManagerUserID']").attr("exploituserid", "");
        getCustomerManager();
    });
    /*查询按钮*/ 
    $(".customer-search-btn").click(function () {
      getCustomerManager();
    });
    var flag;
    /*开发人员选择*/
    $("input[name='ExploitUserName']").click(function() {
        openPop("", ".data-addcustomer-remindUser-pop", "选择开发人员");
        flag = 1;
        form.render();
    });
 
    /*添加顾客弹窗中-选择开发人员页面中搜索*/
    //$(".layui-field-box").on("click", ".search-user", function () {
    //    currentExploitUser();
    //});
    /*咨询人员选择*/
    $("input[name='ManagerUserName']").click(function() {
        openPop("", ".data-addcustomer-remindUser-pop", "选择开发人员");
        flag = 2;
        form.render();
    });
 
    /*添加顾客弹窗中-选择咨询人员页面中搜索*/
    $(".data-addcustomer-remindUser-pop").on("click", ".search-user", function () {
   
        currentExploitUser();
    });
    /*添加顾客弹窗中-选择开发人员页面中-选择该用户*/
    $(".data-addcustomer-remindUser-pop").on("click", ".present-user", function () {
        var userid = $(this).attr("value");
        var username = $(this).parent().parent().find("td")[1].innerHTML;
        if (flag === 1) {
            $("input[name='hideExploitUserID']").attr("exploituserid", userid);
            $("input[name='ExploitUserName']").val(username);
            if (userid !== "0" || username !== "") {
                layer.msg("开发人员选择成功!", { icon: 6 });
                closeLayer(this);
            }
        }
        if (flag === 2) {
            $("input[name='hideManagerUserID']").attr("manageruserid", userid);
            $("input[name='ManagerUserName']").val(username);
            if (userid !== "0" || username !== "") {
                layer.msg("咨询人员选择成功!", { icon: 6 });
                closeLayer(this);
            }
        }

       
    });

});
var pageIndex = 1, pageSize = 12;
/*查询顾客*/
var getCustomerManager = function () {
    var ajaxObj = {
        url: "/Customer/GetCustomerManager",
        paraObj: {
            data: {
                CustomerID: $("input[name='customerID']").val(),
                CustomerName: $("input[name='customerName']").val(),
                Gender: $("select[name='customerGender']").val(),
                Mobile: $("input[name='customerMobile']").val(),
                ChannelID: $(".customerChannel").val(),
                StoreID: $(".customerStoreCategory").val(),
                SymptomID: $(".customerCategory").val(),
                ComeType: $("select[name='customerComeType']").val(),
                DealType: $("select[name='customerDealType']").val(),
                WechatStatus: $("select[name='customerWechatStatus']").val(),
//                VisitHospitalID: $(".customerDZHospital").val(),
                ExploitUserID: $("input[name='hideExploitUserID']").attr("exploituserid"),
                FirstVisitTimeStart: $("input[name='customerFirstVisitTimeStart']").val(),
                FirstVisitTimeEnd: $("input[name='customerFirstVisitTimeEnd']").val(),
                LastVisitTimeStart: $("input[name='customerLastVisitTimeStart']").val(),
                LastVisitTimeEnd: $("input[name='customerLastVisitTimeEnd']").val(),
                LastConsultTimeStart: $("input[name='customerLastConsultTimeStart']").val(),
                LastConsultTimeEnd: $("input[name='customerLastConsultTimeEnd']").val(),
                CreateTimeStart: $("input[name='customerCreateTimeStart']").val(),
                CreateTimeEnd: $("input[name='customerCreateTimeEnd']").val(),
                AppointmentStart: $("input[name='customerAppointmentStart']").val(),
                AppointmentEnd: $("input[name='customerAppointmentEnd']").val(),
                CashStart: $("input[name='customerCashStart']").val(),
                CashEnd: $("input[name='customerCashEnd']").val(),
                TagID: $(".customerTagCategory").val(),
                MemberCategoryID: $(".customerMemberCategory").val(),
                ShareCategoryID: $(".customerShareCategory").val(),
                PageNum: pageIndex,
                PageSize: pageSize
            }
        },
        hasPage: true,
        dotEle: [
            {
                container: ".td-customer-info",
                tmp: ".customer-data-tmp"
            }
        ]
    };
     dataFunc(ajaxObj);
}
/*选择开发人员*/
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
    var html = "<blockquote class='layui-elem-quote'>" + "没有该用户信息!请核实!" + "</blockquote>";
    if (result.length <= 0) {
        $(".user-info").html(html);
        $(".user-table").empty();
    } else {
        $(".user-table").html(interText(result));
        $(".user-info").html("");
    }

}