$(function () {
    layui.use(["form", "layer","laypage","laydate"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.laydate = layui.laydate;
        window.form = layui.form();
        getOperationLogData();
    });

    // 查看按钮
    $(".operationLog-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/OperationLog/OperationLogEditGet",
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=name]").val(data.LogCreateName);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPop("", ".charge-pop", "系统日志管理");
    });

    //// 关闭
    $(".submit-btn").click(function () {
        layer.close($(this).parents(".layui-layer").data("index"));
        closeLayer();
    });

    // 获取日志类型下拉框
    var dotElem = [{ container: ".logType-select", tmp: ".logType-tmp" }];
    ajaxObj.setUrl("/OperationLog/GetLogSelect").setDotEle(dotElem).getData();
});
var pageSize = 15;
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getOperationLogData = function () {
    var ajaxObj = {
        url: "/OperationLog/OperationLogGet",
        paraObj: {
            data: {
                BeginTime: $("#beginDate").val(),
                EndTime: $("#endDate").val(),
                Name: $("#nameValue").val(),
                Account: $("#account").val(),
                LogType: $("[name=LogType]").val(),
                PageNum: 1,
                PageSize: pageSize
            }
        },
        hasPage:true,
        dotEle: [
            {
                container: ".operationLog-table",
                tmp: ".operationLog-tmp"
            },
            {
                container: "#operationLog"
            }
        ]
    };

    dataFunc(ajaxObj);
}

//点击查询日期数据
function onClickBtn() {
    getOperationLogData();
}
