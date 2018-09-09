using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 病例模板接口
    /// </summary>
    public interface ICaseTemplateService
    {
        /// <summary>
        /// 添加病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CaseTemplateAdd dto);

        /// <summary>
        /// 修改病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CaseTemplateUpdate dto);

        /// <summary>
        /// 查询所有病例模板
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CaseTemplateInfo>> Get(CaseTemplateSelect dto);


        /// <summary>
        /// 根据ID获取病例模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CaseTemplateInfo> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
