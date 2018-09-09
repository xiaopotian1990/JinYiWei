$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getPageData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("ShareCategoryAdd", ".shareCategory-pop", "分享家");
    });
    // 编辑按钮
    $(".shareCategory-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/ShareCategory/ShareCategoryGetByID",
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=shareCategoryId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=level]").val(data.Level);
                $("[name=number]").val(data.Number);
                $("[name=icon]").data("url",data.Icon).attr("src",data.Icon);
                $("[name=remark]").val(data.Remark);
            }
        };
        dataFunc(ajaxObj);
        openPop("ShareCategoryUpdate", ".shareCategory-pop", "分享家");
    });
    // 弹窗提交
    $(".submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var ajaxObj = {
            url: "/ShareCategory/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=shareCategoryId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Level: $("[name=level]").val(),
                    Number: $("[name=number]").val(),
                    Icon: Common.StrUtils.isFalseSetEmpty($("[name=icon]").data("url")),
                    Remark: Common.StrUtils.isFalseSetEmpty($("[name=remark]").val())
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
            $("[name=icon]").src("").data("url", "");
        }

    });
    // 删除按钮
    $(".shareCategory-table").on("click", ".btn-remove", function () {
        var id=Common.StrUtils.isFalseSetEmpty($(this).data("id"));
        layer.confirm('您确定删除本条数据？',
               {
                   btn: ['确定', '取消'] //按钮
               },
                function () {
                    var ajaxObj = {
                        url: "/ShareCategory/ShareCategoryDelete",
                        paraObj: {
                            data: {
                                ID: id
                            }
                        },
                        isUpdate: true
                    };
                    dataFunc(ajaxObj);
                }, function () {
                    layer.msg('已经取消此操作',
                    {
                        icon: 6
                    });
                });       
    });
    // 上传图片按钮
    $(".upload-btn").click(function () {
        $("#hideFile").click();
    });
    $("#hideFile").change(function (data) {
        FileUpload("share", function (data) {
            $("[name=icon]").data("url", data.Data).attr("src", data.Data);
        }, "hideFile");
    });
    $(".close-layer").click(function () {
        $("[name=icon]").src("").data("url","");
    });
});
var verify = function () {
    var name = $("[name=name]").val(),
        level = $("[name=level]").val(),
        number = $("[name=number]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(level)) {
        layer.msg("等级不能为空！", { icon: 2 });
        return false;
    } else if (isNaN(level)) {
        layer.msg("等级只能是数字！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("[name=icon]").data("url"))) {
        layer.msg("请选择图标！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(number)) {
        layer.msg("分享数量限制不能为空！", { icon: 2 });
        return false;
    } else if (isNaN(number)) {
        layer.msg("分享数量限制只能是数字！", { icon: 2 });
        return false;
    }
    return true;
}
var getShareCategoryData = function () {
    var ajaxObj = {
        url: "/ShareCategory/ShareCategoryGet",
        paraObj: {},
        dotEle: [
            {
                container: ".shareCategory-table",
                tmp: ".shareCategory-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getPageData = getShareCategoryData;