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
    /// 分享家api
    /// </summary>
    public class ShareCategoryController : ApiController
    {
        private IShareCategoryService _shareCategoryService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="shareCategoryService"></param>
        public ShareCategoryController(IShareCategoryService shareCategoryService)
        {
            _shareCategoryService = shareCategoryService;
        }

        /// <summary>
        /// 添加分享家
        /// </summary>
        /// <param name="dto">添加分享家</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(ShareCategoryAdd dto)
        {
            return _shareCategoryService.Add(dto);
        }

        /// <summary>
        /// 更新分享家
        /// </summary>
        /// <param name="dto">更新分享家</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(ShareCategoryUpdate dto)
        {
            return _shareCategoryService.Update(dto);
        }

        /// <summary>
        /// 删除分享家
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(ShareCategoryDelete dto)
        {
            return _shareCategoryService.Delete(dto);
        }

        /// <summary>
        /// 查询所有分享家
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ShareCategoryInfo>> Get()
        {
            return _shareCategoryService.Get();
        }

        /// <summary>
        /// 查询分享家详细
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ShareCategoryInfo> GetByID(long id)
        {
            return _shareCategoryService.GetByID(id);
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
            return _shareCategoryService.GetSelect();
        }
    }
}
