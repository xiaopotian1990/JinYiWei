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
    /// 进货信息api控制器
    /// </summary>
    public class SmartPurchaseController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ISmartPurchaseService _smartPurchaseService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="smartPurchaseService"></param>
        public SmartPurchaseController(ISmartPurchaseService smartPurchaseService)
        {
            _smartPurchaseService = smartPurchaseService;
        }

        /// <summary>
        /// 查询所有进货信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartPurchaseInfo>>> Get([FromBody]SmartPurchaseSelect dto1)
        {
            return _smartPurchaseService.Get(dto1);
        }

        /// <summary>
        /// 根据医院id查询当前医院所有的进货记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartPurchaseInfo>> GetByHospitalID(SmartPurchaseSelect dto)
        {
            return _smartPurchaseService.GetByHospitalID(dto);
        }




        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartPurchaseInfo> GetByID(long id)
        {
            return _smartPurchaseService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartPurchaseAdd dto)
        {
            return _smartPurchaseService.Add(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartPurchaseUpdate dto)
        {
            return _smartPurchaseService.Update(dto);
        }

        /// <summary>
        /// 删除供应商管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartPurchaseDelete dto)
        {
            return _smartPurchaseService.SmartPurchaseDelete(dto);
        }

        /// <summary>
        /// 根据仓库进货id查询仓库进货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="purchaspID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, string> SmartPurchasePrint(string purchaspID, long hospitalID)
        {
            return _smartPurchaseService.SmartPurchasePrint(purchaspID, hospitalID);
        }
    }
}
