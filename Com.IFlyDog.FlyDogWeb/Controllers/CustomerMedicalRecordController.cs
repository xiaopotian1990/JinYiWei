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
    public class CustomerMedicalRecordController : Controller
    {
        /// <summary>
        /// 添加客户病例模板页面
        /// </summary>
        /// <returns></returns>
        // GET: CustomerMedicalRecord
        public ActionResult CustomerMedicalRecordInfo()
        {
            return View();
        }

        /// <summary>
        /// 客户病例列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerMedicalRecordTable() {
            return View();
        }

        #region 新增客户病例模板
        /// <summary>
        /// 新增客户病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerMedicalRecordAdd(CustomerMedicalRecordAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerMedicalRecord/Add", dto);
            return result;
        }
        #endregion

        #region 根据id查询详情
        /// <summary>
        ///根据id查询详情
        /// </summary>
        /// <param name="id">根据id查询详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerMedicalRecordEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/CustomerMedicalRecord/GetByID", d);
            return result;
        }
        #endregion


        #region 列表查询客户病例信息
        /// <summary>
        /// 列表查询客户病例信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> CustomerMedicalRecordGet(CustomerMedicalRecordSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerMedicalRecord/Get", dto);
            return result;
        }
        #endregion

        #region 根据id查询病例模板详情，客户病例模板列表使用
        /// <summary>
        ///根据id查询病例模板详情，客户病例模板列表使用
        /// </summary>
        /// <param name="id">根据id查询病例模板详情，客户病例模板列表使用</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByPKIDGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/CustomerMedicalRecord/GetByPKID", d);
            return result;
        }
        #endregion


        #region 更新客户病例模板详情
        /// <summary>
        /// 更新客户病例模板详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerMedicalRecordSubmit(CustomerMedicalRecordUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerMedicalRecord/Update", dto);
            return result;
        }
        #endregion

        #region 删除顾客病例模板
        /// <summary>
        /// 删除顾客病例模板
        /// </summary>
        /// <param name="smartUnitDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerMedicalRecordDelete(CustomerMedicalRecordDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerMedicalRecord/Delete", dto);
            return result;
        }
        #endregion
    }
}