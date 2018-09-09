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
    /// 收费项目api
    /// </summary>
    public class ChargeController : ApiController
    {
        private IChargeService _chargeService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="chargeService"></param>
        public ChargeController(IChargeService chargeService)
        {
            _chargeService = chargeService;
        }
        #endregion

        #region 添加收费项目
        /// <summary>
        /// 添加收费项目[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加收费项目</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ChargeAdd dto)
        {
            return _chargeService.Add(dto);
        }
        #endregion

        #region 修改收费项目
        /// <summary>
        /// 修改收费项目[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改收费项目</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ChargeUpdate dto)
        {
            return _chargeService.Update(dto);
        }
        #endregion

        /// <summary>
        /// 查询所有收费项目信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeInfo>>> Get([FromBody]ChargeSelect dto1)
        {
            return _chargeService.Get(dto1);
        }

        /// <summary>
        /// 检测收费项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeCheckItem>>> GetCheckCharge(ChargeSelect dto)
        {
            return _chargeService.GetCheckCharge(dto);
        }

        /// <summary>
        /// 查询所有收费项目信息（不分页给弹窗使用）
        /// </summary>
        /// <returns></returns>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeInfo>> GetData(ChargeSelect dto)
        {
            return _chargeService.GetData(dto);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ChargeInfo> GetByID(long id)
        {
            return _chargeService.GetByID(id);
        }


    }
}
