// 关闭弹窗后执行的事件
var closeLayer = function (obj, func) {
    var layerIndex = $(obj).parents(".layui-layer").data("index") ? $(obj).parents(".layui-layer").data("index") : $(obj).parents(".layui-layer").attr("times");
    layer.close(layerIndex);
    var forms = $(obj).parents(".layui-layer").find("form");
    forms.find("[type=hidden]").val("");
    if (forms.length > 0) {
        forms[0].reset();
    }
    typeof (form) != "undefined" ? form.render() : "";
    typeof (func) != "undefined" ? func() : "";
}
// 打开弹窗
var openPopWithOpt = function (opt) {
    layer.open({
        type: 1,
        title: opt.title,
        area: opt.area,
        shade: [0.8, '#B3B3B3', false],
        closeBtn: 1,
        shadeClose: false, //点击遮罩关闭
        content: $(opt.popEle),//#aaa  span   .aaa
        success: function (layero, index) {
            typeof (opt.func) != "undefined" ? opt.func(layero) : "";
            layero.data("url", opt.url).data("index", index);
            // 右上角关闭按钮
            layero.on("click", ".close-layer,.dept_close,.layui-layer-close", function () {
                closeLayer(this);
            });
            typeof (form) != "undefined" ? form.render() : "";
        }
    });
}
var openPop = function (url, popEle, title, func) {
    var opt = {};
    opt.title = title;
    opt.url = url;
    opt.popEle = popEle;
    opt.func = func;
    opt.area = ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"];
    openPopWithOpt(opt);
}


