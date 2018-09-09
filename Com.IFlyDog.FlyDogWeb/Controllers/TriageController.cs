using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 分诊记录
    /// </summary>
    public class TriageController : Controller
    {
        // GET: Triage
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