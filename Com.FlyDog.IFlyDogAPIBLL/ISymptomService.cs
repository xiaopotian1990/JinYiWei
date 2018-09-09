using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ISymptomService
    {
        /// <summary>
        /// 添加症状
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SymptomAdd dto);

        /// <summary>
        /// 症状修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SymptomUpdate dto);

        /// <summary>
        /// 症状使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(SymptomStopOrUse dto);

        /// <summary>
        /// 查询所有症状
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> Get();

        /// <summary>
        /// 查询所有可用的症状信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> GetStateIsOk();

        /// <summary>
        /// 检测查询所有症状
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGetSymptomInfo>> ItemGetSymptom();

        /// <summary>
        /// 查询所有症状
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Symptom> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
