//显示
var i = 1;
$("#toolhtml")
    .ready(function () {
        var getinfoFunc = function () {
            var url = "/SmartTool/ToolGet";
            var paratObj = new Object();
            var data = ajaxProcess(url, paratObj);

            var interText = doT.template($("#tool_template").text());
            $(".layui-field-box").html(interText(data.Data));
        };
        getinfoFunc();
    });
//添加内容
$("#toolAddbut")
    .on("click",
        function () {
            //输出模板引擎
            var innerText = doT.template($("#showtoolAddInfo_template").text());
            var contentData = $("#showtoolAddInfo_div").html(innerText());
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
                success: function (layero, index) {
                    //提交按钮
                    $(".dept_commit")
                        .on("click",
                            function () {
                                //获取页面值
                                var addName = $("#toolinfoAddName").val();
                                var addRemark = $("#toolinfoAddRemark").val();
                                if (addRemark.length > 50) {
                                    layer.msg("描述长度不可大过50!抱歉!", { icon: 5, style: "color:red" });
                                    return false;
                                }
                                //获取到页面值,组装DTO赋值
                                var realData = {};
                                realData.Name = addName;
                                realData.Remark = addRemark;

                                //将带有值的DTO放进集合
                                var paraObj = {};
                                paraObj.data = realData;
                                //接口地址
                                var url = "/SmartTool/ToolAdd";

                                //传参 带有数据的DTO跟接口地址
                                var data = ajaxProcess(url, paraObj);
                                //判断接口
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //则请求成功返回消息
                                        //清空添加页面的Div
                                        $("#showtoolAddInfo_div").html("");
                                        var message = data.Message;
                                        //关闭窗口
                                        layer.close(index);
                                        //提示接口返回消息
                                        layer.msg(message, { icon: 6 });
                                        //页面消失后刷新
                                        setTimeout(function () {
                                            location.reload();
                                        },1500);

                                    } else {
                                        layer.msg(data.Message, { icon: 5, style: "color:red" });
                                    }
                                }
                                return false;
                            });

                    //取消关闭
                    $(".dept_close")
                        .on("click",
                            function () {
                                $("#showtoolAddInfo_div").html("");
                                layer.close(index);
                            });
                },
                cancel: function (index) {
                    $("#showtoolAddInfo_div").html("");
                    layer.close(index);
                }
            });
        });
//修改
$(document)
    .on("click",
        ".Edit",
        function () {
            //获取当前
            var showEditDialog = $(this);
            //当前ID
            var infoEditId = $(showEditDialog).attr("infoiEditId");

            //回填数据接口
            var url = "/SmartTool/ToolEditGetByID";

            //回填数据组装DTO
            var dto = new Object();
            dto.ID = infoEditId;


            //集合传入Dto
            var paraObj = new Object();
            paraObj.data = dto;

            //传参数,带接口地址跟集合
            var data = ajaxProcess(url, paraObj);


            //成功类型
            var ResultType = data.ResultType;
            if (parseInt(ResultType) === 0) { //接口返回的消息

                //从模版输出
                var innerText = doT.template($("#showtoolEditInfo_template").text());
                var contentData = $("#showtoolEditInfo_div").html(innerText(data.Data));


                //打开浮窗
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
                    success: function (layero, index1) {
                        //确认提交按钮
                        $(".layui-btn.layui-btn-normal.dept_commit")
                            .on("click",
                                function () {
                                    //获取值
                                    var id = $("#toolinfoEditID").val();
                                    var infoeditName = $("#toolinfoEditName").val();
                                    var infoeditRemark = $("#toolinfoEditRemark").val();

                                    if (infoeditRemark.length > 50) {
                                        layer.msg('描述不可超出50字!抱歉!', { icon: 5, style: "color:red" });
                                        return false;
                                    }
                                    //赋值Dto
                                    var dto = new Object();
                                    dto.ID = id;
                                    dto.Name = infoeditName;
                                    dto.Remark = infoeditRemark;
                                    //将Dto放进集合
                                    var paraObj = new Object();
                                    paraObj.data = dto;
                                    //接口地址
                                    var url = "/SmartTool/ToolEditSubmit";
                                    //传参
                                    var data = ajaxProcess(url, paraObj);
                                    if (data) {
                                        if (parseInt(data.ResultType) === 0) { //请求成功返回

                                            $("#showtoolEditInfo_div").html("");
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
                        //取消关闭按钮
                        $(document)
                            .on("click",
                                ".dept_close",
                                function () {
                                    $("#showtoolEditInfo_div").html("");
                                    layer.close(index1);
                                });
                        //右上角关闭
                    }, cancel: function (index1) {
                        $("#showtoolEditInfo_div").html("");

                        layer.close(index1);
                        return false;
                    }
                });
            }
        });
//停用数据
$("#channelhtml")
    .ready(function () {
        ///使用停用
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function () {

                    var showEditStop = $(this);
                    var infoStopStatus = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");

                    if (infoStopStatus === "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function () {
                                //给dto赋值
                                var realData = {};
                                realData.ID = infoStopID;
                                realData.Status = infoStopStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartTool/ToolDisable";
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
                                realData.ID = infoStopID;
                                realData.Status = infoStopStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;


                                var url = "/SmartTool/ToolDisable";
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
                            function () {
                                layer.msg('已经取消此操作',
                                {
                                    icon: 6
                                });
                            });
                    }
                });

    });