using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 客户组接口
    /// </summary>
   public interface ICustomerGroupService
    {
        /// <summary>
        /// 添加客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CustomerGroupAdd dto);

        /// <summary>
        /// 按照条件筛选出客户结果后保存客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CustomerFilterFiltrateAdd(CustomerFilterFiltrateAdd dto);

        /// <summary>
        /// 修改客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CustomerGroupUpdate dto);

        /// <summary>
        /// 查询所有客户组
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerGroupInfo>> Get(CustomerGroupSelect dto);


        /// <summary>
        /// 根据ID获取客户组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CustomerGroupInfo> GetByID(long id);

        /// <summary>
        /// 删除客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(CustomerGroupDelete dto);

        /// <summary>
        /// 根据客户组id查询客户组用户
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerGroupDetailInfo>>>> GetByCustomerGroupID(CustomerGroupDetailSelect dto);

        /// <summary>
        /// 添加客户组详情客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailAdd(CustomerGroupDetailAdd dto);



        /// <summary>
        ///删除全部客户组客户详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailDeleteAll(CustomerGroupDetailDeleteAll dto);

        /// <summary>
        /// 批量添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchCallbackAdd(CustomerGroupBatchCallbackAdd dto);

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchSSMAdd(BatchSSM dto);

        /// <summary>
        /// 查询用户创建的所有结果集
        /// </summary>
        /// <returns></returns>
        //IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerFilterInfo>> GetCustomerFilter(long createUserID);

        /// <summary>
        /// 根据客户组id查询客户组详情中的客户id
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<long>> GetByFilterID(long filterID);

        /// <summary>
        /// 合并客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> MergeCustomerFilterAdd(MergeCustomerFilter dto);

        /// <summary>
        /// 新增结果集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
       // IFlyDogResult<IFlyDogResultType, int> CustomerFilterAdd(CustomerFilterAdd dto);

        /// <summary>
        /// 新增结果集详情信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //IFlyDogResult<IFlyDogResultType, int> CustomerFilterDetailAdd(CustomerFilterDetailAdd dto);

        #region 按照条件查询接口
        /// <summary>
        ///基本条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetBasicConditionSelect(BasicConditionSelect dto);

        /// <summary>
        ///账户条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetAccountConditionSelect(AccountConditionSelect dto);

        /// <summary>
        ///上门条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetDoorConditionSelect(DoorConditionSelect dto);

        /// <summary>
        ///订单条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetOrderConditionSelect(OrderConditionSelect dto);

        /// <summary>
        ///咨询条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetConsultConditionSelect(ConsultConditionSelect dto);

        /// <summary>
        ///执行条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetExecuteConditionSelect(ExecuteConditionSelect dto);

        /// <summary>
        ///会员条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetMemberConditionSelect(MemberConditionSelect dto);

        /// <summary>
        ///未成交条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetFailtureConditionSelect(FailtureConditionSelect dto);

        /// <summary>
        ///标签条件查询
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetTagConditionSelect(TagConditionSelect dto);

        /// <summary>
        ///回访条件查询
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>> GetCallbackConditionSelect(CallbackConditionSelect dto);
        #endregion
    }
}
