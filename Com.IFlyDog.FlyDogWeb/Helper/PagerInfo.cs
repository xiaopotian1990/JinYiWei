using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    public class PagerInfo
    {
        public int TotalCount { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// 回调函数
        /// </summary>
        public string PageCallback { get; set; }
    }
}