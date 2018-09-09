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
    /// 单位管理控制器
    /// </summary>
    public class SmartUnitController : Controller
    {
        #region 单位信息
       /// <summary>
       /// 单位管理
       /// </summary>
       /// <returns></returns>
        public ActionResult SmartUnitInfo()
        {
            return View();
        }
        #endregion

        #region 查询所有单位信息
        /// <summary>
        /// 查询所有单位信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartUnitGet()
        {
            var result = await WebAPIHelper.Get("/api/SmartUnit/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据单位信息id查询单位详情
        /// <summary>
        ///根据单位信息id查询单位详情
        /// </summary>
        /// <param name="smartUnitInfo">根据单位信息id查询单位详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartUnitEditGet(SmartUnitInfo smartUnitInfo)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", smartUnitInfo.ID.ToString());
            var result = await WebAPIHelper.Get("/api/SmartUnit/GetByID", d);
            return result;
        }
        #endregion

        #region 新增单位信息
        /// <summary>
        /// 添加单位信息
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartUnitAdd(SmartUnitAdd smartUnitAdd)
        {
            smartUnitAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartUnit/Add", smartUnitAdd);
            return result;
        }
        #endregion

        #region 更新单位信息
        /// <summary>
        /// 更新单位信息
        /// </summary>
        /// <param name="smartUnitUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartUnitSubmit(SmartUnitUpdate smartUnitUpdate)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", smartUnitUpdate.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("Name", smartUnitUpdate.Name);
            var result = await WebAPIHelper.Post("/api/SmartUnit/Update", dic);
            return result;
        }
        #endregion

        #region 删除单位信息
        /// <summary>
        /// 删除单位信息
        /// </summary>
        /// <param name="smartUnitDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartUnitDelete(SmartUnitDelete smartUnitDelete)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", smartUnitDelete.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            var result = await WebAPIHelper.Post("/api/SmartUnit/Delete", dic);
            return result;
        }
        #endregion
    }
}