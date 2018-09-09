(function (win, $) {
    var successFunc = function (pageContent) {
        //提交
        pageContent.on("click", ".Treat.add-submit", function () {
            if (!verify()) {
                return false;
            };
            var chargeIDS = [];
            pageContent.find(".info-charge-table").find("tr").each(function (i, item) {
                item = $(item);
                chargeIDS.push(item.attr("chargeId"));
            });
            params.setDataParams({
                CustomerID: custid,
                HospitalID: pageContent.find(".treatHospital").val(),
                AppointmentDate: pageContent.find("[name=AppointmentDate]").val(),
                AppointmentStartTime: pageContent.find("[name=AppointmentStartTime]").val(),
                AppointmentEndTime: pageContent.find("[name=AppointmentEndTime]").val(),
                UserID: pageContent.find("[name=UserID]").val(),
                ChargeID: pageContent.find("[name=ChargeID]").val(),
                ChargeName: pageContent.find("[name=ChargeName]").val(),
                Remark: pageContent.find("[name=Remark]").val()
            });
            if (ajaxObj.setUrl("/Treat/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).getData().ResultType == 0) {
                closeLayer(this);
            }
        });
        pageContent.on("click", ".charge-rmv", function () {
            $(this).parents("tr").remove();
        });
        // 弹窗选择项目按钮
        pageContent.on("click", ".buy.add-btn", function () {
            if (getChargeData()) {
                openPop("", $(".charge-pop-subset"), "选择收费项目");
            }
        });
        // 弹窗选择所有项目按钮
        pageContent.on("click", ".all.add-btn", function () {
            if (getChargeData(true)) {
                openPop("", $(".charge-pop-subset"), "选择收费项目");
            }
        });
        pageContent.on("click", "[name=UserName]", function () {
            if (Common.StrUtils.isNullOrEmpty(pageContent.find("[name=AppointmentDate]").val())) {
                layer.msg("请选择日期！", { icon: 2 })
                return false;
            }
            UserInfo.setUrl("/User/GetSSYYUsers").unUsePopParams().setParams({ hospitalID: pageContent.find(".treatHospital").val(), date: pageContent.find("[name=AppointmentDate]").val() }).setConfimFunc(function (userInfo) {
                pageContent.find("[name=UserID]").val(userInfo.id);
                pageContent.find("[name=UserName]").val(userInfo.name);
            }).setOpenSuccess(function (popContent) {
                var popTr = popContent.find(".user-table").parent().find("tr");
                popTr.find("td,th").hide();
                popTr.find("td:eq(0),td:eq(2),th:eq(0),th:eq(2)").show();
                popTr.click(function () {
                    event.stopPropagation();
                });
            }).openPop();
        });
        var getChargeData = function (isUseAllCharge) {
            params.setDataParams({
                customerID: custid,
                PinYin: $(".customer-pops").find("[name=smartProductPinYin]").val(),
                Name: $(".customer-pops").find("[name=smartProductNmae]").val()
            });
            ajaxObj.setUrl(isUseAllCharge ? "/Charge/ChargeGetData" : "/Order/GetAppointCharges").setDataCallBack(function (data) {
                var tmpHtml = $(doT.template(pageContent.find(".charge-tmp").text())(data.Data));
                pageContent.find(".info-charge-table").find("tr").each(function (i, item) {
                    item = $(item);
                    tmpHtml = tmpHtml.not("[chargeId= " + item.attr("chargeId") + "]");
                });
                tmpHtml.find("[type=checkbox]").click(function () {
                    $(this).parents(".charge-table").find(":checked").not(this).prop("checked", false);
                    event.stopPropagation();
                });
                $(".customer-pops").find(".charge-table").html(tmpHtml);
            }).setParaObj(params).getData();
            return true;
        }
        $(".customer-pops").on("click", ".chargeSubset.treat", function () {
            //fillData(".info-charge", ".info-charge-tmp", []);
            $(this).parents(".charge-pop-subset").find(":checked").each(function (i, item) {
                item = $(item);
                pageContent.find("[name=ChargeID]").val(item.val());
                pageContent.find("[name=ChargeName]").val(item.attr("title"));
            });
            closeLayer(this);
        });
        pageContent.find(".search-btn").click(function () {
            getChargeData(true)
        });
        window.Treat.getDetail = function (id) {
            params.setDataParams({
                ID: id
            });
            var result = ajaxObj.setUrl("/Treat/GetDetail").setParaObj(params).getData();
            this.setEntry(result.Data);
            return this;
        }
        window.Treat.remove = function (id) {
            layer.confirm("确认删除？", function () {
                params.setDataParams({
                    ID: id,
                    CustomerID: custid
                });
                var result = ajaxObj.setUrl("/Treat/Delete").setIsUpdateTrue().setParaObj(params).getData();
            }, function () {
                layer.msg("已取消操作~", { icon: 1 });
            });
            return this;
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
        fillData(pageContent.find(".treat-form"), pageContent.find(".treat-tmp"), data);
        BeginTime = new TimePlugin(pageContent.find("[name=AppointmentStartTime]").val(), ".timePluginBegin").setSelectFunc(function (time, timer) {
            pageContent.find("[name=AppointmentStartTime]").val(time);
        }).init();
        EndTime = new TimePlugin(pageContent.find("[name=AppointmentEndTime]").val(), ".timePluginEnd").setSelectFunc(function (time, timer) {
            pageContent.find("[name=AppointmentEndTime]").val(time);
        }).init();
        var chargeEle = pageContent.find(".charge-pop-subset");
        chargeEle.length == 1 && $(".customer-pops").find(".charge-pop-subset").remove() && chargeEle.appendTo($(".customer-pops"));
        $(".customer-pops").find(".chargeSubset").addClass("treat").removeClass("surgery");
        form.render();
    }
    getPageData = getAppointment;
    var MakeBeginTime = null, MakeEndTime = null,
    // 填充页面模版到页面容器中
    emptyFormData = { HospitalName: "", AppointmentDate: "", AppointmentStartTime: "", AppointmentEndTime: "", UserID: "", UserName: "", ChargeID: "", ChargeName: "", Remark: "" };
    Model.init("Treat", "/Customer/Treat", "添加治疗预约", "Add", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });
})(window, jQuery);