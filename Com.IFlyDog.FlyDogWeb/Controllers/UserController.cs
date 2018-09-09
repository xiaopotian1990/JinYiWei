using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: 用户管理
        public ActionResult Index()
        {
            return View();
        }

        #region 权限查询角色,医院所有数据
        [HttpPost]
        public async Task<string> UserGet(SmartUserSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartUser/Get", dto);
            return result;
        }
        #endregion

        #region 查询所有数据
        [HttpPost]
        public async Task<string> UserGetPages(SmartUserSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartUser/GetPages", dto);
            return result;
        }
        #endregion

        #region 通过医院ID查询医院
        [HttpPost]
        public async Task<string> HospitalByID(HospitalInfo dto)
        {

            var dic = new Dictionary<string, string>();
            dic.Add("id", IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/Hospital/Get", dic);
            return result;
        }
        #endregion

        #region 停用数据
        [HttpPost]
        public async Task<string> Disable(UserStopOrUse dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("UserID", dto.UserID.ToString());
            dic.Add("HospitalID", dto.HospitalID.ToString());
            dic.Add("UserHospitalID", dto.UserHospitalID.ToString());
            dic.Add("Status", dto.Status.ToString());
            var result = await WebAPIHelper.Post("/api/SmartUser/StopOrUse", dic);
            return result;
        }
        #endregion

        #region 添加数据

        //添加角色-
        [HttpPost]
        public async Task<string> UserAdd(UserAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.UserHospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartUser/Add", dto);
            return result;
        }
        #endregion

        #region 修改数据-通过当前ID查询当前一条数据  
        [HttpPost]
        public async Task<string> GetDetail(string hospitalId)
        {
            var d = new Dictionary<string, string>();
            d.Add("userHositalID", IDHelper.GetUserID().ToString());
            d.Add("id", hospitalId);
            var result = await WebAPIHelper.Get("/api/SmartUser/GetDetail", d);
            return result;
        }
        #endregion

        #region 提交修改数据  
        //添加角色-
        [HttpPost]
        public async Task<string> UserUpdate(UserAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartUser/Update", dto);
            return result;
        }
        #endregion

        #region 获取客户权限详细信息
        public async Task<string> GetCustomerPermissionDetail(string userId)
        {

            var dic = new Dictionary<string, string>();
            dic.Add("userID", userId);
            var result = await WebAPIHelper.Get("/api/SmartUser/GetCustomerPermissionDetail", dic);
            return result;
        }
        #endregion

        #region 设置客户权限详细信息
        [HttpPost]
        public async Task<string> SetCustomerPermissionDetail(UserCustomerPermission ucpdto)
        {
            ucpdto.CreateUserID = IDHelper.GetUserID();
            ucpdto.UserHospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartUser/SetCustomerPermission", ucpdto);
            return result;
        }
        #endregion

        #region 获取回访权限详细信息
        public async Task<string> GetCallBackPermissionDetail(string userId)
        {

            var dic = new Dictionary<string, string>();
            dic.Add("userID", userId);
            var result = await WebAPIHelper.Get("/api/SmartUser/GetCallBackPermissionDetail", dic);
            return result;
        }
        #endregion

        #region 设置回访权限
        [HttpPost]
        public async Task<string> SetCustomerCallBackPermission(UserCustomerPermission ucpdto)
        {
            ucpdto.CreateUserID = IDHelper.GetUserID();
            ucpdto.UserHospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartUser/SetCustomerCallBackPermission", ucpdto);
            return result;
        }
        #endregion

        #region 密码重置 
        [HttpPost]
        public async Task<string> UserPasswordReset(UserPasswordReset passRedto)
        {

            var result = await WebAPIHelper.Post("/api/SmartUser/PasswordReset", passRedto);
            return result;
        }
        #endregion

        #region  获取接诊人员列表

        public async Task<string> GetFzUsers()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/SmartUser/GetFZUsers", dic);
            return result;
        }
        #endregion

        /// <summary>
        /// 获取参与预约用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetSSYYUsers(long hospitalID,DateTime date)
        {
            //var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalID.ToString() }, { "date", date.ToShortDateString() } };
            var result = await WebAPIHelper.Get("/api/SmartUser/GetSSYYUsers", dic);
            return result;
        }
    }
}