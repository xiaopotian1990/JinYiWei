using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ICustomerProfileService
    {
        /// <summary>
        /// 客户档案里面的预约情况
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileAppoint>> GetAppointment(long userID, long customerID);

        /// <summary>
        /// 客户档案上门情况
        /// </summary>
        /// <param name="userID">操作用户</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileVisitCase>> GetVisit(long userID, long customerID);

        /// <summary>
        /// 标签组选择
        /// </summary>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileTagGroup>>> SelectTageGroup();

        /// <summary>
        /// 添加顾客标签的时候查询出来的详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerTag>> GetCustomerTageGroup(long userID, long customerID);

        /// <summary>
        /// 客户档案添加标签
        /// </summary>
        /// <param name="dto">标签信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddTag(ProfileCustomerTagAdd dto);

        /// <summary>
        /// 客户档案添加关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddRelation(ProfileRelationAdd dto);

        /// <summary>
        /// 获取客户间关系
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileRelation>>> GetRelation(long userID, long customerID);

        /// <summary>
        /// 客户档案详细查询
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">操作人所在医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerDetail>> GetCustomerDetail(long userID, long customerID, long hospitalID);

        /// <summary>
        /// 修改渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateChannel(ProfileCustomerUpdate dto);

        /// <summary>
        /// 修改联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobile(ProfileCustomerUpdate dto);

        /// <summary>
        /// 修改备用联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobileBackup(ProfileCustomerUpdate dto);

        /// <summary>
        /// 更新主咨询项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCurrentConsultSymptom(ProfileCustomerUpdate dto);

        /// <summary>
        /// 更新推荐店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateStore(ProfileCustomerUpdate dto);

        /// <summary>
        /// 清除微信绑定
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> ClearWechatBinding(ProfileCustomerUpdate dto);

        /// <summary>
        /// 顾客资料编辑之前获取信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerInfo>> GetCustomerInfoUpdate(long userID, long customerID);

        /// <summary>
        /// 顾客详细资料编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CustomerInfoUpdate(ProfileCustomerInfoUpdate dto);

        /// <summary>
        /// 添加佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddStoreCommission(ProfileStoreCommissionAdd dto);

        /// <summary>
        /// 删除佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeleteStoreCommission(ProfileStoreCommissionDelete dto);

        /// <summary>
        /// 查询激活码信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileActiveCoupon>> GetActiveCoupon(string code);

        /// <summary>
        /// 券激活
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddActiveCoupon(ProfileActiveCouponAdd dto);

        /// <summary>
        /// 店铺佣金记录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerStoreCommission>> GetCustomerStoreCommission(long userID, long customerID);

        /// <summary>
        /// 客户档案删除关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeleteRelation(CommonDelete dto);

        /// <summary>
        /// 客户档案获取负责用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileOwinnerShip>> GetOwinerShip(long userID, long customerID);

        /// <summary>
        /// 查询历史信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileOwinerShipTemp>>> GetOwinerShipHistory(long userID, long customerID, OwnerShipType type);

        /// <summary>
        /// 客户档案订单情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileOrders>> GetOrders(long userID, long customerID);

        /// <summary>
        /// 客户档案账户情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileMoney>> GetMoney(long userID, long customerID);

        /// <summary>
        /// 客户档案消费项目
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerCharges>> GetCharges(long userID, long customerID);

        /// <summary>
        /// 客户档案划扣记录
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>> GetOperation(long userID, long customerID);
    }
}
