(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".medicalRecord.btn-submit", function () {
            if (!verify()) {
                return false;
            };
            params.setDataParams({
                ID:pageContent.find("[name=ID]").val(),
                CustomerID: custid,
                MedicalRecordID: pageContent.find("[name=MedicalRecordID]").val(),
                No: pageContent.find("[name=No]").val(),
                Location: pageContent.find("[name=Location]").val(),
                Content: editor.getContent(),
                Remark: pageContent.find("[name=Remark]").val()
            });
            getPageData = getMedicalRecord;
            if (ajaxObj.setUrl("/CustomerMedicalRecord/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        window.MedicalRecord.getDetail = function (id) {
            var result = ajaxObj.setUrl("/CustomerMedicalRecord/GetByPKIDGet").setParaObj(params.setDataParam("ID", id)).getData();
            this.setEntry(result.Data);
            return this;
        }
        window.MedicalRecord.look = function (id) {
            var result = ajaxObj.setUrl("/CustomerMedicalRecord/GetByPKIDGet").setParaObj(params.setDataParam("ID", id)).getData();
            result = result.Data;
            result.look = true;
            this.setEntry(result);
            return this;
        }
        window.MedicalRecord.remove = function (id) {
            layer.confirm("确认删除？", function () {
                getPageData = getMedicalRecord;
                var result = ajaxObj.setUrl("/CustomerMedicalRecord/CustomerMedicalRecordDelete").setIsUpdateTrue().setParaObj(params.setDataParam("ID", id)).getData();
            }, function () {
                layer.msg("已取消操作~", { icon: 1 });
            });
            return this;
        }
        var verify = function () {
            return true;
        };
        initEditer();
        form.on("select(MedicalRecordID)", function (data) {
            var result = ajaxObj.setUrl("/CaseTemplate/CaseTemplateEditGet").setParaObj(params.setDataParam("id", data.value)).getData();
            var content = result.Data ? result.Data.RtfContent : "";
            editor.setContent(content);

        });
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        pageContent.find("[name=MedicalRecordID]").attr("lay-filter", "MedicalRecordID");
        editor.reset()
        if (data.look) {
            pageContent.find(".medicalRecord.btn-submit").hide();
        } else {
            pageContent.find(".medicalRecord.btn-submit").show();
        }
        // 填充页面模版到页面容器中
        fillData(pageContent.find(".MedicalRecord-form"), pageContent.find(".MedicalRecord-form-tmp"), data);
        pageContent.find("[name=MedicalRecordID]").find("[value=" + data.MedicalRecordID + "]").prop("selected", true);
        getEditerContent = function () {
            editor.setContent(data.Content);
        }
        form.render();
    }
    getPageData = function () { };
    var getEditerContent = function () { },
        editor = null;
    var initEditer = function () {
        UEDITOR_CONFIG.UEDITOR_HOME_URL = '/UEditer/'; //一定要用这句话，否则你需要去ueditor.config.js修改路径的配置信息  
        UEDITOR_CONFIG.serverUrl = "/Picture/UploadImage?imagepath=print";
        UE.getEditor('caseTemplateContent').ready(function () { getEditerContent(); });
        editor = UE.getEditor('caseTemplateContent');
    }
    // 填充页面模版到页面容器中
    emptyFormData = { ID: "", MedicalRecordID: "", No: "", Location: "",Remark:"", Content: "" };
    Model.init("MedicalRecord", "/CustomerMedicalRecord/CustomerMedicalRecordInfo", "添加电子病例", "CustomerMedicalRecordAdd", emptyFormData, successFunc, openFunc).setArea(["80%","80%"]);
})(window, jQuery);