$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getbedData();
  
    /*添加床位*/
    $(".btn-add").click(function() {
        openPop("", ".data-bed-pop", "添加床位");
         
        $("textarea").text("");
    });
    /*添加-提交按钮*/
    $(".data-bed-pop").on("click", ".bed-sub-btn", function () {
        if (!verify()) { return false; }
        var paraObj = {};
        paraObj.data = {
            Name: Common.StrUtils.isFalseSetEmpty($("input[name='benaddName']").val()),
            Remark: $("[name='benaddRemark']").val()
        };
        var url = "/Bed/BedAdd";
        var result = ajaxProcess(url, paraObj);
        if (result) {
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1, time: 1000 });
                closeLayer(this);

            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
   
    /*修改*/
     $(".layui-field-box").on("click", ".btn-edit", function () {
      
        openPop("", ".data-bed-pop", "修改信息");
        $(".bed-sub-btn").addClass("edit-btn").removeClass("bed-sub-btn");
        var bedId = $(this).attr("data-id");
        $("input[name='bedthId']").val(bedId);
        var url = "/Bed/BedGetById";
        var paraObj = { data: { ID: bedId } };
        var result = ajaxProcess(url, paraObj).Data;
        $("input[name='benaddName']").val(result.Name);
        $("[name='benaddRemark']").text(result.Remark === null ? "" : result.Remark);
        form.render();
    
    });
    /*修改提交*/
    $(".data-bed-pop").on("click", ".edit-btn", function () {
        if (!verify()) { return false; }
        var paraObj = {};
        paraObj.data = {
            ID: $("input[name='bedthId']").val(),
            Name: Common.StrUtils.isFalseSetEmpty($("input[name='benaddName']").val()),
            Remark: $("[name='benaddRemark']").val()
        };
        var url = "/Bed/BedUpdate";
        var result = ajaxProcess(url, paraObj);
        if (result) {
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1, time: 1000 });
                closeLayer(this);
                getbedData();
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
    /*停用*/
    $(".layui-field-box").on("click", ".EditStopBut", function() {
        var editStopid = $(this).attr("status");
        var thistopId = $(this).attr("stopID");
        var tipMsg = editStopid == 0 ? "您确定要停用本条数据?" : "您确定要启用本条数据?";
        layer.confirm(tipMsg,
            { btn: ["确定", "取消"] },
            function () {
                var paraObj = { data: { ID: thistopId, Status: editStopid } };
                var url = "/Bed/BedStopOrUse";
                var result = ajaxProcess(url, paraObj);
                if (result) {
                    if (result.ResultType === 0) {
                        layer.msg("操作成功！", { icon: 1, time: 2000 });
                        getbedData();
                    } else {
                        layer.msg(result.Message, { icon: 5 });
                    }
                };
                return false;
            },
            function () {
                layer.msg('已经取消此操作', { icon: 6 });
            });
    });
});

var verify = function () {
    var name = $("input[name=benaddName]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("请输入床位名称！", { icon: 2 });
        return false;
    }
    return true;
}
/*查询*/
var getbedData = function () {
    var url = "/Bed/BedIndexGet";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".bed-tmp").text());
    $(".bed-tbody").html(interText(result));
};