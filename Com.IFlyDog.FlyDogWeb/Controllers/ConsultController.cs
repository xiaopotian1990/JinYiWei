using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class ConsultController : Controller
    {
        // GET: 咨询
        public ActionResult Index()
        {
            return View();
        }

        #region   通过顾客ID查询咨询情况

        public async Task<string> GetConsult(string customerId)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerId } };
            var result = await WebAPIHelper.Get("/api/Consult/GetConsult", dic);
            return result;
        }

        #endregion
        
        #region 添加咨询记录

        [HttpPost]
        public async Task<string> ConsultAdd(ConsultAddUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Consult/ConsultAdd", dto);
            return result;
        }
        #endregion

        #region 删除咨询记录

        [HttpPost]
        public async Task<string> ConsultDelete(ConsultDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Consult/ConsultDelete", dto);
            return result;
        }
        #endregion

        #region   修改数据回填

        public async Task<string> GetConsultDetail(string consultid)
        {
          
            var dic = new Dictionary<string, string> { { "ID", consultid } };
            var result = await WebAPIHelper.Get("/api/Consult/GetConsultDetail", dic);
            return result;
        }

        #endregion

        #region   修改提交数据

        public async Task<string> ConsultUpdate(ConsultAddUpdate dtoUp)
        {
            dtoUp.CreateUserID = IDHelper.GetUserID();       
            var result = await WebAPIHelper.Post("/api/Consult/ConsultUpdate", dtoUp);
            return result;
        }

        #endregion

    }
}