var Params = function () {
    this.data = {};
    this.setDataParam = function (paramName, paramValue) {
        this.data[paramName] = paramValue;
        return this;
    }
    this.setDataParams = function (paramData) {
        for (paramName in paramData) {
            this.data[paramName] = paramData[paramName];
        }
        return this;
    }
}
var AjaxObj = function (obj) {
    var ajaxObj = {
        url: "",
        paraObj: {},
        isUpdate: false,
        dataCallBack: null,
        dotEle: [{ container: "", tmp: "" }],
        isDataRoot: false,
        hasPage: false,
        pageDiv:""
    };
    if (obj) {
        for (fieldName in obj) {
            !fieldName in ajaxObj || (ajaxObj[fieldName] = obj[fieldName])
        }
    }
    var init = function () {
        window.params = new Params();
    }
    init();
    this.constructor = function AjaxObj() { },
    // 设置访问url
    this.setUrl = function (url) {
        ajaxObj.url = url;
        return this;
    },
    // 获取访问url
    this.getUrl = function () {
        return ajaxObj.url;
    },
    // 设置提交数据对象
    this.setParaObj = function (paraObj) {
        ajaxObj.paraObj = paraObj;
        return this;
    },
    // 获取提交数据对象
    this.getParaObj = function () {
        return setObjFieldEmpty(ajaxObj.paraObj, true);
    },
    // 修改方法
    this.setIsUpdateTrue = function () {
        ajaxObj.isUpdate = true;
        return this;
    },
    // 非修改方法
    this.setIsUpdateFalse = function () {
        ajaxObj.isUpdate = false;
        return this;
    },
    // 获取是否修改方法
    this.getIsUpdate = function () {
        return ajaxObj.isUpdate;
    },
    // 设置回调方法
    this.setDataCallBack = function (dataCallBack) {
        ajaxObj.dataCallBack = dataCallBack;
        return this;
    },
    // 设置数据容器
    this.getDataCallBack = function () {
        return ajaxObj.dataCallBack;
    },
    // 设置数据容器
    this.setDotEle = function (dotEle) {
        ajaxObj.dotEle = dotEle;
        return this;
    },
    // 获取数据容器
    this.getDotEle = function () {
        return ajaxObj.dotEle;
    },
    // 数据对象是顶级属性
    this.setIsDataRootTrue = function () {
        ajaxObj.isDataRoot = true;
        return this;
    },
    // 数据对象不是顶级属性
    this.setIsDataRootFalse = function () {
        ajaxObj.isDataRoot = false;
        return this;
    },
    // 获取数据对象是否是顶级属性
    this.getIsDataRoot = function () {
        return ajaxObj.isDataRoot;
    },
    // 使用分页
    this.usePage = function (pageEle) {
        ajaxObj.pageDiv = pageEle;
        ajaxObj.hasPage = true;
        return this;
    },
    // 不是用分页
    this.unPage = function () {
        ajaxObj.hasPage = false;
        return this;
    },
    // 获取是否使用分页
    this.getHasPage = function () {
        return ajaxObj.hasPage;
    }
    // 获取分页元素
    this.getPageDiv = function () {
        return ajaxObj.pageDiv;
    }
    // 查询数据
    this.getData = function () {
        return dataFunc(this);
    }
}
var setObjFieldEmpty = function (data, isFalseSetEmpty) {
    if (Common.StrUtils.isNullOrEmpty(data)) return isFalseSetEmpty ? "" : [];
    for (var item in data) {
        if (typeof (data[item]) == "object") {
            data[item] = setObjFieldEmpty(data[item]);
            continue;
        }
        data[item] = isNaN(data[item]) ? Common.StrUtils.isFalseSetEmpty(data[item]) : data[item];
    }
    return data;
}
// 数据请求对象
window.ajaxObj = new AjaxObj();
var fillData = function (container, tmp, data, isAppend) {
    data = setObjFieldEmpty(data);
    // 将数据填入模版并且把填充后的dom添加页面容器中
    if (isAppend) {
        $(container).append(doT.template($(tmp).text())(data));
    } else {
        $(container).html(doT.template($(tmp).text())(data));
    }
}
// 数据请求方法
var dataFunc = function (ajaxObj) {
    if (!(ajaxObj instanceof AjaxObj)) {
        ajaxObj = new AjaxObj(ajaxObj);
    }
    var ajaxObj = ajaxObj;
    window.ajaxObj = new AjaxObj();
    // ajaxObj.url   请求路径
    // ajaxObj.paraObj   请求对象
    // ajaxObj.isUpdate   请求是否是修改信息
    // ajaxObj.dataCallBack   请求完成的回调函数，参数为请求后返回的数据
    // ajaxObj.dotEle   数组，dot[n].container  数据容器，dot[n].tmp   返回数据模版，用于填充数据
    // ajaxObj.isDataRoot     请求返回的数据是否可以直接用做模版数据填充
    // ajaxObj.hasPage   返回数据是否有分页
    var result = ajaxProcess(ajaxObj.getUrl(), ajaxObj.getParaObj());

    if (ajaxObj.getIsUpdate()) {
        layer.msg(result.Message, { icon: result.ResultType + 1 });
        // 用于刷新主列表数据
        typeof (getPageData) != "undefined" ? getPageData() : "";
    } else {
        if (Common.StrUtils.isNullOrEmpty(ajaxObj.getDataCallBack())) {
            // 遍历需要填充数据的模版
            for (var i = 0; i < ajaxObj.getDotEle().length; i++) {
                var container = ajaxObj.getDotEle()[i].container,
                    tmp = ajaxObj.getDotEle()[i].tmp;
                // 将数据填入模版并且把填充后的dom添加页面容器中
                fillData(container, tmp, ajaxObj.getIsDataRoot() ? result : (ajaxObj.getHasPage() ? result.Data.PageDatas : result.Data));
                // 判断数据是否被转换成字符串，并且转回字符串，用作后续操作
                ajaxObj.getParaObj().data = typeof (ajaxObj.getParaObj().data) == 'string' ? JSON.parse(ajaxObj.getParaObj().data) : ajaxObj.getParaObj().data;
                // 判断是否有分页
                if (ajaxObj.getHasPage() && ajaxObj.getParaObj().data.PageNum == 1) {
                    laypage({
                        cont: ajaxObj.getPageDiv() ? ajaxObj.getPageDiv() : 'pageDiv', //分页容器的id
                        pages: Math.ceil(parseInt(result.Data.PageTotals) / pageSize), //总页数
                        skip: true, //开启跳页
                        jump: function (obj, first) {
                            ajaxObj.getParaObj().data.PageNum = obj.curr;
                            if (!first) {
                                dataFunc(ajaxObj);
                            }
                        }
                    });
                }

            }
        } else {
            ajaxObj.getDataCallBack()(result);
        }
    }
    typeof (form) != "undefined" ? form.render() : "";
    return result;

}
// 通用Ajax操作,推荐调用此方法进行,可以在真正调用之前对数据对象进行必要的设置.
function ajaxProcess(url, paraObj) {
    //拼接rootPath
    //url = url.indexOf(rootPath)==0?url:url.indexOf("/")==0?(rootPath+url):url;
    //拼接流程记录id
    //url = addRecordIds(url);
    //url = addTime(url);
    return ajaxServlet(url, paraObj);
}

