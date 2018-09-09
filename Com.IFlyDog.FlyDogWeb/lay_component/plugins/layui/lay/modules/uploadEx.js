/*!
 @Title: layui.uploadEx 单文件上传修改版 - 全浏览器兼容版
 @Author: Beginner
 @WebSite:http://beginner.zhengjinfan.cn/demo/beginner_admin/
 @Date 2016-12-15 18:00
 @License：LGPL
 */
layui.define("layer",
    function(e) {
        var f = layui.jquery;
        var g = layui.layer;
        var b = layui.device();
        var a = "layui-upload-enter";
        var h = "layui-upload-iframe";
        var d = { icon: 2, shift: 6 }, i = { file: "文件", video: "视频", audio: "音频" };
        var c = function(j) { this.options = j };
        c.prototype.init = function() {
            var n = this, k = n.options;
            var j = f("body"), m = f(k.elem || ".layui-upload-file");
            var l = f('<iframe id="' + h + '" class="' + h + '" name="' + h + '"></iframe>');
            f("#" + h)[0] || j.append(l);
            return m.each(function(o, r) {
                r = f(r);
                var q = '<form target="' +
                    h +
                    '" method="' +
                    (k.method || "post") +
                    '" key="set-mine" enctype="multipart/form-data" action="' +
                    (k.url || "") +
                    '"></form>';
                var p = r.attr("lay-type") || k.type;
                if (!k.unwrap) {
                    q = '<div class="layui-box layui-upload-button">' +
                        q +
                        '<span class="layui-upload-icon"><i class="layui-icon">&#xe608;</i>' +
                        (r.attr("lay-title") || k.title || ("上传" + (i[p] || "图片"))) +
                        "</span></div>"
                }
                q = f(q);
                if (!k.unwrap) {
                    q.on("dragover",
                            function(s) {
                                s.preventDefault();
                                f(this).addClass(a)
                            })
                        .on("dragleave", function() { f(this).removeClass(a) })
                        .on("drop", function() { f(this).removeClass(a) })
                }
                if (r.parent("form").attr("target") === h) {
                    if (k.unwrap) {
                        r.unwrap()
                    } else {
                        r.parent().next().remove();
                        r.unwrap().unwrap()
                    }
                }
                r.wrap(q);
                r.off("change").on("change", function() { n.action(this, p) })
            })
        };
        c.prototype.action = function(u, t) {
            var r = this, y = r.options, m = u.value;
            var x = f(u), l = x.attr("lay-ext") || y.ext || "";
            if (!m) {
                return
            }
            switch (t) {
            case"file":
                if (l && !RegExp("\\w\\.(" + l + ")$", "i").test(escape(m))) {
                    g.msg("不支持该文件格式", d);
                    return u.value = ""
                }
                break;
            case"video":
                if (!RegExp("\\w\\.(" + (l || "avi|mp4|wma|rmvb|rm|flash|3gp|flv") + ")$", "i").test(escape(m))) {
                    g.msg("不支持该视频格式", d);
                    return u.value = ""
                }
                break;
            case"audio":
                if (!RegExp("\\w\\.(" + (l || "mp3|wav|mid") + ")$", "i").test(escape(m))) {
                    g.msg("不支持该音频格式", d);
                    return u.value = ""
                }
                break;
            default:
                if (!RegExp("\\w\\.(" + (l || "jpg|png|gif|bmp|jpeg") + ")$", "i").test(escape(m))) {
                    g.msg("不支持该图片格式", d);
                    return u.value = ""
                }
                break
            }
            if (y.size !== undefined && typeof(y.size) === "number") {
                var p = y.size * 1024 * 1024;
                try {
                    var w = u;
                    if (w.value == "") {
                        g.msg("请先选择上传文件", d);
                        return
                    }
                    if (!b.ie) {
                        var v = 0, q = "", j = w.files;
                        for (var o = 0; o < j.length; o++) {
                            if (j[o].size > p) {
                                v++;
                                q = "文件名为：" + j[o].name + " 不能超过" + y.size + "M！";
                                break
                            }
                        }
                        if (v > 0) {
                            g.msg(q, d);
                            return
                        }
                    }
                } catch (s) {
                    g.msg(s.message, d);
                    return
                }
            }
            y.before && y.before(u);
            x.parent().submit();
            var n = f("#" + h),
                k = setInterval(function() {
                        var z;
                        try {
                            z = n.contents().find("body").text()
                        } catch (A) {
                            g.msg("上传接口存在跨域", d);
                            clearInterval(k)
                        }
                        if (z) {
                            clearInterval(k);
                            n.contents().find("body").html("");
                            try {
                                z = JSON.parse(z)
                            } catch (A) {
                                z = {};
                                return g.msg("请对上传接口返回JSON字符", d)
                            }
                            typeof y.success === "function" && y.success(z, u)
                        }
                    },
                    30);
            u.value = ""
        };
        e("uploadEx",
            function(k) {
                var j = new c(k = k || {});
                j.init()
            })
    });