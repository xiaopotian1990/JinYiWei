(function (win, $) { 

  
var successFunc = function (pageContent) {
    //删除标签

    //提交
    pageContent.on("click", ".Tags.add-submit", function () {  
    
                params.setDataParams({
                    CustomerID: custid,
                    Tags: tags
                }); 
            });
            if (ajaxObj.setUrl("/CustomerProfile/" + $(this).parents(".layui-layer").data("url")).setIsUpdateTrue().setParaObj(params).ResultType === 0) {
                closeLayer(this);
            }
    }); 
};
var openFunc = function (layero, pageContent, data) {
    // 填充页面模版到页面容器中
    fillData(pageContent.find(".surgery-form"), pageContent.find(".surgery-tmp"), data); 
}
    
var MakeBeginTime = null, MakeEndTime = null,
// 填充页面模版到页面容器中
emptyFormData = {};
Model.init("AddTags", "AddTags", "添加顾客标签", "AddTags", emptyFormData, successFunc, function (layero, pageContent, data) { openFunc(layero, pageContent, data); });


})(window, jQuery);