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
    public class ChargeCategoryController : Controller
    {

        #region 项目分类信息
        /// <summary>
        /// 项目分类信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeCategoryInfo()
        {
            return View();
        }
        #endregion

        #region 查询所有项目分类信息
        /// <summary>
        /// 查询所有药物品信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> ChargeCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/ChargeCategory/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据项目分类信息id查询详情
        /// <summary>
        /// 根据项目分类信息id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeCategoryEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/ChargeCategory/GetByID", d);
            return result;
        }
        #endregion

        #region 新增项目分类信息
        /// <summary>
        /// 新增项目分类信息
        /// </summary>
        /// <param name="">chargeCategoryAdd</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeCategoryAdd(ChargeCategoryAdd chargeCategoryAdd)
        {
            chargeCategoryAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeCategory/Add", chargeCategoryAdd);
            return result;
        }
        #endregion

        #region 更新项目分类信息
        /// <summary>
        /// 更新项目分类信息
        /// </summary>
        /// <param name="chargeCategoryUpdate"></param>
        /// <returns></returns>
        [HttpPost]

        public async Task<string> ChargeCategorySubmit(ChargeCategoryUpdate chargeCategoryUpdate)
        {
            chargeCategoryUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeCategory/Update", chargeCategoryUpdate);
            return result;
        }
        #endregion

        #region 删除项目分类信息
        /// <summary>
        /// 删除项目分类信息
        /// </summary>
        /// <param name="chargeCategoryDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartProductCategoryDelete(ChargeCategoryDelete chargeCategoryDelete)
        {
            chargeCategoryDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeCategory/Delete", chargeCategoryDelete);
            return result;
        }
        #endregion
    }
}