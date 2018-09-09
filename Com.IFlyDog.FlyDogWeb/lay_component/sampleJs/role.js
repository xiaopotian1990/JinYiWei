var i = 1;
$(function () {
    layui.use(["layer", "form", "tree", "element"], function () {
        var layer = layui.layer,
            element = layui.element();
        window.form = layui.form();

    });

    getInfoRoleData();

    //添加角色
    $("#roleAdd").click(function () {
        //打开浮窗
        var url = "/Role/GetRoleMenu";
        var paraObj = {};
        var contentData = getInfoRoleMenu(url, paraObj);
        layer.open({
            type: 1,
            title: "添加角色信息",
            skin: "layerbackground_color",
            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"],
            content: contentData,
            shadeClose: false,
            success: function (layero, index) {
                layero.data("index",index);
                $(".role-num").hide();
                form.render();
                //确认提交
                layero.find(".dept_commit").bind("click", function () {

                    var url = "/Role/RoleAdd";
                    getEditData(url, index);

                    return false;
                });

                //可操作权限Tree菜单
                layui.tree({
                    elem: "#demoTree", //指定元素，生成的树放到哪个元素上
                    check: "checkbox", //勾选风格
                    skin: "treeC", //设定皮肤
                    drag: true, //点击每一项时是否生成提示信息
                    checkboxName: "checkboxname", //复选框的name属性值
                    checkboxStyle: "", //设置复选框的样式，必须为字符串，css样式怎么写就怎么写
                    click: function (item) { //点击节点回调
                        console.log(item);
                    },
                    nodes: treeData
                });

                //取消关闭
                $(".layui-btn.layui-btn-danger.dept_close").on("click", function () {
                    layer.close(index);
                    $("#RoleInfoPop").html("");
                });
            },
            cancel: function (index) {
                layer.close(index);
                $("#RoleInfoPop").html("");
            }
        });
    });

    //删除
    $(document).on("click", ".deleteBut", function () {
        var infoDeleteId = $(this).attr("infoiDeleteId");

        layer.confirm("您确定删除本条数据？",
            { btn: ["确定", "取消"] },
            function () {
                //给dto赋值
                var realData = {};
                realData.RoleID = infoDeleteId;
                realData.HospitalID = $("#smartDRWarehouse").val();
                //组合传值
                var paraObj = {};
                paraObj.data = realData;

                var url = "/Role/RoleDelete";
                var data = ajaxProcess(url, paraObj);

                if (data) {
                    if (parseInt(data.ResultType) === 0) { //请求成功返回
                        //提示消息
                        layer.msg(data.Message, { icon: 6 });
                        //刷新主页面数据.
                        getInfoRoleData();
                    } else {
                        //请求成功返回,但是后台出现错误
                        layer.msg(data.Message, { icon: 5 });
                    }
                }
                return false;
            },
            function () {
                layer.msg("已经取消此操作",
                {
                    icon: 6
                });
            });
    });

    //编辑
    $(document).on("click", ".Edit", function () {

        var url = "/Role/GetRoleDetail";

        var paraObj = {};
        paraObj.data = {
            ID: $(this).attr("infoieditid"),
            HospitalID: $("#smartDRWarehouse").val()
           // userHositalID: 1
        };

        var contentData = getInfoRoleMenu(url, paraObj);

        layer.open({
            type: 1,
            title: "修改信息",
            skin: "layerbackground_color",
            area: ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"], //宽高
            shade: [0.8, "#B3B3B3", false],
            closeBtn: 1,
            Boolean: false,
            shadeClose: false, //点击遮罩关闭
            content: contentData,
            success: function (layero, index) {
                layero.data("index", index);
                form.render();
                //确认提交
                layero.find(".dept_commit").bind("click", function () {

                    var url = "/Role/RoleUpdate";
                    getEditData(url, index);

                    return false;
                });

                //右上角关闭回调

                //可操作权限Tree菜单
                layui.tree({
                    elem: "#demoTree", //指定元素，生成的树放到哪个元素上
                    check: "checkbox", //勾选风格
                    skin: "treeC", //设定皮肤
                    drag: true, //点击每一项时是否生成提示信息
                    checkboxName: "checkboxname", //复选框的name属性值
                    checkboxStyle: "", //设置复选框的样式，必须为字符串，css样式怎么写就怎么写
                    click: function (item) { //点击节点回调
                        console.log(item);
                    },
                    nodes: treeData
                });
            },
            cancel: function (index1) {
                layer.close(index1);
                $("#RoleInfoPop").html("");
            }
        });



    });

    $(document).on("click", ".dept_close", function () {
        layer.close($(this).parents(".layui-layer").data("index"));
        $("#RoleInfoPop").html("");
    });

    $(document).on("click", "#subtmValue", function () {
        getInfoRoleData();
    });

});

