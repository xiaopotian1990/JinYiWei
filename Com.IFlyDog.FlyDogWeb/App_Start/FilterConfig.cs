using Com.IFlyDog.FlyDogWeb.Filter;
using Com.JinYiWei.MVCFrameWork.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CommonExceptionAttribute());
            //filters.Add(new CommonAuthorizeAttribute());
        }
    }
}