using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    /// <summary>
    /// 分页控件
    /// </summary>
    public class PagerHelper
    {
        public static string GetNormalPage(PagerInfo pager)
        {
            int currentPageIndex = pager.PageIndex;
            int pageSize = pager.PageSize;
            int recordCount = pager.TotalCount;
            string pageCallback = pager.PageCallback;
            int pageCount = (recordCount % pageSize == 0 ? recordCount / pageSize : recordCount / pageSize + 1);
            //StringBuilder url = new StringBuilder();
            //url.Append(HttpContext.Current.Request.Url.AbsolutePath + "?Page={0}");
            //NameValueCollection collection = HttpContext.Current.Request.QueryString;
            //string[] keys = collection.AllKeys;
            //for (int i = 0; i < keys.Length; i++)
            //{
            //    //if (keys[i].ToLower() != "page")
            //        //url.AppendFormat("&{0}={1}", keys[i], collection[keys[i]]);no bug
            //}
            StringBuilder sb = new StringBuilder();

 
           
            sb.AppendFormat("<div class=\"col-sm-6\"><div class=\"dataTables_info\" id=\"sample -table-2_info\">当前第{2}页，共{1}页，总共{0}条记录</div></div>", recordCount, pageCount, currentPageIndex);
            //sb.Append("</div>");

            sb.Append("<div class=\"col-sm-6\"><div class=\"dataTables_paginate paging_bootstrap\"> <ul class=\"pagination\">");
            if (currentPageIndex == 1)
                sb.Append("<li class=\"prev disabled\"> <a href=\"javascript:void(0)\"><i class=\"icon-double-angle-left\"></i>首页</a></li>");
            else
            {
                sb.AppendFormat("<li><a href='#' onclick = '{0}(1)'><i class=\"icon-double-angle-left\"></i>首页</a></li>", pageCallback);
            }
            if (currentPageIndex > 1)
            {
                sb.AppendFormat("<li><a href='#' onclick = '{0}({1})'>上一页</a></li>&nbsp", pageCallback, currentPageIndex - 1);
            }
            else
            {
                sb.Append("<li  class=\"active\"><a href=\"javascript:void(0)\">上一页</a></li>");
            }
            if (currentPageIndex < pageCount)
            { 
                sb.AppendFormat("<li><a href='#' onclick = '{0}({1})'>下一页</a></li>&nbsp", pageCallback, currentPageIndex + 1); 
            }
            //else
                //sb.Append("<li  class=\"active\"><a href=\"javascript:void(0)\">下一页</a></li>");

            //if (currentPageIndex == pageCount)
                //sb.Append("<li class=\"active\"><a href=\"javascript:void(0)\"><i class=\"icon-double-angle-right\">末页</a></li>");
            else
            {
                sb.AppendFormat("<li><a href='#' onclick = '{0}({1})'>末页</a></li>", pageCallback, pageCount);
            }
            sb.Append("</ul></div></div>");
            return sb.ToString();
        }

    }
}