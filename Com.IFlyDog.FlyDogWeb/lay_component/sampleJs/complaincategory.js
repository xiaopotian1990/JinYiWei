var i = 1;
//显示
$("#complainCategoryhtml")
    .ready(function() {
        var infoFunc = function() {

            var url = "/ComplainCategory/ComplainCategoryGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#complainCategoryInfo_template").text());
            $(".layui-field-box").html(interText(data.Data));
        };
        infoFunc();
    });

//添加信息
$("#complainCategoryhtml")
    .ready(function() {
        $(".layui-btn.layui-btn-small")
            .on("click",
                function() {

                    var innerText = doT.template($("#showcomplainCategoryAddInfo_template").text());

                    var contentData = $("#showcomplainCategoryAddInfo_div").html(innerText());

                    //页面层
                    layer.open({
                        type: 1,
                        title: "添加信息",
                        //skin: 'layui-layer-rim', //加上边框
                        skin: 'layerbackground_color',
                        area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                        shade: [0.8, '#B3B3B3', false],
                        closeBtn: 1,
                        Boolean: false,
                        shadeClose: false, //点击遮罩关闭
                        content: contentData,
                        success: function(layero, index) {

                            $(".layui-btn.layui-btn-normal.dept_commit")
                                .on("click",
                                    function() {
                                        var addName = $("#complainCategoryinfoAddName").val();
                                        var addRemark = $("#complainCategoryinfoAddRemark").val();

                                        if (addRemark.length > 50) {
                                            layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                            return false;
                                        }

                                        var realData = {};
                                        realData.Name = addName;

                                        realData.Remark = addRemark;

                                        var paraObj = {};
                                        paraObj.data = realData;

                                        var url = "/ComplainCategory/ComplainAdd";
                                        var data = ajaxProcess(url, paraObj);
                                        if (data) {
                                            if (parseInt(data.ResultType) === 0) { //请求成功返回
                                                $("#showcomplainCategoryAddInfo_div").html("");
                                                var message = data.Message;
                                                //关闭窗口
                                                layer.close(index);
                                                //提示消息
                                                layer.msg(message, { icon: 6 });
                                                //刷新主页面数据.
                                                setTimeout(function() {
                                                        location.reload();
                                                    },
                                                    1500);

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
                                    function() {
                                        $("#showcomplainCategoryAddInfo_div").html("");
                                        layer.close(index);
                                    });
                            //右上角关闭回调
                        },
                        cancel: function(index) {
                            $("#showcomplainCategoryAddInfo_div").html("");
                            layer.close(index);
                            return false;
                        }
                    });
                });
    });
//修改部门
$("#complainCategoryhtml")
    .ready(function() {
        $(".commonEdit")
            .on("click",
                function() {
                    var showEditDialog = $(this);
                    var infoId = $(showEditDialog).attr("infoiEditId");
                    var url = "/ComplainCategory/ComplainGetByID";

                    var dto = new Object();
                    dto.ID = infoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showcomplainCategoryEditInfo_template").text());

                        var contentData = $("#showcomplainCategoryEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改信息",
                            skin: 'layerbackground_color',
                            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false],
                            closeBtn: 1,
                            Boolean: false,
                            shadeClose: false, //点击遮罩关闭
                            content: contentData,
                            success: function(layero, index1) {

                                //确认提交
                                $(".layui-btn.layui-btn-normal.dept_commit")
                                    .on("click",
                                        function() {

                                            var id = $("#complainCategoryinfoEditID").val();
                                            var infoeditName = $("#complainCategorytinfoEditName").val();
                                            var infoeditRemark = $("#complainCategoryinfoEditRemark").val();

                                            if (infoeditRemark.length > 50) {
                                                layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = infoeditName;

                                            dto.Remark = infoeditRemark;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/ComplainCategory/ComplainlEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showcomplainCategoryEditInfo_div").html("");
                                                    var message = data.Message;
                                                    //关闭窗口
                                                    layer.close(index1);
                                                    //提示信息
                                                    layer.msg(message, { icon: 6 });
                                                    //刷新主页面数据.
                                                    setTimeout(function() {
                                                            location.reload();
                                                        },
                                                        1500);;
                                                } else {
                                                    //请求成功返回,但是后台出现错误
                                                    layer.msg(data.Message, { icon: 5 });
                                                }
                                            }
                                            return false;
                                        });
                                //取消关闭按钮
                                $(document)
                                    .on("click",
                                        ".dept_close",
                                        function() {
                                            $("#showcomplainCategoryEditInfo_div").html("");
                                            layer.close(index1);
                                        });
                                //右上角关闭回调
                            },
                            cancel: function(index1) {
                                $("#showcomplainCategoryEditInfo_div").html("");

                                layer.close(index1);
                                return false;
                            }
                        });
                    }

                });
    });

//停用数据
$("#complainCategoryhtml")
    .ready(function() {
        ///使用停用
        $($(".EditStopBut"))
            .on("click",
                function() {

                    var showEditStop = $(this);
                    var infoStatus = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");

                    if (infoStatus === "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.ComplainID = infoStopID;
                                realData.Status = infoStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;


                                var url = "/ComplainCategory/ComplainDisable";
                                var data = ajaxProcess(url, paraObj);
                                layer.msg('已成功停用!', { icon: 1 });
                                setTimeout(function () {
                                    location.reload();
                                }, 1500);

                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        var message = data.Message;
                                        //关闭窗口

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

                            },
                            function() {
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
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.ComplainID = infoStopID;
                                realData.Status = infoStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/ComplainCategory/ComplainDisable";
                                var data = ajaxProcess(url, paraObj);

                                layer.msg('已成功停用!', { icon: 1 });
                                setTimeout(function () {
                                    location.reload();
                                }, 1500);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        var message = data.Message;
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
                            },
                            function() {
                                layer.msg('已经取消此操作',
                                {
                                    icon: 6
                                });
                            });
                    }
                });

    });