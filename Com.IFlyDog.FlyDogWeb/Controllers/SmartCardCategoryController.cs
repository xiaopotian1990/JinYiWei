using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    ///     银行卡信息
    /// </summary>
    public class SmartCardCategoryController : Controller
    {
        // 银行卡信息
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     查询全部信息
        /// </summary>
        /// <returns></returns>
        //查询全部
        public async Task<string> CardCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/CardCategory/Get", new Dictionary<string, string>());

            return result;
        }

        /// <summary>
        ///     银行卡添加
        /// </summary>
        /// <param name="smartDeptAddDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CardCategoryAdd(CardCategoryAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CardCategory/Add", dto);
            return result;
        }

        //设置停用
        [HttpPost]
        public async Task<string> CardCategoryDisable(CardCategoryStopOrUse dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("ID", dto.ID.ToString());
            dic.Add("Status", dto.Status.ToString());
            var result = await WebAPIHelper.Post("/api/CardCategory/StopOrUse", dic);
            return result;
        }

        /// <summary>
        ///     修改数据根据ID查询本条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CardCategoryEditGetByID(CardCategoryUpdate dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/CardCategory/GetByID", d);
            return result;
        }

        /// <summary>
        ///     提交修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CardCategoryEditSubmit(CardCategoryUpdate dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", dto.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString()); 
            dic.Add("Name", dto.Name);
            dic.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/CardCategory/Update", dic);
            return result;
        }
    }
}