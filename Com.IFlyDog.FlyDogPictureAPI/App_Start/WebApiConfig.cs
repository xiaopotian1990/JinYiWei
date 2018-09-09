using Com.JinYiWei.WebAPI.Filters;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Com.IFlyDog.FlyDogPictureAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
            // Web API 配置和服务
            config.Filters.Add(new CommonExceptionFilterAttribute());
            //config.Filters.Add(new FlyDogSignFilterAttribute());
            //config.Filters.Add(new FlyDogTokenFilterAttribute());
            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}