using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 回访工作台
    /// </summary>
    public class CallbackDeskController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region   查询所有数据  
        [HttpPost]
        public async Task<string> GetCallbackDeskPages(CallbackSelect dto)
         {
            
             dto.LoginUserID = IDHelper.GetUserID();
             var result = await WebAPIHelper.Post("/api/Callback/CallbackSelect", dto);
            return result;
        }
        #endregion

        #region   通过当前回访ID查询详细
       
        public async Task<string> GetDetailByCallbackId(string callbackId)
        {
            var dic = new Dictionary<string, string> {{ "ID", callbackId}};
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackDetail", dic);
            return result;
        }

        #endregion

        #region   添加回访信息以增加增加下次回访
        [HttpPost]
        public async Task<string> CallbackAddByDesk(CallbackAddByDesk dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/CallbackAddByDesk", dto);
            return result;
        }

        #endregion

        #region   提交修改回访信息
        [HttpPost]
        public async Task<string> UpdateCallback(CallbackUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/UpdateCallback", dto);
            return result;
        }

        #endregion

        #region  查询回访情况

        public async Task<string> GetCallbackByCustomerId(string customerId)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerId } };
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackByCustomerID", dic);
            return result;
        }

        #endregion

    }
}