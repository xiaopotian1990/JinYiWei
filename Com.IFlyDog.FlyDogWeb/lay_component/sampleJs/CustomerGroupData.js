$(function () {
    layui.use(["form", "layer", "laydate", "laypage", "element"], function () {
        var layer = layui.layer,
            laydate = layui.laydate;
        window.laypage = layui.laypage;
        window.element = layui.element();
        window.form = layui.form();
        getPageData();
        form.on("radio(useScope)", function (data) {
            $("[name=useScope]").siblings("input,select").prop("disabled", true);
            $(data.elem).siblings("input,select").prop("disabled", false);
        });
    });
    $("[name=ItemCategoryID]").prop("disabled", true);
    // 添加按钮
    $(".btn-add").click(function () {
        openPop("CustomerGroupDetailAdd", ".customerGroup-pop", "添加客户组");
    });
    // 添加用户弹窗确认按钮
    $(".customerGroup.submit-btn").click(function () {
        var users = [];
        $.each($("[name=UserListAdd]").val().split("\n"), function (i, item) {
            if (item) {
                users.push({ UserID: item });
            }
        });
        params.setDataParams({ CustomerGroupID: customerGroupId, UserListAdd: users });
        if (ajaxObj.setUrl("/CustomerGroup/" + $(this).parents(".layui-layer").data("url")).setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 清空按钮
    $(".rmv-all").click(function () {
        layer.confirm("确认清空？", function () {
            params.setDataParam("CustomerGroupID", customerGroupId);
            ajaxObj.setUrl("/CustomerGroup/CustomerGroupDetailDeleteAll").setParaObj(params).setIsUpdateTrue().getData();
        }, function () {
            layer.msg("已取消操作！", {icon:1});
        });
    });
    // 批量添加发送短信
    $(".add-msg").click(function () {
        openPop("", ".msg-pop", "批量发送短信");
    });
    // 批量添加发送短信确认按钮
    $(".msg.submit-btn").click(function () {
        params.setDataParams({
            GroupID: getQueryString("customerGroup"),
            Content: $("[name=Content]").val()
        });
        if (ajaxObj.setUrl("/CustomerGroup/CustomerGroupBatchSSMAdd").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 批量添加回访
    $(".add-callBack").click(function () {
        openPop("", ".callBack-pop", "批量添加回访");
    });
    // 批量添加回访弹窗确认按钮
    $(".callBack.submit-btn").click(function () {
        params.setDataParams({
            CustomerGroupID: getQueryString("customerGroup"),
            CallbackCategoryID: $("[name=CallbackCategoryID]").val(),
            CallbackTime: $("[name=CallbackTime]").val(),
            Info: $("[name=Info]").val(),
            CallbackUserID: $("[name=CallbackUserID]").val()
        });
        if (ajaxObj.setUrl("/CustomerGroup/CustomerGroupBatchCallbackAdd").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 回访用户文本框点击
    $("[name=CallbackUserName]").click(function () {
        UserInfo.setConfimFunc(function (userInfo) {
            $("[name=CallbackUserID]").val(userInfo.id);
            $("[name=CallbackUserName]").val(userInfo.name);
        }).openPop();
    });
    // 条件弹窗重置按钮
    $(".btn-reset").click(function () {
        var formEle = $(this).parent().prev("form");
        formEle[0].reset();
        formEle.find(".multiselect,.singleselect,.tableField").empty();
    });
    $(".tableField").on("click", ".remove-btn", function () {
        $(this).parents("tr").remove();
    });
    $(".add-search").hover(function () {
        $(".select-btns").show();
    }, function () {
        $(".select-btns").hide();
    });
    $(".select-btns").hover(function () {
        $(".select-btns").show();
    }, function () {
        $(".select-btns").hide();
    });
    // 添加条件按钮
    $(".select-btns").find("li").click(function () {
        openPop("", "." + $(this).data("condition") + "Condition-pop", $(this).text());
    });
    // 弹窗查询按钮
    $(".search-condition").click(function () {
        var _this = $(this),
            formEle = _this.parent().prev("form"),
            fieldEles = formEle.find("input,select,textarea").not(".layui-select-title input,[type=checkbox],[type=radio]"),
            radios = formEle.find("[type=radio]:checked"),
            selects = formEle.find(".multiselect,.singleselect").length > 0 && fieldEles.add(formEle.find(".multiselect,.singleselect")),
            tableFields = formEle.find(".tableField");
        tableFields.each(function (i, item) {
            item = $(item);
            var arrField = [];
            // 遍历tr
            item.find("tr").each(function (j, tr) {
                tr = $(tr);
                // 获取tr的字段
                var trFields = tr.data("field").split(",");
                var fieldObj = {};
                $.each(trFields, function (k, objField) {
                    fieldObj[objField] = tr.data(objField.toLowerCase());
                });
                arrField.push(fieldObj);
            });
            params.setDataParam(item.attr("name"), arrField);
        });
        fieldEles.each(function (i, item) {
            item = $(item);
            params.setDataParam(item.attr("name"), item.val());
        });
        radios.each(function (i, item) {
            item = $(item);
            params.setDataParam(item.attr("name"), item.val());
        });
        var dotEle = [{ container: ".screen-form", tmp: ".screen-form-tmp" }];
        if (ajaxObj.setUrl("/CustomerGroup/Get" + _this.parents(".search-pop").data("condition") + "ConditionSelect").setParaObj(params).setDotEle(dotEle).getData().ResultType == 0) {
            dotEle = [{ container: ".filters", tmp: ".filters-tmp" }];
            ajaxObj.setUrl("/CustomerGroup/CustomerGroupGet").setDotEle(dotEle).getData()
            closeLayer(this);
        }
        tableFields && tableFields.empty();
        selects && selects.empty();
        openPop("CustomerFilterFiltrateAdd", ".screen-pop", "查询结果");
    });
    // 弹窗选择项目分类按钮
    $("[name=ChargeCategory]:not(disabled)").click(function () {
        var _this = $(this);
        chargeEle = null;
        chargeCategoryEle = {};
        chargeCategoryEle.ID = _this.siblings("[name=ChargeCategoryID]");
        chargeCategoryEle.Name = _this;
        if (getChargeCategoryData()) {
            openPop("", ".chargeCategory-pop", "选择收费项目类型");
        }
    });
    // 弹窗选择项目按钮
    $("[name=Charge]:not(disabled)").click(function () {
        var _this = $(this);
        chargeCategoryEle = null;
        chargeEle = {};
        chargeEle.ID = _this.siblings("[name=ChargeID]");
        chargeEle.Name = _this;
        if (getChargeData()) {
            openPop("", ".charge-pop", "选择收费项目");
        }
    });
    // 项目和项目分类弹窗事件
    $(".charge-pop,.chargeCategory-pop").on("click", "[type=checked]", function () {
        $(this).parents("table").find(":checked").not(this).prop("checked", false);
    })
    $(".charge-pop,.chargeCategory-pop").find(".submit-btn").click(function () {
        var _this = $(this),
            checkedEle = _this.parents(".charge-pop,.chargeCategory-pop").find(":checked");
        if (_this.hasClass("charge") && chargeEle) {
            chargeEle.ID.val(checkedEle.val());
            chargeEle.Name.val(checkedEle.attr("title"));
        } else if (chargeCategoryEle) {
            chargeCategoryEle.ID.val(checkedEle.val());
            chargeCategoryEle.Name.val(checkedEle.attr("title"));
        }
        closeLayer(this);
    })
    // 执行人文本框点击
    $("[name=ExecuteUserName]").click(function () {
        UserInfo.useHospital().setConfimFunc(function (userInfo) {
            $("[name=ExecuteUserID]").val(userInfo.id);
            $("[name=ExecuteUserName]").val(userInfo.name);
        }).openPop();
    });
    // 添加标签
    $(".add-tag").click(function () {
        getTagData();
        openPop("", ".tag-pop", "选择标签");
    });
    // 选择标签提交按钮
    $(".tag.submit-btn").click(function () {
        var tags = [];
        $(".tag-table").find(":checked").each(function (i, item) {
            item = $(item);
            if ($(".tags-table").find("[data-tagid=" + item.val() + "]").length == 0) {
                tags.push({
                    TagID: item.val(),
                    TagName: item.attr("title")
                });
            }
        });
        $(".tags-table").append(doT.template($(".tags-tmp").text())(tags));
        closeLayer(this);
    });
    // 保存结果集
    $(".screen.submit-btn").click(function () {
        var popEle = $(this).parents(".screen-pop");
        var filtrateCustormInfoAdd = [];
        popEle.find(".filtrateCustormInfoAdd-table").find("tr").each(function (i, item) {
            item = $(item);
            filtrateCustormInfoAdd.push({ CustormID: item.data("id") });
        });
        params.setDataParams({
            SaveResult: popEle.find("[name=SaveResult]:checked").val(),
            FilterID: popEle.find("[name=FilterID]").val(),
            FilterName: popEle.find("[name=FilterName]").val(),
            FiltrateCustormInfoAdd: filtrateCustormInfoAdd
        });
        if (ajaxObj.setUrl("/CustomerGroup/CustomerFilterFiltrateAdd").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
    // 合并按钮
    $(".btn-merge").click(function () {
        var dotEle = [{ container: ".filters", tmp: ".filters-tmp" }];
        ajaxObj.setUrl("/CustomerGroup/CustomerGroupGet").setDotEle(dotEle).getData()
        openPop("MergeCustomerFilterAdd", ".merge-pop", "查询结果");
    });
    // 保存合并
    $(".merge.submit-btn").click(function () {
        var popEle = $(this).parents(".merge-pop");
        params.setDataParams({
            CustomerFilterIDOne: popEle.find("[name=CustomerFilterIDOne]").val(),
            CustomerFilterIDTwo: popEle.find("[name=CustomerFilterIDTwo]").val(),
            MergeType: popEle.find("[name=MergeType]").val(),
            SaveResult: popEle.find("[name=SaveResult]:checked").val(),
            FilterID: popEle.find("[name=FilterID]").val(),
            FilterName: popEle.find("[name=FilterName]").val()
        });
        if (ajaxObj.setUrl("/CustomerGroup/MergeCustomerFilterAdd").setParaObj(params).setIsUpdateTrue().getData().ResultType == 0) {
            closeLayer(this);
        }
    });
});
var pageNum = 1,
    pageSize = 15,
    pageTotal = 0,
    customerGroupId = getQueryString("customerGroup"),
    chargeEle = null,
    chargeCategoryEle = null;
var verify = function () { return true; }
//获取用户通知列表数据
var getcustomerGroupData = function () {
    params.setDataParams({ CustomerGroupID: customerGroupId, pageNum: pageNum, pageSize: pageSize });
    var dotEle = [{ container: ".customerGroup-table", tmp: ".customerGroup-tmp" }];
    ajaxObj.setUrl("/CustomerGroup/GetByCustomerGroupID").usePage().setParaObj(params).setDotEle(dotEle).getData();
}
var getChargeData = function () {
    if ($("[name=useScope]").filter("[value=1]:checked").length == 0) {
        return false;
    }
    var dotEle = [{ container: ".charge-table", tmp: ".charge-tmp" }];
    params.setDataParams({
        PinYin: Common.StrUtils.isFalseSetEmpty($("[name=smartProductPinYin]").val()),
        Name: Common.StrUtils.isFalseSetEmpty($("[name=smartProductNmae]").val())
    });
    ajaxObj.setUrl("/Charge/ChargeGetData").setParaObj(params).setDotEle(dotEle).getData();
    return true;
}
var getChargeCategoryData = function () {
    if ($("[name=useScope]").filter("[value=2]:checked").length == 0) {
        return false;
    }
    var dotEle = [{ container: ".charge-table", tmp: ".charge-tmp" }];
    ajaxObj.setUrl("/ChargeCategory/ChargeCategoryGet").setDotEle(dotEle).getData();
    return true;
}
var getTagData = function () {
    var dotEle = [{ container: ".tag-table", tmp: ".tag-tmp" }];
    ajaxObj.setUrl("/SmartTag/TagGetByIsOk").setDotEle(dotEle).getData();
}
var getPageData = getcustomerGroupData;
