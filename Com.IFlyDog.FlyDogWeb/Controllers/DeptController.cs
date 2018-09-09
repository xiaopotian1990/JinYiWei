using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Data;
using System;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class DeptController : Controller
    { 
        #region 页面
        public ActionResult DeptInfo()
        {
            return View();
        }
        #endregion

        #region API
            #region 查询全部部门
            public async Task<string> DeptGet()
            {
                var dic = new Dictionary<string, string>
                {
                    {"hospitalID", IDHelper.GetHospitalID().ToString()}
                };
                var result = await WebAPIHelper.Get("/api/Dept/Get", dic);
                return result;
            }
            #endregion

            #region 根据医院查询部门信息
            /// <summary>
            ///  根据医院查询当前的部门信息
            /// </summary>
            /// <returns></returns>
            public async Task<string> DeptByHospitalIdGet(string hospitalId)
            {
                var dic = new Dictionary<string, string>
                {
                    {"hospitalID", hospitalId == "-1" ? IDHelper.GetHospitalID().ToString() : hospitalId}
                };
                var result = await WebAPIHelper.Get("/api/Dept/Get", dic);

                return result;
            }
            #endregion

            #region  修改部门

        /// <summary>
        /// 修改之前根据ID查询当前详细
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [HttpPost]
            public async Task<string> DeptEditGet(string ID)
            {
                var dic = new Dictionary<string, string>
                {
                    {"id", ID},
                    {"UserHospitalID", IDHelper.GetHospitalID().ToString()}
                };
                var result = await WebAPIHelper.Get("/api/Dept/GetByID", dic);
                return result;
            }
            /// <summary>
            /// 修改部门-提交
            /// </summary>
            /// <param name="dto"></param>
            /// <returns></returns>
            [HttpPost]
            public async Task<string> DeptEditSubmit(SmartDeptUpdate dto)
            {
                dto.CreateUserID = IDHelper.GetUserID();
                dto.UserHospitalID = IDHelper.GetUserID();
                var result = await WebAPIHelper.Post("/api/Dept/Update", dto);
                return result;
            }
            #endregion

            #region 添加部门
            /// <summary>
            /// 添加部门
            /// </summary>
            /// <param name="dto"></param>
            /// <returns></returns>
            [HttpPost]
            public async Task<string> DeptAdd(SmartDeptAdd dto)
            {
                dto.CreateUserID = IDHelper.GetUserID();
                dto.UserHospitalID = IDHelper.GetHospitalID();
                var result = await WebAPIHelper.Post("/api/Dept/Add", dto);
                return result;
            }
            #endregion

            #region 删除部门
            /// <summary>
            /// 删除部门
            /// </summary>
            /// <param name="dto"></param>
            /// <returns></returns>
            [HttpPost]
            public async Task<string> DeptDelete(DeptDelete dto)
            {
                dto.CreateUserID = IDHelper.GetUserID();
                var result = await WebAPIHelper.Post("/api/Dept/Delete", dto);
                return result;
            }
            #endregion

            #region 根据医院ID查询部门
            public async Task<string> DeptGetByHospitalId(string hospitalId)
            {
                var dic = new Dictionary<string, string> { { "hospitalId", hospitalId } };
                var result = await WebAPIHelper.Get("/api/Dept/Get", dic);
                return result;
            }
            #endregion

        #endregion 
    }
}