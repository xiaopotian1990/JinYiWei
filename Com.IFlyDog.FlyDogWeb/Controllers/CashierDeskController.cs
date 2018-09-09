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
    public class CashierDeskController : Controller
    {
        // GET:收银台
        #region 页面
        //收银台-主页
        public ActionResult Index()
        {
            return View();
        }
        //今日收银打印页面
        public ActionResult CashierTodayPrint(string cashierTodayId)
        {
            return View();
        }


        #region 待收费列表
        //待收费列表-退项目单-页面
        public ActionResult BackOrder()
            {
                return View();
            }
            //待收费列表-订单-页面
            public ActionResult CashierOrder()
            {
                return View();
            }
            //待收费列表-退预收款单-页面
            public ActionResult DepositRebate()
            {
                return View();
            }

            //待收费列表-添加预收款-页面
            public ActionResult AddDeposit()
            {
                return View();
            }

        #endregion

        //结算记录
        public ActionResult SettlementRecord()
        {
            return View();
        }
        //收银记录
        public ActionResult CashierRecord()
        {
            return View();
        }
        //欠款记录
        public ActionResult RepayDebt()
        {
            return View();
        }
        //欠款记录-还款弹窗
        public ActionResult RefundMoney()
        {
            return View();
        }
        //结算界面
        public ActionResult SettleAccounts()
        {
            return View();
        }

        //顾客识别出信息后-开发票页面
        public ActionResult AddInvoice()
        {
            return View();
        }

        #endregion

        #region API接口
        //待收费列表
        public async Task<string> GetNoPaidOrders()
        {
            var dic = new Dictionary<string, string> {{"hospitalID", IDHelper.GetHospitalID().ToString()}};

            var result = await WebAPIHelper.Get("/api/Cashier/GetNoPaidOrders", dic);
            return result;
        }

        //预收款收银
        [HttpPost]
        public async Task<string> DepositOrderCashier(DepositCashierAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/DepositOrderCashier", dto);
            return result;
        }

        //获取订单收银时可使用的券跟预收款
        public async Task<string> GetCanCashier(string customerId,string orderId)
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID", IDHelper.GetHospitalID().ToString() }, 
                { "customerId", customerId }, 
                { "orderId", orderId}

            };

            var result = await WebAPIHelper.Get("/api/Cashier/GetCanCashier", dic);
            return result;
        }

        //订单收银
        [HttpPost]
        public async Task<string> OrderCashier(OrderCashierAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/OrderCashier", dto);
            return result;
        }
        //退款收银
        [HttpPost]
        public async Task<string> DepositRebateOrderCashier(DepositRebateCashierAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/DepositRebateOrderCashier", dto);
            return result;
        }
        //退项目单
        [HttpPost]
        public async Task<string> BackOrderCashier(BackCashierAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/BackOrderCashier", dto);
            return result;
        }

        //欠款收银 
        [HttpPost]
        public async Task<string> DebtCashier(DebtCashierAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/DebtCashier", dto);
            return result;
        }

        //查询欠款记录
        public async Task<string> GetDebtOrdes(DebtSelect dto)
        { 
            var result = await WebAPIHelper.Post("/api/Order/GetDebtOrdes", dto);
            return result;
        }

        //获取更新收银详细信息
        public async Task<string> GetCashierUpdateInfo(string cashierId)
        {
            var dic = new Dictionary<string, string>
            { 
                { "cashierId", cashierId }  
            };

            var result = await WebAPIHelper.Get("/api/Cashier/GetCashierUpdateInfo", dic);
            return result;
        }

        //订单修改
        [HttpPost]
        public async Task<string> CashierUpdate(CashierUpdate dto)
        { 
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/CashierUpdate", dto);
            return result;
        }

        //今日收银记录
        public async Task<string> GetCashierToday()
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID",IDHelper.GetHospitalID().ToString()}
            };

            var result = await WebAPIHelper.Get("/api/Cashier/GetCashierToday", dic);
            return result;
        }
        //查询全部-收银记录
        public async Task<string> GetCashier(CashierSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Cashier/GetCashier", dto);
            return result;
        }
        //查询订单 详细 -欠款
        public async Task<string> GetOrderDetail(string customerId,string orderId)
        {
            var dic = new Dictionary<string, string>
            {
                { "customerID",customerId},
                { "orderID",orderId}
            };

            var result = await WebAPIHelper.Get("/api/Order/GetDetail", dic);
            return result;
        }

        //查询预收款详细
        public async Task<string> GetDepositDetail( string orderId)
        {
            var dic = new Dictionary<string, string>
            { 
                { "orderID",orderId}
            };

            var result = await WebAPIHelper.Get("/api/Deposit/GetDetail", dic);
            return result;
        }
        //查询退项目单详细
        public async Task<string> GetBackOrderDetail(string customerId, string orderId)
        {
            var dic = new Dictionary<string, string>
            {
                { "customerID",customerId},
                { "orderID",orderId},
                { "userID",IDHelper.GetUserID().ToString()}
            };

            var result = await WebAPIHelper.Get("/api/BackOrder/GetDetail", dic);
            return result;
        }
        //查询退预收款详细
        public async Task<string> GetDepositRebateDetail(string customerId, string orderId)
        {
            var dic = new Dictionary<string, string>
            {
                { "customerID",customerId},
                { "orderID",orderId},
                { "userID",IDHelper.GetUserID().ToString()}
            };

            var result = await WebAPIHelper.Get("/api/DepositRebateOrder/GetDetail", dic);
            return result;
        }

        //收银台-今日发票记录
        public async Task<string> GetBillToday()
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID",IDHelper.GetHospitalID().ToString()}
            }; 
            var result = await WebAPIHelper.Get("/api/Bill/GetBillToday", dic);
            return result;
        }

        //结算记录
        public async Task<string> SettlementGet(SettlementSelect dto)
        {
            //dto.HospitalID = IDHelper.GetHospitalID();
           var result = await WebAPIHelper.Post("/api/Settlement/Get", dto);
            return result;
        }
    
        //结算时查询出的收银信息-
        public async Task<string> SettlementGetCashier()
        {
            var dic = new Dictionary<string, string>
            {
                { "userID",IDHelper.GetHospitalID().ToString()}
            }; 
            var result = await WebAPIHelper.Get("/api/Settlement/GetCashier", dic);
            return result;
        }

        //结算
        public async Task<string> AddSettlement(SettlementAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Settlement/AddSettlement", dto);
            return result;
        }

        //查询可开发票项目
        public async Task<string> GetCanBillCharges(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID",IDHelper.GetHospitalID().ToString()},
                { "customerID",customerId}
            };
            var result = await WebAPIHelper.Get("/api/Bill/GetCanBillCharges", dic);
            return result;
        }

        //添加发票
        public async Task<string> AddBill(BillAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Bill/Add", dto);
            return result;
        }
        //删除发票
        public async Task<string> DeleteBill(BillDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Bill/Delete", dto);
            return result;
        }

        //打印
        public async Task<string> CashierPrint(string cashierTodayId)
        {
            var dic = new Dictionary<string, string>
            { 
                { "ID",cashierTodayId}
            };
            var result = await WebAPIHelper.Get("/api/Cashier/Print", dic);
            return result;
        }

        //退项目单-取消(删除)
        public async Task<string> DeleteBackOrder(DepositOrderDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/BackOrder/Delete", dto);
            return result;
        }

        //订单-项目单取消(删除)
        public async Task<string> DeleteOrder(DepositOrderDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Order/Delete", dto);
            return result;
        }

        //退预收款-取消(删除)
        public async Task<string> DeleteDepositRebateOrderr(DepositOrderDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/DepositRebateOrder/Delete", dto);
            return result;
        }

        //预收款-取消(删除)
        public async Task<string> DeleteDeposit(DepositOrderDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Deposit/Delete", dto);
            return result;
        }


        #endregion
    }
}