var getEditData = function (url, popIndex) {

    var smartDRWarehouse = $("#Hospital").val();
    var roleinfoAddName = $("#roleInfoName").val();
    var roleinfoAddRemark = $("#roleInfoRemark").val();
    var roleinforadioNameFz = $('input[name="FZ"]:checked ').val();
    var roleinforadioNameYhry = $('input[name="YHRY"]:checked ').val();
    var roleinforadioNameCypb = $('input[name="CYPB"]:checked ').val();
    var roleinforadioNameSsyy = $('input[name="SSYY"]:checked ').val();
    var roleinforadioNameCklxfs = $('input[name="CKLXFS"]:checked ').val();
    var roleinforadioNameCkypcbj = $('input[name="CKYPCBJ"]:checked ').val();
    var check = $("input[name='checkboxname']:checked");
    var datalist = [];
    $("[lev=3]:checked").each(function (i, item) {
        datalist.push($(item).val());
    });
    var paraObj = {};
    paraObj.data = {
        Name: roleinfoAddName,
        Remark: roleinfoAddRemark,
        //UserHospitalID: "1",
        HospitalID: smartDRWarehouse,
        ActionIDS: 0,
        FZ: roleinforadioNameFz,
        YHRY: roleinforadioNameYhry,
        CYPB: roleinforadioNameCypb,
        SSYY: roleinforadioNameSsyy,
        CKLXFS: roleinforadioNameCklxfs,
        CKYPCBJ: roleinforadioNameCkypcbj,
        ActionIDS: datalist,
        ID: $("#roleInfoID").val()

    };
    var data = ajaxProcess(url, paraObj);

    if (data) {
        if (parseInt(data.ResultType) === 0) { //请求成功返回

            $("#RoleInfoPop").html("");
            var message = data.Message;
            //关闭窗口
            layer.close(popIndex);
            //提示信息
            layer.msg(message, { icon: 6 });
            //刷新主页面数据.
            getInfoRoleData();
        } else {
            //请求成功返回,但是后台出现错误
            layer.msg(data.Message, { icon: 5 });
        }
    }

    return paraObj;
}


var treeData;
//显示角色
var getInfoRoleData = function () {
    var url = "/Role/GetAllRole";
    var paraObj = {};
    paraObj.data = {
        hospitalId: $("#smartDRWarehouse").val()
    };

    //返回数据
    var result = ajaxProcess(url, paraObj).Data, innerText = doT.template($("#roleinfo_template").text());
    $("#showRoleInfo_tb").html(innerText(result));
    //$(".smartDRWarehouse").find("[value=" + result.Data.HospitalID + "]").prop("selected", true);

};

//显示角色菜单
var getInfoRoleMenu = function (url, paraObj) {
    var data = ajaxProcess(url, paraObj);

    treeData = url == "/Role/GetRoleMenu" ? data.Data : data.Data.MenuRole;
    //输出页面模版
    var innerText = doT.template($("#RoleInfoTmp").text());
    return $("#RoleInfoPop").html(innerText(data.Data));
};
