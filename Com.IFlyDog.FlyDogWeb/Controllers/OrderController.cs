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
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 预约界面获取已购买项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetAppointCharges(long customerID)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerID.ToString() } };
            var result = await WebAPIHelper.Get("/api/Order/GetAppointCharges", dic);
            return result;
        }

        //查询所有套餐
        public async Task<string> ChargeSetGet(string name , string pym)
        {
            var dic = new Dictionary<string, string> {{"name", name}, {"pym", pym}};
            var result = await WebAPIHelper.Get("/api/Order/GetChargeSet", dic);
            return result;

        }
 
        #region 添加订单 
        [HttpPost]
        public async Task<string> AddOrder(OrderAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Order/Add", dto);
            return result;
        }
        #endregion
        #region 修改订单 
        [HttpPost]
        public async Task<string> UpdateOrder(OrderAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Order/Update", dto);
            return result;
        }
        #endregion
        #region 删除订单 
        [HttpPost]
        public async Task<string> DeleteOrder(DepositOrderDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Order/Delete", dto);
            return result;
        }
        #endregion

        #region 查看详细订单 
        public async Task<string> GetOrderDetail(string customerId, string orderId)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerId }, { "orderID", orderId } };
            var result = await WebAPIHelper.Get("/api/Order/GetDetail", dic);
            return result;

        }

        #endregion
    }
}