using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.JinYiWei.Common.Extensions;
using System.Linq;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Filter
{
    public class CommonAuthorizeAttribute : ActionFilterAttribute
    {
        private Session session = new Session();
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string url = filterContext.RouteData.Values["controller"] as string + "/" + filterContext.RouteData.Values["action"] as string;
            //var user = session["User"];
            var user = IDHelper.Get<LoginUserInfo>("User");
       
            if (url == "Login/Login" && user != null)
            {
                filterContext.Result = new RedirectResult("/Home/Index");
            }
            else if ((url == "Home/Index" || url == "Home/Main" || url == "Home/GetMenu") && user != null)
            {
                base.OnActionExecuting(filterContext);
            }
            else if (url == "Login/Login" && user == null)
            {
                base.OnActionExecuting(filterContext);
            }
            else if (url == "Login/LoginOut")
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                if (user == null)
                {
                    filterContext.Result = new RedirectResult("/Login/Login");
                    return;
                }
                if (!user.Actions.Any(u => u.Contains(url)))
                {
                    //filterContext.HttpContext.Response.Redirect("/Login/Competence");
                    var result = new IFlyDogResult<IFlyDogResultType, int>()
                    {
                        Message = "权限不足",
                        Data = 0,
                        ResultType = IFlyDogResultType.NoAuth
                    };
                    filterContext.Result = new JsonResult() { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    session.Postpone("User");
                    session.Postpone("UserID");
                    session.Postpone("HospitalID");
                    base.OnActionExecuting(filterContext);
                }
            }
        }
    }
}