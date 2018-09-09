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
    /// 医院管理api接口
    /// </summary>
    public class HospitalController : ApiController
    {
        private IHospitalService _hospitalService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalService"></param>
        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        #endregion

        #region 查询所有医院信息
        /// <summary>
        /// 查询医院
        /// </summary>
        /// <param name="id">查询所有医院输入0，其他输入医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalInfo>> Get(long id)
        {
            return _hospitalService.Get(id);
        }
        #endregion

        /// <summary>
        /// 下拉菜单,传0查询所有医院
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _hospitalService.GetSelect(hospitalID);
        }
    }
}
