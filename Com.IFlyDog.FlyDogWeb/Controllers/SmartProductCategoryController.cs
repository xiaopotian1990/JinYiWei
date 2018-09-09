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
    public class SmartProductCategoryController : Controller
    {

        #region 药物品分类
        /// <summary>
        /// 药物品分类
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartProductCategoryInfo()
        {
            return View();
        }
        #endregion

        /// <summary>
        /// 药物品分类信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartProductCategoryType() {
            return View();
        }

        #region 查询所有药物品信息
        /// <summary>
        /// 查询所有药物品信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartProductCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/SmartProductCategory/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据药物品信息id查询药物品详情
        /// <summary>
        ///根据药物品信息id查询药物品详情
        /// </summary>
        /// <param name="smartUnitInfo">根据单位信息id查询单位详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartProductCategoryEditGet(SmartProductCategoryInfo smartProductCategoryInfo)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", smartProductCategoryInfo.ID.ToString());
            var result = await WebAPIHelper.Get("/api/SmartProductCategory/GetByID", d);
            return result;
        }
        #endregion

        #region 新增药物品信息
        /// <summary>
        /// 新增药物品信息
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartProductCategoryAdd(SmartProductCategoryAdd smartProductCategoryAdd)
        {
            smartProductCategoryAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartProductCategory/Add", smartProductCategoryAdd);
            return result;
        }
        #endregion

        #region 更新药物品信息
        /// <summary>
        /// 更新药物品信息
        /// </summary>
        /// <param name="smartUnitUpdate"></param>
        /// <returns></returns>
        [HttpPost]
                               
        public async Task<string> SmartProductCategorySubmit(SmartProductCategoryUpdate smartProductCategoryUpdate)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", smartProductCategoryUpdate.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("Name", smartProductCategoryUpdate.Name);
            dic.Add("SortNo", smartProductCategoryUpdate.SortNo.ToString());
            dic.Add("Remark", smartProductCategoryUpdate.Remark);
            dic.Add("PID", smartProductCategoryUpdate.PID);
            var result = await WebAPIHelper.Post("/api/SmartProductCategory/Update", dic);
            return result;
        }
        #endregion

        #region 删除药物品信息
        /// <summary>
        /// 删除药物品信息
        /// </summary>
        /// <param name="smartUnitDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartProductCategoryDelete(SmartProductCategoryDelete smartProductCategoryDelete)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", smartProductCategoryDelete.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            var result = await WebAPIHelper.Post("/api/SmartProductCategory/Delete", dic);
            return result;
        }
        #endregion
    }
}