$(function () {
    layui.use(["form", "layer","laypage"], function () {
        var layer = layui.layer;
        window.form = layui.form();
        getPageData();
    });
});
var getHospitalData = function () {
    var dotEle = [{ container: ".hospital-table", tmp: ".hospital-tmp" }];
    ajaxObj.setUrl("/Hospital/HospitalGet").setDotEle(dotEle).getData();
}
getPageData = getHospitalData;