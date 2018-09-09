$(function () {
    layui.use("form", function () {
        window.form = layui.form();
        form.on("select(hospital)", function (data) {
            var url = "/SmartWarehouse/SmartWarehouseGetByHospitalId",
                paraObj = {
                    data: {
                        hospitalId: data.value
                    }
                };
            var result = ajaxProcess(url, paraObj).Data;
            $("#smartAddDRWarehouse").empty().append("<option value>请选择</option>");
            $.each(result, function (i, item) {
                $("#smartAddDRWarehouse").append("<option value=" + item.ID + ">" + item.Name + "</option>");
            })
            form.render();
        });
    });
    $(document).on("change propertychange keydown keyup", "#smartAllocateDetailTD .layui-input", function () {
        if ($(this).val() > parseInt($(this).parents("tr").data("size"))) {
            $(this).val(parseInt($(this).parents("tr").data("size")));
        }
    });
    $(document).on("click", ".add-user", function () {
        UserInfo.setConfimFunc(function (userInfo) {
            $("[name=useUserId]").val(userInfo.id);
            $("[name=smartAddUser]").val(userInfo.name);
        }).openPop();
    });
});

var i = 1;
//显示
$("#userDiscountHtml")
    .ready(function () {
        var infoFunc = function () {
            var url = "/UserDiscount/UserDiscountGetPage";

            var realData = {};
            realData.PageNum = 1;
            realData.PageSize = 2;

            var paraObj = new Object();
            paraObj.data = realData;

            var data = ajaxProcess(url, paraObj).Data;
            pageFun(1, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数
            var interText = doT.template($("#userDiscount_template").text());
            $(".site-table").html(interText(data.PageDatas));
        };
        infoFunc();
    });

function pageFun(curr, size) {

    layui.use(['layer', 'laypage', 'element'], function () {
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

                    var url = "/UserDiscount/UserDiscountGetPage";
                    var realData = {};
                    realData.PageNum = obj.curr;
                    realData.PageSize = 2;
                    var paraObj = {};
                    paraObj.data = realData;
                    var data = ajaxProcess(url, paraObj).Data;

                    var interText = doT.template($("#userDiscount_template").text());
                    $(".site-table").html(interText(data.PageDatas));
                    pageFun(obj.curr, data.PageTotals);//测试分页数据  data.PageTotals返回的数据条数 先放到最后，可能有执行顺序的问题
                }
            }
        });
    });
};