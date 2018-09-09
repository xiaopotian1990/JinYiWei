using Com.IFlyDog.APIDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class RoleController : Controller
    {
        // GET: 角色管理
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> GetAllRole(string hospitalId)
        {
            var d = new Dictionary<string, string>();
            d.Add("userHositalID", IDHelper.GetHospitalID().ToString());
            d.Add("hositalID", hospitalId);
            var result = await WebAPIHelper.Get("/api/Role/GetAllRole", d);
            return result;
        }

        //查询全部角色-
        [HttpPost]
        public async Task<string> GetRoleMenu()
        {
            var result = await WebAPIHelper.Get("/api/Role/GetRoleMenu", new Dictionary<string, string>());
            return result;
        }

        //添加角色-
        [HttpPost]
        public async Task<string> RoleAdd(RoleAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            
            var result = await WebAPIHelper.Post("/api/Role/Add", dto);
            return result;
        }

        //删除角色-
        [HttpPost]
        public async Task<string> RoleDelete(RoleDelete dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("RoleID", dto.RoleID.ToString());
            dic.Add("UserHospitalID", IDHelper.GetHospitalID().ToString());
            dic.Add("HospitalID", dto.HospitalID.ToString());
            var result = await WebAPIHelper.Post("/api/Role/Delete", dic);
            return result;
        }

    
        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetRoleDetail(RoleDetail dto)
        {

            var dic = new Dictionary<string, string>();

            dic.Add("roleID", dto.ID.ToString());
            dic.Add("userHositalID", IDHelper.GetHospitalID().ToString());
            dic.Add("hositalID", dto.HospitalID.ToString());
            var result = await WebAPIHelper.Get("/api/Role/GetRoleDetail", dic);
            return result;

        }
        //修改角色-
        [HttpPost]
        public async Task<string> RoleUpdate(RoleUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/Role/Update", dto);
            return result;
        }

    }
}