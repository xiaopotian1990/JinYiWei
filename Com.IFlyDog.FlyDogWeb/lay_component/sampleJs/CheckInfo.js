// 两次弹窗相关变量
var detailOpenCount = 0, drugsOpenCount = 0;
$(function () {
    layui.use(['form', 'laydate', 'layer'], function () {
        var laydate = layui.laydate,
            layer = layui.layer;
        window.form = layui.form();
        
    });
    // 获取表格数据
    getTableData();

    $(".search").click(function () {
        getTableData();
    });

    // 查看按钮
    $(".check-table").on("click", ".btn-edit", function () {
        var pdId = $(this).parents("tr").data("id");

        layer.open({
            type: 1,
            title: "盘点信息",
            area: ["55%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"],
            shade: [0.8, '#B3B3B3', false],
            closeBtn: 1,
            shadeClose: false, //点击遮罩关闭
            content: $(".data-detail-pop"),
            success: function (layero, index) {

                layero.find("input,textarea,select").prop("disabled",true);

                // 提交URL
                var url = "/Check/CheckGetByID",
                    // 提交参数
                    paraObj = {
                        data: {
                            ID: pdId
                        }
                    };

                // ajax返回数据
                var result = ajaxProcess(url, paraObj).Data;

                layero.find("[value=" + result.WarehouseID + "]").prop("selected", true);
                layero.find("[name=date]").val(result.CreateDate);
                layero.find("[name=remark]").val(result.Remark);
                // 加载表格数据
                var interText = doT.template($("#checkDetail").text());
                $("#check-detail").append(interText(result.CheckDetailAdd));

                layero.find("[name=date],[name=remark],input,select").prop("disabled", true);
                layero.find(".dept_commit").hide();
                form.render();
                layero.on("click", ".dept_close,.layui-layer-close", function () {
                    layer.close(index);
                    closeEve($(this));
                });
            }
        });
    });

    // 添加按钮
    $(".btn-add").click(function () {
        layer.open({
            type: 1,
            title: "盘点信息",
            area: ["55%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"],
            shade: [0.8, '#B3B3B3', false],
            closeBtn: 1,
            shadeClose: false, //点击遮罩关闭
            content: $(".data-detail-pop"),
            success: function (layero, index) {
                //detailOpenCount++;
                //if (detailOpenCount == 1) {
                //}
                layero.find("[name=date],[name=remark],input,select").prop("disabled", false);
                layero.on("click", ".trDelete", function () {
                    window[condition] = Common.Utils.Array.remove(window[condition], $(this).parents("tr").attr("value"));
                    $(this).parents("tr").remove();
                });
                layero.find(".dept_commit").show();
                layero.on("click", ".dept_commit", function () {
                    //确认提交按钮
                    if (!$("[name=date]").val()) {
                        layer.tips("请选择盘点时间！", $("[name=date]")); return false;
                    }
                    var datalist = [];

                    var tr = $("#check-detail tr");

                    if (tr.length == 0) {
                        layer.msg("请添加药物品！", { icon: 5 });
                        return false;
                    }

                    tr.each(function (i, item) {
                        item = $(item);
                        datalist.push({
                            ProductID: item.attr("value"),
                            Num: item.find("td").eq(3).children().val(),
                            Price: item.find("td").eq(4).text(),
                            StockId: item.find("td#smartWarehouseRemarkTdhidden").text(),
                            Status: item.find("[name=productStatus]").val(),
                            CheckProductId: item.data("checkproduct")
                        });
                    });

                    var realData = {
                        WarehouseID: $("#popSmartWarehouse").val(),
                        CreateDate: $("[name=date]").val(),
                        Remark: $("[name=remark]").val(),
                        Status: 1,
                        CheckDetailAdd: datalist
                    };

                    var paraObj = {};
                    paraObj.data = realData;

                    var url = "/Check/CheckAdd";

                    var data = ajaxProcess(url, paraObj);
                    if (data) {
                        if (parseInt(data.ResultType) === 0) { //请求成功返回
                            window[condition] = [];
                            var message = data.Message;
                            //关闭窗口
                            layer.close(index);
                            closeEve($(this));
                            //提示消息
                            layer.msg(message, { icon: 6 });
                            //刷新主页面数据.
                            getTableData();

                        } else {
                            //请求成功返回,但是后台出现错误
                            layer.msg(data.Message, { icon: 5 });
                        }
                    }
                });
                layero.on("click", ".dept_close,.layui-layer-close", function () {
                    layer.close(index);
                    closeEve($(this));
                });
                layero.on("click", ".show-detail-btn", function () { showDetail(); });
                form.render();
            }
        });
    });

    // 添加详细按钮
    var showDetail = function () {
        getDrugsTableData();
        layer.open({
            type: 1,
            title: "选择药物品",
            closeBtn: 1, //不显示关闭按钮
            shade: [0],
            area: ["55%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"],
            anim: 2,
            content: $(".drugs-pop"),
            success: function (layero, index) {
                drugsOpenCount++;
                layero.data("index",index);
                // 判断是不是第一次弹出窗口
                if(drugsOpenCount == 1){
                    layero.find("#subtmValue").click(function () {
                        getDrugsTableData();
                    });
                    layero.find(".dept_commit").click(function () {
                        var StockId,
                            datas = [];
                        $.each($('input[name="cheId"]:checked'),function (i, item) {
                            item = $(item);
                            StockId = item.parents("tr").attr(condition); //id
                            if ($('#check-detail').find("tr[value=" + StockId + "]").length == 0){
                                if (window[condition] === undefined) {
                                    window[condition] = [];
                                }
                                window[condition].push(StockId);
                            }
                            datas.push({
                                ProductID: StockId,
                                CheckProductId: Common.StrUtils.isFalseSetEmpty(item.attr("productID")),
                                ProductName: Common.StrUtils.isFalseSetEmpty(item.attr("productNameVal")),
                                Size: Common.StrUtils.isNullOrEmpty(item.attr("productSizeVal")) ? 1 : item.attr("productSizeVal"),
                                KcName: Common.StrUtils.isFalseSetEmpty(item.attr("productKcNameVal")),
                                Price: Common.StrUtils.isNullOrEmpty(item.attr("productPriceVal")) ? 0 : item.attr("productPriceVal"),
                                Amount: Common.StrUtils.isFalseSetEmpty(item.attr("productAmountVal")),
                                Num: Common.StrUtils.isNullOrEmpty(item.attr("productCountVal")) ? 1 : item.attr("productCountVal"),
                                No: Common.StrUtils.isFalseSetEmpty(item.attr("productNoVal")),
                                //StockId: Common.StrUtils.isNullOrEmpty(item.attr("productStockId")),
                                StockId: item.attr("productStockId"),
                                Batch: Common.StrUtils.isNullOrEmpty(item.attr("productBatchVal")),
                                Expiration: Common.StrUtils.isNullOrEmpty(item.attr("productExpirationVal")),
                                verify: condition == "productStockId" ? true : false
                            });
                        });
                        var interText = doT.template($("#checkDetail").text());
                        $("#check-detail").append(interText(datas));
                        form.render();
                        layer.close($(this).parents(".layui-layer").data("index"));
                        closeEve($(this));
                    });
                    layero.find(".dept_close").click(function () {
                        layer.close($(this).parents(".layui-layer").data("index"));
                        closeEve($(this));
                    });
                }
            }
        });
    }

    // 关闭弹窗按钮
    var closeEve = function (btn) {
        btn.parents(".layui-layer").find("form")[0].reset();
        btn.parents(".layui-layer").find("tbody").empty();
    };

    // 文本框输入监控
    $(document).on("change propertychange keyup", "#check-detail [data-calc],#check-detail [data-verify],.drugs-table .layui-input", function () {
        var _this = $(this);
        if (_this.data("verify") == "size" && _this.val() > parseInt(_this.parents("tr").data("size"))) {
            _this.val(parseInt(_this.parents("tr").data("size")));
        }
        var calc = eval('(' + _this.data("calc").replace("(", "").replace(")", "") + ')');
        if (calc.calc == "totals" && !isNaN(_this.val())) {
            _this.parents("tr").find(calc.Amount).text(parseFloat(_this.val()) * parseFloat($(calc.price).attr("value")));
            $("#productCountVal").val(_this.val());
        }
    });

});
// 分页相关变量
var pageNum = 1,
    pageSize = 15,
    pageTotals;
