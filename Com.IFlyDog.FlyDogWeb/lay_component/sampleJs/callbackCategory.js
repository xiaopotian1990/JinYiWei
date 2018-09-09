//回访类型js


var i = 1;

//回访类型显示
$("#callbackCategoryhtml")
    .ready(function() {
        var getDeptInfoFunc = function() {

            var url = "/CallbackCategory/CallbackCategoryGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($("#callbackCategory_template").text());
            $(".layui-field-box").html(interText(data));
        };
        getDeptInfoFunc();
    });

//添加回访类型信息
$("#callbackCategoryhtml")
    .ready(function() {
        $(".layui-btn.layui-btn-small")
            .on("click",
                function() {
                    //页面
                    var innerText = doT.template($("#showcallbackCategoryAddInfo_template").text());

                    var contentData = $("#showAddInfo_div").html(innerText());

                    layer.open({
                        type: 1, //浮窗样式
                        title: "添加信息", //标题
                        skin: 'layerbackground_color', //设置浮窗背景颜色
                        area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //设置宽高
                        shade: [0.8, '#B3B3B3', false],
                        closeBtn: 1, //关闭按钮
                        Boolean: false,
                        shadeClose: false, //点击关闭遮罩
                        content: contentData, //输出样式
                        success: function(layero, index) { //浮窗弹出成功后的操作
                            //确认提交按钮
                            $(".layui-btn.layui-btn-normal.commitT")
                                .on("click",
                                    function() {
                                        var categorAddName = $("#callbackCategoryinfoAddName").val();
                                        var categoryAddRemark = $("#callbackCategoryinfoAddRemark").val();

                                        if (categorAddName=="") {
                                            layer.msg('名称不可为空!抱歉!', { icon: 5 });
                                            return false;
                                        } else if (categorAddName!=""&&categorAddName.length > 20) {
                                            layer.msg('名称不可超出20字!抱歉!', { icon: 5 });
                                            return false;
                                        }

                                        if (categoryAddRemark.length > 50) {
                                            layer.msg('描述不可为空!或则超出50字!抱歉!', { icon: 5 });
                                            return false;
                                        }

                                        //创建DTO赋值
                                        var realData = {};
                                        realData.Name = categorAddName;
                                        realData.Remark = categoryAddRemark;

                                        //创建一个list把上方DTO放入
                                        var paraObj = {};
                                        paraObj.data = realData;

                                        //接口地址
                                        var url = "/CallbackCategory/CallbackAdd";
                                        var data = ajaxProcess(url, paraObj);

                                        if (data) {
                                            var message = data.Message;
                                            if (parseInt(data.ResultType) === 0) { //请求接口成功后返回信息                                             
                                                $("#showAddInfo_div").html("");
                                                //关闭当前窗口
                                                layer.close(index);
                                                setTimeout(function () {
                                                    location.reload();
                                                },
                                                             1500);
                                                //提示消息
                                                layer.msg(message, { icon: 6 });


                                            } else {

                                                layer.msg(message, { icon: 5 });
                                            }
                                        }
                                    });
                            //取消并关闭按钮
                            $(".layui-btn.layui-btn-danger.closeT")
                                .on("click",
                                    function() {
                                        $("#showAddInfo_div").html("");
                                        layer.close(index);
                                    });
                        },
                        cancel: function (index, layero) {
                            layer.close(index);
                            $("#showAddInfo_div").html("");
                        }
                    });
                });
    });


//修改回填数据
$("#callbackCategoryhtml")
    .ready(function() {
        $(".layui-btn.layui-btn-mini.edit")
            .on("click",
                function() {
                    //获取当前点击行ID
                    var showEditDialog = $(this);
                    var infoID = $(showEditDialog).attr("infoiEditId");
                    //请求接口
                    var url = "/CallbackCategory/CallbackGetByID/";

                    //实例dto
                    var dto = {}
                    dto.ID = infoID;
                    //数据放进集合
                    var paraObj = {};
                    paraObj.data = dto;
                    //调用Json方法
                    var data = ajaxProcess(url, paraObj);

                    //判断返回数据是否为空
                    var ResultType = data.ResultType;

                    if (ResultType === 0) {
                        //dot输出浮窗样式
                        var innerText = doT.template($("#showcallbackCategoryEditInfo_template").text());

                        var contentData = $("#showEditInfo_div").html(innerText(data.Data));

                        //打开浮窗样式
                        layer.open({
                            type: 1,
                            title: "修改信息", //标题
                            skin: 'layerbackground_color', // 背景颜色
                            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false], //窗体透明度
                            close: 1,
                            shadeClose: false, //点击关闭遮罩
                            content: contentData, //输出样式,
                            success: function(layero, index) { //窗体弹出成功后的操作
                                //取消并关闭按钮
                                $(".layui-btn.layui-btn-danger.closeT")
                                    .on("click",
                                        function() {
                                            $("#showEditInfo_div").html("");
                                            layer.close(index);
                                        });
                                //点击提交
                                $(".layui-btn.layui-btn-normal.commitT")
                                    .on("click",
                                        function() {
                                            //进行传值
                                            var id = $("#callbackCategoryinfoEditID").val();
                                            var infoeditName = $("#callbackCategoryinfoEditName").val();
                                            var infoeditRemark = $("#callbackCategoryinfoEditRemark").val();;

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
                                            var url = "/CallbackCategory/CallbackEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回
                                                    $("#showEditInfo_div").html("");
                                                    var message = data.Message;
                                                    //关闭窗口
                                                    layer.close(index);
                                                    //提示信息
                                                    layer.msg(message, { icon: 6 });
                                                    //刷新主页面数据.
                                                    setTimeout(function() {
                                                        location.reload();
                                                    }, 500);
                                                  
                                                   
                                                } else {
                                                    //请求成功返回,但是后台出现错误
                                                    layer.msg(data.Message, { icon: 5 });
                                                }
                                            }
                                            return false;
                                        });
                            }, cancel: function (index) {
                                $("#showEditInfo_div").html("");
                                layer.close(index);
                            }
                        });
                    }
                });
    });

//停用及启用数据
$("#callbackCategoryhtml")
    .ready(function() {
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function() {

                    var showEditStop = $(this);
                    var dataInfoId = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");

                    if (dataInfoId === "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.CallbackID = infoStopID;
                                realData.Status = dataInfoId;

                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/CallbackCategory/CallbackDisable";
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
                                        },500);
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
                                realData.CallbackID = infoStopID;
                                realData.Status = dataInfoId;

                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/CallbackCategory/CallbackDisable";
                                var data = ajaxProcess(url, paraObj);
                    

                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        var message = data.Message;
                                        //提示消息
                                        layer.msg(message, { icon: 6 });
                                        //刷新主页面数据.
                                        setTimeout(function () {
                                            location.reload();
                                        }, 500);
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