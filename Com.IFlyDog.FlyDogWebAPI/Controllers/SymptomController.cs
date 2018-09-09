using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 症状相关API
    /// </summary>
    public class SymptomController : ApiController
    {
        private ISymptomService _symptomService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="symptomService"></param>
        public SymptomController(ISymptomService symptomService)
        {
            _symptomService = symptomService;
        }

        /// <summary>
        /// 添加症状
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SymptomAdd dto)
        {
            return _symptomService.Add(dto);
        }

        /// <summary>
        /// 症状修改
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SymptomUpdate dto)
        {
            return _symptomService.Update(dto);
        }

        /// <summary>
        /// 症状使用停用
        /// </summary>
        /// <param name="dto">症状停用使用信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]SymptomStopOrUse dto)
        {
            return _symptomService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有症状
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> Get()
        {
            return _symptomService.Get();
        }

        /// <summary>
        /// 查询所有可用的症状信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> GetStateIsOk()
        {
            return _symptomService.GetStateIsOk();
        }

        /// <summary>
        /// 检测症状用接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGetSymptomInfo>> ItemGetSymptom()
        {
            return _symptomService.ItemGetSymptom();
        }

        /// <summary>
        /// 根据ID查询症状详细信息
        /// </summary>
        /// <param name="id">症状id</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Symptom> GetByID(long id)
        {
            return _symptomService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _symptomService.GetSelect();
        }
    }
}
