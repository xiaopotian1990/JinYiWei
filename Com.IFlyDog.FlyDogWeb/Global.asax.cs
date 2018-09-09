using Com.JinYiWei.MVCFrameWork.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Com.IFlyDog.FlyDogWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            UnityWebActivator.Start();
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            LoggingInitialize.Init();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }
    }
}
