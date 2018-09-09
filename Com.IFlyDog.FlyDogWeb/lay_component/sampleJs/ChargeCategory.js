$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getCargeCategoryData();
    // 添加按钮
    $(".btn-add").click(function () {
        var opt = {};
        opt.title = "收费项目类型";
        opt.url = "ChargeCategoryAdd";
        opt.popEle = ".charge-pop";
        opt.area = ["35%;min-width:680px;max-width:800px", "75%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 编辑按钮
    $(".chargeCategory-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/ChargeCategory/ChargeCategoryEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=chargeCategoryId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("#ChargeCategory").find("[value=" + data.ParentID + "]").prop("selected", true);
                $("[name=sort]").val(data.SortNo);
                $("[name=remark]").val(data.Remark);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title = "收费项目类型";
        opt.url = "ChargeCategorySubmit";
        opt.popEle = ".charge-pop";
        opt.area = ["35%;min-width:680px;max-width:800px", "75%;min-height:500px;max-height:600px"];
        openPopWithOpt(opt);
    });
    // 删除按钮
    $(".chargeCategory-table").on("click", ".btn-remove", function () {
        var rmvId = $(this).data("id");
        layer.confirm("确定删除本条信息？", {
            btn: ['是', '否'] //按钮
        }, function () {
            var ajaxObj = {
                url: "/ChargeCategory/SmartProductCategoryDelete",
                paraObj: {
                    data: {
                        id: Common.StrUtils.isFalseSetEmpty(rmvId)
                    }
                },
                isUpdate:true
            };
            dataFunc(ajaxObj);
        }, function () {});
    });
    // 弹窗提交
    $(".submit-btn").click(function () {
         if(!verify()){
             return false;
        }
        var ajaxObj = {
            url: "/ChargeCategory/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=chargeCategoryId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    ParentID: Common.StrUtils.isNullOrEmpty($("#ChargeCategory").val()) ? 0 : $("#ChargeCategory").val(),
                    SortNo: Common.StrUtils.isFalseSetEmpty($("[name=sort]").val()),
                    Remark: Common.StrUtils.isFalseSetEmpty($(".remark").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            layer.close($(this).parents(".layui-layer").data("index"));
            closeLayer();
        }
        
    });
});
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (isNaN($("[name=sort]").val())) {
        layer.msg("排序号必须为数字！", { icon: 2 });
        return false;
    }
    return true;
}
var getCargeCategoryData = function () {
    var ajaxObj = {
        url:"/ChargeCategory/ChargeCategoryGet",
        paraObj: {},
        dotEle:[
            {
                container: ".chargeCategory-table",
                tmp: ".chargeCategory-tmp"
            },
            {
                container: "#ChargeCategory",
                tmp: ".select-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var dataFunc = function (ajaxObj) {
    var result = ajaxProcess(ajaxObj.url, ajaxObj.paraObj);
    if (ajaxObj.isUpdate) {
        layer.msg(result.Message, { icon: result.ResultType + 1 });
        getCargeCategoryData();
    } else {
        if (Common.StrUtils.isNullOrEmpty(ajaxObj.dataCallBack)) {
            for (var i = 0;i < ajaxObj.dotEle.length;i++ ) {
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