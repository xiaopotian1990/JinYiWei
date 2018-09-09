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
    /// 店铺相关接口
    /// </summary>
    public class StoreController : ApiController
    {
        private IStoreService _storeService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="storeService"></param>
        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }
        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _storeService.GetSelect(hospitalID);
        }

        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(StoreAdd dto)
        {
            return _storeService.Add(dto);
        }

        /// <summary>
        /// 删除店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(StoreDelete dto)
        {
            return _storeService.Delete(dto);
        }

        /// <summary>
        /// 查询全部店家信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<StoreInfo>>> Get(StoreSelect dto)
        {
            return _storeService.Get(dto);
        }

        /// <summary>
        ///查询所有店家信息（不分页，主要给弹窗使用）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfo>> GetNoPageStoreInfo(StoreSelect dto)
        {
            return _storeService.GetNoPageStoreInfo(dto);
        }

        /// <summary>
        ///  根据医院id，用户id查询当前用户是否有权限操作更新等方法
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> GetByHCCount(string storeID, string crateUserID)
        {
            return _storeService.GetByHCCount(storeID, crateUserID);
        }

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, StoreInfo> GetByID(string id)
        {
            return _storeService.GetByID(Convert.ToInt64(id));
        }

        /// <summary>
        /// 根据店家id获取店家基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, StoreBasicInfo> GetByIDStoreBasicData(string id)
        {
            return _storeService.GetByIDStoreBasicData(Convert.ToInt64(id));
        }

        /// <summary>
        /// 根据店家id获取店家佣金记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreCommissionInfo>> GetByIDStoreCommissionData(string id)
        {
            return _storeService.GetByIDStoreCommissionData(Convert.ToInt64(id));
        }

        /// <summary>
        /// 根据店家id获取店家客户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManagerInfo>> GetByIDStoreManagerData(string id)
        {
            return _storeService.GetByIDStoreManagerData(Convert.ToInt64(id));
        }

        /// <summary>
        /// 根据店家id获取店家回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreSaleBackInfo>> GetByIDStoreSaleBackData(string id)
        {
            return _storeService.GetByIDStoreSaleBackData(Convert.ToInt64(id));
        }

        /// <summary>
        /// 修改店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(StoreUpdate dto)
        {
            return _storeService.Update(dto);
        }
    }
}
