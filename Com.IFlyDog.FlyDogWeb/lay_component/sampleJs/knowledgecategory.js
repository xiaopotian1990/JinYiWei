$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getKnowledgeCategoryData();
    // 添加按钮
    $(".btn-add").click(function () {
        OpenPop("KnowledgeCategoryAdd");
    });
    // 编辑按钮
    $(".knowledgeCategory-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/KnowledgeCategory/KnowledgeCategoryGetByID",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=knowledgeCategoryId]").val(data.ID);
                $("[name=status]").val(data.OpenStatus);
                $("[name=name]").val(data.Name);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        OpenPop("KnowledgeCategoryEdit");
    });

    // 弹窗提交
    $(".submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var ajaxObj = {
            url: "/KnowledgeCategory/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=knowledgeCategoryId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    OpenStatus: Common.StrUtils.isFalseSetEmpty($("[name=status]").val()),
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
var OpenPop = function (url) {
    openPop(url, ".charge-pop", "知识分类管理")
}
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getKnowledgeCategoryData = function () {
    var ajaxObj = {
        url: "/KnowledgeCategory/KnowledgeCategoryIndexGet",
        paraObj: {},
        dotEle: [
            {
                container: ".knowledgeCategory-table",
                tmp: ".knowledgeCategory-tmp"
            },
            {
                container: "#knowledgecategory"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var dataFunc = function (ajaxObj) {
    var result = ajaxProcess(ajaxObj.url, ajaxObj.paraObj);
    if (ajaxObj.isUpdate) {
        layer.msg(result.Message, { icon: result.ResultType + 1 });
        getKnowledgeCategoryData();
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