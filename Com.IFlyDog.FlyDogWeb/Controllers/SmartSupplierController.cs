using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class SmartSupplierController : Controller
    {

        #region 供应商管理页面
      /// <summary>
      /// 获取供应商数据
      /// </summary>
      /// <param name="PinYin"></param>
      /// <param name="Name"></param>
      /// <param name="PageNum"></param>
      /// <returns></returns>
        public ActionResult SmartSupplierInfo()
        {
            return View();
        }
        #endregion

        #region 查询数据
        public async Task<string> SmartSupplierInfoGet(SmartSupplierSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartSupplier/Get",
            dto);
            return result;
        }
        #endregion

        #region 根据id获取供应商信息
        /// <summary>
        /// 根据id获取供应商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> SmartSupplierGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/SmartSupplier/GetByID", d);
            return result;
        }
        #endregion

        #region 修改数据
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Name">名称</param>
        /// <param name="PinYin"></param>
        /// <param name="LinkMan">联系人</param>
        /// <param name="Contact"></param>
        /// <param name="Remark"></param>
        /// <returns></returns>
        public async Task<string> SmartSupplierEdit(SmartSupplierUpdate dto)
        {
            var d = new Dictionary<string, string>();
            string userId = IDHelper.GetUserID().ToString();
            d.Add("CreateUserID", userId);
            d.Add("ID", dto.ID);
            d.Add("Name", dto.Name);
            d.Add("PinYin", dto.PinYin);
            d.Add("LinkMan", dto.LinkMan);
            d.Add("Contact", dto.Contact);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/SmartSupplier/Update", d);
            return result;
        }
        #endregion


        #region 添加供应商信息
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="PinYin"></param>
        /// <param name="LinkMan">联系人</param>
        /// <param name="Contact"></param>
        /// <param name="Remark"></param>
        ///  /// <returns></returns>
        public async Task<string> SmartSupplierAdd(SmartSupplierAdd dto)
        {
            string userId = IDHelper.GetUserID().ToString();
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", userId);
            d.Add("Name", dto.Name);
            d.Add("PinYin", dto.PinYin);
            d.Add("LinkMan", dto.LinkMan);
            d.Add("Contact", dto.Contact);
            d.Add("Remark", dto.Remark);
            d.Add("HospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Post("/api/SmartSupplier/Add", d);
            return result;
        }
        #endregion

        #region 删除供应商数据
        /// <summary>
        /// 删除供应商数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartSupplierDelete(SmartSupplierDelete smartSupplierDelete)
        {
            string userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", userId);
            dic.Add("ID", smartSupplierDelete.ID.ToString());
            var result = await WebAPIHelper.Post("/api/SmartSupplier/Delete", dic);
            return result;
        }
        #endregion
    }
}