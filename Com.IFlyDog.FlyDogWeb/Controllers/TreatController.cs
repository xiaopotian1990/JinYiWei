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
    public class TreatController : Controller
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Add(TreatAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Treat/Add", dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Delete(SurgeryDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Treat/Delete", dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Update(TreatUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Treat/Update", dto);
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetDetail(long ID)
        {
            var dic = new Dictionary<string, string> { { "ID", ID.ToString() } };
            var result = await WebAPIHelper.Get("/api/Treat/GetDetail", dic);
            return result;
        }
    }
}