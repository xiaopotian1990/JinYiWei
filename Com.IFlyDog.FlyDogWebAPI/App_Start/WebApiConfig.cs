using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWebAPI.Filters;
using Com.JinYiWei.WebAPI.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI
{
    /// <summary>
    /// web api配置
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new CommonExceptionFilterAttribute());


            if (Key.CS == 0)
            {
                config.Filters.Add(new FlyDogSignFilterAttribute());
                config.Filters.Add(new FlyDogTokenFilterAttribute());
            }

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings();
        }
    }
}
