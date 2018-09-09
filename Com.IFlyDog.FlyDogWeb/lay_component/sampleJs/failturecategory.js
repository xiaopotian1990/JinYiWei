var i = 1;
//显示
$("#failtureCategoryhtml")
    .ready(function() {
        var getDeptInfoFunc = function() {

            var url = "/FailtureCategory/FailtureGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#failtureCategory_template").text());
            $(".layui-field-box").html(interText(data.Data));
        };
        getDeptInfoFunc();
    });

//添加
$("#failtureCategoryhtml")
    .ready(function() {
        $("#infoAdd")
            .on("click",
                function() {

                    var innerText = doT.template($("#showfailtureCategoryAddInfo_template").text());

                    var contentData = $("#showfailtureCategoryAddInfo_div").html(innerText());
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
                                        var addName = $("#failtureCategoryinfoAddName").val();

                                        var addRemark = $("#failtureCategoryinfoAddRemark").val();
                                        if (addName=="") {
                                            layer.msg('名称不可为空!抱歉!', { icon: 5 });
                                            return false;
                                        } else if (addName != "" && addName.length>20) {
                                            layer.msg('名称不可超出20字!抱歉!', { icon: 5 });
                                            return false;
                                        }
                                        if (addRemark.length > 50) {
                                            layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                            return false;
                                        }

                                        var realData = {};
                                        realData.Name = addName;
                                        realData.Remark = addRemark;

                                        var paraObj = {};
                                        paraObj.data = realData;

                                        var url = "/FailtureCategory/FailtureCategoryAdd";
                                        var data = ajaxProcess(url, paraObj);
                                        if (data) {
                                            if (parseInt(data.ResultType) === 0) { //请求成功返回
                                                $("#showfailtureCategoryAddInfo_div").html("");
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
                                        $("#showfailtureCategoryAddInfo_div").html("");
                                        layer.close(index);
                                    });
                            //右上角关闭回调
                        },
                        cancel: function(index) {
                            $("#showfailtureCategoryAddInfo_div").html("");
                            layer.close(index);
                            return false;
                        }
                    });
                });
    });

//修改
$("#failtureCategoryhtml")
    .ready(function() {
        $(".failtureCategoryEdit")
            .on("click",
                function() {
                    var showEditDialog = $(this);
                    var infoId = $(showEditDialog).attr("infoiEditId");
                    var url = "/FailtureCategory/FailtureCategoryGetByID";

                    var dto = new Object();
                    dto.ID = infoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showfailtureCategoryEditInfo_template").text());

                        var contentData = $("#showfailtureCategoryinfoEditInfo_div").html(innerText(data.Data));

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

                                            var id = $("#failtureCategoryinfoinfoEditID").val();
                                            var infoeditName = $("#failtureCategoryinfoinfoEditName").val();
                                            var infoeditRemark = $("#failtureCategoryinfoinfoEditRemark").val();

                                            if (infoeditName == "") {
                                                layer.msg('名称不可为空!抱歉!', { icon: 5 });
                                                return false;
                                            } else if (infoeditName != "" && infoeditName.length > 20) {
                                                layer.msg('名称不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }
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
                                            var url = "/FailtureCategory/FailtureCategoryEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showfailtureCategoryinfoEditInfo_div").html("");
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
                                            $("#showfailtureCategoryinfoEditInfo_div").html("");
                                            layer.close(index1);
                                        });
                                //右上角关闭回调
                            },
                            cancel: function(index1) {
                                $("#showfailtureCategoryinfoEditInfo_div").html("");

                                layer.close(index1);
                                return false;
                            }
                        });
                    }

                });
    });


//停用数据
$("#failtureCategoryhtml")
    .ready(function() {
        ///使用停用
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function() {

                    var showEditStop = $(this);
                    var infoStopstatus = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");

                    if (infoStopstatus === "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.FailtureCategoryID = infoStopID;
                                realData.Status = infoStopstatus;;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/FailtureCategory/FailtureCategoryDisable";
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
                                realData.FailtureCategoryID = infoStopID;
                                realData.Status = infoStopstatus;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/FailtureCategory/FailtureCategoryDisable";
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