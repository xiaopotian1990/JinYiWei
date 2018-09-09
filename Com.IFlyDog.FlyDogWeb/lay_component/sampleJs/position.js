$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getPositionData();
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("PositionAdd", ".charge-pop","岗位分工管理");
    });
    // 编辑按钮
    $(".position-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/Position/PositionEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=positionId]").val(data.ID);
                $("[name=status]").val(data.Status);
                $("[name=name]").val(data.Name);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPop("PositionSubmit", ".charge-pop","岗位分工管理");
    });

    // 弹窗提交
    $(".submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var ajaxObj = {
            url: "/Position/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=positionId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Status: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
                    Remark: Common.StrUtils.isFalseSetEmpty($(".remark").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
        }

    });
});
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getPositionData = function () {
    var ajaxObj = {
        url: "/Position/PositionGet",
        paraObj: {},
        dotEle: [
            {
                container: ".position-table",
                tmp: ".position-tmp"
            },
            {
                container: "#position"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var dataFunc = function (ajaxObj) {
    var result = ajaxProcess(ajaxObj.url, ajaxObj.paraObj);
    if (ajaxObj.isUpdate) {
        layer.msg(result.Message, { icon: result.ResultType + 1 });
        getPositionData();
    } else {
        if (Common.StrUtils.isNullOrEmpty(ajaxObj.dataCallBack)) {
            for (var i = 0; i < ajaxObj.dotEle.length; i++) {
                var container = ajaxObj.dotEle[i].container,
                    tmp = ajaxObj.dotEle[i].tmp;
                $(container).html(doT.template($(tmp).text())(ajaxObj.isDataRoot ? result : result.Data));
            }
        } else {
            ajaxObj.dataCallBack(result);
        }
    }
    typeof (form) != "undefined" ? form.render() : "";
    return result;
}