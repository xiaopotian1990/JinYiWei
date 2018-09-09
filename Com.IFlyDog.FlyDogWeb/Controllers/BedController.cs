using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class BedController : Controller
    {
        // 床位管理
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取全部床位管理
        /// </summary>
        public async Task<string> BedIndexGet()
        {
            var dic = new Dictionary<string, string> { { "hospitalID", IDHelper.GetHospitalID().ToString() } };
            var result = await WebAPIHelper.Get("/api/Bed/Get", dic);
            return result;
        }

        /// <summary>
        /// 获取本医院床位列表
        /// </summary>
        public async Task<string> Get()
        {
            var dic = new Dictionary<string, string> { { "hospitalID", IDHelper.GetHospitalID().ToString() }, { "status", CommonStatus.Use.ToString() } };
            var result = await WebAPIHelper.Get("/api/Bed/Get", dic);
            return result;
        }

        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> BedGetById(string id)
        {
            var dic = new Dictionary<string, string> { { "ID", id } };
            var result = await WebAPIHelper.Get("/api/Bed/GetByID", dic);
            return result;
        }

        /// <summary>
        /// 床位下拉菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetSelect()
        {
            var dic = new Dictionary<string, string> { { "hospitalID", IDHelper.GetHospitalID().ToString() } };
            var result = await WebAPIHelper.Get("/api/Bed/GetSelect", dic);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BedUpdate(BedAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Bed/Update", dto);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BedAdd(BedAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Bed/Add", dto);
            return result;
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BedStopOrUse(BedStop dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Bed/StopOrUse", dto);
            return result;
        }
    }
}