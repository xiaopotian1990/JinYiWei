;(function ($) {
    window.TimePlugin = function (time, parentEle) {
        var preEle = parentEle ? parentEle : "";
        var hour = 0;
        var minute = 0;
        var selectFunc = function () { };
        var timePlugin = null;
        var _thisPlugin = null;
        var initTime = function (time) {
            time = (time + "").split(":");
            if (Common.StrUtils.isNullOrEmpty(time[0] = time[0].replace(" ", ""))) {
                throw "hour is undefined";
            }
            if (isNaN(time[0])) {
                throw "hour is not number";
            }
            hour = time[0];
            if (time.length > 1 && isNaN(time[1])) {
                throw "minute is not number";
            } else if (!Common.StrUtils.isNullOrEmpty(time[1])) {
                minute = time[1];
            }
        }
        // 初始化时间控件  isReApply：是否重新渲染
        var init = function (isReApply) {
            if (timePlugin && !isReApply) {
                timePlugin.find("[hour=" + hour + "]").click();
                timePlugin.find("[minute=" + minute + "]").click();
            } else {
                timePlugin = $("<div>").addClass("timer");
                timePlugin.append(createHourEle(hour)).append("<div class='colon'>:</div>").append(createMinuteEle(minute));
                timePlugin.on("click", ".hour,.minute", function () {
                    $(this).toggleClass("open");
                });
                timePlugin.on("click", "li", function () {
                    var _this = $(this);
                    _this.addClass("active").siblings("li").removeClass("active");
                    if (_this.parent().is(".hour")) {
                        hour = _this.text();
                    } else if (_this.parent().is(".minute")) {
                        minute = _this.text();
                    }
                    selectFunc(_thisPlugin.getTime(), _thisPlugin);
                });
                timePlugin.show();
                $(document).on("click", ":not([type=checkbox]),:not([type=radio]),:not([type=button]),:not(.hour),:not(.minute)", function (event) {
                    var _this = $(this);
                    _this = _this.hasClass("hour") ? _this : _this.parent();
                    var openClass = _this.hasClass("open") ? "" : "open";
                    $(".hour,.minute").removeClass("open");
                    if (_this.hasClass("hour") || _this.hasClass("minute")) {
                        _this.addClass(openClass);
                    }
                    event.stopPropagation();
                });
                $(preEle).append(timePlugin);
            }
        };
        var createHourEle = function (now) {
            var hourEle = $("<ul>").addClass("hour");
            for (var i = 0 ; i < 24; i++) {
                var num = i + "";
                num = num.length == 1 ? "0" + num : num;
                hourEle.append($("<li>").attr("hour", num).text(num).addClass(i == now ? "active" : "")).data("now", i == now ? num : "");
            }
            return hourEle;
        };
        var createMinuteEle = function (now) {
            var minuteEle = $("<ul>").addClass("minute");
            for (var i = 0 ; i < 60; i++) {
                var num = i + "";
                num = num.length == 1 ? "0" + num : num;
                minuteEle.append($("<li>").attr("minute", num).text(num).addClass(i == now ? "active" : "")).data("now", i == now ? num : "");
            }
            return minuteEle;
        }
        this.constructor = function TimePlugin() { };
        // 初始化
        this.init = function () {
            _thisPlugin = this;
            init();
            return this;
        };
        // 隐藏插件
        this.close = function () {
            timePlugin.hide();
            return this;
        };
        // 删除插件
        this.remove = function () {
            timePlugin.remove();
            return this;
        };
        // 设置获取选中事件
        this.setSelectFunc = function (SelectFunc) {
            selectFunc = SelectFunc;
            return this;
        };
        this.getSelectFunc = function () {
            return selectFunc;
        };
        // 设置获取时间
        this.setTime = function (time) {
            initTime(time);
            init(true);
            return this;
        };
        this.getTime = function () {
            return hour + ":" + minute;
        };
        // 设置获取父元素
        this.setParentEle = function (parentEle, isRemoveOld) {
            if (isRemoveOld) {
                $(preEle).find(".timer").remove();
            }
            preEle = parentEle;
            init(true);
            return this;
        };
        this.getParentEle = function () {
            return preEle;
        };
        // 初始化时间
        initTime(time);
    }
})(jQuery);