using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class CustomerProfileController : Controller
    {

        #region 客户详细

        public async Task<string> GetCustomerDetail(string customerId)
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"hospitalID", hospitalId},
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetCustomerDetail", dic);
            return result;

        }

        #endregion

        #region 客户详细档案里面的预约情况

        public async Task<string> GetAppointment(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();

            var dic = new Dictionary<string, string> {{"userID", userId}, {"customerID", customerId}};
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetAppointment", dic);
            return result;

        }

        #endregion

        #region 客户详细档案里面上门情况

        public async Task<string> GetVisit(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();

            var dic = new Dictionary<string, string> {{"userID", userId}, {"customerID", customerId}};
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetVisit", dic);
            return result;

        }

        #endregion

        #region 修改顾客主联系方式

        public async Task<string> UpdateMobile(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/UpdateMobile", dto);
            return result;

        }

        #endregion

        #region 修改顾客备用联系方式

        public async Task<string> UpdateMobileBackup(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/UpdateMobileBackup", dto);
            return result;

        }

        #endregion

        #region 修改顾客渠道 

        public async Task<string> UpdateChannel(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/UpdateChannel", dto);
            return result;

        }

        #endregion

        #region 更新主咨询项目  

        public async Task<string> UpdateCurrentConsultSymptom(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/UpdateCurrentConsultSymptom", dto);
            return result;

        }

        #endregion

        #region 更新推荐店家

        public async Task<string> UpdateStore(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/UpdateStore", dto);
            return result;

        }

        #endregion

        #region 清除微信绑定

        public async Task<string> ClearWechatBinding(ProfileCustomerUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();

            var result = await WebAPIHelper.Post("/api/CustomerProfile/ClearWechatBinding", dto);
            return result;

        }

        #endregion

        #region 顾客资料编辑之前获取信息-回填信息

        public async Task<string> GetCustomerInfoUpdate(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"userId", IDHelper.GetUserID().ToString()},
                {"customerID", customerId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetCustomerInfoUpdate", dic);
            return result;
        }

        #endregion

        #region 修改顾客信息提交

        public async Task<string> CustomerInfoUpdate(ProfileCustomerInfoUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/CustomerInfoUpdate", dto);
            return result;

        }

        #endregion

        #region 客户关系

        //添加关系
        public async Task<string> AddRelation(ProfileRelationAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/AddRelation", dto);
            return result;

        }

        //查询关系
        public async Task<string> GetRelation(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"userId", IDHelper.GetUserID().ToString()},
                {"customerID", customerId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetRelation", dic);
            return result;
        }

        //删除关系
        public async Task<string> DeleteRelation(CommonDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/DeleteRelation", dto);
            return result;

        }

        #endregion

        #region 店铺佣金

        //添加店铺佣金
        public async Task<string> AddStoreCommission(ProfileStoreCommissionAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/AddStoreCommission", dto);
            return result;

        }

        //删除店铺佣金
        public async Task<string> DeleteStoreCommission(ProfileStoreCommissionDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/DeleteStoreCommission", dto);
            return result;

        }

        //查询店铺佣金
        public async Task<string> GetCustomerStoreCommission(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"userId", IDHelper.GetUserID().ToString()},
                {"customerID", customerId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetCustomerStoreCommission", dic);
            return result;
        }

        #endregion

        #region 查看订单

        public async Task<string> GetOrders(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetOrders", dic);
            return result;

        }

        #endregion

        #region 负责用户

        public async Task<string> GetOwinerShip(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetOwinerShip", dic);
            return result;

        }

        #endregion

        #region 负责用户-历史信息

        public async Task<string> GetOwinerShipHistory(string customerId,string type)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId},
                {"type", type}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetOwinerShipHistory", dic);
            return result;

        }

        #endregion

        #region 标签

        //添加顾客标签的时候查询出来的详细信息
        public async Task<string> GetCustomerTageGroup(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetCustomerTageGroup", dic);
            return result;

        }

        //客户档案添加标签
        [HttpPost]
        public async Task<string> AddTags(ProfileCustomerTagAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/AddTag", dto);
            return result;
        }

        #endregion

        #region 账户查询

        public async Task<string> GetMoney(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetMoney", dic);
            return result;    
        }
        #endregion

        #region 激活券信息
        //确认激活前,查询激活码信息,是否有效!
        public async Task<string> GetActiveCoupon(string code)
        { 
            var dic = new Dictionary<string, string>{{"code", code} };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetActiveCoupon", dic);
            return result;
        }
        //确认激活券激活券
        [HttpPost]
        public async Task<string> AddActiveCoupon(ProfileActiveCouponAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerProfile/AddActiveCoupon", dto);
            return result;
        }
        #endregion

        #region 客户档案-添加回访-添加回访提醒-添加回访计划
        //添加回访
        [HttpPost]
        public async Task<string> CallbackAdd(CallbackAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID(); 
            var result = await WebAPIHelper.Post("/api/Callback/CallbackAdd", dto);
            return result;
        }

        //根据ID查询回访内容
        public async Task<string> GetCallbackDetail(string ID, string customerID)
        {
            var dic = new Dictionary<string, string>
            {
                {"ID", ID},{"customerID", customerID}, { "UserID",IDHelper.GetUserID().ToString()}
            };
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackUpdateDetail", dic);
            return result;
        }

        //回访回填
        public async Task<string> GetCallRemindDetail(string ID, string customerID)
        {
            var dic = new Dictionary<string, string>
            {
                {"ID", ID},{"customerID", customerID}, { "UserID",IDHelper.GetUserID().ToString() }
            };
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackRemindDetail", dic);
            return result;
        }

        //修改回访
        [HttpPost]
        public async Task<string> UpdateCallback(CallbackUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/UpdateCallback", dto);
            return result;
        }
        
        //个人回访情况
        public async Task<string> GetCallbackByCustomerId(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId} 
            };
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackByCustomerID", dic);
            return result;
        }

        //客户档案里面添加回访提醒
        [HttpPost]
        public async Task<string> CallbackRemindAdd(CallbackRemindAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/CallbackRemindAdd", dto);
            return result;
        }

        //修改回访提醒
        [HttpPost]
        public async Task<string> UpdateCallbackRemind(CallbackRemindUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/UpdateCallbackRemind", dto);
            return result;
        }

        //客户档案里面添加回访计划
        [HttpPost]
        public async Task<string> CallbackPlanAdd(CallbackPlanAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Callback/CallbackPlanAdd", dto);
            return result;
        }

        //获取所有可用的回访计划
        [HttpPost]
        public async Task<string> GetCallbackSet()
        {
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackSet", new Dictionary<string, string>());
            return result;
        }

        //客根据id获取可用回访计划详情
        [HttpPost]
        public async Task<string> GetCallbackSetDetail(string setID)
        {
            var dic = new Dictionary<string, string>
            {
                {"setID", setID}
            };
            var result = await WebAPIHelper.Get("/api/Callback/GetCallbackSetDetail", dic);
            return result;
        }
        #endregion

        #region 顾客档案中-添加退项目单
        //顾客档案中-添加退订单中-查询未完成的订单
        public async Task<string> GetNoDoneOrders(string customerId)
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"hospitalID", hospitalId}
          
            };
            var result = await WebAPIHelper.Get("/api/Order/GetNoDoneOrders", dic);
            return result;
        }
        //添加退项目单
        public async Task<string> AddBackOrder(BackOrderAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/BackOrder/Add", dto);
            return result; 
        }
        //删除订单
        public async Task<string> DeleteBackOrder(DepositOrderDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/BackOrder/Delete", dto);
            return result;
        }
        //查询详细订单
        public async Task<string> GetDetailOrder(string customerId,string orderId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId},
                 {"userID", orderId}
            };
            var result = await WebAPIHelper.Get("/api/BackOrder/GetDetail", dic);
            return result;
        }
        #endregion

        #region 消费项目GET

        public async Task<string> GetCharges(string customerId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetCharges", dic);
            return result;
        }
        #endregion

        #region 顾客档案中-添加退预收款
        //获取可退代金券跟预收款
        public async Task<string> GetCanRebate(string customerId)
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"hospitalID", hospitalId}

            };
            var result = await WebAPIHelper.Get("/api/DepositRebateOrder/GetCanRebate", dic);
            return result;
        }
        //添加退预收款
        public async Task<string> AddDepositRebate(DepositRebateOrderAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DepositRebateOrder/Add", dto);
            return result;
        }
        //删除退预收款
        public async Task<string> DeleteDepositRebate(DepositOrderDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/DepositRebateOrder/Delete", dto);
            return result;
        }
        //查询详细订单
        public async Task<string> GetDepositRebateDetail(string customerId, string orderId)
        {
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", userId},
                 {"userID", orderId}
            };
            var result = await WebAPIHelper.Get("/api/DepositRebateOrder/GetDetail", dic);
            return result;
        }
        #endregion

        #region 客户档案-划扣记录
        public async Task<string> GetOperation(string customerId)
        { 
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"userID", IDHelper.GetUserID().ToString()}
            };
            var result = await WebAPIHelper.Get("/api/CustomerProfile/GetOperation", dic);
            return result;
        }
        #endregion

    }
}