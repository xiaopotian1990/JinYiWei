using System.Collections.Generic;
using System.Web.Http;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 银行卡相关接口
    /// </summary>
    public class CardCategoryController : ApiController
    {
        private readonly ICardCategoryService _cardCategoryService;

        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="cardCategoryService"></param>
        public CardCategoryController(ICardCategoryService cardCategoryService)
        {
            _cardCategoryService = cardCategoryService;
        }

        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Add(CardCategoryAdd dto)
        {
            return _cardCategoryService.Add(dto);
        }

        /// <summary>
        /// 更新银行卡信息
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Update(CardCategoryUpdate dto)
        {
            return _cardCategoryService.Update(dto);
        }

        /// <summary>
        /// 银行卡使用停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(CardCategoryStopOrUse dto)
        {
            return _cardCategoryService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有银行卡
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CardCategory>> Get()
        {
            return _cardCategoryService.Get();
        }

        /// <summary>
        /// 查询银行卡详细
        /// </summary>
        /// <param name="id">银行卡ID</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, CardCategory> GetByID(long id)
        {
            return _cardCategoryService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _cardCategoryService.GetSelect();
        }
    }
}