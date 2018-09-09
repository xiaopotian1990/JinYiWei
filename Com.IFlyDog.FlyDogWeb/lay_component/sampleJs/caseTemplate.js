$(function () {
    layui.use(["form", "layer", 'layedit'], function () {
        var layer = layui.layer;
        window.layedit = layui.layedit;
        // editerIndex = layedit.build('caseTemplateContent'); //建立编辑器
        window.form = layui.form();
    });
    getCaseTemplateData();
    // 添加按钮
    $(".btn-add").click(function () {
        var editor = UE.getEditor('caseTemplateContent');
        getEditerContent = function () { editor.reset(); };
        openPopWithOpt({
            url: "CaseTemplateAdd",
            popEle: ".caseTemplate-pop",
            title: "病例模板",
            func: function () { form.render(); },
            area: ["85%;min-width:70%;max-width:85%", "85%;min-height:70%;max-height:85%"]
        });
    });
    // 编辑按钮
    $(".caseTemplate-table").on("click", ".btn-edit", function () {
        var ajaxObj = {
            url: "/CaseTemplate/CaseTemplateEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=caseTemplateId]").val(data.ID);
                $("[name=title]").val(data.Title);
                $("#remarkValue").val(data.Remark);
                var editor = UE.getEditor('caseTemplateContent');
                getEditerContent = function () {
                    editor.setContent(data.RtfContent); //编辑器家在完成后，让编辑器拿到焦点
                };
                $("[name=status]").find("[value=" + data.OpenStatus + "]").prop("selected", true);
                form.render();
            }
        };
        dataFunc(ajaxObj);
        openPopWithOpt({
            url:"CaseTemplateSubmit",
            popEle:".caseTemplate-pop",
            title:"修改病例模板",
            func:function () { form.render(); },
            //area:["55%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"]
            area: ["85%;min-width:70%;max-width:85%", "85%;min-height:70%;max-height:85%"]
        });
    });


    // 查看按钮
    $(".caseTemplate-table").on("click", ".btn-sel", function () {
        var ajaxObj = {
            url: "/CaseTemplate/CaseTemplateEditGet",
            paraObj: {
                data: {
                    id: Common.StrUtils.isFalseSetEmpty($(this).data("id"))
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=caseTemplateTitle]").text("" + data.Title + "");
                var editor = UE.getEditor('selContent');
                getEditerContent = function () {
                    editor.setContent(data.RtfContent); //编辑器家在完成后，让编辑器拿到焦点
                };
                //取消并关闭按钮
                $(".layui-btn.knowledge.dept_close").on("click", function () {
                    //$("#sels").html("");
                   // layer.close(index);
                });
                form.render();
            }
        };
        dataFunc(ajaxObj);
        var opt = {};
        opt.title = "查看模板";
        // opt.url = "KnowledgeEdit";
        opt.popEle = ".caseTemplates-pop";
        opt.area = ["80%;min-width:680px", "85%;min-height:500px"];
        opt.func = function () { form.render(); };
        openPopWithOpt(opt);
    });

    // 弹窗提交
    $(".caseTemplate.submit-btn").click(function () {
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
            url: "/CaseTemplate/" + $(this).parents(".layui-layer").data("url"),
            paraObj: {
                data: {
                    ID: Common.StrUtils.isFalseSetEmpty($("[name=caseTemplateId]").val()),
                    Title: Common.StrUtils.isFalseSetEmpty($("[name=title]").val()),
                    Remark: Common.StrUtils.isFalseSetEmpty($("#remarkValue").val()),
                    OpenStatus: $("[name=status]").val(),
                    RtfContent: Common.StrUtils.isFalseSetEmpty(UE.getEditor('caseTemplateContent').getContent())

                }
            },
            isUpdate: true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
        }

    });
    $(".search-btn").click(function () {
        getCaseTemplateData();
    });
    initEditer();
});
var getEditerContent = function () { },
    pageIndex = 1,
    pageSize = 15; //分页数据
var initEditer = function () {
    UEDITOR_CONFIG.UEDITOR_HOME_URL = '/UEditer/'; //一定要用这句话，否则你需要去ueditor.config.js修改路径的配置信息  
    //UEDITOR_CONFIG.serverUrl = "/Picture/UploadImage?imagepath=print";
    UE.getEditor('caseTemplateContent').ready(function () { getEditerContent(); });
    UE.getEditor('selContent').ready(function () { getEditerContent(); });
}
var verify = function () {
    var name = $("[name=title]").val();
    if (Common.StrUtils.isNullOrEmpty(name)) {
        layer.msg("标题不能为空！", { icon: 2 });
        return false;
    }

    if (Common.StrUtils.isNullOrEmpty(UE.getEditor('caseTemplateContent').getContent())) {
        layer.msg("内容不能为空！", { icon: 2 });
        return false;
    }
    return true;
}
var getCaseTemplateData = function () {
    var ajaxObj = {
        url: "/CaseTemplate/CaseTemplateGet",
        paraObj: {
            data: {
                //CategoryID: Common.StrUtils.isFalseSetEmpty($(".s-knowledgeCategory").val()),
                  Title:$("#titleValue").val()
                //PageNum: pageIndex,
                //PageSize: pageSize
            }
        },
        dotEle: [
            {
                container: ".caseTemplate-table",
                tmp: ".caseTemplate-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getPageData = getCaseTemplateData;


//function clickBtn() {
//    var url = "/CaseTemplate/CaseTemplateGet";

//    var realData = {};
//    realData.Title = $("#titleValue").val();
//    //realData.PageNum = 1;
//    //realData.PageSize = 2;


//    var paraObj = {};
//    paraObj.data = realData;

//    var data = ajaxProcess(url, paraObj).Data;
//    var interText = doT.template($("#caseTemplate-tmp").text());
//    $(".caseTemplate-table").html(interText(data.PageDatas));
//}