function ajaxServlet(url, paraObj) {
    var result = "false";
    var isGet = false;// 是否为Get方式
    var handleAs = "json";// 预期服务器返回的数据类型"text","json","script","xml",html,jsonp
    var isSync = true;// 是否同步
    var loadFunction = null;
    var errorFunc = null;
    var processData = true;
    var contentType = 'text/plain';
    var isRequestJson = true; //增加后台请求的json开关，默认设置为true，以字符串的形式进行请求，若是需要强制前台数据使用json字符串格式，则需要在paraObj中进行显式的设置！
    if (paraObj != null) {
        result = paraObj.result == null ? result : paraObj.result;
        isGet = paraObj.isGet == null ? isGet : paraObj.isGet;
        handleAs = paraObj.handleAs == null ? handleAs : paraObj.handleAs;
        //form =paraObj.form==null?form:paraObj.form;
        isSync = paraObj.isSync == null ? isSync : paraObj.isSync;
        loadFunction = paraObj.loadFunction == null ? loadFunction : paraObj.loadFunction;
        errorFunc = paraObj.errorFunc == null ? errorFunc : paraObj.errorFunc;
        processData = paraObj.processData == null ? processData : paraObj.processData;
        contentType = paraObj.contentType == null ? contentType : paraObj.contentType;
        isRequestJson = paraObj.isRequestJson == null ? isRequestJson : paraObj.isRequestJson;
    }
    if (loadFunction == null || typeof (loadFunction) === "undefined") {
        loadFunction = function (data, status, jqXHR) {
            result = handleAs == "text" ? trim(data) : data;
        };
    }
    if (errorFunc == null || typeof (errorFunc) == "undefined") {
        errorFunc = function (jqXHR, textStatus, errorThrown) {
            result = "false";
        };
    }
    if (isRequestJson) {
        if (paraObj.data != undefined && paraObj.data != '' && typeof (paraObj.data) != 'string' && typeof (paraObj.data) == 'object') {

            paraObj.data = JSON.stringify(paraObj.data);//将所有的json对象转换为json字符串传到后端
        }
    }

    $.ajax({
        url: url,
        async: !isSync,  //是否异步
        dataType: handleAs,  //预期服务器返回的数据类型
        type: isGet ? "GET" : "POST",  //请求方式
        processData: processData,
        contentType: 'application/json',//contentType值为text/json时为java的请求方式,并且后台javacontroller需要加入@RequestBody的注解.
        //contentType: contentType,
        //header:'Access-Control-Allow-Origin',
        //data: isGet||!form?"":$('#'+form.getAttribute('id')).serialize(),//不可用form.id，form.id会取form中id为'id'的元素
        data: paraObj.data,
        success: loadFunction,     //loadFunction , //成功回调方法
        error: errorFunc
    });
    return result;
}

