using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{ 
    public class CustomerController : Controller
    {
        private Session session = new Session();

        // GET: 顾客管理
        #region 页面

        #region 全部客户列表-页面

        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region 客户详细-操作-页面 

        public ActionResult CustomerProfile()
        {
            return View();
        }
        #endregion

        #region 添加订单-页面

        public ActionResult AddOrder()
        {
            return View();
        }
        #endregion

        #region 添加咨询-页面

        public ActionResult AddConsult()
        {
            return View();
        }
        #endregion

        #region 添加预收款-页面

        public ActionResult AddDeposit()
        {
            return View();
        }
        #endregion

        #region 添加退预收款-页面

        public ActionResult AddRebateDeposit()
        {
            return View();
        }
        #endregion

        #region 添加退项目单-页面

        public ActionResult AddBackOrder()
        {
            return View();
        }
        #endregion

        #region 添加客户关系-页面

        public ActionResult AddRelation()
        {
            return View();
        }
        #endregion

        #region 添加店铺佣金-页面

        public ActionResult AddStoreCommission()
        {
            return View();
        }
        #endregion

        #region 添加未成交类型-页面

        public ActionResult AddFailture()
        {
            return View();
        }
        #endregion

        #region 顾客-添加咨询预约-页面

        public ActionResult Appointment()
        {
            return View();
        }
        #endregion

        #region 顾客-添加治疗预约-页面

        public ActionResult Treat()
        {
            return View();
        }
        #endregion  Surgery

        #region 顾客-添加手术-页面

        public ActionResult Surgery()
        {
            return View();
        }
        #endregion

        #region 添加退项目单-页面 
        public ActionResult EditCustomerInfo()
        {
            return View();
        }
        #endregion

        #region 照片管理
        public ActionResult CustomerPotoInfo()
        {
            return View();
        }
        #endregion

        #region 添加标签-页面
        public ActionResult AddTags()
        {
            return View();
        }
        #endregion
         
        #region 激活券-页面
        public ActionResult AddCoupon()
        {
            return View();
        }
        #endregion

        #region 添加回访页面
        public ActionResult CallBack()
        {
            return View();
        }
        #endregion

        #region 添加回访提示页面
        public ActionResult CallBackRemind()
        {
            return View();
        }
        #endregion

        #region 添加回访提示页面
        public ActionResult CallBackPlan()
        {
            return View();
        }
        #endregion

        #endregion

        #region Customer API接口

        #region 查询所有顾客及分页
        public async Task<string> GetCustomerManager(CustomerSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Customer/GetCustomerManager", dto);
            return result;

        }
        #endregion

        #region 顾客识别
        public async Task<string> CustomerIdentifyAsync(string name)
        {
            var dic = new Dictionary<string, string> { { "name", name } };
            var result = await WebAPIHelper.Get("/api/Customer/CustomerIdentifyAsync", dic);
            return result;

        }
        #endregion

        #region 登记新顾客-网店工作台
        public async Task<string> AddAsync(CustomerAdd dto)
        {
            
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CustomerRegisterType = CustomerRegisterType.Exploit;
            var result = await WebAPIHelper.Post("/api/Customer/AddAsync", dto);
            return result;
        }
        #endregion

        #region 登记新顾客-前台接待
        public async Task<string> AddAsyncByFore(CustomerAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CustomerRegisterType = CustomerRegisterType.ForeGround;
            var result = await WebAPIHelper.Post("/api/Customer/AddAsync", dto);
            return result;
        }
        #endregion

        #region 登记新顾客-市场工作台
        public async Task<string> AddAsyncByMarketDesk(CustomerAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CustomerRegisterType = CustomerRegisterType.Market;
            var result = await WebAPIHelper.Post("/api/Customer/AddAsync", dto);
            return result;
        }
        #endregion
        
        #region 今日新登记顾客-网电工作台  
        public async Task<string> CustomerCreateTodayAsync()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var type = CustomerRegisterType.Exploit.ToString();
            var dic = new Dictionary<string, string>();
            dic.Add("hospitalID", hospitalId);
            dic.Add("type", type);
            var result = await WebAPIHelper.Get("/api/Customer/CustomerCreateTodayAsync", dic);
            return result;

        }
        #endregion

        #region 今日新登记顾客-前台接待  
        public async Task<string> CustomerCreateTodayAsyncByForeGround()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var type = CustomerRegisterType.ForeGround.ToString();
            var dic = new Dictionary<string, string>();
            dic.Add("hospitalID", hospitalId);
            dic.Add("type", type);
            var result = await WebAPIHelper.Get("/api/Customer/CustomerCreateTodayAsync", dic);
            return result;

        }
        #endregion

        #region 今日新登记顾客-市场工作台  
        public async Task<string> CustomerCreateTodayAsyncByMarketDesk()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var type = CustomerRegisterType.Market.ToString();
            var dic = new Dictionary<string, string>();
            dic.Add("hospitalID", hospitalId);
            dic.Add("type", type);
            var result = await WebAPIHelper.Get("/api/Customer/CustomerCreateTodayAsync", dic);
            return result;

        }
        #endregion

        #region 根据省级ID查询市级
        public async Task<string> GetCity(int provinceID)
        {
            var dic = new Dictionary<string, string> { { "provinceID", provinceID.ToString() } };
            var result = await WebAPIHelper.Get("/api/Province/GetCity", dic);
            return result;

        }
        #endregion

        #region 根据手机号查询手机号归属地 
        public async Task<string> GetProvinceCityByPhone(string phone)
        {
            var dic = new Dictionary<string, string> { { "phone", phone } };
            var result = await WebAPIHelper.Get("/api/Province/GetProvinceCity", dic);
            return result;

        }
        #endregion


        #endregion
    }
}