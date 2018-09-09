$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getRelationData();
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("RelationAdd", ".charge-pop", "关系管理");
    });
    // 编辑按钮
    $(".relation-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/Relation/RelationEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=relationId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPop("RelationSubmit", ".charge-pop", "关系管理");
    });
    // 删除按钮
    $(".relation-table").on("click", ".btn-remove", function () {
        var rmvId = $(this).data("id");
        layer.confirm("确定删除本条信息？", {
            btn: ['是', '否'] //按钮
        }, function () {
            var ajaxObj = {
                url: "/Relation/RelationDelete",
                paraObj: {
                    data: {
                        id: Common.StrUtils.isFalseSetEmpty(rmvId)
                    }
                },
                isUpdate: true
            };
            dataFunc(ajaxObj);
        }, function () { });
    });
    // 弹窗提交
    $(".submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var ajaxObj = {
            url: "/Relation/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=relationId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
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
var getRelationData = function () {
    var ajaxObj = {
        url: "/Relation/RelationGet",
        paraObj: {},
        dotEle: [
            {
                container: ".relation-table",
                tmp: ".relation-tmp"
            },
            {
                container: "#relation"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var dataFunc = function (ajaxObj) {
    var result = ajaxProcess(ajaxObj.url, ajaxObj.paraObj);
    if (ajaxObj.isUpdate) {
        layer.msg(result.Message, { icon: result.ResultType + 1 });
        getRelationData();
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