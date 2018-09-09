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
    /// 药物品设置api
    /// 
    /// </summary>
    public class SmartProductController : ApiController
    {
        private ISmartProductService _smartProductService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartProductService"></param>
        public SmartProductController(ISmartProductService smartProductService)
        {
            _smartProductService = smartProductService;
        }
        #endregion

        #region 添加药物品设置
        /// <summary>
        /// 添加药物品设置[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">药物品设置信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartProductAdd dto)
        {
            return _smartProductService.Add(dto);
        }
        #endregion

        #region 药物品信息修改
        /// <summary>
        /// 药物品信息修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">药物品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartProductUpdate dto)
        {
            return _smartProductService.Update(dto);
        }
        #endregion

        #region 查询所有药物品信息
        /// <summary>
        /// 查询所有药物品信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartProductInfo>>> Get([FromBody]SmartProductSelect dto1)
        {
            return _smartProductService.Get(dto1);
        }
        #endregion
        /// <summary>
        /// 获取所有药物品信息不分页(后期可能还需要关联医院id，现在还没有，所以先查所有的)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetAll(SmartProductSelect dto)
        {
            return _smartProductService.GetAll(dto);
        }

        /// <summary>
        /// 根据仓库id查询仓库内的商品
        /// </summary>
        /// <param name="warehouseID">仓库id</param>
        /// <param name="type">类型1：入库2：调拨3：盘点4：科室领用</param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetByWarehouseIDDataAll([FromBody]SmartProductSelect dto)
        {
            return _smartProductService.GetByWarehouseIDDataAll(dto);
        }


        #region 
            /// <summary>
            /// 根据ID获取药物品信息
            /// </summary>
            /// <param name="id">药物品ID</param>
            /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartProductInfo> GetByID(long id)
        {
            return _smartProductService.GetByID(id);
        }
        #endregion

        #region 停用启用药物品信息
        /// <summary>
        /// 停用启用药物品信息信息[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">停用启用药物品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SmartProductDispose([FromBody]SmartProductStopOrUse dto)
        {
            return _smartProductService.SmartProductDispose(dto);
        }
        #endregion
    }
}
