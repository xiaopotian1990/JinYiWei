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
    /// 排班api
    /// </summary>
    public class ShiftController : ApiController
    {
        private IShiftService _shiftService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="shiftService"></param>
        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }
        #endregion

        #region 添加排班
        /// <summary>
        /// 添加排班[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ShiftAdd dto)
        {
            return _shiftService.Add(dto);
        }
        #endregion

        #region 修改排班
        /// <summary>
        /// 修改排班[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改排班</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ShiftUpdate dto)
        {
            return _shiftService.Update(dto);
        }
        #endregion

        #region 查询所有排班信息
        /// <summary>
        /// 查询所有排班信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Shift> Get(string hospitalID, int number)
        {
            return _shiftService.Get(Convert.ToInt64(hospitalID),number);
        }
        #endregion

        #region 根据ID获取排班信息
        /// <summary>
        /// 根据ID获取排班信息
        /// </summary>
        /// <param name="id">排班ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ShiftInfo> GetByID(long id)
        {
            return _shiftService.GetByID(id);
        }
        #endregion

        #region 删除排班信息
        /// <summary>
        /// 删除排班信息[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除排班信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]ShiftDelete dto)
        {
            return _shiftService.Delete(dto);
        }
        #endregion
    }
}
