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
    /// 顾客档案相关接口
    /// </summary>
    public class CustomerProfileController : ApiController
    {
        private ICustomerProfileService _customerProfileService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customerProfileService"></param>
        public CustomerProfileController(ICustomerProfileService customerProfileService)
        {
            _customerProfileService = customerProfileService;
        }


        #region 预约
        /// <summary>
        /// 客户档案里面的预约情况
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileAppoint>> GetAppointment(long userID, long customerID)
        {
            return await _customerProfileService.GetAppointment(userID, customerID);
        }
        #endregion

        #region 上门情况
        /// <summary>
        /// 客户档案上门情况
        /// </summary>
        /// <param name="userID">操作用户</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileVisitCase>> GetVisit(long userID, long customerID)
        {
            return await _customerProfileService.GetVisit(userID, customerID);
        }
        #endregion

        #region 标签
        /// <summary>
        /// 标签组选择
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileTagGroup>>> SelectTageGroup()
        {
            return await _customerProfileService.SelectTageGroup();
        }

        /// <summary>
        /// 添加顾客标签的时候查询出来的详细信息
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerTag>> GetCustomerTageGroup(long userID, long customerID)
        {
            return await _customerProfileService.GetCustomerTageGroup(userID, customerID);
        }

        /// <summary>
        /// 客户档案添加标签
        /// </summary>
        /// <param name="dto">标签信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddTag(ProfileCustomerTagAdd dto)
        {
            return await _customerProfileService.AddTag(dto);
        }
        #endregion

        #region 关系
        /// <summary>
        /// 客户档案添加关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddRelation(ProfileRelationAdd dto)
        {
            return await _customerProfileService.AddRelation(dto);
        }

        /// <summary>
        /// 获取客户间关系
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileRelation>>> GetRelation(long userID, long customerID)
        {
            return await _customerProfileService.GetRelation(userID, customerID);
        }

        /// <summary>
        /// 客户档案删除关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteRelation(CommonDelete dto)
        {
            return await _customerProfileService.DeleteRelation(dto);
        }
        #endregion

        #region 顾客信息
        /// <summary>
        /// 客户档案详细查询
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">操作人所在医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerDetail>> GetCustomerDetail(long userID, long customerID, long hospitalID)
        {
            return await _customerProfileService.GetCustomerDetail(userID, customerID, hospitalID);
        }

        /// <summary>
        /// 修改渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateChannel(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.UpdateChannel(dto);
        }

        /// <summary>
        /// 修改联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobile(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.UpdateMobile(dto);
        }

        /// <summary>
        /// 修改备用联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobileBackup(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.UpdateMobileBackup(dto);
        }

        /// <summary>
        /// 更新主咨询项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCurrentConsultSymptom(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.UpdateCurrentConsultSymptom(dto);
        }

        /// <summary>
        /// 更新推荐店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateStore(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.UpdateStore(dto);
        }

        /// <summary>
        /// 清除微信绑定
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ClearWechatBinding(ProfileCustomerUpdate dto)
        {
            return await _customerProfileService.ClearWechatBinding(dto);
        }

        /// <summary>
        /// 顾客资料编辑之前获取信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerInfo>> GetCustomerInfoUpdate(long userID, long customerID)
        {
            return await _customerProfileService.GetCustomerInfoUpdate(userID, customerID);
        }

        /// <summary>
        /// 顾客详细资料编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerInfoUpdate(ProfileCustomerInfoUpdate dto)
        {
            return await _customerProfileService.CustomerInfoUpdate(dto);
        }
        #endregion

        #region 佣金
        /// <summary>
        /// 添加佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddStoreCommission(ProfileStoreCommissionAdd dto)
        {
            return await _customerProfileService.AddStoreCommission(dto);
        }

        /// <summary>
        /// 删除佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteStoreCommission(ProfileStoreCommissionDelete dto)
        {
            return await _customerProfileService.DeleteStoreCommission(dto);
        }

        /// <summary>
        /// 店铺佣金记录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerStoreCommission>> GetCustomerStoreCommission(long userID, long customerID)
        {
            return await _customerProfileService.GetCustomerStoreCommission(userID, customerID);
        }
        #endregion

        #region 券激活
        /// <summary>
        /// 查询激活码信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileActiveCoupon>> GetActiveCoupon(string code)
        {
            return await _customerProfileService.GetActiveCoupon(code);
        }

        /// <summary>
        /// 券激活
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddActiveCoupon(ProfileActiveCouponAdd dto)
        {
            return await _customerProfileService.AddActiveCoupon(dto);
        }
        #endregion

        #region 负责用户
        /// <summary>
        /// 客户档案获取负责用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileOwinnerShip>> GetOwinerShip(long userID, long customerID)
        {
            return await _customerProfileService.GetOwinerShip(userID, customerID);
        }

        /// <summary>
        /// 查询历史信息
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="type">咨询师类型</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileOwinerShipTemp>>> GetOwinerShipHistory(long userID, long customerID, OwnerShipType type)
        {
            return await _customerProfileService.GetOwinerShipHistory(userID, customerID, type);
        }
        #endregion

        #region 订单情况
        /// <summary>
        /// 客户档案订单情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileOrders>> GetOrders(long userID, long customerID)
        {
            return await _customerProfileService.GetOrders(userID, customerID);
        }
        #endregion

        #region 账户情况
        /// <summary>
        /// 客户档案账户情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileMoney>> GetMoney(long userID, long customerID)
        {
            return await _customerProfileService.GetMoney(userID, customerID);
        }
        #endregion

        #region 消费项目
        /// <summary>
        /// 客户档案消费项目
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerCharges>> GetCharges(long userID, long customerID)
        {
            return await _customerProfileService.GetCharges(userID, customerID);
        }
        #endregion

        #region 客户档案划扣记录
        /// <summary>
        /// 客户档案划扣记录
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>> GetOperation(long userID, long customerID)
        {
            return await _customerProfileService.GetOperation(userID, customerID);
        }
        #endregion
    }
}
