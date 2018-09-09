var i = 1;

$(function() {
    //显示
    var getinfoFunc = function() {
        var url = "/Channel/ChannelIndexGet";
        var paraObj = {};
        var data = ajaxProcess(url, paraObj).Data, interText = doT.template($("#channel_template").text());
        $(".layui-field-box").html(interText(data));
    }
    getinfoFunc();
});


//添加
$("#channelhtml")
    .ready(function() {
        $("#channeAdd")
            .on("click",
                function() {

                    var innerText = doT.template($("#showchannelAddInfo_template").text());

                    var contentData = $("#showchannelAddInfo_div").html(innerText());

                    //页面层
                    layer.open({
                        type: 1,
                        title: "添加信息",
                        //skin: 'layui-layer-rim', //加上边框
                     
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
                                        var addName = $("#channeltinfoAddName").val();
                                        var addSortNo = $("#channeltinfoAddSortNo").val();
                                        var addRemark = $("#channeltinfoAddRemark").val();

                                        var reg = /^[0-9]*$/;
                                        if (!reg.test(addSortNo) || addSortNo === "") {
                                            layer.msg('排序号只能为整数!抱歉!', { icon: 5, style: "color:red" });
                                            return false;
                                        }
                                        if (addRemark.length > 50) {
                                            layer.msg('描述不可超出50字!抱歉!', { icon: 5, style: "color:red" });
                                            return false;
                                        }

                                        var realData = {};
                                        realData.Name = addName;
                                        realData.SortNo = addSortNo;
                                        realData.Remark = addRemark;

                                        var paraObj = {};
                                        paraObj.data = realData;

                                        var url = "/Channel/ChannelAdd";
                                        var data = ajaxProcess(url, paraObj);

                                        if (data) {
                                            if (parseInt(data.ResultType) === 0) { //请求成功返回
                                                $("#showchannelAddInfo_div").html("");
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
                                        $("#showchannelAddInfo_div").html("");
                                        layer.close(index);
                                    });
                            //右上角关闭回调
                        },
                        cancel: function(index) {
                            $("#showchannelAddInfo_div").html("");
                            layer.close(index);
                            return false;
                        }
                    });
                });
    });

//修改
$("#channelhtml")
    .ready(function() {
        $(".layui-btn.layui-btn-mini.Edit")
            .on("click",
                function() {

                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("infoiEditId");
                    var url = "/Channel/ChannelGetByID";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showchannelEditInfo_template").text());

                        var contentData = $("#showchannelEditInfo_div").html(innerText(data.Data));

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

                                            var id = $("#channeltinfoEditID").val();
                                            var infoeditName = $("#channeltinfoEditName").val();
                                            var infoeditSortNo = $("#channeltinfoEditSortNo").val();
                                            var infoeditRemark = $("#channeltinfoEditRemark").val();


                                            var reg = /^[0-9]*$/;
                                            if (!reg.test(infoeditSortNo) || infoeditSortNo == null) {
                                                layer.msg('排序号只能为整数!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (infoeditRemark.length > 50) {
                                                layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = infoeditName;
                                            dto.SortNo = infoeditSortNo;
                                            dto.Remark = infoeditRemark;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/Channel/ChannelEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showchannelEditInfo_div").html("");
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
                                            $("#showchannelEditInfo_div").html("");
                                            layer.close(index1);
                                        });
                                //右上角关闭回调
                            },
                            cancel: function(index1) {
                                $("#showchannelEditInfo_div").html("");

                                layer.close(index1);
                                return false;

                            }
                        });
                    }

                });
    });

//停用数据
$("#channelhtml")
    .ready(function() {
        ///使用停用
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function() {

                    var showEditStop = $(this);
                    var infoStopStatus = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");

                    if (infoStopStatus === "0") {

                        layer.confirm('您确定停用本条数据？',{btn: ['确定', '取消']},
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.ChannelID = infoStopID;
                                realData.Status = infoStopStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/Channel/ChannelDisable";
                                var data = ajaxProcess(url, paraObj);
                          

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
                                realData.ChannelID = infoStopID;
                                realData.Status = infoStopStatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;


                                var url = "/Channel/ChannelDisable";
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