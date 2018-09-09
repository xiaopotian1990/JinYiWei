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
    /// 床位相关接口
    /// </summary>
    public class BedController : ApiController
    {
        private IBedService _bedService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bedService"></param>
        public BedController(IBedService bedService)
        {
            _bedService = bedService;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(BedAdd dto)
        {
            return _bedService.Add(dto);
        }

        /// <summary>
        /// 部门修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(BedAdd dto)
        {
            return _bedService.Update(dto);
        }

        /// <summary>
        /// 查询当前医院下的所有直接部门
        /// </summary>
        /// <param name="hospitalID">所属医院ID</param>
        /// <param name="status">使用状态，默认所有</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Bed>> Get(long hospitalID, CommonStatus status = CommonStatus.All)
        {
            return _bedService.Get(hospitalID,status);
        }

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Bed> GetByID(long ID)
        {
            return _bedService.GetByID(ID);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _bedService.GetSelect(hospitalID);
        }

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(BedStop dto)
        {
            return _bedService.StopOrUse(dto);
        }
    }
}
