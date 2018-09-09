$(function () {
    layui.use("form", function () {
        window.form = layui.form();
    });
});

var i = 1;

//开始展示单位信息数据
$("#smartSupplierHtml").ready(

    function () {
        var getSmartSupplierFunc = function () {
            
            var py = $("#pinYinValue").val();//拼音码
            var gyName = $("#nameValue").val();//供应商名称
            
            var url = "/SmartSupplier/SmartSupplierInfoGet";//测试查询及分页
            var realData = {};
            realData.PinYin = py;
            realData.Name = gyName;
            realData.PageNum = 1;
            realData.PageSize = 2;
          
            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj).Data;
            if (data != null) {
                pageFun(1, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
                var interText = doT.template($("#smartSupplier_template").text());
                $(".layui-field-box").html(interText(data.PageDatas));
            }
           
         
           
        };
        getSmartSupplierFunc();
    });

function aa() {
    var py = $("#pinYinValue").val();//拼音码
    var gyName = $("#nameValue").val();//供应商名称

    var url = "/SmartSupplier/SmartSupplierInfoGet";//测试查询及分页
    var realData = {};
    realData.PinYin = py;
    realData.Name = gyName;
    realData.PageNum = 1;
    realData.PageSize = 2;
    var paraObj = {};
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj).Data;
    var interText = doT.template($("#smartSupplier_template").text());
    if (data==null) {
        $(".layui-field-box").html(interText(""));
    } else {
        $(".layui-field-box").html(interText(data.PageDatas));
        pageFun(1, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
    }

 
}

function pageFun(curr,size) {

    layui.use(['layer', 'laypage', 'element'], function () {
        var laypage = layui.laypage;
        var pageCount = Math.ceil(size / 2);

        //显示分页
        laypage({//size/2
            cont: 'pageDiv', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
            pages: pageCount, //通过后台拿到的总页数 （如果只有1页，则不显示分页控件）
            curr: curr || 1, //当前页
            shadeClose: false,
            jump: function (obj, first) { //触发分页后的回调
                if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                    //pageFun(obj.curr, 2);

                    var py = $("#pinYinValue").val();//拼音码
                    var gyName = $("#nameValue").val();//供应商名称

                    var url = "/SmartSupplier/SmartSupplierInfoGet";//测试查询及分页
                    var realData = {};
                    realData.PinYin = py;
                    realData.Name = gyName;
                    realData.PageNum = obj.curr;
                    realData.PageSize = 2;
                    var paraObj = {};
                    paraObj.data = realData;
                    var data = ajaxProcess(url, paraObj).Data;
                   
                    if (data == null) {
                        var interText = doT.template($("#smartSupplier_template").text());
                        $(".layui-field-box").html(interText(""));

                    } else {
                        var interText = doT.template($("#smartSupplier_template").text());
                        $(".layui-field-box").html(interText(data.PageDatas));
                        pageFun(obj.curr, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
                    }

                
                }
            }
        });
    });
};