// 上传文件
function FileUpload(imagepath, successFun, fileElementId) {
    // document.domain = 'localhost';
    $.ajaxFileUpload
    (
        {
            url: '/Picture/UploadImage?imagepath=' + imagepath,
            secureuri: false,
            dataType: 'json',
            type: 'post',
            data: { enctype: "multipart/form-data" },
            fileElementId: fileElementId,
            success: function (data) {
                successFun(data);

            },
            error: function (e) {
                //alert(e.ResultMsg);
            }
        }
    );
}
function closeCurrentWindow(index, time) {
    setTimeout(function () {
        layer.close(index);
    }, time);
}
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//打开新的//iframe层
var openiframe = function (opi) {
    layer.open({
        //type: 2,
        type: 1,
        title: opi.title,
        shadeClose: false,
        shade: [0.8, "#B3B3B3", false],
        area: opi.area,
        content: $("#" + opi.contentDiv),
        success: function (layero, index) {
            typeof (opi.func) != "undefined" ? opi.func(layero) : "";
            layero.data("url", opi.url).data("index", index);
            // 右上角关闭按钮
            layero.on("click", ".close-layer,.dept_close,.layui-layer-close", function () {
                closeLayer(this);
            });
            form.render();
        },
        cancel: function (index, layero) {
            $("#" + opi.contentDiv).html("");
        }
    });

}
var openParam = function (title, url, conurl, contentDiv, func) {
    //"编辑咨询", "", "AddConsult", iframeCallBack
    var opi = {};
    opi.title = title;
    opi.url = url;
    opi.conurl = conurl;
    opi.func = func;
    opi.contentDiv = contentDiv;
    opi.area = ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"];
    openiframe(opi);

}

var openDynamicParam = function (title, url, conurl, contentDiv, width, height, func) {
    //"编辑咨询", "", "AddConsult", iframeCallBack
    var opi = {};
    opi.title = title;
    opi.url = url;
    opi.conurl = conurl;
    opi.func = func;
    opi.contentDiv = contentDiv;
    opi.area = [width, height];
    openiframe(opi);

}

