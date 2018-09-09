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
    public class DeparDeskController : Controller
    {
        #region 页面
        // 科室中心-主页
        public ActionResult Index()
        {
            return View();
        }
        //添加划扣页面
        public ActionResult OperationAdd()
        {
            return View();
        }

        #endregion

        #region api接口
      
        //查询医院上门-api 
        public async Task<string> GetHospitalVisitTodayAsync(DeptVisitSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DeptDesk/GetHospitalVisitTodayAsync", dto);
            return result;
        } 
        //今日科室顾客-api 
        public async Task<string> GetDeptVisitTodayAsync(DeptVisitSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DeptDesk/GetDeptVisitTodayAsync", dto);
            return result;
        }
        //查询顾客可划扣项目
        public async Task<string> GetCanOperation(string customerId)
        {
            var dic = new Dictionary<string, string> { { "customerID", customerId } };
            var result = await WebAPIHelper.Get("/api/DeptDesk/GetCanOperation", dic);
            return result;
        }

        //获取今日划扣列表-(执行列表)
        public async Task<string> GetOperationToday()
        {
            var dic = new Dictionary<string, string> { { "hospitalID", IDHelper.GetHospitalID().ToString() } };
            var result = await WebAPIHelper.Get("/api/DeptDesk/GetOperationToday", dic);
            return result;
        }
        //划扣详细
        public async Task<string> GetOperationDetail(string id)
        {
            var dic = new Dictionary<string, string> { { "ID", id } };
            var result = await WebAPIHelper.Get("/api/DeptDesk/GetOperationDetail", dic);
            return result;
        }
        //修改划扣-提交
        public async Task<string> UpdateOperation(OperationUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DeptDesk/UpdateOperation", dto);
            return result;
        }

        //添加耗材
        public async Task<string> AddProduct(OperationProductAdd dto)
        { 
            var result = await WebAPIHelper.Post("/api/DeptDesk/AddProduct", dto);
            return result;
        }
        //删除耗材
        public async Task<string> DeleteProduct(OperationDelete dto)
        {
            var result = await WebAPIHelper.Post("/api/DeptDesk/DeleteProduct", dto);
            return result;
        }

        //添加划扣
        public async Task<string> AddOperation(OperationAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DeptDesk/AddOperation", dto);
            return result;
        }

        //删除划扣记录
        public async Task<string> DeleteOperation(OperationDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            
            var result = await WebAPIHelper.Post("/api/DeptDesk/DeleteOperation", dto);
            return result;
        }

        //获取耗材默认详细 
        public async Task<string> GetDefaultChargeInfo(string operationId)
        {
            var dic = new Dictionary<string, string> { { "operationID", operationId } };
            var result = await WebAPIHelper.Get("/api/DeptDesk/GetDefaultChargeInfo", dic);
            return result;
        }

        #endregion
    }
}