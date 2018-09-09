using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class OnlineDeskController : Controller
    {
        /// <summary>
        /// 网电工作台
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        { 
            return View();
        }
    }
}