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
    public class PositionController : Controller
    {
        /// <summary>
        /// 岗位分工页面
        /// </summary>
        /// <returns></returns>
        // GET: Position
        public ActionResult PositionInfo()
        {
            return View();
        }

        #region 查询所有岗位分工信息
        /// <summary>
        /// 查询所有单位信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> PositionGet()
        {
            var result = await WebAPIHelper.Get("/api/Position/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id获取岗位分工详情
        /// <summary>
        ///根据id获取岗位分工详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PositionEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Position/GetByID", d);
            return result;
        }
        #endregion

        #region 新增岗位分工
        /// <summary>
        /// 新增岗位分工
        /// </summary>
        /// <param name="positionAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PositionAdd(PositionAdd positionAdd)
        {
            positionAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Position/Add", positionAdd);
            return result;
        }
        #endregion

        #region 更新岗位分工
        /// <summary>
        /// 更新岗位分工
        /// </summary>
        /// <param name="positionUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PositionSubmit(PositionUpdate positionUpdate)
        {
            positionUpdate.CreateUserID= IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Position/Update", positionUpdate);
            return result;
        }
        #endregion
    }
}