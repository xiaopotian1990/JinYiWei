using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 科室中心相关接口
    /// </summary>
    public class DeptDeskController : ApiController
    {
        private IDeptDeskService _deptDeskService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deptDeskService"></param>
        public DeptDeskController(IDeptDeskService deptDeskService)
        {
            _deptDeskService = deptDeskService;
        }

        /// <summary>
        /// 今日科室上门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetDeptVisitTodayAsync(DeptVisitSelect dto)
        {
            return await _deptDeskService.GetDeptVisitTodayAsync(dto);
        }

        /// <summary>
        /// 今日医院上门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetHospitalVisitTodayAsync(DeptVisitSelect dto)
        {
            return await _deptDeskService.GetHospitalVisitTodayAsync(dto);
        }

        /// <summary>
        /// 查询顾客可划扣项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanOperation>>> GetCanOperation(long customerID)
        {
            return await _deptDeskService.GetCanOperation(customerID);
        }

        /// <summary>
        /// 获取今日划扣列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>> GetOperationToday(long hospitalID)
        {
            return await _deptDeskService.GetOperationToday(hospitalID);
        }

        /// <summary>
        /// 划扣详细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, OperationToday>> GetOperationDetail(long ID)
        {
            return await _deptDeskService.GetOperationDetail(ID);
        }

        /// <summary>
        /// 添加耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddProduct(OperationProductAdd dto)
        {
            return await _deptDeskService.AddProduct(dto);
        }

        /// <summary>
        /// 删除耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteProduct(OperationDelete dto)
        {
            return await _deptDeskService.DeleteProduct(dto);
        }

        /// <summary>
        /// 删除划扣记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteOperation(OperationDelete dto)
        {
            return await _deptDeskService.DeleteOperation(dto);
        }

        /// <summary>
        /// 添加划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddOperation(OperationAdd dto)
        {
            return await _deptDeskService.AddOperation(dto);
        }

        /// <summary>
        /// 获取耗材默认详细
        /// </summary>
        /// <param name="operationID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, DefaultChargeInfo>> GetDefaultChargeInfo(long operationID)
        {
            return await _deptDeskService.GetDefaultChargeInfo(operationID);
        }

        /// <summary>
        /// 更新划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateOperation(OperationUpdate dto)
        {
            return await _deptDeskService.UpdateOperation(dto);
        }
    }
}
