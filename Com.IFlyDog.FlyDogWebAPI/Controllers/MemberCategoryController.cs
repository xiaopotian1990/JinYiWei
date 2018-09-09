using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 会员类型接口
    /// </summary>
    public class MemberCategoryController : ApiController
    {
        private IMemberCategoryService _memberCategoryService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="memberCategoryService"></param>
        public MemberCategoryController(IMemberCategoryService memberCategoryService)
        {
            _memberCategoryService = memberCategoryService;
        }

        /// <summary>
        /// 添加会员卡
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(MemberCategoryAdd dto)
        {
            return _memberCategoryService.Add(dto);
        }
        /// <summary>
        /// 更新会员卡信息
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(MemberCategoryUpdate dto)
        {
            return _memberCategoryService.Update(dto);
        }
        /// <summary>
        /// 会员卡删除
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(MemberCategoryDelete dto)
        {
            return _memberCategoryService.Delete(dto);
        }
        /// <summary>
        /// 查询所有会员卡
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<MemberCategory>> Get()
        {
            return _memberCategoryService.Get();
        }
        /// <summary>
        /// 查询会员卡详细
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, MemberCategory> GetByID(long id)
        {
            return _memberCategoryService.GetByID(id);
        }
        /// <summary>
        /// 添加会员权益
        /// </summary>
        /// <param name="dto">会员权益列表</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> AddMemberCategoryEquity(MemberCategoryEquityAdd dto)
        {
            return _memberCategoryService.AddMemberCategoryEquity(dto);
        }
        /// <summary>
        /// 查询详细会员权益
        /// </summary>
        /// <param name="id">会员ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, MemberCategoryEquity> GetMemberCategoryEquitysByID(long id)
        {
            return _memberCategoryService.GetMemberCategoryEquitysByID(id);
        }

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _memberCategoryService.GetSelect();
        }
    }
}
