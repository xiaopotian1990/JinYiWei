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
    /// 仓库管理api控制器
    /// </summary>
    public class SmartWarehouseController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ISmartWarehouseService _smartWarehouseService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="smartWarehouseService"></param>
        public SmartWarehouseController(ISmartWarehouseService smartWarehouseService)
        {
            _smartWarehouseService = smartWarehouseService;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> Get(string hospitalId)
        {
            return _smartWarehouseService.Get(hospitalId);
        }

        /// <summary>
        /// 查询所有(根据用户id查询用户所能操作的仓库)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> GetByUserId(string userId, string hospitalId)
        {
            return _smartWarehouseService.GetByUserId(userId, hospitalId);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartWarehouseInfo> GetByID(long id)
        {
            return _smartWarehouseService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartWarehouseAdd dto)
        {
            return _smartWarehouseService.Add(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartWarehouseUpdate dto)
        {
            return _smartWarehouseService.Update(dto);
        }

        /// <summary>
        /// 删除仓库信息（注 如果仓库里有商品了就不能再删除了）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(SmartWarehouseDelete dto)
        {
            return _smartWarehouseService.Delete(dto);
        }

        /// <summary>
        /// 下拉菜单,医院下所有仓库
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _smartWarehouseService.GetSelect(hospitalID);
        }

        /// <summary>
        /// 下拉菜单,用户管辖的仓库
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelectByUserID(long hospitalID, long userID)
        {
            return _smartWarehouseService.GetSelectByUserID(hospitalID, userID);
        }
    }
}
