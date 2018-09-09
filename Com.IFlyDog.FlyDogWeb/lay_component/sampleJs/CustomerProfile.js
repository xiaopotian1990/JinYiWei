$(function () {
    layui.use(["form", "element", "layer", "laydate", "upload"], function () {
        var element = layui.element(), laydate = layui.laydate(), layer = layui.layer;
        window.form = layui.form();
        window.element = layui.element()
        var upload = layui.upload(opration);
        //检测添加图片中-选择项目类型
        form.on("select(addImgType)",
        function (data) {
            //清空选项
            $("input[name=ChargeID]").val("");
            $("select[name=SymptomID]").val("");
            //咨询
            if (data.value === 1 || data.value === "1") {
                $(".add-custmer-img .layui-form-item.imgaddSymptom_div").removeClass("hide");
                $(".add-custmer-img .imgaddCharge_div").addClass("hide");
                return false;
            } else if (data.value === 2 ||
                    data.value === "2" ||
                    data.value === 3 ||
                    data.value === "3" ||
                    data.value === 4 ||
                    data.value === "4"
            ) {
                $(".add-custmer-img .imgaddCharge_div").removeClass("hide");
                $(".add-custmer-img .layui-form-item.imgaddSymptom_div").addClass("hide");
                return false;
            } else {
                $(".add-custmer-img .imgaddCharge_div").addClass("hide");
                $(".add-custmer-img .imgaddSymptom_div").addClass("hide");
                return false;
            }
        });
        //添加照片提交
        form.on("submit(formDemo)", function (data) {
            var custom = data.field;
            if (Common.StrUtils.isNullOrEmpty(custom.CreateTime)) {
                layer.msg("日期不能为空！", {icon:2});
                return false;
            }
            var Images = [];
            $.each($("input[name=BigImage]"), function (index, item) {
                Images.push({
                    BigImage: $(item).val(),
                    ReducedImage: $(item).attr("data-reduceImage")
                });
            });
            var paraObj = {
                data: {
                    CustomerID: custom.CustomerID,
                    CreateTime: custom.CreateTime,
                    Type: custom.Type,
                    ChargeID: custom.ChargeID,
                    SymptomID: custom.SymptomID,
                    Images: Images,
                    Remark: custom.Remark
                }
            };
            ajaxObj = {
                url: "/Photo/BatchAdd",
                paraObj: paraObj
            };
            var result = dataFunc(ajaxObj);
            layer.msg(result.Message);
            if (result.ResultType === 0) {
                closeLayer(this);
                getcustomerPoto();
            }
            return false;
        });
        form.render();
    });
    getCustomerDetail();
    //获取顾客ID
    var hicutomerId = parent.layui.tab({ elem: ".admin-nav-card" }).title().split(":")[2];
    //给页面隐藏域一个顾客ID
    $("input[name='hicutomerId']").attr("hicutomerId", hicutomerId);
    $(".addcustomerimgCategory").attr("lay-filter", "imgCategory");
    //取消微信
    $(".tb-cu-detail-info tr:eq(0) > td:eq(1)").click(function () {
        layer.confirm("是否清除当前顾客的微信绑定!",
            {
                btn: ["确定", "取消"] //按钮
            }, function () {
                if ($(".tb-cu-info tr:eq(3) > td:eq(1)").text() === "暂无绑定") {
                    layer.msg("当前用户暂无绑定微信,无需清除!");
                    return false;
                } else {
                    var ajaxObj = {
                        url: "/Consult/ConsultDelete",
                        paraObj: {
                            data: { CustomerID: hicutomerId }
                        }
                    };
                    var resule = dataFunc(ajaxObj);
                    if (resule.ResultType === 0) {
                        layer.msg("清除成功!", { icon: 1 });
                        getConsultInfo();
                    } else {
                        layer.msg(resule.Message);
                    }
                }
                return false;
            }, function () {
                layer.msg("已取消!");
            });
    });
    //修改顾客信息
    $(".tb-cu-info").prev().children("span").children("i").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("EditCustomerInfo", "", function () {
            openParam("添加顾客咨询", "", "", "cuoPerationComm");
            var ajaxObj = {
                url: "/CustomerProfile/GetCustomerInfoUpdate/",
                paraObj: {
                    data: {
                        CustomerID: hicutomerId
                    }
                }
            };
            var result = dataFunc(ajaxObj);
            if (result.ResultType === 0) {

                $("input[name='editCustomerName']").val(result.Data.CustomerName);
                $("select[name='editCustomercGender']").find("option[value=" + result.Data.Gender + "]").attr("selected", true);

                $("select[name='editCustomerProvince']").find("option[value=" + result.Data.ProvinceID + "]").attr("selected", true);
                getCity();
                $("select[name='editCustomerCity']").find("option[value=" + result.Data.CityID + "]").attr("selected", true);

                $("input[name='editCustomerAddress']").val(result.Data.Address);
                $("input[name='editCustomerBirthday']").val(result.Data.Birthday.substring(0, 10));
                $("input[name='editCustomerQQ']").val(result.Data.QQ);
                $(".Custom1Name").text(result.Data.Custom1Name + ":");
                $("input[name='editCustomerCustom1']").val(result.Data.Custom1);

                $(".Custom2Name").text(result.Data.Custom2Name + ":");
                $("input[name='editCustomerCustom2']").val(result.Data.Custom2);

                $(".Custom3Name").text(result.Data.Custom3Name + ":");
                $("input[name='editCustomerCustom3']").val(result.Data.Custom3);

                $(".Custom4Name").text(result.Data.Custom4Name + ":");
                $("input[name='editCustomerCustom4']").val(result.Data.Custom4);

                $(".Custom5Name").text(result.Data.Custom5Name + ":");
                $("input[name='editCustomerCustom5']").val(result.Data.Custom5);

                $(".Custom6Name").text(result.Data.Custom6Name + ":");
                $("input[name='editCustomerCustom6']").val(result.Data.Custom6);

                $(".Custom7Name").text(result.Data.Custom7Name + ":");
                $("input[name='editCustomerCustom7']").val(result.Data.Custom7);

                $(".Custom8Name").text(result.Data.Custom8Name + ":");
                $("input[name='editCustomerCustom8']").val(result.Data.Custom8);

                $(".Custom9Name").text(result.Data.Custom9Name + ":");
                $("input[name='editCustomerCustom9']").val(result.Data.Custom9);

                $(".Custom10Name").text(result.Data.Custom10Name + ":");
                $("input[name='editCustomerCustom10']").val(result.Data.Custom10);


            } else {
                layer.msg(result.Message, { icon: 5 });
            }
            form.render();

        });

    });
    //修改顾客渠道
    $(".tb-cu-info").on("click", "tr:eq(0)>td:eq(7)", function () {
        openPop("", ".edit-customer-channel", "修改客户渠道");
        form.render();
    });
    //渠道提交修改
    $(".edit-customer-channel").on("click", ".edit-Channel-btn", function () {
        var channelId = $(".EditCustomerChannel").val();
        if (channelId === "-1") {
            layer.msg("请正确选择!");
            return false;
        }
        customerinfo("ChannelID", channelId, "UpdateChannel");
    });
    //修改店家
    $(".tb-cu-info").on("click", "tr:eq(2)>td:eq(1)", function () {
        openPop("", ".edit-customer-StoreCategory", "修改店家");
        form.render();
    });
    //店家提交
    $(".edit-customer-StoreCategory").on("click", ".edit-StoreCategory-btn", function () {
        var storeCategoryId = $(".EditCustomerStoreCategory").val();
        if (storeCategoryId === "-1") {
            layer.msg("请正确选择!");
            return false;
        }
        customerinfo("StoreID", storeCategoryId, "UpdateStore");
    });
    //修改客户主咨询项目
    $(".tb-cu-info").on("click", "tr:eq(1)>td:eq(1)", function () {
        openPop("", ".edit-customer-Symptom", "修改主咨询项目");
        form.render();
    });
    //修改客户主咨询项目提交
    $(".edit-customer-Symptom").on("click", ".edit-Symptom-btn", function () {
        var symptomId = $(".EditCustomerSymptom").val();
        if (symptomId === "-1") {
            layer.msg("请正确选择!");
            return false;
        }
        customerinfo("CurrentConsultSymptomID", symptomId, "UpdateCurrentConsultSymptom");
    });
    //修改顾客联系方式
    $(".tb-cu-info").on("click", "tr:eq(0)>td:eq(9)", function () {
        var moText = $(this).text().substring(1);
        customerEditInfo("请输入顾客主联系方式!", moText, "Mobile", "UpdateMobile");
    });
    //修改顾客备用联系方式  
    $(".tb-cu-info").on("click", "tr:eq(0)>td:eq(11)", function () {
        var moText = $(this).text().substring(1);
        customerEditInfo("请输入顾客备用联系方式!", moText, "MobileBackup", "UpdateMobileBackup");
    });
    //激活券
    $(".add-actCoupon-btn").click(function () {
        layer.prompt({ title: "请输入券代码验证!", formType: 0 },
            function (accode, index) {
                var url = "/CustomerProfile/GetActiveCoupon";
                var paraObj = { data: { code: accode } };
                var result = ajaxProcess(url, paraObj);
                if (result.ResultType === 0) {
                    var isRepetition = "", isEfficacy = "";
                    if (result.Data.IsRepetition === 0) {
                        isRepetition = "不可重复使用";
                    }
                    if (result.Data.IsRepetition === 1) {
                        isRepetition = "可重复使用";
                    }
                    if (result.Data.IsRepetition === 2) {
                        isRepetition = "有效";
                    }
                    if (result.Data.IsRepetition === 3) {
                        isRepetition = "失效";
                    }

                    if (result.Data.IsEfficacy === 0) {
                        isEfficacy = "不可重复使用";
                    }
                    if (result.Data.IsEfficacy === 1) {
                        isEfficacy = "可重复使用";
                    }
                    if (result.Data.IsEfficacy === 2) {
                        isEfficacy = "有效";
                    }
                    if (result.Data.IsEfficacy === 3) {
                        isEfficacy = "失效";
                    }
                    var testaaa = $("#cuoPerationComm")
                        .load("AddCoupon",
                            "",
                            function () {
                                openParam("激活顾客券", "AddActiveCoupon", "", "cuoPerationComm");
                                $("input").css("border", "none");
                                $("input").css("color", "#FF5722");
                                $("input[name=Code]").val(result.Data.Code);
                                $("input[name=ActiveName]").val(result.Data.ActiveName);
                                $("input[name=CouponCategory]").val(result.Data.CouponCategoryName);
                                $("input[name=Amount]").val(result.Data.Amount);
                                $("input[name=CreateDate]").val(result.Data.CreateDate === "" ? "" : result.Data.CreateDate.slice(0, 10));
                                $("input[name=Expiration]").val(result.Data.Expiration === "" ? "" : result.Data.Expiration.slice(0, 10));
                                $("input[name=IsRepetition]").val(isRepetition);
                                $("input[name=IsEfficacy]").val(isEfficacy);
                                $("input[name=CouponID]").val(result.Data.CouponID);
                            });
                } else {
                    layer.msg(result.Message);
                    layer.close(index);
                }
            });
    });
    //添加图片
    $(".si-add-img").on("click", function () {
        openPop("", ".add-custmer-img", "添加客户图片");
        $(".add-custmer-img .imgaddSymptom_div").addClass("hide");
        $(".add-custmer-img .imgaddCharge_div").addClass("hide");
        form.render();
        $(".add-custmer-img input[name=CustomerID]").val(hicutomerId);
        $("div[name=ReducedImageInfo]").empty();
        $("div[name=ImageInfoDiv]").empty();
        $("div[name=percentProgress]").hide();
        progress(10);
    });
    //添加图片弹窗中-选择收费项目
    $(".add-custmer-img").on("click", "input[name='addCharge']", function () {
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择收费项目");
        }
    });
    //添加图片中-添加项目-查询按钮
    $(".charge-pop").on("click", ".search-charge-btn", function () {
        getChargeData();
    });
    // 添加按钮
    $(".model-btns.add").on("click", "span", function () {
        var _this = $(this), model = _this.data("model"),
            action = _this.data("action");
        if (!model) return false;
        if (!window[model]) {
            loadScript("/lay_component/sampleJs/customer_operation/" + model + ".js",
                function () {
                    action ? window[model] && window[model].setSubmitUrl(action).useEmptyEntry().openPop() : window[model] && window[model].useEmptyEntry().openPop();
                });
        } else {
            action ? window[model].useEmptyEntry().setSubmitUrl(action).openPop() : window[model].useEmptyEntry().openPop();
        }
    });
    // 修改按钮
    $(".model-btns.edit").on("click", ".btn-edit", function () {
        var _this = $(this), model = _this.data("model");
        if (!model) return false;
        if (!window[model]) {
            loadScript("/lay_component/sampleJs/customer_operation/" + model + ".js",
                function () {
                    window[model] && window[model].setSubmitUrl(_this.data("action")).unUseEmptyEntry().getDetail(_this.parent().data("id")).openPop();
                });
        } else {
            window[model].setSubmitUrl(_this.data("action")).unUseEmptyEntry().getDetail(_this.parent().data("id")).openPop();
        }
    });
    // 查看按钮
    $(".model-btns.edit").on("click", ".btn-look", function () {
        var _this = $(this), model = _this.data("model");
        if (!model) return false;
        if (!window[model]) {
            loadScript("/lay_component/sampleJs/customer_operation/" + model + ".js",
                function () {
                    window[model] && window[model].unUseEmptyEntry().look(_this.parent().data("id")).openPop();
                });
        } else {
            window[model].unUseEmptyEntry().look(_this.parent().data("id")).openPop();
        }
    });
    // 删除按钮
    $(".model-btns.edit").on("click", ".btn-rmv", function () {
        var _this = $(this), model = _this.data("model");
        if (!model) return false;
        if (!window[model]) {
            loadScript("/lay_component/sampleJs/customer_operation/" + model + ".js",
                function () {
                    window[model] && window[model].remove(_this.parent().data("id"));
                });
        } else {
            window[model].remove(_this.parent().data("id"));
        }
    });
    //添加标签
    $(".add-Tags-btn").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddTags", "", function () {
            openParam("添加顾客标签", "AddTags", "", "cuoPerationComm");
            form.render();
            var ajaxObj = {
                url: "/CustomerProfile/GetCustomerTageGroup/",
                paraObj: {
                    data: { customerId: hicutomerId }
                }
            };
            var result1 = dataFunc(ajaxObj);
            if (result1.ResultType === 0) {
                if ($(".tag-st p:eq(2) >span").length > 0) {

                    $.each(result1.Data.Tags, function (i, item) {
                        $(".add-tags").append("<span class='layui-btn-small layui-btn m-bt-10' tagsId=" + item.TagID + ">" + item.TagName + "<i class='layui-icon'>&#xe640;</i></span>");
                    });
                }

            } else {
                layer.msg(result1.Message, { icon: 5 });
            }
        });
    });
    //添加佣金
    $(".add-custmer-storeCommi").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddStoreCommission", "", function () {
            openParam("添加店铺佣金", "AddStoreCommission", "", "cuoPerationComm");
            form.render();
        });
    });
    //添加咨询
    $(".add-custmer-consult").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddConsult", "", function () {
            openParam("添加顾客咨询", "ConsultAdd", "", "cuoPerationComm");
            form.render();
        });
    });
    //编辑咨询
    $(".tb-cus-zxqk").on("click", ".editConsult", function () {
        var edId = $(this).parent().attr("edId");

        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddConsult", "", function () {
            openParam("编辑顾客咨询", "ConsultUpdate", "", "cuoPerationComm");
            $("input[name=consultId]").attr("consultid", edId);
            var ajaxObj = {
                url: "/Consult/GetConsultDetail/",
                paraObj: {
                    data: { consultid: edId }
                }
            };
            var result1 = dataFunc(ajaxObj);

            if (result1.ResultType === 0) {
                $(".AddcustomerTool").find("option[value=" + result1.Data.ToolID + "]").attr("selected", true);
                $.each(result1.Data.Symptoms, function (i, item) {
                    $(".add-selected").append("<span class='layui-btn-small layui-btn m-bt-10' symptomid=" + item.SymptomID + ">" + item.SymptomName + "<i class='layui-icon'>&#xe640;</i></span>");
                });
                $(".AddRemark").val(result1.Data.Content);
            } else {
                layer.msg(result1.Message, { icon: 5 });
            }

            form.render();
        });
    });
    //未成交
    $(".custo-Failture-cli").click(function () { getFailture(); });
    //添加未成交
    $(".add-failture-btn").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddFailture", "", function () {
            openParam("添加未成交", "AddFailture", "", "cuoPerationComm");
            form.render();
        });
    });
    //编辑未成交
    $(".tb-failture").on("click", ".edit", function () {
        var edId = $(this).parent().attr("edId");

        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddFailture", "",
                function () {
                    openParam("编辑未成交", "UpdateFailture", "", "cuoPerationComm");
                    $("input[name=IDComm]").val(edId);

                    var ajaxObj = {
                        url: "/Failture/GetDetail/",
                        paraObj: {
                            data: { faid: edId }
                        }
                    };
                    var result1 = dataFunc(ajaxObj);
                    if (result1.ResultType === 0) {
                        $(".AddFailtureCategory")
                            .find("option[value=" + result1.Data.CategoryID + "]")
                            .attr("selected", true);
                        $("textarea[name='FailtureRemark']").val(result1.Data.Content);
                    } else {
                        layer.msg(result1.Message, { icon: 5 });
                    }
                    form.render();
                });
    });
    //删除未成交
    $(".tb-failture").on("click", ".delete", function () {
        var delId = $(this).parent().attr("edId");
        layer.confirm("确定删除本条数据吗？",
            { btn: ["确定", "取消"] },
    function () {
        var ajaxObj = {
            url: "/Failture/DeleteFailture",
            paraObj: {
                data: {
                    ID: delId,
                    customerId: hicutomerId
                }
            }
        };
        if (dataFunc(ajaxObj).ResultType === 0) {
            layer.msg("删除成功!", { icon: 1 });
            getFailture();
        }
    },
    function () {
        layer.msg("已取消!", { icon: 1 });
    });
    });
    //添加订单
    $(".add-order-sp").click(function () {
        $("#opration").attr("data-type", "");
        $("#opration").attr("data-orderId", "");
        $("#cuoPerationComm").html("");
        $("#cuoPerationComm").load("AddOrder", "", function () {
            openDynamicParam("添加顾客订单", "AddOrder", "", "cuoPerationComm", "90%", "90%");
            form.render();
        });

    });
    //添加预收款
    $(".add-deposit-btn").click(function () {
        $("#cuoPerationComm").load("AddDeposit", "", function () {
            openDynamicParam("添加预收款订单", "", "", "cuoPerationComm", "90%", "90%");
            $(".order-deposit-charge-table").empty();
        });
    });
    //咨询情况
    $(".custo-getConsult-cli").click(function () { getConsultInfo(); });
    //删除咨询
    $(".tb-cus-zxqk").on("click", ".delConsult", function () {
        var delId = $(this).parent().attr("edId");
        layer.confirm("确定删除本条数据吗？", { btn: ["确定", "取消"] }, function () {
            var ajaxObj = {
                url: "/Consult/ConsultDelete",
                paraObj: {
                    data: {
                        ID: delId,
                        customerId: hicutomerId
                    }
                }
            };
            var resu = dataFunc(ajaxObj);
            if (resu.ResultType === 0) {
                layer.msg(resu.Message, { icon: 1 });
                getConsultInfo();
            } else {
                layer.msg(resu.Message);
            }
        }, function () {
            layer.msg("已取消!", { icon: 1 });
        });
    });
    //预约情况
    $(".custo-appointment-cli").click(function () { getAppointment(); });
    //预约情况
    $(".custo-callBack-cli").click(function () { getCallBack(); });
    //负责用户
    $(".custo-owinerShip-cli").click(function () { getOwinerShip(); });
    //病例
    $(".custo-medicalRecord-cli").click(function () { getMedicalRecord(); });
    // 历史详细按钮
    $(".history-detail").click(function () {
        var type = $(this).data("type");//owinerShip-detail-table
        params.setDataParams({ "customerId": custid, type: type });
        var dotEle = [{ container: ".owinerShip-detail-table", tmp: ".owinerShip-detail-tmp" }];
        ajaxObj.setUrl("/CustomerProfile/GetOwinerShipHistory").setParaObj(params).setDotEle(dotEle).getData();
        openPop("", ".owinerShip-detail-pop", (type == 1 ? "开发人员" : "咨询人员") + "历史详细");
    });
    //预约编辑
    $(".tb-Appo-Appointment").on("click", ".app-edit-btn", function () { });
    //上门情况
    $(".custo-Visit-cli").click(function () { getVisit(); });
    //选择收费项目确认事件
    $(".charge-pop").on("click", "span[name=chargeConfirmBtn]", function () {
        var val = $("input[name=radio]:checked").val();
        var name = $("input[name=radio]:checked").attr("title");
        $("#addCharge").val(name);
        $("input[name=ChargeID]").val(val);
        closeLayer(this);
    });
    //添加关系
    $(".add-custmer-Relation").click(function () {
        $("#cuoPerationComm").html(""); //使用之前，清空div中数据
        var testaaa = $("#cuoPerationComm").load("AddRelation", "", function () {
            openParam("添加顾客咨询", "AddRelation", "", "cuoPerationComm");
            form.render();
        });
    });
    //客户间关系
    $(".custo-rela-cli").click(function () {
        getRelationt();
    });
    //删除关系
    $(".tb-rela").on("click", ".delrelation", function () {
        var relaId = $(this).parent().attr("rela");
        layer.confirm("确定删除本条数据吗？",
            { btn: ["确定", "取消"] },
        function () {
            var ajaxObj = {
                url: "/CustomerProfile/DeleteRelation",
                paraObj: {
                    data: {
                        ID: relaId,
                        CustomerID: hicutomerId
                    }
                }
            };
            var resu = dataFunc(ajaxObj);
            if (resu.ResultType === 0) {
                layer.msg("删除成功!", { icon: 1 });
                getRelationt();
            } else {
                layer.msg(resu.Message, { icon: 1 });
            }
        },
        function () {
            layer.msg("已取消!", { icon: 1 });
        });
    });
    //照片
    $(".custo-poto-cli").click(function () { getcustomerPoto(); });
    //查看大图
    $(".layui-tab-content").on("click", ".max-img", function () {
        var imgEle = $(this).prev("img");
        layer.open({
            type: 1,
            title: false,
            closeBtn: 1,
            area: "",
            skin: "layui-layer-nobg", //没有背景色
            shadeClose: false,
            content: imgEle,
            cancel: function (index, layero) {
                layer.close(index);
            }
        });
    });
    //照片删除
    $(".layui-tab-content").on("click", ".del-img", function () {
        var potoId = $(this).parent().attr("potoId");
        layer.confirm("确定删除本条数据吗？", { btn: ["确定", "取消"] }, function () {
            var ajaxObj = {
                url: "/Photo/DeletePoto",
                paraObj: {
                    data: {
                        ID: potoId,
                        CustomerID: hicutomerId
                    }
                }
            };
            if (dataFunc(ajaxObj).ResultType === 0) {
                layer.msg("删除成功!", { icon: 1 });
                getcustomerPoto();
            }
        }, function () {
            layer.msg("已取消!", { icon: 1 });
        });
    });
    //查看佣金
    $(".custo-record-cli").click(function () { getCommission(); });
    //删除佣金
    $(".tb-storeCommi").on("click", ".del-storeCommi", function () {
        var sId = $(this).parent().attr("storeId");
        layer.confirm("确定删除本条数据吗？",
            { btn: ["确定", "取消"] },
    function () {
        var ajaxObj = {
            url: "/CustomerProfile/DeleteStoreCommission",
            paraObj: {
                data: {
                    ID: sId,
                    CustomerID: hicutomerId
                }
            }
        };
        if (dataFunc(ajaxObj).ResultType === 0) {
            layer.msg("删除成功!", { icon: 1 });
            getCommission();
        }
    },
    function () {
        layer.msg("已取消!", { icon: 1 });
    });
    });
    //查询账户
    $(".custo-money-cli").click(function () {
        getcustomerMoney();
    });

    //查询消费项目
    $(".custo-charges-cli").click(function () {
        getCharges();
    });
    //划扣记录
    $(".custo-oper-cli").click(function () {
        getOperation();
    });
    //划扣记录-删除
    $(".tb-Opera").on("click", ".btn-rmv", function () {
        var operId = $(this).parent().attr("operId");
        layer.confirm("确定删除本条数据吗？", { btn: ["确定", "取消"] },
         function () {
             var ajaxObj = {
                 url: "/DeparDesk/DeleteOperation",
                 paraObj: {
                     data: {
                         ID: operId
                     }
                 }
             };
             var resu = dataFunc(ajaxObj).ResultType;
             if (resu === 0) {
                 layer.msg("删除成功!", { icon: 1 });
                 getOperation();
             } else {
                 layer.msg(resu.Message, { icon: 1 });
             }
         },
         function () {
             layer.msg("已取消!", { icon: 1 });
         });
    });
    //划扣编辑
    $(".tb-Opera").on("click", ".btn-edit", function () {
        var operId = $(this).parent().attr("operId");
        openPop("", ".edit-depar-pop", "编辑划扣");
        $(".layui-fontColor-6").css("border", "none");

        var ajaxObj = {
            url: "/DeparDesk/GetOperationDetail",
            paraObj: {
                data: {
                    id: operId
                }
            }
        };
        var result = dataFunc(ajaxObj);

        if (result.ResultType === 0) {
            $("input[name=cuName]").val(result.Data.CustomerName);
            $("input[name=cuChargeName]").val(result.Data.ChargeName);
            $("input[name=cuId]").val(result.Data.CustomerID);
            $("input[name=cuOperId]").val(result.Data.OperationID);
            $("input[name=cuNum]").val(result.Data.Num);

            if (result.Data.OperatorList.length > 0) {
                var tdHs;
                $.each(result.Data.OperatorList, function (i, item) {
                    var tdH = "<td style=\"border: none\" ><span class=\"layui-btn layui-btn-danger fr deltr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
                    tdHs += "<tr>" + "<td><input type=\"hidden\" name=\"editdocId\" editdocId=\"" + item.UserID + "\" /><input class=\"layui-input pointer\" name=\"thseldoc\" value=\"" + item.UserName + "\" readonly=\"readonly\"/></td>" + "<td><select name=\"editPosition\" class=\"sel-position\">" + "<option value=\"" + item.PositionID + "\">" + item.Name + "</option></select></td>" + tdH + "</tr>";
                });
                $(".tb-editDoc").html(tdHs);
                getPosition();
                $.each(result.Data.OperatorList, function (i, item) {
                    $(".sel-position").eq(i).val(item.PositionID);
                });
                form.render();
            }
        }
    });
    //删除当前行
    $(".tb-editDoc").on("click", ".deltr", function () {
        _this = $(this);
        _tr = _this.parent().parent();
        _tr.remove();
    });
    //添加医生跟分工
    $(".add-pay-btn").click(function () {
        _this = $(this);
        _tr = _this.parent().parent();
        var tdH = "<td style=\"border: none\" ><span class=\"layui-btn layui-btn-danger fr deltr \"><i class=\"layui-icon\">&#xe640;</i></span></td>";
        var tdHs = "<tr>" + "<td><input type=\"hidden\" name=\"editdocId\" editdocId=\"\" /><input class=\"layui-input pointer\" name=\"thseldoc\" readonly=\"readonly\"/></td>" +
                        "<td><select name=\"editPosition\" class=\"sel-position\">" + "<option>请选择</option></select></td>" + tdH + "</tr>";

        $(".tb-editDoc").append(tdHs);
        getPosition();
        form.render();
    });
    //选择医生
    $(".tb-editDoc").on("click", "input[name=thseldoc]", function () {
        //获取行号
        var trindex = $(this).parent().parent()[0].rowIndex - 1;
        $("input[name=edittrIndex]").val(trindex);
        openPop("", ".doctor-pop", "选择医生");
        form.render();
    });
    //选择用户
    $(".user-table").on("click", ".present-user", function () {
        var userid = $(this).attr("value");
        var username = $(this).parent().parent().find("td")[1].innerHTML;
        var hidtrIndex = $("input[name=edittrIndex]").val();
        var tr = $(".tb-editDoc").children().eq(hidtrIndex);
        tr.find("input[name=editdocId]").attr("editdocid", userid);
        tr.find("input[name=thseldoc]").val(username);
        if (userid !== "0" || username !== "") {
            layer.msg("选择成功!", { icon: 6 });
            closeLayer(this);
        }
    });
    //查询
    $(".doctor-pop").on("click", ".search-user", function () {
        currentExploitUser();
    });
    //编辑保存
    $(".edit-btn").click(function () {
        var operationerList = [];
        var doctr = $(".tb-editDoc").children();
        $.each(doctr, function (i, item) {
            item = $(item);
            var obj = {};
            obj["UserID"] = item.find($("input[name=editdocId]")).attr("editdocid");
            obj["PositionID"] = item.find($(".sel-position")).val();
            operationerList.push(obj);
        });

        var paraObj = {};
        paraObj.data = {
            ID: $("input[name=cuOperId]").val(),
            CustomerID: $("input[name=cuId]").val(),
            OperationerList: operationerList
        };
        var url = "/DeparDesk/UpdateOperation";
        var result = ajaxProcess(url, paraObj);
        if (result) {
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1, time: 1000 });
                closeLayer(this);
                getOperation();
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        };
        return false;
    });
    //查看订单
    $(".custo-order-cli").click(function () {
        getOrder();
        //查看订单
        $("span[name=seeOrder]").on("click", function () {
            $span = $(this);
            var orderId = $span.parent().parent().attr("orderId");
            $("#opration").attr("data-type", "see");
            $("#opration").attr("data-orderId", orderId);
            $("#cuoPerationComm").html("");
            $("#cuoPerationComm").load("AddOrder", "", function () {
                openDynamicParam("查看顾客订单", "AddOrder", "", "cuoPerationComm", "65%", "65%");
                $("#saveOrder").hide();
                $(".layui-elem-field.fl").css("width", "100%");
                $(".layui-elem-field.fr").hide();
                form.render();
            });
        });
        //修改订单
        $("span[name=modifyOrder]").click(function () {
            $span = $(this);
            var orderId = $span.parent().parent().attr("orderId");
            $("#opration").attr("data-type", "modify");
            $("#opration").attr("data-orderId", orderId);
            $("#cuoPerationComm").html("");
            $("#cuoPerationComm").load("AddOrder", "", function () {
                openDynamicParam("修改顾客订单", "AddOrder", "", "cuoPerationComm", "90%", "90%");
                form.render();
            });
        });

        //查看预收款订单 
        $("span[name=seeDeposit]").click(function () {
            $span = $(this);
            var orderId = $span.parent().parent().attr("orderId");
            $("#opration").attr("data-type", "see");
            $("#opration").attr("data-orderId", orderId);
            $("#cuoPerationComm").html("");
            $("#cuoPerationComm").load("AddDeposit", "", function () {
                openDynamicParam("查看顾客预收款", "AddDeposit", "", "cuoPerationComm", "60%", "65%");
                $("#saveDepositOrder").hide();
                $(".layui-elem-field.fl").css("width", "100%");
                $(".layui-elem-field.fr").hide();
                form.render();
            });
        });

        //查看退项目单
        //        $("span[name=seeorderBk]").click(function () {
        //            $span = $(this);
        //            var orderId = $span.parent().parent().attr("orderId");
        //            $("#opration").attr("data-type", "see");
        //            $("#opration").attr("data-orderId", orderId);
        //            $("#cuoPerationComm").html("");
        //            $("#cuoPerationComm").load("AddRebateDeposit", "", function () {
        //                openDynamicParam("查看顾客预收款", "AddRebateDeposit", "", "cuoPerationComm", "60%", "70%");
        //                $(".add-submit").hide();
        //                $(".layui-elem-field.fl").css("width", "100%");
        //                $(".layui-elem-field.fr").hide();
        //                form.render();
        //            });
        //        });
        //     

    });
    //取消订单
    $(".tb-or-Orders,.tb-or-InpOrders,.tb-or-DepositOrder,.tb-or-bkOrder,.tb-or-drebateOrder").on("click", ".cancel-btn", function () {
        var orderType = $(this).attr("order-Type");
        var orderId = $(this).parent().parent().attr("orderId");
        var customerid = $("input[name=hicutomerId]").attr("hicutomerid");

        layer.confirm("您确定要删除当前订单吗？", {
            btn: ["确定", "取消"] //按钮
        }, function () {
            if (orderType === "1") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteOrder").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getOrder();
                }
            }
            if (orderType === "3") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteDeposit").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getOrder();
                }
            }
            if (orderType === "4") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteBackOrder").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getOrder();
                }
            }
            if (orderType === "5") {
                params.setDataParams({
                    CustomerID: customerid,
                    OrderID: orderId
                });
                if (ajaxObj.setUrl("/CashierDesk/DeleteDepositRebateOrderr").setParaObj(params).setIsUpdateTrue().getData().ResultType === 0) {
                    getOrder();
                }
            }
            return false;
        }, function () {
        });
    });
    //查看退预收款订单
    $(".tb-or-drebateOrder").on("click", "span[name=seeBackDeposit]", function () {
        var orderId = $(this).parent().parent().attr("orderId");
        var customerid = $("input[name=hicutomerId]").attr("hicutomerid");
        var opt = {};
        opt.title = "查看退预收款订单";
        opt.url = "";
        opt.popEle = ".see-DepositRebate";
        opt.func = function () { };
        opt.area = ["70%", "80%"];
        openPopWithOpt(opt);

        var url = "/CashierDesk/GetDepositRebateDetail";
        var paraObj = {
            data: {
                customerId: customerid,
                orderId: orderId
            }
        };
        var result = ajaxProcess(url, paraObj);

        if (result != null && result.ResultType === 0) {
            $("input[name=toAmount]").val(result.Data.Amount);
            $("input[name=FinalPrice]").val(result.Data.Amount);

            $("input[name=CustomerID]").val($("input[name=custid]").val());
            $("input[name=CreateTime]").val(result.Data.CreateTime);
            $("input[name=CreateUserName]").val(result.Data.CreateUserName);
            $("input[name=Amount]").val(result.Data.Amount);
            $("input[name=Remark]").val(result.Data.Remark);

            fillData($(".DepositRebate-tbody"), $(".DepositRebate-tmp"), result.Data.Details);
            fillData($(".DepositRebateC-tbody"), $(".DepositRebateC-tmp"), result.Data.CouponDetails);
        } else {
            layer.msg("暂无!");
        }
        form.render();
    });
    //查看退项目单
    $(".tb-or-bkOrder").on("click", "span[name=seeorderBk]", function () {
        var orderId = $(this).parent().parent().attr("orderId");
        var customerid = $("input[name=hicutomerId]").attr("hicutomerid");
        var opt = {};
        opt.title = "查看退项目单";
        opt.url = "";
        opt.popEle = ".see-BackOrder";
        opt.func = function () { };
        opt.area = ["70%", "70%"];
        openPopWithOpt(opt);
        var url = "/CashierDesk/GetBackOrderDetail";
        var paraObj = {
            data: {
                customerId: customerid,
                orderId: orderId
            }
        };
        var result = ajaxProcess(url, paraObj);
        if (result != null && result.ResultType === 0) {
            $("input[name=toAmount]").val(result.Data.Amount);
            $("input[name=FinalPrice]").val(result.Data.Amount);
            $("input[name=CustomerID]").val(customerid);
            $("input[name=CreateTime]").val(result.Data.CreateTime);
            $("input[name=CreateUserName]").val(result.Data.CreateUserName);
            $("input[name=Amount]").val(result.Data.Amount);
            $("input[name=Remark]").val(result.Data.Remark);
            fillData($(".BackOrder-tbody"), $(".BackOrder-tmp"), result.Data.Details);
        } else {
            layer.msg("暂无!");
        }
    });
});
var custid = $("input[name='hicutomerId']").attr("hicutomerId");

