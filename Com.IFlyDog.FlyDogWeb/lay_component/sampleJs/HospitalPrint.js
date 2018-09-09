$(function () {
    layui.use(["form"], function () {
        window.form = layui.form();

        typeof (UE) != "undefined" ? initEditer() : getHospitalPrintData();
    });
    $(".printDetail-table").on("click", ".btn-edit", function () {
        var id = $(this).data("id");
        var ajaxObj = {
            url: "/HospitalPrint/HospitalPrintEditGet",
            paraObj: {
                data: {
                    id: id
                }
            },
            dataCallBack: function (data) {
                data = data.Data;
                $("[name=Width]").val(data.Width);
                $("[name=FontSize]").val(data.FontSize);
                $("[name=FontFamily]").val(data.FontFamily);
                $(".PrintExplain").text(data.PrintExplain);
                var editor = UE.getEditor('container');
                getEditerContent = function () {
                    editor.setContent(data.Content); //编辑器家在完成后，让编辑器拿到焦点
                };
                $("[name=printDetailId]").val(data.ID)
                var opt = {};
                opt.title = "编辑打印设置";
                opt.url = "";
                opt.popEle = ".printDetail-pop";
                opt.area = ["85%", "85%"];
                openPopWithOpt(opt);
            }
        };
        dataFunc(ajaxObj);
    });
    // submit-btn
    $(".printDetail-pop").on("click", ".submit-btn", function () {
        var editer = UE.getEditor('container');
        var ajaxObj = {
            url: "/HospitalPrint/HospitalPrintSubmit",
            paraObj: {
                data: {
                    ID: $("[name=printDetailId]").val(),
                    Content: editer.getContent(),
                    Width: $("[name=Width]").val(),
                    FontSize: $("[name=FontSize]").val(),
                    FontFamily: $("[name=FontFamily]").val()
                }
            },
            isUpdate:true
        };
        if (dataFunc(ajaxObj).ResultType == 0) {
            closeLayer(this);
            editer.reset();
        }
    });
});
var getEditerContent = null;
var initEditer = function () {
    getPrintDetailData();
    UEDITOR_CONFIG.UEDITOR_HOME_URL = '/UEditer/'; //一定要用这句话，否则你需要去ueditor.config.js修改路径的配置信息  
   // UEDITOR_CONFIG.serverUrl = "http://192.168.11.220:8097/api/Picture/UploadImage?imagepath=print";
    UE.getEditor('container').ready(function () { getEditerContent(); });
}
var getPrintDetailData = function () {
    var ajaxObj = {
        url: "/HospitalPrint/HospitalPrintGet",
        paraObj: {
            data: {
                hospitalID: GetQueryString("hospitalID")
            }
        },
        dotEle: [
            {
                container: ".printDetail-table",
                tmp: ".printDetail-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}
var getHospitalPrintData = function () {
    var ajaxObj = {
        url: "/Hospital/HospitalGet",
        paraObj: {},
        dotEle: [
            {
                container: ".hospital-table",
                tmp: ".hospital-tmp"
            }
        ]
    };
    dataFunc(ajaxObj);
}

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
var getPageData = getPrintDetailData;