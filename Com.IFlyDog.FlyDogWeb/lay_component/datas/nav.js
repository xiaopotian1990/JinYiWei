var navs;
$.ajaxSettings.async = false;
$.getJSON("/Home/GetMenu", function (data) {
//    console.log(data);
    navs = data;
});
$.ajaxSettings.async = true;