//上传图片滚动条
function progress(num) {
    var elem = $("div[name=percentProgress] .layui-progress-bar");
    elem.attr("style", "width: " + num + "%;");
    elem.find("span").text(num + "%");
}
/*上传图片方法*/
var opration = {
    url: "/Picture/BatchUploadImage",
    method: "post",
    before: function (input) {
        $("div[name=percentProgress]").show();
        setTimeout(progress(50), 10000);
    },
    success: function (res, input) {
        if (res.ResultType === 0) {
            progress(100);
            var bigImage = "", reducedImageHtml = "";
            $.each(res.Data,
                function (index, item) {
                    bigImage += "<input type=\"hidden\" name=\"BigImage\" data-reduceImage=\"" + item.ReducedImage + "\" value=\"" + item.BigImage + "\"/>";
                    reducedImageHtml += "<img src=\"" + item.ReducedImage + "\"/>";
                });
            $("div[name=ImageInfoDiv]").append(bigImage);
            $("div[name=ReducedImageInfo]").append(reducedImageHtml);
            layer.msg(res.Message);
            getcustomerPoto();
        } else {
            layer.msg(res.Message);
        }
    }
};
/*获取客户ID*/
var custid = parent.layui.tab({ elem: ".admin-nav-card" }).title().split(":")[2];
var htmlnu = "<blockquote class='layui-elem-quote'>暂无数据</blockquote>";
//客户详细信息
var getCustomerDetail = function () {
    var url = "/CustomerProfile/GetCustomerDetail";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj).Data;
    //顾客信息-输出
    var interText = doT.template($(".cuinfo-temp").text());
    $(".tb-cu-info").html(interText(result));
    $(".tb-cu-info td:even").attr("class", "layui-bg-gray");
    //顾客自定义详细-输出
    var interText1 = doT.template($(".cuinfo-detail-temp").text());
    $(".tb-cu-detail-info").html(interText1(result));
    $(".tb-cu-detail-info td:even").attr("class", "layui-bg-gray");
    //顾客概要-输出
    var interText2 = doT.template($(".cu-gy-temp").text());
    $(".fi-cugy-info").html(interText2(result));

};
//咨询情况
var getConsultInfo = function () {
    var dotEle = [{ container: ".tb-cus-zxqk", tmp: ".cu-consult-temp" }];
    ajaxObj.setUrl("/Consult/GetConsult").setParaObj(params.setDataParam("customerId", custid)).setDotEle(dotEle).getData();
};
//划扣情况
var getOperation = function () {
    var dotEle = [{ container: ".tb-Opera", tmp: ".opera-temp" }];
    ajaxObj.setUrl("/CustomerProfile/GetOperation").setParaObj(params.setDataParam("customerId", custid)).setDotEle(dotEle).getData();
};
var getCallBack = function () {
    var dotEle = [{ container: ".callBack-table", tmp: ".callBack-tmp" }];
    ajaxObj.setUrl("/CustomerProfile/GetCallbackByCustomerId").setParaObj(params.setDataParam("customerId", custid)).setDotEle(dotEle).getData();
}
var getOwinerShip = function () {
    ajaxObj.setUrl("/CustomerProfile/GetOwinerShip").setParaObj(params.setDataParam("customerId", custid)).setDataCallBack(function (data) {
        data = data.Data;
        fillData(".owinerShip-table[type=1]", ".owinerShip-tmp", data.Exploits);
        fillData(".owinerShip-table[type=2]", ".owinerShip-tmp", data.Managers);
    }).getData();
}
var getMedicalRecord = function () {
    var dotEle = [{ container: ".medicalRecord-table", tmp: ".medicalRecord-tmp" }];
    ajaxObj.setUrl("/CustomerMedicalRecord/CustomerMedicalRecordGet").setParaObj(params.setDataParam("CustomerID", custid)).setDotEle(dotEle).getData();
}
//预约情况
var getAppointment = function () {
    var url = "/CustomerProfile/GetAppointment";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);
    //预约情况-咨询预约
    if (result.ResultType === 0) {
        if (result.Data.AppointmentList.length > 0) {
            var interText = doT.template($(".Appo-Appointment-temp").text());
            $(".tb-Appo-Appointment").html(interText(result.Data.AppointmentList));
        } else {
            $(".tb-Appo-Appointment-null").html(htmlnu);
        }
    }
    //治疗预约详细
    if (result.ResultType === 0) {
        if (result.Data.TreatList.length > 0) {
            var interText1 = doT.template($(".Appo-Treat-temp").text());
            $(".tb-Appoi-Treat").html(interText1(result.Data.TreatList));
        } else {
            $(".tb-Appoi-Treat-null").html(htmlnu);
        }
    }
    //手术预约
    if (result.ResultType === 0) {
        if (result.Data.SurgeryList.length > 0) {
            var interText2 = doT.template($(".Appo-Surgery-temp").text());
            $(".tb-Appo-Surgery").html(interText2(result.Data.SurgeryList));
        } else {
            $(".tb-Appo-Surgery-null").html(htmlnu);
        }
    }
};
//上门情况
var getVisit = function () {
    var url = "/CustomerProfile/GetVisit";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        if (result.Data !== null) {
            //上门情况-上门情况
            var interText = doT.template($(".visitCase-visit-temp").text());
            $(".tb-visitCase-visit").html(interText(result.Data));
        } else {
            $(".tb-visitCase-visit-null").html(htmlnu);
        }
        //    //分诊记录 
        if (result.Data !== null) {
            var interText1 = doT.template($(".visitCase-Triage-temp").text());
            $(".tb-visitCase-Triage").html(interText1(result.Data));
        } else {
            $(".tb-visitCase-Triage-null").html(htmlnu);
        }
        //    //部门接待记录 
        if (result.Data !== null) {
            var interText2 = doT.template($(".visitCase-DeptVisit-temp").text());
            $(".tb-visitCase-DeptVisit").html(interText2(result.Data));
        } else {
            $(".tb-visitCase-DeptVisit-null").html(htmlnu);

        }
        //    //住院记录 
        if (result.Data !== null) {
            var interText3 = doT.template($(".visitCase-Inpatient-temp").text());
            $(".tb-visitCase-Inpatient").html(interText3(result.Data));
        } else {
            $(".tb-visitCase-Inpatient-null").html(htmlnu);
        }

    } else {
        layer.msg(result.Message);
    }
};
//未成交情况
var getFailture = function () {
    var url = "/Failture/GetFailtureByCustomerId";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        if (result.Data !== null) {
            var interText = doT.template($(".failture-tmp").text());
            $(".tb-failture").html(interText(result.Data));
        }
    }
};
//选择收费项目弹窗
var getChargeData = function () {
    var ajaxObj = {
        url: "/Charge/ChargeGetData",
        paraObj: {
            data: {
                PinYin: $("[name=smartProductPinYin]").val(),
                Name: $("[name=smartProductNmae]").val()
            }
        },
        dataCallBack: function (data) {
            var tmpHtml = $(doT.template($(".charge-tmp").text())(data.Data));
            $(".charge-selected")
                .find("span")
                .each(function (i, item) {
                    item = $(item);
                    tmpHtml = tmpHtml.not("[chargeId= " + item.attr("chargeId") + "]");
                });
            $(".charge-table").html(tmpHtml);
        }
    };
    dataFunc(ajaxObj);
    return true;
};
//顾客编辑信息
var customerEditInfo = function (title, moText, fieldType, courl) {

    layer.prompt({ title: title, value: moText },
        function (val, index) {

            var data = { "CustomerID": custid };
            data[fieldType] = val;

            var ajaxObj = {
                url: "/CustomerProfile/" + courl,
                paraObj: {
                    data: data
                }
            };
            var result = dataFunc(ajaxObj);
            if (result.ResultType === 0) {
                layer.msg(result.Message, { icon: 1 });
                layer.close(index);
                getCustomerDetail();
            } else {
                layer.msg(result.Message, { icon: 5 });
            }
        });
};
//编辑顾客渠道信息
var customerinfo = function (fieldType, val, courl) {
    var data = { "CustomerID": custid };
    data[fieldType] = val;
    var ajaxObj = {
        url: "/CustomerProfile/" + courl,
        paraObj: {
            data: data
        }
    };
    var result = dataFunc(ajaxObj);
    if (result.ResultType === 0) {
        layer.msg(result.Message, { icon: 1 });
        closeLayer(".layui-form-item");
        getCustomerDetail();
    } else {
        layer.msg(result.Message, { icon: 5 });
    }
};
//查看顾客照片
var getcustomerPoto = function () {
    var url = "/Photo/GetPhotoByCustomerId";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);

    //咨询照片-
    if (result.ResultType === 0) {
        if (result.Data.Consult !== null) {
            var interText = doT.template($(".poto-consul-tmp").text());
            $(".tb-poto-Consult").html(interText(result.Data.Consult));
            $(".tb-poto-Consultnull").empty();
        } else {
            $(".tb-poto-Consultnull").html(htmlnu);
        }
    }
    //治疗前
    if (result.ResultType === 0) {
        if (result.Data.Before.length > 0) {
            var interText1 = doT.template($(".poto-baU-tmp").text());
            $(".tb-poto-Before").html(interText1(result.Data.Before));
            $(".tb-poto-Beforetnull").empty();
        } else {
            $(".tb-poto-Beforetnull").html(htmlnu);
        }
    }
    //治疗中
    if (result.ResultType === 0) {
        if (result.Data.Under.length > 0) {
            var interText2 = doT.template($(".poto-baU-tmp").text());
            $(".tb-poto-Under").html(interText2(result.Data.Under));
            $(".tb-poto-Undernull").empty();
        } else {
            $(".tb-poto-Undernull").html(htmlnu);
        }
    }
    //治疗后
    if (result.ResultType === 0) {
        if (result.Data.After.length > 0) {
            var interText3 = doT.template($(".poto-consul-tmp").text());
            $(".tb-poto-After").html(interText3(result.Data.After));
            $(".tb-poto-Afternull").empty();
        } else {
            $(".tb-poto-Afternull").html(htmlnu);
        }
    }
    //其他
    if (result.ResultType === 0) {
        if (result.Data.Other.length > 0) {
            var interText4 = doT.template($(".poto-other-tmp").text());
            $(".tb-poto-Other").html(interText4(result.Data.Other));
            $(".tb-poto-Othernull").empty();
        } else {
            $(".tb-poto-Othernull").html(htmlnu);
        }
    }
};
//查询佣金
var getCommission = function () {
    var url = "/CustomerProfile/GetCustomerStoreCommission";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);

    if (result.ResultType === 0) {
        if (result.Data !== null) {
            var interText = doT.template($(".storeCommission-temp").text());
            $(".tb-storeCommi").html(interText(result.Data));
        } else {
            $(".tb-storeCommi-null").html(htmlnu);
        }
    }
};
//查询订单
var getOrder = function () {
    var url = "/CustomerProfile/GetOrders";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);
    //订单信息
    if (result.ResultType === 0) {
        if (result.Data.Orders.length > 0) {
            var interText = doT.template($(".order-Orders-temp").text());
            $(".tb-or-Orders").html(interText(result.Data.Orders));
        } else {
            $(".tb-or-Ordersnull").html(htmlnu);
        }

    }
    //住院单
    if (result.ResultType === 0) {
        if (result.Data.InpatientOrders.length > 0) {
            var interText1 = doT.template($(".order-Orders-temp").text());
            $(".tb-or-InpOrders").html(interText1(result.Data.InpatientOrders));
        } else {
            $(".tb-or-InpOrdersnull").html(htmlnu);
        }
    }
    //预收款
    if (result.ResultType === 0) {
        if (result.Data.DepositOrders.length > 0) {
            var interText2 = doT.template($(".order-deOrder-temp").text());
            $(".tb-or-DepositOrder").html(interText2(result.Data.DepositOrders));
        } else {
            $(".tb-or-DepositOrdernull").html(htmlnu);
        }
    }
    //退项目订单
    if (result.ResultType === 0) {
        if (result.Data.BackOrders.length > 0) {
            var interText3 = doT.template($(".order-bkOrder-temp").text());
            $(".tb-or-bkOrder").html(interText3(result.Data.BackOrders));
        } else {
            $(".tb-or-bkOrdernull").html(htmlnu);
        }
    }
    //退预收款单
    if (result.ResultType === 0) {
        if (result.Data.DepositRebateOrders.length > 0) {
            var interText4 = doT.template($(".order-drebateOrder-temp").text());
            $(".tb-or-drebateOrder").html(interText4(result.Data.DepositRebateOrders));
        } else {
            $(".tb-or-drebateOrdernull").html(htmlnu);
        }
    }
};
//客户关系
var getRelationt = function () {
    var url = "/CustomerProfile/GetRelation";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);

    if (result.ResultType === 0) {
        if (result.Data !== null) {
            var interText = doT.template($(".relation-tmp").text());
            $(".tb-rela").html(interText(result.Data));
        }
    } else {
        layer.msg(result.Message);
    }
};
//查询账户
var getcustomerMoney = function () {
    var url = "/CustomerProfile/GetMoney";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);

    if (result.ResultType === 0) {
        if (result.Data !== null) {
            var interText = doT.template($(".money-temp").text());
            $(".tb-money").html(interText(result.Data));
            $(".tb-money td:even ").attr("class", "layui-bg-gray");
        }
        if (result.Data.Deposits !== null) {
            var interText1 = doT.template($(".money-deposit-temp").text());
            $(".tb-money-deposit").html(interText1(result.Data.Deposits));
        }
        if (result.Data.Coupons !== null) {
            var interText2 = doT.template($(".money-coupon-temp").text());
            $(".tb-money-coupon").html(interText2(result.Data.Coupons));
        }
        if (result.Data.OverDueCoupons !== null) {
            var interText3 = doT.template($(".money-overcoupon-temp").text());
            $(".tb-money-overcoupon").html(interText3(result.Data.OverDueCoupons));
        }
        if (result.Data.CouponChanges !== null) {
            var interText4 = doT.template($(".money-coupchan-temp").text());
            $(".tb-money-coupchan").html(interText4(result.Data.CouponChanges));
        }
        if (result.Data.PointChanges !== null) {
            var interText5 = doT.template($(".money-poinchan-temp").text());
            $(".tb-money-poichan").html(interText5(result.Data.PointChanges));
        }
        if (result.Data.DepositChanges !== null) {
            var interText6 = doT.template($(".money-depochan-temp").text());
            $(".tb-money-depochan").html(interText6(result.Data.DepositChanges));
        }
        if (result.Data.CommissionChanges !== null) {
            var interText7 = doT.template($(".money-commichan-temp").text());
            $(".tb-money-commichange").html(interText7(result.Data.CommissionChanges));
        }
    } else {
        layer.msg(result.Message);
    }
};
//查询消费项目
var getCharges = function () {
    var url = "/CustomerProfile/GetCharges";
    var paraObj = { data: { customerId: custid } };
    var result = ajaxProcess(url, paraObj);
    if (result.ResultType === 0) {
        if (result.Data !== null) {
            var interText = doT.template($(".cu-charges-temp").text());
            $(".tb-charges").html(interText(result.Data));
            $(".tb-charges td:even ").attr("class", "layui-bg-gray");
        }
        if (result.Data.Charges !== null) {
            var interText1 = doT.template($(".expense-charges-temp").text());
            $(".tb-expense-charges").html(interText1(result.Data.Charges));
        }
    } else {
        layer.msg(result.Message);
    }
};
//查询岗位 
var getPosition = function () {
    var url = "/Position/PositionGet";
    var paraObj = {};
    var result = ajaxProcess(url, paraObj);

    var interText = doT.template($(".position-tmp").text());
    $(".sel-position").html(interText(result.Data));
};
//选择用户
var currentExploitUser = function () {
    var url = "/SmartUser/SmartUserGet";
    var paraObj = {
        data: {
            Name: $("input[name='addcuuserNmae']").val(),
            DeptId: $(".smartaddcuDept").val(),
            PageSize: 999,
            PageNum: 1
        }
    };
    var result = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($(".user-tmp").text());
    var html = "<blockquote class='layui-elem-quote'>没有该用户信息!请核实!</blockquote>";
    if (result.length <= 0) {
        $(".user-info").html(html);
        $(".user-table").empty();
    } else {
        $(".user-table").html(interText(result));
        $(".user-info").html("");
    }

}