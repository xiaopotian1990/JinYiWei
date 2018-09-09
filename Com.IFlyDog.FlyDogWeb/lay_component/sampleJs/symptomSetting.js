var i = 1;
//显示
$("#symptomSettinghtml")
    .ready(function() {
        var getDeptInfoFunc = function() {

            var url = "/SymptomSetting/SymptomGet";
            var paraObj = new Object();

            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#symptomSetting_template").text());
            $(".layui-field-box").html(interText(data));
        };
        getDeptInfoFunc();
    });

//添加症状信息
$("#symptomSettinghtml")
    .ready(function() {
        $("#symptomSettingAdd")
            .on("click",
                function() {

                    var innerText = doT.template($("#showsymptomSettingAddInfo_template").text());

                    var contentData = $("#showsymptomSettingAddInfo_div").html(innerText());
                    //页面层
                    layer.open({
                        type: 1,
                        title: "症状添加",
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
                                        var addName = $("#symptomSettingAddName").val();
                                        var addSortNo = $("#symptomSettingAddSortNo").val();
                                        var addRemark = $("#symptomSettingAddRemark").val();

                                        var reg = /^[0-9]*$/;
                                        if (!reg.test(addSortNo) || addSortNo === "") {
                                            layer.msg('排序号只能为整数!抱歉!', { icon: 5, style: "color:red" });
                                            return false;
                                        }
                                        if (addRemark.length > 50 || addSortNo === "") {
                                            layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                            return false;
                                        }

                                        var realData = {};
                                        realData.Name = addName;
                                        realData.SortNo = addSortNo;
                                        realData.Remark = addRemark;

                                        var paraObj = {};
                                        paraObj.data = realData;

                                        var url = "/SymptomSetting/Add";
                                        var data = ajaxProcess(url, paraObj);
                                        if (data) {
                                            if (parseInt(data.ResultType) === 0) { //请求成功返回
                                                $("#showsymptomSettingAddInfo_div").html("");
                                                var message = data.Message;
                                                //关闭窗口
                                                layer.close(index);
                                                //提示消息
                                                layer.msg(message, { icon: 6 });
                                                //刷新主页面数据.
                                                setTimeout(function() {
                                                        location.reload();
                                                    },
                                                    500);

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
                                        $("#showsymptomSettingAddInfo_div").html("");
                                        layer.close(index);
                                    });
                            //右上角关闭回调
                        },
                        cancel: function(index) {
                            $("#showsymptomSettingAddInfo_div").html("");
                            layer.close(index);
                            return false;
                        }
                    });
                });
    });

//修改
$("#symptomSettinghtml")
    .ready(function() {
        $(".layui-btn.layui-btn-mini.edit")
            .on("click",
                function() {
                    //获取ID
                    var showEditDialog = $(this);
                    var infoId = $(showEditDialog).attr("InfoiEditId");
                    var url = "/SymptomSetting/SymptomlGetByIDEdit";

                    //赋值DTO
                    var dto = new Object();
                    dto.SymptomID = infoId;


                    var paraObj = new Object();
                    paraObj.data = dto;


                    var data = ajaxProcess(url, paraObj);


                    var resultType = data.ResultType;

                    //后台返回数据 ResultType类型 等于0
                    if (parseInt(resultType) === 0) {

                        var innerText = doT.template($("#showsymptomSettingEditInfo_template").text());

                        var contentData = $("#showsymptomSettingEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "症状修改",
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

                                            var id = $("#symptomSettingEditID").val();
                                            var infoeditDeptName = $("#symptomSettingEditName").val();
                                            var infoeditDeptSortNo = $("#symptomSettingEditSortNo").val();
                                            var infoeditDeptRemark = $("#symptomSettingEditRemark").val();


                                            var reg = /^[0-9]*$/;
                                            if (!reg.test(infoeditDeptSortNo) || infoeditDeptSortNo == null) {
                                                layer.msg('排序号只能为整数!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (infoeditDeptRemark.length > 50) {
                                                layer.msg('描述不可超出50字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = infoeditDeptName;
                                            dto.SortNo = infoeditDeptSortNo;
                                            dto.Remark = infoeditDeptRemark;


                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SymptomSetting/SymptomlEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showsymptomSettingEditInfo_div").html("");
                                                    var message = data.Message;
                                                    //关闭窗口
                                                    layer.close(index1);
                                                    //提示信息
                                                    layer.msg(message, { icon: 6 });
                                                    //刷新主页面数据.
                                                    setTimeout(function() {
                                                            location.reload();
                                                        },
                                                        500);;
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
                                            $("#showsymptomSettingEditInfo_div").html("");

                                            layer.close(index1);
                                        });

                                //右上角关闭回调
                            },
                            cancel: function(index1) {
                                $("#showsymptomSettingEditInfo_div").html("");
                                layer.close(index1);
                                return false;
                            }
                        });
                    }

                });
    });

//停用数据
$("#deptHtml")
    .ready(function() {
        ///使用停用
        $($(".layui-btn-mini.EditStopBut"))
            .on("click",
                function() {

                    var showEditStop = $(this);
                    var infoStatus = $(showEditStop).attr("status");

                    var showEditStopID = $(this);
                    var infoStopID = $(showEditStopID).attr("stopID");
                    var status = $(showEditStopID).attr("status");
                    if (status == "0") {

                        layer.confirm('您确定停用本条数据？',
                            {
                                btn: ['确定', '取消'] //按钮
                            },
                            function() {
                                //给dto赋值
                                var realData = {};
                                realData.SymptomID = infoStopID;
                                realData.Status = infoStatus;

                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SymptomSetting/SymptomDisable";
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
                                realData.SymptomID = infoStopID;
                                realData.Status = infoStatus;
                                //组合传值
                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SymptomSetting/SymptomDisable";
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