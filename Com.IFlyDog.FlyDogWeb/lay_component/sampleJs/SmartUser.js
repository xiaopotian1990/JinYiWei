(function (win, $) {
    $(function () {
        layui.use(['form', 'layer'], function () {
            var layer = layui.layer;
            window.form = layui.form();
            // 医院下拉框选中事件
            form.on("select(Hospital)", function () {
                getDeptFunc();
            });
        });
    });
    // 获取用户弹窗dom
    var getuserPageContentFunc = function () {
        var url = "/SmartUser/UserInfo";
        var paraObj = { handleAs: "html" };
        var result = ajaxProcess(url, paraObj);
        return result;
    }
    // 创建userInfo对象，用于外部调用
    var userInfo = new userPop()();
    function userPop() {
        var pageSize = 15; // 每页条数
        var pageIndex = 1; // 当前页
        var isUseHospital = false; // 是否使用医院弹窗
        var userPageContent = ""; // 获取到的用户弹窗页面html
        var popContent = ""; // 用户弹窗html内容
        var confimFunc = null; // 确认按钮回调函数
        var openSuccess = null; // 打开弹窗回调函数 参数是弹窗内容部分的html  dom
        var tmpContent = ""; // 用户数据模版dom
        var isSingleSelect = true; // 是否单选
        return function () {
            // 获取用户弹窗dom
            userPageContent = $(getuserPageContentFunc());
            userPageContent.find("#smartHospital").attr("lay-filter", "Hospital");
            // 用户弹窗html内容
            popContent = userPageContent.find(".user-info-content").hide();
            // 把弹窗dom加入到页面中
            $("body").append(popContent);
            // 用户数据模版的dom
            tmpContent = userPageContent.find(".dot-tmp");
            return {
                setUrl: function (url) { userInfoUrl = url; return this; },
                // 使用医院下拉框
                useHospital: function () {
                    isUseHospital = true;
                    isUsePopParams = true;
                    getDeptFunc(paramsData);
                    return this;
                },
                // 不使用医院下拉框
                notUseHospital: function () {
                    isUseHospital = false;
                    getDeptFunc(paramsData);
                    return this;
                },
                // 打开用户列表弹窗
                openPop: function () {
                    // 判断是否使用医院下拉框，不是用则隐藏
                    isUseHospital ? $$.find(".hospital-dom").show() : $$.find(".hospital-dom").hide()
                    isUsePopParams ? $$.find(".layui-form").show() : $$.find(".layui-form").hide()
                    layer.open({
                        type: 1,
                        title: "选择用户",
                        area: ["55%;min-width:680px;max-width:800px", "75%;min-height:500px;max-height:600px"],
                        shade: [0.8, '#B3B3B3', false],
                        closeBtn: 1,
                        shadeClose: false, //点击遮罩关闭
                        content: popContent,
                        success: function (layero, index) {
                            getUserInfo();
                            Common.StrUtils.isNullOrEmpty(openSuccess) ? "" : (typeof openSuccess == 'function' ? openSuccess(popContent) : "");
                            form.render();
                            // 搜索按钮
                            $$.find(".search-user").bind("click", function () {
                                getUserInfo();
                            });
                            $$.find(".close-layer").bind("click", function () {
                                var forms = $(this).parents(".layui-layer").find("form");
                                forms.find("[type=hidden]").val("");
                                if (forms.length > 0) {
                                    forms[0].reset();
                                }
                                closeLayer();
                            });
                            // 确认按钮
                            $$.find(".confim-user").bind("click", function () {
                                userChecked = null;
                                if (isSingleSelect) {
                                    userChecked = {
                                        id: $(".user-table").find("[name=userDept]:checked").val(),
                                        name: $(".user-table").find("[name=userDept]:checked").data("name"),
                                        account: $(".user-table").find("[name=userDept]:checked").data("account")
                                    };
                                } else {
                                    userChecked = [];
                                    $(".user-table").find("[name=userDept]:checked").each(function (i, item) {
                                        item = $(item);
                                        userChecked.push({
                                            id: item.val(),
                                            name: item.data("name"),
                                            account: item.data("account")
                                        });
                                    });
                                }
                                Common.StrUtils.isNullOrEmpty(confimFunc) ? "" : (typeof confimFunc == 'function' ? confimFunc(userChecked) : "");
                                openSuccess
                                layer.close(index);
                                closeLayer(layero);
                            });
                            // 右上角关闭按钮
                            layero.on("click", ".close-layer,.layui-layer-close", function () {
                                layer.close(index);
                                closeLayer();
                            });
                            if (isSingleSelect) {
                                // 复选框选中事件
                                layero.on("change", "[name=userDept]", function () {
                                    layero.find("[name=userDept]").not($(this)).prop("checked", false);
                                });
                            }
                        }
                    });
                    return this;
                },
                // 设置是否单选
                setIsSingleSelect: function (flag) {
                    isSingleSelect = flag;
                    return this;
                },
                // 获取是否单选
                getIsSingleSelect: function () {
                    return isSingleSelect;
                },
                // 获取是否使用医院下拉框的值
                getUseHospitalBool: function () {
                    return isUseHospital;
                },
                // 获取用户弹窗dom的jquery对象
                getPopContent: function () {
                    return popContent;
                },
                // 获取用户弹窗数据模版dom的jquery对象
                getTmpContent: function () {
                    return tmpContent;
                },
                // 确认按钮点击回调函数，参数为选中的用户信息对象
                setConfimFunc: function (func) {
                    confimFunc = func;
                    return this;
                },
                // 分页每页条数
                setPageSize: function (pageSize) {
                    pageSize = pageSize;
                    return this;
                },
                // 分页当前页
                setPageIndex: function (pageIndex) {
                    pageIndex = pageIndex;
                    return this;
                },
                // 设置外部参数
                setParams: function (params) {
                    paramsData = params;
                    return this;
                },
                // 获取外部参数
                getParams: function () {
                    return paramsData;
                },
                // 不使用弹窗内条件
                unUsePopParams: function () {
                    isUsePopParams = false;
                    return this;
                },
                // 使用弹窗内条件
                usePopParams: function () {
                    isUsePopParams = true;
                    return this;
                },
                // 获取是否使用弹窗内条件
                getIsUsePopParams: function () {
                    return isUsePopParams;
                },
                // 设置打开窗口的回调函数
                setOpenSuccess: function (success) {
                    openSuccess = success;
                    return this;
                },
                // 获取打开窗口的回调函数
                getOpenSuccess: function () {
                    return openSuccess;
                }
            };
        }
    };
    // 用户弹窗相关变量，用于方便获取子元素
    var $$ = userInfo.getPopContent();
    var $tmp = userInfo.getTmpContent();
    // 关闭弹窗后执行的事件
    function closeLayer() {
        var forms = $$.find("form");
        forms.find("[type=hidden]").val("");
        if (forms.length > 0) {
            forms[0].reset();
        }
        $$.find(".search-user,.confim-user").unbind("click");
    }
    // 获取部门下拉框数据
    function getDeptFunc() {
        var url = "/Dept/DeptByHospitalIDGet",
            paraObj = {};
        paraObj.data = {
            hospitalID: userInfo.getUseHospitalBool() ? $("#smartHospital").val() : -1
        };
        var result = ajaxProcess(url, paraObj);
        $(".user-info-content").find("#smartDept").html(doT.template($tmp.find(".dept-tmp").text())(result.Data));
        getUserInfo();
        typeof (form) != "undefined" ? form.render() : "";
    }
    var userInfoUrl = "/SmartUser/SmartUserGet";
    var paramsData = {}; // 弹窗的筛选条件以外的参数
    var isUsePopParams = true; // 是否引用弹窗内的条件
    // 获取用户列表方法
    function getUserInfo(param) {
        var url = userInfoUrl,
            paraObj = {};
        paraObj.data = {
            DeptId: Common.StrUtils.isFalseSetEmpty($(".smartDept").val()),
            Name: Common.StrUtils.isFalseSetEmpty($(".user-info-content [name=name]").val()),
            HospitalID: userInfo.getUseHospitalBool() ? $("#smartHospital").val() : "",
            PageSize: userInfo.pageSize,
            PageNum: userInfo.pageIndex
        };
        if (isUsePopParams) {
            if (typeof str == 'string') {
                paramsData = JSON.parse(paramsData);
            }
            paraObj.data = jQuery.extend({}, paraObj.data, paramsData);
        } else {
            paraObj.data = paramsData;
        }
        var result = ajaxProcess(url, paraObj);
        $(".user-info-content").find(".user-table").html(doT.template($tmp.find(".user-tmp").text())(result.Data));
    }
    win.UserInfo = userInfo;
})(window, jQuery);