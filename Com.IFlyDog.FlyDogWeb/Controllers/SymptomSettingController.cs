using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.JinYiWei.Common.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class SymptomSettingController : Controller
    {

        // GET: 症状设置
        public  ActionResult  SymptomIndex()
        {
             
            return View();
        }
        
        // GET: 症状设置
        public async Task<string> SymptomGet()
        {
            var result = await WebAPIHelper.Get<IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>>>("/api/Symptom/Get", new Dictionary<string, string>());

            return JsonHelper.ToJson(result.Data);

        }

        #region 查询所有可用的症状
        /// <summary>
        /// 查询所有可用的症状
        /// </summary>
        /// <returns></returns>
        public async Task<string> SymptomGetByOk()
        {
            var result = await WebAPIHelper.Get("/api/Symptom/GetStateIsOk", new Dictionary<string, string>());
            return result;
        }
        #endregion


        /// <summary>
        /// 修改数据查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SymptomlGetByIDEdit(SymptomStopOrUse dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", dto.SymptomID.ToString());
            var result = await WebAPIHelper.Get("/api/Symptom/GetByID", dic);
            return result;
        }

        // GET: 修改数据回填
        [HttpPost]
        public async Task<string> SymptomlEdit(SymptomUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Symptom/Update", dto);
            return result;
        }

        // GET: 症状设置停用
        [HttpPost]
        public Task<string> SymptomDisable(SymptomStopOrUse dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = WebAPIHelper.Post( "/api/Symptom/StopOrUse", dto);
            return result;
        }

        // GET: 症状设置添加页面
        public ActionResult SymptomAdd()
        {
            return View();
        }

        // GET: 症状设置添加接口
        [HttpPost]
        public async Task<string> Add(SymptomAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Symptom/Add", dto);
            return result;
        }
    }
}