var i = 1;

//开始展示单位信息数据
$("#smartUnitHtml").ready(
    function () {
        var getSmartUnitFunc = function () {
            var url = "/SmartUnit/SmartUnitGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($("#smartUnit_template").text());
            $(".layui-field-box").html(interText(data));
        };
        getSmartUnitFunc();
    });

//添加单位信息
$("#smartUnitHtml").ready(
    function () {
        //为添加单位信息注册点击事件
        $(".layui-btn.layui-btn-small").on("click", function () {
            var innerText = doT.template($("#showSmartUnitAddInfo_template").text());//得到添加单位模板
            var contentData = $("#showSmartUnitAddInfo_div").html(innerText());//回填数据
            //打开一个添加单位弹层页
            layer.open({
                type: 1,
                title: "添加单位信息",
                //skin: 'layui-layer-rim', //加上边框
                skin: 'layerbackground_color',
                area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                shade: [0.8, '#B3B3B3', false],
                closeBtn: 1,
                Boolean: false,
                shadeClose: false, //点击遮罩关闭
                content: contentData,
                success: function (layero, index) {
                    $(".layui-btn.layui-btn-normal.smartUnitAdd_commit")
                        .on("click",
                            function () {
                                var smartUnitInfoEditName = $("#smartUnitInfoEditName").val();//班次名称

                                if (smartUnitInfoEditName == "" || undefined || null) {
                                    layer.msg('单位名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (smartUnitInfoEditName.lenght > 20 || smartUnitInfoEditName === "") {
                                    layer.msg('单位名称不可超出20字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                var realData = {};
                                realData.Name = smartUnitInfoEditName;

                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartUnit/SmartUnitAdd";
                                var data = ajaxProcess(url, paraObj);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        $("#showSmartUnitAddInfo_div").html(""); //请求成功之后将div元素清空
                                        var message = data.Message;
                                        //关闭窗口
                                        layer.close(index);
                                        //提示消息
                                        layer.msg(message, { icon: 6 });
                                        //刷新主页面数据.
                                        setTimeout(function () {
                                            location.reload();
                                        },1500);

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
                                $("#showSmartUnitAddInfo_div").html("");
                                layer.close(index);
                            });
                },
                cancel: function (index) {
                    $("#showSmartUnitAddInfo_div").html("");
                    layer.close(index);
                    return false;
                }
            });
        });
    });

//修改单位信息
$("#smartUnitHtml")
    .ready(function () {
        $(".layui-btn.layui-btn-mini.smartUnitEdit")
            .on("click",
                function () {
                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("smartUnitiEditId");
                    var url = "/SmartUnit/SmartUnitEditGet";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showSmartUnitEditInfo_template").text());

                        var contentData = $("#showsmartUnitEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改单位信息",
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
                                            $("#showsmartUnitEditInfo_div").html("");

                                            layer.close(index1);
                                        });
                                //确认提交
                                $(".layui-btn.layui-btn-normal.smartUnitEdit_commit")
                                    .on("click",
                                        function () {

                                            var id = $("#smartUnitEditID").val();
                                            var smartUnitEditName = $("#smartUnitEditName").val();


                                            var reg = /^[0-9]*$/;
                                            if (smartUnitEditName == "" || undefined || null) {
                                                layer.msg('班次名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (smartUnitEditName.lenght > 20 || smartUnitEditName === "") {
                                                layer.msg('班次名称不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = smartUnitEditName;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SmartUnit/SmartUnitSubmit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showsmartUnitEditInfo_div").html("");
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

                            }, cancel: function (index1) {
                                $("#showsmartUnitEditInfo_div").html("");

                                layer.close(index1);
                            }
                        });
                    }

                });
    });

//删除数据
$("#smartUnitHtml")
    .ready(function () {
        ///使用停用
        $($(".layui-btn-mini.smartUnitDel"))
            .on("click",
                function () {

                    var showEditStopID = $(this);
                    var smartUnitDelId = $(showEditStopID).attr("smartUnitDelId");

                    //console.log(deptInfoId);
                    //console.log(deptInfoStopID);
                        layer.confirm('您确定删除本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function () {
                                //给dto赋值
                                var realData = {};
                                realData.ID = smartUnitDelId;


                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartUnit/SmartUnitDelete";
                                var data = ajaxProcess(url, paraObj);
                                layer.msg('已成功删除!', { icon: 1 });
                                setTimeout( function () {
                                    window.location.reload();
                                },1500);
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
                });

    });