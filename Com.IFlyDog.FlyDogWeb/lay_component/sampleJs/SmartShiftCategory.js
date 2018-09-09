var i = 1;

layui.use("form", function () {
    window.form = layui.form();
});

//开始展示班次信息数据
$("#smartShiftCategoryHtml").ready(
    function () {
        var getSmartShiftCategoreyFunc = function () {
            var url = "/SmartShiftCategory/SmartShiftCategoryGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($("#smartShiftCategory_template").text());
            $(".layui-field-box").html(interText(data));
        };
        getSmartShiftCategoreyFunc();
    });

//添加班次信息
$("#smartShiftCategoryHtml").ready(
    function () {
        //为添加班次信息注册点击事件
        $(".layui-btn.layui-btn-small").on("click", function () {
            var innerText = doT.template($("#showSmartShiftCategoryAddInfo_template").text());//得到添加班次模板
            var contentData = $("#showSmartShiftCategoryAddInfo_div").html(innerText());//回填数据
            //打开一个添加班次弹层页
            layer.open({
                type: 1,
                title: "添加班次信息",
                //skin: 'layui-layer-rim', //加上边框
                skin: 'layerbackground_color',
                area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                shade: [0.8, '#B3B3B3', false],
                closeBtn: 1,
                Boolean: false,
                shadeClose: false, //点击遮罩关闭
                content: contentData,
                success: function (layero, index) {
                    $(".layui-btn.layui-btn-normal.smartShiftAdd_commit")
                        .on("click",
                            function () {
                                var smartShiftInfoEditName = $("#smartShiftInfoEditName").val();//班次名称
                                var shiftCategoryType = $("#shiftCategoryType").val();//工作，非工作
                                var shiftState = $("#shiftState").val();//启用/停用状态

                                if (smartShiftInfoEditName == "" || undefined || null) {
                                    layer.msg('班次名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (smartShiftInfoEditName.length > 20 || smartShiftInfoEditName === "") {
                                    layer.msg('班次名称不可超出50字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                var realData = {};
                                realData.Name = smartShiftInfoEditName;
                                realData.ShiftCategoryType = shiftCategoryType;
                                realData.ShiftState = shiftState;

                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartShiftCategory/SmartShiftCategoryAdd";
                                var data = ajaxProcess(url, paraObj);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        $("#showSmartShiftCategoryAddInfo_div").html("");
                                        var message = data.Message;
                                        //关闭窗口
                                        layer.close(index);
                                        //提示消息
                                        layer.msg(message, { icon: 6 });
                                        //刷新主页面数据.
                                        setTimeout(function () {
                                            location.reload();
                                        }, 1500);

                                    } else {
                                        //请求成功返回,但是后台出现错误
                                        layer.msg(data.Message, { icon: 5 });
                                    }
                                }
                                return false;
                            });
                    //取消并关闭按钮
                    $(".layui-btn.layui-btn-danger.dept_close")
                        .on("click",
                            function () {
                                $("#showSmartShiftCategoryAddInfo_div").html("");
                                layer.close(index);
                            });
                    form.render();
                },
                cancel: function (index) {
                    $("#showSmartShiftCategoryAddInfo_div").html("");
                    layer.close(index);
                    return false;
                }
            });
        });
    });

//修改班次信息
$("#smartShiftCategoryHtml")
    .ready(function () {
        $(".layui-btn.layui-btn-mini.smartEdit")
            .on("click",
                function () {
                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("smartInfoiEditId");
                    var url = "/SmartShiftCategory/SmartShiftCategoryEditGet";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showSmartShiftCategoryEditInfo_template").text());

                        var contentData = $("#showSmartShiftCategoryEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改班次信息",
                            skin: 'layerbackground_color',
                            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false],
                            closeBtn: 1,
                            Boolean: false,
                            shadeClose: false, //点击遮罩关闭
                            content: contentData,
                            success: function (layero, index1) {
                                //取消并关闭按钮
                                $(".layui-btn.layui-btn-danger.dept_close")
                                    .on("click",
                                        function () {
                                            $("#showSmartShiftCategoryEditInfo_div").html("");

                                            layer.close(index1);
                                        });

                                //确认提交
                                $(".layui-btn.layui-btn-normal.smartShiftEdit_commit")
                                    .on("click",
                                        function () {

                                            var id = $("#smartEditID").val();
                                            var smartInfoEditName = $("#smartInfoEditName").val();
                                            var shiftCategoryEditType = $("#shiftCategoryEditType").val();
                                            var shiftEditState = $("#shiftEditState").val();


                                            var reg = /^[0-9]*$/;
                                            if (smartInfoEditName == "" || undefined || null) {
                                                layer.msg('班次名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (smartInfoEditName.length > 20 || smartInfoEditName === "") {
                                                layer.msg('班次名称不可超出50字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = smartInfoEditName;
                                            dto.Type = shiftCategoryEditType;
                                            dto.Status = shiftEditState;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SmartShiftCategory/SmartShiftCategorySubmit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showSmartShiftCategoryEditInfo_div").html("");
                                                    var message = data.Message;
                                                    //关闭窗口
                                                    layer.close(index1);
                                                    //提示信息
                                                    layer.msg(message, { icon: 6 });
                                                    //刷新主页面数据.
                                                    setTimeout(function () {
                                                        location.reload();
                                                    }, 1500);
                                                } else {
                                                    //请求成功返回,但是后台出现错误
                                                    layer.msg(data.Message, { icon: 5 });
                                                }
                                            }
                                            return false;
                                        });
                                form.render();
                            }, cancel: function (index1) {
                                $("#showSmartShiftCategoryEditInfo_div").html("");

                                layer.close(index1);
                            }
                        });
                    }

                });
    });

//停用数据
$("#smartShiftCategoryHtml")
    .ready(function () {
        ///使用停用
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function () {

                    var showEditStop = $(this);
                    var deptInfoId = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var deptInfoStopID = $(showEditStopID).attr("stopID");

                    //console.log(deptInfoId);
                    //console.log(deptInfoStopID);

                    if (deptInfoId === "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function () {
                                //给dto赋值
                                var realData = {};
                                realData.ID = deptInfoStopID;
                                realData.Status = deptInfoId;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartShiftCategory/SmartShiftEditStart";
                                var data = ajaxProcess(url, paraObj);
                                layer.msg('已成功停用!', { icon: 1 });
                                setTimeout(function () {
                                    location.reload();
                                }, 1500);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        //刷新主页面数据.
                                        setTimeout(function () {
                                            location.reload();
                                        }, 1500);
                                    } else {
                                        //请求成功返回,但是后台出现错误
                                        layer.msg(data.Message, { icon: 5 });
                                    }
                                }
                                return false;

                            },
                            function () {
                                layer.msg('已经取消此操作',
                                {
                                    icon: 6
                                });
                            });
                    } else {
                        layer.confirm('您确定启用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function () {
                                //给dto赋值
                                var realData = {};
                                realData.ID = deptInfoStopID;
                                realData.Status = deptInfoId;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartShiftCategory/SmartShiftEditStart";
                                var data = ajaxProcess(url, paraObj);

                                layer.msg('已成功启用!', { icon: 1 });
                                setTimeout(function () {
                                    location.reload();
                                }, 1500);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        //刷新主页面数据.
                                        setTimeout(function () {
                                            location.reload();
                                        }, 1500);
                                    } else {
                                        //请求成功返回,但是后台出现错误
                                        layer.msg(data.Message, { icon: 5 });
                                    }
                                }
                                return false;
                            },
                            function () {
                                layer.msg('已经取消此操作',
                                {
                                    icon: 6
                                });
                            });
                    }
                });

    });