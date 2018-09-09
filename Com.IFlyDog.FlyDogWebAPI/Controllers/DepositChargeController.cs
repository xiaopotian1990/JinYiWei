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
    /// 预收款设置api
    /// </summary>
    public class DepositChargeController : ApiController
    {
        private IDepositChargeService _depositChargeService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="depositChargeService"></param>
        public DepositChargeController(IDepositChargeService depositChargeService)
        {
            _depositChargeService = depositChargeService;
        }

        /// <summary>
        /// 添加预收款类型管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">预收款</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]DepositChargeAdd dto)
        {
            return _depositChargeService.Add(dto);
        }

        // <summary>
        /// 修改预收款类型[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">预收款</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]DepositChargeUpdate dto)
        {
            return _depositChargeService.Update(dto);
        }

        /// <summary>
        /// 获取全部代收款类型，
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeInfo>> Get()
        {
            return _depositChargeService.Get();
        }

        /// <summary>
        /// 根据id获取预收款类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, DepositChargeInfo> GetByID(long id)
        {
            return _depositChargeService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _depositChargeService.GetSelect(hospitalID);
        }
    }
}
