$(function () {
    layui.use(['form', "layer"], function () {
        window.layer = layui.layer;
        window.form = layui.form();
    });
    $(document).on("change propertychange keydown keyup", "#priceValue,#scaleValue", function () {
        if (!parseInt($(this).val()) || !/^[0-9]*$/.test($(this).val()) || parseInt($(this).val()) < 0) {
            $(this).val(1);
            layer.tips("请输入正整数",$(this));
        }
    });
});
var i = 1;

//开始展示单位信息数据
$("#smartProductHtml").ready(
    function () {
        var getSmartProductFunc = function () {

            var py = $("#pinYinValue").val();//拼音码
            var gyName = $("#nameValue").val();//名称
            var smartProductDetaiName = $("#smartProductDetaiName").val();//类型
            var smartProductType = $("#smartProductType").val(); //-1全部。1 使用0 停用

            var url = "/SmartProduct/SmartProductInfoGet";//测试查询及分页
            var realData = {};
            realData.PinYin = py;
            realData.Name = gyName;
            realData.CategoryId = smartProductDetaiName;
            realData.Status = smartProductType;
            realData.PageNum = 1;
            realData.PageSize = 2;

            var paraObj = new Object();
            paraObj.data = realData;
            var data = ajaxProcess(url, paraObj).Data;
            pageFun(1, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
            var interText = doT.template($("#smartProduct_template").text());
            $(".site-table").html(interText(data.PageDatas));

        };
        getSmartProductFunc();

    });


//修改单位信息
$("#smartProductHtml")
    .ready(function () {
        $("#subtmValue")
            .on("click",
                function () {
                    var py = $("#pinYinValue").val();//拼音码
                    var gyName = $("#nameValue").val();//名称
                    var smartProductDetaiName = $("#smartProductDetaiName").val();//类型
                    var smartProductType = $("#smartProductType").val(); //-1全部。1 使用0 停用

                    var url = "/SmartProduct/SmartProductInfoGet";//测试查询及分页
                    var realData = {};
                    realData.PinYin = py;
                    realData.Name = gyName;
                    realData.CategoryId = smartProductDetaiName;
                    realData.Status = smartProductType;
                    realData.PageNum = 1;
                    realData.PageSize = 2;
                    var paraObj = {};
                    paraObj.data = realData;
                    var data = ajaxProcess(url, paraObj).Data;
                    var interText = doT.template($("#smartProduct_template").text());
                    if (data == null) {
                        $(".site-table").html(interText(""));
                    } else {
                        $(".site-table").html(interText(data.PageDatas));
                        pageFun(1, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
                    }

                });
    });





function aa() {


}
//分页方法
function pageFun(curr, size) {

    layui.use(['layer', 'laypage', 'element','form'], function () {
        var laypage = layui.laypage;
        var pageCount = Math.ceil(size / 2);

        //显示分页
        laypage({//size/2
            cont: 'pageDiv', //容器。值支持id名、原生dom对象，jquery对象。【如该容器为】：<div id="page1"></div>
            pages: pageCount, //通过后台拿到的总页数 （如果只有1页，则不显示分页控件）
            curr: curr || 1, //当前页
            jump: function (obj, first) { //触发分页后的回调
                if (!first) { //点击跳页触发函数自身，并传递当前页：obj.curr
                    //pageFun(obj.curr, 2);

                    var py = $("#pinYinValue").val();//拼音码
                    var gyName = $("#nameValue").val();//名称
                    var smartProductDetaiName = $("#smartProductDetaiName").val();//类型
                    var smartProductType = $("#smartProductType").val(); //-1全部。1 使用0 停用

                    var url = "/SmartProduct/SmartProductInfoGet";//测试查询及分页
                    var realData = {};
                    realData.PinYin = py;
                    realData.Name = gyName;
                    realData.CategoryId = smartProductDetaiName;
                    realData.Status = smartProductType;
                    realData.PageNum = obj.curr;
                    realData.PageSize = 2;
                    var paraObj = {};
                    paraObj.data = realData;
                    var data = ajaxProcess(url, paraObj).Data;

                    var interText = doT.template($("#smartProduct_template").text());
                    $(".site-table").html(interText(data.PageDatas));
                    pageFun(obj.curr, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数 先放到最后，可能有执行顺序的问题
                }
            }
        });
    });
};

//添加药品、物品信息
$("#smartProductHtml").ready(
    function () {


        //为添加单位信息注册点击事件
        $(".layui-btn.layui-btn-small").on("click", function () {
            var innerText = doT.template($("#smartProductAddInfo_template").text());//得到添加单位模板
            var contentData = $("#smartProductAddInfo_div").html(innerText());//回填数据
            //打开一个添加单位弹层页
            layer.open({
                type: 1,
                title: "添加药品/物品信息",
                //skin: 'layui-layer-rim', //加上边框
                skin: 'layerbackground_color',
                area: ["35%;min-width:680px;max-width:800px", "90%;min-height:500px;max-height:600px"], //宽高
                shade: [0.8, '#B3B3B3', false],
                closeBtn: 1,
                Boolean: false,
                shadeClose: false, //点击遮罩关闭
                content: contentData,
                success: function (layero, index) {
                    form.render();
                    $(".layui-btn.layui-btn-normal.smartProductAdd_commit")
                        .on("click",
                            function () {
                                //alert($("#smartProductDetaiEditName").find("option:selected").text());
                                var smartProductInfoName = $("#smartProductInfoName").val();//供应商名称
                                var pinYinMaInfoName = $("#pinYinMaInfoName").val();
                                var smartProductDetaiEditName = $("#smartProductDetaiEditName").val();
                                var smartProductAddType = $("#smartProductAddType").val();
                                var sizeValue = $("#sizeValue").val();
                                var priceValue = $("#priceValue").val();
                                var sartUnitKCDetaiName = $("#sartUnitKCDetaiName").val();
                                var sartUnitSYDetaiName = $("#sartUnitSYDetaiName").val();
                                var scaleValue = $("#scaleValue").val();
                                var remarkValue = $("#remarkValue").val();

                                if (smartProductInfoName == "" || undefined || null) {
                                    layer.msg('名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (smartProductInfoName.lenght > 20 || smartProductInfoName === "") {
                                    layer.msg('名称不可超出20字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                if (pinYinMaInfoName == "" || undefined || null) {
                                    layer.msg('拼音码不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }
                                if (pinYinMaInfoName.lenght > 20 || pinYinMaInfoName === "") {
                                    layer.msg('拼音码不可超出20字!抱歉!', { icon: 5 });
                                    return false;
                                }

                                if (smartProductDetaiEditName == "") {
                                    layer.msg('请选择类型!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                if (smartProductAddType == "") {
                                    layer.msg('请选择类型!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                if (sizeValue == "" || undefined || null) {
                                    layer.msg('规格不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                if (priceValue == "" || undefined || null) {
                                    layer.msg('价格不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                } 

                                if (sartUnitKCDetaiName == "" || undefined || null) {
                                    layer.msg('请选择库存单位!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                if (sartUnitSYDetaiName == "" || undefined || null) {
                                    layer.msg('请选择使用单位!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                if (scaleValue == "" || undefined || null) {
                                    layer.msg('进制不能为空!抱歉!', { icon: 5, style: "color:red" });
                                    return false;
                                }

                                var realData = {};
                                realData.Name = smartProductInfoName;
                                realData.PinYin = pinYinMaInfoName;
                                realData.CategoryID = smartProductDetaiEditName;
                                realData.Size = sizeValue;
                                realData.Price = priceValue;

                                realData.Status = smartProductAddType;
                                realData.Remark = remarkValue;
                                realData.UnitID = sartUnitKCDetaiName;
                                realData.MiniUnitID = sartUnitSYDetaiName;
                                realData.Scale = scaleValue;

                                var paraObj = {};
                                paraObj.data = realData;

                                var url = "/SmartProduct/SmartProductAdd";
                                var data = ajaxProcess(url, paraObj);
                                if (data) {
                                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                                        $("#smartProductAddInfo_div").html(""); //请求成功之后将div元素清空
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
                                $("#smartProductAddInfo_div").html("");
                                layer.close(index);
                            });
                },
                cancel: function (index) {
                    $("#smartProductAddInfo_div").html("");
                    layer.close(index);
                    return false;
                }
            });
        });
    });
//修改单位信息
$("#smartProductHtml")
    .ready(function () {
        $(".layui-field-box").on("click", ".layui-btn.layui-btn-mini.smartProductEdit",
                function () {
                    var showEditDialog = $(this);
                    var deptInfoId = $(showEditDialog).attr("smartProductiEditId");
                    var url = "/SmartProduct/SmartProductGet";

                    var dto = new Object();
                    dto.ID = deptInfoId;

                    var paraObj = new Object();
                    paraObj.data = dto;

                    var data = ajaxProcess(url, paraObj);
                    var ResultType = data.ResultType;

                    if (parseInt(ResultType) === 0) {
                      
                        var innerText = doT.template($("#showSmartProductEditInfo_template").text());

                        var contentData = $("#showSmartProductEditInfo_div").html(innerText(data.Data));

                        layer.open({
                            type: 1,
                            title: "修改药品/物品信息",
                            skin: 'layerbackground_color',
                            area: ["35%;min-width:680px;max-width:800px", "90%;min-height:500px;max-height:600px"], //宽高
                            shade: [0.8, '#B3B3B3', false],
                            closeBtn: 1,
                            Boolean: false,
                            shadeClose: false, //点击遮罩关闭
                            content: contentData,
                            success: function (layero, index1) {

                                $("#smartProductDetaiEditName").find("[value=" + data.Data.CategoryID + "]").prop("selected", true);

                                $("#sartUnitKCEditDetaiName").find("[value=" + data.Data.UnitID + "]").prop("selected", true);

                                $("#sartUnitSYEditDetaiName").find("[value=" + data.Data.MiniUnitID + "]").prop("selected", true);

                                $("#smartProductEditsType").find("[value=" + data.Data.Status + "]").prop("selected", true);
                                form.render();
                                //取消并关闭按钮
                                $(".layui-btn.layui-btn-danger.dept_close")
                                    .on("click",
                                        function () {
                                            $("#showSmartProductEditInfo_div").html("");

                                            layer.close(index1);
                                        });                
                                //确认提交
                                $(".layui-btn.layui-btn-normal.SmartProductEdit_commit")
                                    .on("click",
                                        function () {

                                            var smartProductInfoEditName = $("#smartProductInfoEditName").val();//供应商名称
                                            var pinYinMaInfoEditName = $("#pinYinMaInfoEditName").val();
                                            var smartProductDetaiEditName = $("#smartProductDetaiEditName").val();
                                            var smartProductEditsType = $("#smartProductEditsType").val();
                                            var sizeEditValue = $("#sizeEditValue").val();
                                            var priceEditValue = $("#priceEditValue").val();
                                            var sartUnitKCEditDetaiName = $("#sartUnitKCEditDetaiName").val();
                                            var sartUnitSYEditDetaiName = $("#sartUnitSYEditDetaiName").val();
                                            var scaleEditValue = $("#scaleEditValue").val();
                                            var remarkEditValue = $("#remarkEditValue").val();

                                            if (smartProductInfoEditName == "" || undefined || null) {
                                                layer.msg('名称不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (smartProductInfoEditName.lenght > 20 || smartProductInfoEditName === "") {
                                                layer.msg('名称不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            if (pinYinMaInfoEditName == "" || undefined || null) {
                                                layer.msg('拼音码不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }
                                            if (pinYinMaInfoEditName.lenght > 20 || pinYinMaInfoEditName === "") {
                                                layer.msg('拼音码不可超出20字!抱歉!', { icon: 5 });
                                                return false;
                                            }

                                            if (smartProductDetaiEditName == "" ) {
                                                layer.msg('请选择类型!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            } 

                                            if (smartProductEditsType == "") {
                                                layer.msg('请选择状态!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            if (sizeEditValue == "" || undefined || null) {
                                                layer.msg('规格不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            if (priceEditValue == "" || undefined || null) {
                                                layer.msg('价格不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            if (sartUnitKCEditDetaiName == "" || undefined || null) {
                                                layer.msg('请选择库存单位!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            if (sartUnitSYEditDetaiName == "" || undefined || null) {
                                                layer.msg('请选择使用单位!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            if (scaleEditValue == "" || undefined || null) {
                                                layer.msg('进制不能为空!抱歉!', { icon: 5, style: "color:red" });
                                                return false;
                                            }

                                            var dto = {};
                                            dto.ID = $("#smartSupplierEditID").val();
                                            dto.Name = smartProductInfoEditName;
                                            dto.PinYin = pinYinMaInfoEditName;
                                            dto.CategoryID = smartProductDetaiEditName;
                                            dto.Size = sizeEditValue;
                                            dto.Price = priceEditValue;
                                            dto.Status = smartProductEditsType;
                                            dto.Remark = remarkEditValue;
                                            dto.UnitID = sartUnitKCEditDetaiName;
                                            dto.MiniUnitID = sartUnitSYEditDetaiName;
                                            dto.Scale = scaleEditValue;

                                            var paraObj = new Object();
                                            paraObj.data = dto;
                                            var url = "/SmartProduct/SmartProductEdit";
                                            var data = ajaxProcess(url, paraObj);

                                            if (data) {
                                                if (parseInt(data.ResultType) === 0) { //请求成功返回

                                                    $("#showSmartProductEditInfo_div").html("");
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
                                $("#showSmartProductEditInfo_div").html("");

                                layer.close(index1);
                            }
                        });
                    }

                });
    });
