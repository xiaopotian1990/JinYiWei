using Com.JinYiWei.WebAPI.App_Start;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogPictureAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            LoggingInitialize.Init();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