// 获取表格数据方法
var getTableData = function () {
    var url = "/Check/CheckGet",
        paraObj = {};
    // 提交参数
    paraObj.data = {
        WarehouseID: $("#smartWarehouse").val(),
        BeginDate: $("[name=beginDate]").val(),
        EndDate: $("[name=endDate]").val(),
        No: $("[name=pdNo]").val(),
        PageNum: pageNum,
        PageSize: pageSize
    };

    // ajax返回数据
    var result = ajaxProcess(url, paraObj).Data,
        interText = doT.template($("#checkTmp").text());
    $(".check-table").html(interText(result.PageDatas));
    pageTotals = result.PageTotals;
    pageFun();
}
// 获取页面数据相关变量
var condition;
var getDrugsTableData = function () {
    var urlA = "/SmartProduct/SmartProductInfoGetAll", //在盘盈的情况下，查询医院所有的商品
        urlB = "/SmartStock/SmartStockInfoGet", //在盘亏情况下展示的的是当前用户所能操作仓库的库存商品
        paraObj = {};
    // 提交参数
    paraObj.data = {
        PinYin: $("#smartProductPinYin").val(),
        Name: $("#smartProductName").val(),
        CategoryId: $("#smartProductDetaiName").val(),
    };
    var status = $("#psStatus").val();

    //这里需要判断是盘盈盘亏来传相应的url
    var url = (status == 0 ? urlA : urlB);
    condition = (status == 0 ? "deptInfoId" : "productStockId");
    // ajax返回数据
    var result = ajaxProcess(url, paraObj).Data,
        interText = doT.template($("#drugs").text());
    var tempHtml = $(interText(result));
    if (window[condition] != undefined) {
        $.each(window[condition], function (i, item) {
            tempHtml = tempHtml.filter(":not([" + condition + "=" + item + "])");
        });
    }
    $(".drugs-table").html(tempHtml);
}
// 分页方法
var pageFun = function () {
    layui.use('laypage', function () {
        var laypage = layui.laypage
            , pageCount = Math.ceil(pageTotals / pageSize);
        laypage({
            cont: 'pageDiv',
            pages: pageCount,
            curr: pageNum || 1,
            jump: function (obj, first) {
                if (!first) {
                    pageNum = obj.curr;
                    getTableData();
                }
            }

        });
    });
}