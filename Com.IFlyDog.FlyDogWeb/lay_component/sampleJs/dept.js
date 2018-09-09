$(function() {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        form.render();
    }); 
    getDept();
    /*添加部门-按钮*/
    $(".add").click(function() {
        openPop("DeptAdd", ".dept-pop", "添加部门");
    }); 
    /*查询按钮*/
    $(".search-btn").click(function() {
        getDept();
    });
    /*编辑按钮*/
    $(".tb-dept").on("click", ".deptEdit", function() {
        openPop("DeptEditSubmit", ".dept-pop", "编辑信息");
        var deptId = $(this).parent().attr("deptId");
        $(".deptIdHi").attr("deptIdHi", deptId);
        var url = "/Dept/DeptEditGet";
        var paraObj = { data: { ID: deptId } };
        var result = ajaxProcess(url, paraObj).Data;
        $("input[name=SortNo]").val(result.SortNo);
        $("input[name=deptName]").val(result.Name);
        $(".deptStatus").find("[value=" + result.OpenStatus + "]").prop("selected", true);
        $(".DeptAddHospitalt").find("[value=" + result.HospitalID + "]").prop("selected", true);
        $("textarea[name=deptRemark]").val(result.Remark);
        form.render();
    });
    /*提交*/
    $(".dept-pop").on("click", ".dept-sub", function () {
        addEdit();
    });
    /*删除按钮*/
    $(".tb-dept").on("click", ".deptDel", function () {
        var deptId = $(this).parent().attr("deptId");
        layer.confirm("您确定删除本条数据？",{btn: ["确定", "取消"]},function () {
            var paraObj = {};
            paraObj.data = {
                ID: deptId 
            };
            var url = "/Dept/DeptDelete";
            var data = ajaxProcess(url, paraObj); 
                if (parseInt(data.ResultType) === 0) { //请求成功返回
                    var message = data.Message; 
                    //提示消息
                    layer.msg(message, { icon: 6 });
                    getDept();
                } else { 
                    layer.msg(data.Message, { icon: 5 });
                } 
            return false; 
        },function () {});
    });  
});
/*验证*/
var verify = function () {
    var no = $("input[name=deptNo]").val();
    var name = $("input[name=deptName]").val(); 
    var status = $(".deptStatus").val();
    var remark = $("textarea[name=deptRemark]").val();
    if (no==="") {
        layer.msg("请输入排序号！", { icon: 2 });
        return false;
    }
    if (name === "") {
        layer.msg("请输入部门名称！", { icon: 2 });
        return false;
    }
    if (status==="-1") {
        layer.msg("请选择开放状态！", { icon: 2 });
        return false;
    } 
    if (remark==="" || remark.length > 50) {
        layer.msg("描述不可为空且不可超过50字符！", { icon: 5 });
        return false;
    }
    return true;
}
/*查询部门*/
var getDept= function() {
    var url = "/Dept/DeptByHospitalIDGet";
    var paraObj = {
        data: {
            hospitalId: $(".HospitalID").val()
        }
    };
    var result = ajaxProcess(url, paraObj); 
    var interText = doT.template($(".dept-tmp").text());
    $(".tb-dept").html(interText(result.Data));
}
/*添加-修改部门*/
var addEdit = function() {
   if (!verify()) { return false; };
    var paraObj = {};
    paraObj.data = {
        ID: $(".deptIdHi").attr("deptIdHi") === "" ? "" : $(".deptIdHi").attr("deptIdHi"),
        SortNo: $("input[name='SortNo']").val(),
        Name: $("input[name='deptName']").val(),
        OpenStatus: $(".deptStatus").val(),
        HospitalID: $(".DeptAddHospital").val(),
        Remark: $("[name='deptRemark']").val()
    };
    var url = "/Dept/" + $(".dept-sub").parents(".layui-layer").data("url");
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        layer.msg(result.Message, { icon: 1 });
        getDept();
        closeLayer(".layui-form");
    } else {
        layer.msg(result.Message, { icon: 5 });
    }
    return false;
};