var selectOptionOperation = {
    setOptionSelected: function (selectTag, defaultValue) {
        //对通过layui生成的在iframe中的select标签进行默认option选中的操作，在option中设置attr("selected", "selected")实现不了效果。
        var tempValue = defaultValue ? defaultValue : "";
        var addcustomerConsultTool = selectTag;
        $(addcustomerConsultTool).find("option").removeAttr("selected");//移除所有option下面的selected属性
        $($(addcustomerConsultTool).find("[value=" + tempValue + "]")[0]).attr("selected", "selected"); //单纯此种方式无法实现默认选中。
        var addcustomerConsultToolParent = $(addcustomerConsultTool).parent();
        var addcustomerConsultToolDl = $(addcustomerConsultToolParent).find("dl")[0];
        $(addcustomerConsultToolDl).find("dd").attr("class", "");
        $($(addcustomerConsultToolDl).find("[lay-value='" + tempValue + "']")[0]).attr("class", "layui-this");
        var tempText = $($(addcustomerConsultToolDl).find("[lay-value='" + tempValue + "']")[0]).text();
        $($(addcustomerConsultToolParent).find(".layui-input.layui-unselect")[0]).attr("value", tempText);
    }
};
function loadScript(url, callback) {
    var script = document.createElement("script");
    script.type = "text/javascript";
    if (typeof (callback) != "undefined") {
        if (script.readyState) {
            script.onreadystatechange = function () {
                if (script.readyState == "loaded" || script.readyState == "complete") {
                    script.onreadystatechange = null;
                    callback();
                }
            };
        } else {
            script.onload = function () {
                callback();
            };
        }
    }
    script.src = url;
    document.body.appendChild(script);
}
(function (win, $) {
    // 获取用户弹窗dom
    var getuserPageContentFunc = function (url) {
        return ajaxObj.setUrl(url).setParaObj({ handleAs: "html" }).getData();
    }

    var model = new Model();

    function Model() {
        var model = ""; // 模块名称
        var viewUrl = ""; // 弹窗请求的html内容的url
        var successCallBack = null; // 弹窗html内容加载完成后的回调函数
        var openPopCallBack = null; // 打开弹窗完成后的回调函数
        var pageContent = ""; // 获取到的弹窗页面html
        var popContent = ""; // 弹窗html内容
        var popTitle = ""; // 弹窗标题
        var submitUrl = ""; // 提交的url
        var isUseEmptyEntry = true; // 使用空对象填充数据
        var emptyEntry = {}; // 空对象
        var fillEntry = {}; // 填充数据对象
        var area = ""; // 弹窗大小

        var init = function (elem) {
            pageContent = $("<div class='model-" + model + "'>" + getuserPageContentFunc(viewUrl) + "</div>");
            popContent = elem ? pageContent.find(elem) : pageContent.children();
            if ($(".page-models").length == 0) {
                $("body").append("<div class='page-models'></div>");
            }
            $(".page-models").append(pageContent);
            successCallBack && successCallBack(pageContent);
        }

        this.constructor = function Model() {
        };

        // 初始化页面（获取弹窗内容）
        this.init = function (modelName, url, title, subUrl, entry, success, open, elem) {
            model = modelName;
            viewUrl = url;
            popTitle = title;
            submitUrl = subUrl;
            openPopCallBack = open;
            successCallBack = success;
            entry && (emptyEntry = entry);
            window[model] = this;
            init(elem);
            window.Model = new Model();
            return window[model];
        }
        this.setUrl = function (url) {
            viewUrl = url; return this;
        }
        this.getUrl = function () {
            return viewUrl;
        }
        // 打开用户列表弹窗
        this.openPop = function () {
            var opt = {};
            opt.title = popTitle;
            opt.url = submitUrl;
            opt.popEle = popContent;
            opt.func = function (layero) {
                openPopCallBack && openPopCallBack(layero, pageContent, isUseEmptyEntry ? emptyEntry : fillEntry);
            };
            opt.area = area ? area : ["35%;min-width:680px;max-width:800px", "65%;min-height:500px;max-height:600px"];
            openPopWithOpt(opt);
            return this;
        }
        // 获取弹窗dom的jquery对象
        this.getPopContent = function () {
            return popContent;
        }
        // 设置模块名称
        this.setModelName = function (modelName) {
            model = modelName;
            return this;
        }
        // 获取模块名称
        this.getModelName = function () {
            return model;
        }
        // 设置弹窗标题
        this.setPopTitle = function (title) {
            popTitle = title;
            return this;
        }
        // 获取弹窗标题
        this.getPopTitle = function () {
            return popTitle;
        }
        // 设置打开弹窗回调函数
        this.setOpenPop = function (open) {
            openPopCallBack = open;
            return this;
        }
        // 获取打开弹窗回调函数
        this.getOpenPop = function () {
            return openPopCallBack;
        }
        // 设置请求html成功回调函数
        this.setSuccess = function (success) {
            successCallBack = success;
            return this;
        }
        // 获取请求html成功回调函数
        this.getSuccess = function () {
            return successCallBack;
        }
        // 设置请求的url
        this.setSubmitUrl = function (url) {
            submitUrl = url;
            return this;
        }
        // 获取请求的url
        this.getSubmitUrl = function () {
            return submitUrl;
        }
        // 设置使用空对象填充数据
        this.useEmptyEntry = function () {
            isUseEmptyEntry = true;
            return this;
        }
        // 设置不使用空对象填充数据
        this.unUseEmptyEntry = function () {
            isUseEmptyEntry = false;
            return this;
        }
        // 获取使用空对象填充数据
        this.getUseEmptyEntry = function () {
            return isUseEmptyEntry;
        }
        // 设置空对象
        this.setEmptyEntry = function (entry) {
            entry && (emptyEntry = entry);
            return this;
        }
        // 获取空对象
        this.getEmptyEntry = function () {
            return emptyEntry;
        }
        // 设置填充数据对象
        this.setEntry = function (entry) {
            entry && (fillEntry = entry);
            return this;
        }
        // 获取填充数据对象
        this.getEntry = function () {
            return fillEntry;
        }
        // 设置弹窗大小
        this.setArea = function (popArea) {
            area = popArea;
            return this;
        }
        // 获取弹窗大小
        this.getArea = function () {
            return area;
        }
    }

    window.Model = model;

})(window, jQuery);
