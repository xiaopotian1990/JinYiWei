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
    public class FailtureController : Controller
    {
        // GET: Failture
        public ActionResult Index()
        {
            return View();
        }

        #region   通过顾客ID查询未成交情况

        public async Task<string> GetFailtureByCustomerId(string customerId)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerId } };
            var result = await WebAPIHelper.Get("/api/Failture/GetByCustomerID", dic);
            return result;
        }

        #endregion

        #region   添加未成交情况

        public async Task<string> AddFailture(FailtureAddUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Failture/Add", dto);
            return result;
        }

        #endregion

        #region  修改之前回填

        public async Task<string> GetDetail(string faid)
        {
            var dic = new Dictionary<string, string> { { "ID", faid } };
            var result = await WebAPIHelper.Get("/api/Failture/GetDetail", dic);
            return result;
        }

        #endregion
        
        #region   修改未成交情况

        public async Task<string> UpdateFailture(FailtureAddUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
           var result = await WebAPIHelper.Post("/api/Failture/Update", dto);
            return result;
        }

        #endregion

        #region   删除未成交情况

        public async Task<string> DeleteFailture(FailtureDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Failture/Delete", dto);
            return result;
        }

        #endregion

    }
}