using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 短信工作台
    /// </summary>
    public class SMSDeskController : Controller
    {
        // GET: SMSDesk
        public ActionResult Index()
        {
            PagerInfo p = new PagerInfo();
            p.PageIndex = 1;
            p.PageSize = 1;
            p.TotalCount = 10;

            ViewBag.PagerInfo = p;

            return View(ViewBag);
        }
    }
}