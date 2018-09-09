(function (win, $) {
    var successFunc = function (pageContent) {

        //提交
        pageContent.on("click", ".Appointment.add-submit", function () {
            if (!verify()) {
                return false;
            };
            params.setDataParams({
                CustomerID: custid,
                HospitalID: pageContent.find(".appointmentHospital").val(),
                AppointmentDate: pageContent.find("[name=AppointmentDate]").val(),
                AppointmentStartTime: pageContent.find("[name=AppointmentStartTime]").val(),
                AppointmentEndTime: pageContent.find("[name=AppointmentEndTime]").val(),
                Content: pageContent.find("[name=Content]").val()
            });
            getPageData = getAppointment;
            if (ajaxObj.setUrl("/Appointment/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        window.Appointment.getDetail = function (id) {
            params.setDataParams({
                ID: id
            });
            var result = ajaxObj.setUrl("/Appointment/GetDetail").setParaObj(params).getData();
            this.setEntry(result.Data);
            return this;
        }
        window.Appointment.remove = function (id) {
            layer.confirm("您确定删除本条数据？", function () {
                params.setDataParams({
                    ID: id,
                    CustomerID: custid
                });
                getPageData = getAppointment;
                var result = ajaxObj.setUrl("/Appointment/Delete").setIsUpdateTrue().setParaObj(params).getData();
                return this;
            }, function () {
                layer.msg("您已取消操作~", { icon: 1 });
            });
        }
        var verify = function () {
            var date = pageContent.find("[name=AppointmentDate]").val();
            if (Common.StrUtils.isNullOrEmpty(date)) {
                layer.msg("请选择预约日期！", { icon: 5 });
                return false;
            }
            return true;
        };
        form.render();
    };
    var openFunc = function (layero, pageContent, data) {
        // 填充页面模版到页面容器中
        fillData(pageContent.find(".appointment-form"), pageContent.find(".appointment-tmp"), data);
        BeginTime = new TimePlugin(pageContent.find("[name=AppointmentStartTime]").val(), ".timePluginBegin").setSelectFunc(function (time, timer) {
            pageContent.find("[name=AppointmentStartTime]").val(time);
        }).init();
        EndTime = new TimePlugin(pageContent.find("[name=AppointmentEndTime]").val(), ".timePluginEnd").setSelectFunc(function (time, timer) {
            pageContent.find("[name=AppointmentEndTime]").val(time);
        }).init();
        form.render();
    }
    getPageData = getAppointment;
    var MakeBeginTime = null, MakeEndTime = null,
    // 填充页面模版到页面容器中
    emptyFormData = { HospitalName: "", AppointmentDate: "", AppointmentStartTime: "", AppointmentEndTime: "", Content: "" };
    Model.init("Appointment", "/Customer/Appointment", "添加咨询预约", "AppointmentAdd", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });
})(window, jQuery);