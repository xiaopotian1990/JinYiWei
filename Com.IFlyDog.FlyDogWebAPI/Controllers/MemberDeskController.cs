using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 会员工作台相关API
    /// </summary>
    public class MemberDeskController : ApiController
    {
        private IMemberDeskService _memeberDeskService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memeberDeskService"></param>
        public MemberDeskController(IMemberDeskService memeberDeskService)
        {
            _memeberDeskService = memeberDeskService;
        }

        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskCustomer>>> Get(MemberDeskCustomerSelect dto)
        {
            return await _memeberDeskService.Get(dto);
        }

        /// <summary>
        /// 最近七日生日顾客
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskBirthdayCustomer>>> GetBirthday(long hospitalID)
        {
            return await _memeberDeskService.GetBirthday(hospitalID);
        }
    }
}
