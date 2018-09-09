var i = 1;
//显示
$("#selUserhtml")
    .ready(function () {
        var infoFunc = function () {
       

            var deptId = $("#smartDeptName").val();
            var userName = $("#smartUserName").val();

            var realData = {};
          
            realData.DeptID = deptId;
            realData.Name = userName;

             var url = "/SmartUser/SmartUserGet";
            //一会还要拼接查询条件

          //  var url = "/SmartWarehouse/SmartWarehouseGet";
             var paraObj = new Object();
             paraObj.data = realData;
            var data = ajaxProcess(url, paraObj);

            var interText = doT.template($("#smartUser_template").text());
            $(".layui-field-box").html(interText(data.Data));
            form.render();
        };
        infoFunc();
    });

function btnSerache() {
    var deptId = $("#smartDeptName").val();
    var userName = $("#smartUserName").val();

    var realData = {};

    realData.DeptID = deptId;
    realData.Name = userName;

    var url = "/SmartUser/SmartUserGet";
    //一会还要拼接查询条件

    //  var url = "/SmartWarehouse/SmartWarehouseGet";
    var paraObj = new Object();
    paraObj.data = realData;
    var data = ajaxProcess(url, paraObj);

    var interText = doT.template($("#smartUser_template").text());
    $(".layui-field-box").html(interText(data.Data));
    form.render();
}