using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 用户中心
    /// </summary>
    public class UserCenterController : Controller
    {
        // 知识库
        public ActionResult KnowledgeCenter()
        {
            return View();
        }
    }
}