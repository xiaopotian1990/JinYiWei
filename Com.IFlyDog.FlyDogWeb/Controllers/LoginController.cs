using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.JinYiWei.Common.Extensions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginController : Controller
    {
        private Session session = new Session();
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="login">用户名密码</param>
        /// <returns></returns>
        public async Task<ActionResult> Login(Login login = null)
        {
            if (login.Account == null)
            {
                return View();
            }
            var result = await WebAPIHelper.Post<IFlyDogResult<IFlyDogResultType, LoginUserInfo>, Login>("/api/Login/Login", login);

            if (result.ResultType != IFlyDogResultType.Success)
            {
                ViewData["Info"] = "用户不存在";
                return View();
            }

            var u = result.Data;
            session["User"] = u;
            session["UserID"] = u.ID;
            session["HospitalID"] = u.HospitalID;

            return Redirect("/Home/Index");
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut()
        {
            session["User"] = null;
            session["UserID"] = null;
            session["HospitalID"] = null;
            return Redirect("/Login/Login");
        }
    }
}