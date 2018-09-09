using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 投诉类型
    /// </summary>
    public class ComplainCategoryController : Controller
    {
        /// <summary>
        ///  查询全部
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {        
            return View();
        }
        /// <summary>
        ///  查询全部
        /// </summary>
        /// <returns></returns>

        public async Task<string> ComplainCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/ComplainCategory/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        ///  通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ComplainGetByID(SmartComplainCategory dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/ComplainCategory/GetByID", d);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ComplainlEdit(SmartComplainCategoryUpdate dto)
        {         
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/ComplainCategory/Update", d);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ComplainAdd(SmartComplainCategoryAdd dto)
        {         
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);  
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/ComplainCategory/Add", d);
            return result;
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
         [HttpPost]
        public Task<string> ComplainDisable(SmartComplainCategoryStopOrUse dto)
        {      
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("ComplainID", dto.ComplainID.ToString());  
            d.Add("Status", dto.Status.ToString());
            var result = WebAPIHelper.Post("/api/ComplainCategory/StopOrUse", d);
            return result;
        }

    }
}