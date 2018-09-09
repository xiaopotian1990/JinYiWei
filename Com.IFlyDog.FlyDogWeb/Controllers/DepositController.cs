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
    public class DepositController : Controller
    {
        // GET: Deposit
        public ActionResult Index()
        {
            return View();
        }

        #region 添加预收款
     
        [HttpPost]
        public async Task<string> DepositAdd(DepositOrderAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Deposit/AddOrder", dto);
            return result;
        }
        #endregion

        #region 预收款删除

        [HttpPost]
        public async Task<string> DepositDelete(DepositOrderDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Deposit/Delete", dto);
            return result;
        }
        #endregion

        #region 查询详细

        [HttpPost]
        public async Task<string> GetDepositDetail(string orderId)
        {
            var dic = new Dictionary<string, string> {{"orderID", orderId}}; 
            var result = await WebAPIHelper.Get("/api/Deposit/GetDetail", dic);
            return result;
      
        }
        #endregion
         
        #region 查询剩余预收款

        [HttpGet]
        public async Task<string> GetNoDoneOrders(string hospitalId,string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"hospitalID", IDHelper.GetHospitalID().ToString()},
                {"customerId", customerId}
            };
            var result = await WebAPIHelper.Get("/api/Deposit/GetNoDoneOrders", dic);
            return result;
        }
        #endregion

        #region 添加预收款界面获取可购买的预收款 
      
        public async Task<string> GetAllDeposit()
        {
            var dic = new Dictionary<string, string>
            {
                {"hospitalID", IDHelper.GetHospitalID().ToString()} 
            };
            var result = await WebAPIHelper.Get("/api/Deposit/GetAllDeposit", dic);
            return result;
        }
        #endregion
    }
}