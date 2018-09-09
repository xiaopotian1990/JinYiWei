(function (win,$) {
    var pageIndex = 1;
    var TreeDate = {};
    var selNode = 0;
    var pageSize = 15;
    pageLoad();
    function pageLoad() {
        loadTree();
        LoadRightContent(0);
        //绑定页面操作事件
        $("#dvImageTree").on("click", "li", function () {
            LoadRightContent($(this).data("value"));
            selNode = $(this).data("value");
            return false;
        });
    }

    //加载店铺自定义商品分类
    function LoadShopProductType() {
        var path = "/ShopGoodsCategory/GetShopProductCategory";
        Common.Ajax.Post(path, {}, CreateProductTypeTree);
    }

    //加载店铺自定义分类商品时创建数据
    function CreateProductTypeTree(data) {
        var treeHtml = "";
        $.each(data, function (index, ele) {
            if (ele.ParentId === 0) {
                treeHtml += '<h4 class="store-class close">' + ele.StrProductTypeName + '</h4>';
                treeHtml += '<ul class="store-class-ul" data-id="' + ele.Id + '">';
                $.each(data, function (i, cEle) {
                    if (ele.Id === cEle.ParentId) {
                        treeHtml += '<li><input type="checkbox" value="' + cEle.Id + '">' + cEle.StrProductTypeName + '</li>';
                    }
                });

                treeHtml += '</ul>';
            }
        });
        $("#dvShopProductType").html(treeHtml);
    }

    //开始加载图库

    //加载左侧分类树
    function loadTree() {
        var path = "/ImageManager/GetImageType";
        Common.Ajax.Post(path, "", function (obj) {
            TreeDate = obj;//这个得问一下干嘛的
            var strHtml = CreateTreeNode(0);
            $("#dvImageTree").html(strHtml);
        }, false);
    }

    //创建树节点
    function CreateTreeNode(nodeId) {
        var strHtml = "";
        $.each(TreeDate, function (index, ele) {
            if (ele.ParentId == nodeId) {
                if (ele.ParentId == 0) {
                    strHtml += '<li data-value="' + ele.Id + '">';
                    strHtml += '<h5><i></i>' + ele.StrCatalogName + '</h5>';
                    strHtml += '<ul>';
                    strHtml += CreateTreeNode(ele.Id);
                    strHtml += '</ul>';
                    strHtml += '</li>';
                } else {
                    if (!HasChildNode(ele.Id)) {
                        strHtml += ' <li data-value="' + ele.Id + '" class="nav-close">'
                            + '<h5><i></i>' + ele.StrCatalogName + '</h5></li>';
                    } else {
                        strHtml += '<li data-value="' + ele.Id + '" class="nav-close">';
                        strHtml += '<h5><i></i>' + ele.StrCatalogName + '</h5>';
                        strHtml += '<ul>';
                        strHtml += CreateTreeNode(ele.Id);
                        strHtml += '</ul>';
                        strHtml += '</li>';
                    }
                }
            }
        });
        return strHtml;
    }

    //检测是否含有子节点
    function HasChildNode(nodeId) {
        var isHav = false;
        $.each(TreeDate, function (index, ele) {
            if (ele.ParentId == nodeId) {
                isHav = true;
                return false;
            }
        });
        return isHav;
    }

    var LoadRightContentPage = function (pageIndex1) {
        LoadRightContent(selNode, pageIndex1);
    }

    win.LoadRightContentPage = LoadRightContentPage;

    function LoadRightContent(nodeId, pageIndex1) {
        if (!nodeId) nodeId = selNode;//这种的也不知道
        if (pageIndex1) pageIndex = pageIndex1;
        var path = "/ImageManager/GetImageTypeAndInfo";
        var data = { parentId: nodeId, pageIndex: pageIndex, pageSize: pageSize };

        var createContent = function (id, name, className, imgPath, parentId, imgKey) {
            var html = "";
            html += '<li data-id="' + id + '" data-key="' + imgKey + '" data-parentId = "' + parentId + '"><img src="' + imgPath.replace(/\\/g, '/') + '" alt=""><div></div></li>';
            return html;
        }


        Common.Ajax.Post(path, data, function (obj) {
            var html = "";
            $.each(obj.ImageList, function (index, ele) {
                html += createContent(ele.Id, ele.StrName, "", ele.ImagePath, ele.ImageCatalogId, ele.ImageKey);
            });

            $("#ul_content").html(html);
            pageCount = obj.pageCount;
            //分页
            pager(pageIndex, pageCount, "LoadRightContentPage", ".page");
        });
    }

    function LoadCategoryFloder(a, b) {
        var parentNodeId = 0, successFun;

        if (typeof a == "string" || typeof a == "number") parentNodeId = a;
        if (typeof a == "function") successFun = a;
        if (typeof b == "function") successFun = b;

        var path = "/ImageManager/GetChildCategory";
        Common.Ajax.Post(path, { ParentId: parentNodeId }, function (obj) {
            if (obj) {
                var strHtml = "";
                $.each(obj, function (index, ele) {
                    strHtml += '<li data-id="' + ele.Id + '">';
                    strHtml += '<div class="folder-img"></div>';
                    strHtml += '<div class="folder-name">' + ele.StrCatalogName + '</div>';
                    strHtml += '</li>';
                });

                if (!strHtml.isNullOrEmpty()) {
                    $(".folder-list").html(strHtml);
                }

                if (successFun && typeof successFun == "function") {
                    successFun();
                }
            }
            else alert("上传失败");
        }, false);
        return false;
    }

    function FileUpload(successFun) {
        $.ajaxFileUpload
		(
			{
				url: '/FileManager/SaveUploadedFile?no=' + new Date().getTime() + '&type=' + 1,
				secureuri: false,
				dataType: 'json',
				type: 'post',
				data: { enctype: "multipart/form-data" },
				fileElementId: 'hidFile',
				success: function (data) {
					if (data.code === 0) {
						var path = "/ImageManager/AddImage";
					    var parentNodeId = 0;
						var sendObj = {
							parentId: parentNodeId,
							imageName: data.OriginalName,
							imagePath: data.FileKey
						}

						Common.Ajax.Post(path, sendObj, function (obj) {
							if (obj.code === 0) {
							   LoadRightContent(parentNodeId);
								successFun();
							}
							else alert("上传失败");
						}, false);

					} else {
						alert(data.ResultMsg);
					}
				},
				error: function (e) {
					alert(e.ResultMsg);
				}
			}
		);
    }

    function SoketFileUpload(successFun) {
        var path = "/FileManager/SoketFileUpload?no=" + new Date().getTime();
        var sendObj = {
            picUrl: $(".web-img input").val()
        }

        Common.Ajax.Post(path, sendObj, function (data) {
            if (data.code === 0) {

                var path = "/ImageManager/AddImage";
                var parentNodeId = 0;
                var sendObj = {
                    parentId: parentNodeId,
                    imageName: data.OriginalName,
                    imagePath: data.FilePath
                }

                Common.Ajax.Post(path, sendObj, function (obj) {
                    if (obj.code == 0) {
                        LoadRightContent(parentNodeId);
                        successFun();

                    }
                    else alert("上传失败");
                });

            } else {
                alert(data.ResultMsg);
            }
        });
    }

    function GetFileUrl(sourceId) {
        var url;
        if (navigator.userAgent.indexOf("MSIE") >= 1) { // IE 
            url = document.getElementById(sourceId).value;
        } else if ((sUserAgent.toLowerCase().indexOf("trident") > -1 && sUserAgent.indexOf("rv") > -1)) {
            url = document.getElementById(sourceId).value;
        } else if (navigator.userAgent.indexOf("Firefox") > 0) { // Firefox 
            url = window.URL.createObjectURL(document.getElementById(sourceId).files.item(0));
        } else if (navigator.userAgent.indexOf("Chrome") > 0) { // Chrome 
            url = window.URL.createObjectURL(document.getElementById(sourceId).files.item(0));
        }
        return url;
    }
})(window,jQuery)
