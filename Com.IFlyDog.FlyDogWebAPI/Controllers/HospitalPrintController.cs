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
    /// 打印设置api
    /// </summary>
    public class HospitalPrintController : ApiController
    {
        private IHospitalPrintService _hospitalPrintService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalPrintService"></param>
        public HospitalPrintController(IHospitalPrintService hospitalPrintService)
        {
            _hospitalPrintService = hospitalPrintService;
        }
        #endregion

        #region 打印设置修改
        /// <summary>
        /// 打印设置修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">打印设置修改</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]HospitalPrintUpdate dto)
        {
            return _hospitalPrintService.Update(dto);
        }
        #endregion

        #region 查询所有打印设置
        /// <summary>
        /// 查询所有打印设置[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalPrintInfo>> Get(string hospitalID)
        {
            return _hospitalPrintService.Get(hospitalID);
        }
        #endregion

        /// <summary>
        /// 根据医院id，类型查询打印设置
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByHospitalAndType(long hospitalID, string type)
        {
            return _hospitalPrintService.GetByHospitalAndType(hospitalID, type);
        }

        #region 根据id查询打印设置
            /// <summary>
            /// 根据id查询打印设置
            /// </summary>
            /// <param name="id">打印ID</param>
            /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByID(long id)
        {
            return _hospitalPrintService.GetByID(id);
        }
        #endregion
    }
}