//添加供应商信息
$("#smartSupplierHtml").ready(
    function () {
        //为添加单位信息注册点击事件
        $(".layui-btn.layui-btn-small").on("click", function () {
            var innerText = doT.template($("#showSmartSupplierAddInfo_template").text());//得到添加单位模板
            var contentData = $("#showSmartSupplierAddInfo_div").html(innerText());//回填数据
            //打开一个添加单位弹层页
            layer.open({
                type: 1,
                title: "添加供应商信息",
                //skin: 'layui-layer-rim', //加上边框
                skin: 'layerbackground_color',
                area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                shade: [0.8, '#B3B3B3', false],
                closeBtn: 1,
                shadeClose: false,
                content: contentData,
                success: function (layero, index) {
                    form.render();
                    $(".layui-btn.layui-btn-normal.smartSupplierAdd_commit")
                        .on("click",
                            function () {
                                var smartSupplierInfoName = $("#smartSupplierInfoName").val();//供应商名称
                                var pinYinMaInfoName = $("#pinYinMaInfoName").val();
                                var linkManInfoName = $("#linkManInfoName").val();
                                var conTactInfoName = $("#conTactInfoName").val();
                                var remark = $("#remarkValue").val();
                                if (smartSupplierInfoName == "" || undefined || null) {
                                    layer.msg('供应商名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (smartSupplierInfoName.lenght > 20 || smartSupplierInfoName === "") {
                                    layer.msg('供应商名称不可超出50字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                if (linkManInfoName == "" || undefined || null) {
                                    layer.msg('联系人不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (linkManInfoName.lenght > 20 || linkManInfoName === "") {
                                    layer.msg('联系人名称不可超出20字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})| (17[0-9]{1}))+\d{8})$/;
                                if (!myreg.test($("#conTactInfoName").val())) {
                                    layer.msg('请输入有效的手机号!抱歉!', { icon: 5 });
                                    return false;
                                }

                                var realData = {};
                                realData.Name = smartSupplierInfoName;
                                realData.LinkMan = linkManInfoName;
                                realData.Contact = conTactInfoName;
                                realData.Remark = remark;
                                realData.PinYin = pinYinMaInfoName;

                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartSupplier/SmartSupplierAdd";
                                var data = ajaxProcess(url, paraObj);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        $("#showSmartSupplierAddInfo_div").html(""); //请求成功之后将div元素清空
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
                                $("#showSmartSupplierAddInfo_div").html("");
                                layer.close(index);
                            });
                },
                cancel: function (index) {
                    $("#showSmartSupplierAddInfo_div").html("");
                    layer.close(index);
                    return false;
                }
            });
        });
    });

//修改单位信息
$("#smartSupplierHtml")
    .ready(function () {//.layui-btn.layui-btn-mini.smartSupplierEdit
        $(".admin-main").on("click","#smartSupplierEdit",function () {
                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("smartSupplieriEditId");
                    var url = "/SmartSupplier/SmartSupplierGet";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);

                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {

                        var innerText = doT.template($("#showSmartSupplierEditInfo_template").text());

                        var contentData = $("#showsmartSupplierEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改供应商信息",
                            skin: 'layerbackground_color',
                            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false],
                            closeBtn: 1,
                            shadeClose: false,
                            content: contentData,
                            success: function (layero, index1) {
                                //取消并关闭按钮
                                $(".layui-btn.layui-btn-danger.dept_close")
                                    .on("click",
                                        function () {
                                            $("#showsmartSupplierEditInfo_div").html("");

                                            layer.close(index1);
                                        });
                     
                                //确认提交
                                $(".layui-btn.layui-btn-normal.smartSupplierEdit_commit")
                                    .on("click",
                                        function () {

                                            var smartSupplierEditName = $("#smartSupplierEditName").val();//供应商名称
                                            var pinYinMaInfoEditName = $("#pinYinMaInfoEditName").val();
                                            var linkManInfoEditName = $("#linkManInfoEditName").val();
                                            var conTactInfoEditName = $("#conTactInfoEditName").val();
                                            var remarkEditValue = $("#remarkEditValue").val();
                                            if (smartSupplierEditName == "" || undefined || null) {
                                                layer.msg('供应商名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (smartSupplierEditName.lenght > 20 || smartSupplierEditName === "") {
                                                layer.msg('供应商名称不可超出50字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            if (linkManInfoEditName == "" || undefined || null) {
                                                layer.msg('联系人不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (linkManInfoEditName.lenght > 20 || linkManInfoEditName === "") {
                                                layer.msg('联系人名称不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var myreg = /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1})| (17[0-9]{1}))+\d{8})$/;
                                            if (!myreg.test($("#conTactInfoEditName").val())) {
                                                layer.msg('请输入有效的手机号!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            var dto = {};
                                            dto.ID = $("#smartSupplierEditID").val();
                                            dto.Name = smartSupplierEditName;
                                            dto.LinkMan = linkManInfoEditName;
                                            dto.Contact = conTactInfoEditName;
                                            dto.Remark = remarkEditValue;
                                            dto.PinYin = pinYinMaInfoEditName;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SmartSupplier/SmartSupplierEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showsmartSupplierEditInfo_div").html("");
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
                                $("#showsmartSupplierEditInfo_div").html("");

                                layer.close(index1);
                            }
                        });
                    }

                });
    });

//删除数据
$("#smartSupplierHtml")
    .ready(function () {
        ///使用停用
        $(".admin-main").on("click", "#smartSupplierDel", function () {
                    var showEditStopID = $(this);
                    var smartUnitDelId = $(showEditStopID).attr("smartSupplierDelId");

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

                            var url = "/SmartSupplier/SmartSupplierDelete";
                            var data = ajaxProcess(url, paraObj);
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