$(function() {
    layui.use(["form", "layer", "laydate"], function () {
        var layer = layui.layer, laydate = layui.laydate();
        window.form = layui.form();
        form.render();
    });
 
    surgeryGet();

    /*开始手术*/
    $(".layui-field-box").on("click", ".start-Surgery-btn", function () {
        var status= $(this).attr("surgerystatus");
        var suId = $(this).attr("surgeryid");
        sugerydone(status, suId);
    });
    /*手术中...*/
    $(".layui-field-box").on("click", ".surgerying-btn", function () {
        layer.tips("正在手术中..", $(this));
    });
    /*手术已完成*/
    $(".layui-field-box").on("click", ".complete-btn", function () {
        layer.tips("该手术已经完成!", $(this));
    });
    /*手术结束*/
    $(".layui-field-box").on("click", ".end-Surgery-btn", function () {
        var status = $(this).attr("surgerystatus");
        var suId = $(this).attr("surgeryid");
        sugerydone(status, suId);
     //   layer.confirm("是否要结束手术", {
        //      btn: ['确认', '取消'] //按钮
        //   }, function () {
        //  }, function () {layer.msg('已经取消!', {});
        // });
      
    });
 
});
//查询手术
var thdate = new Date();
 
var surgeryGet = function () {
    var url = "/SurgeryDesk/SurgeryGet";
    var paraObj = {
        data: { datetime: $("input[name='selsugeryTime']").val() === "" ? thdate.toLocaleDateString() : $("input[name='selsugeryTime']").val() }
    };
    var result = ajaxProcess(url, paraObj);
    //判断返回数据是否成功
    if (result.ResultType === 0) {
        if (result.Data.length > 0) {
            /*在doT模版输出数据*/
            var interText = doT.template($(".surgeryDesk-temp").text());
            $(".tb-surgeryDesk-info").html(interText(result.Data));
            $(".tb-surgeryDesk-null").html("");
        } else {
            var html = "<blockquote class='layui-elem-quote'>" + "暂无数据" + "</blockquote>";
            $(".tb-surgeryDesk-info").html("");
            $(".tb-surgeryDesk-null").html(html);
          
        }
    }
};
 

//手术结束与开始
var sugerydone = function (status,suId) {
    var ajaxObj = {
        url: "/SurgeryDesk/SugeryDone",
        paraObj: {
            data: {
                ID: suId,
                Status: status,
                Time: thdate.toLocaleTimeString()
            }
        }
    };
    dataFunc(ajaxObj).ResultType === 1;
    surgeryGet();
};
