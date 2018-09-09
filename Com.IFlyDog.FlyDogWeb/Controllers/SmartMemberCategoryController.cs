using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class SmartMemberCategoryController : Controller
    {
        
        // 会员类型 GET
        public ActionResult TextIndex()
        {
            return View();
        }


        // 会员类型 GET
        public ActionResult Index()
        {
            return View();
        }
        //查询全部
        [HttpPost]
        public async Task<string> MemberCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/MemberCategory/Get", new Dictionary<string, string>());

            return result;
        }


        //添加
        [HttpPost]
        public async Task<string> MemberCategoryAdd(MemberCategory dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/MemberCategory/Add", dto);
            return result;

        }
        //删除
        [HttpPost]
        public async Task<string> MemberCategoryDelete(MemberCategoryDelete dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Post("/api/MemberCategory/Delete", dic);
            return result;

        }
        //通过ID查询
        [HttpPost]
        public async Task<string> MemberCategoryGetByID(MemberCategory dto)
        {
            
            var dic = new Dictionary<string, string>();
            dic.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/MemberCategory/GetByID", dic);
            return result;

        }
        //更新
        [HttpPost]
        public async Task<string> MemberCategoryUpdate(MemberCategoryUpdate dto)
        {
            var result = await WebAPIHelper.Post("/api/MemberCategory/Update", dto);
            return result;

        }

        //查询通过ID权益
        [HttpPost]
        public async Task<string> GetMemberCategoryEquitysByID(MemberCategoryEquity dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CategoryID", dto.CategoryID.ToString());
            var result = await WebAPIHelper.Post("/api/MemberCategory/GetMemberCategoryEquitysByID", dic);
            return result;
        }     
    }
}