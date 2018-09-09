$(function () {
    layui.use(["form", "layer"], function () {
        var layer = layui.layer;
        window.form = layui.form();
    });
    getTagGroupData();
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("TagGroupAdd", ".tagGroup-pop", "添加标签组", function () { $(".tags-table").parents(".layui-layer").find(".layui-layer-close").click(function () { closeFunc(); }) });
    });
    // 编辑按钮
    $(".tagGroup-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/TagGroup/TagGroupEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=tagGroupId]").val(data.ID);
                $("[name=name]").val(data.Name);
                $("[name=remark]").val(data.Remark);
                $(".tags-table").append(doT.template($(".tags-tmp").text())(data.TagGroupDetailAdd));
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPop("TagGroupSubmit", ".tagGroup-pop", "标签组", function () { $(".tags-table").parents(".layui-layer").find(".layui-layer-close").click(function () { closeFunc(); }) });
    });
    // 弹窗提交
    $(".tagGroup.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var tags = [];
        $(".tags-table").find("tr").each(function (i, item) {
            item = $(item);
            tags.push({
                TagID : item.attr("tagID")
            });
        });
        var ajaxObj = {
            url: "/TagGroup/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=tagGroupId]").val()),
                    Name: Common.StrUtils.isFalseSetEmpty($("[name=name]").val()),
                    Remark : Common.StrUtils.isFalseSetEmpty($("[name=remark]").val()),
                    TagGroupDetailAdd: tags
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this, closeFunc);
        }

    });
    // 选择标签提交按钮
    $(".tag.submit-btn").click(function () {
        var tags = [];
        $(".tag-table").find(":checked").each(function (i,item) {
            item = $(item);
            if ($(".tags-table").find("[tagID=" + item.val() + "]").length == 0) {
                tags.push({
                    TagID: item.val(),
                    TagName: item.attr("title")
                });
            }
        });
        $(".tags-table").append(doT.template($(".tags-tmp").text())(tags));
        closeLayer(this);
    });
    // 弹窗表格删除按钮
    $(".tags-table").on("click", ".remove-btn", function () {
        $(this).parents("tr").remove();
    });
    // 弹窗增加按钮
    $(".add-detail-btn").click(function () {
        getTagData();
        openPop("", ".tag-pop", "选择标签");
    });
    $(".tagGroup-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("确认删除本条数据？", function () {
            if (ajaxObj.setIsUpdateTrue().setUrl("TagGroupDelete").setParaObj({ data: { id: id } }).getData().ResultType == 0) {
                closeLayer(this, closeFunc);
            }
        }, function () {
            layer.msg("您已取消操作！", { icon: 1 });
        });
    });
});
var closeFunc = function () {
    $(".tags-table").empty();
}
var verify = function () {
    var name = $("[name=name]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getTagGroupData = function () {
    var ajaxObj = {
        url: "/TagGroup/TagGroupPost",
        paraObj: {},
        dotEle: [
            {
                container: ".tagGroup-table",
                tmp: ".tagGroup-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getTagData = function () {
    var ajaxObj = {
        url: "/SmartTag/TagGetByIsOk",
        paraObj: {},
        dataCallBack: function (data) {
            var tmpHtml = $(doT.template($(".tag-tmp").text())(data.Data));
            $(".tags-table").find("tr").each(function (i,item) {
                item = $(item);
                tmpHtml = tmpHtml.not("[tagid=" + item.attr("tagid") + "]");
            });
            $(".tag-table").html(tmpHtml);
        }
    };
    dataFunc(ajaxObj);
}
var getPageData = getTagGroupData;