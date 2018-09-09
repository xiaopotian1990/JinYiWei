$(function () {
    layui.use(["form", "layer", 'layedit', "laypage"], function () {
        var layer = layui.layer;
        window.laypage = layui.laypage;
        window.layedit = layui.layedit;
        // editerIndex = layedit.build('knowledgeContent'); //建立编辑器
        window.form = layui.form();
        getKnowledgeData();
    });
    // 添加按钮
    $(".btn-add").click(function () {
        var editor = UE.getEditor('knowledgeContent');
        getEditerContent = function () { editor.reset(); };
        var opt = {};
        opt.title = "添加知识";
        opt.url = "KnowledgeAdd";
        opt.popEle = ".knowledge-pop";
        opt.area = ["80%;min-width:680px", "85%;min-height:500px"];
        openPopWithOpt(opt);
    });  
    // 编辑按钮
    $(".knowledge-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/Knowledge/KnowledgeGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=knowledgeId]").val(data.ID);
                $("[name=title]").val(data.Title);
                $("#knowledgeCategory").find("[value=" + data.CategoryID + "]").prop("selected", true);
                $("[name=status]").find("[value=" + data.OpenStatus + "]").prop("selected", true);
                var editor = UE.getEditor('knowledgeContent');
                getEditerContent = function () {
                    editor.setContent(data.RtfContent); //编辑器家在完成后，让编辑器拿到焦点
                };
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title = "修改知识";
        opt.url = "KnowledgeEdit";
        opt.popEle = ".knowledge-pop";
        opt.area = ["80%;min-width:680px", "85%;min-height:500px"];
        opt.func = function () { form.render(); };
        openPopWithOpt(opt);
    });

    // 查看按钮
    $(".knowledge-table").on("click", ".btn-sel", function () {
        var ajaxObj = {
            url: "/Knowledge/KnowledgeGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=knowledgeTitle]").text(""+data.Title+"");
                var editor = UE.getEditor('selContent');
                getEditerContent = function () {
                    editor.setContent(data.RtfContent); //编辑器家在完成后，让编辑器拿到焦点
                };
                //$("[name=knowledgeContents]").html("" + data.RtfContent + "");

                //取消并关闭按钮
                $(".layui-btn.knowledge.dept_close").on("click", function () {
                    //$("#sels").html("");
                    layer.close(index);
                });
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title = "查看知识";
       // opt.url = "KnowledgeEdit";
        opt.popEle = ".knowledgeSel-pop";
        opt.area = ["80%;min-width:680px", "85%;min-height:500px"];
        opt.func = function () { form.render(); };
        openPopWithOpt(opt);
    });

    // 弹窗提交
    $(".knowledge.submit-btn").click(function () {
        if (!verify()) {
            return false;
        }
        var tags = [];
        $(".tags-table").find("tr").each(function (i, item) {
            item = $(item);
            tags.push({
                TagID: item.attr("tagID")
            });
        });
        var ajaxObj = {
            url: "/Knowledge/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=knowledgeId]").val()),
                    CategoryID: Common.StrUtils.isFalseSetEmpty($("#knowledgeCategory").val()),
                    Title: Common.StrUtils.isFalseSetEmpty($("[name=title]").val()),
                    RtfContent: Common.StrUtils.isFalseSetEmpty(UE.getEditor('knowledgeContent').getContent()),
                    OpenStatus: $("[name=status]").val()
                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
        }

    });
    $(".search-btn").click(function () {
        getKnowledgeData();
    });
    initEditer();
});
var getEditerContent = function () { },
    pageIndex = 1,pageSize = 15; //分页数据
var initEditer = function () {
    UEDITOR_CONFIG.UEDITOR_HOME_URL = '/UEditer/'; //一定要用这句话，否则你需要去ueditor.config.js修改路径的配置信息  
  //  UEDITOR_CONFIG.serverUrl = "http://192.168.11.220:8097/api/Picture/UploadImage?imagepath=print";
    UE.getEditor('knowledgeContent').ready(function () { getEditerContent(); });
    UE.getEditor('selContent').ready(function () { getEditerContent(); });
}
var verify = function () {
    var name = $("[name=title]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("标题不能为空！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty($("#knowledgeCategory").val()) || $("#knowledgeCategory").val() == -1) {
        layer.msg("请选择分类！", { icon: 2 });
        return false;
    }
    if (Common.StrUtils.isNullOrEmpty(UE.getEditor('knowledgeContent').getContent())) {
        layer.msg("内容不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getKnowledgeData = function () {
    var ajaxObj = {
        url: "/Knowledge/KnowledgeIndexGet",
        paraObj: {
            data: {
                CategoryID: Common.StrUtils.isFalseSetEmpty($(".s-knowledgeCategory").val()),
                Title: $("[name=s-title]").val(),
                PageNum:pageIndex,
                PageSize :pageSize
            }
        },
        hasPage:true,
        dotEle: [
            {
                container: ".knowledge-table",
                tmp: ".knowledge-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getPageData = getKnowledgeData; 