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
    /// <summary>
    /// 审核工作台控制器
    /// </summary>
    public class AuditWorkbenchController : Controller
    {

        /// <summary>
        /// 展示所有待审核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditWorkbenchInfo()
        {
            return View();
        }

        /// <summary>
        ///  审核操作界面
        /// </summary>
        /// <param name="auditDataID">要审核的数据id</param>
        /// <param name="customerID">客户id</param>
        /// <returns></returns>
        public ActionResult AuditOperationInfo(string auditDataID,string customerID)
        {
            return View();
        }

        /// <summary>
        /// 展示订单详情
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditOrderDetail() {
            return View();
        }

        #region 查询所有待审核信息
        /// <summary>
        /// 查询所有待审核信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> GetAllAuditGet(AuditWorkbenchSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/AuditWorkbench/GetAllAudit",
            dto);
            return result;
        }
        #endregion

        #region 查询所有审核记录信息
        /// <summary>
        /// 查询所有审核记录信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> AuditRecordGet(AuditRecordSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/AuditWorkbench/GetAuditRecord",
            dto);
            return result;
        }
        #endregion

        #region 点击查询查询此类型的审核用户信息
        /// <summary>
        /// 点击查询查询此类型的审核用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> ByTypeGet(AuditUserSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.UserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/AuditWorkbench/GetByType",
            dto);
            return result;
        }
        #endregion

        /// <summary>
        ///     审核操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditOperationAdd(AuditOperationAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.AuditTime = DateTime.Now;
            var result = await WebAPIHelper.Post("/api/AuditWorkbench/AuditOperationAdd", dto);
            return result;
        }


        #region 查看退项目单详细 
        public async Task<string> GetBackOrderDetail(string customerId, string orderId)
        {   
            var dic = new Dictionary<string, string> { { "userID",IDHelper.GetUserID().ToString() },{ "customerID", customerId }, { "orderID", orderId } };
            var result = await WebAPIHelper.Get("/api/BackOrder/GetDetail", dic);
            return result;

        }

        #endregion

        #region 查看退退预收款单详细 
        public async Task<string> GetDepositRebateOrderDetail(string customerId, string orderId)
        {
            var dic = new Dictionary<string, string> { { "userID", IDHelper.GetUserID().ToString() }, { "customerID", customerId }, { "orderID", orderId } };
            var result = await WebAPIHelper.Get("/api/DepositRebateOrder/GetDetail", dic);
            return result;

        }

        #endregion


        #region 查询开发人员或者咨询人员订单详细 
        public async Task<string> GetAuditOrderInfo(string id, string type)
        {
            var dic = new Dictionary<string, string> { { "id", id }, { "type", type } };
            var result = await WebAPIHelper.Get("/api/AuditWorkbench/GetAuditOrderInfo", dic);
            return result;

        }

        #endregion
    }
}