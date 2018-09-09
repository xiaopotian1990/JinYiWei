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
    /// 顾客标签相关API
    /// </summary>
    public class TagController : ApiController
    {
        private ITagService _tagService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tagService"></param>
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// 添加顾客标签
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Add(TagAdd dto)
        {
            return _tagService.Add(dto);
        }

        /// <summary>
        /// 更新顾客标签信息
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Update(TagUpdate dto)
        {
            return _tagService.Update(dto);
        }

        /// <summary>
        /// 顾客标签停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(TagStopOrUse dto)
        {
            return _tagService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有顾客标签
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> Get()
        {
            return _tagService.Get();
        }

        /// <summary>
        /// 查询所有可用的标签
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> GetByIsOk()
        {
            return _tagService.GetByIsOk();
        }

        /// <summary>
        /// 查询顾客标签详细
        /// </summary>
        /// <param name="id">顾客标签ID</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, Tag> GetByID(long id)
        {
            return _tagService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _tagService.GetSelect();
        }
    }
}
