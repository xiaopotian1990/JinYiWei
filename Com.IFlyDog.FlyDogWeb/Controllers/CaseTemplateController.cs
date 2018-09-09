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
    public class CaseTemplateController : Controller
    {
        /// <summary>
        /// 病例模板页面
        /// </summary>
        /// <returns></returns>
        // GET: CaseTemplate
        public ActionResult CaseTemplateInfo()
        {
            return View();
        }


        #region 查询所有病例管理信息
        /// <summary>
        /// 查询所有病例模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CaseTemplateGet(CaseTemplateSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/CaseTemplate/Get", dto);
            return result;
        }
        #endregion

        #region 根据id查询病例模板详情
        /// <summary>
        ///根据id查询病例模板详情
        /// </summary>
        /// <param name="smartUnitInfo">根据id查询病例模板详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CaseTemplateEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/CaseTemplate/GetByID", d);
            return result;
        }
        #endregion

        #region 新增病例模板详情
        /// <summary>
        /// 新增病例模板详情
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CaseTemplateAdd(CaseTemplateAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CaseTemplate/Add", dto);
            return result;
        }
        #endregion

        #region 更新病例模板详情
        /// <summary>
        /// 更新病例模板详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CaseTemplateSubmit(CaseTemplateUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CaseTemplate/Update", dto);
            return result;
        }
        #endregion
    }
}
