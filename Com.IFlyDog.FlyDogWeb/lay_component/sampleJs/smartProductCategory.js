$(function () {
    layui.use("form", function () {
        window.form = layui.form();
    });
});
var i = 1;

//开始展示药物品信息数据
$("#smartProductCategoryHtml").ready(
    function () {
        var getSmartProductCategoryFunc = function () {
            var url = "/SmartProductCategory/SmartProductCategoryGet";
            var paraObj = new Object();
            var data = ajaxProcess(url, paraObj).Data;
            var interText = doT.template($("#smartProductCategory_template").text());
            $(".layui-field-box").html(interText(data));
        };
        getSmartProductCategoryFunc();
    });

//添加药物品信息
$("#smartProductCategoryHtml").ready(
    function () {
        //为添加药物品信息注册点击事件
        $(".layui-btn.layui-btn-small").on("click", function () {
            var innerText = doT.template($("#showSmartProductCategoryAddInfo_template").text());//得到添加药物品模板
            var contentData = $("#showSmartProductCategoryAddInfo_div").html(innerText());//回填数据
            //打开一个添加药物品弹层页
            layer.open({
                type: 1,
                title: "药品/物品信息添加",
                //skin: 'layui-layer-rim', //加上边框
                skin: 'layerbackground_color',
                area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                shade: [0.8, '#B3B3B3', false],
                closeBtn: 1,
                Boolean: false,
                shadeClose: false, //点击遮罩关闭
                content: contentData,
                success: function (layero, index) {
                    form.render();
                    $(".layui-btn.layui-btn-normal.smartProductCategoryAdd_commit")
                        .on("click",
                            function () {
                                var smartUnitInfoEditName = $("#smartProductCategoryInfoEditName").val();//药品物品名称
                                var smartProductCategoryInfoEditSortNo = $("#smartProductCategoryInfoEditSortNo").val();//序号
                                var smartProductCategoryInfoEditRemark = $("#smartProductCategoryInfoEditRemark").val();//备注
                                var smartProductCategoryVal = $("#smartProductName").val();//选择的上级分类id
                                if (isNaN(smartProductCategoryInfoEditSortNo) || !(/^\d+$/.test(smartProductCategoryInfoEditSortNo))) {
                                    layer.msg("序号只能是数字！", { icon: 2 });
                                    return false;
                                }

                                if (smartUnitInfoEditName == "" || undefined || null) {
                                    layer.msg('药品/物品名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (smartUnitInfoEditName.lenght > 20 || smartUnitInfoEditName === "") {
                                    layer.msg('药品/物品名称不可超出20字!抱歉!', { icon: 5 });
                                    return false;
                                }
                                if (smartProductCategoryVal == "" || smartProductCategoryVal == "-1") {
                                    smartProductCategoryVal = "0";
                                }

                                var realData = {};
                                realData.Name = smartUnitInfoEditName;
                                realData.SortNo = smartProductCategoryInfoEditSortNo;
                                realData.Remark = smartProductCategoryInfoEditRemark;
                                realData.PID = smartProductCategoryVal;

                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartProductCategory/SmartProductCategoryAdd";
                                var data = ajaxProcess(url, paraObj);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        $("#showSmartProductCategoryAddInfo_div").html(""); //请求成功之后将div元素清空
                                        var message = data.Message;
                                        //关闭窗口
                                        layer.close(index);
                                        //提示消息
                                        layer.msg(message, { icon: 6 });
                                        //刷新主页面数据.
                                        setTimeout(function () {
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
                            function () {
                                $("#showSmartProductCategoryAddInfo_div").html("");
                                layer.close(index);
                            });
                },
                cancel: function (index) {
                    $("#showSmartProductCategoryAddInfo_div").html("");
                    layer.close(index);
                    return false;
                }
            });
        });
    });

//修改药品物品信息
$("#smartProductCategoryHtml")
    .ready(function () {
        $(".layui-btn.layui-btn-mini.smartProductCategoryEdit")
            .on("click",
                function () {
                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("smartProductCategoryiEditId");
                    var url = "/SmartProductCategory/SmartProductCategoryEditGet";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showSmartProductCategoryEditInfo_template").text());

                        var contentData = $("#showSmartProductCategoryEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改药品/物品信息",
                            skin: 'layerbackground_color',
                            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false],
                            closeBtn: 1,
                            Boolean: false,
                            shadeClose: false, //点击遮罩关闭
                            content: contentData,
                            success: function (layero, index1) {
                                if (data.Data.PID) {
                                    $("#smartProductEditName").find("[value=" + data.Data.PID + "]").prop("selected",true);
                                }

                                form.render();
                                //取消并关闭按钮
                                $(".layui-btn.layui-btn-danger.dept_close")
                                    .on("click",
                                        function () {
                                            $("#showSmartProductCategoryEditInfo_div").html("");

                                            layer.close(index1);
                                        });

                                //确认提交
                                $(".layui-btn.layui-btn-normal.smartProductCategory_commit")
                                    .on("click",
                                        function () {

                                            var id = $("#smartProductCategoryEditID").val();
                                            var smartProductCategoryEditName = $("#smartProductCategoryEditName").val();//药品物品名称
                                            var smartProductCategoryEditSortNo = $("#smartProductCategoryEditSortNo").val();//序号
                                            var smartProductCategoryEditRemark = $("#smartProductCategoryEditRemark").val();//备注
                                            var smartProductCategoryEditVal = $("#smartProductEditName").val();//选择的上级分类id
                                            if (isNaN(smartProductCategoryEditSortNo) || !(/^\d+$/.test(smartProductCategoryEditSortNo))) {
                                                layer.msg("序号只能是数字！", { icon: 2 });
                                                return false;
                                            }

                                            var reg = /^[0-9]*$/;
                                            if (smartProductCategoryEditName == "" || undefined || null) {
                                                layer.msg('药品/物品名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (smartProductCategoryEditName.lenght > 20 || smartProductCategoryEditName === "") {
                                                layer.msg('药品/物品名称不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            if (smartProductCategoryEditVal == "" || smartProductCategoryEditVal == "-1") {
                                                smartProductCategoryEditVal = "0";
                                            }

                                            var dto = new Object();
                                            dto.ID = id;
                                            dto.Name = smartProductCategoryEditName;
                                            dto.SortNo = smartProductCategoryEditSortNo;
                                            dto.Remark = smartProductCategoryEditRemark;
                                            dto.PID = smartProductCategoryEditVal;
                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SmartProductCategory/SmartProductCategorySubmit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showSmartProductCategoryEditInfo_div").html("");
                                                    var message = data.Message;
                                                    //关闭窗口
                                                    layer.close(index1);
                                                    //提示信息
                                                    layer.msg(message, { icon: 6 });
                                                    //刷新主页面数据.
                                                    setTimeout(function () {
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
                            }, cancel: function (index1) {
                                $("#showSmartProductCategoryEditInfo_div").html("");

                                layer.close(index1);
                            }
                        });
                    }

                });
    });

//删除数据
$("#smartProductCategoryHtml")
    .ready(function () {
        ///使用停用
        $($(".layui-btn-mini.smartProductCategoryDel"))
            .on("click",
                function () {

                    var showEditStopID = $(this);
                    var smartProductCategoryDelId = $(showEditStopID).attr("smartProductCategoryDelId");

                    //console.log(deptInfoId);
                    //console.log(deptInfoStopID);
                    layer.confirm('您确定删除本条数据？',
                        {
                            btn: ['确定', '取消'] //按钮
                        },
                        function () {
                            //给dto赋值
                            var realData = {};
                            realData.ID = smartProductCategoryDelId;


                            //组合传值
                            var paraObj = {};
                            paraObj.data = realData;

                            var url = "/SmartProductCategory/SmartProductCategoryDelete";
                            var data = ajaxProcess(url, paraObj);
                            layer.msg('已成功删除!', { icon: 1 });
                           

                            setTimeout(function () {
                                window.location.reload();
                            }, 2000);
                            if (data) {
                                if (parseInt(data.ResultType) === 0) { //请求成功返回
                                    var message = data.Message;
                                    //提示消息
                                    layer.msg(message, { icon: 6 });
                                    //刷新主页面数据.
                                    setTimeout(function () {
                                        location.reload();
                                    }, 2000);
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