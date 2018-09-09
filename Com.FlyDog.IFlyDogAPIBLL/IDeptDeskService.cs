using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IDeptDeskService
    {
        /// <summary>
        /// 今日科室上门
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetDeptVisitTodayAsync(DeptVisitSelect dto);
        /// <summary>
        /// 今日医院上门
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetHospitalVisitTodayAsync(DeptVisitSelect dto);

        /// <summary>
        /// 查询顾客可划扣项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanOperation>>> GetCanOperation(long customerID);

        /// <summary>
        /// 获取今日划扣列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>> GetOperationToday(long hospitalID);

        /// <summary>
        /// 划扣详细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, OperationToday>> GetOperationDetail(long ID);

        /// <summary>
        /// 添加耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddProduct(OperationProductAdd dto);

        /// <summary>
        /// 删除耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeleteProduct(OperationDelete dto);

        /// <summary>
        /// 删除划扣记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeleteOperation(OperationDelete dto);

        /// <summary>
        /// 添加划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddOperation(OperationAdd dto);

        /// <summary>
        /// 获取耗材默认详细
        /// </summary>
        /// <param name="operationID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, DefaultChargeInfo>> GetDefaultChargeInfo(long operationID);

        /// <summary>
        /// 更新划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateOperation(OperationUpdate dto);
    }
}
