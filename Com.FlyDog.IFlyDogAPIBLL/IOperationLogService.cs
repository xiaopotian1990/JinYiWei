using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 系统日志接口
    /// </summary>
    public interface IOperationLogService
    {
        /// <summary>
        /// 查询所有系统日志
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartOperationLog>> Get();

        /// <summary>
        /// 查询所有系统日志
        /// </summary>SmartSupplierSelect
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartOperationLog>>> GetPages(OperationLogSelect dto);

        /// <summary>
        /// 根据ID获取系统日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartOperationLog> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<OperationLogType>> GetLogSelect();

        }
}
