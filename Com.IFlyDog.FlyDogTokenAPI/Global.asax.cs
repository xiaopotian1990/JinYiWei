using Com.JinYiWei.WebAPI.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Com.IFlyDog.FlyDogTokenAPI
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// /
        /// </summary>
        protected void Application_Start()
        {
            UnityConfig.RegisterComponents();
            LoggingInitialize.Init();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
