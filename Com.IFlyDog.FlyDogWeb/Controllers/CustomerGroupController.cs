using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 客户组控制器
    /// </summary>
    public class CustomerGroupController : Controller
    {
        /// <summary>
        /// 客户组列表开始页面
        /// </summary>
        /// <returns></returns>
        // GET: CustomerGroup
        public ActionResult CustomerGroupInfo()
        {
            return View();
        }

        /// <summary>
        /// 展示客户组客户信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomerGroupData(string customerGroup) {
            return View();
        }

        #region 新增客户组
        /// <summary>
        /// 添加客户组
        /// </summary>
        /// <param name="customerGroupAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupAdd(CustomerGroupAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/Add", dto);
            return result;
        }
        #endregion

        #region 按照条件筛选出客户结果后保存客户组
        /// <summary>
        /// 按照条件筛选出客户结果后保存客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerFilterFiltrateAdd(CustomerFilterFiltrateAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerFilterFiltrateAdd", dto);
            return result;
        }
        #endregion

        #region 查询用户创建的所有结果集
        /// <summary>
        /// 查询用户创建的所有结果集
        /// </summary>
        /// <returns></returns>
        //public async Task<string> GetCustomerFilterGet()
        //{
        //    var userID = new Dictionary<string, string>();
        //    userID.Add("createUserID", IDHelper.GetUserID().ToString());
        //    var result = await WebAPIHelper.Get("/api/CustomerGroup/GetCustomerFilter", userID);
        //    return result;
        //}
        #endregion

        #region 批量添加回访
        /// <summary>
        /// 批量添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupBatchCallbackAdd(CustomerGroupBatchCallbackAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerGroupBatchCallbackAdd", dto);
            return result;
        }
        #endregion

        #region 批量发送短信
        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupBatchSSMAdd(BatchSSM dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerGroupBatchSSMAdd", dto);
            return result;
        }
        #endregion

        #region 添加客户组详情客户
        /// <summary>
        /// 添加客户组详情客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupDetailAdd(CustomerGroupDetailAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerGroupDetailAdd", dto);
            return result;
        }
        #endregion

        #region 删除全部客户组客户详情
        /// <summary>
        /// 删除全部客户组客户详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupDetailDeleteAll(CustomerGroupDetailDeleteAll dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerGroupDetailDeleteAll", dto);
            return result;
        }
        #endregion

        #region 删除客户组
        /// <summary>
        /// 删除客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupDelete(CustomerGroupDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/Delete", dto);
            return result;
        }
        #endregion

        #region 列表展示客户组
        /// <summary>
        /// 列表展示客户组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupGet(CustomerGroupSelect dto)
        {
            dto.createUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/Get", dto);
            return result;
        }
        #endregion

        #region 根据客户组id查询客户组用户
        /// <summary>
        /// 根据客户组id查询客户组用户
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByCustomerGroupID(CustomerGroupDetailSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetByCustomerGroupID", dto);
            return result;
        }
        #endregion

        #region 根据id查询客户组详情
        /// <summary>
        /// 根据id查询客户组详情
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/CustomerGroup/GetByID", d);
            return result;
        }
        #endregion

        #region 更新客户组
        /// <summary>
        /// 更新客户组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerGroupUpdate(CustomerGroupUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/Update", dto);
            return result;
        }
        #endregion

        #region 合并客户组
        /// <summary>
        /// 合并客户组
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> MergeCustomerFilterAdd(MergeCustomerFilter dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/MergeCustomerFilterAdd", dto);
            return result;
        }
        #endregion

        #region 新增结果集
        /// <summary>
        /// 新增结果集
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<string> CustomerFilterAdd(CustomerFilterAdd dto)
        //{
        //    dto.CreateUserID = IDHelper.GetUserID();
        //    dto.HospitalID = IDHelper.GetHospitalID();
        //    var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerFilterAdd", dto);
        //    return result;
        //}
        #endregion

        #region 新增结果集详情信息
        /// <summary>
        /// 新增结果集详情信息
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        //[HttpPost]
        //public async Task<string> CustomerFilterDetailAdd(CustomerFilterDetailAdd dto)
        //{
        //    dto.CreateUserID = IDHelper.GetUserID();
        //    var result = await WebAPIHelper.Post("/api/CustomerGroup/CustomerFilterDetailAdd", dto);
        //    return result;
        //}
        #endregion

        #region 基本条件查询
        /// <summary>
        /// 基本条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetBasicConditionSelect(BasicConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetBasicConditionSelect", dto);
            return result;
        }
        #endregion

        #region 账户条件查询
        /// <summary>
        /// 账户条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetAccountConditionSelect(AccountConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetAccountConditionSelect", dto);
            return result;
        }
        #endregion

        #region 上门条件查询
        /// <summary>
        /// 上门条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetDoorConditionSelect(DoorConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetDoorConditionSelect", dto);
            return result;
        }
        #endregion

        #region 订单条件查询
        /// <summary>
        /// 订单条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetOrderConditionSelect(OrderConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetOrderConditionSelect", dto);
            return result;
        }
        #endregion

        #region 咨询条件查询
        /// <summary>
        /// 咨询条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetConsultConditionSelect(ConsultConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetConsultConditionSelect", dto);
            return result;
        }
        #endregion

        #region 执行条件查询
        /// <summary>
        /// 执行条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetExecuteConditionSelect(ExecuteConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetExecuteConditionSelect", dto);
            return result;
        }
        #endregion

        #region 会员条件查询
        /// <summary>
        /// 会员条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetMemberConditionSelect(MemberConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetMemberConditionSelect", dto);
            return result;
        }
        #endregion

        #region 未成交条件查询
        /// <summary>
        /// 未成交条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetFailtureConditionSelect(FailtureConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetFailtureConditionSelect", dto);
            return result;
        }
        #endregion

        #region 标签条件查询
        /// <summary>
        /// 标签条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetTagConditionSelect(TagConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetTagConditionSelect", dto);
            return result;
        }
        #endregion

        #region 回访条件查询
        /// <summary>
        /// 回访条件查询
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetCallbackConditionSelect(CallbackConditionSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CustomerGroup/GetCallbackConditionSelect", dto);
            return result;
        }
        #endregion
    }
}