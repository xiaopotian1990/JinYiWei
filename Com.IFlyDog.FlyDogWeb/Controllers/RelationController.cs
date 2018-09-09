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
    public class RelationController : Controller
    {
        /// <summary>
        /// 关系管理页面
        /// </summary>
        /// <returns></returns>
        // GET: Relation
        public ActionResult RelationInfo()
        {
            return View();
        }

        #region 查询所有关系信息
        /// <summary>
        /// 查询所有单位信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> RelationGet()
        {
            var result = await WebAPIHelper.Get("/api/Relation/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id查询关系详情
        /// <summary>
        ///根据id查询关系详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> RelationEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Relation/GetByID", d);
            return result;
        }
        #endregion

        #region 新增关系
        /// <summary>
        /// 新增关系
        /// </summary>
        /// <param name="relationAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> RelationAdd(RelationAdd relationAdd)
        {
            relationAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Relation/Add", relationAdd);
            return result;
        }
        #endregion

        #region 修改关系
        /// <summary>
        /// 修改关系
        /// </summary>
        /// <param name="relationUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> RelationSubmit(RelationUpdate relationUpdate)
        {
            relationUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Relation/Update", relationUpdate);
            return result;
        }
        #endregion

        #region 删除关系
        /// <summary>
        /// 删除关系
        /// </summary>
        /// <param name="relationDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> RelationDelete(RelationDelete relationDelete)
        {
            relationDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Relation/Delete", relationDelete);
            return result;
        }
        #endregion
    }
}