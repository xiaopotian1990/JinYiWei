using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 客户组api
    /// </summary>
    public class CustomerGroupController : ApiController
    {
        private ICustomerGroupService _customerGroupService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="customerGroupService"></param>
        public CustomerGroupController(ICustomerGroupService customerGroupService)
        {
            _customerGroupService = customerGroupService;
        }
        #endregion

        #region 添加客户组
        /// <summary>
        /// 添加客户组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加客户组</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]CustomerGroupAdd dto)
        {
            return _customerGroupService.Add(dto);
        }
        #endregion

        #region 按照条件筛选出客户结果后保存客户组
        /// <summary>
        /// 按照条件筛选出客户结果后保存客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CustomerFilterFiltrateAdd(CustomerFilterFiltrateAdd dto)
        {
            return _customerGroupService.CustomerFilterFiltrateAdd(dto);
        }
        #endregion

        /// <summary>
        /// 查询用户创建的所有结果集
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //[ModuleAuthorization("CRM")]
        //public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerFilterInfo>> GetCustomerFilter(long createUserID)
        //{
        //    return _customerGroupService.GetCustomerFilter(createUserID);
        //}

        ///// <summary>
        ///// 根据结果集id查询结果集详情中的客户id
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //[ModuleAuthorization("CRM")]
        //public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerFilterDetailInfo>> GetByFilterID(long filterID)
        //{
        //    return _customerGroupService.GetByFilterID(filterID);
        //}

        /// <summary>
        /// 批量添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchCallbackAdd(CustomerGroupBatchCallbackAdd dto)
        {
            return await _customerGroupService.CustomerGroupBatchCallbackAdd(dto);
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchSSMAdd(BatchSSM dto)
        {
            return await _customerGroupService.CustomerGroupBatchSSMAdd(dto);
        }

        /// <summary>
        /// 添加客户组详情客户
        /// </summary>CustomerGroupBatchCallbackAdd
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailAdd(CustomerGroupDetailAdd dto)
        {
            return _customerGroupService.CustomerGroupDetailAdd(dto);
        }

        /// <summary>
        ///删除全部客户组客户详情
        /// </summary>
        /// <param name="dto"></param>CustomerGroupDetailAdd
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailDeleteAll(CustomerGroupDetailDeleteAll dto)
        {
            return _customerGroupService.CustomerGroupDetailDeleteAll(dto);
        }

        /// <summary>
        /// 删除客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(CustomerGroupDelete dto)
        {
            return _customerGroupService.Delete(dto);
        }

        /// <summary>
        /// 列表展示客户组
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerGroupInfo>> Get(CustomerGroupSelect dto)
        {
            return _customerGroupService.Get(dto);
        }

        /// <summary>
        /// 根据客户组id查询客户组用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerGroupDetailInfo>>>> GetByCustomerGroupID(CustomerGroupDetailSelect dto)
        {
            return await _customerGroupService.GetByCustomerGroupID(dto);
        }

        /// <summary>
        /// 根据id查询客户组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, CustomerGroupInfo> GetByID(long id)
        {
            return _customerGroupService.GetByID(id);
        }

        /// <summary>
        /// 更新客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(CustomerGroupUpdate dto)
        {
            return _customerGroupService.Update(dto);
        }

        /// <summary>
        /// 合并客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> MergeCustomerFilterAdd(MergeCustomerFilter dto)
        {
            return _customerGroupService.MergeCustomerFilterAdd(dto);
        }

        /// <summary>
        /// 新增结果集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ModuleAuthorization("CRM")]
        //public IFlyDogResult<IFlyDogResultType, int> CustomerFilterAdd(CustomerFilterAdd dto)
        //{
        //    return _customerGroupService.CustomerFilterAdd(dto);
        //}

        /// <summary>
        /// 新增结果集详情信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //[HttpPost]
        //[ModuleAuthorization("CRM")]
        //public IFlyDogResult<IFlyDogResultType, int> CustomerFilterDetailAdd(CustomerFilterDetailAdd dto)
        //{
        //    return _customerGroupService.CustomerFilterDetailAdd(dto);
        //}

        #region 按照条件查询接口
        /// <summary>
        /// 基本条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetBasicConditionSelect(BasicConditionSelect dto)
        {
            return await _customerGroupService.GetBasicConditionSelect(dto);
        }

        /// <summary>
        /// 账户条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetAccountConditionSelect(AccountConditionSelect dto)
        {
            return await _customerGroupService.GetAccountConditionSelect(dto);
        }

        /// <summary>
        /// 上门条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetDoorConditionSelect(DoorConditionSelect dto)
        {
            return await _customerGroupService.GetDoorConditionSelect(dto);
        }

        /// <summary>
        /// 订单条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetOrderConditionSelect(OrderConditionSelect dto)
        {
            return await _customerGroupService.GetOrderConditionSelect(dto);
        }

        /// <summary>
        /// 咨询条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetConsultConditionSelect(ConsultConditionSelect dto)
        {
            return await _customerGroupService.GetConsultConditionSelect(dto);
        }

        /// <summary>
        /// 执行条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetExecuteConditionSelect(ExecuteConditionSelect dto)
        {
            return await _customerGroupService.GetExecuteConditionSelect(dto);
        }

        /// <summary>
        /// 会员条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetMemberConditionSelect(MemberConditionSelect dto)
        {
            return await _customerGroupService.GetMemberConditionSelect(dto);
        }

        /// <summary>
        /// 未成交条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetFailtureConditionSelect(FailtureConditionSelect dto)
        {
            return await _customerGroupService.GetFailtureConditionSelect(dto);
        }

        /// <summary>
        /// 标签条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetTagConditionSelect(TagConditionSelect dto)
        {
            return await _customerGroupService.GetTagConditionSelect(dto);
        }

        /// <summary>
        /// 回访条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>> GetCallbackConditionSelect(CallbackConditionSelect dto)
        {
            return _customerGroupService.GetCallbackConditionSelect(dto);
        }
        #endregion
    }
}
