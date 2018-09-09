$(function () {
    layui.use(["form", "layer", "laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.form = layui.form();
        getPageData();
    });
    $(".btn-add").click(function () {
        var data = {ID:"",Name:"",SortNo:"", Remark:"",ChannelGroupDetailAdd: [] }
        fillData(".channel-table", ".channel-tmp", data);
        fillData(".channel-form", ".channel-form-tmp", data);
        openPop("channelGroupAdd", ".channelGroup-pop", "添加渠道组");
    });
    // 店铺列表按钮
    $(".channelGroup-table").on("click", ".btn-edit", function () {
        params.setDataParam("id", $(this).data("id"));
        $("[name=StoreUserID]").val(params.data.userID);
        var dotEle = [{ container: ".channel-form", tmp: ".channel-form-tmp" }, { container: ".channel-table", tmp: ".channel-tmp" }];
        ajaxObj.setUrl("/ChannelGroup/ChannelGroupEditGet").setParaObj(params).setDotEle(dotEle).getData();
        openPop("ChannelGroupSubmit", ".channelGroup-pop", "编辑渠道组");
    });
    // 添加弹窗提交按钮
    $(".channelGroup.btn-submit").on("click", function () {
        if (!verify()) {
            return false;
        }
        var channelGroupDetailAdd = [];
        $(".channel-table").find("tr").each(function (i, item) {
            channelGroupDetailAdd.push({ ChannelID: $(item).attr("channelid") });
        });
        params.setDataParams({
            ID: $("[name=ID]").val(),
            Name: $("[name=Name]").val(),
            SortNo: $("[name=SortNo]").val(),
            Remark: $("[name=Remark]").val(),
            ChannelGroupDetailAdd: channelGroupDetailAdd
        });
        if (ajaxObj.setUrl("/ChannelGroup/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 渠道组表格删除按钮
    $(".channelGroup-table").on("click", ".btn-remove", function () {
        var id = $(this).data("id");
        layer.confirm("您确定删除本条数据？", function () {
            params.setDataParams({
                ID: id
            });
            if (ajaxObj.setUrl("/ChannelGroup/ChannelGroupDelete").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        }, function () {
            layer.msg("已取消操作！",{icon:1});
        });
    });
    // 添加弹窗表格删除按钮
    $(".channel-table").on("click", ".btn-remove", function () {
        $(this).parents("tr").remove();
    });
    // 打开选择渠道
    $(".add-channel").click(function () {
        getChannelData();
        openPop("", ".channel-pop", "选择渠道");
    })
    // 选择商铺弹窗提交按钮
    $(".channel.btn-submit").click(function () {
        $(".channel-pop-table").find("[type=checkbox]:checked").each(function (i, item) {
            item = $(item);
            if ($(".channel-table").find("[channelid=" + item.val() + "]").length == 0) {
                fillData(".channel-table", ".channel-tmp", { ChannelGroupDetailAdd: [{ ChannelID: item.val(), ChannelName: item.data("name") }] }, true);
            }
        });
        closeLayer(this);
    });
    $(".btn-check").click(function () {
        var dotEle = [{ container: ".channel-check-table", tmp: ".channel-check-tmp" }];
        ajaxObj.setUrl("/ChannelGroup/GetChannelGroupCheckPost").setDotEle(dotEle).getData();
        openPop("", ".channel-check-pop", "渠道组检测");
    });
});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
var getchannelGroupData = function () {
    var dotEle = [{ container: ".channelGroup-table", tmp: ".channelGroup-tmp" }];
    ajaxObj.setUrl("/ChannelGroup/ChannelGroupPost").setDotEle(dotEle).setParaObj(params).getData();
}
var verify = function () {
    if (SUtils.isNullOrEmpty($("[name=Name]").val())) {
        layer.msg("名称不能为空！", { icon: 2 });
        return false;
    }
    if (SUtils.isNullOrEmpty($("[name=SortNo]").val())) {
        layer.msg("序号不能为空！", { icon: 2 });
        return false;
    }else if (isNaN($("[name=SortNo]").val())) {
        layer.msg("序号只能是数字！", { icon: 2 });
        return false;
    }
    return true;
}
var getChannelData = function () {
    ajaxObj.setUrl("/Channel/ChannelGetIsOk").setDataCallBack(function (data) {
        fillData(".channel-pop-table", ".channel-pop-tmp", []);
        $.each(data.Data, function (i, item) {
            $(".channel-table").find("[channelid=" + item.ID + "]").length == 0 && fillData(".channel-pop-table", ".channel-pop-tmp", [item], true);
        });
    }).getData();
}
getPageData = getchannelGroupData;
var SUtils = Common.StrUtils;
SUtils.setEmpty = SUtils.isFalseSetEmpty;