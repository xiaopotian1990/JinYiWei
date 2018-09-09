using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.JinYiWei.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 主页
    /// </summary>
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Main()
        {
            ViewData["UserID"] = IDHelper.GetUserID();
            return View();
        }

        public string GetMenu()
        {
            var user = IDHelper.Get<LoginUserInfo>("User");

            return user.Menus.ToJsonString();
        }
    }
}