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
    public class PhotoController : Controller
    {
        /// <summary>
        /// 添加照片
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BatchAdd(BatchPhotoAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Photo/BatchAdd", dto);
            return result;
        }

        #region 查询顾客照片

        public async Task<string> GetPhotoByCustomerId(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"userID", IDHelper.GetUserID().ToString()},
                {"customerID", customerId}
            };
            var result = await WebAPIHelper.Get("/api/Photo/GetByCustomerID", dic);
            return result;

        }

        #endregion

        #region 删除顾客照片

        [HttpPost]
        public async Task<string> DeletePoto(PhotoDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Photo/Delete", dto);
            return result;
        }
        #endregion
    }
};