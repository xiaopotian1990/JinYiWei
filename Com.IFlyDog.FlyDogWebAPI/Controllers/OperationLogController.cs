using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 系统日志api
    /// </summary>
    public class OperationLogController : ApiController
    {
        private IOperationLogService _operationLogService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="operationLogService"></param>
        public OperationLogController(IOperationLogService operationLogService)
        {
            _operationLogService = operationLogService;
        }
        #endregion

        #region 查询所有系统日志信息
        /// <summary>
        /// 查询所有系统日志信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartOperationLog>> Get()
        {
            return _operationLogService.Get();
        }
        #endregion

        /// <summary>
        /// 获取日志下拉
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<OperationLogType>> GetLogSelect()
        {
            return _operationLogService.GetLogSelect();
        }

        #region 根据id查询日志信息
            /// <summary>
            /// 根据id查询日志信息
            /// </summary>
            /// <param name="id">日志ID</param>
            /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartOperationLog> GetByID(long id)
        {
            return _operationLogService.GetByID(id);
        }
        #endregion

        /// <summary>
        /// 分页查询系统日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartOperationLog>>> GetPages(OperationLogSelect dto)
        {
            return _operationLogService.GetPages(dto);
        }
    }
}
