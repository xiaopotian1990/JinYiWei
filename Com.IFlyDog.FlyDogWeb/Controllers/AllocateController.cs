using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 仓库调拨控制器
    /// </summary>
    public class AllocateController : Controller
    {
        /// <summary>
        /// 仓库调拨页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: Allocate
        public ActionResult AllocateInfo()
        {
            return View();
        }



        /// <summary>
        /// 仓库调拨详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult AllocateIndex()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> AllocateGet(AllocateSelect dto)
        {
            dto.CreateUserId = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Allocate/Get",dto);
            return result;
        }
        #endregion

        /// <summary>
        ///     通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AllocateGetByID(AllocateSelect dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());

            var result = await WebAPIHelper.Get("/api/Allocate/GetByID", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AllocateAdd(AllocateAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Allocate/Add", dto);
            return result;
        }

        /// <summary>
        /// 打印页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartAllocatePrint(string allocateID) {
            return View();
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> SmartAllocatePrintFun(string allocateID)
        {

            var d = new Dictionary<string, string>();
            d.Add("allocateID", allocateID);
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/Allocate/SmartAllocatePrint", d);
            return result;
        }

    }
}